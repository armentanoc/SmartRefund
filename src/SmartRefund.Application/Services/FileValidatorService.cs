using SmartRefund.Application.Interfaces;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private IRepositoryTeste _repository;
        public FileValidatorService(IRepositoryTeste repository)
        {
            _repository = repository;
        }
    }
}
