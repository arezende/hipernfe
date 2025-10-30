using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Email;

namespace HiperNFe.TestApp;

/// <summary>
/// Serviço de e-mail em memória que armazena as mensagens enviadas para consulta posterior.
/// </summary>
public class InMemoryEmailService : IEmailService
{
    private readonly List<EmailMessage> _sentEmails = new();
    private readonly object _syncRoot = new();

    public event EventHandler<EmailMessage>? EmailSent;

    public IReadOnlyList<EmailMessage> SentEmails
    {
        get
        {
            lock (_syncRoot)
            {
                return _sentEmails.Select(CloneMessage).ToList().AsReadOnly();
            }
        }
    }

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        cancellationToken.ThrowIfCancellationRequested();

        lock (_syncRoot)
        {
            _sentEmails.Add(CloneMessage(message));
        }

        EmailSent?.Invoke(this, message);
        return Task.CompletedTask;
    }

    private static EmailMessage CloneMessage(EmailMessage original)
    {
        var clone = new EmailMessage
        {
            From = original.From,
            Subject = original.Subject,
            Body = original.Body,
            IsBodyHtml = original.IsBodyHtml
        };

        foreach (var address in original.To)
        {
            clone.To.Add(address);
        }

        foreach (var address in original.Cc)
        {
            clone.Cc.Add(address);
        }

        foreach (var address in original.Bcc)
        {
            clone.Bcc.Add(address);
        }

        foreach (var attachment in original.Attachments)
        {
            clone.Attachments.Add(new EmailAttachment
            {
                FileName = attachment.FileName,
                ContentType = attachment.ContentType,
                Content = attachment.Content.ToArray()
            });
        }

        return clone;
    }
}
