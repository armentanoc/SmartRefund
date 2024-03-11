
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;

using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using System;
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

        public async Task<IEnumerable<TranslatedVisionReceipt>> GetAllByStatus()
        {
            try
            {
                return await _receiptRepository.GetAllByStatusAsync(TranslatedVisionReceiptStatusEnum.SUBMETIDO);
            }
            catch
            {
                throw new InvalidOperationException("Ocorreu um erro ao buscar as notas fiscais!");
            }
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

    }
}

