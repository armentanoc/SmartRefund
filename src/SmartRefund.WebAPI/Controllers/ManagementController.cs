using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Requests;
using SmartRefund.ViewModels.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartRefund.WebAPI.Controllers
{
    
    [Route("api/receipts")]
    [ApiController]
    public class ManagementController : Controller
    {
        private readonly IInternalAnalyzerService _analyzerService;

        public ManagementController(IInternalAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpGet("/submitted")]
        [SwaggerOperation("Visualize todas as solicitações pendentes")]
        [ProducesResponseType(typeof(IEnumerable<TranslatedReceiptResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetAllByStatus()
        {
            var receipts = await _analyzerService.GetAllByStatus();
            if (receipts != null && receipts.Count() != 0)
                return Ok(receipts);

            return NotFound("Não encontrado");
        }

   /*     [HttpPatch]
        [Route("/status")]
        [SwaggerOperation("Atualize o status da solicitação por ID")]
        [ProducesResponseType(typeof(TranslatedVisionReceipt), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTVRStatusRequest updateRequest)
        {
            var UpdatedObject = await _analyzerService.UpdateStatus(updateRequest.Id, updateRequest.NewStatus);
            return Ok(UpdatedObject);
        }*/

        [HttpPatch]
        [Route("/status/{id}")]
        [SwaggerOperation("Atualize o status da solicitação por ID")]
        [ProducesResponseType(typeof(TranslatedVisionReceipt), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(string), 405)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateStatus(uint id, [FromBody] string newStatus)
        {
            var UpdatedObject = await _analyzerService.UpdateStatus(id, newStatus);
            return Ok(UpdatedObject);
        }

        // Apenas para visualização
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var receipts = await _analyzerService.GetAll();
            //if (receipts != null && receipts.Count() != 0)
            return Ok(receipts);

            //return NotFound();
        }

    }
}
