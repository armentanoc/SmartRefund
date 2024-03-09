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
    }
}
