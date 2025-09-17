# HiperNFe

Biblioteca modular para comunicação com os serviços da SEFAZ utilizando a versão mais recente da NF-e. O projeto foi organizado para facilitar a emissão, consulta e gerenciamento de documentos fiscais eletrônicos, além de oferecer módulos de impressão do DANFE e envio por e-mail.

## Estrutura da solução

A solução `src/HiperNFe.sln`, alvo .NET 9, contém três projetos:

- `HiperNFe` – biblioteca principal com camadas de **Services**, **Models**, **Infrastructure**, **Printing** e **Email**.
- `HiperNFe.Tests` – testes unitários que validam os principais fluxos com uso extensivo de mocks.
- `HiperNFe.SampleApp` – aplicativo de console que demonstra como consumir a biblioteca simulando os serviços da SEFAZ.

```
src/
 ├─ HiperNFe.sln
 ├─ HiperNFe/
 │   ├─ Email/
 │   ├─ Infrastructure/
 │   ├─ Models/
 │   ├─ Printing/
 │   └─ Services/
 ├─ HiperNFe.Tests/
 │   ├─ Infrastructure/
 │   ├─ Printing/
 │   └─ Services/
 └─ HiperNFe.SampleApp/
     └─ Program.cs
```

## Principais recursos

- **Serviços SEFAZ**: autorização, consulta, cancelamento, inutilização, carta de correção, manifestação do destinatário e consulta de status, todos com APIs síncronas e assíncronas.
- **Ambientes múltiplos**: configuração centralizada via `SefazConfiguration`, permitindo alternar entre homologação e produção.
- **Infraestrutura XML**: geração, assinatura com `X509Certificate2`, validação XSD com relatórios detalhados e processamento de lotes com logs.
- **Impressão DANFE**: geração de PDF simplificado com opções de personalização.
- **Envio de e-mail**: anexos do XML e PDF utilizando SMTP configurável.
- **Injeção de dependência**: extensão `AddHiperNFe` para registrar todos os componentes em `IServiceCollection`.

## Exemplo de uso

O console `HiperNFe.SampleApp` registra a biblioteca na DI, injeta implementações simuladas de certificado e comunicação HTTP e executa o fluxo de autorização de forma totalmente offline.

```bash
cd src
# Ajuste o comando conforme o SDK instalado
DOTNET_CLI_TELEMETRY_OPTOUT=1 dotnet run --project HiperNFe.SampleApp
```

O trecho abaixo ilustra os passos essenciais da aplicação de exemplo:

```csharp
var services = new ServiceCollection();
services.AddLogging(builder => builder.AddSimpleConsole());

var configuration = new SefazConfiguration
{
    Environment = EnvironmentType.Homologation,
    AuthorizationUrl = new Uri("https://homologacao.sefaz.fazenda.gov.br/autorizacao"),
    Certificate = new CertificateConfiguration()
};

services.AddHiperNFe(configuration);
services.AddSingleton<CertificateManager, DemoCertificateManager>();
services.AddSingleton<SefazHttpClient, FakeSefazHttpClient>();

await using var provider = services.BuildServiceProvider();
var authorization = provider.GetRequiredService<AuthorizationService>();
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

- .NET SDK 9.0 (preview) ou superior.
- Certificado digital A1 instalado ou arquivo `.pfx` com senha.
- Schemas oficiais da NF-e disponíveis no caminho configurado para validação.

## Licença

Projeto disponibilizado para fins educacionais. Ajuste conforme as necessidades do seu ambiente.
