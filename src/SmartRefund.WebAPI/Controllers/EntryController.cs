using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Application.Services;
using System.ComponentModel.DataAnnotations;
using SmartRefund.ViewModels;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/entry")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private IFileValidatorService _fileValidator;

        public EntryController (IFileValidatorService fileValidator)
        {
            _fileValidator = fileValidator;
        }

        [HttpPost]
        [Route("submit-receipt")]
        [SwaggerOperation("Envie o seu comprovante fiscal para análise")]
        public async Task<IActionResult> Post([Required] IFormFile file, [Required] uint employeeId)
        {
            var result = await _fileValidator.Validate(file, employeeId);
            return Ok(result);
        }
    }
}
