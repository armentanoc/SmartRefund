using SmartRefund.Infra.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.ViewModels.Responses;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;

namespace SmartRefund.Application.Services
{
    public class EventSourceService : IEventSourceService
    {
        private IEventSourceRepository _repository;

        public EventSourceService(IEventSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReceiptEventSourceResponse>> GetAllEventSourceResponseAsync(bool isFrontEndpoint, uint userId, string userType)
        {
            IEnumerable<ReceiptEventSource> eventSources;

            if(userType.Equals("employee"))
                eventSources = await _repository.GetAllByEmployeeIdAsync(userId);
            else
                eventSources = await _repository.GetAllAsync();

            var eventSourceResponses = new List<ReceiptEventSourceResponse>();

            foreach (var eventSource in eventSources)
                if(eventSource is ReceiptEventSource)
                    eventSourceResponses.Add(new ReceiptEventSourceResponse(eventSource, isFrontEndpoint));

            return eventSourceResponses;
        }

        public async Task<ReceiptEventSourceResponse> GetReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint)
        {
            var eventSource = await _repository.GetByUniqueHashAsync(hash);
            
            if(eventSource is ReceiptEventSource)
                return new ReceiptEventSourceResponse(eventSource, isFrontEndpoint);

            throw new EntityNotFoundException(hash);
        }

        public async Task<AuditReceiptEventSourceResponse> GetAuditReceiptEventSourceResponseAsync(string hash)
        {
            var eventSource = await _repository.GetByUniqueHashAsync(hash);

            if (eventSource is ReceiptEventSource)
                return new AuditReceiptEventSourceResponse(eventSource);

            throw new EntityNotFoundException(hash);
        }

        public async Task<ReceiptEventSourceResponse> GetEmployeeReceiptEventSourceResponseAsync(string hash, bool isFrontEndpoint, uint parsedUserId)
        {
            var eventSource = await _repository.GetEmployeeByUniqueHashAsync(hash, parsedUserId);

            if (eventSource is ReceiptEventSource)
                return new ReceiptEventSourceResponse(eventSource, isFrontEndpoint);

            throw new EntityNotFoundException(hash);
        }
    }
}
