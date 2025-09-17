using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Models;
using HiperNFe.Services;
using Microsoft.Extensions.Logging;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Processa lotes de documentos enviando requisições paralelas para a SEFAZ.
/// </summary>
public sealed class BatchProcessor
{
    private readonly IEnumerable<ISefazService> _services;
    private readonly ILogger<BatchProcessor> _logger;

    public BatchProcessor(IEnumerable<ISefazService> services, ILogger<BatchProcessor> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task<BatchResult> ProcessAsync(string batchId, IEnumerable<SefazRequest> requests, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processando lote {BatchId} com {Count} requisições", batchId, requests.Count());
        var tasks = requests.Select(request => DispatchAsync(request, cancellationToken));
        var responses = await Task.WhenAll(tasks).ConfigureAwait(false);
        return new BatchResult { BatchId = batchId, Responses = responses };
    }

    private async Task<SefazResponse> DispatchAsync(SefazRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var service = _services.FirstOrDefault(s => s.CanHandle(request.ServiceName));
            if (service is null)
            {
                _logger.LogWarning("Serviço não encontrado para {Service}", request.ServiceName);
                return new SefazResponse
                {
                    Success = false,
                    StatusCode = "404",
                    Message = $"Serviço {request.ServiceName} não está configurado."
                };
            }

            _logger.LogDebug("Enviando requisição para {Service}", request.ServiceName);
            return await service.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar requisição para {Service}", request.ServiceName);
            return SefazResponse.FromException(ex);
        }
    }
}
