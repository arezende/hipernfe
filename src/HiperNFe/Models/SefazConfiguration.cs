using System;
using HiperNFe.Infrastructure;

namespace HiperNFe.Models;

/// <summary>
/// Configurações de acesso aos serviços da SEFAZ.
/// </summary>
public sealed class SefazConfiguration
{
    public EnvironmentType Environment { get; init; } = EnvironmentType.Homologation;

    public Uri AuthorizationUrl { get; init; } = new("https://homolog.sefaz/api/autorizacao");

    public Uri QueryUrl { get; init; } = new("https://homolog.sefaz/api/consulta");

    public Uri CancellationUrl { get; init; } = new("https://homolog.sefaz/api/cancelamento");

    public Uri LetterCorrectionUrl { get; init; } = new("https://homolog.sefaz/api/cartacorrecao");

    public Uri InutilizationUrl { get; init; } = new("https://homolog.sefaz/api/inutilizacao");

    public Uri ManifestationUrl { get; init; } = new("https://homolog.sefaz/api/manifestacao");

    public CertificateConfiguration Certificate { get; init; } = new();

    public TimeSpan RequestTimeout { get; init; } = TimeSpan.FromSeconds(30);
}
