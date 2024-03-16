using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Interfaces
{
    public interface ICacheService
    {
        Task<IEnumerable<T>> GetCachedDataAsync<T>(string key);
        Task SetCachedDataAsync<T>(string key, IEnumerable<T> data);
    }
}
