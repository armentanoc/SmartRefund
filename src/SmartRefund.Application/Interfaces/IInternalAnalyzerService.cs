using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRefund.Domain.Models;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IInternalAnalyzerService
    {
        Task<IEnumerable<TranslatedReceiptResponse>> GetAllByStatus();
        Task<TranslatedVisionReceipt> UpdateStatus(uint id, string newStatus);
    }
}
