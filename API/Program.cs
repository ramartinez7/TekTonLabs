using API;
using Data;
using Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
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
    options.UseInMemoryDatabase("TekTonLabs");
});
services.AddScoped(typeof(IEntityStore<,>), typeof(EntityStore<,>));
services.AddScoped<IOrderItemsStore, OrderItemsStore>();
services.AddScoped<IEntityStore<Models.Order, int>, OrderStore>();
services.AddMediatR(typeof(MediatRReferencePoint).Assembly);
services.AddMemoryCache(options =>
{
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
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
