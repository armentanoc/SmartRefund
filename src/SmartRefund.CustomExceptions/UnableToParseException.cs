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
        public string Value { get; private set; }
        public UnableToParseException(string value)
           : base($"Invalid value. Unable to parse: '{value}'")
        {
            Value = value;
        }
    }
}
