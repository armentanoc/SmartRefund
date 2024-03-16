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
    public class Event : BaseEntity
    {
        public EventSourceStatusEnum Status { get; private set; }
        public DateTime EventDate { get; private set; }
        public string? Description { get; private set; }
        public string? HashCode { get; private set; }

        public Event() { }

        public Event(string eventSourceHash, EventSourceStatusEnum status, DateTime eventDate, string? description)
        {
            HashCode = eventSourceHash;
            Status = status;
            EventDate = eventDate;
            Description = description;
        }
    }

    
    //public record InternalReceiptCreated(uint internalReceiptId, DateTime creation, InternalReceiptStatusEnum status, string description = "Internal Receipt created with success");
    //public record VisionExecutorFailed(DateTime eventdate, InternalReceiptStatusEnum status, string description = "Vision Executor process failed, trying again...");
    //public record VisionExecutorUnsuccessful(DateTime eventdate, InternalReceiptStatusEnum status, string description = "Vision Executor process failed. A person in charge will process this receipt");
    //public record VisionExecutorSuccessful(DateTime eventdate, InternalReceiptStatusEnum status, string description = "The file passed by the Vision Executor and a Raw Vision Receipt was created");
    //public record FileFailedTranslation(DateTime eventdate, string status,string description = "The translation process failed, trying again...");
    //public record FileTranslated(DateTime eventdate, string status, string description = "The translation process succed. Ready to be analized.");
    //public record PaymentAccepted(DateTime eventdate, TranslatedVisionReceiptStatusEnum status, string description = "Receipt accepted and paid.");
    //public record PaymentDenied(DateTime eventdate, TranslatedVisionReceiptStatusEnum status, string description = "Receipt denied.");


}
