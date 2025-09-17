using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Carrega certificados digitais de arquivos ou do repositório local.
/// </summary>
public class CertificateManager
{
    public virtual X509Certificate2 GetCertificate(CertificateConfiguration configuration)
    {
        if (!string.IsNullOrWhiteSpace(configuration.Path))
        {
            return new X509Certificate2(configuration.Path, configuration.Password, X509KeyStorageFlags.MachineKeySet);
        }

        if (!string.IsNullOrWhiteSpace(configuration.SubjectName))
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates
                .Find(X509FindType.FindBySubjectName, configuration.SubjectName, validOnly: false)
                .OfType<X509Certificate2>()
                .FirstOrDefault() ?? throw new InvalidOperationException("Certificado não encontrado.");

            return certificate;
        }

        throw new InvalidOperationException("Nenhuma informação de certificado foi fornecida.");
    }
}
