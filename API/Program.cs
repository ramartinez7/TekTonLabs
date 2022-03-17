using API;
using Data;
using Mediator;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

// Add services to the container.
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<DatabaseContext>(options =>
{
    options.UseInMemoryDatabase("TekTonLabs Inc.");
});
services.AddScoped(typeof(IEntityStore<,>), typeof(EntityStore<,>));
services.AddScoped<IOrderItemsStore, OrderItemsStore>();
services.AddScoped<IEntityStore<Models.Order, int>, OrderStore>();
services.AddMediatR(typeof(MediatRReferencePoint).Assembly);
services.AddMemoryCache(options =>
{
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
});
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddOptions<RetoolAPI>()
    .Bind(configuration.GetSection(RetoolAPI.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddHttpClient("RetoolAPI", client =>
{
    var apiConfig = configuration.GetSection(RetoolAPI.SectionName).Get<RetoolAPI>();
    client.BaseAddress = new Uri(apiConfig.BaseAddress);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Logging.AddSerilog();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var cache = app.Services.GetRequiredService<IMemoryCache>();
    cache.Set("CompanyName", "TekTon Labs Inc.");
    cache.Set("CompanyAddress", "800 W El Camino Real Suite 180, Mountain View, CA 94040, United States");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting service");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public class RetoolAPI
{
    public const string SectionName = "RetoolAPI";
    [Required(AllowEmptyStrings = false)]
    public string BaseAddress { get; set; }
};