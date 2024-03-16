using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Models
{
    public class ReceiptEventSource : BaseEntity
    {
        public InternalReceipt InternalReceipt { get; set; }
        public string UniqueHash { get; private set; }
        public List<Event> Events { get; set; }

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

    }
}
