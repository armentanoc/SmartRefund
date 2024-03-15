using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using System.Reflection.PortableExecutable;


namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private IInternalReceiptRepository _repository;
        private ILogger<FileValidatorService> _logger;
        private IEventSourceRepository _eventSourceRepository;

        public FileValidatorService(IInternalReceiptRepository repository, ILogger<FileValidatorService> logger, IEventSourceRepository eventSourceRepository)
        {
            _repository = repository;
            _logger = logger;
            _eventSourceRepository = eventSourceRepository;
        }

        public async Task<InternalReceiptResponse?> Validate(IFormFile file, uint employeeId)
        {
            
            if (ValidateSize(file.Length) && ValidateExtension(file.FileName)) // && await ValidateType(file)
            {
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                InternalReceipt receipt = new InternalReceipt(employeeId, imageBytes);
                InternalReceiptResponse response = new InternalReceiptResponse(receipt);

                ReceiptEventSource eventSourcing = new ReceiptEventSource(receipt.Id);
                await _eventSourceRepository.AddEvent(eventSourcing.Id, new InternalReceiptCreated(receipt.Id, receipt.CreationDate, receipt.Status));

                await _repository.AddAsync(receipt);

                return response;
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
                throw new ArgumentException("Extensão não permitida");
            }

        }

        public async Task<bool> ValidateType(IFormFile file)
        {
            byte[] header = new byte[4];
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Read(header, 0, 4);
            }

            if (IsJpeg(header) || IsPng(header))
            {
                return true;
            }

            throw new ArgumentException("Extensão não permitida");
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
