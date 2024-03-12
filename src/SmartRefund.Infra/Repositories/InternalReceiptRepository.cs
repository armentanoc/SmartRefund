using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Infra.Repositories
{
    public class InternalReceiptRepository : Repository<InternalReceipt>, IInternalReceiptRepository
    {
        public InternalReceiptRepository(AppDbContext context) : base(context)
        {
        }

    }
}
