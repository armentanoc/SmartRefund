using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Infra.Interfaces;


namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private IRepositoryTeste _repository;
        private ILogger<FileValidatorService> _logger;
        private string? _errormessage;
        public string ErrorMessage
        {
            get
            {
                return _errormessage;
            }
            private set
            {
                _errormessage = value;
            }
        }


        public FileValidatorService(IRepositoryTeste repository, ILogger<FileValidatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public bool Validate(long lenght, string name)
        {
            //var path = filePath;
            //var file = System.IO.File.ReadAllBytes(path);

            if (!ValidateSize(lenght) && !ValidateType(name))
            {
                return false;
            }

            return true;
        }

        public bool ValidateSize(long lenght)
        {
            if (lenght > 2 * 1024 * 1024)
            {
                _errormessage = "Erro ao fazer upload. Arquivo é maior do que 20MB";
                return false;
            }
            return true;
        }

        public bool ValidateType(string fileName)
        {
            string[] extensoesPermitidas = [".png", ".jpg", ".jpeg"];
            if (!extensoesPermitidas.Contains(fileName))
            {
                _errormessage = "Erro ao fazer upload. Arquivo é maior do que 20MB";
                return false;
            }
            return true;
        }

    }
}
