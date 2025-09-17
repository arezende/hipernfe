using System;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Services;

public sealed class InutilizationService : SefazServiceBase
{
    public InutilizationService(
        SefazConfiguration configuration,
        CertificateManager certificateManager,
        XmlGenerator xmlGenerator,
        XmlSigner xmlSigner,
        SchemaValidator schemaValidator,
        SefazHttpClient sefazHttpClient,
        ILogger<InutilizationService> logger)
        : base("inutilizacao", configuration, certificateManager, xmlGenerator, xmlSigner, schemaValidator, sefazHttpClient, logger)
    {
    }

    protected override Uri ResolveEndpoint(SefazConfiguration configuration) => configuration.InutilizationUrl;
}
