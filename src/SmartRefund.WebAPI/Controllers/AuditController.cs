using Microsoft.AspNetCore.Mvc;

namespace SmartRefund.WebAPI.Controllers
{
    [Route("api/audit")]
    [ApiController]
    public class AuditController : Controller
    {
        // private IEventSourceService _eventSourceService;
        public AuditController(/*IEventSourceService eventSourceService*/) {
        // _eventSourceService = eventSourceService;
        }

        [HttpGet]
        [Route("/receipt/{id}")]
        public async Task<IActionResult> GetReceiptEventSourceById(uint id)
        {
            // var audit = await _eventSourceService.GetReceiptEventSourceById(id);
            return Ok();
        }



    }
}
