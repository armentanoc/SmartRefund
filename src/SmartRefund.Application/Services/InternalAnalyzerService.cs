using SmartRefund.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Services
{
    public class InternalAnalyzerService
    {
        private readonly ITranslatedVisionReceiptRepository _receiptRepository;

        public InternalAnalyzerService(ITranslatedVisionReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

    }
}
