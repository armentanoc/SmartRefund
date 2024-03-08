using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private IFileValidatorService _fileValidator;
        private readonly IContentTypeProvider _contentTypeProvider;


        public EntryController (IFileValidatorService fileValidator, IContentTypeProvider contentTypeProvider)
        {
            _fileValidator = fileValidator;
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file, uint id)
        {
            var filePath = $"wwwroot/{id}{file.FileName}";

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fileStream);
            };

            _fileValidator.Validate(file.Length, file.FileName);
            
            //criar InternalReceipt
            return Ok();

        }

    }
}
