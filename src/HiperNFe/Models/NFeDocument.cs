using System;
using System.Collections.Generic;

namespace HiperNFe.Models;

/// <summary>
/// Representa uma Nota Fiscal eletrônica pronta para emissão.
/// </summary>
public class NFeDocument
{
    public string AccessKey { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public NFeEmitter Emitter { get; set; } = new();
    public NFeRecipient Recipient { get; set; } = new();
    public List<NFeItem> Items { get; set; } = new();
    public NFeTotals Totals { get; set; } = new();
    public NFeTransport Transport { get; set; } = new();
    public NFePayment Payment { get; set; } = new();
    public string AdditionalInformation { get; set; } = string.Empty;
    public IDictionary<string, string> CustomTags { get; set; } = new Dictionary<string, string>();
}
