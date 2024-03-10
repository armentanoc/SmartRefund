using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;
        private readonly IInternalAnalyzerService _internalAnalyzerService;

        public ManagementController(ILogger<ManagementController> logger, IInternalAnalyzerService internalAnalyzerService)
        {
            _logger = logger;
            _internalAnalyzerService = internalAnalyzerService;
        }

        [HttpPatch]
        [Route("update-status")]
        [SwaggerOperation("Update TranslatedVisionReceipt's Status.")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTVRStatusRequest updateRequest)
        {
            var UpdatedObject = await _internalAnalyzerService.UpdateStatus(updateRequest.Id, updateRequest.NewStatus);
            return Ok(UpdatedObject);
        }


    }
}
