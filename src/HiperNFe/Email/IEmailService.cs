using System.Threading;
using System.Threading.Tasks;

namespace HiperNFe.Email;

/// <summary>
/// Serviço responsável por envio de e-mails com documentos fiscais.
/// </summary>
public interface IEmailService
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
