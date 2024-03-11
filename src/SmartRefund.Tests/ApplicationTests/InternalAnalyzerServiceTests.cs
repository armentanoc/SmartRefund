using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Tests.ApplicationTests
{
    public class InternalAnalyzerServiceTests
    {

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
            repository.GetAsync(1).Returns(receipt);
            repository.UpdateAsync(Arg.Is<TranslatedVisionReceipt>(r => r.Id == id)).Returns(updatedReceipt);

            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var service = new InternalAnalyzerService(repository, logger);

            // Act
            var result = await service.UpdateStatus(1, newStatus);

            // Assert
            result.Status.Should().Be(expectedStatus);
        }


        [Theory]
        [InlineData("SUBMETIDO", TranslatedVisionReceiptStatusEnum.SUBMETIDO)]
        [InlineData("PAGA", TranslatedVisionReceiptStatusEnum.PAGA)]
        [InlineData("RECUSADA", TranslatedVisionReceiptStatusEnum.RECUSADA)]
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
