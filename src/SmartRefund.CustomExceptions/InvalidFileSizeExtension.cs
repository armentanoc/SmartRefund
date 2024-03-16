using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidFileSizeException : Exception
    {
        public long Size { get; private set; }
        public InvalidFileSizeException(long size)
           : base($"Invalid file size, more than 20 MB. (File size: {size})")
        {
            Size = size; 
        }
    }
}
