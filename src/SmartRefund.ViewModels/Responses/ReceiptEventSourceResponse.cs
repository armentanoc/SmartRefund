
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;

namespace SmartRefund.ViewModels.Responses
{
    public class ReceiptEventSourceResponse
    {
        public InternalReceiptResponse InternalReceipt { get; private set; }
        public RawVisionResponse? RawVision { get; private set; }
        public TranslatedReceiptResponse? TranslatedVision { get; private set; }
        public string UniqueHash { get; private set; }
        public string CurrentStatus { get; private set; }
        public List<Event> Events { get; private set; }

        public ReceiptEventSourceResponse(ReceiptEventSource receiptEventSource, bool isFrontEndpoint)
        {
            UniqueHash = receiptEventSource.UniqueHash;
            CurrentStatus = receiptEventSource.CurrentStatus.ToString();
            Events = receiptEventSource.Events;
            if(isFrontEndpoint)
            {
                InternalReceipt = new InternalReceiptResponse(receiptEventSource.InternalReceipt, isFrontEndpoint);
                RawVision = new RawVisionResponse(receiptEventSource.RawVisionReceipt);
                TranslatedVision = new TranslatedReceiptResponse(receiptEventSource.TranslatedVisionReceipt);
            }
        }
    }
}

