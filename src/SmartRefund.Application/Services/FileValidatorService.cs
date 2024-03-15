using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;


namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private IInternalReceiptRepository _repository;
        private ILogger<FileValidatorService> _logger;

        public FileValidatorService(IInternalReceiptRepository repository, ILogger<FileValidatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<InternalReceiptResponse?> Validate(IFormFile file, uint employeeId)
        {
            
            if (ValidateSize(file.Length) && ValidateExtension(file.FileName) && await ValidateType(file)) 
            {
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                var uniqueHash = await GenerateUniqueHash();
                InternalReceipt receipt = new InternalReceipt(employeeId, imageBytes, uniqueHash);
                InternalReceiptResponse response = new InternalReceiptResponse(receipt);

                await _repository.AddAsync(receipt);

                return response;
            }

            return null;

        }

        private async Task<string> GenerateUniqueHash()
        {
            var receipts = await _repository.GetAllAsync();
            InternalReceipt? lastReceipt = receipts.LastOrDefault();
            uint id = lastReceipt?.Id ?? 0;
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(id));
        }

        public bool ValidateSize(long lenght)
        {
            if (lenght >= 20 * 1024 * 1024)
            {
                throw new InvalidFileSizeException(lenght);
            }
            return true;
        }

        public bool ValidateExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            string[] possibleExtensions = [".png", ".jpg", ".jpeg"];

            if(possibleExtensions.Contains(extension))
            {
                return true;
            }
            else
            {
                throw new InvalidFileTypeException(extension);
            }

        }

        public async Task<bool> ValidateType(IFormFile file)
        {
            byte[] header = new byte[4];

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                memoryStream.Read(header, 0, 4);
            }

            if (IsJpeg(header) || IsPng(header))
            {
                return true;
            }

            throw new InvalidFileTypeException(Path.GetExtension(file.FileName));
        }

        private static bool IsJpeg(byte[] header)
        {
            // Verifica se os primeiros bytes correspondem ao header de um arquivo JPEG or JPG
            return (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF) // JPEG
                || (header[0] == 0x4A && header[1] == 0x46 && header[2] == 0x49 && header[3] == 0x46); // JFIF
        }

        private static bool IsPng(byte[] header)
        {
            // Verifica se os primeiros bytes correspondem ao header de um arquivo PNG
            return header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;
        }
    }
}
