using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using System.Reflection;

namespace SurveyBasket
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {

            services.AddFluentValidationCofig();
            services.AddSwaggerCinfig();
            services.AddMapsterCinfig();


            services.AddScoped<IPollService, PollService>();
            return services;
        }



        private static IServiceCollection AddMapsterCinfig(this IServiceCollection services)
        {

            // Add Mapster
            // To read data from mapping Configurations
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }
        private static IServiceCollection AddSwaggerCinfig(this IServiceCollection services)
        {
            // Enable Swagger/OpenAPI
            services.AddEndpointsApiExplorer(); // Required for Swagger UI
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddFluentValidationCofig(this IServiceCollection services)
        {

            //Register For Fluent Validation
            //services.AddScoped<IValidator<CreatePollRequest>, CreatePollRequestValidator>();
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
