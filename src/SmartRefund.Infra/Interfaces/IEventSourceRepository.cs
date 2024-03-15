using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Infra.Interfaces
{
    public interface IEventSourceRepository
    {
        public Task<ReceiptEventSource> GetById(uint id);
        public Task<ReceiptEventSource> AddEvent(uint id, IEvent evnt);
    }
}
