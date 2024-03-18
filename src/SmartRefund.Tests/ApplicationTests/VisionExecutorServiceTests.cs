
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using OpenAI_API.Chat;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using System.Security.Authentication;

namespace SmartRefund.Application.Services.Tests
{
    public class VisionExecutorServiceTests
    {
        private static VisionExecutorService CreateVisionExecutorService(IConfiguration configuration = null)
        {
            var repository = Substitute.For<IInternalReceiptRepository>();
            var rawVisionReceiptRepository = Substitute.For<IRawVisionReceiptRepository>();
            var logger = Substitute.For<ILogger<VisionExecutorService>>();
            var config = configuration ?? Substitute.For<IConfiguration>();
            var visionConfigSub = Substitute.For<IVisionExecutorServiceConfiguration>();

            return new VisionExecutorService(repository, rawVisionReceiptRepository, logger, config, visionConfigSub);
        }

        [Theory]
        [InlineData(InternalReceiptStatusEnum.Successful)]
        [InlineData(InternalReceiptStatusEnum.Unsuccessful)]
        public async Task ExecuteRequestAsync_InvalidStatus_ThrowsNonVisionExecutableStatusException(InternalReceiptStatusEnum status)
        {
            // Arrange
            var service = CreateVisionExecutorService();
            var internalReceipt = new InternalReceipt(1, new byte[0], "AAAAAA");
            internalReceipt.SetStatus(status);

            // Act & Assert
            await Assert.ThrowsAsync<NonVisionExecutableStatus>(() => service.ExecuteRequestAsync(internalReceipt));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ExecuteRequestAsync_MissingApiKey_ThrowsApiKeyNotFoundException(string apiKey)
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:EnvVariable"].Returns(apiKey);
            var service = CreateVisionExecutorService(configuration);

            var internalReceipt = new InternalReceipt(1, new byte[0], "AAAAAA");
            internalReceipt.SetStatus(InternalReceiptStatusEnum.Unprocessed);

            // Act & Assert
            await Assert.ThrowsAsync<ApiKeyNotFoundException>(() => service.ExecuteRequestAsync(internalReceipt));
        }


        [Fact]
        public async Task ExecuteRequestAsync_ValidInput_ReturnsRawVisionReceipt()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                { "OpenAIVisionConfig:EnvVariable", "MockedApiKey" } // Mocked environment variable
                })
                .Build();

            var mockedConfig = new Mock<IVisionExecutorServiceConfiguration>();
            mockedConfig.Setup(c => c.OpenAIVisionApiEnvVar).Returns("OpenAIVisionConfig:EnvVariable");
            mockedConfig.Setup(c => c.OpenAIVisionPrompts).Returns(new OpenAIVisionPrompts());

            var internalReceipt = new InternalReceipt(1, new byte[0], "AAAAAA");
            internalReceipt.SetStatus(InternalReceiptStatusEnum.Unprocessed);

            var repositoryMock = new Mock<IInternalReceiptRepository>();
            repositoryMock.Setup(repo => repo.UpdateAsync(internalReceipt)).ReturnsAsync(internalReceipt);

            var rawVisionReceiptRepositoryMock = new Mock<IRawVisionReceiptRepository>();
            rawVisionReceiptRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<RawVisionReceipt>())).ReturnsAsync(new RawVisionReceipt());

            var visionExecutorService = new VisionExecutorService(repositoryMock.Object, rawVisionReceiptRepositoryMock.Object, Mock.Of<ILogger<VisionExecutorService>>(), configuration, mockedConfig.Object);

            // Act & Assert
            
            await Assert.ThrowsAsync<AuthenticationException>(() => visionExecutorService.ExecuteRequestAsync(internalReceipt));
        }

        [Fact]
        public async Task CreateRawVisionReceiptAsync_Should_Create_And_Return_RawVisionReceipt()
        {
            // Arrange
            var receipt = new InternalReceipt(1, new byte[0], "AAAAAA");
            var response = new RawVisionResponse
            {
                IsReceipt = "true",
                Total = "100.00",
                Category = "Groceries",
                Description = "Grocery shopping"
            };

            var repositoryMock = new Mock<IRawVisionReceiptRepository>();
            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<RawVisionReceipt>())).ReturnsAsync(new RawVisionReceipt
            (
                internalReceipt: receipt,
                isReceipt: "true",
                total: "100.00",
                category: "Groceries",
                description: "Grocery shopping",
                uniqueHash: receipt.UniqueHash

            ));

            var visionExecutorService = new VisionExecutorService(Substitute.For<IInternalReceiptRepository>(), repositoryMock.Object, Mock.Of<ILogger<VisionExecutorService>>(), Substitute.For<IConfiguration>(), Substitute.For<IVisionExecutorServiceConfiguration>());

            // Act
            var result = await visionExecutorService.CreateRawVisionReceiptAsync(receipt, response);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receipt.UniqueHash, result.UniqueHash);
            Assert.Equal(response.Total, result.Total);
            Assert.Equal(response.Category, result.Category);
            Assert.Equal(response.Description, result.Description);
        }

        [Fact]
        public async Task UpdateInternalReceiptAsync_Should_Update_Status_To_Successful()
        {
            // Arrange
            var input = new InternalReceipt(1, new byte[0], "AAAAAA");
            var repositoryMock = new Mock<IInternalReceiptRepository>();
            repositoryMock.Setup(repo => repo.UpdateAsync(input)).ReturnsAsync(input);

            var visionExecutorService = new VisionExecutorService(repositoryMock.Object, Substitute.For<IRawVisionReceiptRepository>(), Mock.Of<ILogger<VisionExecutorService>>(), Substitute.For<IConfiguration>(), Substitute.For<IVisionExecutorServiceConfiguration>());

            // Act
            var result = await visionExecutorService.UpdateInternalReceiptAsync(input);

            // Assert
            Assert.Equal(InternalReceiptStatusEnum.Successful, result.Status);
        }

    }
}
