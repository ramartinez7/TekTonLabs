using API;
using Data;
using Data.ExternalData;
using Mediator;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Headers;

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
services.AddScoped<IProductsDataFromExternalSource, ProductsDataFromExternalSource>();
services.AddMemoryCache(options =>
{
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
});
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddOptions<MockAPI>()
    .Bind(configuration.GetSection(MockAPI.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddHttpClient("MockAPI", client =>
{
    var apiConfig = configuration.GetSection(MockAPI.SectionName).Get<MockAPI>();
    client.BaseAddress = new Uri(apiConfig.BaseAddress);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromMilliseconds(apiConfig.TimeoutInMilliseconds);
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

public class MockAPI
{
    public const string SectionName = "MockAPI";
    [Required(AllowEmptyStrings = false)]
    public string BaseAddress { get; set; }
    public int TimeoutInMilliseconds { get; set; }
};