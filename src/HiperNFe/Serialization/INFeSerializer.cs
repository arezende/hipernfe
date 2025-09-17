using System.Threading.Tasks;
using HiperNFe.Models;

namespace HiperNFe.Serialization;

/// <summary>
/// Responsável por converter documentos NF-e em XML e vice-versa.
/// </summary>
public interface INFeSerializer
{
    Task<string> SerializeAsync(NFeDocument document);
    Task<NFeDocument> DeserializeAsync(string xml);
}
