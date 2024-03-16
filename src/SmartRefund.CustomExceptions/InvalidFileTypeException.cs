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
    public class InvalidFileTypeException : Exception
    {
        public string Extension { get; private set; }
        public InvalidFileTypeException(string extension)
           : base($"Invalid file extension. (Extension: {extension})")
        {
            Extension = extension;
        }
    }
}
