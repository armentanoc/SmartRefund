using Microsoft.Extensions.Configuration;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;

namespace SmartRefund.Application.Services
{
    public class VisionExecutorServiceConfiguration : IVisionExecutorServiceConfiguration
    {
        private readonly IConfiguration _configuration;

        public VisionExecutorServiceConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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

        public OpenAIVisionPrompts OpenAIVisionPrompts
        {
            get
            {
                return new OpenAIVisionPrompts
                {
                    SystemPrompt = GetPrompt("Prompts:System", "System prompt"),
                    ImagePrompt = GetPrompt("Prompts:User:Image", "Image prompt"),
                    IsReceiptPrompt = GetPrompt("Prompts:User:IsReceipt", "IsReceipt prompt"),
                    TotalPrompt = GetPrompt("Prompts:User:Total", "Total prompt"),
                    CategoryPrompt = GetPrompt("Prompts:User:Category", "Category prompt"),
                    DescriptionPrompt = GetPrompt("Prompts:User:Description", "Description prompt")
                };
            }
        }

        private string GetPrompt(string configKey, string propertyName)
        {
            var prompt = _configuration[$"OpenAIVisionConfig:{configKey}"];
            if (string.IsNullOrWhiteSpace(prompt))
                throw new VisionConfigurationException(propertyName);
            return prompt;
        }
    }
}
