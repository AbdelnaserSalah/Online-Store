using StackExchange.Redis;
using Store.Domain.Contracts;
using Store.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Presistence.Repostories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        // get  object represent database from redis connection
        // Bec. connection multiplexer manage connection to all redis server
        private readonly IDatabase database = connection.GetDatabase();

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
           var radivalue = await database.StringGetAsync(basketId);

            if(radivalue.IsNullOrEmpty) return null;
            var basket = JsonSerializer.Deserialize<CustomerBasket>(radivalue);

            if (basket is null) return null;

            return basket;
        }

        public async Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket, TimeSpan duration)
        {
            var redisvalue = JsonSerializer.Serialize(basket);

           var flag =  await database.StringSetAsync(basket.Id, redisvalue, duration);
            if (!flag) return null;

            return await GetBasketAsync(basket.Id);
        }



        public async Task<bool> DeleteBasketAsync(string basketId)
        {
          return await database.KeyDeleteAsync(basketId);
        }

       
    }
}
