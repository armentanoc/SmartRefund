using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartRefund.Application.Services
{
    public class VisionTranslatorService : IVisionTranslatorService
    {
        private ITranslatedVisionReceiptRepository _translatorRepository;
        private IRawVisionReceiptRepository _rawRepository;
        private ILogger<VisionTranslatorService> _logger;

        public VisionTranslatorService(ITranslatedVisionReceiptRepository translatorRepository, IRawVisionReceiptRepository rawRepository, ILogger<VisionTranslatorService> logger)
        {
            _translatorRepository = translatorRepository;
            _rawRepository = rawRepository;
            _logger = logger;
        }

        public async Task<TranslatedVisionReceipt> GetTranslatedVisionReceipt(RawVisionReceipt rawVisionReceipt)
        {
            if (rawVisionReceipt.IsTranslated)
                throw new ReceiptAlreadyTranslatedException(rawVisionReceipt.Id);

            TranslatedVisionReceipt translatedVisionReceipt =
                new TranslatedVisionReceipt(
                    rawVisionReceipt: rawVisionReceipt,
                    isReceipt: GetIsReceipt(rawVisionReceipt.IsReceipt),
                    category: GetCategory(rawVisionReceipt.Category),
                    status: TranslatedVisionReceiptStatusEnum.SUBMETIDO,
                    total: GetTotal(rawVisionReceipt.Total),
                    description: GetDescription(rawVisionReceipt.Description)
                    , "1");

            var addedReceipt = await _translatorRepository.AddAsync(translatedVisionReceipt);
            rawVisionReceipt.SetIsTranslated(true);
            var updatedRawReceipt = await _rawRepository.UpdateAsync(rawVisionReceipt);

            LogSuccess(addedReceipt, updatedRawReceipt);

            return addedReceipt;
        }

        public bool GetIsReceipt(string isReceipt)
        {
            if (string.IsNullOrWhiteSpace(isReceipt))
                throw new FieldIsNullOrWhitespaceException("IsRecept", isReceipt);

            bool isYes = Regex.IsMatch(isReceipt, @"\bsim\b", RegexOptions.IgnoreCase);
            bool isNo = Regex.IsMatch(isReceipt, @"\bn(ã|a)o\b", RegexOptions.IgnoreCase);
            bool isReceiptBool;

            if (isYes && !isNo)
                isReceiptBool = true;
            else if (isNo)
                isReceiptBool = false;
            else
                throw new UnnableToTranslateException("IsReceipt", isReceipt);

            LogInformation("IsReceipt", isReceipt, isReceiptBool.ToString());
            return isReceiptBool;
        }
        public TranslatedVisionReceiptCategoryEnum GetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new FieldIsNullOrWhitespaceException("Category", category);

            string cleanCategory = RemoveDiacritics(category)
                .ToLower().Replace("ç", "c").Replace("ã", "a");
            //por que é necessário se o RemoveDiacritics passa no teste???

            string enumCandidate = GetMatchingCategory(cleanCategory);

            if (Enum.TryParse(enumCandidate, out TranslatedVisionReceiptCategoryEnum result))
            {
                LogInformation("Category", category, result.ToString());
                return result;
            }

            throw new UnnableToTranslateException("Category", category);
        }
        public decimal GetTotal(string total)
        {
            if (string.IsNullOrWhiteSpace(total))
                throw new FieldIsNullOrWhitespaceException("Total", total);

            string numberPattern = @"[\d]+(?:[,.][\d]+)*";
            Match match = Regex.Match(total, numberPattern);

            if (match.Success)
            {
                string decimalString = match.Value;

                if (decimalString.Length >= 3 && decimalString[decimalString.Length - 3] == ',')
                    decimalString = decimalString.Remove(decimalString.Length - 3, 1).Insert(decimalString.Length - 3, ".");

                for (int i = 0; i < decimalString.Length; i++)
                {
                    if (i != decimalString.Length - 3 && decimalString[i] == '.')
                        decimalString = decimalString.Remove(i, 1);
                }

                if (decimal.TryParse(decimalString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
                {
                    LogInformation("Total", total, decimalValue.ToString());
                    return decimalValue;
                }
                else
                    throw new UnnableToTranslateException("Total", $"couldn't parse to decimal - {total}");
            }
            else
            {
                throw new UnnableToTranslateException("Total", $"no number found at string - {total}");
            }
        }
        public string GetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new FieldIsNullOrWhitespaceException("Description", description);

            string cleanDescription = description.Replace("\n", "").Trim();
            LogInformation("Description", description, cleanDescription);
            return cleanDescription;
        }

        #region Aux Category Methods
        public string GetMatchingCategory(string cleanCategory)
        {
            var validCategories = Enum.GetNames(typeof(TranslatedVisionReceiptCategoryEnum));

            return validCategories.FirstOrDefault(validCategory =>
                cleanCategory.Contains(validCategory, StringComparison.OrdinalIgnoreCase))
                ?? "OUTROS";
        }

        public string RemoveDiacritics(string text)
        {
            string normalizedText = text.Normalize(NormalizationForm.FormD);
            StringBuilder result = new StringBuilder();

            foreach (char c in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    result.Append(c);
            }

            return result.ToString();
        }

        #endregion

        #region Log Aux
        private void LogSuccess(TranslatedVisionReceipt addedReceipt, RawVisionReceipt updatedRawReceipt)
        {
            _logger.LogInformation($"Receipt translated and saved to database " +
                $"as TranslatedVisionReceipt (Id {addedReceipt.Id}) " +
                $"and RawVisionReceiptUpdated marked as translated (Id {updatedRawReceipt.Id})");
        }
        private void LogInformation(string type, string raw, string clean)
        {
            _logger.LogInformation($"Raw{type}: {raw}");
            _logger.LogInformation($"Clean{type}: {clean}");
        }
        #endregion
    }
}
