namespace HiperNFe.Models;

/// <summary>
/// Informações de pagamento da NF-e.
/// </summary>
public class NFePayment
{
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CardBrand { get; set; } = string.Empty;
    public string CardAuthorizationCode { get; set; } = string.Empty;
    public int Installments { get; set; }
    public string PaymentIndicator { get; set; } = string.Empty;
}
