using System.Collections.Generic;

namespace HiperNFe.Email;

/// <summary>
/// Representa um e-mail com anexos.
/// </summary>
public class EmailMessage
{
    public string From { get; set; } = string.Empty;
    public List<string> To { get; set; } = new();
    public List<string> Cc { get; set; } = new();
    public List<string> Bcc { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsBodyHtml { get; set; } = true;
    public List<EmailAttachment> Attachments { get; set; } = new();
}

/// <summary>
/// Representa um anexo de e-mail.
/// </summary>
public class EmailAttachment
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = System.Array.Empty<byte>();
    public string ContentType { get; set; } = "application/octet-stream";
}
