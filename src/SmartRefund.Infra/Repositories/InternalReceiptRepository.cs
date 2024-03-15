using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.InternalReceipt.Where(r => r.Equals(statuses)).ToListAsync();
        }
    }
}
