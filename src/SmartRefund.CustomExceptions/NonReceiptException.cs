
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class NonResolutionReadableException : Exception
    {
        public uint Id { get; private set; }
        public NonResolutionReadableException(uint id)
           : base($"Resolução ilegível (InternalReceiptId: {id})")
        {
            Id = id;
        }
    }
}
