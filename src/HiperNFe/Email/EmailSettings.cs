namespace HiperNFe.Email;

/// <summary>
/// Configurações de envio de e-mail via SMTP.
/// </summary>
public sealed class EmailSettings
{
    public string Host { get; init; } = string.Empty;

    public int Port { get; init; } = 587;

    public bool EnableSsl { get; init; } = true;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string From { get; init; } = string.Empty;
}
