using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Services;

/// <summary>
/// Classe base para implementação dos serviços com lógica comum de geração, assinatura e envio.
/// </summary>
public abstract class SefazServiceBase : ISefazService
{
    private readonly SefazConfiguration _configuration;
    private readonly CertificateManager _certificateManager;
    private readonly XmlGenerator _xmlGenerator;
    private readonly XmlSigner _xmlSigner;
    private readonly SchemaValidator _schemaValidator;
    private readonly SefazHttpClient _sefazHttpClient;
    private readonly ILogger _logger;

    protected SefazServiceBase(
        string name,
        SefazConfiguration configuration,
        CertificateManager certificateManager,
        XmlGenerator xmlGenerator,
        XmlSigner xmlSigner,
        SchemaValidator schemaValidator,
        SefazHttpClient sefazHttpClient,
        ILogger logger)
    {
        Name = name;
        _configuration = configuration;
        _certificateManager = certificateManager;
        _xmlGenerator = xmlGenerator;
        _xmlSigner = xmlSigner;
        _schemaValidator = schemaValidator;
        _sefazHttpClient = sefazHttpClient;
        _logger = logger;
    }

    public string Name { get; }

    protected abstract Uri ResolveEndpoint(SefazConfiguration configuration);

    protected virtual string[] GetSchemas(SefazRequest request) => request.SchemaPaths;

    protected virtual XDocument CreateDocument(SefazRequest request)
    {
        return _xmlGenerator.GenerateDocument(request.Payload);
    }

    public async Task<SefazResponse> SendAsync(SefazRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var document = CreateDocument(request);
            var xml = new XmlDocument();
            xml.LoadXml(document.ToString());

            var certificate = _certificateManager.GetCertificate(_configuration.Certificate);
            xml = _xmlSigner.Sign(xml, certificate);

            var xmlBytes = System.Text.Encoding.UTF8.GetBytes(xml.OuterXml);
            using var stream = new System.IO.MemoryStream(xmlBytes);
            var errors = _schemaValidator.Validate(stream, GetSchemas(request));
            if (errors.Count > 0)
            {
                _logger.LogWarning("Validação XSD falhou para {Service}", Name);
                return new SefazResponse
                {
                    Success = false,
                    StatusCode = "XSD",
                    Message = "Falha de validação XSD",
                    Errors = errors
                };
            }

            var endpoint = ResolveEndpoint(_configuration);
            _logger.LogInformation("Enviando XML para {Service} no endpoint {Endpoint}", Name, endpoint);

            var signedDocument = XDocument.Parse(xml.OuterXml);
            return await _sefazHttpClient.PostXmlAsync(endpoint, signedDocument, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar requisição para {Service}", Name);
            return SefazResponse.FromException(ex);
        }
    }
}
