using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Configuration;
using HiperNFe.Models;

namespace HiperNFe.Services;

/// <summary>
/// Cliente SOAP básico que monta envelopes para comunicação com a SEFAZ.
/// </summary>
public class SoapSefazClient : ISefazClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly NFeServiceConfig _config;

    public SoapSefazClient(HttpClient httpClient, NFeServiceConfig config)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
    }

    public Task<string> SendAuthorizationAsync(string xml, CancellationToken cancellationToken = default)
        => SendSoapAsync("nfeAutorizacaoLote", xml, cancellationToken);

    public Task<string> QueryStatusAsync(string stateCode, CancellationToken cancellationToken = default)
        => SendSoapAsync("nfeStatusServicoNF", $"<consStatServ versao='4.00'><tpAmb>{(int)_config.Environment}</tpAmb><cUF>{stateCode}</cUF><xServ>STATUS</xServ></consStatServ>", cancellationToken);

    public Task<string> QueryReceiptAsync(string receiptNumber, string stateCode, CancellationToken cancellationToken = default)
        => SendSoapAsync("nfeRetAutorizacaoLote", $"<consReciNFe versao='4.00'><tpAmb>{(int)_config.Environment}</tpAmb><nRec>{receiptNumber}</nRec></consReciNFe>", cancellationToken);

    public Task<string> QueryProtocolAsync(string accessKey, string stateCode, CancellationToken cancellationToken = default)
        => SendSoapAsync("nfeConsultaProtocolo", $"<consSitNFe versao='4.00'><tpAmb>{(int)_config.Environment}</tpAmb><xServ>CONSULTAR</xServ><chNFe>{accessKey}</chNFe></consSitNFe>", cancellationToken);

    public Task<string> CancelAsync(string accessKey, string justification, CancellationToken cancellationToken = default)
    {
        EnsureEmitterCnpj();
        return SendSoapAsync("RecepcaoEvento", BuildEventEnvelope(accessKey, justification, "110111", _config.EmitterCnpj, _config.Environment), cancellationToken);
    }

    public Task<string> SubmitCorrectionAsync(string accessKey, string correctionText, CancellationToken cancellationToken = default)
    {
        EnsureEmitterCnpj();
        return SendSoapAsync("RecepcaoEvento", BuildEventEnvelope(accessKey, correctionText, "110110", _config.EmitterCnpj, _config.Environment), cancellationToken);
    }

    public Task<string> InutilizeAsync(NFeInutilizationRequest request, CancellationToken cancellationToken = default)
        => SendSoapAsync("nfeInutilizacaoNF", $"<inutNFe versao='4.00'><tpAmb>{(int)request.Environment}</tpAmb><xServ>INUTILIZAR</xServ><cUF>{request.State}</cUF><ano>{request.Year}</ano><CNPJ>{request.Cnpj}</CNPJ><mod>55</mod><serie>{request.Series}</serie><nNFIni>{request.StartNumber}</nNFIni><nNFFin>{request.EndNumber}</nNFFin><xJust>{request.Justification}</xJust></inutNFe>", cancellationToken);

    public Task<string> DistributeAsync(NFeDistributionRequest request, CancellationToken cancellationToken = default)
        => SendSoapAsync("NFeDistribuicaoDFe", $"<distDFeInt versao='1.01'><tpAmb>{(int)request.Environment}</tpAmb><cUFAutor>{request.State}</cUFAutor><CNPJ>{request.Cnpj}</CNPJ><distNSU><ultNSU>{request.LastNSU}</ultNSU></distNSU></distDFeInt>", cancellationToken);

    public Task<string> ManifestAsync(NFeManifestationRequest request, CancellationToken cancellationToken = default)
        => SendSoapAsync("RecepcaoEvento", BuildEventEnvelope(request.AccessKey, request.Justification, ((int)request.Manifestation).ToString(), request.Cnpj, request.Environment), cancellationToken);

    public Task<string> DownloadXmlAsync(string accessKey, CancellationToken cancellationToken = default)
        => SendSoapAsync("NFeDistribuicaoDFe", $"<distDFeInt versao='1.01'><tpAmb>{(int)_config.Environment}</tpAmb><distChNFe><chNFe>{accessKey}</chNFe><tpDown>AN</tpDown></distChNFe></distDFeInt>", cancellationToken);

    private async Task<string> SendSoapAsync(string action, string xmlBody, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_config.ServiceUrl))
        {
            throw new InvalidOperationException("A URL do serviço da SEFAZ não foi configurada.");
        }

        var envelope = $"<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Body>{xmlBody}</soap:Body></soap:Envelope>";
        using var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
        content.Headers.Add("SOAPAction", action);

        using var response = await _httpClient.PostAsync(_config.ServiceUrl, content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    private static string BuildEventEnvelope(string accessKey, string description, string eventCode, string cnpj, SefazEnvironment environment)
    {
        var escapedDescription = System.Security.SecurityElement.Escape(description);
        var detEvento = eventCode switch
        {
            "110111" => $"<detEvento versao='1.00'><descEvento>Cancelamento</descEvento><xJust>{escapedDescription}</xJust></detEvento>",
            "110110" => $"<detEvento versao='1.00'><descEvento>Carta de Correcao</descEvento><xCorrecao>{escapedDescription}</xCorrecao><xCondUso>A Carta de Correcao e disciplinada pelo Ajuste SINIEF 01/07</xCondUso></detEvento>",
            "210200" => "<detEvento versao='1.00'><descEvento>Confirmacao da Operacao</descEvento></detEvento>",
            "210210" => "<detEvento versao='1.00'><descEvento>Ciência da Operacao</descEvento></detEvento>",
            "210220" => $"<detEvento versao='1.00'><descEvento>Desconhecimento da Operacao</descEvento><xJust>{escapedDescription}</xJust></detEvento>",
            "210240" => "<detEvento versao='1.00'><descEvento>Operacao nao Realizada</descEvento></detEvento>",
            _ => $"<detEvento versao='1.00'><descEvento>{eventCode}</descEvento><xCorrecao>{escapedDescription}</xCorrecao></detEvento>"
        };

        return $"<envEvento versao='1.00'><evento versao='1.00'><infEvento Id='ID{eventCode}{accessKey}01'><cOrgao>91</cOrgao><tpAmb>{(int)environment}</tpAmb><CNPJ>{cnpj}</CNPJ><chNFe>{accessKey}</chNFe><dhEvento>{DateTime.UtcNow:yyyy-MM-ddTHH:mm:sszzz}</dhEvento><tpEvento>{eventCode}</tpEvento><nSeqEvento>1</nSeqEvento><verEvento>1.00</verEvento>{detEvento}</infEvento></evento></envEvento>";
    }

    private void EnsureEmitterCnpj()
    {
        if (string.IsNullOrWhiteSpace(_config.EmitterCnpj))
        {
            throw new InvalidOperationException("Informe o CNPJ do emitente em NFeServiceConfig.EmitterCnpj para enviar eventos.");
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
