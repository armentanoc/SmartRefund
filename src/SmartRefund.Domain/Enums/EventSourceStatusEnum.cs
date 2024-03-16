using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Enums
{
    public enum EventSourceStatusEnum
    {
        InternalReceiptCreated = 1,
        VisionExecutorFailed = 2,
        VisionExecutorUnsuccessful = 3,
        VisionExecutorSuccessful = 4,
        FileTranslationFailed = 5,
        FileTranslated = 6,
        PaymentAccepted = 7,
        PaymentDenied = 8,
    }
}
