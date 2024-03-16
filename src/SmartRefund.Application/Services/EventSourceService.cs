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

        public async Task<bool> PrintEventSourcing(uint idEventSource)
        {
            var eventSource = await _repository.GetById(idEventSource);
            Console.WriteLine($"HASHCODE: {eventSource.UniqueHash} \n");

            foreach (Event evnt in eventSource.Events)
            {
                Console.WriteLine($"{evnt.Status} | {evnt.EventDate}");
                Console.WriteLine(evnt.Description);
                Console.WriteLine();
            }
            return true;
        }
    }
}
