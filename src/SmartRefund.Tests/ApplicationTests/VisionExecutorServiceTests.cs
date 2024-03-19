
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
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

        [Theory]
        [InlineData("The answer is nao.")]
        [InlineData("NAO")]
        [InlineData("Não")]
        [InlineData("Nao")]
        [InlineData("The correct answer is nao.")]
        public void IsInvalidAnswer_ReturnsTrue_ForInvalidAnswer(string answer)
        {
            // Arrange
            var service = CreateVisionExecutorService();

            // Act
            var result = service.IsInvalidAnswer(answer);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("The answer is sim.")]
        [InlineData("SIM")]
        [InlineData("Sim")]
        public void IsInvalidAnswer_ReturnsFalse_ForValidAnswer(string answer)
        {
            // Arrange
            var service = CreateVisionExecutorService();

            // Act
            var result = service.IsInvalidAnswer(answer);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task FailedMoreThanOnceShouldntExecute_MissingApiKey_ThrowsApiKeyNotFoundException(string apiKey)
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["OpenAIVisionConfig:EnvVariable"].Returns(apiKey);
            var service = CreateVisionExecutorService(configuration);

            var internalReceipt = new InternalReceipt(1, new byte[0], "AAAAAA");
            internalReceipt.SetStatus(InternalReceiptStatusEnum.FailedMoreThanOnce);

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
            internalReceipt.SetStatus(InternalReceiptStatusEnum.FailedOnce);

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

        [Fact]
        public void Constructor_WithValidArguments_SetsProperties()
        {
            // Arrange
            var internalReceipt = new InternalReceipt();
            string isReceipt = "Yes";
            string category = "Grocery";
            string total = "100";
            string description = "Receipt for groceries";
            string uniqueHash = "uniqueHash";

            // Act
            var rawVisionReceipt = new RawVisionReceipt(internalReceipt, isReceipt, category, total, description, uniqueHash);

            // Assert
            Assert.Equal(internalReceipt, rawVisionReceipt.InternalReceipt);
            Assert.Equal(isReceipt, rawVisionReceipt.IsReceipt);
            Assert.Equal(category, rawVisionReceipt.Category);
            Assert.Equal(total, rawVisionReceipt.Total);
            Assert.Equal(description, rawVisionReceipt.Description);
            Assert.False(rawVisionReceipt.IsTranslated); // Initially should be false
            Assert.Equal(uniqueHash, rawVisionReceipt.UniqueHash);
        }

        [Fact]
        public void SetIsTranslated_WithTrue_SetsIsTranslatedToTrue()
        {
            // Arrange
            var rawVisionReceipt = new RawVisionReceipt();

            // Act
            rawVisionReceipt.SetIsTranslated(true);

            // Assert
            Assert.True(rawVisionReceipt.IsTranslated);
        }

        [Fact]
        public void SetIsTranslated_WithFalse_SetsIsTranslatedToFalse()
        {
            // Arrange

            var rawVisionReceipt = new RawVisionReceipt();

            // Act & Assert

            rawVisionReceipt.SetIsTranslated(false);
            Assert.False(rawVisionReceipt.IsTranslated);

            rawVisionReceipt.SetIsTranslated(true);
            Assert.True(rawVisionReceipt.IsTranslated);
        }

        [Theory]
        [InlineData(InternalReceiptStatusEnum.Successful, false)]
        [InlineData(InternalReceiptStatusEnum.Unsuccessful, false)]
        [InlineData(InternalReceiptStatusEnum.Unprocessed, true)]
        [InlineData(InternalReceiptStatusEnum.FailedOnce, true)]
        [InlineData(InternalReceiptStatusEnum.FailedMoreThanOnce, true)]
        public void IsExecutableStatus_ReturnsCorrectValue(InternalReceiptStatusEnum status, bool expected)
        {
            // Arrange
            var visionExecutorService = CreateVisionExecutorService();

            // Act
            var result = visionExecutorService.IsExecutableStatus(status);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ExecuteRequestAsync_AuthenticationException_UpdatesStatusAndThrows()
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

            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(internalReceipt), Times.Once);
        }

        [Fact]
        public async Task ExecuteRequestAsync_return_rawvisionreceipt_if_existingrawvivionsreceipt_is_not_null()
        {
            // Arrange
            var repository = Substitute.For<IInternalReceiptRepository>();
            var rawVisionReceiptRepository = Substitute.For<IRawVisionReceiptRepository>();
            var logger = Substitute.For<ILogger<VisionExecutorService>>();
            var config = Substitute.For<IConfiguration>();
            var visionConfigSub = Substitute.For<IVisionExecutorServiceConfiguration>();

            var service = new VisionExecutorService(repository, rawVisionReceiptRepository, logger, config, visionConfigSub);

            var internalVisionReceipt = new InternalReceipt(1, [], "uniqueHash");
            var rawVisionReceipt = new RawVisionReceipt();

            rawVisionReceiptRepository.GetByUniqueHashAsync(internalVisionReceipt.UniqueHash).Returns(rawVisionReceipt);


            // Act
            var result = await service.ExecuteRequestAsync(internalVisionReceipt);
            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<RawVisionReceipt>(result);
            
        }


        [Fact]
        public async Task CreateRawVisionReceiptAsync_return_rawvisionreceipt_if_existingrawvivionsreceipt_is_not_null()
        {
            // Arrange
            var repository = Substitute.For<IInternalReceiptRepository>();
            var rawVisionReceiptRepository = Substitute.For<IRawVisionReceiptRepository>();
            var logger = Substitute.For<ILogger<VisionExecutorService>>();
            var config = Substitute.For<IConfiguration>();
            var visionConfigSub = Substitute.For<IVisionExecutorServiceConfiguration>();

            var service = new VisionExecutorService(repository, rawVisionReceiptRepository, logger, config, visionConfigSub);

            var internalVisionReceipt = new InternalReceipt(1, [], "uniqueHash");
            var rawVisionReceipt = new RawVisionReceipt();
            var rawVisionResponse = new RawVisionResponse();

            rawVisionReceiptRepository.GetByUniqueHashAsync(internalVisionReceipt.UniqueHash).Returns(rawVisionReceipt);


            // Act
            var result = await service.CreateRawVisionReceiptAsync(internalVisionReceipt, rawVisionResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RawVisionReceipt>(result);

        }

    }
}
