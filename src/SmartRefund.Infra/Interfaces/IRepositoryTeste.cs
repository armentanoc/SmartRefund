using SmartRefund.Domain.Models;

namespace SmartRefund.Infra.Interfaces
{
    public interface IRepositoryTeste : IRepository<Teste>
    {
        Task<bool> QualquerCoisa();
    }
}
