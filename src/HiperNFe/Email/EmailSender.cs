using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HiperNFe.Email;

/// <summary>
/// Responsável por enviar NF-e por e-mail anexando XML e PDF gerados.
/// </summary>
public sealed class EmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(EmailSettings settings)
    {
        _settings = settings;
    }

    public async Task SendAsync(string recipient, string subject, string body, IEnumerable<Attachment> attachments)
    {
        using var message = new MailMessage(_settings.From, recipient)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        foreach (var attachment in attachments)
        {
            message.Attachments.Add(attachment);
        }

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password)
        };

        await client.SendMailAsync(message).ConfigureAwait(false);
    }

    public async Task SendDocumentsAsync(string recipient, string subject, string body, Stream xmlStream, Stream pdfStream)
    {
        xmlStream.Position = 0;
        pdfStream.Position = 0;

        using var xmlAttachment = new Attachment(xmlStream, "nota.xml", "application/xml");
        using var pdfAttachment = new Attachment(pdfStream, "danfe.pdf", "application/pdf");

        await SendAsync(recipient, subject, body, new[] { xmlAttachment, pdfAttachment }).ConfigureAwait(false);
    }
}
