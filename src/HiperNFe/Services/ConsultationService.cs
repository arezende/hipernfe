using System;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Services;

public sealed class ConsultationService : SefazServiceBase
{
    public ConsultationService(
        SefazConfiguration configuration,
        CertificateManager certificateManager,
        XmlGenerator xmlGenerator,
        XmlSigner xmlSigner,
        SchemaValidator schemaValidator,
        SefazHttpClient sefazHttpClient,
        ILogger<ConsultationService> logger)
        : base("consulta", configuration, certificateManager, xmlGenerator, xmlSigner, schemaValidator, sefazHttpClient, logger)
    {
    }

    protected override Uri ResolveEndpoint(SefazConfiguration configuration) => configuration.QueryUrl;
}
