using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionProcessingWorkerService
    {
        Task ProcessChangesAsync(CancellationToken stoppingToken);
    }
}
