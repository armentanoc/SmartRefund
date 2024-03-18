
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Responses
{
    [ExcludeFromCodeCoverage]
    public class InternalReceiptResponse
    {
        public string UniqueHash { get; private set; }
        public uint EmployeeId { get; private set; }
        public DateTime CreationDate { get; private set; }
        public InternalReceiptStatusEnum Status { get; private set; }
        public string Image { get; private set; }
        public InternalReceiptResponse(InternalReceipt internalReceipt, bool isFrontEndpoint = false)
        {
            UniqueHash = internalReceipt.UniqueHash;
            EmployeeId = internalReceipt.EmployeeId;
            CreationDate = internalReceipt.CreationDate;
            Status = internalReceipt.Status;
            if(isFrontEndpoint)
            {
                Image = Convert.ToBase64String(internalReceipt.Image);
            }
            else
            {
                if (internalReceipt.Image != null) { Image = "Saved Image"; }
                else { Image = "The file is missing"; };
            }
        }
    }
}
