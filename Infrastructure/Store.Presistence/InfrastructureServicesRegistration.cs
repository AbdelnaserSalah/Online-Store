using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Store.Domain.Contracts;
using Store.Presistence.Data.Contexts;
using Store.Presistence.Identity.Context;
using Store.Presistence.Repostories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presistence
{
    public  static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });


            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitofWork, Unitofwork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            // life time should be singleton for redis connection bec. object not remove per application
            services.AddSingleton<IConnectionMultiplexer>((ServiceProvider)=>
                  ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"))
                
                );

            return services;
        }
    }
}
