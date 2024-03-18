using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.ViewModels.Responses
{
    public class EventResponse
    {
        public String Status { get; private set; }
        public DateTime EventDate { get; private set; }
        public string? Description { get; private set; }
        public string? HashCode { get; private set; }

        public EventResponse(Event evnt)
        {
            HashCode = evnt.HashCode;
            Status = evnt.Status.ToString();
            EventDate = evnt.EventDate;
            Description = evnt.Description;
        }

    }

}
