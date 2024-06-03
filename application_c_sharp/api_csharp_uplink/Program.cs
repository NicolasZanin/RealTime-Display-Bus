using api_csharp_uplink.Composant;
using api_csharp_uplink.Repository;
using api_csharp_uplink.Config;
using api_csharp_uplink.DB;

var builder = WebApplication.CreateBuilder(args);
string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IBusService, BusComposant>();
builder.Services.AddScoped<IInfluxDBBus, InfluxDbBus>();
builder.Services.AddEndpointsApiExplorer();

if (environment != "Test")
{
    Console.WriteLine("Environment: " + environment);
    builder.Services.AddSingleton(new GlobalInfluxDb());
    builder.Services.AddSwaggerGen();
}
else
{
    builder.Services.AddSingleton(new GlobalInfluxDb("i1K_ifwjv_yAMbRFISRo2gi9jyiFXcAopdj32fj0a98Pk1D7ujvqWNt-PTu5rcEjy5ZGVRG_BIRDyb1RMEJh3g=="));
}

var app = builder.Build();
app.UseExceptionHandler("/error");
app.UseMiddleware<CustomExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();