
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using NSubstitute;
using SmartRefund.ViewModels.Requests;
using SmartRefund.Domain.Enums;

namespace SmartRefund.Tests.Application.Services
{
    public class EventSourceServiceTests
    {
        [Fact]
        public async Task GetAllEventSourceResponseAsync_Employee_UserType_Returns_Response()
        {
            var repository = Substitute.For<IEventSourceRepository>();
            var service = new EventSourceService(repository);
            var frontFilter = new FrontFilter();
            uint userId = 1;
            string userType = "employee";
            bool isFrontEndpoint = true;

            repository.GetAllByEmployeeIdAsync(userId, frontFilter).Returns(new List<ReceiptEventSource>());

            var result = await service.GetAllEventSourceResponseAsync(isFrontEndpoint, userId, userType, frontFilter);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllEventSourceResponseAsync_NonEmployee_UserType_Returns_Response()
        {
            var repository = Substitute.For<IEventSourceRepository>();
            var service = new EventSourceService(repository);
            var frontFilter = new FrontFilter();
            uint userId = 1; // Just any user ID, not an employee
            string userType = "non_employee"; // Assuming this is not an employee
            bool isFrontEndpoint = true;

            repository.GetAllWithFrontFilterAsync(frontFilter).Returns(new List<ReceiptEventSource>());

            var result = await service.GetAllEventSourceResponseAsync(isFrontEndpoint, userId, userType, frontFilter);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReceiptEventSourceResponseAsync_Invalid_Hash_Throws_EntityNotFoundException()
        {
            var repository = Substitute.For<IEventSourceRepository>();
            var service = new EventSourceService(repository);
            string hash = "invalid_hash";
            bool isFrontEndpoint = true;

            repository.GetByUniqueHashAsync(hash).Returns((ReceiptEventSource)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetReceiptEventSourceResponseAsync(hash, isFrontEndpoint));
        }

        [Fact]
        public async Task GetAuditReceiptEventSourceResponseAsync_Invalid_Hash_Throws_EntityNotFoundException()
        {
            var repository = Substitute.For<IEventSourceRepository>();
            var service = new EventSourceService(repository);
            string hash = "invalid_hash";

            repository.GetByUniqueHashAsync(hash).Returns((ReceiptEventSource)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetAuditReceiptEventSourceResponseAsync(hash));
        }

        [Fact]
        public async Task GetEmployeeReceiptEventSourceResponseAsync_Invalid_Hash_Throws_EntityNotFoundException()
        {
            var repository = Substitute.For<IEventSourceRepository>();
            var service = new EventSourceService(repository);
            string hash = "invalid_hash";
            uint parsedUserId = 1;
            bool isFrontEndpoint = true;

            repository.GetEmployeeByUniqueHashAsync(hash, parsedUserId).Returns((ReceiptEventSource)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetEmployeeReceiptEventSourceResponseAsync(hash, isFrontEndpoint, parsedUserId));
        }

        [Fact]
        public void Constructor_WithValidArguments_SetsProperties()
        {
            // Arrange
            string eventSourceHash = "sourceHash";
            EventSourceStatusEnum status = EventSourceStatusEnum.EventSourceInitialized;
            DateTime eventDate = DateTime.Now;
            string description = "Event description";

            // Act
            var @event = new Event(eventSourceHash, status, eventDate, description);

            // Assert
            Assert.Equal(eventSourceHash, @event.HashCode);
            Assert.Equal(status, @event.Status);
            Assert.Equal(eventDate, @event.EventDate);
            Assert.Equal(description, @event.Description);
        }

        [Fact]
        public void ToString_ReturnsExpectedString()
        {
            // Arrange
            string eventSourceHash = "sourceHash";
            EventSourceStatusEnum status = EventSourceStatusEnum.EventSourceInitialized;
            DateTime eventDate = new DateTime(2024, 3, 18);
            string description = "Event description";

            var @event = new Event(eventSourceHash, status, eventDate, description);

            // Act
            string result = @event.ToString();

            // Assert
            Assert.Equal($"Event: {status} - {eventDate} - {description}", result);
        }

        [Fact]
        public void Constructor_WithValidArguments_SetProperties()
        {
            // Arrange
            string uniqueHash = "uniqueHash";
            EventSourceStatusEnum currentStatus = EventSourceStatusEnum.EventSourceInitialized;
            var internalReceipt = new InternalReceipt();

            // Act
            var receiptEventSource = new ReceiptEventSource(uniqueHash, currentStatus, internalReceipt);

            // Assert
            Assert.Equal(uniqueHash, receiptEventSource.UniqueHash);
            Assert.Equal(currentStatus, receiptEventSource.CurrentStatus);
            Assert.Equal(internalReceipt, receiptEventSource.InternalReceipt);
            Assert.Null(receiptEventSource.RawVisionReceipt);
            Assert.Null(receiptEventSource.TranslatedVisionReceipt);
            Assert.NotNull(receiptEventSource.Events);
            Assert.Empty(receiptEventSource.Events);
        }

        [Fact]
        public void SetUniqueHash_WithValidArgument_SetsUniqueHash()
        {
            // Arrange
            string uniqueHash = "newUniqueHash";
            var receiptEventSource = new ReceiptEventSource();

            // Act
            receiptEventSource.SetUniqueHash(uniqueHash);

            // Assert
            Assert.Equal(uniqueHash, receiptEventSource.UniqueHash);
        }

        [Fact]
        public void ChangeStatus_WithValidArgument_ChangesStatus()
        {
            // Arrange
            var receiptEventSource = new ReceiptEventSource();
            EventSourceStatusEnum newStatus = EventSourceStatusEnum.InternalReceiptCreated;

            // Act
            receiptEventSource.ChangeStatus(newStatus);

            // Assert
            Assert.Equal(newStatus, receiptEventSource.CurrentStatus);
        }

        [Fact]
        public void AddEvent_WithValidEvent_AddsEventToList()
        {
            // Arrange
            var receiptEventSource = new ReceiptEventSource();
            var @event = new Event();

            // Act
            receiptEventSource.AddEvent(@event);

            // Assert
            Assert.Single(receiptEventSource.Events);
            Assert.Contains(@event, receiptEventSource.Events);
        }

        [Fact]
        public void SetTranslatedVisionReceipt_WithValidArgument_SetsTranslatedVisionReceipt()
        {
            // Arrange
            var receiptEventSource = new ReceiptEventSource();
            var translatedVisionReceipt = new TranslatedVisionReceipt();

            // Act
            bool result = receiptEventSource.SetTranslatedVisionReceipt(translatedVisionReceipt);

            // Assert
            Assert.True(result);
            Assert.Equal(translatedVisionReceipt, receiptEventSource.TranslatedVisionReceipt);
        }

        [Fact]
        public void SetRawVisionReceipt_WithValidArgument_SetsRawVisionReceipt()
        {
            // Arrange
            var receiptEventSource = new ReceiptEventSource();
            var rawVisionReceipt = new RawVisionReceipt();

            // Act
            bool result = receiptEventSource.SetRawVisionReceipt(rawVisionReceipt);

            // Assert
            Assert.True(result);
            Assert.Equal(rawVisionReceipt, receiptEventSource.RawVisionReceipt);
        }
    }
}
