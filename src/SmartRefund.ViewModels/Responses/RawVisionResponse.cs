
using SmartRefund.Domain.Models;

namespace SmartRefund.ViewModels.Responses
{
    public class RawVisionResponse
    {
        public string IsReceipt { get; set; }
        public string Total { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public RawVisionResponse()
        {
            
        }
        public RawVisionResponse(RawVisionReceipt rawVisionReceipt)
        {
            if(rawVisionReceipt is RawVisionReceipt)
            {
                IsReceipt = rawVisionReceipt.IsReceipt;
                Total = rawVisionReceipt.Total;
                Category = rawVisionReceipt.Category;
                Description = rawVisionReceipt.Description;
            }
        }
    }
}
