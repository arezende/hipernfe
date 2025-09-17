using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Realiza a assinatura digital dos XMLs utilizando certificados A1.
/// </summary>
public sealed class XmlSigner
{
    public XmlDocument Sign(XmlDocument document, X509Certificate2 certificate)
    {
        var privateKey = certificate.GetRSAPrivateKey() ?? throw new CryptographicException("Certificado sem chave privada.");

        var signedXml = new SignedXml(document)
        {
            SigningKey = privateKey
        };

        var reference = new Reference("#" + document.DocumentElement?.GetAttribute("Id"));
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        signedXml.AddReference(reference);

        signedXml.ComputeSignature();
        var xmlDigitalSignature = signedXml.GetXml();
        document.DocumentElement?.AppendChild(document.ImportNode(xmlDigitalSignature, true));
        return document;
    }
}
