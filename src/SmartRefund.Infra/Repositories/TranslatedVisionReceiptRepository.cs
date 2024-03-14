using Microsoft.EntityFrameworkCore;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Infra.Repositories
{
    public class TranslatedVisionReceiptRepository : Repository<TranslatedVisionReceipt>, ITranslatedVisionReceiptRepository
    {
        public TranslatedVisionReceiptRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<TranslatedVisionReceipt>> GetAllByStatusAsync(TranslatedVisionReceiptStatusEnum status)
        {
            return await _context.Set<TranslatedVisionReceipt>()
                .Where(receipt => receipt.Status == status)
                .ToListAsync();
        }

        public async Task<TranslatedVisionReceipt> GetByIdAsync(uint id)
        {
            var entityToReturn = await _context
                .Set<TranslatedVisionReceipt>()
                .Include(entity => entity.RawVisionReceipt)
                .Include(entity => entity.RawVisionReceipt.InternalReceipt)
                .FirstOrDefaultAsync(entity =>
                entity.Id == id
            );

            if (entityToReturn != null)
            {
                entityToReturn.SetId(id);
                await _context.SaveChangesAsync();
                return entityToReturn;
            }

            throw new EntityNotFoundException(_specificEntity, id);
        }

        // Apenas para visualização
        public async Task<IEnumerable<TranslatedVisionReceipt>> GetAllWithRawVisionReceiptAsync()
        {
            return await _context.Set<TranslatedVisionReceipt>()
                 .Include(receipt => receipt.RawVisionReceipt)
                 .Include(receipt => receipt.RawVisionReceipt.InternalReceipt)
                 .ToListAsync();
        }
    }
}
