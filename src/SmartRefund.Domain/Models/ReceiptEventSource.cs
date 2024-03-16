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
        public List<IEvent> events { get; set; }

        public ReceiptEventSource() { }

        public ReceiptEventSource(InternalReceipt internalReceipt)
        {
            InternalReceipt = internalReceipt;
            events = new List<IEvent>();
        }

    }
}
