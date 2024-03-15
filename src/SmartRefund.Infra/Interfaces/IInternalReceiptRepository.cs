using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;

namespace SmartRefund.Infra.Interfaces
{
    public interface IInternalReceiptRepository : IRepository<InternalReceipt>
    {
        Task<IEnumerable<InternalReceipt>> GetByStatusAsync(List<InternalReceiptStatusEnum> statuses);
    }
}
