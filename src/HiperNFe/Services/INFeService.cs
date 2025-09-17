using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Email;
using HiperNFe.Models;
using HiperNFe.Printing;

namespace HiperNFe.Services;

/// <summary>
/// Interface principal para consumo dos serviços da SEFAZ.
/// </summary>
public interface INFeService
{
    Task<SefazStatusResponse> GetStatusAsync(string stateCode, CancellationToken cancellationToken = default);
    Task<NFeAuthorizationResult> AuthorizeAsync(NFeDocument document, CancellationToken cancellationToken = default);
    Task<NFeAuthorizationResult> QueryReceiptAsync(string receiptNumber, string stateCode, CancellationToken cancellationToken = default);
    Task<NFeAuthorizationResult> QueryProtocolAsync(string accessKey, string stateCode, CancellationToken cancellationToken = default);
    Task<NFeCancellationResult> CancelAsync(string accessKey, string justification, CancellationToken cancellationToken = default);
    Task<NFeCorrectionResult> SubmitCorrectionAsync(string accessKey, string correctionText, CancellationToken cancellationToken = default);
    Task<NFeInutilizationResult> InutilizeAsync(NFeInutilizationRequest request, CancellationToken cancellationToken = default);
    Task<NFeDistributionResponse> DistributeAsync(NFeDistributionRequest request, CancellationToken cancellationToken = default);
    Task<NFeManifestationResult> ManifestAsync(NFeManifestationRequest request, CancellationToken cancellationToken = default);
    Task<NFeDocument> DownloadXmlAsync(string accessKey, CancellationToken cancellationToken = default);
    Task<Stream> PrintDanfeAsync(NFeDocument document, DanfePrintOptions? options, CancellationToken cancellationToken = default);
    Task SendEmailAsync(NFeDocument document, EmailMessage email, CancellationToken cancellationToken = default);
}
