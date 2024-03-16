using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OpenAI_API.Chat;
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
                    IsResolutionReadable = GetPrompt("Prompts:User:IsResolutionReadable", "IsResolutionReadable prompt"),
                    TotalPrompt = GetPrompt("Prompts:User:Total", "Total prompt"),
                    CategoryPrompt = GetPrompt("Prompts:User:Category", "Category prompt"),
                    DescriptionPrompt = GetPrompt("Prompts:User:Description", "Description prompt")
                };
            }
        }

        public ChatRequest ChatRequestConfig
        {
            get 
            {
                return new ChatRequest
                {
                    Model = GetChatRequestConfig("Model"),
                    ResponseFormat = GetChatRequestConfig("ResponseFormat"),
                    MaxTokens = TryParseToInt(GetChatRequestConfig("MaxTokens")),
                    Temperature = TryParseToDouble(GetChatRequestConfig("Temperature")),
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

        private string? GetChatRequestConfig(string configKey)
        {
            var value = _configuration[$"OpenAIVisionConfig:ChatRequestConfig:{configKey}"];
            if (string.IsNullOrWhiteSpace(configKey))
            {
                return null;
            }

            return value;
        }

        private int? TryParseToInt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Não foi passado o número máximo de tokens no appsettings");
            }

            if (int.TryParse(value, out int result))
            {
                return result;
            };

            throw new UnableToParseException(value);
        }

        private double? TryParseToDouble(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Temperatura não foi passada no appsettings");
            }

            if (double.TryParse(value, out double result))
            {
                return result;
            };

            throw new UnableToParseException(value);
        }

    }
}
