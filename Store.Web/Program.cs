



using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Presistence;
using Store.Presistence.Data.Contexts;
using Store.Services;
using Store.Services.Abstractions;
using Store.Services.Mapping.Products;
using Store.Shared.ErrorModels;
using Store.Web.Extensions;
using Store.Web.Middlewares;

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

             builder.Services.AddAllServices(builder.Configuration);



            var app = builder.Build();

            // Configure the HTTP request pipeline.

            await app.ConfiguraMiddleware();


            app.Run();
        }
    }
}
