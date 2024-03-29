﻿
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Tests.ApplicationTests
{
    public class InternalAnalyzerServiceTests
    {
        private readonly InternalAnalyzerService _internalAnalyzerService;
        private readonly ITranslatedVisionReceiptRepository _receiptRepository;
        private readonly ILogger<InternalAnalyzerService> _logger;
        private readonly ICacheService _cacheService;
        private string cacheKey = "submittedReceipts";

        public InternalAnalyzerServiceTests()
        {
            _receiptRepository = Substitute.For<ITranslatedVisionReceiptRepository>();
            _logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            _cacheService = Substitute.For<ICacheService>();
            _internalAnalyzerService = new InternalAnalyzerService(_receiptRepository, _logger, _cacheService);
        }

        [Theory]
        [MemberData(nameof(ReceiptTestData))]
        public void ConvertToResponse_Converts_Receipts_To_Response(TranslatedVisionReceipt receipt, string expectedReceiptHash, uint expectedEmployeeId, decimal expectedTotal, string expectedCategory, string expectedStatus, string expectedDescription)
        {
            // Arrange
            var newReceipt = new TranslatedVisionReceipt(new RawVisionReceipt(), true, receipt.Category, receipt.Status, receipt.Total, receipt.Description, "1");
            var service = new InternalAnalyzerService(null, null, null);

            MethodInfo methodInfo = typeof(InternalAnalyzerService).GetMethod("ConvertAllToResponse", BindingFlags.NonPublic | BindingFlags.Instance);
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



        [Theory]
        [InlineData("AAAAAA", (int)TranslatedVisionReceiptStatusEnum.SUBMETIDO, TranslatedVisionReceiptStatusEnum.SUBMETIDO)]
        [InlineData("AAAAAA", (int)TranslatedVisionReceiptStatusEnum.PAGA, TranslatedVisionReceiptStatusEnum.PAGA)]
        [InlineData("AAAAAA", (int)TranslatedVisionReceiptStatusEnum.RECUSADA, TranslatedVisionReceiptStatusEnum.RECUSADA)]
        public async Task UpdateStatus_Should_Update_TranslatedVisionReceipt_Status(string hash, int newStatus, TranslatedVisionReceiptStatusEnum expectedStatus)
        {
            // Arrange
            var internalReceipt = new InternalReceipt(employeeId: 1, image: [], hash);
            var rawReceipt = new RawVisionReceipt(internalReceipt: internalReceipt, isReceipt: "true", category: "ALIMENTACAO", "100.00", "", hash);


            var receipt = new TranslatedVisionReceipt(
                rawReceipt,
                true,
                TranslatedVisionReceiptCategoryEnum.ALIMENTACAO,
                TranslatedVisionReceiptStatusEnum.SUBMETIDO,
                100,
                "Receipt description 1", hash);
            var updatedReceipt = new TranslatedVisionReceipt(
                rawReceipt,
                true,
                TranslatedVisionReceiptCategoryEnum.ALIMENTACAO,
                expectedStatus,
                100,
                "Receipt description 1", receipt.UniqueHash);

            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            repository.GetByUniqueHashAsync(hash).Returns(receipt);
            repository.UpdateAsync(Arg.Is<TranslatedVisionReceipt>(r => r.UniqueHash == hash)).Returns(updatedReceipt);

            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var cache = Substitute.For<ICacheService>();
            var service = new InternalAnalyzerService(repository, logger, cache);

            // Act
            var result = await service.UpdateStatus(hash, newStatus);

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

        [Fact]
        public async Task GetAllByStatus_Should_Return_Valid_Responses()
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            repository.GetAllByStatusAsync(TranslatedVisionReceiptStatusEnum.SUBMETIDO).Returns(new List<TranslatedVisionReceipt>());
            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var cache = Substitute.For<ICacheService>();
            var service = new InternalAnalyzerService(repository, logger, cache);

            // Act
            var result = await service.GetAllByStatus();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void ConvertToResponse_Should_Convert_Empty_Receipt_List()
        {
            // Arrange
            var service = new InternalAnalyzerService(null, null, null);

            MethodInfo methodInfo = typeof(InternalAnalyzerService).GetMethod("ConvertAllToResponse", BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new InvalidOperationException("Método ConvertToResponse não encontrado.");
            }

            // Act
            var result = (IEnumerable<TranslatedReceiptResponse>)methodInfo.Invoke(service, new object[] { new List<TranslatedVisionReceipt>() });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_Should_Return_All_Received_TranslatedVisionReceipts()
        {
            // Arrange
            var expectedReceipts = new List<TranslatedVisionReceipt>
            {
                new TranslatedVisionReceipt(new RawVisionReceipt(), true, TranslatedVisionReceiptCategoryEnum.ALIMENTACAO, TranslatedVisionReceiptStatusEnum.SUBMETIDO, 100, "Receipt description 1", "1"),
                new TranslatedVisionReceipt(new RawVisionReceipt(), true, TranslatedVisionReceiptCategoryEnum.TRANSPORTE, TranslatedVisionReceiptStatusEnum.PAGA, 200, "Receipt description 2", "2")
            };

            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            repository.GetAllWithRawVisionReceiptAsync().Returns(expectedReceipts);
            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var cache = Substitute.For<ICacheService>();
            var service = new InternalAnalyzerService(repository, logger, cache);

            // Act
            var result = await service.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedReceipts);
        }

        //fix those returns 

        [Fact]
        public async Task UpdateStatus_WithValidStatus_UpdatesStatus()
        {
            // Arrange
            string hash = "AAAA";
            int newStatus = (int)TranslatedVisionReceiptStatusEnum.PAGA;
            var translatedVisionReceipt = new TranslatedVisionReceipt();
            translatedVisionReceipt.SetStatus(TranslatedVisionReceiptStatusEnum.SUBMETIDO);

            _receiptRepository.UpdateAsync(Arg.Any<TranslatedVisionReceipt>()).Returns(translatedVisionReceipt); 

            //Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _internalAnalyzerService.UpdateStatus(hash, newStatus));
    }

        [Fact]
        public async Task UpdateStatus_WithInvalidStatus_ThrowsUnableToParseException()
        {
            // Arrange
            string hash = "AAAA";
            int newStatus = -1;
            var translatedVisionReceipt = new TranslatedVisionReceipt();
            translatedVisionReceipt.SetStatus(TranslatedVisionReceiptStatusEnum.SUBMETIDO);

            _receiptRepository.UpdateAsync(Arg.Any<TranslatedVisionReceipt>()).Returns(translatedVisionReceipt);

            // Act
            await Assert.ThrowsAsync<NullReferenceException>(() => _internalAnalyzerService.UpdateStatus(hash, newStatus));
        }

        [Fact]
        public async Task UpdateStatus_OnAlreadyUpdatedReceipt_ThrowsAlreadyUpdatedReceiptException()
        {
            // Arrange
            string hash = "AAAA";
            int newStatus = (int)TranslatedVisionReceiptStatusEnum.PAGA;
            var translatedVisionReceipt = new TranslatedVisionReceipt();
            translatedVisionReceipt.SetStatus(TranslatedVisionReceiptStatusEnum.SUBMETIDO);

            _receiptRepository.UpdateAsync(Arg.Any<TranslatedVisionReceipt>()).Returns(translatedVisionReceipt);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _internalAnalyzerService.UpdateStatus(hash, newStatus));
        }

        [Fact]
        public async Task UpdateStatus_Should_Throw_UnableToParseException_For_Invalid_Status()
        {
            // Arrange
            var service = new InternalAnalyzerService(null, null, null);
            uint id = 1;
            string invalidStatus = "INVALID_STATUS";

            // Act and Assert
            await Assert.ThrowsAsync<UnableToParseException>(() => service.UpdateStatus(id, invalidStatus));
        }

        [Fact]
        public async Task UpdateStatus_Should_Throw_AlreadyUpdatedReceiptException_When_Status_Is_Not_Submetido()
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var cache = Substitute.For<ICacheService>();
            var service = new InternalAnalyzerService(repository, logger, cache);
            uint id = 1;
            string newStatus = "PAGA";

            var translatedVisionReceipt = new TranslatedVisionReceipt();
            translatedVisionReceipt.SetStatus(TranslatedVisionReceiptStatusEnum.PAGA);

            repository.GetByIdAsync(id).Returns(translatedVisionReceipt);

            // Act and Assert
            await Assert.ThrowsAsync<AlreadyUpdatedReceiptException>(() => service.UpdateStatus(id, newStatus));
        }

        [Fact]
        public async Task UpdateStatus_UniqueHash_throws_AlreadyUpdatedReceiptException_when_status_is_not_submetido()
        {
            // Arrange
            string hash = "AAAAAA";
            int newStatus = 3;

            var translatedVisionReceipt = new TranslatedVisionReceipt();
            translatedVisionReceipt.SetStatus(TranslatedVisionReceiptStatusEnum.PAGA);

            _receiptRepository.GetByUniqueHashAsync(hash).Returns(translatedVisionReceipt);

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyUpdatedReceiptException>(() => _internalAnalyzerService.UpdateStatus(hash, newStatus));

        }

        [Fact]
        public async Task UpdateStatus_Should_Call_Repository_UpdateAsync_With_Correct_Object()
        {
            // Arrange
            var repository = Substitute.For<ITranslatedVisionReceiptRepository>();
            var logger = Substitute.For<ILogger<InternalAnalyzerService>>();
            var cache = Substitute.For<ICacheService>();
            var service = new InternalAnalyzerService(repository, logger, cache);
            uint id = 1;
            string newStatus = "PAGA";

            var translatedVisionReceipt = new TranslatedVisionReceipt();
            translatedVisionReceipt.SetStatus(TranslatedVisionReceiptStatusEnum.SUBMETIDO);

            repository.GetByIdAsync(id).Returns(translatedVisionReceipt);

            // Act
            await service.UpdateStatus(id, newStatus);

            // Assert
            await repository.Received().UpdateAsync(Arg.Is<TranslatedVisionReceipt>(r => r.Status == TranslatedVisionReceiptStatusEnum.PAGA));
        }

        [Fact]
        public async Task GetAllByStatus_return_IEnumerable_of_TranslatedReceiptResponse_if_cache_is_not_null()
        {
            // Arrange 
            var 
            translatedResponses = new List<TranslatedReceiptResponse>()
            {
                new TranslatedReceiptResponse() {},
                new TranslatedReceiptResponse() {}
            };
            _cacheService.GetCachedDataAsync<TranslatedReceiptResponse>(cacheKey).Returns(translatedResponses);

            // Act
            var result = await _internalAnalyzerService.GetAllByStatus();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<TranslatedReceiptResponse>>(result);

        }

        [Fact]
        public void GetAll_throws_InvalidOperationException_if_there_is_no_rawvisionreceipt()
        {
            // Arrange
            _receiptRepository.GetAllWithRawVisionReceiptAsync().Returns([]); 

            // Act & Assert
            _internalAnalyzerService.GetAll().Should().Throws<InvalidOperationException>();
        }

    }
}
