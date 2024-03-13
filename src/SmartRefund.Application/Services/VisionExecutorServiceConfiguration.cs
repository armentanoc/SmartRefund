using Microsoft.Extensions.Configuration;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;

namespace SmartRefund.Application.Services
{
    public class VisionExecutorServiceConfiguration : IVisionExecutorServiceConfiguration
    {
        public string OpenAIVisionApiEnvVar
        {
            get
            {
                var apiKey = _configuration["OpenAIVisionConfig:EnvVariable"];
                if (string.IsNullOrWhiteSpace(apiKey))
                    throw new ApiKeyNotFoundException();
                return apiKey;
            }
        }

        private readonly IConfiguration _configuration;

        public VisionExecutorServiceConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OpenAIVisionPrompts OpenAIVisionPrompts => new OpenAIVisionPrompts
        {
            SystemPrompt = _configuration["OpenAIVisionConfig:Prompts:System"],
            ImagePrompt = _configuration["OpenAIVisionConfig:Prompts:User:Image"],
            IsReceiptPrompt = _configuration["OpenAIVisionConfig:Prompts:User:IsReceipt"],
            TotalPrompt = _configuration["OpenAIVisionConfig:Prompts:User:Total"],
            CategoryPrompt = _configuration["OpenAIVisionConfig:Prompts:User:Category"],
            DescriptionPrompt = _configuration["OpenAIVisionConfig:Prompts:User:Description"]
        };
    }
}
