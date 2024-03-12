using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.ViewModels.Responses
{
    public class TranslatedReceiptResponse
    {
        public decimal Total { get; set; }
        public string Category { get; set; }    
        public string Status { get; set; }
        public string Description { get; set; }

        public TranslatedReceiptResponse()
        {

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
