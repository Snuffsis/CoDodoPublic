using CoDodoApi;
using CoDodoApi.Extensions;
using CoDodoApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;
services.AddDatabase(builder.Configuration);
services.AddSwagger();
services.AddSingleton(TimeProvider.System);
services.AddScoped<ExcelImporter>();
services.AddConfiguredAuthentication();
services.AddAuthorization();

WebApplication app = builder.Build();
app.ApplyMigrations();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapAllRoutes();
app.UseAuthentication();
app.UseAuthorization();

app.Run();