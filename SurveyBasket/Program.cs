using Serilog;
using SurveyBasket;

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
{    //To read from appSetting
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

app.UseCors();

app.UseAuthorization();

//For Caching
//app.UseResponseCaching();
//app.UseOutputCache();

app.MapControllers();
app.UseExceptionHandler();

app.Run();
