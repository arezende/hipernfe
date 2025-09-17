using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using HiperNFe.Models;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Valida XMLs da NF-e utilizando schemas XSD oficiais.
/// </summary>
public sealed class SchemaValidator
{
    public IReadOnlyCollection<ErrorDetail> Validate(Stream xmlStream, params string[] schemaPaths)
    {
        if (schemaPaths is null || schemaPaths.Length == 0)
        {
            return Array.Empty<ErrorDetail>();
        }

        var errors = new List<ErrorDetail>();
        var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };

        foreach (var path in schemaPaths)
        {
            if (!File.Exists(path))
            {
                errors.Add(new ErrorDetail("XSD404", $"Schema não encontrado: {path}"));
                continue;
            }

            settings.Schemas.Add(null, path);
        }

        if (settings.Schemas.Count == 0)
        {
            return errors;
        }

        settings.ValidationEventHandler += (_, args) =>
        {
            var code = args.Exception?.SourceUri ?? "XSD";
            errors.Add(new ErrorDetail(code, args.Message));
        };

        using var reader = XmlReader.Create(xmlStream, settings);
        while (reader.Read())
        {
            // Apenas força a leitura completa para validação.
        }

        return errors;
    }
}
