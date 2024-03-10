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


    }

}
