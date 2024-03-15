using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Models
{
    public class ReceiptEventSource
    {
        public uint Id { get; private set; }
        public List<IEvent> events = new List<IEvent>();

        public ReceiptEventSource() { }

        public ReceiptEventSource(uint id)
        {
        Id = id;
        }
    }
}
