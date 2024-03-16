using SmartRefund.Domain.Models;

namespace SmartRefund.Infra.Interfaces
{
    public interface IRawVisionReceiptRepository : IRepository<RawVisionReceipt>
    {
        Task<IEnumerable<RawVisionReceipt>> GetByIsTranslatedFalseAsync();
    }
}
