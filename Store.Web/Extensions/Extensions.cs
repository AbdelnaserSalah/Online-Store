using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Domain.Contracts;
using Store.Domain.Entities.Identity;
using Store.Presistence;
using Store.Presistence.Identity.Context;
using Store.Services;
using Store.Shared;
using Store.Shared.ErrorModels;
using Store.Web.Middlewares;
using System.Text;
namespace Store.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add web-specific services here in the future if needed

            services.AddWebService();
            services.AddIdentityService();

            services.AddInfrastructureServices(configuration);

            services.AddApplicationServices(configuration);

            services.ConfiguraApiBehaviorOption();

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            services.AddAuthenticationService(configuration);

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


        private static IServiceCollection AddAuthenticationService(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                // dectect the type of AuthenticateScheme we are using in our application
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(options =>
            {
                // detect properties of the token and validate it
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.issuer,
                    ValidAudience = jwtOptions.audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                };
            });

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


        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
             {
                 options.User.RequireUniqueEmail = true;
             }).AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<IdentityStoreDbContext>();
                
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


            app.UseAuthentication();
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
            await dbInitializer.InitializeIdentityAsync();
            return app;
        }
    }
}
