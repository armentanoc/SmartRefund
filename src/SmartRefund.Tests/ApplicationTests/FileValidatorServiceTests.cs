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

        //public void Arquivos_jpg_sao_aceitos()
        //{

        //}

        //public void Arquivos_jpeg_sao_aceitos()
        //{

        //}

        //public void Arquivos_png_sao_aceitos()
        //{

        //}

        //public void Outros_tipos_de_arquivos_nao_sao_aceitos()
        //{

        //}

        //public void Se_houver_validacao_internalReceipt_eh_criado()
        //{

        //}

    }
}
