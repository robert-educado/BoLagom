// File-scoped namespace
namespace BoLagom.Api.Models;

public class Property
{
    // 3 stycken properties (auto-implemented properties)
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    // Superhemlig portkod
    public string PortCode { get; set; }
}

/*
CREATE TABLE Properties (
  Id,
  Name,
  Address,
  PortCode
)
 */
