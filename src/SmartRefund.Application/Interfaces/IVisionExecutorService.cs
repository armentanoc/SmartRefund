
using SmartRefund.Domain.Models;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionExecutorService
    {
        Task<RawVisionReceipt> Start(InternalReceipt internalReceipt, string apiKey);
    }
}