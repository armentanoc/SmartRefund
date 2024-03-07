
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RepositoryTeste : Repository<Teste>, IRepositoryTeste
    {
        public RepositoryTeste(AppDbContext context) : base(context)
        {
        }
    }
}
