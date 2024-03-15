using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Models
{
    public interface IEvent { };

    public record InternalReceiptCreated(uint internalReceiptId, DateTime creation, InternalReceiptStatusEnum status, string description = "Internal Receipt created with success") : IEvent;
    public record VisionExecutorFailed(DateTime eventdate, InternalReceiptStatusEnum status, string description = "Vision Executor process failed, trying again...") : IEvent;
    public record VisionExecutorUnsuccessful(DateTime eventdate, InternalReceiptStatusEnum status, string description = "Vision Executor process failed. A person in charge will process this receipt") : IEvent;
    public record VisionExecutorSuccessful(DateTime eventdate, InternalReceiptStatusEnum status, string description = "The file passed by the Vision Executor and a Raw Vision Receipt was created") : IEvent;
    public record FileFailedTranslation(DateTime eventdate, string status,string description = "The translation process failed, trying again...") : IEvent;
    public record FileTranslated(DateTime eventdate, string status, string description = "The translation process succed. Ready to be analized.") : IEvent;
    public record PaymentAccepted(DateTime eventdate, TranslatedVisionReceiptStatusEnum status, string description = "Receipt accepted and paid.") : IEvent;
    public record PaymentDenied(DateTime eventdate, TranslatedVisionReceiptStatusEnum status, string description = "Receipt denied.") : IEvent;


}
