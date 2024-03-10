using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;


namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private IInternalReceiptRepository _repository;
        private ILogger<FileValidatorService> _logger;
        private string? _errormessage;
        public string? ErrorMessage
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


        public FileValidatorService(IInternalReceiptRepository repository, ILogger<FileValidatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public bool Validate(long lenght, string name)
        {
            //var path = filePath;
            //var file = System.IO.File.ReadAllBytes(path);

            if (ValidateSize(lenght) && ValidateType(name))
            {
                return true;
            }

            return false;
        }

        public bool ValidateSize(long lenght)
        {
            if (lenght > 5 * 1024 * 1024) //mudar para 20MB?
            {
                throw new ArgumentException("Arquivo é maior do que 20MB");
            }
            return true;
        }

        public bool ValidateType(string extension)
        {
            string[] possibleExtensions = [".png", ".jpg", ".jpeg"];

            if (!possibleExtensions.Contains(extension))
                throw new ArgumentException("Extensão não permitida");

            return true;
        }

    }
}
