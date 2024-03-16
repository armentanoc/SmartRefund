using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Services
{
    public class InternalAnalyzerService : IInternalAnalyzerService
    {
        private readonly ITranslatedVisionReceiptRepository _receiptRepository;
        private readonly ILogger<InternalAnalyzerService> _logger;
        private readonly ICacheService _cacheService;
        private string cacheKey = "submittedReceipts";

        public InternalAnalyzerService(ITranslatedVisionReceiptRepository translatedVisionReceiptRepository, ILogger<InternalAnalyzerService> logger, ICacheService cacheService)
        {
            _receiptRepository = translatedVisionReceiptRepository;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<TranslatedReceiptResponse>> GetAllByStatus()
        {
            try
            {
                var cachedReceipts =  await _cacheService.GetCachedDataAsync<TranslatedReceiptResponse>(cacheKey);
                if (cachedReceipts != null && cachedReceipts.Any())
                {
                    return cachedReceipts;
                }
                else
                {
                    var receipts = await _receiptRepository.GetAllByStatusAsync(TranslatedVisionReceiptStatusEnum.SUBMETIDO);
                    var response = ConvertToResponse(receipts);
                    await _cacheService.SetCachedDataAsync(cacheKey, response);

                    return response;
                }
            }
            catch
            {
                throw new GetReceiptsException();
            }
        }


        private IEnumerable<TranslatedReceiptResponse> ConvertToResponse(IEnumerable<TranslatedVisionReceipt> receipts)
        {
            return receipts.Select(receipt =>
                new TranslatedReceiptResponse(
                    receiptId: receipt.Id,
                    employeeId: receipt.RawVisionReceipt.InternalReceipt.EmployeeId,
                    total: receipt.Total,
                    category: receipt.Category.ToString(),
                    status: receipt.Status.ToString(),
                    description: receipt.Description
                )
            ); 
        }



        public async Task<TranslatedVisionReceipt> UpdateStatus(uint id, string newStatus)
        {
            if (TryParseStatus(newStatus, out var result))
            {
                var translatedVisionReceipt = await GetById(id);
                    if (translatedVisionReceipt.Status == TranslatedVisionReceiptStatusEnum.SUBMETIDO)
                    {
                        translatedVisionReceipt.SetStatus(result);
                        var updatedObject = await _receiptRepository.UpdateAsync(translatedVisionReceipt);
                        return updatedObject;
                    }
                throw new AlreadyUpdatedReceiptException(id);
            }
                throw new UnableToParseException(newStatus);
            }

        public bool TryParseStatus(string newStatus, out TranslatedVisionReceiptStatusEnum result)
        {
            return Enum.TryParse<TranslatedVisionReceiptStatusEnum>(newStatus, true, out result);
        }

        private async Task<TranslatedVisionReceipt> GetById(uint id)
        {
            var translatedVisionReceipt = await _receiptRepository.GetByIdAsync(id);

            return translatedVisionReceipt;
        }

        public async Task<IEnumerable<TranslatedVisionReceipt>> GetAll()
        {
            var result = await _receiptRepository.GetAllWithRawVisionReceiptAsync();

            if (result != null && result.Count() != 0)
            {
                return result;
            }

            throw new InvalidOperationException("Nenhum objeto encontrado");
        }
    }

 }


