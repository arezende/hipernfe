using System;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Services;

public sealed class CancellationService : SefazServiceBase
{
    public CancellationService(
        SefazConfiguration configuration,
        CertificateManager certificateManager,
        XmlGenerator xmlGenerator,
        XmlSigner xmlSigner,
        SchemaValidator schemaValidator,
        SefazHttpClient sefazHttpClient,
        ILogger<CancellationService> logger)
        : base("cancelamento", configuration, certificateManager, xmlGenerator, xmlSigner, schemaValidator, sefazHttpClient, logger)
    {
    }

    protected override Uri ResolveEndpoint(SefazConfiguration configuration) => configuration.CancellationUrl;
}
