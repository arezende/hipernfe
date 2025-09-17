using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Configuration;
using HiperNFe.Email;
using HiperNFe.Models;
using HiperNFe.Printing;
using HiperNFe.Serialization;

namespace HiperNFe.Services;

/// <summary>
/// Implementação de alto nível do serviço de NF-e.
/// </summary>
public class HiperNFeService : INFeService, IDisposable
{
    private readonly NFeServiceConfig _config;
    private readonly INFeSerializer _serializer;
    private readonly IDanfePrinter _printer;
    private readonly IEmailService _emailService;
    private readonly ISefazClient _sefazClient;
    private readonly bool _disposeHttpClient;

    public HiperNFeService(NFeServiceConfig config, INFeSerializer serializer, IDanfePrinter printer, IEmailService emailService, HttpClient? httpClient = null)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _printer = printer ?? throw new ArgumentNullException(nameof(printer));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        if (httpClient == null)
        {
            httpClient = new HttpClient();
            _disposeHttpClient = true;
        }

        _sefazClient = new SoapSefazClient(httpClient, _config);
    }

    public HiperNFeService(NFeServiceConfig config, INFeSerializer serializer, IDanfePrinter printer, IEmailService emailService, ISefazClient sefazClient)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _printer = printer ?? throw new ArgumentNullException(nameof(printer));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _sefazClient = sefazClient ?? throw new ArgumentNullException(nameof(sefazClient));
    }

    public async Task<SefazStatusResponse> GetStatusAsync(string stateCode, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.QueryStatusAsync(stateCode, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseStatus(response);
    }

    public async Task<NFeAuthorizationResult> AuthorizeAsync(NFeDocument document, CancellationToken cancellationToken = default)
    {
        var xml = await _serializer.SerializeAsync(document).ConfigureAwait(false);
        var response = await _sefazClient.SendAuthorizationAsync(xml, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseAuthorization(response);
    }

    public async Task<NFeAuthorizationResult> QueryReceiptAsync(string receiptNumber, string stateCode, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.QueryReceiptAsync(receiptNumber, stateCode, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseAuthorization(response);
    }

    public async Task<NFeAuthorizationResult> QueryProtocolAsync(string accessKey, string stateCode, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.QueryProtocolAsync(accessKey, stateCode, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseAuthorization(response);
    }

    public async Task<NFeCancellationResult> CancelAsync(string accessKey, string justification, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.CancelAsync(accessKey, justification, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseCancellation(response);
    }

    public async Task<NFeCorrectionResult> SubmitCorrectionAsync(string accessKey, string correctionText, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.SubmitCorrectionAsync(accessKey, correctionText, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseCorrection(response);
    }

    public async Task<NFeInutilizationResult> InutilizeAsync(NFeInutilizationRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.InutilizeAsync(request, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseInutilization(response);
    }

    public async Task<NFeDistributionResponse> DistributeAsync(NFeDistributionRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.DistributeAsync(request, cancellationToken).ConfigureAwait(false);
        // Implementação simplificada: devolve o XML recebido para processamento posterior.
        return new NFeDistributionResponse
        {
            StatusCode = "138",
            StatusMessage = "Documentos obtidos com sucesso",
            LastNSU = request.LastNSU,
            DocumentsXml = { response }
        };
    }

    public async Task<NFeManifestationResult> ManifestAsync(NFeManifestationRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _sefazClient.ManifestAsync(request, cancellationToken).ConfigureAwait(false);
        return SefazResponseParser.ParseManifestation(response);
    }

    public async Task<NFeDocument> DownloadXmlAsync(string accessKey, CancellationToken cancellationToken = default)
    {
        var xml = await _sefazClient.DownloadXmlAsync(accessKey, cancellationToken).ConfigureAwait(false);
        return await _serializer.DeserializeAsync(xml).ConfigureAwait(false);
    }

    public Task<Stream> PrintDanfeAsync(NFeDocument document, DanfePrintOptions? options, CancellationToken cancellationToken = default)
        => _printer.PrintAsync(document, options, cancellationToken);

    public async Task SendEmailAsync(NFeDocument document, EmailMessage email, CancellationToken cancellationToken = default)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (email == null)
        {
            throw new ArgumentNullException(nameof(email));
        }

        var danfeStream = await PrintDanfeAsync(document, new DanfePrintOptions(), cancellationToken).ConfigureAwait(false);
        using var memory = new MemoryStream();
        await danfeStream.CopyToAsync(memory, cancellationToken).ConfigureAwait(false);
        email.Attachments.Add(new EmailAttachment
        {
            FileName = $"DANFE-{document.AccessKey}.pdf",
            Content = memory.ToArray(),
            ContentType = "application/pdf"
        });

        var xml = await _serializer.SerializeAsync(document).ConfigureAwait(false);
        email.Attachments.Add(new EmailAttachment
        {
            FileName = $"{document.AccessKey}.xml",
            Content = System.Text.Encoding.UTF8.GetBytes(xml),
            ContentType = "application/xml"
        });

        await _emailService.SendAsync(email, cancellationToken).ConfigureAwait(false);
    }

    public void Dispose()
    {
        if (_disposeHttpClient && _sefazClient is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
