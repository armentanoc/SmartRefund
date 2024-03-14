
namespace SmartRefund.ViewModels.Responses
{
    public class TranslatedReceiptResponse
    {
        public uint ReceiptId { get; set; }

        public uint EmployeeId { get; set; }
        public decimal Total { get; set; }
        public string Category { get; set; }    
        public string Status { get; set; }
        public string Description { get; set; }

        public TranslatedReceiptResponse()
        {
            // required by EF
        }
        public TranslatedReceiptResponse(uint receiptId, uint employeeId, decimal total, string category, string status, string description)
        {
            ReceiptId = receiptId;
            EmployeeId = employeeId;
            Total = total;
            Category = category;
            Status = status;
            Description = description;
        }
    }
}
