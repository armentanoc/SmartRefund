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

        public async Task<InternalReceipt?> Validate(IFormFile file, uint employeeId)
        {
            
            if (ValidateSize(file.Length) && ValidateType(file.FileName) && ValidateExtension(file)) //await
            {
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                InternalReceipt receipt = new InternalReceipt(employeeId, imageBytes);

                await _repository.AddAsync(receipt);

                return receipt;
            }

            return null;

        }

        public bool ValidateSize(long lenght)
        {
            if (lenght >= 20 * 1024 * 1024)
            {
                throw new ArgumentException("Arquivo é maior do que 20MB");
            }
            return true;
        }

        public bool ValidateType(string fileName)
        {
            string[] possibleExtensions = [".png", ".jpg", ".jpeg"];

            if(possibleExtensions.Contains(fileName))
            {
                return true;
            }
            else
            {
                throw new ArgumentException("Extensão não permitida");
            }

            return false;
        }

        public bool ValidateExtension(IFormFile file)  //async Task<bool>
        {
            //byte[] header = new byte[4];
            //using (var memoryStream = new MemoryStream())
            //{
            //    await file.CopyToAsync(memoryStream);
            //    memoryStream.Read(header, 0, 4);
            //}

            //if (IsJpeg(header) || IsPng(header))
            //{
                return true;
            //}

            //throw new ArgumentException("Extensão não permitida");
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
