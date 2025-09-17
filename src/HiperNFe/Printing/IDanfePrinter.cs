using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Models;

namespace HiperNFe.Printing;

/// <summary>
/// Responsável por gerar a DANFE a partir de um documento NF-e.
/// </summary>
public interface IDanfePrinter
{
    Task<Stream> PrintAsync(NFeDocument document, DanfePrintOptions? options, CancellationToken cancellationToken = default);
}
