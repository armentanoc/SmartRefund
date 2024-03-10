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
        private readonly ITranslatedVisionReceiptRepository _translatedVisionReceiptRepository;

        public InternalAnalyzerService(ITranslatedVisionReceiptRepository translatedVisionReceiptRepository)
        {
            _translatedVisionReceiptRepository = translatedVisionReceiptRepository;
        }

        public async Task<TranslatedVisionReceipt> UpdateStatus(uint id, string newStatus)
        {
            var translatedVisionReceipt = await GetById(id);

            if (Enum.TryParse<TranslatedVisionReceiptStatusEnum>(newStatus, true, out var result))
            {
                translatedVisionReceipt.SetStatus(result);
                return await _translatedVisionReceiptRepository.UpdateAsync(translatedVisionReceipt);
            }

            throw new InvalidOperationException();

        }

        private async Task<TranslatedVisionReceipt> GetById(uint id)
        {
            var translatedVisionReceipt = await _translatedVisionReceiptRepository.GetAsync(id);

            return translatedVisionReceipt;
        }

    }
}
