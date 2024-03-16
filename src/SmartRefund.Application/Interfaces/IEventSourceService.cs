using SmartRefund.Infra.Interfaces;
using SmartRefund.Domain.Models;

using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IEventSourceService
    {
        public Task<ReceiptEventSourceResponse> GetReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint);
    }
}
