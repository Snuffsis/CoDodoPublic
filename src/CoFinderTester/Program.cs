WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapPost("WonOpportunities", (WonOpportunity _) => _);

app.Run();

class WonOpportunity(string Name, string Uri);