using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Services
{
    public class FileValidatorService : IFileValidatorService
    {
        private double _minPPI;
        private readonly IConfiguration _configuration;
        private IInternalReceiptRepository _repository;
        private ILogger<FileValidatorService> _logger;
        private IEventSourceRepository _eventSourceRepository;

        public FileValidatorService(IInternalReceiptRepository repository, ILogger<FileValidatorService> logger, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
            GetPPIConfiguration();
            _eventSourceRepository = eventSourceRepository;
        }

        public void GetPPIConfiguration()
        {
            var property = _configuration["OpenAIVisionConfig:MinResolutionInPPI"];
            if (double.TryParse(property, out double minPPI))
                _minPPI = minPPI;
            else 
                throw new VisionConfigurationException(nameof(property));
        }

        public async Task<InternalReceiptResponse?> Validate(IFormFile file, uint employeeId)
        {
            if (ValidateSize(file.Length) && ValidateExtension(file.FileName))
            {
                byte[] imageBytes;

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    ValidateType(file.FileName, memoryStream);
                    ValidateResolution(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                var uniqueHash = await GenerateUniqueHash();
                InternalReceipt receipt = new InternalReceipt(employeeId, imageBytes, uniqueHash);
                InternalReceiptResponse response = new InternalReceiptResponse(receipt);

                ReceiptEventSource eventSource = new ReceiptEventSource(receipt, receipt.UniqueHash);
                await _eventSourceRepository.AddEvent(eventSource.UniqueHash, new Event(eventSource.UniqueHash, EventSourceStatusEnum.InternalReceiptCreated, receipt.CreationDate, "Internal Receipt created with success"));

                await _repository.AddAsync(receipt);

                return response;
            }

            throw new InvalidOperationException("File validation failed");
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
                throw new InvalidFileSizeException(lenght);
            return true;
        }

        public bool ValidateExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            string[] possibleExtensions = [".png", ".jpg", ".jpeg"];

            if (!possibleExtensions.Contains(extension))
                throw new InvalidFileTypeException(extension);
            return true;
        }

        public bool ValidateType(string fileName, MemoryStream memoryStream)
        {
            byte[] header = new byte[4];
            memoryStream.Read(header, 0, 4);

            if (IsJpeg(header) || IsPng(header))
                return true;

            throw new InvalidFileTypeException(Path.GetExtension(fileName));
        }

        public bool ValidateResolution(MemoryStream memoryStream)
        {
            // Reseta a posição da stream antes de ler a imagem
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (var image = Image.Load(memoryStream))
            {
                // Calcula a dimensão da imagem em polegadas
                double widthInInches = image.Width / image.Metadata.HorizontalResolution;
                double heightInInches = image.Height / image.Metadata.VerticalResolution;

                // Calcula o PPI horizontal e vertical
                double horizontalPPI = image.Width / widthInInches;
                double verticalPPI = image.Height / heightInInches;

                // Calcula o PPI médio
                double averagePPI = (horizontalPPI + verticalPPI) / 2;

                //Checa se o PPI médio é maior que o PPI mínimo
                if (averagePPI >= _minPPI)
                    return true;

                throw new InvalidFileResolutionException(requiredPPI: _minPPI, imagePPI: averagePPI);
            }
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
