using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Configuration;
using HiperNFe.Email;
using HiperNFe.Models;
using HiperNFe.Printing;
using HiperNFe.Serialization;
using HiperNFe.Services;

namespace HiperNFe;

/// <summary>
/// Classe de alto nível que concentra todas as operações de NF-e para o emissor.
/// </summary>
public class HiperNFe : IDisposable
{
    private readonly List<NFeDocument> _documents = new();
    private readonly object _syncRoot = new();
    private readonly INFeService _service;
    private readonly bool _ownsService;

    /// <summary>
    /// Inicializa a classe com todas as dependências explícitas.
    /// </summary>
    public HiperNFe(
        NFeServiceConfig config,
        INFeSerializer serializer,
        IDanfePrinter printer,
        IEmailService emailService,
        HttpClient? httpClient = null)
        : this(new HiperNFeService(config, serializer, printer, emailService, httpClient), config, true)
    {
    }

    /// <summary>
    /// Inicializa a classe com um cliente SEFAZ personalizado.
    /// </summary>
    public HiperNFe(
        NFeServiceConfig config,
        INFeSerializer serializer,
        IDanfePrinter printer,
        IEmailService emailService,
        ISefazClient sefazClient)
        : this(new HiperNFeService(config, serializer, printer, emailService, sefazClient), config, true)
    {
    }

    /// <summary>
    /// Inicializa a classe com um serviço previamente configurado.
    /// </summary>
    public HiperNFe(INFeService service, NFeServiceConfig config)
        : this(service, config, false)
    {
    }

