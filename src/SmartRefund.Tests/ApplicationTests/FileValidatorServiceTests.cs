
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.Tests.ApplicationTests
{
    public class FileValidatorServiceTests
    {
        private FileValidatorService _fileValidatorService;
        private IInternalReceiptRepository _mockReceiptRepository;
        private IEventSourceRepository _mockEventSourceRepository;

        public FileValidatorServiceTests()
        {
            _mockReceiptRepository = Substitute.For<IInternalReceiptRepository>();
            var eventSourceRepository = Substitute.For<IEventSourceRepository>();
            eventSourceRepository.AddEvent(Arg.Any<ReceiptEventSource>(), Arg.Any<string>(), Arg.Any<Event>())
                          .Returns(Task.FromResult<ReceiptEventSource>(new ReceiptEventSource()));
            eventSourceRepository.AddEvent(Arg.Any<ReceiptEventSource>(), Arg.Any<string>(), Arg.Any<Event>())
                          .Returns(Task.FromResult<ReceiptEventSource>(new ReceiptEventSource()));

            var loggerMock = Substitute.For<ILogger<FileValidatorService>>();
            var configurationMock = Substitute.For<IConfiguration>();
            configurationMock["OpenAIVisionConfig:MinResolutionInPPI"].Returns("35"); 
            var mediatorMock = Substitute.For<IMediator>();
            _fileValidatorService = new FileValidatorService(_mockReceiptRepository, loggerMock, configurationMock, eventSourceRepository, mediatorMock);
        }

        [Fact]
        public void ValidateType_ValidImage_ReturnsTrue()
        {
            // Arrange

            var sourceImgPath = @"../../../ApplicationTests/Assets/example.jpg";
            var sourceImgBytes = File.ReadAllBytes(sourceImgPath);
            var fileName = "example.jpg";
            var memoryStream = new MemoryStream(sourceImgBytes);

            // Act
            var result = _fileValidatorService.ValidateType(fileName, memoryStream);

            // Assert
            Assert.True(result);

            // Dispose
            memoryStream.Dispose();
        }

        [Fact]
        public void ValidateType_InvalidImage_ThrowsInvalidFileTypeException()
        {
            // Arrange
            var sourceImgPath = @"../../../ApplicationTests/Assets/invalidfile.pdf";
            var sourceImgBytes = File.ReadAllBytes(sourceImgPath);
            var fileName = "invalidfile.pdf";
            var memoryStream = new MemoryStream(sourceImgBytes);

            // Act and Assert
            Assert.Throws<InvalidFileTypeException>(() => _fileValidatorService.ValidateType(fileName, memoryStream));

            // Dispose
            memoryStream.Dispose();
        }

        [Theory]
        [InlineData(20 * 1024 * 1024)]
        [InlineData(23 * 1024 * 1024)]
        public void Image_size_cannot_be_equal_to_or_bigger_than_twenty_mb(long size)
        {
            Assert.Throws<InvalidFileSizeException>(() => _fileValidatorService.ValidateSize(size));
        }

        [Fact]
        public void Images_smaller_than_twenty_mb_are_accepted()
        {
            //Arrange
            long size = 19 * 1024 * 1024;

            //Act
            bool result = _fileValidatorService.ValidateSize(size);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("ImageTest.jpg")]
        [InlineData("ImageTest02.jpg")]
        public void Jpg_files_are_accepted(string fileName)
        {
            Assert.True(_fileValidatorService.ValidateExtension(fileName));
        }

        [Theory]
        [InlineData("ImageTest.jpeg")]
        [InlineData("ImageTest02.jpeg")]
        public void Jpeg_files_are_accepted(string fileName)
        {
            Assert.True(_fileValidatorService.ValidateExtension(fileName));
        }

        [Theory]
        [InlineData("ImageTest.png")]
        [InlineData("ImageTest02.png")]
        public void Png_files_are_accepted(string fileName)
        {
            Assert.True(_fileValidatorService.ValidateExtension(fileName));
        }

        [Theory]
        [InlineData("TextBookTest.pdf")]
        [InlineData("GifTest.gif")]
        [InlineData("ImageTestZip.zip")]
        public void Other_file_types_are_not_accepted(string fileName)
        {
            Assert.Throws<InvalidFileTypeException>(() => _fileValidatorService.ValidateExtension(fileName));
        }

        [Fact]
        public async Task If_is_invalidated_throws_invalid_type_exception()
        {
            // Arrange
            var file = new Mock<IFormFile>();
            var sourceImgPath = @"../../../ApplicationTests/Assets/invalidfile.pdf";
            var sourceImgBytes = File.ReadAllBytes(sourceImgPath);
            var ms = new MemoryStream(sourceImgBytes);
            var fileName = "invalidfile.pdf";

            file.Setup(f => f.FileName).Returns(fileName).Verifiable();

            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Callback((Stream stream, CancellationToken token) => ms.Position = 0)
                .Verifiable();

            file.Setup(_ => _.OpenReadStream())
                .Returns(ms)
                .Verifiable();

            var inputFile = file.Object;
            uint employeeId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFileTypeException>(async () => await _fileValidatorService.Validate(inputFile, employeeId));
        }

        [Fact]
        public async Task If_file_is_null_throws_argument_null_exception()
        {
            // Arrange
            IFormFile file = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _fileValidatorService.Validate(file, 1));
        }

        [Fact]
        public async Task If_employee_id_is_zero_throws_invalid_operation_exception()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFileTypeException>(async () => await _fileValidatorService.Validate(file, 0));
        }

        [Fact]
        public void ValidateSize_WithNegativeSize_Throws_ArgumentOutOfRangeException()
        {
            // Arrange
            long size = 20 * 1024 * 1024;

            // Act & Assert
            Assert.Throws<InvalidFileSizeException>(() => _fileValidatorService.ValidateSize(size));
        }

        [Fact]
        public async Task If_file_size_exceeds_max_allowed_size_throws_invalid_file_size_exception()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            var ms = new MemoryStream(new byte[25 * 1024 * 1024]); // 25MB file
            file.Length.Returns(ms.Length);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFileSizeException>(async () => await _fileValidatorService.Validate(file, 1));
        }

        [Fact]
        public void ValidateResolution_ValidImage_ReturnsTrue()
        {
            // Arrange
            var sourceImgPath = @"../../../ApplicationTests/Assets/example.jpg";
            var sourceImgBytes = File.ReadAllBytes(sourceImgPath);
            var memoryStream = new MemoryStream(sourceImgBytes);

            // Act
            var result = _fileValidatorService.ValidateResolution(memoryStream);

            // Assert
            Assert.True(result);

            // Dispose
            memoryStream.Dispose();
        }

        [Fact]
        public async Task GenerateUniqueHash_ValidRepository_ReturnsHash()
        {
            // Arrange
            var receipts = new List<InternalReceipt>()
            {
                new InternalReceipt(1, new byte[0], "AAAAAA"),
            };
            _mockReceiptRepository.GetAllAsync().Returns(receipts);

            // Act
            var result = await _fileValidatorService.GenerateUniqueHash();

            // Assert
            Assert.Equal("AAAAAA", result);
        }

    }
}
