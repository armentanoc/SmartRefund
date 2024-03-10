using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
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
        private ILogger<InternalAnalyzerService> _logger;

        public InternalAnalyzerService(ITranslatedVisionReceiptRepository receiptRepository, ILogger<InternalAnalyzerService> logger)
        {
            _receiptRepository = receiptRepository;
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

    }
}
