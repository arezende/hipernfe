using System;
using System.Xml.Linq;

namespace HiperNFe.Models;

/// <summary>
/// Representa uma requisição para a SEFAZ.
/// </summary>
public sealed class SefazRequest
{
    public string ServiceName { get; init; } = string.Empty;

    public XElement Payload { get; init; } = new("empty");

    public string[] SchemaPaths { get; init; } = Array.Empty<string>();
}
