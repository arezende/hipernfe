using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HiperNFe.Models;

/// <summary>
/// Representa uma NF-e com dados simplificados para geração de XML.
/// </summary>
public sealed class FiscalDocument
{
    public string AccessKey { get; init; } = string.Empty;

    public string Version { get; init; } = "4.00";

    public DateTime IssueDate { get; init; } = DateTime.UtcNow;

    public IDictionary<string, object> Payload { get; init; } = new Dictionary<string, object>();

    public XElement ToXml()
    {
        var document = new XElement("NFe",
            new XAttribute("versao", Version),
            new XAttribute("Id", $"NFe{AccessKey}"),
            new XElement("infNFe",
                new XElement("ide",
                    new XElement("dhEmi", IssueDate.ToString("yyyy-MM-ddTHH:mm:sszzz"))),
                new XElement("det", new XAttribute("nItem", 1),
                    new XElement("prod",
                        new XElement("cProd", Payload.TryGetValue("ProductCode", out var productCode) ? productCode : ""),
                        new XElement("xProd", Payload.TryGetValue("ProductName", out var productName) ? productName : "Produto")))));

        return document;
    }
}
