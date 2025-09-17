namespace HiperNFe.Infrastructure;

/// <summary>
/// Configurações de certificado digital utilizado na assinatura da NF-e.
/// </summary>
public sealed class CertificateConfiguration
{
    public string? SubjectName { get; init; }

    public string? Path { get; init; }

    public string? Password { get; init; }
}
