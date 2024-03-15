using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _mockEventSourceRepository = Substitute.For<IEventSourceRepository>();
            ILogger<FileValidatorService> loggerMock = Substitute.For<ILogger<FileValidatorService>>();
            _fileValidatorService = new FileValidatorService(_mockReceiptRepository, loggerMock, _mockEventSourceRepository);
        }

        [Theory]
        [InlineData(20 * 1024 * 1024)]
        [InlineData(23 * 1024 * 1024)]
        public void Image_size_cannot_be_equal_to_or_bigger_than_twenty_mb(long size)
        {
            Assert.Throws<ArgumentException>(() => _fileValidatorService.ValidateSize(size));
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
            Assert.Throws<ArgumentException> (() => _fileValidatorService.ValidateExtension(fileName));
        }

        [Fact]
        public async void If_is_validated_an_internal_receipt_is_created()
        {
            //Arrange
            var fileMock = Substitute.For<IFormFile>();
            fileMock.Length.Returns(2 * 1024 * 1024);
            fileMock.FileName.Returns("example.jpg");
            uint employeeId = 123;

            // Act
            var result = await _fileValidatorService.Validate(fileMock, employeeId);

            // Assert
            result.Should().BeOfType<InternalReceiptResponse>();

        }

    }
}
