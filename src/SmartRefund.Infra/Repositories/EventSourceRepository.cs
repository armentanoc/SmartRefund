using Microsoft.EntityFrameworkCore;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Infra.Repositories
{
    public class EventSourceRepository : IEventSourceRepository
    {
        internal readonly AppDbContext _context;
        private string _specificEntity = typeof(ReceiptEventSource).Name;

        public EventSourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReceiptEventSource> AddAsync(ReceiptEventSource entityToAdd)
        {
                _context.Set<ReceiptEventSource>().Add(entityToAdd);
                await _context.SaveChangesAsync();
                return entityToAdd;
        }

        public async Task<ReceiptEventSource> AddEvent(string hashCode, Event evnt)
        {
            var eventSource = await GetByUniqueHashAsync(hashCode);
            eventSource.AddEvent(evnt);
            eventSource.ChangeStatus(evnt.Status);
            await _context.SaveChangesAsync();
            return eventSource;
        }

        public async Task<ReceiptEventSource> GetById(uint id)
        {
            var entityToReturn = await _context.Set<ReceiptEventSource>().FirstOrDefaultAsync(entity => entity.Id == id);
            if (entityToReturn == null) { throw new EntityNotFoundException(_specificEntity, id); }
            else { return entityToReturn;  }
        }

        public async Task<ReceiptEventSource> GetByUniqueHashAsync(string hash)
        {
            var entityToReturn = await _context.ReceiptEventSource
                .Include(receipt => receipt.Events)
                .FirstOrDefaultAsync(receiptEventSource => receiptEventSource.UniqueHash.Equals(hash));

            if (entityToReturn is ReceiptEventSource)
                return entityToReturn;

            throw new EntityNotFoundException(_specificEntity, hash);
        }

    }
}
