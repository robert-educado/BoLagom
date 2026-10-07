using BoLagom.Api.Dtos;
using BoLagom.Api.Models;
using BoLagom.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoLagom.Api.Endpoints;

public static class PropertyEndpoints
{
    // Extensionmetod
    public static void MapPropertyEndpoints(this WebApplication app)
    {
        // Endpoints

        // Be dependency injection-containern om en instans av PropertyService
        app.MapGet("/api/properties", (PropertyService propertyService) =>
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

            var properties = propertyService.GetProperties();

            var propertyDtos = properties.Select(x => new PropertyDto(x.Id, x.Name, x.Address));

            // JavaScript / TypeScript
            //const propertyDtos = properties.map(x => new PropertyDto(x.id, x.name, x.address))

            return Results.Ok(propertyDtos); // -> JSON
        });

        // Be dependency injection-containern om en instans av PropertyService
        app.MapGet("/api/properties/{id:int}", (int id, PropertyService propertyService) =>
        {
            var property = propertyService.GetPropertyById(id);

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
        app.MapPost("/api/properties", (
            CreatePropertyDto createPropertyDto,
            PropertyService propertyService) =>
        {
            // Validering
            // - Namn måste vara mellan 1-50 tecken
            // - Address måste vara mellan 1-100 tecken

            var errors = ValidateProperty(
                name: createPropertyDto.Name, 
                address: createPropertyDto.Address, 
                portCode: createPropertyDto.PortCode);

            // TODO: Lägg till validerings för portkod, en portkod kan vara 0000-9999

            // TIPS: Använd RegEx - Regular Expression för att säkerställa att värdet för portkod 
            // följer detta.

            if (errors.Count > 0)
            {
                // 400 Bad Request + ett standardiserat felmeddelande som kan innehålla
                // flera valideringsfel samtidigt.
                return Results.ValidationProblem(errors);
            }

            var property = propertyService.CreateProperty(
                name: createPropertyDto.Name,
                address: createPropertyDto.Address,
                portCode: createPropertyDto.PortCode);

            // AutoMapper
            PropertyDto propertyDto = new PropertyDto(
                Id: property.Id,
                Name: property.Name,
                Address: property.Address
            );

            // 201 Created
            return Results.Created("", propertyDto);
        });


        // Be dependency injection-containern om en instans av PropertyService
        app.MapDelete("/api/properties/{id:int}", (int id, PropertyService propertyService) =>
        {
            bool propertyDeleted = propertyService.DeleteProperty(id);

            if (!propertyDeleted)
            {
                // 404 Not Found
                return Results.NotFound();
            }

            // 204 No Content
            return Results.NoContent();
        });

        // Be dependency injection-containern om en instans av PropertyService
        
        
        // PUT https://localhost:8000/api/properties/1
        // ...
        //
        // {
        //   "name": "Tornet",
        //   "address": "Landskronavägen 1, Helsingborg",
        //   "portCode": "1234"
        // }

        app.MapPut("/api/properties/{id:int}", (
            int id, 
            UpdatePropertyDto updatePropertyDto, 
            PropertyService propertyService) =>
        {
            var errors = ValidateProperty(
                name: updatePropertyDto.Name,
                address: updatePropertyDto.Address,
                portCode: updatePropertyDto.PortCode);

            if (errors.Count > 0)
            {
                // 400 Bad Request + ett standardiserat felmeddelande som kan innehålla
                // flera valideringsfel samtidigt.
                return Results.ValidationProblem(errors);
            }

            var updatedProperty = propertyService.UpdateProperty(
                id, 
                updatePropertyDto.Name, 
                updatePropertyDto.Address, 
                updatePropertyDto.PortCode);

            if (updatedProperty is null)
            {
                // 404 Not Found
                return Results.NotFound();
            }

            // 204 No Content
            return Results.NoContent();
        });

    }

    static IDictionary<string, string[]> ValidateProperty(string name, string address, string portCode)
    {
        // List<T> och Dictionary<string, string>
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(name) ||
            name.Length > 50)
        {
            errors["name"] = ["Namn måste vara mellan 1 och 50 tecken"];
        }

        if (string.IsNullOrWhiteSpace(address) ||
            address.Length > 100)
        {
            errors["address"] = ["Adress måste vara mellan 1 och 100 tecken"];
        }

        return errors;
    }
}
