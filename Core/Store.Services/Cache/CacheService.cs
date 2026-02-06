using Store.Domain.Contracts;
using Store.Services.Abstractions.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Cache
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string key)
        {
            var result = await cacheRepository.GetAsync(key);
            return result;
        }
        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
           await cacheRepository.SetAsync(key,value,duration);
        }
    }
}
