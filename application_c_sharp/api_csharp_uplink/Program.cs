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
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IScheduleService, ScheduleComposant>();
builder.Services.AddScoped<IInfluxDBSchedule, InfluxDBSchedule>();
builder.Services.AddScoped<ICardFinder, CardComposant>();
builder.Services.AddScoped<ICardRegistration, CardComposant>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPositionRegister, PositionComposant>();
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IStationRegister, StationComposant>();
builder.Services.AddScoped<IStationFinder, StationComposant>();
builder.Services.AddSingleton<IGlobalInfluxDb, GlobalInfluxDb>();
builder.Services.AddEndpointsApiExplorer();

string jsonToRead;

if (environment != "Test")
{
    jsonToRead = "appsettings.json";
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
}
else
    jsonToRead = "appsettings.Test.json";

builder.Configuration.AddJsonFile(jsonToRead, optional: false, reloadOnChange: true);
builder.Services.Configure<InfluxDbSettings>(builder.Configuration.GetSection("InfluxDB"));

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