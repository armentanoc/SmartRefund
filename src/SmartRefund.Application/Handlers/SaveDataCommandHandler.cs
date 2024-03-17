using MediatR;
using SmartRefund.Application.Handlers.Requests;
using SmartRefund.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Handlers
{
    public class SaveDataCommandHandler : IRequestHandler<SaveDataCommandRequest, Unit>
    {
        private readonly IVisionProcessingWorkerService _visionProcessingWorker;

        public SaveDataCommandHandler(IVisionProcessingWorkerService visionProcessingWorker)
        {
            _visionProcessingWorker = visionProcessingWorker;
        }

        public async Task<Unit> Handle(SaveDataCommandRequest request, CancellationToken cancellationToken)
        {
            await _visionProcessingWorker.ProcessChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
