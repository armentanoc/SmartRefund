
using SmartRefund.Domain.Models;

namespace SmartRefund.Infra.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(uint id);
        Task<T> AddAsync(T newEntity);
        Task<T> UpdateAsync(T newEntity);
        Task<bool> DeleteAsync(uint id);
        Task<bool> EntityExistsAsync(T newEntity);
    }
}
