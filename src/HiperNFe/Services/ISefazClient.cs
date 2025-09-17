using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Models;

namespace HiperNFe.Services;

/// <summary>
/// Cliente responsável por enviar requisições SOAP/REST à SEFAZ.
/// </summary>
public interface ISefazClient
{
    Task<string> SendAuthorizationAsync(string xml, CancellationToken cancellationToken = default);
    Task<string> QueryStatusAsync(string stateCode, CancellationToken cancellationToken = default);
    Task<string> QueryReceiptAsync(string receiptNumber, string stateCode, CancellationToken cancellationToken = default);
    Task<string> QueryProtocolAsync(string accessKey, string stateCode, CancellationToken cancellationToken = default);
    Task<string> CancelAsync(string accessKey, string justification, CancellationToken cancellationToken = default);
    Task<string> SubmitCorrectionAsync(string accessKey, string correctionText, CancellationToken cancellationToken = default);
    Task<string> InutilizeAsync(NFeInutilizationRequest request, CancellationToken cancellationToken = default);
    Task<string> DistributeAsync(NFeDistributionRequest request, CancellationToken cancellationToken = default);
    Task<string> ManifestAsync(NFeManifestationRequest request, CancellationToken cancellationToken = default);
    Task<string> DownloadXmlAsync(string accessKey, CancellationToken cancellationToken = default);
}
