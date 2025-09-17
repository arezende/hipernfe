namespace HiperNFe.Printing;

/// <summary>
/// Opções de impressão da DANFE.
/// </summary>
public class DanfePrintOptions
{
    public bool Portrait { get; set; } = true;
    public bool ShowLogo { get; set; } = true;
    public bool ShowProtocol { get; set; } = true;
    public string? CustomFooter { get; set; }
}
