using SmartRefund.Domain.Enums;
using System;

namespace SmartRefund.Domain.Models
{
    public class TranslatedVisionReceipt : BaseEntity
    {
        public RawVisionReceipt RawVisionReceipt { get; private set; }
        public bool IsReceipt { get; private set; }
        public TranslatedVisionReceiptCategoryEnum Category { get; private set; }
        public TranslatedVisionReceiptStatusEnum Status { get; private set; }
        public decimal Total { get; private set; }
        public string Description { get; private set; }

        public TranslatedVisionReceipt()
        {
            //required by EF Core
        }

        public TranslatedVisionReceipt(RawVisionReceipt rawVisionReceipt, bool isReceipt, TranslatedVisionReceiptCategoryEnum category, TranslatedVisionReceiptStatusEnum status, decimal total, string description)
        {
            RawVisionReceipt = rawVisionReceipt;
            IsReceipt = isReceipt;
            Category = category;
            Status = status;
            Total = total;
            Description = description;
        }

        public void SetStatus(TranslatedVisionReceiptStatusEnum status)
        {
            Status = status;
        }
    }
}
