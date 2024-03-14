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
    public class GetReceiptsException : Exception
    {
        public GetReceiptsException(): base("An error occurred when searching for receipts!")
        {
        }


    }
}
