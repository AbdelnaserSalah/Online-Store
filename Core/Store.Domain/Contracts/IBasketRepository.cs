using Store.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket,TimeSpan duration);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
