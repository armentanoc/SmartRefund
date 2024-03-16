using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Application.Services;
using System.ComponentModel.DataAnnotations;
using SmartRefund.ViewModels;
using SmartRefund.WebAPI.Filters;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthorizationFilterEmployee))]
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
            
            var result = await _fileValidator.Validate(file, employeeId);

            return Ok(result);

        }
    }
}
