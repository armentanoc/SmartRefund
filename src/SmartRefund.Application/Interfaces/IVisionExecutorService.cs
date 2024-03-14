using OpenAI_API;
using OpenAI_API.Chat;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionExecutorService
    {
        Task<RawVisionReceipt> ExecuteRequestAsync(InternalReceipt input);
        Task<RawVisionResponse> ProcessVisionResponseAsync(Conversation conversation, byte[] rawImage, InternalReceipt receipt);
        Task<RawVisionReceipt> CreateRawVisionReceiptAsync(InternalReceipt receipt, RawVisionResponse response);
        Task<InternalReceipt> UpdateInternalReceiptAsync(InternalReceipt input);
        Task<string> GetResponseAsync(Conversation conversation, string prompt, Exception exceptionIfInvalid);
        OpenAIAPI ConfigureApiKey();
        bool IsExecutableStatus(InternalReceiptStatusEnum status);
        bool IsInvalidAnswer(string answer);
        Task<IEnumerable<InternalReceipt>> GetInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum status);
    }
}
