namespace HiperNFe.Models;

/// <summary>
/// Dados do emitente da NF-e.
/// </summary>
public class NFeEmitter
{
    public string CorporateName { get; set; } = string.Empty;
    public string TradeName { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string StateRegistration { get; set; } = string.Empty;
    public string MunicipalRegistration { get; set; } = string.Empty;
    public NFeAddress Address { get; set; } = new();
    public string TaxRegime { get; set; } = string.Empty;
}
