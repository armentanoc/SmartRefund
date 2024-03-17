using SmartRefund.Domain.Enums;

namespace SmartRefund.Domain.Models
{
    public class ReceiptEventSource : BaseEntity
    {
        public InternalReceipt InternalReceipt { get; private set; }
        public RawVisionReceipt? RawVisionReceipt { get; private set; }
        public TranslatedVisionReceipt? TranslatedVisionReceipt { get; private set; }
        public string UniqueHash { get; private set; }
        public List<Event> Events { get; private set; }
        public EventSourceStatusEnum CurrentStatus {  get; private set; }

        public ReceiptEventSource()
        { 
            // required by EF Core
        }

        public ReceiptEventSource(string uniqueHash, EventSourceStatusEnum currentStatus, InternalReceipt internalReceipt)
        {
            UniqueHash = uniqueHash;
            Events = new List<Event>();
            CurrentStatus = currentStatus;
            InternalReceipt = internalReceipt;
            RawVisionReceipt = null;
            TranslatedVisionReceipt = null;
        }

        public void SetUniqueHash(string hash)
        {
            UniqueHash = hash;
        }

        public void ChangeStatus(EventSourceStatusEnum status)
        {
            CurrentStatus = status;
        }

        public void AddEvent (Event evnt)
        {
            if(Events is null)
                Events = new List<Event>();
            Events.Add(evnt);
        }

        public bool SetTranslatedVisionReceipt(TranslatedVisionReceipt translatedVisionReceipt)
        {
            if (translatedVisionReceipt is TranslatedVisionReceipt)
            {
                TranslatedVisionReceipt = translatedVisionReceipt;
                return true;
            }
            return false;
        }

        public bool SetRawVisionReceipt(RawVisionReceipt rawVisionReceipt)
        {
            if (rawVisionReceipt is RawVisionReceipt)
            {
                RawVisionReceipt = rawVisionReceipt;
                return true;
            }
            return false;
        }
    }
}
