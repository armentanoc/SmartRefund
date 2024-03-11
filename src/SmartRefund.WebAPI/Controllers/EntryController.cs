using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Application.Services;
using System.ComponentModel.DataAnnotations;
using SmartRefund.ViewModels;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private IFileValidatorService _fileValidator;

        public EntryController (IFileValidatorService fileValidator)
        {
            _fileValidator = fileValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([Required] IFormFile file, [Required] uint employeeId)
        {
            var filePath = $"wwwroot/{Guid.NewGuid()}-{file.FileName}";

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fileStream);
            };
            var extension = Path.GetExtension(file.FileName);

            _fileValidator.Validate(file.Length, extension, filePath);
           
            InternalReceipt receipt = new InternalReceipt(employeeId);
            return Ok();


                //Para tentarmos fazer a imagem ir para o banco
                /*byte[] barray;
                using (var memoryStream = new MemoryStream())
                {
                await file.CopyToAsync(memoryStream);

                barray = memoryStream.ToArray();
                }*/
                //Colocar barray como parâmetro do construtor

                //CLASSE INTERNALRECEIPT
                // public byte[] Image { get; set; }
        }
    }
}
