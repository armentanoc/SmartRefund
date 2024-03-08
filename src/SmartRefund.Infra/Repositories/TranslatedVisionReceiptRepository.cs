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
    }
}
