
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class NonReceiptException : Exception
    {
        public uint Id {  get; private set; }
        public NonReceiptException(uint id)
           : base($"Comprovante Inválido (InternalReceiptId: {id})")
        {
            Id = id;
        }
    }
}
