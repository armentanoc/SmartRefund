
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ReceiptAlreadyTranslatedException : Exception
    {
        public uint ReceiptId { get; private set; }
        public ReceiptAlreadyTranslatedException(uint receiptId)
           : base($"RawVisionReceipt is already translated. (Id {receiptId})")
        {
            ReceiptId = receiptId;
        }
    }
}
