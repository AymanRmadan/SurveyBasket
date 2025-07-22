using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SurveyBasket.Authantication;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;
using System.Reflection;
using System.Text;

namespace SurveyBasket
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            /*  services.AddCors(options =>
              options.AddPolicy("MyPolicy", builder =>
                   builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                          .WithOrigins(allowedOrigins!)
                          ));*/

            services.AddCors(options =>
                 options.AddDefaultPolicy(builder =>
                     builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .WithOrigins(allowedOrigins!)
                      )
                 );

            services.AddFluentValidationConfig();
            services.AddSwaggerConfig();
            services.AddMapsterConfig();
            services.AddDataBaseCofig(configuration);
            services.AddAuthConfig(configuration);


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteServics, VoteService>();
            services.AddScoped<IResultService, ResultService>();


            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
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

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>().
        AddEntityFrameworkStores<AppDbContext>();
            services.AddSingleton<IJwtProvider, JwtProvider>();

            //  services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddOptions<JwtOptions>()
                  .BindConfiguration("Jwt")
                  .ValidateDataAnnotations()
                  .ValidateOnStart();
            var jwtSetting = configuration.GetSection("Jwt").Get<JwtOptions>();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSetting?.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSetting?.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting?.Key!)),

                    };
                }
                );



            return services;
        }
    }
}
