using LabRegistry.Application;
using LabRegistry.Infrastructure.Database.Context;
using LabRegistry.Server.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Configuration.AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var configuration = builder.Configuration;

#region Database

services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(
        configuration.GetConnectionString("Postgres"),
        b => b.MigrationsAssembly("LabRegistry.Infrastructure")));

#endregion

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

services.AddHealthChecks();
services.AddControllers();
services.AddSwaggerGen();

services.AddExceptionHandler<ExceptionHandler>();
services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(); 
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
