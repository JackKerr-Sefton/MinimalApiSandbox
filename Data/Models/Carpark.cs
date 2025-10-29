namespace MinimalApiSandbox.Data.Models;

public class Carpark
{
    public int Id { get; set; }

    public string Name { get; set; } = null!; // Learning: Nullability handling

    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public string AddressLineOne { get; set; } = null!;
    public string? AddressLineTwo { get; set; }
    public string? AddressLineThree { get; set; }
    public string? AddressLineFour { get; set; }
    public string Postcode { get; set; } = null!;
}
