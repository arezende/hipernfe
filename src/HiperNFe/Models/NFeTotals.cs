namespace HiperNFe.Models;

/// <summary>
/// Totais da NF-e.
/// </summary>
public class NFeTotals
{
    public decimal TotalProducts { get; set; }
    public decimal TotalServices { get; set; }
    public decimal TotalInvoice { get; set; }
    public decimal TotalTributes { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalFreight { get; set; }
    public decimal TotalInsurance { get; set; }
    public decimal TotalOtherExpenses { get; set; }
}
