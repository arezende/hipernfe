using System;
using System.Globalization;
using System.Xml;
using HiperNFe.Models;

namespace HiperNFe.Services;

/// <summary>
/// Auxilia na conversão das respostas XML da SEFAZ.
/// </summary>
public static class SefazResponseParser
{
    public static SefazStatusResponse ParseStatus(string xml)
    {
        var document = LoadXml(xml);
        return new SefazStatusResponse
        {
            State = document.SelectSingleNode("//*[local-name()='cUF']")?.InnerText ?? string.Empty,
            StatusCode = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText ?? string.Empty,
            StatusMessage = document.SelectSingleNode("//*[local-name()='xMotivo']")?.InnerText ?? string.Empty,
            Environment = document.SelectSingleNode("//*[local-name()='tpAmb']")?.InnerText ?? string.Empty,
            SefazRegion = document.SelectSingleNode("//*[local-name()='xServ']")?.InnerText ?? string.Empty
        };
    }

    public static NFeAuthorizationResult ParseAuthorization(string xml)
    {
        var document = LoadXml(xml);
        return new NFeAuthorizationResult
        {
            IsAuthorized = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText == "100",
            StatusCode = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText ?? string.Empty,
            StatusMessage = document.SelectSingleNode("//*[local-name()='xMotivo']")?.InnerText ?? string.Empty,
            ProtocolNumber = document.SelectSingleNode("//*[local-name()='nProt']")?.InnerText ?? string.Empty,
            ReceptionDate = TryParseDate(document.SelectSingleNode("//*[local-name()='dhRecbto']")?.InnerText),
            Xml = xml
        };
    }

    public static NFeCancellationResult ParseCancellation(string xml)
    {
        var document = LoadXml(xml);
        return new NFeCancellationResult
        {
            IsCancelled = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText == "135",
            StatusCode = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText ?? string.Empty,
            StatusMessage = document.SelectSingleNode("//*[local-name()='xMotivo']")?.InnerText ?? string.Empty,
            ProtocolNumber = document.SelectSingleNode("//*[local-name()='nProt']")?.InnerText ?? string.Empty,
            EventDate = TryParseDate(document.SelectSingleNode("//*[local-name()='dhRegEvento']")?.InnerText),
            Xml = xml
        };
    }

    public static NFeCorrectionResult ParseCorrection(string xml)
    {
        var document = LoadXml(xml);
        return new NFeCorrectionResult
        {
            IsRegistered = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText == "135",
            StatusCode = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText ?? string.Empty,
            StatusMessage = document.SelectSingleNode("//*[local-name()='xMotivo']")?.InnerText ?? string.Empty,
            ProtocolNumber = document.SelectSingleNode("//*[local-name()='nProt']")?.InnerText ?? string.Empty,
            EventDate = TryParseDate(document.SelectSingleNode("//*[local-name()='dhRegEvento']")?.InnerText),
            Xml = xml
        };
    }

    public static NFeInutilizationResult ParseInutilization(string xml)
    {
        var document = LoadXml(xml);
        return new NFeInutilizationResult
        {
            IsInutilized = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText == "102",
            StatusCode = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText ?? string.Empty,
            StatusMessage = document.SelectSingleNode("//*[local-name()='xMotivo']")?.InnerText ?? string.Empty,
            ProtocolNumber = document.SelectSingleNode("//*[local-name()='nProt']")?.InnerText ?? string.Empty,
            EventDate = TryParseDate(document.SelectSingleNode("//*[local-name()='dhRecbto']")?.InnerText),
            Xml = xml
        };
    }

    public static NFeManifestationResult ParseManifestation(string xml)
    {
        var document = LoadXml(xml);
        return new NFeManifestationResult
        {
            IsRegistered = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText == "135",
            StatusCode = document.SelectSingleNode("//*[local-name()='cStat']")?.InnerText ?? string.Empty,
            StatusMessage = document.SelectSingleNode("//*[local-name()='xMotivo']")?.InnerText ?? string.Empty,
            ProtocolNumber = document.SelectSingleNode("//*[local-name()='nProt']")?.InnerText ?? string.Empty,
            EventDate = TryParseDate(document.SelectSingleNode("//*[local-name()='dhRegEvento']")?.InnerText),
            Xml = xml
        };
    }

    private static XmlDocument LoadXml(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException("XML inválido", nameof(xml));
        }

        var document = new XmlDocument();
        document.LoadXml(xml);
        return document;
    }

    private static DateTime TryParseDate(string? value)
    {
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var date))
        {
            return date;
        }

        return DateTime.MinValue;
    }
}
