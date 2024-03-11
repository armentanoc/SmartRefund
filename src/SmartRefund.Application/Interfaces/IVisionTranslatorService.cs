using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionTranslatorService
    {
        TranslatedVisionReceipt GetTranslatedVisionReceipt(RawVisionReceipt rawVisionReceipt);
        bool GetIsReceipt(string isReceipt);
        TranslatedVisionReceiptCategoryEnum GetCategory(string category);
        decimal GetTotal(string total);
        string GetDescription(string description);
    }
}
