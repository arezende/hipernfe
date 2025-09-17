using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using HiperNFe.Models;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Cliente HTTP responsável por enviar XMLs para os endpoints da SEFAZ.
/// </summary>
public class SefazHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SefazHttpClient> _logger;

    public SefazHttpClient(HttpClient httpClient, ILogger<SefazHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public virtual async Task<SefazResponse> PostXmlAsync(Uri endpoint, XDocument document, CancellationToken cancellationToken)
    {
        var content = new StringContent(document.ToString(), Encoding.UTF8, "application/xml");
        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("SEFAZ retornou {StatusCode} para {Endpoint}", response.StatusCode, endpoint);
        return new SefazResponse
        {
            Success = response.IsSuccessStatusCode,
            StatusCode = ((int)response.StatusCode).ToString(),
            Message = response.ReasonPhrase ?? string.Empty,
            Payload = XDocument.Parse(string.IsNullOrWhiteSpace(payload) ? "<empty/>" : payload).Root
        };
    }
}
