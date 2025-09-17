namespace HiperNFe.Printing;

/// <summary>
/// Opções de personalização do DANFE.
/// </summary>
public sealed class DanfeOptions
{
    public string CompanyLogoPath { get; init; } = string.Empty;

    public bool IncludeAdditionalInfo { get; init; } = true;

    public bool Landscape { get; init; } = false;
}
