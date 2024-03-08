using Microsoft.AspNetCore.Mvc;
using SmartRefund.Infra.Interfaces;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
