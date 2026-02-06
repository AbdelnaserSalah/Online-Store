using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Store.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation.Attributes
{
    public class CacheAttribute(int timesec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // get object from cache service by service provider
            var cacheservice = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var cacheKey = GetCacheKey(context.HttpContext.Request);

            var result = await cacheservice.GetAsync(cacheKey);

            // if found result in cache return it without execute endpoint action  
            if (! string.IsNullOrEmpty(result))
            {
                var response=new ContentResult()
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = response;
                return;
            }
            //execute endpoint action and get result to save it in cache for next request
            var actionContext = await next.Invoke();
            // actionContext contain the result of endpoint action its means all products
            if (actionContext.Result is OkObjectResult okObjectResult)
            {
                // save the result in cache for next request with cache key and expire time
                cacheservice.SetAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timesec));
            }
        }

        private string GetCacheKey(HttpRequest request)
        {
            var key= new StringBuilder();
            // path = /api/products    only not contian query string
            key.Append(request.Path);


            // we can use query string as part of cache key because it may change the result of the request
            // EX: Request = /api/products?page=2&pageSize=10  => cache key = /api/products|page-2|pageSize-10
            // Adding querys to path
            foreach (var item in request.Query)
            {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString();
        }
    }
}
