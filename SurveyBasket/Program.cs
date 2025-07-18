using SurveyBasket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// this will add all endpionts specified for identity like login,register,password endpionts and so on
/*//builder.Services.AddIdentityApiEndpoints<ApplicationUser>().
//    AddEntityFrameworkStores<AppDbContext>();
//app.MapIdentityApi<ApplicationUser>();*/

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
