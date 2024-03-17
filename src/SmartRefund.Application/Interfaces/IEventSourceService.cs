using SmartRefund.Infra.Interfaces;
using SmartRefund.Domain.Models;

using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IEventSourceService
    {
        public Task<ReceiptEventSourceResponse> GetReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint);
        public Task<IEnumerable<ReceiptEventSourceResponse>> GetAllEventSourceResponseAsync(bool isFrontEndpoint, uint userId, string userType);
        public Task<AuditReceiptEventSourceResponse> GetAuditReceiptEventSourceResponseAsync(string hash);
        Task<ReceiptEventSourceResponse> GetEmployeeReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint, uint parsedUserId);
    }
}
