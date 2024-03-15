using SmartRefund.Infra.Interfaces;
using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRefund.ViewModels.Responses;
using SmartRefund.Application.Interfaces;

namespace SmartRefund.Application.Services
{
    public class EventSourceService : IEventSourceService
    {
        private IEventSourceRepository _repository;

        public EventSourceService(IEventSourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> PrintEventSourcing(uint idEventSourcing)
        {
            var eventSourcing = await _repository.GetById(idEventSourcing);
            Console.WriteLine($"ID: {eventSourcing.Id} \n");
            foreach(IEvent evnt in eventSourcing.events)
            {
                Console.WriteLine(evnt.ToString());
            }
            return true;
        }
    }
}
