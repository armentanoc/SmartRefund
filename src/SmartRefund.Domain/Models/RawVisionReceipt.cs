
namespace SmartRefund.Domain.Models
{
    public class RawVisionReceipt : BaseEntity
    {

        public InternalReceipt InternalReceipt { get; private set; }
        public string IsReceipt { get; private set; }
        public string Category { get; private set; }
        public string Total { get; private set; }
        public string Description { get; private set; }
        public bool IsTranslated { get; private set; }

        public RawVisionReceipt()
        {
            // required by EF Core
        }

        public RawVisionReceipt(InternalReceipt internalReceipt, string isReceipt, string category, string total, string description, bool isTranslated)
        {
            InternalReceipt = internalReceipt;
            IsReceipt = isReceipt;
            Category = category;
            Total = total;
            Description = description;
            IsTranslated = isTranslated;
        }
        public void SetIsTranslated(bool isTranslated)
        {
            IsTranslated = isTranslated;
        }
    }
}