using System.Collections.Generic;

namespace HiperNFe.Models;

/// <summary>
/// Resultado do processamento de um lote de NF-e.
/// </summary>
public sealed class BatchResult
{
    public string BatchId { get; init; } = string.Empty;

    public IReadOnlyCollection<SefazResponse> Responses { get; init; } = new List<SefazResponse>();
}
