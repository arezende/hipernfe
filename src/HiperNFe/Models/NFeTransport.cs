namespace HiperNFe.Models;

/// <summary>
/// Dados do transporte da NF-e.
/// </summary>
public class NFeTransport
{
    public string Mode { get; set; } = string.Empty;
    public string FreightResponsibility { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public string VehicleState { get; set; } = string.Empty;
    public string CarrierCnpj { get; set; } = string.Empty;
    public string CarrierName { get; set; } = string.Empty;
}
