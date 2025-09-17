using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using HiperNFe.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HiperNFe.SampleApp;

internal sealed class DemoCertificateManager : CertificateManager
{
    public override X509Certificate2 GetCertificate(CertificateConfiguration configuration)
    {
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=HiperNFe Demo", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
    }
}

internal sealed class FakeSefazHttpClient : SefazHttpClient
{
    public FakeSefazHttpClient(ILogger<SefazHttpClient> logger)
        : base(new HttpClient(), logger)
    {
    }

    public override Task<SefazResponse> PostXmlAsync(Uri endpoint, XDocument document, CancellationToken cancellationToken)
    {
        var responsePayload = new XDocument(
            new XElement("retEnviNFe",
                new XElement("tpAmb", "2"),
                new XElement("cStat", "100"),
                new XElement("xMotivo", "Autorizado o uso da NF-e de demonstração")));

        return Task.FromResult(new SefazResponse
        {
            Success = true,
            StatusCode = "200",
            Message = $"Simulação bem-sucedida em {endpoint}",
            Payload = responsePayload.Root
        });
    }
}

public static class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddSimpleConsole(options => options.SingleLine = true));

        var configuration = new SefazConfiguration
        {
            Environment = EnvironmentType.Homologation,
            AuthorizationUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/autorizacao"),
            ConsultationUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/consulta"),
            CancellationUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/cancelamento"),
            InutilizationUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/inutilizacao"),
            LetterCorrectionUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/carta-correcao"),
            ManifestationUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/manifestacao"),
            StatusServiceUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/status"),
            RequestTimeout = TimeSpan.FromSeconds(30),
            Certificate = new CertificateConfiguration()
        };

        services.AddHiperNFe(configuration);
        services.AddSingleton<CertificateManager, DemoCertificateManager>();
        services.AddSingleton<SefazHttpClient, FakeSefazHttpClient>();

        await using var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("Sample");
        logger.LogInformation("Iniciando demonstração da HiperNFe");

        var authorizationService = provider.GetRequiredService<AuthorizationService>();
        var request = new SefazRequest
        {
            ServiceName = authorizationService.Name,
            Payload = new XElement("enviNFe",
                new XAttribute("Id", "NFe12345678901234567890123456789012345678901234"),
                new XElement("versao", "4.00"),
                new XElement("infNFe", new XAttribute("Id", "NFe12345678901234567890123456789012345678901234")))
        };

        var response = await authorizationService.SendAsync(request);
        logger.LogInformation("Resultado: {Status} - {Message}", response.StatusCode, response.Message);

        if (response.Payload is not null)
        {
            Console.WriteLine("Payload retornado:\n" + response.Payload);
        }
        else
        {
            Console.WriteLine("Nenhum payload retornado.");
        }

        logger.LogInformation("Demonstração concluída");
    }
}
