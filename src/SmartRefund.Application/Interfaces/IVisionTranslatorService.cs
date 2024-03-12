using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;

namespace SmartRefund.Application.Interfaces
{
    public interface IVisionTranslatorService
    {
        Task<TranslatedVisionReceipt> GetTranslatedVisionReceipt(RawVisionReceipt rawVisionReceipt);
        bool GetIsReceipt(string isReceipt);
        TranslatedVisionReceiptCategoryEnum GetCategory(string category);
        decimal GetTotal(string total);
        string GetDescription(string description);
    }
}
