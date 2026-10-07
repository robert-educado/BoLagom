using BoLagom.Api.Endpoints;
using BoLagom.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registrera PropertyService i dependency injection-containern som en singleton, dvs.
// det kommer enbart finnas en enda instans/objekt av den här servicen under hela applikationens livslängd
// dvs. tills dess att applikationen stängs ner.
builder.Services.AddSingleton<PropertyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapPropertyEndpoints();

app.Run();
