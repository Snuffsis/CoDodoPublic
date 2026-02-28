using CoDodoApi;
using CoDodoApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;
services.AddSwagger();
services.AddSingleton(TimeProvider.System);
services.AddSingleton<ProcessInMemoryStore>();
services.AddScoped<ExcelImporter>();
services.AddConfiguredAuthentication();
services.AddAuthorization();

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapAllRoutes();
app.UseAuthentication();
app.UseAuthorization();

app.Run();