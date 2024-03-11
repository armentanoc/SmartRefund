using SmartRefund.Domain.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartRefund.Application.Interfaces
{
    public interface IInternalAnalyzerService
    {
        Task<IEnumerable<TranslatedVisionReceipt>> GetAllByStatus();

        Task<TranslatedVisionReceipt> UpdateStatus(uint id, string newStatus);
    }
}
