
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Responses
{
    [ExcludeFromCodeCoverage]
    public class TranslatedReceiptResponse
    {
        public decimal Total { get; set; }
        public string Category { get; set; }    
        public string Status { get; set; }
        public string Description { get; set; }

        public TranslatedReceiptResponse()
        {
            // required by EF
        }
        public TranslatedReceiptResponse(decimal total, string category, string status, string description)
        {
            Total = total;
            Category = category;
            Status = status;
            Description = description;
        }
    }
}
