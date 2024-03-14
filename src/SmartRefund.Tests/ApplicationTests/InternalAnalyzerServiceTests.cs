using System;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Tests.ApplicationTests
{
    public class InternalAnalyzerServiceTests
    {

        [Theory]
        [MemberData(nameof(ReceiptData))]
        public void ConvertToResponse_Converts_Receipts_To_Response(decimal total, TranslatedVisionReceiptCategoryEnum category, TranslatedVisionReceiptStatusEnum status, string description, decimal expectedTotal, string expectedCategory, string expectedStatus, string expectedDescription)
        {
            // Arrange
            var receipt = new TranslatedVisionReceipt(new RawVisionReceipt(), true, category, status, total, description);
            var service = new InternalAnalyzerService(null, null);

            MethodInfo methodInfo = typeof(InternalAnalyzerService).GetMethod("ConvertToResponse", BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new InvalidOperationException("Método ConvertToResponse não encontrado.");
            }
            //Act
            var result = (IEnumerable<TranslatedReceiptResponse>)methodInfo.Invoke(service, new object[] { new List<TranslatedVisionReceipt> { receipt } });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Total.Should().Be(expectedTotal);
            result.First().Category.Should().Be(expectedCategory);
            result.First().Status.Should().Be(expectedStatus);
            result.First().Description.Should().Be(expectedDescription);
        }

        public static TheoryData<decimal, TranslatedVisionReceiptCategoryEnum, TranslatedVisionReceiptStatusEnum, string, decimal, string, string, string> ReceiptData()
        {
            return new TheoryData<decimal, TranslatedVisionReceiptCategoryEnum, TranslatedVisionReceiptStatusEnum, string, decimal, string, string, string>
        {
            { 100, TranslatedVisionReceiptCategoryEnum.ALIMENTACAO, TranslatedVisionReceiptStatusEnum.SUBMETIDO, "Receipt description 1", 100, "ALIMENTACAO", "SUBMETIDO", "Receipt description 1" },
            { 200, TranslatedVisionReceiptCategoryEnum.TRANSPORTE, TranslatedVisionReceiptStatusEnum.PAGA, "Receipt description 2", 200, "TRANSPORTE", "PAGA", "Receipt description 2" },
            { 150, TranslatedVisionReceiptCategoryEnum.HOSPEDAGEM, TranslatedVisionReceiptStatusEnum.SUBMETIDO, "Receipt description 3", 150, "HOSPEDAGEM", "SUBMETIDO", "Receipt description 3" }
        };
        }


        [Theory]
        [InlineData(1, "SUBMETIDO", TranslatedVisionReceiptStatusEnum.SUBMETIDO)]
        [InlineData(1, "PAGA", TranslatedVisionReceiptStatusEnum.PAGA)]
        [InlineData(1, "RECUSADA", TranslatedVisionReceiptStatusEnum.RECUSADA)]
        public async Task UpdateStatus_Should_Update_TranslatedVisionReceipt_Status(uint id, string newStatus, TranslatedVisionReceiptStatusEnum expectedStatus)
        {
            // Arrange
            var receipt = new TranslatedVisionReceipt(
                new RawVisionReceipt(), 
                true, 
                TranslatedVisionReceiptCategoryEnum.ALIMENTACAO, 
                TranslatedVisionReceiptStatusEnum.SUBMETIDO, 
                100, 
                "Receipt description 1");
            var updatedReceipt = new TranslatedVisionReceipt(
                new RawVisionReceipt(),
                true,
                TranslatedVisionReceiptCategoryEnum.ALIMENTACAO,
                expectedStatus,
                100,
                "Receipt description 1");
            receipt.SetId(id);
            updatedReceipt.SetId(id);

            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            repository.GetByIdAsync(1).Returns(receipt);
            repository.UpdateAsync(Arg.Is<TranslatedVisionReceipt>(r => r.Id == id)).Returns(updatedReceipt);

            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var service = new InternalAnalyzerService(repository, logger);

            // Act
            var result = await service.UpdateStatus(1, newStatus);

            // Assert
            result.Status.Should().Be(expectedStatus);
        }


        [Theory]
        [InlineData("SUBMETIDo", TranslatedVisionReceiptStatusEnum.SUBMETIDO)]
        [InlineData("PaGA", TranslatedVisionReceiptStatusEnum.PAGA)]
        [InlineData("REcUSADA", TranslatedVisionReceiptStatusEnum.RECUSADA)]
        public void TryParseStatus_Should_Parse_Valid_Status(string statusString, TranslatedVisionReceiptStatusEnum expectedStatus)
        {
            // Arrange
            var service = new InternalAnalyzerService(null, null);

            // Act
            var result = service.TryParseStatus(statusString, out var parsedStatus);

            // Assert
            result.Should().BeTrue();
            parsedStatus.Should().Be(expectedStatus);
        }

        [Fact]
        public void TryParseStatus_Should_Return_False_For_Invalid_Status()
        {
            // Arrange
            var service = new InternalAnalyzerService(null, null);

            // Act
            var result = service.TryParseStatus("INVALID_STATUS", out var parsedStatus);

            // Assert
            result.Should().BeFalse();
        }

    }

}
