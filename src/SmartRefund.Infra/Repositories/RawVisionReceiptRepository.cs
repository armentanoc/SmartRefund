using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RawVisionReceiptRepository : Repository<RawVisionReceipt>, IRawVisionReceiptRepository
    {
        public RawVisionReceiptRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RawVisionReceipt>> GetByIsTranslatedFalseAsync()
        {
            return await _context.RawVisionReceipt.Where(r => r.IsTranslated == false).ToListAsync();
        }

        public async Task<RawVisionReceipt> GetByUniqueHashAsync(string uniqueHash)
        {
            return await _context.RawVisionReceipt.FirstOrDefaultAsync(r => r.UniqueHash == uniqueHash);
        }
    }
}
