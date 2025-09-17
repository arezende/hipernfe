using System.Collections.Generic;

namespace HiperNFe.Models;

/// <summary>
/// Resposta do serviço de distribuição de NF-e.
/// </summary>
public class NFeDistributionResponse
{
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string LastNSU { get; set; } = string.Empty;
    public List<string> DocumentsXml { get; set; } = new();
}
