
using Microsoft.AspNetCore.Mvc;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;

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
        //public MyBackgroundWorker _backgroundWorker;
        public EventSourceController(IRawVisionReceiptRepository repository, ILogger<EventSourceController> logger, IVisionTranslatorService service, IInternalReceiptRepository internalReceiptRepository, IVisionExecutorService visionExecutorService, IEventSourceService eventSourceService)
        {
            _rawRepository = repository;
            _logger = logger;
            _translatorService = service;
            _internalReceiptRepository = internalReceiptRepository;
            _visionExecutorService = visionExecutorService;
            _eventSourceService = eventSourceService;
        }

        [HttpGet("front/{hash}")]
        public async Task<ActionResult> GetToFrontUsingEventSource([FromRoute] string hash)
        {
            var eventSource = await _eventSourceService.GetReceiptEventSourceResponseAsync(hash, true);
            return Ok(eventSource);
        }

        [HttpGet("front/all")]
        public async Task<ActionResult> GetAllToFrontUsingEventSource()
        {
            var eventSources = await _eventSourceService.GetAllEventSourceResponseAsync(true);
            return Ok(eventSources);
        }

        [HttpGet("audit/{hash}")]
        public async Task<ActionResult> GetEventSource([FromRoute] string hash)
        {
            var eventSource = await _eventSourceService.GetReceiptEventSourceResponseAsync(hash, false);
            return Ok(eventSource);
        }
    }
}