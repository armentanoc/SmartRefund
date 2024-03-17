using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.ViewModels.Requests;
using SmartRefund.WebAPI.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthorizationFilterFinance))]
    public class UpdateStatusController : ControllerBase
    {
        private readonly IInternalAnalyzerService _analyzerService;

        public UpdateStatusController(IInternalAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpPatch]
        [SwaggerOperation("Update TranslatedVisionReceipt's Status.")]
        public async Task<IActionResult> Update([FromBody] UpdateTVRStatusRequest updateRequest)
        {
            var UpdatedObject = await _analyzerService.UpdateStatus(updateRequest.Id, updateRequest.NewStatus);
            return Ok(UpdatedObject);
        }
    }
}
