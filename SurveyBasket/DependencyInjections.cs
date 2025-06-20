using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SurveyBasket.Persistence;
using System.Reflection;

namespace SurveyBasket
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationConfig();
            services.AddSwaggerConfig();
            services.AddMapsterConfig();
            services.AddDataBaseCofig(configuration);

            services.AddScoped<IPollService, PollService>();
            return services;
        }

        private static IServiceCollection AddDataBaseCofig(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Connction String Config
            var connectionString = configuration.GetConnectionString("DefualtConnection");
            services.AddDbContext<AppDbContext>(option =>
            option.UseSqlServer(connectionString));
            return services;
        }
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(config));
            return services;
        }

        private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SurveyBasket API",
                    Version = "v1"
                });

                // Optional: Add custom filters here
                // options.OperationFilter<YourCustomFilter>();
            });
            return services;
        }

        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
