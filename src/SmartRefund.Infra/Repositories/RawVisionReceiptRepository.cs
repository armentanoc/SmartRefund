using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Infra.Repositories
{
    public class RawVisionReceiptRepository : Repository<RawVisionReceipt>, IRawVisionReceiptRepository
    {
        public RawVisionReceiptRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RawVisionReceipt>> GetByIsTranslatedFalseAsync()
        {
            return await _context.RawVisionReceipt.Where(r => r.IsTranslated == false).ToListAsync();
        }

    }
}
