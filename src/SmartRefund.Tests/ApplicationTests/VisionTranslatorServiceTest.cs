
using FluentAssertions;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.ViewModels.Requests
{
    public class VisionTranslatorServiceTest
    {
        [Fact]
        public void GetCategory_WithValidCategory_ShouldReturnCorrectEnum()
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<VisionTranslatorService>>();
            var sut = new VisionTranslatorService(repository, logger);

            // Act
            Action act = () => sut.GetCategory("SomeValidCategory");

            // Assert
            act.Should().NotThrow();
        }
    }
}

