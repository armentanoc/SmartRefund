using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Models.Enums
{
    public enum InternalReceiptStatusEnum
    {
        Unprocessed = 1,
        Successful = 2,
        FailedOnce = 3,
        FailedMoreThanOnce = 4,
        Unsuccessful = 5,
        VisionAuthenticationFailed = 6
    }
}