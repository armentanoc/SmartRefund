using SmartRefund.Infra.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.ViewModels.Responses;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.ViewModels.Requests;
using SmartRefund.Domain.Enums;

namespace SmartRefund.Application.Services
{
    public class EventSourceService : IEventSourceService
    {
        private IEventSourceRepository _repository;

        public EventSourceService(IEventSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ReceiptEventSourceResponse>> GetAllEventSourceResponseAsync(bool isFrontEndpoint, uint userId, string userType, FrontFilter frontFilter)
        {
            IEnumerable<ReceiptEventSource> eventSources = await _repository.GetAllWithFrontFilterAsync(frontFilter);
            eventSources = ApplyAdditionalFilters(eventSources, frontFilter, userType, userId).Reverse();
            var eventSourceResponses = new List<ReceiptEventSourceResponse>();

            foreach (var eventSource in eventSources)
                if(eventSource is ReceiptEventSource)
                    eventSourceResponses.Add(new ReceiptEventSourceResponse(eventSource, isFrontEndpoint));

            return eventSourceResponses;
        }

        private IEnumerable<ReceiptEventSource> ApplyAdditionalFilters(IEnumerable<ReceiptEventSource> eventSources, FrontFilter frontFilter, string userType, uint userId)
        {
            if (userType.Equals("employee"))
                eventSources = eventSources.Where(receipt => receipt.InternalReceipt.EmployeeId == userId);
            if (frontFilter.OptionsStatusTranslate.Length != 2) //[0,1]
                eventSources = eventSources.Where(receipt => receipt.RawVisionReceipt != null && frontFilter.OptionsStatusTranslate.ToList().Select(value => value == 1).Contains(receipt.RawVisionReceipt.IsTranslated));
            if (frontFilter.OptionsStatusRefund.Length != Enum.GetValues(typeof(TranslatedVisionReceiptStatusEnum)).Length) //[0,1,2,3]
            {
                var refundStatusSet = new HashSet<int>(frontFilter.OptionsStatusRefund);
                eventSources = eventSources.Where(receipt => receipt.TranslatedVisionReceipt != null && refundStatusSet.Contains((int)receipt.TranslatedVisionReceipt.Status));
            }
            return eventSources;
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
