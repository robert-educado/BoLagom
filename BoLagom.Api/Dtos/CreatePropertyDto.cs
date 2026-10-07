namespace BoLagom.Api.Dtos;

record CreatePropertyDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string PortCode { get; set; }
}
