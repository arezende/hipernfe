namespace HiperNFe.Models;

/// <summary>
/// Dados necessários para inutilização de numeração da NF-e.
/// </summary>
public class NFeInutilizationRequest
{
    public string Cnpj { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public int Year { get; set; }
    public string State { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public int StartNumber { get; set; }
    public int EndNumber { get; set; }
    public SefazEnvironment Environment { get; set; } = SefazEnvironment.Homologation;
}
