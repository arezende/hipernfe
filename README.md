# hipernfe

Biblioteca para Emissão e Gestão de NF-e em C#.

## Visão Geral

A HiperNFe é uma biblioteca escrita em C# voltada para facilitar a integração com os serviços da SEFAZ para emissão e gestão de Notas Fiscais eletrônicas (NF-e). Ela oferece uma interface completa para os serviços oficiais, além de utilitários para geração da DANFE e envio de e-mails com os documentos fiscais.

## Recursos Principais

- Autorização, consulta de recibo e protocolo de NF-e.
- Cancelamento, carta de correção eletrônica e inutilização de numeração.
- Distribuição de documentos eletrônicos e manifestação do destinatário.
- Consulta de status de serviço da SEFAZ e download de XML autorizado.
- Impressão da DANFE por meio de uma interface de impressora extensível.
- Envio de e-mails com anexos do XML e DANFE gerados automaticamente.

## Estrutura do Projeto

```
src/
 └── HiperNFe/
     ├── Configuration/        # Configurações de ambiente e certificado digital
     ├── Email/                # Serviço e modelos para envio de e-mail
     ├── Models/               # Modelos de domínio para NF-e
     ├── Printing/             # Impressoras e opções de DANFE
     ├── Serialization/        # Serializadores de NF-e
     ├── Services/             # Interfaces e serviços para comunicação com a SEFAZ
     └── HiperNFe.cs           # Super classe que concentra as operações de NF-e
 └── HiperNFe.TestApp/         # Aplicativo Windows Forms para testes manuais da biblioteca
```

## Aplicativo de Testes Windows Forms

O diretório `src/HiperNFe.TestApp` contém um pequeno aplicativo Windows Forms que facilita a experimentação com a biblioteca. Ele utiliza um serviço "fake" em memória para simular respostas da SEFAZ e permite:

- Registrar documentos de teste e acompanhar a fila de emissão;
- Executar operações de emissão, cancelamento e consulta de status;
- Visualizar o DANFE gerado pelo `SimpleDanfePrinter` e baixar o XML simulado;
- Enviar e-mails de demonstração utilizando um serviço SMTP em memória.

Para executar o projeto é necessário utilizar o SDK do .NET em um ambiente Windows:

```bash
cd src/HiperNFe.TestApp
dotnet build
dotnet run
```

O aplicativo é destinado apenas para fins de demonstração e não realiza comunicação real com a SEFAZ.

## Como Utilizar

1. **Configuração do Serviço**

```csharp
var config = new NFeServiceConfig
{
    FederalUnit = "SP",
    Environment = SefazEnvironment.Homologation,
    ServiceUrl = "https://homologacao.sefaz.gov.br/NFeAutorizacao4/NFeAutorizacao4.asmx",
    EmitterCnpj = "12345678000190"
};
```

2. **Instanciação dos Componentes**

```csharp
var serializer = new SimpleNFeSerializer();
var printer = new SimpleDanfePrinter();
var smtpClient = new SmtpClient("smtp.seudominio.com")
{
    Credentials = new NetworkCredential("usuario", "senha"),
    EnableSsl = true,
    Port = 587
};
var emailService = new SmtpEmailService(smtpClient);

using var hipernfe = new HiperNFe(config, serializer, printer, emailService);
```

3. **Registro e Emissão de NF-e**

```csharp
var documento = new NFeDocument
{
    AccessKey = "12345678901234567890123456789012345678901234",
    Emitter = new NFeEmitter { TradeName = "Minha Empresa", Cnpj = "12345678000190" },
    Recipient = new NFeRecipient { CorporateName = "Cliente", Cnpj = "10987654000199" }
};

hipernfe.RegistrarDocumento(documento);

var autorizacao = await hipernfe.EmitirAsync(documento.AccessKey);
if (autorizacao.IsAuthorized)
{
    var email = new EmailMessage
    {
        From = "nfe@seudominio.com",
        To = { "cliente@empresa.com" },
        Subject = "NF-e emitida",
        Body = "Segue anexa a NF-e emitida."
    };

    await hipernfe.EnviarEmailAsync(documento.AccessKey, email);
}
```

4. **Outras Operações**

```csharp
// Consultar o status da SEFAZ
await hipernfe.ConsultarStatusAsync("SP");

// Cancelar uma nota e removê-la da fila
await hipernfe.CancelarAsync(documento.AccessKey, "Erro de digitação nos dados da NF-e");

// Emitir todas as notas cadastradas
await hipernfe.EmitirTodasAsync();
```

## Extensibilidade

- **Serialização**: implemente `INFeSerializer` para customizar o formato de serialização do XML.
- **Impressão**: implemente `IDanfePrinter` para gerar DANFE em PDF, HTML ou outros formatos.
- **Comunicação com a SEFAZ**: implemente `ISefazClient` para suportar diferentes abordagens (SOAP, REST, intermediadores).
- **Envio de E-mail**: implemente `IEmailService` para se integrar com serviços externos (SMTP, APIs de e-mail, etc.).

## Aviso

Os envelopes SOAP fornecidos são exemplos simplificados e podem exigir ajustes conforme a SEFAZ de cada estado e os requisitos de certificado digital da sua aplicação.
