
using OpenAI_API;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionExecutorService
    {
        OpenAIAPI ConfigureApiKey();
        bool IsExecutableStatus(InternalReceiptStatusEnum status);
        Task<RawVisionReceipt> ExecuteRequest(InternalReceipt internalReceipt);
    }
}