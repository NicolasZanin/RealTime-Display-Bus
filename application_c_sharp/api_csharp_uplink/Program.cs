using System.Reflection;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Repository;
using api_csharp_uplink.DB;
using api_csharp_uplink.Interface;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ICardFinder, BusComposant>();
builder.Services.AddScoped<ICardRegistration, BusComposant>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPositionComposant, PositionComposant>();
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IStationRegister, StationComposant>();
builder.Services.AddScoped<IStationFinder, StationComposant>();
builder.Services.AddEndpointsApiExplorer();

if (environment != "Test")
{
    builder.Services.AddSingleton(new GlobalInfluxDb());
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        // Add XML comments if you have them
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
}
else
{
    builder.Services.AddSingleton(new GlobalInfluxDb("mNxnpUdxk7h6z8GOchqIL7AM8au7Zt3y9uXX_jz9OXhEdi0qnOkLc3ZjWqW5rSc-ASVLafSF0xk_-IIWxir78A=="));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();