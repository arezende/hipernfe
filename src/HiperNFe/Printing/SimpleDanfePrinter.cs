using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Models;

namespace HiperNFe.Printing;

/// <summary>
/// Implementação básica que gera a DANFE em formato PDF simplificado (texto).
/// </summary>
public class SimpleDanfePrinter : IDanfePrinter
{
    public Task<Stream> PrintAsync(NFeDocument document, DanfePrintOptions? options, CancellationToken cancellationToken = default)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        options ??= new DanfePrintOptions();

        return Task.Run<Stream>(() =>
        {
            var builder = new StringBuilder();
            builder.AppendLine("DANFE SIMPLIFICADO");
            builder.AppendLine($"Chave de Acesso: {document.AccessKey}");
            builder.AppendLine($"Emitente: {document.Emitter.TradeName} - CNPJ: {document.Emitter.Cnpj}");
            builder.AppendLine($"Destinatário: {document.Recipient.CorporateName}");
            builder.AppendLine($"Valor Total: {document.Totals.TotalInvoice:C}");
            builder.AppendLine("Itens:");
            foreach (var item in document.Items)
            {
                builder.AppendLine($" - {item.LineNumber:D2} {item.Description} ({item.Quantity} {item.Unit}) = {item.NetTotal:C}");
            }

            if (!string.IsNullOrEmpty(options.CustomFooter))
            {
                builder.AppendLine(options.CustomFooter);
            }

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            return (Stream)new MemoryStream(bytes, writable: false);
        }, cancellationToken);
    }
}
