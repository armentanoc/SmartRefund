using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Tests.ApplicationTests
{
    public class FileValidatorServiceTests
    {
        private FileValidatorService _fileValidatorService;
        private IInternalReceiptRepository _mockReceiptRepository;

        public FileValidatorServiceTests()
        {
            _mockReceiptRepository = Substitute.For<IInternalReceiptRepository>();
            ILogger<FileValidatorService> loggerMock = Substitute.For<ILogger<FileValidatorService>>();
            _fileValidatorService = new FileValidatorService(_mockReceiptRepository, loggerMock);
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
        public async Task If_is_validated_an_internal_receipt_is_created()
        {
            // Arrange
            var file = new Mock<IFormFile>();
            var sourceImgPath = @"../../../ApplicationTests/Assets/example.jpg";
            var sourceImgBytes = File.ReadAllBytes(sourceImgPath);
            var ms = new MemoryStream(sourceImgBytes);
            var fileName = "example.jpg";

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

            // Act
            var result = await _fileValidatorService.Validate(inputFile, employeeId);

            // Assert
            result.Should().BeOfType<InternalReceiptResponse>();
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

            // Act and Assert
            await Assert.ThrowsAsync<InvalidFileTypeException>(async () => await _fileValidatorService.Validate(inputFile, employeeId));
        }

        [Fact]
        public async Task Pdf_with_png_extension_not_accepted()
        {
            // Arrange
            var file = new Mock<IFormFile>();
            var sourceImgPath = @"../../../ApplicationTests/Assets/invalidfile.png"; 
            var sourceImgBytes = File.ReadAllBytes(sourceImgPath);
            var ms = new MemoryStream(sourceImgBytes);
            var fileName = "invalidfile.png"; 

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

            // Act
            var result = await _fileValidatorService.Validate(inputFile, employeeId);

            // Assert
            Assert.Null(result);
        }

    }
}