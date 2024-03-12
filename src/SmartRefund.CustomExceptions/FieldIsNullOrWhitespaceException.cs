
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class FieldIsNullOrWhitespaceException : Exception
    {
        public string SpecificEntity { get; private set; }
        public string Field { get; private set; }
        public FieldIsNullOrWhitespaceException(string field, string specificEntity)
           : base($"{field} cannot be null or empty. We need to send it again to GPT ({specificEntity})")
        {
            Field = field;
            SpecificEntity = specificEntity;
        }
    }
}
