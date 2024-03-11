using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using System.Reflection.PortableExecutable;


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

        public async bool Validate(IFormFile file)
        {
            //var path = filePath;
            //var file = System.IO.File.ReadAllBytes(path);

            byte[] header = new byte[4];

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(header, 0, 4);
            }

            if (ValidateSize(lenght) && ValidateType(name, header))
            {
                await _repository.AddAsync(); //fazer o objeto e passar aqui!
                return true;
            }

            return false;
        }

        public bool ValidateSize(long lenght)
        {
            if (lenght > 5 * 1024 * 1024) //mudar para 20MB?
            {
                throw new ArgumentException("Arquivo é maior do que 5MB");
            }
            return true;
        }

        public bool ValidateType(string extension, byte[] header)
        {
            string[] possibleExtensions = [".png", ".jpg", ".jpeg"];

            if(possibleExtensions.Contains(extension))
            {
                if (IsJpeg(header) || IsPng(header))
                {
                    return true;
                }
            }
            else
            {
                throw new ArgumentException("Extensão não permitida");
            }

            return false;
        }

        private static bool IsJpeg(byte[] header)
        {
            // Verifica se os primeiros bytes correspondem ao header de um arquivo JPG
            return header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
        }

        private static bool IsPng(byte[] header)
        {
            // Verifica se os primeiros bytes correspondem ao header de um arquivo PNG
            return header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;
        }

    }
}
