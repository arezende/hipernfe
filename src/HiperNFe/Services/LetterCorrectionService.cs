using System;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Services;

public sealed class LetterCorrectionService : SefazServiceBase
{
    public LetterCorrectionService(
        SefazConfiguration configuration,
        CertificateManager certificateManager,
        XmlGenerator xmlGenerator,
        XmlSigner xmlSigner,
        SchemaValidator schemaValidator,
        SefazHttpClient sefazHttpClient,
        ILogger<LetterCorrectionService> logger)
        : base("carta-correcao", configuration, certificateManager, xmlGenerator, xmlSigner, schemaValidator, sefazHttpClient, logger)
    {
    }

    protected override Uri ResolveEndpoint(SefazConfiguration configuration) => configuration.LetterCorrectionUrl;
}
