namespace HiperNFe.Configuration;

/// <summary>
/// Configuração do certificado digital.
/// </summary>
public class NFeCertificateConfig
{
    public string CertificatePath { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = string.Empty;
    public byte[]? CertificateRawData { get; set; }
    public string? CertificateThumbprint { get; set; }
}
