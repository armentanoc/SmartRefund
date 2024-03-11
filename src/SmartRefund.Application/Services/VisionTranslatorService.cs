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
        private ITranslatedVisionReceiptRepository _repository;
        private ILogger<VisionTranslatorService> _logger;
        public VisionTranslatorService(ITranslatedVisionReceiptRepository repository, ILogger<VisionTranslatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public TranslatedVisionReceipt GetTranslatedVisionReceipt(RawVisionReceipt rawVisionReceipt)
        {
            var isReceipt = GetIsReceipt(rawVisionReceipt.IsReceipt);
            var category = GetCategory(rawVisionReceipt.Category);
            var total = GetTotal(rawVisionReceipt.Total);
            var description = GetDescription(rawVisionReceipt.Description);

            TranslatedVisionReceipt translatedVisionReceipt =

                new TranslatedVisionReceipt(
                    rawVisionReceipt,
                    isReceipt,
                    category,
                    TranslatedVisionReceiptStatusEnum.SUBMETIDO,
                    total,
                    description
                    );

            var addedReceipt = _repository.AddAsync(translatedVisionReceipt);
            _logger.LogInformation($"Receipt translated and saved to database (id {addedReceipt.Id})");
            return translatedVisionReceipt;
        }

        public TranslatedVisionReceiptCategoryEnum GetCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new FieldIsNullOrWhitespaceException("Category", category);

            string[] validCategories = Enum.GetNames(typeof(TranslatedVisionReceiptCategoryEnum));
            string[] words = category.Split(new[] { ' ', '\t', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries);

            string cleanCategory = RemoveDiacritics(category);
            string enumCandidate = ""; //= word to be tested if it's a valid category

            //_logger.LogInformation($"RawCategory: {category}");
            //_logger.LogInformation($"FirstCleanseCategory: {cleanCategory}");

            if (Enum.TryParse(enumCandidate, out TranslatedVisionReceiptCategoryEnum result))
            {
                LogInformation("Category", category, result.ToString());
                return result;
            }

            throw new UnnableToTranslateException("Category", category);
        }

        private string RemoveDiacritics(string text)
        {

            string textOutsideDatabase = "Essa despesa se enquadra na categoria de alimentação. 🍽️";

            string normalizedText = textOutsideDatabase.Normalize(NormalizationForm.FormD);
            _logger.LogInformation($"NormalizedText: {normalizedText}");
            _logger.LogInformation($"RawTextOutsideDatabase: {textOutsideDatabase} {textOutsideDatabase.GetType()}");
            _logger.LogInformation($"Tipo do texto: {text.GetType()}");

            
            StringBuilder result = new StringBuilder();

            //foreach (char c in normalizedText)
            //{
            //    _logger.LogInformation($"Char: {c} {CharUnicodeInfo.GetUnicodeCategory(c)}");
            //    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            //        result.Append(c);
            //}

            return result.ToString();
        }

        public string GetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new FieldIsNullOrWhitespaceException("Description", description);

        string cleanDescription = description.Replace("\n", "").Trim();
        LogInformation("Description", description, cleanDescription);
        return cleanDescription;
    }

    public bool GetIsReceipt(string isReceipt)
    {
        if (string.IsNullOrWhiteSpace(isReceipt))
            throw new FieldIsNullOrWhitespaceException("IsRecept", isReceipt);

        bool isYes = Regex.IsMatch(isReceipt, @"^sim$", RegexOptions.IgnoreCase);
        bool isNo = Regex.IsMatch(isReceipt, @"^n(ã|a)o$", RegexOptions.IgnoreCase);
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

    private void LogInformation(string type, string raw, string clean)
    {
        _logger.LogInformation($"Raw{type}: {raw}");
        _logger.LogInformation($"Clean{type}: {clean}");
    }

    public decimal GetTotal(string total)
    {
        if (string.IsNullOrWhiteSpace(total))
            throw new FieldIsNullOrWhitespaceException("Total", total);

        string numberPattern = @"[\d]+([.,][\d]+)?";
        Match match = Regex.Match(total, numberPattern);

        _logger.LogInformation($"RawTotal: {total}");

        if (match.Success)
        {
            string decimalString = match.Value;

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
}
}
