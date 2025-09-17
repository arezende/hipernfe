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
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace HiperNFe.Tests.Services;

public class AuthorizationServiceTests
{
    [Fact]
    public async Task SendAsync_WhenDependenciesSucceed_ReturnsResponseFromSefaz()
    {
        var configuration = new SefazConfiguration
        {
            AuthorizationUrl = new Uri("https://sefaz.test/autorizacao")
        };

        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=Test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));

        var certificateManager = new Mock<CertificateManager>();
        certificateManager
            .Setup(manager => manager.GetCertificate(It.IsAny<CertificateConfiguration>()))
            .Returns(certificate);

        var schemaValidator = new SchemaValidator();
        var xmlGenerator = new XmlGenerator();
        var xmlSigner = new XmlSigner();

        var sefazResponse = new SefazResponse
        {
            Success = true,
            StatusCode = "200",
            Message = "Autorizado",
            Payload = new XElement("retEnviNFe")
        };

        var sefazClient = new Mock<SefazHttpClient>(MockBehavior.Strict, new HttpClient(new FakeHttpMessageHandler()), NullLogger<SefazHttpClient>.Instance);
        sefazClient
            .Setup(client => client.PostXmlAsync(It.IsAny<Uri>(), It.IsAny<XDocument>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sefazResponse);

        var service = new AuthorizationService(
            configuration,
            certificateManager.Object,
            xmlGenerator,
            xmlSigner,
            schemaValidator,
            sefazClient.Object,
            NullLogger<AuthorizationService>.Instance);

        var requestPayload = new FiscalDocument { AccessKey = "12345678901234567890123456789012345678901234" }.ToXml();
        var response = await service.SendAsync(new SefazRequest
        {
            ServiceName = "autorizacao",
            Payload = requestPayload
        });

        Assert.True(response.Success);
        Assert.Equal("200", response.StatusCode);
        Assert.Equal("Autorizado", response.Message);
        sefazClient.Verify(client => client.PostXmlAsync(configuration.AuthorizationUrl, It.IsAny<XDocument>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
    }
}
