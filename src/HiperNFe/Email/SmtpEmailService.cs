using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace HiperNFe.Email;

/// <summary>
/// Implementação do serviço de e-mail utilizando SMTP.
/// </summary>
public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient _client;

    public SmtpEmailService(SmtpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(message.From),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsBodyHtml
        };

        foreach (var to in message.To)
        {
            mailMessage.To.Add(to);
        }

        foreach (var cc in message.Cc)
        {
            mailMessage.CC.Add(cc);
        }

        foreach (var bcc in message.Bcc)
        {
            mailMessage.Bcc.Add(bcc);
        }

        foreach (var attachment in message.Attachments)
        {
            var stream = new MemoryStream(attachment.Content);
            var mailAttachment = new Attachment(stream, attachment.FileName, attachment.ContentType);
            mailMessage.Attachments.Add(mailAttachment);
        }

        using var cancellationRegistration = cancellationToken.Register(() => _client.SendAsyncCancel());
        await _client.SendMailAsync(mailMessage).ConfigureAwait(false);
    }
}
