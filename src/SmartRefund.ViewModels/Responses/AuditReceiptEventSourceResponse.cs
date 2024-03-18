using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.ViewModels.Responses
{
    public class AuditReceiptEventSourceResponse
    {
        public string UniqueHash { get; private set; }
        public string CurrentStatus { get; private set; }
        public List<EventResponse> Events { get; private set; }

        public AuditReceiptEventSourceResponse(ReceiptEventSource receiptEventSource)
        {
            UniqueHash = receiptEventSource.UniqueHash;
            CurrentStatus = receiptEventSource.CurrentStatus.ToString();
            Events = new List<EventResponse>();
            foreach (Event evnt in receiptEventSource.Events)
            {
                Events.Add(new EventResponse(evnt));
            }
        }

    }
}
