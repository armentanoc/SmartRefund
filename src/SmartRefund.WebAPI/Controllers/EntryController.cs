using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Application.Services;
using System.ComponentModel.DataAnnotations;

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

            if(_fileValidator.Validate(file.Length, file.FileName))
            {
                //criar InternalReceipt
                InternalReceipt receipt = new InternalReceipt(employeeId);
                return Ok();
            }

            //Titulo = "Erro ao fazer upload",
            //Mensagem = _fileValidator.ErrorMessage,
            //StatusCode = 400
            return BadRequest(400);
        }
    }
}
