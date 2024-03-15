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
        [MemberData(nameof(ReceiptTestData))]
        public void ConvertToResponse_Converts_Receipts_To_Response(TranslatedVisionReceipt receipt, string expectedReceiptHash, uint expectedEmployeeId, decimal expectedTotal, string expectedCategory, string expectedStatus, string expectedDescription)
        {
            // Arrange
            var service = new InternalAnalyzerService(null, null, null);

            MethodInfo methodInfo = typeof(InternalAnalyzerService).GetMethod("ConvertToResponse", BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new InvalidOperationException("Método ConvertToResponse não encontrado.");
            }

            var receiptsList = new List<TranslatedVisionReceipt> { receipt };

            // Act
            var result = (IEnumerable<TranslatedReceiptResponse>)methodInfo.Invoke(service, new object[] { receiptsList });

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().UniqueHash.Should().Be(expectedReceiptHash);
            result.First().EmployeeId.Should().Be(expectedEmployeeId);
            result.First().Total.Should().Be(expectedTotal);
            result.First().Category.Should().Be(expectedCategory);
            result.First().Status.Should().Be(expectedStatus);
            result.First().Description.Should().Be(expectedDescription);
        }

    
            public static TheoryData<TranslatedVisionReceipt, uint, uint, decimal, string, string, string> ReceiptTestData()
            {
                var testData = new TheoryData<TranslatedVisionReceipt, uint, uint, decimal, string, string, string>();

                // Data set 1
                var internalReceipt1 = new InternalReceipt(123456, new byte[] { 0x00, 0x01, 0x02, 0x03 }, "1");
                internalReceipt1.SetId(1);
                var rawVisionReceipt1 = new RawVisionReceipt(internalReceipt1, "true", "Alimentacao" , "100.00", "Description1", internalReceipt1.UniqueHash);
                rawVisionReceipt1.SetId(1);
                var translatedVisionReceipt1 = new TranslatedVisionReceipt(rawVisionReceipt1, true, TranslatedVisionReceiptCategoryEnum.ALIMENTACAO, TranslatedVisionReceiptStatusEnum.SUBMETIDO, 100.00m, "Description1", rawVisionReceipt1.UniqueHash);
                translatedVisionReceipt1.SetId(1);
                testData.Add(translatedVisionReceipt1, 1, 123456, 100.00m, "ALIMENTACAO", "SUBMETIDO", "Description1");

                // Data set 2
                var internalReceipt2 = new InternalReceipt(789012, new byte[] { 0x04, 0x05, 0x06, 0x07 }, "2");
                internalReceipt2.SetId(2);
                var rawVisionReceipt2 = new RawVisionReceipt(internalReceipt2, "true", "Transporte", "200.00", "Description2", internalReceipt2.UniqueHash);
                rawVisionReceipt2.SetId(2);
                var translatedVisionReceipt2 = new TranslatedVisionReceipt(rawVisionReceipt2, true, TranslatedVisionReceiptCategoryEnum.TRANSPORTE, TranslatedVisionReceiptStatusEnum.PAGA, 200.00m, "Description2", rawVisionReceipt2.UniqueHash);
                translatedVisionReceipt2.SetId(2);
                testData.Add(translatedVisionReceipt2, 2, 789012, 200.00m, "TRANSPORTE", "PAGA", "Description2");


                return testData;
            }
        


        //[Theory]
        //[InlineData(1, "SUBMETIDO", TranslatedVisionReceiptStatusEnum.SUBMETIDO)]
        //[InlineData(1, "PAGA", TranslatedVisionReceiptStatusEnum.PAGA)]
        //[InlineData(1, "RECUSADA", TranslatedVisionReceiptStatusEnum.RECUSADA)]
        //public async Task UpdateStatus_Should_Update_TranslatedVisionReceipt_Status(uint id, string newStatus, TranslatedVisionReceiptStatusEnum expectedStatus)
        //{
        //    // Arrange
        //    var receipt = new TranslatedVisionReceipt(
        //        new RawVisionReceipt(), 
        //        true, 
        //        TranslatedVisionReceiptCategoryEnum.ALIMENTACAO, 
        //        TranslatedVisionReceiptStatusEnum.SUBMETIDO, 
        //        100, 
        //        "Receipt description 1", "3");
        //    var updatedReceipt = new TranslatedVisionReceipt(
        //        new RawVisionReceipt(),
        //        true,
        //        TranslatedVisionReceiptCategoryEnum.ALIMENTACAO,
        //        expectedStatus,
        //        100,
        //        "Receipt description 1", receipt.UniqueHash);
        //    receipt.SetId(id);
        //    updatedReceipt.SetId(id);

        //    var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
        //    repository.GetAsync(1).Returns(receipt);
        //    repository.UpdateAsync(Arg.Is<TranslatedVisionReceipt>(r => r.Id == id)).Returns(updatedReceipt);

        //    var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
        //    var service = new InternalAnalyzerService(repository, logger, null);

        //    // Act
        //    var result = await service.UpdateStatus(1, newStatus);

        //    // Assert
        //    result.Status.Should().Be(expectedStatus);
        //}


        [Theory]
        [InlineData("SUBMETIDo", TranslatedVisionReceiptStatusEnum.SUBMETIDO)]
        [InlineData("PaGA", TranslatedVisionReceiptStatusEnum.PAGA)]
        [InlineData("REcUSADA", TranslatedVisionReceiptStatusEnum.RECUSADA)]
        public void TryParseStatus_Should_Parse_Valid_Status(string statusString, TranslatedVisionReceiptStatusEnum expectedStatus)
        {
            // Arrange
            var service = new InternalAnalyzerService(null, null, null);

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
            var service = new InternalAnalyzerService(null, null, null);

            // Act
            var result = service.TryParseStatus("INVALID_STATUS", out var parsedStatus);

            // Assert
            result.Should().BeFalse();
        }

    }

}
