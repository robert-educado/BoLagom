using BoLagom.Api.Dtos;
using BoLagom.Api.Models;

namespace BoLagom.Api.Endpoints;

public static class PropertyEndpoints
{
    // Tillfällig minnesbaserad "databas"
    static List<Property> properties = [
        new Property { Id = 1, Name = "Gammelstugan", Address = "Kullagatan 1, 252 00 Helsingborg", PortCode = "1234" },
        new Property { Id = 2, Name = "Tornet", Address = "Stora Gatan 1, 722 15 Västerås", PortCode = "1111" },
        new Property { Id = 3, Name = "Sjöängen", Address = "Södra storgatan 5, 300 00 Malmö", PortCode = "2222" },
    ];

    // Extensionmetod
    public static void MapPropertyEndpoints(this WebApplication app)
    {
        // Endpoints

        app.MapGet("/api/properties", () =>
        {
            // 1 - Skapa ny lista som kan innehåll objekt av typ PropertyDto
            //List<PropertyDto> propertyDtos = [];

            //// 2 - Iterera över listan av fastigheter  - för varje fastighet, skapa en PropertyDto och 
            //// lägg till denna i vår nya lista, som håller PropertyDtos (propertyDtos)
            //foreach (Property property in properties)
            //{
            //    // 2.1 - Skapa en PropertyDto och mappa in värden får nuvarande property i denna
            //    PropertyDto propertyDto = new PropertyDto
            //    {
            //        Id = property.Id,
            //        Name = property.Name,
            //        Address = property.Address
            //    };

            //    // 2.2 - Lägg till vår nya PropertyDto till listan av PropertyDtos
            //    propertyDtos.Add(propertyDto);
            // }

            var propertyDtos = properties.Select(x => new PropertyDto(x.Id, x.Name, x.Address));

            // JavaScript / TypeScript
            //const propertyDtos = properties.map(x => new PropertyDto(x.id, x.name, x.address))

            return Results.Ok(propertyDtos); // -> JSON
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

            // Vad returnerar den här endpointen för tillfället? 
            PropertyDto propertyDto = new PropertyDto(
                Id: property.Id,
                Name: property.Name,
                Address: property.Address
            );

            // 200 OK + JSON
            return Results.Ok(propertyDto);
        });

        // POST https://localhost:8000/api/properties
        // Accept: application/json
        // Content-Type: application/json
        //
        // {
        //    "name": "Solgläntan", 
        //    "address": "Storgatan 12, 111 20 Stockholm",
        //    "portCode": "1234"
        // }

        // Model Binding = ramverket mappar automatiskt JSON till en DTO/object
        // Deserialisering = omvandla t.ex. JSON till ett objekt
        app.MapPost("/api/properties", (CreatePropertyDto createPropertyDto) =>
        {
            // Validering
            // - Namn måste vara mellan 1-50 tecken
            // - Address måste vara mellan 1-100 tecken

            // List<T> och Dictionary<string, string>
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(createPropertyDto.Name) ||
                createPropertyDto.Name.Length > 50)
            {
                errors["name"] = ["Namn måste vara mellan 1 och 50 tecken"];
            }

            if (string.IsNullOrWhiteSpace(createPropertyDto.Address) ||
                createPropertyDto.Address.Length > 100)
            {
                errors["address"] = ["Adress måste vara mellan 1 och 100 tecken"];
            }

            // TODO: Lägg till validerings för portkod, en portkod kan vara 0000-9999
            
            // TIPS: Använd RegEx - Regular Expression för att säkerställa att värdet för portkod 
            // följer detta.

            if (errors.Count > 0)
            {
                // 400 Bad Request + ett standardiserat felmeddelande som kan innehålla
                // flera valideringsfel samtidigt.
                return Results.ValidationProblem(errors);
            }

            Property property = new()
            {
                Id = properties.Max(x => x.Id) + 1,
                Name = createPropertyDto.Name,
                Address = createPropertyDto.Address,
                PortCode = createPropertyDto.PortCode
            };

            properties.Add(property);

            PropertyDto propertyDto = new PropertyDto(
                Id: property.Id,
                Name: property.Name,
                Address: property.Address
            );

            // 201 Created
            return Results.Created("", propertyDto);
        });
    }
}
