using BoLagom.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Tillfällig minnesbaserad "databas"
List<Property> properties = [
    new Property { Id = 1, Name = "Gammelstugan", Address = "Kullagatan 1, 252 00 Helsingborg" },
    new Property { Id = 2, Name = "Tornet", Address = "Stora Gatan 1, 722 15 Västerås" },
    new Property { Id = 3, Name = "Sjöängen", Address = "Södra storgatan 5, 300 00 Malmö" },
];

// Endpoints

app.MapGet("/api/properties", () =>
{
    return Results.Ok(properties);
});

app.MapGet("/api/properties/{id:int}", (int id) =>
{
    // FirstOrDefault() returnera antingen fastigheten om den finns, eller null
    Property? property = properties.FirstOrDefault(x => x.Id == id);

    if (property is null)
    {
        // 404 Not Found
        return Results.NotFound();
    }

    // 200 OK + JSON
    return Results.Ok(property);
});

app.Run();
