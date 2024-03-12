
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class UnnableToTranslateException : Exception
    {
        public string SpecificEntity { get; private set; }
        public string Field { get; private set; }
        public UnnableToTranslateException(string field, string specificEntity)
           : base($"{field} couldn't be translated. ({specificEntity})")
        {
            SpecificEntity = specificEntity;
            Field = field;
        }
    }
}
