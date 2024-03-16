
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidFileResolutionException : Exception
    {
        public double RequiredPPI { get; private set; }
        public double ImagePPI { get; private set; }
        public InvalidFileResolutionException(double requiredPPI, double imagePPI)
           : base($"Invalid file resolution, PPI should be >= {requiredPPI} (Image PPI: {imagePPI})")
        {
            RequiredPPI = requiredPPI;
            ImagePPI = imagePPI;
        }
    }
}
