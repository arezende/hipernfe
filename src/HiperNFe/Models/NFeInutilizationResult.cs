using System;

namespace HiperNFe.Models;

/// <summary>
/// Resultado da inutilização de NF-e.
/// </summary>
public class NFeInutilizationResult
{
    public bool IsInutilized { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string ProtocolNumber { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Xml { get; set; } = string.Empty;
}
