using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Infra.Repositories
{
    public class InternalReceiptRepository : Repository<InternalReceipt>, IInternalReceiptRepository
    {
        public InternalReceiptRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InternalReceipt>> GetByStatusAsync(InternalReceiptStatusEnum status)
        {
            return await _context.InternalReceipt.Where(r => r.Status == status).ToListAsync();
        }
    }
}
