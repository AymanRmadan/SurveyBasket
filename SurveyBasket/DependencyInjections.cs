using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SurveyBasket.Authantication;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;
using SurveyBasket.Settings;
using System.Reflection;
using System.Text;

namespace SurveyBasket
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //For Caching Types
            //services.AddResponseCaching();
            //services.AddOutputCache();
            //services.AddDistributedMemoryCache();
            services.AddHybridCache();




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
            services.AddScoped<IEmailSender, EmailService>();

            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteServics, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            // services.AddScoped<ICacheService, CacheService>();


            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddHttpContextAccessor();

            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));


            // API Versions Configurations
            #region API Versions Configurations
            services.AddApiVersioning(options =>
              {
                  options.DefaultApiVersion = new ApiVersion(1);
                  options.AssumeDefaultVersionWhenUnspecified = true;
                  options.ReportApiVersions = true;

                  options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
              })
             .AddApiExplorer(options =>
             {
                 options.GroupNameFormat = "'v'V";
                 options.SubstituteApiVersionInUrl = true;
             });
            #endregion


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

                // 🔐 Add JWT Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your JWT token (e.g., Bearer eyJhbGciOi...)."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
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
                 AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

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

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });




            return services;
        }
    }
}
