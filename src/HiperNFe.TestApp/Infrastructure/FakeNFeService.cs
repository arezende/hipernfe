using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Configuration;
using HiperNFe.Email;
using HiperNFe.Models;
using HiperNFe.Printing;
using HiperNFe.Serialization;
using HiperNFe.Services;
using HiperNFe.TestApp;

namespace HiperNFe.TestApp.Infrastructure;

/// <summary>
/// Implementação simplificada do <see cref="INFeService"/> para cenários de demonstração.
/// Nenhuma comunicação externa é realizada: todas as respostas são geradas em memória.
/// </summary>
internal sealed class FakeNFeService : INFeService
{
    private readonly NFeServiceConfig _config;
    private readonly INFeSerializer _serializer;
    private readonly IDanfePrinter _printer;
    private readonly InMemoryEmailService _emailService;
    private readonly Dictionary<string, NFeDocument> _authorizedDocuments = new();
    private readonly HashSet<string> _cancelledDocuments = new();
    private readonly object _syncRoot = new();

    public FakeNFeService(NFeServiceConfig config, InMemoryEmailService emailService)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _serializer = new SimpleNFeSerializer();
        _printer = new SimpleDanfePrinter();
    }

    public event EventHandler<string>? OperationPerformed;

    public Task<SefazStatusResponse> GetStatusAsync(string stateCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(stateCode))
        {
            throw new ArgumentException("Código de UF inválido.", nameof(stateCode));
        }

        var response = new SefazStatusResponse
        {
            State = stateCode.ToUpperInvariant(),
            Environment = _config.Environment.ToString(),
            StatusCode = "107",
            StatusMessage = "Serviço em operação",
            SefazRegion = "Região simulada"
        };

        Log($"Consulta de status para {response.State}: {response.StatusMessage}.");
        return Task.FromResult(response);
    }

    public async Task<NFeAuthorizationResult> AuthorizeAsync(NFeDocument document, CancellationToken cancellationToken = default)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var xml = await _serializer.SerializeAsync(document).ConfigureAwait(false);
        NFeAuthorizationResult result;
        lock (_syncRoot)
        {
            _authorizedDocuments[document.AccessKey] = document;
            _cancelledDocuments.Remove(document.AccessKey);
            result = new NFeAuthorizationResult
            {
                IsAuthorized = true,
                StatusCode = "100",
                StatusMessage = "Autorizado o uso da NF-e",
                ProtocolNumber = GenerateProtocol(),
                ReceptionDate = DateTime.UtcNow,
                Xml = xml
            };
        }

        Log($"NF-e {document.AccessKey} autorizada.");
        return result;
    }

    public Task<NFeAuthorizationResult> QueryReceiptAsync(string receiptNumber, string stateCode, CancellationToken cancellationToken = default)
    {
        var response = new NFeAuthorizationResult
        {
            IsAuthorized = true,
            StatusCode = "104",
            StatusMessage = $"Recibo {receiptNumber} localizado",
            ProtocolNumber = receiptNumber,
            ReceptionDate = DateTime.UtcNow
        };

        Log($"Consulta de recibo {receiptNumber} para {stateCode}: {response.StatusMessage}.");
        return Task.FromResult(response);
    }

    public Task<NFeAuthorizationResult> QueryProtocolAsync(string accessKey, string stateCode, CancellationToken cancellationToken = default)
    {
        lock (_syncRoot)
        {
            if (!_authorizedDocuments.TryGetValue(accessKey, out var document))
            {
                throw new InvalidOperationException($"NF-e {accessKey} não encontrada.");
            }

            var response = new NFeAuthorizationResult
            {
                IsAuthorized = true,
                StatusCode = "100",
                StatusMessage = "Protocolo localizado",
                ProtocolNumber = GenerateProtocol(),
                ReceptionDate = document.IssueDate,
                Xml = string.Empty
            };

            Log($"Consulta de protocolo para {accessKey}: {response.StatusMessage}.");
            return Task.FromResult(response);
        }
    }

    public Task<NFeCancellationResult> CancelAsync(string accessKey, string justification, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(accessKey))
        {
            throw new ArgumentException("Chave de acesso inválida.", nameof(accessKey));
        }

        NFeCancellationResult result;
        lock (_syncRoot)
        {
            if (!_authorizedDocuments.ContainsKey(accessKey))
            {
                throw new InvalidOperationException($"NF-e {accessKey} não está autorizada.");
            }

            _cancelledDocuments.Add(accessKey);
            result = new NFeCancellationResult
            {
                IsCancelled = true,
                StatusCode = "135",
                StatusMessage = justification,
                ProtocolNumber = GenerateProtocol(),
                EventDate = DateTime.UtcNow,
                Xml = $"<cancelamento chave=\"{accessKey}\" justificativa=\"{System.Security.SecurityElement.Escape(justification)}\" />"
            };
        }

        Log($"Cancelamento da NF-e {accessKey} registrado.");
        return Task.FromResult(result);
    }

    public Task<NFeCorrectionResult> SubmitCorrectionAsync(string accessKey, string correctionText, CancellationToken cancellationToken = default)
    {
        var response = new NFeCorrectionResult
        {
            IsRegistered = true,
            StatusCode = "135",
            StatusMessage = "Carta de correção registrada",
            ProtocolNumber = GenerateProtocol(),
            EventDate = DateTime.UtcNow,
            Xml = $"<cce chave=\"{accessKey}\">{System.Security.SecurityElement.Escape(correctionText)}</cce>"
        };

        Log($"CC-e registrada para a NF-e {accessKey}.");
        return Task.FromResult(response);
    }

    public Task<NFeInutilizationResult> InutilizeAsync(NFeInutilizationRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var response = new NFeInutilizationResult
        {
            IsInutilized = true,
            StatusCode = "102",
            StatusMessage = "Numeração inutilizada",
            ProtocolNumber = GenerateProtocol(),
            EventDate = DateTime.UtcNow,
            Xml = "<inutilizacao />"
        };

        Log($"Inutilização registrada: série {request.Series} número {request.StartNumber} até {request.EndNumber}.");
        return Task.FromResult(response);
    }

    public async Task<NFeDistributionResponse> DistributeAsync(NFeDistributionRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        List<string> documents;
        lock (_syncRoot)
        {
            documents = _authorizedDocuments.Values
                .Select(d => d.AccessKey)
                .OrderBy(k => k, StringComparer.Ordinal)
                .ToList();
        }

        var payload = await Task.WhenAll(documents.Select(async key =>
        {
            var document = await DownloadXmlAsync(key, cancellationToken).ConfigureAwait(false);
            return await _serializer.SerializeAsync(document).ConfigureAwait(false);
        })).ConfigureAwait(false);

        var response = new NFeDistributionResponse
        {
            StatusCode = "138",
            StatusMessage = "Documentos localizados",
            LastNSU = request.LastNSU,
            DocumentsXml = payload.ToList()
        };

        Log($"Distribuição retornou {response.DocumentsXml.Count} documentos.");
        return response;
    }

    public Task<NFeManifestationResult> ManifestAsync(NFeManifestationRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var response = new NFeManifestationResult
        {
            IsRegistered = true,
            StatusCode = "135",
            StatusMessage = $"Manifestação {request.Manifestation} registrada",
            ProtocolNumber = GenerateProtocol(),
            EventDate = DateTime.UtcNow,
            Xml = "<manifestacao />"
        };

        Log($"Manifestação registrada para {request.AccessKey}.");
        return Task.FromResult(response);
    }

    public Task<NFeDocument> DownloadXmlAsync(string accessKey, CancellationToken cancellationToken = default)
    {
        lock (_syncRoot)
        {
            if (!_authorizedDocuments.TryGetValue(accessKey, out var document))
            {
                throw new InvalidOperationException($"NF-e {accessKey} não está autorizada.");
            }

            Log($"Download do XML para {accessKey} concluído.");
            return Task.FromResult(CloneDocument(document));
        }
    }

    public Task<Stream> PrintDanfeAsync(NFeDocument document, DanfePrintOptions? options, CancellationToken cancellationToken = default)
    {
        Log($"Geração da DANFE iniciada para {document.AccessKey}.");
        return _printer.PrintAsync(document, options, cancellationToken);
    }

    public async Task SendEmailAsync(NFeDocument document, EmailMessage email, CancellationToken cancellationToken = default)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (email == null)
        {
            throw new ArgumentNullException(nameof(email));
        }

        await using var danfeStream = await _printer.PrintAsync(document, new DanfePrintOptions(), cancellationToken).ConfigureAwait(false);
        using var danfeMemory = new MemoryStream();
        await danfeStream.CopyToAsync(danfeMemory, cancellationToken).ConfigureAwait(false);

        email.Attachments.Add(new EmailAttachment
        {
            FileName = $"DANFE-{document.AccessKey}.txt",
            ContentType = "text/plain",
            Content = danfeMemory.ToArray()
        });

        var xml = await _serializer.SerializeAsync(document).ConfigureAwait(false);
        email.Attachments.Add(new EmailAttachment
        {
            FileName = $"{document.AccessKey}.xml",
            ContentType = "application/xml",
            Content = System.Text.Encoding.UTF8.GetBytes(xml)
        });

        await _emailService.SendAsync(email, cancellationToken).ConfigureAwait(false);
        Log($"E-mail preparado com anexos para {string.Join(", ", email.To)}.");
    }

    private static NFeDocument CloneDocument(NFeDocument document)
    {
        var serializer = new SimpleNFeSerializer();
        var xml = serializer.SerializeAsync(document).GetAwaiter().GetResult();
        return serializer.DeserializeAsync(xml).GetAwaiter().GetResult();
    }

    private static string GenerateProtocol()
        => DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

    private void Log(string message)
        => OperationPerformed?.Invoke(this, message);
}
