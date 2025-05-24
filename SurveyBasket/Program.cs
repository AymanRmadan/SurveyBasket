using SurveyBasket.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Enable Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger UI
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IPollService, PollService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();

    // Always show Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
