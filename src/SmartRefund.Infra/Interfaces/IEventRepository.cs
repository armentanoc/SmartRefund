using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Infra.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
    }
}
