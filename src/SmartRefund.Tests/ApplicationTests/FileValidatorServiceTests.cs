using SmartRefund.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Tests.ApplicationTests
{
    public class FileValidatorServiceTests
    {
        private IFileValidatorService _fileValidatorService;

        public FileValidatorServiceTests(IFileValidatorService fileValidatorService)
        {
            _fileValidatorService = fileValidatorService;
        }

        [Fact]
        public void Tamanho_da_imagem_não_pode_ser_igual_ou_maior_que_vinte_mb()
        {
            

        }

        [Fact]
        public void Imagens_menores_ou_igual_20_mb_sao_aceitas()
        {
            //Arrange
            int tamanho = 21 * 1024 * 1024;

            //Act
            _fileValidatorService
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
