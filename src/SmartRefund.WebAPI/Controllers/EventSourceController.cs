
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Requests;
using SmartRefund.ViewModels.Responses;
using SmartRefund.WebAPI.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventSourceController : ControllerBase

    {
        public IInternalReceiptRepository _internalReceiptRepository;
        public IRawVisionReceiptRepository _rawRepository;
        public ILogger<EventSourceController> _logger;
        public IVisionTranslatorService _translatorService;
        public IVisionExecutorService _visionExecutorService;
        public IEventSourceService _eventSourceService;
        public EventSourceController(IRawVisionReceiptRepository repository, ILogger<EventSourceController> logger, IVisionTranslatorService service, IInternalReceiptRepository internalReceiptRepository, IVisionExecutorService visionExecutorService, IEventSourceService eventSourceService)
        {
            _rawRepository = repository;
            _logger = logger;
            _translatorService = service;
            _internalReceiptRepository = internalReceiptRepository;
            _visionExecutorService = visionExecutorService;
            _eventSourceService = eventSourceService;
        }

        [HttpGet("{hash}/front")]
        [SwaggerOperation("Busque um evento e as entidades vinculadas pelo UniqueHash")]
        [ProducesResponseType(typeof(ReceiptEventSourceResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [TypeFilter(typeof(CombinedAuthorizationFilter))]
        public async Task<ActionResult> GetToFrontUsingEventSource([FromRoute] string hash, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst("userId").Value;
            var userType = httpContextAccessor.HttpContext.User.FindFirst("userType").Value;

            ReceiptEventSourceResponse eventSource;

            if (userType.Equals("employee"))
            {
                if (uint.TryParse(userId, out uint parsedUserId))
                {
                    eventSource = await _eventSourceService.GetEmployeeReceiptEventSourceResponseAsync(hash, true, parsedUserId);
                    return Ok(eventSource);
                }
                return BadRequest(new { errorMessage = $"User ID inconsistente ({userId})." });
            } 
            else
            {
                eventSource = await _eventSourceService.GetReceiptEventSourceResponseAsync(hash, true);
            }
            return Ok(eventSource);
        }

        [HttpGet("front/")]
        [SwaggerOperation("Busque todos os eventos e as entidades vinculadas")]
        [ProducesResponseType(typeof(IEnumerable<ReceiptEventSourceResponse>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [TypeFilter(typeof(CombinedAuthorizationFilter))]
        public async Task<ActionResult> GetAllToFrontUsingEventSource(
            [FromQuery] int[] optionsStatusRefund,
            [FromQuery] int[] optionsStatusTranslate,
            [FromQuery] int[] optionsStatusGPT,
            [FromServices] IHttpContextAccessor httpContextAccessor
            )
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst("userId").Value;
            var userType = httpContextAccessor.HttpContext.User.FindFirst("userType").Value;

            var frontFilter = new FrontFilter(optionsStatusRefund, optionsStatusTranslate, optionsStatusGPT);
            frontFilter = InterpretFrontFilter(frontFilter);

            if (uint.TryParse(userId, out uint parsedUserId))
            {
                var eventSources = await _eventSourceService.GetAllEventSourceResponseAsync(true, parsedUserId, userType, frontFilter);
                return Ok(eventSources);
            }
            return BadRequest(new { errorMessage = $"User ID inconsistente ({userId})." });
        }

        [HttpGet("{hash}/audit")]
        [SwaggerOperation("Busque um evento pelo UniqueHash")]
        [ProducesResponseType(typeof(ReceiptEventSourceResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [TypeFilter(typeof(AuthorizationFilterFinance))]
        public async Task<ActionResult> GetEventSource([FromRoute] string hash)
        {
            var eventSource = await _eventSourceService.GetAuditReceiptEventSourceResponseAsync(hash);
            return Ok(eventSource);
        }

        [TypeFilter(typeof(CombinedAuthorizationFilter))]
        private FrontFilter InterpretFrontFilter(FrontFilter frontFilter)
        {
            if (frontFilter.OptionsStatusRefund.IsNullOrEmpty() || frontFilter.OptionsStatusRefund.All(x => x == 0))
                frontFilter.OptionsStatusRefund = (int[])Enum.GetValues(typeof(TranslatedVisionReceiptStatusEnum));
            if (frontFilter.OptionsStatusTranslate.IsNullOrEmpty() || frontFilter.OptionsStatusTranslate.All(x => x == 0))
                frontFilter.OptionsStatusTranslate = new int[] { 0, 1 };
            if (frontFilter.OptionsStatusGPT.IsNullOrEmpty() || frontFilter.OptionsStatusGPT.All(x => x == 0))
                frontFilter.OptionsStatusGPT = (int[])Enum.GetValues(typeof(InternalReceiptStatusEnum));
            return frontFilter;
        }
    }
}