
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class VisionConfigurationException : Exception
    {
        private string? Property { get; init; }
        public VisionConfigurationException(string propertyName)
            : base($"Configuration of prompt {propertyName} cannot be null or whitespace")
        {
            Property = propertyName;
        }

    }
}