    private HiperNFe(INFeService service, NFeServiceConfig config, bool ownsService)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        Config = config ?? throw new ArgumentNullException(nameof(config));
        _ownsService = ownsService;
    }

    /// <summary>
    /// Configurações utilizadas para comunicação com a SEFAZ.
    /// </summary>
    public NFeServiceConfig Config { get; }

    /// <summary>
    /// Documentos controlados pela instância para emissão, consulta e cancelamento.
    /// </summary>
    public IReadOnlyCollection<NFeDocument> Documentos
    {
        get
        {
            lock (_syncRoot)
            {
                return _documents.ToList().AsReadOnly();
            }
        }
    }

    /// <summary>
    /// Adiciona um documento à lista interna de controle.
    /// </summary>
    public void RegistrarDocumento(NFeDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        lock (_syncRoot)
        {
            if (_documents.Any(d => d.AccessKey == document.AccessKey))
            {
                throw new InvalidOperationException($"NF-e com chave {document.AccessKey} já está cadastrada.");
            }

            _documents.Add(document);
        }
    }

    /// <summary>
    /// Remove um documento da lista interna utilizando a chave de acesso.
    /// </summary>
    public bool RemoverDocumento(string accessKey)
    {
        if (string.IsNullOrWhiteSpace(accessKey))
        {
            throw new ArgumentException("Chave de acesso inválida.", nameof(accessKey));
        }

        lock (_syncRoot)
        {
            return _documents.RemoveAll(d => d.AccessKey == accessKey) > 0;
        }
    }

    /// <summary>
    /// Obtém um documento já registrado pela chave de acesso.
    /// </summary>
    public NFeDocument? ObterDocumento(string accessKey)
    {
        if (string.IsNullOrWhiteSpace(accessKey))
        {
            throw new ArgumentException("Chave de acesso inválida.", nameof(accessKey));
        }

        lock (_syncRoot)
        {
            return _documents.FirstOrDefault(d => d.AccessKey == accessKey);
        }
    }

    /// <summary>
    /// Consulta o status operacional da SEFAZ.
    /// </summary>
    public Task<SefazStatusResponse> ConsultarStatusAsync(string stateCode, CancellationToken cancellationToken = default)
        => _service.GetStatusAsync(stateCode, cancellationToken);

    /// <summary>
    /// Emite (autoriza) uma NF-e previamente registrada.
    /// </summary>
    public async Task<NFeAuthorizationResult> EmitirAsync(string accessKey, CancellationToken cancellationToken = default)
    {
        var document = ObterOuLancar(accessKey);
        return await _service.AuthorizeAsync(document, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Emite todas as NF-e atualmente registradas.
    /// </summary>
    public async Task<IReadOnlyCollection<NFeAuthorizationResult>> EmitirTodasAsync(CancellationToken cancellationToken = default)
    {
        List<NFeDocument> documentos;
        lock (_syncRoot)
        {
            documentos = _documents.ToList();
        }

        var resultados = new List<NFeAuthorizationResult>(documentos.Count);
        foreach (var documento in documentos)
        {
            var resultado = await _service.AuthorizeAsync(documento, cancellationToken).ConfigureAwait(false);
            resultados.Add(resultado);
        }

        return resultados.AsReadOnly();
    }

    /// <summary>
    /// Consulta o recibo de uma NF-e.
    /// </summary>
    public Task<NFeAuthorizationResult> ConsultarReciboAsync(string receiptNumber, string stateCode, CancellationToken cancellationToken = default)
        => _service.QueryReceiptAsync(receiptNumber, stateCode, cancellationToken);

    /// <summary>
    /// Consulta o protocolo de uma NF-e.
    /// </summary>
    public Task<NFeAuthorizationResult> ConsultarProtocoloAsync(string accessKey, string stateCode, CancellationToken cancellationToken = default)
        => _service.QueryProtocolAsync(accessKey, stateCode, cancellationToken);

    /// <summary>
    /// Cancela uma NF-e registrada.
    /// </summary>
    public async Task<NFeCancellationResult> CancelarAsync(string accessKey, string justification, CancellationToken cancellationToken = default)
    {
        var resultado = await _service.CancelAsync(accessKey, justification, cancellationToken).ConfigureAwait(false);
        RemoverDocumento(accessKey);
        return resultado;
    }

    /// <summary>
    /// Envia carta de correção para uma NF-e registrada.
    /// </summary>
    public Task<NFeCorrectionResult> CorrigirAsync(string accessKey, string correctionText, CancellationToken cancellationToken = default)
        => _service.SubmitCorrectionAsync(accessKey, correctionText, cancellationToken);

    /// <summary>
    /// Realiza a inutilização de numeração de NF-e.
    /// </summary>
    public Task<NFeInutilizationResult> InutilizarAsync(NFeInutilizationRequest request, CancellationToken cancellationToken = default)
        => _service.InutilizeAsync(request, cancellationToken);

    /// <summary>
    /// Faz a distribuição de documentos do destinatário.
    /// </summary>
    public Task<NFeDistributionResponse> DistribuirAsync(NFeDistributionRequest request, CancellationToken cancellationToken = default)
        => _service.DistributeAsync(request, cancellationToken);

    /// <summary>
    /// Registra a manifestação do destinatário.
    /// </summary>
    public Task<NFeManifestationResult> ManifestarAsync(NFeManifestationRequest request, CancellationToken cancellationToken = default)
        => _service.ManifestAsync(request, cancellationToken);

    /// <summary>
    /// Baixa o XML autorizado de uma NF-e.
    /// </summary>
    public Task<NFeDocument> BaixarXmlAsync(string accessKey, CancellationToken cancellationToken = default)
        => _service.DownloadXmlAsync(accessKey, cancellationToken);

    /// <summary>
    /// Imprime a DANFE de uma NF-e registrada.
    /// </summary>
    public async Task<Stream> ImprimirDanfeAsync(string accessKey, DanfePrintOptions? options = null, CancellationToken cancellationToken = default)
    {
        var document = ObterOuLancar(accessKey);
        return await _service.PrintDanfeAsync(document, options, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Envia por e-mail a NF-e e sua DANFE.
    /// </summary>
    public async Task EnviarEmailAsync(string accessKey, EmailMessage email, CancellationToken cancellationToken = default)
    {
        var document = ObterOuLancar(accessKey);
        await _service.SendEmailAsync(document, email, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Descadastra todos os documentos atualmente controlados.
    /// </summary>
    public void LimparDocumentos()
    {
        lock (_syncRoot)
        {
            _documents.Clear();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_ownsService && _service is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private NFeDocument ObterOuLancar(string accessKey)
    {
        var document = ObterDocumento(accessKey);
        if (document == null)
        {
            throw new InvalidOperationException($"NF-e com chave {accessKey} não está registrada para processamento.");
        }

        return document;
    }
}
