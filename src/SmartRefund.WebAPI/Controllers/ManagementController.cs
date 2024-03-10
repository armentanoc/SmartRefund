using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Enums;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : Controller
    {
        private readonly IInternalAnalyzerService _analyzerService;

        public ManagementController(IInternalAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }


        [HttpGet("receipts/submitted")]
        public async Task<IActionResult> GetAllByStatus()
        {
            var receipts = await _analyzerService.GetAllByStatus();
            if (receipts != null && receipts.Count() != 0)
                return Ok(receipts);
            
            return NotFound();
        }


    }
}
