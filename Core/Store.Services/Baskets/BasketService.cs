using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Baskets;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Baskets;
using Store.Shared.Dtos.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Baskets
{
    public class BasketService(IBasketRepository basketRepository ,IMapper mapper ) : IBasketService
    {
        public async Task<BasketDto?> GetBasketAsync(string id)
        {
          var basket= await basketRepository.GetBasketAsync(id);
            if (basket is null) throw new BasketNotFoundException(id);
            var dto= mapper.Map<BasketDto>(basket);
            return dto;
        }
        public async Task<BasketDto?> CreateUpdataBasketAsync(BasketDto dto, TimeSpan duration)
        {
            var basket = mapper.Map<CustomerBasket>(dto);
            var result =  await basketRepository.CreateUpdateBasketAsync(basket, duration);

            if (result is null) throw new CreateUpdataBasketBadRequestException() ;
             
            return  mapper.Map<BasketDto>(result);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
           var flag= await basketRepository.DeleteBasketAsync(id);
            if (!flag) throw new DeleteBasketBadRequestException();
            return flag;
        }

       
    }
}
