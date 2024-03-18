using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CacheService> _logger;
        public CacheService(IMemoryCache cache, IConfiguration configuration, ILogger<CacheService> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<IEnumerable<T>> GetCachedDataAsync<T>(string key)
        {
            if (!_cache.TryGetValue<IEnumerable<T>>(key, out var cachedData))
            {
                // Se os dados não estiverem em cache, retornamos nulo
                return Enumerable.Empty<T>();
            }

            return cachedData;
        }

        /* public async Task SetCachedDataAsync<T>(string key, IEnumerable<T> data)
         {
                 var cacheEntryOptions = new MemoryCacheEntryOptions()
                     .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                 _cache.Set(key, data, cacheEntryOptions);
         }*/

        public async Task SetCachedDataAsync<T>(string key, IEnumerable<T> data)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromSeconds(Convert.ToDouble(_configuration.GetSection("CacheSettings")["SlidingExpirationSeconds"])))
           .SetAbsoluteExpiration(TimeSpan.FromSeconds(Convert.ToDouble(_configuration.GetSection("CacheSettings")["AbsoluteExpirationSeconds"])));

            // Configurar o tamanho máximo da memória
            var maxMemorySizeMB = Convert.ToInt32(_configuration.GetSection("CacheSettings")["MaxMemorySizeMB"]);
            cacheEntryOptions.Size = maxMemorySizeMB * 1024 * 1024;

            _cache.Set(key, data, cacheEntryOptions);
        }
    }
}


