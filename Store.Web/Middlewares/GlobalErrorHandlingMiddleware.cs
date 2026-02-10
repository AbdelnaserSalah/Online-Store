using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Domain.Exceptions.Unauthorized;
using Store.Shared.ErrorModels;

namespace Store.Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // error happens in the next middleware when not found endpoint or other exception
                await _next.Invoke(context);
                // if  app not hit exception and StatusCode == 404 Handle because routing middleware could not find any endpoint for the request
                if (context.Response.StatusCode == 404)
                {
                    context.Response.ContentType = "application/json";
                    var response = new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = $"The requested {context.Request.Path} was not found."
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {
                // app  hit exception Set the response status code based on exception type
                context.Response.StatusCode = ex switch
                {
                   NotFoundExeception => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    UnauthorizedException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };


                context.Response.ContentType = "application/json";
                var response= new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                // this is function WriteAsJsonAsync convert object to json and write to response body
                await context.Response.WriteAsJsonAsync(response);
            }
        }

    }
}
