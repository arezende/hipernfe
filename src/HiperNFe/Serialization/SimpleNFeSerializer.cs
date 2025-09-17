using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HiperNFe.Models;

namespace HiperNFe.Serialization;

/// <summary>
/// Implementação simples baseada em XmlSerializer.
/// </summary>
public class SimpleNFeSerializer : INFeSerializer
{
    public Task<string> SerializeAsync(NFeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return Task.Run(() =>
        {
            var serializer = new XmlSerializer(typeof(NFeDocument));
            using var writer = new Utf8StringWriter();
            serializer.Serialize(writer, document);
            return writer.ToString();
        });
    }

    public Task<NFeDocument> DeserializeAsync(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException("XML inválido", nameof(xml));
        }

        return Task.Run(() =>
        {
            var serializer = new XmlSerializer(typeof(NFeDocument));
            using var reader = new StringReader(xml);
            return (NFeDocument)serializer.Deserialize(reader)!;
        });
    }

    private class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
