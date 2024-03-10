using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRefund.Domain.Models;

namespace SmartRefund.Application.Interfaces
{
    public interface IInternalAnalyzerService
    {
        Task<IEnumerable<TranslatedVisionReceipt>> GetAllByStatus();
    }
}
