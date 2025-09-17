using System.IO;
using System.Linq;
using System.Text;
using HiperNFe.Infrastructure;
using Xunit;

namespace HiperNFe.Tests.Infrastructure;

public class SchemaValidatorTests
{
    [Fact]
    public void Validate_ReturnsErrorWhenSchemaNotFound()
    {
        var validator = new SchemaValidator();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("<xml />"));

        var errors = validator.Validate(stream, "schema-inexistente.xsd");

        Assert.NotEmpty(errors);
        Assert.Equal("XSD404", errors.First().Code);
    }
}
