
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Responses
{
    [ExcludeFromCodeCoverage]
    public class InternalReceiptResponse
    {
        public uint EmployeeId { get; private set; }
        public DateTime CreationDate { get; private set; }
        public InternalReceiptStatusEnum Status { get; private set; }
        public string Image { get; private set; }


        public InternalReceiptResponse(InternalReceipt internalReceipt)
        {
            EmployeeId = internalReceipt.EmployeeId;
            CreationDate = internalReceipt.CreationDate;
            Status = internalReceipt.Status;
            if (internalReceipt.Image != null) { Image = "Saved Image"; }
                else { Image = "The file is missing"; };
        }
    }
}
