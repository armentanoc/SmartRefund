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
    public class UnableToParseException : Exception
    {
        public string WrongStatus { get; private set; }
        public UnableToParseException( string wrongStatus)
           : base($"Invalid status. Unable to parse: {wrongStatus}")
        {
            WrongStatus = wrongStatus;
        }
    }
}
