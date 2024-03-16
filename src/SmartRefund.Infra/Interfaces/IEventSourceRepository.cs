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
        Task<ReceiptEventSource> GetById(uint id);
        Task<ReceiptEventSource> AddEvent(string hashCode, Event evnt);
        Task<ReceiptEventSource> GetByUniqueHashAsync(string hash);
        Task<ReceiptEventSource> AddAsync(ReceiptEventSource entityToAdd);
    }
}
