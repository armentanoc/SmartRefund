
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using NSubstitute;
using SmartRefund.ViewModels.Requests;

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
    }
}
