using AutoMapper;
using Store.Domain.Contracts;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Baskets;
using Store.Services.Abstractions.Cache;
using Store.Services.Abstractions.Products;
using Store.Services.Baskets;
using Store.Services.Cache;
using Store.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
    public class ServiceManager(IUnitofWork unitofWork,IMapper mapper,IBasketRepository basketRepository, ICacheRepository cacheRepository) : IServiceManager
    {
        public IProductService ProductService { get; }=new ProductService(unitofWork,mapper);
        public IBasketService BasketService { get; } = new BasketService(basketRepository, mapper);

        public ICacheService CacheService { get; }=new CacheService(cacheRepository);
    }
}
