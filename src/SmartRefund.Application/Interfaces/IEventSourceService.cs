
using SmartRefund.ViewModels.Responses;
using SmartRefund.ViewModels.Requests;

namespace SmartRefund.Application.Interfaces
{
    public interface IEventSourceService
    {
        public Task<ReceiptEventSourceResponse> GetReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint);
        public Task<IEnumerable<ReceiptEventSourceResponse>> GetAllEventSourceResponseAsync(bool isFrontEndpoint, uint userId, string userType, FrontFilter frontFilter);
        public Task<AuditReceiptEventSourceResponse> GetAuditReceiptEventSourceResponseAsync(string hash);
        Task<ReceiptEventSourceResponse> GetEmployeeReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint, uint parsedUserId);
    }
}
