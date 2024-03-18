using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class InternalReceiptRepository : Repository<InternalReceipt>, IInternalReceiptRepository
    {
        public InternalReceiptRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InternalReceipt>> GetByStatusAsync(List<InternalReceiptStatusEnum> statuses)
        {
            return await _context.InternalReceipt.Where(r => statuses.Contains(r.Status)).ToListAsync();
        }
    }
}
