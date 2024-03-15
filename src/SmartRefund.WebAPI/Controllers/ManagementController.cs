using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.ViewModels.Requests;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.WebAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/management")]
    public class ManagementController : Controller
    {
        private readonly IInternalAnalyzerService _analyzerService;

        public ManagementController(IInternalAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpGet("receipts/submitted")]
        [SwaggerOperation("Visualize todas as solicitações pendentes")]
        public async Task<IActionResult> GetAllByStatus()
        {
            var receipts = await _analyzerService.GetAllByStatus();
            if (receipts != null && receipts.Count() != 0)
                return Ok(receipts);

            return NotFound();
        }

        [HttpPatch("update-status")]
        [SwaggerOperation("Atualize o status da solicitação por UniqueHash.")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTVRStatusRequest request)
        {
            var UpdatedObject = await _analyzerService.UpdateStatus(request.UniqueHash, request.NewStatus);
            return Ok(UpdatedObject);
        }
    }
}
