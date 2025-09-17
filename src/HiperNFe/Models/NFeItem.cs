namespace HiperNFe.Models;

/// <summary>
/// Item comercializado na NF-e.
/// </summary>
public class NFeItem
{
    public int LineNumber { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ncm { get; set; } = string.Empty;
    public string Cfop { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public decimal Discount { get; set; }
    public decimal NetTotal => TotalPrice - Discount;
}
