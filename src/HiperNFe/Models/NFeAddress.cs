namespace HiperNFe.Models;

/// <summary>
/// Endereço da NF-e.
/// </summary>
public class NFeAddress
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string CityCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = "1058"; // Brasil
    public string Country { get; set; } = "Brasil";
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
