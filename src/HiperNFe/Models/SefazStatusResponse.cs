namespace HiperNFe.Models;

/// <summary>
/// Resposta do serviço de status da SEFAZ.
/// </summary>
public class SefazStatusResponse
{
    public string State { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string SefazRegion { get; set; } = string.Empty;
}
