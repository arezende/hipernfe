using System;

namespace HiperNFe.Models;

/// <summary>
/// Resultado da carta de correção eletrônica.
/// </summary>
public class NFeCorrectionResult
{
    public bool IsRegistered { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string ProtocolNumber { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Xml { get; set; } = string.Empty;
}
