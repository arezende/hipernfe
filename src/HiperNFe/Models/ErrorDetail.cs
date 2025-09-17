namespace HiperNFe.Models;

/// <summary>
/// Detalhes de erros retornados pela SEFAZ.
/// </summary>
public sealed record ErrorDetail(string Code, string Message);
