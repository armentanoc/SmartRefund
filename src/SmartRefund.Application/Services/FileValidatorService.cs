using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private IRepositoryTeste _repository;
        private ILogger<FileValidatorService> _logger;
        public FileValidatorService(IRepositoryTeste repository, ILogger<FileValidatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
    }
}
