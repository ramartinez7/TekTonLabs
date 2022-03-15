using API;
using Data;
using Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
services.AddScoped(typeof(IEntityStore<>), typeof(EntityStore<>));
services.AddScoped<IOrderItemsStore, OrderItemsStore>();
services.AddMediatR(typeof(MediatRReferencePoint).Assembly);

builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        Log.Information($"Elapsed time: {sw.ElapsedMilliseconds} ms");
    }
    catch (Exception e)
    {
        Log.Error(e, "An error has ocurred");
        throw;
    }
});

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
