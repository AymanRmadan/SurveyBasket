using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SurveyBasket;
using SurveyBasket.Services.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// this will add all endpionts specified for identity like login,register,password endpionts and so on
/*//builder.Services.AddIdentityApiEndpoints<ApplicationUser>().
//    AddEntityFrameworkStores<AppDbContext>();
//app.MapIdentityApi<ApplicationUser>();*/

builder.Services.AddDependencies(builder.Configuration);

// Add Serilog or Logger To save errors specify app
builder.Host.UseSerilog((context, configuration) =>
{
    //To read from appSetting
    configuration.ReadFrom.Configuration(context.Configuration);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSitting:Username"),
            Pass = app.Configuration.GetValue<string>("HangfireSitting:Password")
        }
    ],
    DashboardTitle = "Survey Basket Dashboard"
});
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

RecurringJob.AddOrUpdate("SendNewPollsNotification", () => notificationService.SendNewPollsNotification(null), Cron.Daily);


app.UseCors();

app.UseAuthorization();

//For Caching
//app.UseResponseCaching();
//app.UseOutputCache();

app.MapControllers();
app.UseExceptionHandler();

// Rate Limit
app.UseRateLimiter();

//Health Check
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
