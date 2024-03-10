using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
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
        private ILogger<InternalAnalyzerService> _logger;

        public InternalAnalyzerService(ITranslatedVisionReceiptRepository receiptRepository, ILogger<InternalAnalyzerService> logger)
        {
            _receiptRepository = receiptRepository;
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


    }
}
