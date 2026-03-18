using Application;
using CoDodoApi;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using CoDodoApi.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
  loggerConfig.ReadFrom.Configuration(context.Configuration);
  loggerConfig.Enrich.FromLogContext();
  loggerConfig.Enrich.WithProperty("Application", "CoDodoApi");
});

builder.Services
  .AddApplication()
  .AddPresentation()
  .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<ExcelService>();
builder.Services.AddConfiguredAuthentication();
builder.Services.AddAuthorization();

WebApplication app = builder.Build();
var api = app.MapGroup("/api");
app.MapEndpoints(api);

app.ApplyMigrations();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("health", new HealthCheckOptions() 
{
  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.Run();