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
    }
}
