
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using System.Text;

namespace SmartRefund.ViewModels.Responses
{
    public class InternalReceiptResponse
    {
        public string UniqueHash { get; private set; }
        public uint EmployeeId { get; private set; }
        public DateTime CreationDate { get; private set; }
        public InternalReceiptStatusEnum Status { get; private set; }
        public byte[] Image { get; private set; }
        public InternalReceiptResponse(InternalReceipt internalReceipt, bool isFrontEndpoint = false)
        {
            UniqueHash = internalReceipt.UniqueHash;
            EmployeeId = internalReceipt.EmployeeId;
            CreationDate = internalReceipt.CreationDate;
            Status = internalReceipt.Status;
            if(isFrontEndpoint)
            {
                Image = internalReceipt.Image;
            }
            else
            {
                if (internalReceipt.Image != null) { Image = Encoding.ASCII.GetBytes("Saved Image"); }
                else { Image = Encoding.ASCII.GetBytes("The file is missing"); };
            }
        }
    }
}
