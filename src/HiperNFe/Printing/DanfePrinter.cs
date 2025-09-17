using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using HiperNFe.Models;

namespace HiperNFe.Printing;

/// <summary>
/// Gera representações simplificadas do DANFE em PDF.
/// </summary>
public sealed class DanfePrinter
{
    private readonly DanfeOptions _options;

    public DanfePrinter(DanfeOptions options)
    {
        _options = options;
    }

    public Stream GeneratePdf(FiscalDocument document)
    {
        var builder = new StringBuilder();
        builder.AppendLine("DANFE SIMPLIFICADO");
        builder.AppendLine($"Chave: {document.AccessKey}");
        builder.AppendLine($"Emissão: {document.IssueDate:dd/MM/yyyy HH:mm}");
        builder.AppendLine($"Layout Landscape: {_options.Landscape}");
        if (!string.IsNullOrWhiteSpace(_options.CompanyLogoPath))
        {
            builder.AppendLine($"Logo: {_options.CompanyLogoPath}");
        }

        var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(builder.ToString());
        }

        stream.Position = 0;
        return stream;
    }

    public Stream GeneratePdf(XElement document)
    {
        var fiscalDocument = new FiscalDocument
        {
            AccessKey = document.Attribute("Id")?.Value.Replace("NFe", string.Empty) ?? string.Empty,
            IssueDate = DateTime.UtcNow
        };
        return GeneratePdf(fiscalDocument);
    }
}
