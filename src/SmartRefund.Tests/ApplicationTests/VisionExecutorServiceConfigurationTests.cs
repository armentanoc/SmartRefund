using Microsoft.Extensions.Configuration;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using System.Configuration;

namespace SmartRefund.Tests.Application.Services
{
    public class VisionExecutorServiceConfigurationTests
    {
        [Fact]
        public void OpenAIVisionApiEnvVar_Throws_ApiKeyNotFoundException_When_Configuration_Is_Empty()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:EnvVariable"].Returns(string.Empty);
            var service = new VisionExecutorServiceConfiguration(configuration);

            // Act & Assert
            Assert.Throws<ApiKeyNotFoundException>(() => _ = service.OpenAIVisionApiEnvVar);
        }

        [Fact]
        public void OpenAIVisionApiEnvVar_Throws_ApiKeyNotFoundException_When_Configuration_Is_Null()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:EnvVariable"].Returns((string)null);
            var service = new VisionExecutorServiceConfiguration(configuration);

            // Act & Assert
            Assert.Throws<ApiKeyNotFoundException>(() => _ = service.OpenAIVisionApiEnvVar);
        }

        [Fact]
        public void OpenAIVisionApiEnvVar_Returns_Configuration_Value_When_Configuration_Is_Valid()
        {
            // Arrange
            const string apiKey = "valid-api-key";
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:EnvVariable"].Returns(apiKey);
            var service = new VisionExecutorServiceConfiguration(configuration);

            // Act
            var result = service.OpenAIVisionApiEnvVar;

            // Assert
            Assert.Equal(apiKey, result);
        }

        [Fact]
        public void OpenAIVisionPrompts_Returns_Valid_Prompts_When_Configuration_Is_Valid()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:Prompts:System"].Returns("System Prompt");
            configuration["OpenAIVisionConfig:Prompts:User:Image"].Returns("Image Prompt");
            configuration["OpenAIVisionConfig:Prompts:User:IsReceipt"].Returns("Is Receipt Prompt");
            configuration["OpenAIVisionConfig:Prompts:User:Total"].Returns("Total Prompt");
            configuration["OpenAIVisionConfig:Prompts:User:Category"].Returns("Category Prompt");
            configuration["OpenAIVisionConfig:Prompts:User:Description"].Returns("Description Prompt");
            var service = new VisionExecutorServiceConfiguration(configuration);

            // Act
            var prompts = service.OpenAIVisionPrompts;

            // Assert
            Assert.NotNull(prompts);
            Assert.Equal("System Prompt", prompts.SystemPrompt);
            Assert.Equal("Image Prompt", prompts.ImagePrompt);
            Assert.Equal("Is Receipt Prompt", prompts.IsReceiptPrompt);
            Assert.Equal("Total Prompt", prompts.TotalPrompt);
            Assert.Equal("Category Prompt", prompts.CategoryPrompt);
            Assert.Equal("Description Prompt", prompts.DescriptionPrompt);
        }

        [Fact]
        public void OpenAIVisionPrompts_Throws_Exception_When_Prompts_Are_Not_Configured()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:Prompts:System"].Returns((string)null);
            configuration["OpenAIVisionConfig:Prompts:User:Image"].Returns((string)null);
            configuration["OpenAIVisionConfig:Prompts:User:IsReceipt"].Returns((string)null);
            configuration["OpenAIVisionConfig:Prompts:User:Total"].Returns((string)null);
            configuration["OpenAIVisionConfig:Prompts:User:Category"].Returns((string)null);
            configuration["OpenAIVisionConfig:Prompts:User:Description"].Returns((string)null);
            var service = new VisionExecutorServiceConfiguration(configuration);

            // Act & Assert
            Assert.Throws<VisionConfigurationException>(() => _ = service.OpenAIVisionPrompts);
        }
    }
}
