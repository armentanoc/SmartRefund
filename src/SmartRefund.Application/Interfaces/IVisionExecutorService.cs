
using OpenAI_API;
using SmartRefund.Domain.Models;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionExecutorService
    {
        OpenAIAPI ConfigureApiKey();
        Task<RawVisionReceipt> ExecuteRequest(InternalReceipt internalReceipt);
    }
}