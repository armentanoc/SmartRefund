namespace SmartRefund.ViewModels.Requests
{
    public class RawVisionReceiptRequest
    {
        public string IsReceipt { get; private set; }
        public string Category { get; private set; }
        public string Total { get; private set; }
        public string Description { get; private set; }

        public RawVisionReceiptRequest(string isReceipt, string category, string total, string description)
        {
            IsReceipt = isReceipt;
            Category = category;
            Total = total;
            Description = description;
        }
    }
}