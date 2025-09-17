# HiperNFe

Biblioteca modular para comunicação com os serviços da SEFAZ utilizando a versão mais recente da NF-e. O projeto foi organizado para facilitar a emissão, consulta e gerenciamento de documentos fiscais eletrônicos, além de oferecer módulos de impressão do DANFE e envio por e-mail.

## Estrutura da solução

A solução `src/HiperNFe.sln` contém dois projetos:

- `HiperNFe` – biblioteca principal com camadas de **Services**, **Models**, **Infrastructure**, **Printing** e **Email**.
- `HiperNFe.Tests` – testes unitários que validam os principais fluxos com uso extensivo de mocks.

```
src/
 ├─ HiperNFe.sln
 ├─ HiperNFe/
 │   ├─ Email/
 │   ├─ Infrastructure/
 │   ├─ Models/
 │   ├─ Printing/
 │   └─ Services/
 └─ HiperNFe.Tests/
     ├─ Infrastructure/
     ├─ Printing/
     └─ Services/
```

## Principais recursos

- **Serviços SEFAZ**: autorização, consulta, cancelamento, inutilização, carta de correção, manifestação do destinatário e consulta de status, todos com APIs síncronas e assíncronas.
- **Ambientes múltiplos**: configuração centralizada via `SefazConfiguration`, permitindo alternar entre homologação e produção.
- **Infraestrutura XML**: geração, assinatura com `X509Certificate2`, validação XSD com relatórios detalhados e processamento de lotes com logs.
- **Impressão DANFE**: geração de PDF simplificado com opções de personalização.
- **Envio de e-mail**: anexos do XML e PDF utilizando SMTP configurável.
- **Injeção de dependência**: extensão `AddHiperNFe` para registrar todos os componentes em `IServiceCollection`.

## Exemplo de uso

```csharp
using HiperNFe.Infrastructure;
using HiperNFe.Models;
using HiperNFe.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddLogging();

var configuration = new SefazConfiguration
{
    Environment = EnvironmentType.Homologation,
    AuthorizationUrl = new Uri("https://homolog.sefaz/autorizacao"),
    Certificate = new CertificateConfiguration { Path = "certificado.pfx", Password = "senha" }
};

services.AddHiperNFe(configuration);

var provider = services.BuildServiceProvider();
var authorization = provider.GetRequiredService<AuthorizationService>();

var document = new FiscalDocument { AccessKey = "CHAVE" };
var request = new SefazRequest
{
    ServiceName = authorization.Name,
    Payload = document.ToXml(),
    SchemaPaths = new[] { "Schemas/enviNFe_v4.00.xsd" }
};

var response = await authorization.SendAsync(request);
```

### Processamento de lotes

```csharp
var batchProcessor = provider.GetRequiredService<BatchProcessor>();
var batch = await batchProcessor.ProcessAsync("lote-001", new[] { request });
```

### Impressão e e-mail

```csharp
var printer = provider.GetRequiredService<DanfePrinter>();
using var pdf = printer.GeneratePdf(document);

var emailSender = provider.GetRequiredService<EmailSender>();
using var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(document.ToXml().ToString()));
await emailSender.SendDocumentsAsync("destinatario@empresa.com", "NF-e", "Segue nota fiscal", xmlStream, pdf);
```

## Executando os testes

A partir da pasta `src/` execute:

```bash
dotnet test
```

## Requisitos

- .NET SDK 8.0 ou superior.
- Certificado digital A1 instalado ou arquivo `.pfx` com senha.
- Schemas oficiais da NF-e disponíveis no caminho configurado para validação.

## Licença

Projeto disponibilizado para fins educacionais. Ajuste conforme as necessidades do seu ambiente.
