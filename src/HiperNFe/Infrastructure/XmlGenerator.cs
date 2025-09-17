using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Responsável por transformar objetos em XML seguindo o layout da NF-e.
/// </summary>
public sealed class XmlGenerator
{
    public XDocument GenerateDocument(XElement element)
    {
        var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), element);
        return document;
    }

    public byte[] ToBytes(XDocument document)
    {
        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            Indent = true,
            OmitXmlDeclaration = false
        });
        document.WriteTo(writer);
        writer.Flush();
        return stream.ToArray();
    }
}
