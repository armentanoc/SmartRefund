using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Enums
{
    public enum EventSourceStatusEnum
    {
        EventSourceInitialized = 1,
        InternalReceiptCreated,
        VisionExecutorFailed,
        VisionExecutorUnsuccessful,
        VisionExecutorSuccessful,
        FileTranslationFailed,
        FileTranslated,
        PaymentAccepted,
        PaymentDenied,
        WaitingForAuthenticationSolution
    }
}
