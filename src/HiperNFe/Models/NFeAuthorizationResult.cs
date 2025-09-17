using System;

namespace HiperNFe.Models;

/// <summary>
/// Resultado da autorização de NF-e.
/// </summary>
public class NFeAuthorizationResult
{
    public bool IsAuthorized { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string ProtocolNumber { get; set; } = string.Empty;
    public DateTime ReceptionDate { get; set; }
    public string Xml { get; set; } = string.Empty;
}
