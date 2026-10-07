using BoLagom.Api.Dtos;
using BoLagom.Api.Models;

namespace BoLagom.Api.Services;

public class PropertyService
{
    // Tillfällig minnesbaserad "databas"
    private static List<Property> properties = [
        new Property { Id = 1, Name = "Gammelstugan", Address = "Kullagatan 1, 252 00 Helsingborg", PortCode = "1234" },
        new Property { Id = 2, Name = "Tornet", Address = "Stora Gatan 1, 722 15 Västerås", PortCode = "1111" },
        new Property { Id = 3, Name = "Sjöängen", Address = "Södra storgatan 5, 300 00 Malmö", PortCode = "2222" },
    ];

    public List<Property> GetProperties()
    {
        return properties;
    }

    public Property? GetPropertyById(int id)
    {
        // Leta efter fastighet med angivet id
        Property? property = properties.FirstOrDefault(x => x.Id == id);

        return property;
    }

    public Property CreateProperty(string address, string portCode, string name)
    {
        // 1 - Skapa en instans (object) av Property - fyll på med värden från parametrarna
        Property property = new()
        {
            // Gå igenom listan av properties och välja ut det ID som har högst värde och lägg sen till
            // 1 på detta och då får vi nästa id
            Id = properties.Max(x => x.Id) + 1,
            Name = name,
            Address = address,
            PortCode = portCode
        };

        // Lägg till property till listan av properties (alltså vår interna "fake"-databas)
        properties.Add(property);

        // Returnera det nyskapade objektet
        return property;
    }

    public bool DeleteProperty(int id)
    {
        // FirstOrDefault() returnerar antingen fastigheten som matchar ID eller null
        var property = properties.FirstOrDefault(x => x.Id == id);

        if (property is null) return false;

        properties.Remove(property);

        return true;
    }

    public Property? UpdateProperty(int id, string name, string address, string portCode)
    {
        var property = properties.FirstOrDefault(x => x.Id == id);

        // Uppdatera fastighetens egenskaper om den finns
        if (property is not null)
        {
            property.Name = name;
            property.Address = address;
            property.PortCode = portCode;
        }

        return property;
    }
}
