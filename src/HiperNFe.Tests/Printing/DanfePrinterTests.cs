using System.Collections.Generic;
using System.IO;
using HiperNFe.Models;
using HiperNFe.Printing;
using Xunit;

namespace HiperNFe.Tests.Printing;

public class DanfePrinterTests
{
    [Fact]
    public void GeneratePdf_FromFiscalDocument_ReturnsStreamWithContent()
    {
        var printer = new DanfePrinter(new DanfeOptions { CompanyLogoPath = "logo.png" });
        var document = new FiscalDocument { AccessKey = "123", Payload = new Dictionary<string, object> { ["ProductName"] = "Produto" } };

        using var stream = printer.GeneratePdf(document);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();

        Assert.Contains("DANFE", content);
        Assert.Contains("123", content);
        Assert.Contains("logo.png", content);
    }
}
