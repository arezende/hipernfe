using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using HiperNFe.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace HiperNFe.Tests.Services;

public class BatchProcessorTests
{
    [Fact]
    public async Task ProcessAsync_DispatchesRequestsToMatchingService()
    {
        var request = new SefazRequest { ServiceName = "consulta" };
        var response = new SefazResponse { Success = true };

        var serviceMock = new Mock<ISefazService>();
        serviceMock.SetupGet(s => s.Name).Returns("consulta");
        serviceMock.Setup(s => s.CanHandle("consulta")).Returns(true);
        serviceMock.Setup(s => s.SendAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var processor = new BatchProcessor(new[] { serviceMock.Object }, NullLogger<BatchProcessor>.Instance);
        var result = await processor.ProcessAsync("123", new[] { request });

        Assert.Equal("123", result.BatchId);
        Assert.Single(result.Responses);
        Assert.True(result.Responses.First().Success);
        serviceMock.Verify(s => s.SendAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WhenServiceNotFound_ReturnsError()
    {
        var processor = new BatchProcessor(new List<ISefazService>(), NullLogger<BatchProcessor>.Instance);
        var result = await processor.ProcessAsync("456", new[] { new SefazRequest { ServiceName = "desconhecido" } });

        Assert.False(result.Responses.First().Success);
        Assert.Equal("404", result.Responses.First().StatusCode);
    }
}
