using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Baskets
{
    public class CustomerBasket
    {
        // id is string bec. key and value store in memory cache as string
        public string Id { get; set; }
        public IEnumerable<BasketItem> Items { get; set; } 
    }
}
