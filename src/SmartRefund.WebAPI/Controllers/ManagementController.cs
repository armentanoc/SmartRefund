﻿using Microsoft.AspNetCore.Mvc;
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
    
    [Route("api/receipt")]
    [ApiController]
    public class ManagementController : Controller
    {
        private readonly IInternalAnalyzerService _analyzerService;

        public ManagementController(IInternalAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpGet("submitted")]
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

   

        [HttpPatch]
        [Route("status/{id}")]
        [SwaggerOperation("Atualize o status da solicitação por UniqueHash.")]
        [ProducesResponseType(typeof(TranslatedReceiptResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(string), 405)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTVRStatusRequest request)
        {
            var UpdatedObject = await _analyzerService.UpdateStatus(request.UniqueHash, request.NewStatus);
            return Ok(UpdatedObject);
        }
    }
}
