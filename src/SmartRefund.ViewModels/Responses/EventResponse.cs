
using SmartRefund.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Responses
{
    [ExcludeFromCodeCoverage]
    public class EventResponse
    {
        public String Status { get; private set; }
        public DateTime EventDate { get; private set; }
        public string? Description { get; private set; }
        public string? HashCode { get; private set; }

        public EventResponse(Event evnt)
        {
            HashCode = evnt.HashCode;
            Status = evnt.Status.ToString();
            EventDate = evnt.EventDate;
            Description = evnt.Description;
        }

    }

}
