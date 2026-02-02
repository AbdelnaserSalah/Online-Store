using Microsoft.AspNetCore.Mvc;
using Store.Domain.Contracts;
using Store.Presistence;
using Store.Services;
using Store.Shared.ErrorModels;
using Store.Web.Middlewares;
namespace Store.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add web-specific services here in the future if needed

            services.AddWebService();

            services.AddInfrastructureServices(configuration);

            services.AddApplicationServices(configuration);

            services.ConfiguraApiBehaviorOption();

            return services;
        }

        private static IServiceCollection ConfiguraApiBehaviorOption( this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>

            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Any())
                                                        .Select(M => new VaildationError
                                                        {
                                                            Field = M.Key,
                                                            Errors = M.Value.Errors.Select(E => E.ErrorMessage)
                                                        }).ToList();

                    var errorResponse = new VaildationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };

            }
                        );

            return services;
        }

        private static IServiceCollection AddWebService( this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }



        public static async Task<WebApplication> ConfiguraMiddleware(this WebApplication app)
        {
            #region Intialize DataBase
            await app.SeedData();
            #endregion

            app.UseGlobalErrorHandling();

            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }

        private static async Task<WebApplication> SeedData(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR to create an instance of DbInitializer
            await dbInitializer.InitializeAsync();
            return app;
        }
    }
}
