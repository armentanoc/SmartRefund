using SmartRefund.Domain.Models;
using SmartRefund.ViewModels.Requests;
namespace SmartRefund.Infra.Interfaces
{
    public interface IEventSourceRepository
    {
        Task<ReceiptEventSource> AddEvent(ReceiptEventSource eventSource, string hashCode, Event evnt);
        Task<ReceiptEventSource> GetByUniqueHashAsync(string hash);
        Task<ReceiptEventSource> AddAsync(ReceiptEventSource entityToAdd);
        Task<List<ReceiptEventSource>> GetAllByHashAsync(IEnumerable<RawVisionReceipt> rawReceipts);
        Task<List<ReceiptEventSource>> GetAllByHashAsync(IEnumerable<InternalReceipt> internalReceipts);
        Task<IEnumerable<ReceiptEventSource>> GetAllAsync();
        Task<IEnumerable<ReceiptEventSource>> GetAllByEmployeeIdAsync(uint userId, FrontFilter frontFilter);
        Task<ReceiptEventSource> GetEmployeeByUniqueHashAsync(string hash, uint parsedUserId);
        Task<IEnumerable<ReceiptEventSource>> GetAllWithFrontFilterAsync(FrontFilter frontFilter);
    }
}
