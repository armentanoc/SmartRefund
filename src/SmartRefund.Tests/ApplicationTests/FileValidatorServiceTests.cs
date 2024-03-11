using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Infra.Interfaces;
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

        public FileValidatorServiceTests()
        {
            _mockReceiptRepository = Substitute.For<IInternalReceiptRepository>();
            ILogger<FileValidatorService> loggerMock = Substitute.For<ILogger<FileValidatorService>>();
            _fileValidatorService = new FileValidatorService(_mockReceiptRepository, loggerMock);
        }

        [Fact]
        public void Tamanho_da_imagem_não_pode_ser_igual_ou_maior_que_vinte_mb()
        {
            

        }

        [Fact]
        public void Imagens_menores_que_20_mb_sao_aceitas()
        {
            //Arrange
            long tamanho = 19 * 1024 * 1024;

            //Act
            bool result = _fileValidatorService.ValidateSize(tamanho);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("ImageTest.jpg")]
        [InlineData("ImageTest02.jpg")]
        public void Jpg_files_are_accepted(string fileName)
        {
            Assert.True(_fileValidatorService.ValidateType(fileName));
        }

        [Theory]
        [InlineData("ImageTest.jpeg")]
        [InlineData("ImageTest02.jpeg")]
        public void Jpeg_files_are_accepted(string fileName)
        {
            Assert.True(_fileValidatorService.ValidateType(fileName));
        }

        [Theory]
        [InlineData("ImageTest.png")]
        [InlineData("ImageTest02.png")]
        public void Png_files_are_accepted(string fileName)
        {
            Assert.True(_fileValidatorService.ValidateType(fileName));
        }

        [Theory]
        [InlineData("TextBookTest.pdf")]
        [InlineData("GifTest.gif")]
        [InlineData("ImageTestZip.zip")]
        public void Other_file_types_are_not_accepted(string fileName)
        {
            Assert.Throws<ArgumentException> (() => _fileValidatorService.ValidateType(fileName));
        }

        //public void Se_houver_validacao_internalReceipt_eh_criado()
        //{

        //}

    }
}
