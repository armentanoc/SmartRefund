using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.WebAPI.Filters;
using System.ComponentModel.DataAnnotations;
using SmartRefund.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using SmartRefund.ViewModels.Responses;
using SmartRefund.CustomExceptions;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartRefund.WebAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/receipt")]
    [ApiController]
    [TypeFilter(typeof(AuthorizationFilterEmployee))]
    public class EntryController : ControllerBase
    {
        private IFileValidatorService _fileValidator;

        public EntryController(IFileValidatorService fileValidator)
        {
            _fileValidator = fileValidator;
        }

        [HttpPost]
        [SwaggerOperation("Envie o seu comprovante fiscal para análise")]
        [ProducesResponseType(typeof(InternalReceiptResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 413)]
        [ProducesResponseType(typeof(ErrorResponse), 422)]
        public async Task<IActionResult> Post([Required] IFormFile file, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("userId");
            var userTypeClaim = httpContextAccessor.HttpContext.User.FindFirst("userType");

            if (userIdClaim != null && userTypeClaim.Value.Equals("employee") && uint.TryParse(userIdClaim.Value, out uint userId))
            {
                var result = await _fileValidator.Validate(file, userId);
                return Ok(result);
            }

            return Unauthorized(new { errorMessage = "User not authorized." });
        }
    }
}
