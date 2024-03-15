using OpenAI_API.Chat;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionExecutorServiceConfiguration
    {
        string OpenAIVisionApiEnvVar { get; }
        OpenAIVisionPrompts OpenAIVisionPrompts { get; }
        ChatRequest ChatRequestConfig { get; }

    }

    public class OpenAIVisionPrompts
    {
        public string? SystemPrompt { get; set; }
        public string? ImagePrompt { get; set; }
        public string? IsReceiptPrompt { get; set; }
        public string? TotalPrompt { get; set; }
        public string? CategoryPrompt { get; set; }
        public string? DescriptionPrompt { get; set; }
    }
}
