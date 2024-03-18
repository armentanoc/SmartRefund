
using FluentAssertions;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.ViewModels.Requests
{
    public class VisionTranslatorServiceTests
    {
        private VisionTranslatorService MakeSut()
        {
            var rawRepository = Substitute.For<IRawVisionReceiptRepository>();
            var translatorRepository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<VisionTranslatorService>>();
            return new VisionTranslatorService(translatorRepository, rawRepository, logger);
        }

        [Fact]
        public void GetIsReceipt_ShouldThrowExceptionWhenRawSubstituteIsTranslated()
        {
            // Arrange
            var sut = MakeSut();
            var rawVisionReceipt = new RawVisionReceipt();
            rawVisionReceipt.SetIsTranslated(true);

            // Act & Assert
            Assert.ThrowsAsync<ReceiptAlreadyTranslatedException>(() => sut.GetTranslatedVisionReceipt(rawVisionReceipt));
        }

        #region IsReceipt
        [Theory]
        [MemberData(nameof(UseCases_IsReceipt))]
        public void GetIsReceipt_ShouldReturnExpectedBoolean(string input, bool expectedOutput)
        {
            // Arrange
            var sut = MakeSut();
            // Act
            bool result = sut.GetIsReceipt(input);

            // Assert
            result.Should().Be(expectedOutput);
        }
        public static IEnumerable<object[]> UseCases_IsReceipt()
        {
            return new[] {
                new object[] { "  sim  ", true },
                new object[] { "Sim  ", true },
                new object[] { "nAo  ", false },
                new object[] { " NÃO  ", false }
            };
        }

        [Fact]
        public void GetIsReceipt_ShouldThrowExceptionWhenInputIsInvalid()
        {
            // Arrange
            var sut = MakeSut();

            // Act & Assert
            Assert.Throws<UnnableToTranslateException>(() => sut.GetIsReceipt("maybe"));
        }

        [Fact]
        public void GetIsReceipt_ShouldThrowExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = MakeSut();

            // Act & Assert
            Assert.Throws<FieldIsNullOrWhitespaceException>(() => sut.GetIsReceipt(null));
        }

        #endregion

        #region Category

        [Fact]
        public void RemoveDiacritics_WithAccentedText_ShouldRemoveAccents()
        {
            // Arrange
            var sut = MakeSut();
            string inputText = "Olá, mundo! Isso é um teste com acentuação e ç.";
            string expectedCleanText = "Ola, mundo! Isso e um teste com acentuacao e c.";

            // Act
            string cleanText = sut.RemoveDiacritics(inputText);

            // Assert
            cleanText.Should().Be(expectedCleanText);
        }

        [Theory]
        [MemberData(nameof(UseCases_GetMatchingCategory))]
        public void GetMatchingCategory_ShouldReturnValidCategory_WhenInputContainsCategory(string input, string candidateCategory)
        {
            // Arrange
            var sut = MakeSut();

            // Act
            string result = sut.GetMatchingCategory(input);

            // Assert
            result.Should().Be(candidateCategory);
        }
        public static IEnumerable<object[]> UseCases_GetMatchingCategory()
        {
            return new[] {
                new object[] { " hospedagem .", "HOSPEDAGEM" },
                new object[] { "despesa de transporte.", "TRANSPORTE" },
                new object[] { "VIAGEM.  ", "VIAGEM" },
                new object[] { "alimentaCAo.  ", "ALIMENTACAO" },
                new object[] { "Este recibo se refere a uma despesa de xxxx.", "OUTROS" }
            };
        }

        [Theory]
        [MemberData(nameof(UseCases_Category))]
        public void GetCategory_ShouldReturn_TranslatedVisionReceiptCategoryEnum(string phrase, TranslatedVisionReceiptCategoryEnum category)
        {
            // Arrange
            var sut = MakeSut();

            // Act
            TranslatedVisionReceiptCategoryEnum cleanText = sut.GetCategory(phrase);

            // Assert
            cleanText.Should().Be(category);
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

        [Fact]
        public void GetCategory_ShouldThrowExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = MakeSut();

            // Act & Assert
            Assert.Throws<FieldIsNullOrWhitespaceException>(() => sut.GetCategory(null));
        }

        #endregion

        #region Total

        [Theory]
        [MemberData(nameof(UseCases_Total))]
        public void GetTotal_ShouldReturnDecimal(string phrase, decimal total)
        {
            // Arrange
            var sut = MakeSut();

            // Act
            decimal cleanText = sut.GetTotal(phrase);

            // Assert
            cleanText.Should().Be(total);
        }
        public static IEnumerable<object[]> UseCases_Total()
        {
            return new[] {
                new object[] { " 123,456.66 ", 123456.66m },
                new object[] { "O valor total é: 123,444,456.66", 123444456.66m },
                new object[] { "123.444.456,66", 123444456.66m },
                new object[] { "O valor total é: 123.456,66", 123456.66m },
                new object[] { "123456,66", 123456.66m },
                new object[] { "R$123444456,66", 123444456.66m },
                new object[] { "123456 foi o valor da compra", 123456m },
                new object[] { "O valor total é: 12.34", 12.34m },
            };
        }

        [Fact]
        public void GetTotal_ShouldThrowExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = MakeSut();

            // Act & Assert
            Assert.Throws<FieldIsNullOrWhitespaceException>(() => sut.GetTotal(null));
        }

        #endregion

        #region Description

        [Fact]
        public void GetDescription_ShouldRemoveExtraLinesAndSpaces()
        {
            // Arrange
            var sut = MakeSut();

            string inputDescription = " Essa é uma descrição com quebra\n de linha\n e espaços extras.   ";
            string expectedCleanDescription = "Essa é uma descrição com quebra de linha e espaços extras.";

            // Act
            string cleanText = sut.GetDescription(inputDescription);

            // Assert
            cleanText.Should().Be(expectedCleanDescription);
        }

        [Fact]
        public void GetDescription_ShouldThrowExceptionWhenInputIsNull()
        {
            // Arrange
            var sut = MakeSut();

            // Act & Assert
            Assert.Throws<FieldIsNullOrWhitespaceException>(() => sut.GetDescription(null));
        }

        #endregion

        [Fact]
        public void Constructor_WithValidArguments_SetsProperties()
        {
            // Arrange
            var rawVisionReceipt = new RawVisionReceipt();
            bool isReceipt = true;
            TranslatedVisionReceiptCategoryEnum category = TranslatedVisionReceiptCategoryEnum.ALIMENTACAO;
            TranslatedVisionReceiptStatusEnum status = TranslatedVisionReceiptStatusEnum.SUBMETIDO;
            decimal total = 100.50m;
            string description = "Translated receipt for groceries";
            string uniqueHash = "uniqueHash";

            // Act
            var translatedVisionReceipt = new TranslatedVisionReceipt(rawVisionReceipt, isReceipt, category, status, total, description, uniqueHash);

            // Assert
            Assert.Equal(rawVisionReceipt, translatedVisionReceipt.RawVisionReceipt);
            Assert.Equal(isReceipt, translatedVisionReceipt.IsReceipt);
            Assert.Equal(category, translatedVisionReceipt.Category);
            Assert.Equal(status, translatedVisionReceipt.Status);
            Assert.Equal(total, translatedVisionReceipt.Total);
            Assert.Equal(description, translatedVisionReceipt.Description);
            Assert.Equal(uniqueHash, translatedVisionReceipt.UniqueHash);
        }

        [Fact]
        public void SetStatus_WithValidStatus_SetsStatus()
        {
            // Arrange
            var translatedVisionReceipt = new TranslatedVisionReceipt();
            var newStatus = TranslatedVisionReceiptStatusEnum.PAGA;

            // Act
            translatedVisionReceipt.SetStatus(newStatus);

            // Assert
            Assert.Equal(newStatus, translatedVisionReceipt.Status);
        }

        [Fact]
        public async Task GetTranslatedVisionReceipt_RawReceiptAlreadyTranslated_ThrowsException()
        {
            // Arrange
            var rawReceipt = new RawVisionReceipt();
            rawReceipt.SetIsTranslated(true); 
            var translatorService = MakeSut();

            // Act and Assert
            await Assert.ThrowsAsync<ReceiptAlreadyTranslatedException>(() => translatorService.GetTranslatedVisionReceipt(rawReceipt));
        }

        [Fact]
        public void TryParseCategory_ValidEnumCandidate_ReturnsTrueAndParsedEnum()
        {
            // Arrange
            var translatorService = MakeSut();
            string enumCandidate = "ALIMENTAÇÃO"; 
            TranslatedVisionReceiptCategoryEnum expectedEnum = TranslatedVisionReceiptCategoryEnum.ALIMENTACAO; // Assuming this is the expected parsed enum

            // Act
            var result = translatorService.GetCategory(enumCandidate);

            // Assert
            Assert.Equal(result, expectedEnum);
        }

        [Fact]
        public async Task GetTranslatedVisionReceipt_RawReceiptNotTranslated_ReturnsTranslatedReceipt()
        {
            // Arrange
            var rawReceipt = new RawVisionReceipt();
            var translatorService = MakeSut();

            // Act
            await Assert.ThrowsAsync<FieldIsNullOrWhitespaceException>(() => translatorService.GetTranslatedVisionReceipt(rawReceipt));
        }
    }
}

