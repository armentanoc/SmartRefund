using Microsoft.EntityFrameworkCore;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;

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

        public async Task<ReceiptEventSource> AddEvent(ReceiptEventSource eventSource, string hashCode, Event evnt)
        {
            eventSource.SetRawVisionReceipt(eventSource.RawVisionReceipt);
            eventSource.SetTranslatedVisionReceipt(eventSource.TranslatedVisionReceipt);
            eventSource.AddEvent(evnt);
            eventSource.ChangeStatus(evnt.Status);
            await _context.SaveChangesAsync();
            return eventSource;
        }

        public async Task<IEnumerable<ReceiptEventSource>> GetAllAsync()
        {
            return await _context.ReceiptEventSource
                .Include(receipt => receipt.Events)
                .Include(receipt => receipt.InternalReceipt)
                .Include(receipt => receipt.RawVisionReceipt)
                .Include(receipt => receipt.TranslatedVisionReceipt)
                .ToListAsync();
        }

        public Task<List<ReceiptEventSource>> GetAllByHashAsync(IEnumerable<RawVisionReceipt> rawReceipts)
        {
            var hashList = rawReceipts.ToList().Select(receipt => receipt.UniqueHash);
            return _context.ReceiptEventSource.Where(r => hashList.Contains(r.UniqueHash)).ToListAsync();
        }

        public Task<List<ReceiptEventSource>> GetAllByHashAsync(IEnumerable<InternalReceipt> internalReceipts)
        {
            var hashList = internalReceipts.ToList().Select(receipt => receipt.UniqueHash);
            return _context.ReceiptEventSource.Where(r => hashList.Contains(r.UniqueHash)).ToListAsync();
        }

        public async Task<ReceiptEventSource> GetById(uint id)
        {
            var entityToReturn = await _context.Set<ReceiptEventSource>()
                .Include(eventSource => eventSource.Events)
                .FirstOrDefaultAsync(entity => entity.Id == id);
            if (entityToReturn == null) { throw new EntityNotFoundException(_specificEntity, id); }
            else { return entityToReturn;  }
        }

        public async Task<ReceiptEventSource> GetByUniqueHashAsync(string hash)
        {
            var entityToReturn = await _context.ReceiptEventSource
                .Include(receipt => receipt.Events)
                .Include(receipt => receipt.InternalReceipt)
                .Include(receipt => receipt.RawVisionReceipt)
                .Include(receipt => receipt.TranslatedVisionReceipt)
                .FirstOrDefaultAsync(receiptEventSource => receiptEventSource.UniqueHash.Equals(hash));

            if (entityToReturn is ReceiptEventSource)
                return entityToReturn;

            throw new EntityNotFoundException(_specificEntity, hash);
        }
    }
}
