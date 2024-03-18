
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class VisionConfigurationException : Exception
    {
        private string? Property { get; init; }
        public VisionConfigurationException(string propertyName)
            : base($"Configuration of property {propertyName} can't be null, whitespace or invalid.")
        {
            Property = propertyName;
        }
    }
}
