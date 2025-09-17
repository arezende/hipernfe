using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Models;

namespace HiperNFe.Services;

/// <summary>
/// Contrato base para todos os serviços de comunicação com a SEFAZ.
/// </summary>
public interface ISefazService
{
    string Name { get; }

    bool CanHandle(string serviceName) => Name.Equals(serviceName, System.StringComparison.OrdinalIgnoreCase);

    Task<SefazResponse> SendAsync(SefazRequest request, CancellationToken cancellationToken = default);

    SefazResponse Send(SefazRequest request) => SendAsync(request).GetAwaiter().GetResult();
}
