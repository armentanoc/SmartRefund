
using FluentAssertions;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Enums;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.ViewModels.Requests
{
    public class VisionTranslatorServiceTest
    {
        [Fact]
        public void RemoveDiacritics_WithAccentedText_ShouldRemoveAccents()
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<VisionTranslatorService>>();
            var sut = new VisionTranslatorService(repository, logger);

            string inputText = "Olá, mundo! Isso é um teste com acentuação e ç.";
            string expectedCleanText = "Ola, mundo! Isso e um teste com acentuacao e c.";

            // Act
            string cleanText = sut.RemoveDiacritics(inputText);

            // Assert
            cleanText.Should().Be(expectedCleanText);
        }

        [Theory]
        [MemberData(nameof(UseCases_Category))]
        public void GetCategory_ShouldReturn_TranslatedVisionReceiptCategoryEnum(string phrase, TranslatedVisionReceiptCategoryEnum category)
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<VisionTranslatorService>>();
            var sut = new VisionTranslatorService(repository, logger);

            // Act
            TranslatedVisionReceiptCategoryEnum cleanText = sut.GetCategory(phrase);

            // Assert
            cleanText.Should().Be(category);
        }

        [Theory]
        [MemberData(nameof(UseCases_Total))]
        public void GetTotal_ShouldReturnDecimal(string phrase, decimal total)
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<VisionTranslatorService>>();
            var sut = new VisionTranslatorService(repository, logger);

            // Act
            decimal cleanText = sut.GetTotal(phrase);

            // Assert
            cleanText.Should().Be(total);
        }


        public static IEnumerable<object[]> UseCases_Category()
        {
            return new[] {
                new object[] { "Este recibo se refere a uma despesa de hospedagem.", TranslatedVisionReceiptCategoryEnum.HOSPEDAGEM },
                new object[] { "Este recibo se refere a uma despesa de transporte.", TranslatedVisionReceiptCategoryEnum.TRANSPORTE },
                new object[] { "Este recibo se refere a uma despesa relacionada a viagem.", TranslatedVisionReceiptCategoryEnum.VIAGEM },
                new object[] { "Este recibo se refere a uma despesa de alimentação.", TranslatedVisionReceiptCategoryEnum.ALIMENTACAO },
                new object[] { "Este recibo se refere a uma despesa de outro tipo.", TranslatedVisionReceiptCategoryEnum.OUTROS }
            };
        }

        public static IEnumerable<object[]> UseCases_Total()
        {
            return new[] {
                new object[] { "O valor total é: 123,456.66", 123456.66m },
                new object[] { "O valor total é: 123,444,456.66", 123444456.66m },
                new object[] { "O valor total é: 123.444.456,66", 123444456.66m },
                new object[] { "O valor total é: 123.456,66", 123456.66m },
                new object[] { "O valor total é: 123456,66", 123456.66m },
                new object[] { "O valor total é: 123444456,66", 123444456.66m },
                new object[] { "O valor total é: 123456", 123456m },
                new object[] { "O valor total é: 12.34", 12.34m },
            };
        }
    }
}

