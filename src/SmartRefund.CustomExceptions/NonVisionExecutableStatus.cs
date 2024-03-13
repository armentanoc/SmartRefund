
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class NonVisionExecutableStatus : Exception
    {
        public uint InternalReceiptId { get; private set; }
        public string InternalReceiptStatus { get; private set; }
        public NonVisionExecutableStatus(uint internalReceiptId, string internalReceiptStatus)
           : base($"InternalReceipt couldn't be processed by GPT Vision because it have a incompatible status. (Id: {internalReceiptId} | Status: {internalReceiptStatus})")
        {
            InternalReceiptId = internalReceiptId;
            InternalReceiptStatus = internalReceiptStatus;
        }
    }
}
