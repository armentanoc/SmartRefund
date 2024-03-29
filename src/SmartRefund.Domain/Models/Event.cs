﻿using SmartRefund.Domain.Enums;
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

        public override string ToString()
        {
           return $"Event: {Status} - {EventDate} - {Description}";
        }
    }

}
