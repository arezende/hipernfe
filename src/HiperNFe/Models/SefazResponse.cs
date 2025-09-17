using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HiperNFe.Models;

/// <summary>
/// Representa a resposta retornada pela SEFAZ.
/// </summary>
public sealed class SefazResponse
{
    public bool Success { get; init; }

    public string StatusCode { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public XElement? Payload { get; init; }

    public IReadOnlyCollection<ErrorDetail> Errors { get; init; } = Array.Empty<ErrorDetail>();

    public static SefazResponse FromException(Exception exception) => new()
    {
        Success = false,
        StatusCode = "EXC",
        Message = exception.Message,
        Errors = new[] { new ErrorDetail("EXC", exception.Message) }
    };
}
