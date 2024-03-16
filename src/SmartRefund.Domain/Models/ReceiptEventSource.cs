using SmartRefund.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Models
{
    public class ReceiptEventSource : BaseEntity
    {
        public InternalReceipt InternalReceipt { get; private set; }
        public string UniqueHash { get; private set; }
        public List<Event> Events { get; private set; }
        public EventSourceStatusEnum CurrentStatus {  get; private set; }

        public ReceiptEventSource()
        { 
            // required by EF Core
        }

            public ReceiptEventSource(InternalReceipt internalReceipt, string uniqueHash)
        {
            InternalReceipt = internalReceipt;
            UniqueHash = uniqueHash;
            Events = new List<Event>();
        }

        public void ChangeStatus(EventSourceStatusEnum status)
        {
            CurrentStatus = status;
        }

        public void AddEvent (Event evnt)
        {
            Events.Add(evnt);
        }

    }
}
