using Store.Domain.Exceptions.NotFound;
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
                await _next.Invoke(context);

                if(context.Response.StatusCode == 404)
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
                context.Response.StatusCode = ex switch
                {
                   NotFoundExeception => StatusCodes.Status404NotFound,
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
