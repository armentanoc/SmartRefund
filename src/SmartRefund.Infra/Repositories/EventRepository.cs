using SmartRefund.Domain.Models;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Infra.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
        {
        }

    }
}
