using HiperNFe.Models;

namespace HiperNFe.Configuration;

/// <summary>
/// Configurações gerais para comunicação com a SEFAZ.
/// </summary>
public class NFeServiceConfig
{
    public string FederalUnit { get; set; } = "SP";
    public SefazEnvironment Environment { get; set; } = SefazEnvironment.Homologation;
    public string ServiceUrl { get; set; } = string.Empty;
    public NFeCertificateConfig Certificate { get; set; } = new();
    public int TimeoutSeconds { get; set; } = 60;
    public bool EnableXmlValidation { get; set; } = true;
    public string EmitterCnpj { get; set; } = string.Empty;
}
