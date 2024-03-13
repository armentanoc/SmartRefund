using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;

namespace SmartRefund.Infra.Interfaces
{
    public interface ITranslatedVisionReceiptRepository : IRepository<TranslatedVisionReceipt>
    {
        Task<IEnumerable<TranslatedVisionReceipt>> GetAllByStatusAsync(TranslatedVisionReceiptStatusEnum status);
        Task<IEnumerable<TranslatedVisionReceipt>> GetAllWithRawVisionReceiptAsync();
    }

}
