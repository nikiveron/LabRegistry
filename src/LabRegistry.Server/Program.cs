using LabRegistry.Application;
using LabRegistry.Infrastructure.Database.Context;
using LabRegistry.Infrastructure.Database.Repositories;
using LabRegistry.Server.Middleware;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using FluentValidation;

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

services.AddScoped<IInspectionObjectsRepository, InspectionObjectsRepository>();

#endregion

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));

services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<ValidationResultFactory>();
});
services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

services.AddHealthChecks();
services.AddControllers();
services.AddSwaggerGen();

services.AddExceptionHandler<ExceptionHandler>(); 
services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(); 

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                                  .CreateLogger("Startup");

try
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    logger.LogInformation("Миграция применена к базе данных");
}
catch (Exception ex)
{
    logger.LogError(ex, "Не удалось применить миграцию к базе данных");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
