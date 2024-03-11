
using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Requests;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : ControllerBase
    {
        public IRawVisionReceiptRepository _repository;
        public ILogger<TesteController> _logger;
        public IVisionTranslatorService _service;
        public TesteController(IRawVisionReceiptRepository repository, ILogger<TesteController> logger, IVisionTranslatorService service)
        {
            _repository = repository;   
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(RawVisionReceiptRequest rawVisionReceiptRequest)
        {
            var internalReceipt = new InternalReceipt(1);
            var rawVisionReceipt = new RawVisionReceipt(internalReceipt, rawVisionReceiptRequest.IsReceipt, rawVisionReceiptRequest.Category, rawVisionReceiptRequest.Total, rawVisionReceiptRequest.Description, false);
            var createdRawVisionReceipt = await _repository.AddAsync(rawVisionReceipt);
            return CreatedAtAction("GetItemById", new { id = createdRawVisionReceipt.Id }, new { message = "Resource created successfully.", data = createdRawVisionReceipt });
        }

        [HttpGet("{id}")]
        public async Task<RawVisionReceipt> GetItemById([FromRoute] uint id)
        {
            return await _repository.GetAsync(id);
        }

        [HttpGet("testaTraducao")]
        public async Task<ActionResult<IEnumerable<TranslatedVisionReceipt>>> TryTestingTranslation()
        {
            IEnumerable<RawVisionReceipt> rawVisionReceipts = await _repository.GetAllAsync();
            IEnumerable<TranslatedVisionReceipt> translatedReceipts = new List<TranslatedVisionReceipt>();

            foreach (var rawReceipt in rawVisionReceipts)
            {
                var translatedReceipt = _service.GetTranslatedVisionReceipt(rawReceipt);
                translatedReceipts.Append(translatedReceipt);
            }

            return Ok(translatedReceipts);
        }
    }
}
