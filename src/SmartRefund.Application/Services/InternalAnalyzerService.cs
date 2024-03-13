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

        public InternalAnalyzerService(ITranslatedVisionReceiptRepository translatedVisionReceiptRepository, ILogger<InternalAnalyzerService> logger)
        {
            _receiptRepository = translatedVisionReceiptRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<TranslatedReceiptResponse>> GetAllByStatus()
        {
            try
            {
                var receipts = await _receiptRepository.GetAllByStatusAsync(TranslatedVisionReceiptStatusEnum.SUBMETIDO);
                return this.ConvertToResponse(receipts);
            }
            catch
            {
                throw new InvalidOperationException("Ocorreu um erro ao buscar as notas fiscais!");
            }
        }

        private IEnumerable<TranslatedReceiptResponse> ConvertToResponse(IEnumerable<TranslatedVisionReceipt> receipts)
        {
            return receipts.Select(receipt =>
                new TranslatedReceiptResponse(
                    total: receipt.Total,
                    category: receipt.Category.ToString(),
                    status: receipt.Status.ToString(),
                    description: receipt.Description
                )
            );
        }



        public async Task<TranslatedVisionReceipt> UpdateStatus(uint id, string newStatus)
        {
            var translatedVisionReceipt = await GetById(id);

            if (TryParseStatus(newStatus, out var result))
            {
                translatedVisionReceipt.SetStatus(result);
                var updatedObject = await _receiptRepository.UpdateAsync(translatedVisionReceipt);

                return updatedObject;
            }

            throw new InvalidOperationException("Status enviado inválido!");
        }

        public bool TryParseStatus(string newStatus, out TranslatedVisionReceiptStatusEnum result)
        {
            return Enum.TryParse<TranslatedVisionReceiptStatusEnum>(newStatus, true, out result);
        }



        private async Task<TranslatedVisionReceipt> GetById(uint id)
        {
            var translatedVisionReceipt = await _receiptRepository.GetAsync(id);

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


