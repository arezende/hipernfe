using System;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Services;

public sealed class StatusService : SefazServiceBase
{
    public StatusService(
        SefazConfiguration configuration,
        CertificateManager certificateManager,
        XmlGenerator xmlGenerator,
        XmlSigner xmlSigner,
        SchemaValidator schemaValidator,
        SefazHttpClient sefazHttpClient,
        ILogger<StatusService> logger)
        : base("status", configuration, certificateManager, xmlGenerator, xmlSigner, schemaValidator, sefazHttpClient, logger)
    {
    }

    protected override Uri ResolveEndpoint(SefazConfiguration configuration) => configuration.QueryUrl;
}
