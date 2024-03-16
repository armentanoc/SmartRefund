
using SmartRefund.Domain.Models;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IInternalAnalyzerService
    {
        Task<IEnumerable<TranslatedReceiptResponse>> GetAllByStatus();
        Task<TranslatedVisionReceipt> UpdateStatus(uint id, string newStatus);
        Task<TranslatedReceiptResponse> UpdateStatus(string uniqueHash, string newStatus);
        Task<IEnumerable<TranslatedVisionReceipt>> GetAll();
    }
}
