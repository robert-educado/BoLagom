using BoLagom.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapPropertyEndpoints();

app.Run();
