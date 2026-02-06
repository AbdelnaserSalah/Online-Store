using StackExchange.Redis;
using Store.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Presistence.Repostories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private readonly IDatabase database = connection.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
           var result= await  database.StringGetAsync(key);
              return result;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
           await database.StringSetAsync(key, JsonSerializer.Serialize(value),duration);
        }
    }
}
