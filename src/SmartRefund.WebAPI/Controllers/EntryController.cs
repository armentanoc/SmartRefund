using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Application.Services;
using System.ComponentModel.DataAnnotations;
using SmartRefund.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using SmartRefund.ViewModels.Responses;
using SmartRefund.CustomExceptions;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/receipt")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private IFileValidatorService _fileValidator;

        public EntryController (IFileValidatorService fileValidator)
        {
            _fileValidator = fileValidator;
        }

        [HttpPost]
        [SwaggerOperation("Envie o seu comprovante fiscal para análise")]
        [ProducesResponseType(typeof(InternalReceiptResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 413)]
        [ProducesResponseType(typeof(ErrorResponse), 422)]
        public async Task<IActionResult> Post([Required] IFormFile file, [Required] uint employeeId)
        {
            var result = await _fileValidator.Validate(file, employeeId);
            return Ok(result);
        }
    }
}
