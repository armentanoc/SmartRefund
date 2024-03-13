using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<TranslatedVisionReceipt>> GetAllWithRawVisionReceiptAsync()
        {
            return await _context.Set<TranslatedVisionReceipt>()
                 .Include(receipt => receipt.RawVisionReceipt)
                 .Include(receipt => receipt.RawVisionReceipt.InternalReceipt)
                 .ToListAsync();
        }
    }
}
