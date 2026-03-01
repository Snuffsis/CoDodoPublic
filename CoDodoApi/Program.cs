using CoDodoApi;
using CoDodoApi.Extensions;
using CoDodoApi.Services;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;
services.AddEndpoints(Assembly.GetExecutingAssembly());
services.AddDatabase(builder.Configuration);
services.AddSwagger();
services.AddSingleton(TimeProvider.System);
services.AddScoped<ExcelService>();
services.AddConfiguredAuthentication();
services.AddAuthorization();

WebApplication app = builder.Build();
app.MapEndpoints();
app.ApplyMigrations();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();