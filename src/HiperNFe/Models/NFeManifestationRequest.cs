namespace HiperNFe.Models;

/// <summary>
/// Solicitação de manifestação do destinatário.
/// </summary>
public class NFeManifestationRequest
{
    public string AccessKey { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public NFeManifestationType Manifestation { get; set; }
    public string Justification { get; set; } = string.Empty;
    public SefazEnvironment Environment { get; set; } = SefazEnvironment.Homologation;
}
