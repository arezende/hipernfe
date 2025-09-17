namespace HiperNFe.Models;

/// <summary>
/// Solicitação do serviço de distribuição de NF-e.
/// </summary>
public class NFeDistributionRequest
{
    public string Cnpj { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string LastNSU { get; set; } = "000000000000000";
    public SefazEnvironment Environment { get; set; } = SefazEnvironment.Homologation;
}
