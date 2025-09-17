namespace HiperNFe.Models;

/// <summary>
/// Informações do destinatário da NF-e.
/// </summary>
public class NFeRecipient
{
    public string CorporateName { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string StateRegistration { get; set; } = string.Empty;
    public NFeAddress Address { get; set; } = new();
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
