using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.ViewModels.Responses
{
    public class ErrorResponse
    {
        public ErrorDetail Error { get; set; }
    }

    public class ErrorDetail
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}

