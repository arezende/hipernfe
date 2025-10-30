using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiperNFe.Configuration;
using HiperNFe.Email;
using HiperNFe.Models;
using HiperNFe.Printing;

namespace HiperNFe.TestApp;

public partial class MainForm : Form
{
    private readonly InMemoryEmailService _emailService;
    private readonly Infrastructure.FakeNFeService _fakeService;
    private readonly HiperNFe.HiperNFe _hiperNFe;
    private readonly Random _random = new();

    public MainForm()
    {
        InitializeComponent();

        var config = new NFeServiceConfig
        {
            FederalUnit = "SP",
            Environment = SefazEnvironment.Homologation,
            ServiceUrl = "https://sefaz.homologacao.exemplo"
        };

        _emailService = new InMemoryEmailService();
        _fakeService = new Infrastructure.FakeNFeService(config, _emailService);
        _fakeService.OperationPerformed += (_, message) => Log(message);
        _emailService.EmailSent += (_, message) => Log($"E-mail preparado para {string.Join(", ", message.To)} com assunto '{message.Subject}'.");

        _hiperNFe = new HiperNFe.HiperNFe(_fakeService, config);

        ufTextBox.Text = config.FederalUnit;
        emitterTextBox.Text = "Minha Empresa LTDA";
        recipientTextBox.Text = "Cliente de Teste";
        emailTextBox.Text = "cliente@empresa.com";
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        accessKeyTextBox.Text = GenerateAccessKey();
        UpdateDocumentButtons();
    }

    private void generateKeyButton_Click(object? sender, EventArgs e)
    {
        accessKeyTextBox.Text = GenerateAccessKey();
    }

    private async void consultarStatusButton_Click(object? sender, EventArgs e)
    {
        var state = ufTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(state))
        {
            MessageBox.Show(this, "Informe o código da UF.", "Dados obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            consultarStatusButton.Enabled = false;
            var response = await _hiperNFe.ConsultarStatusAsync(state);
            Log($"Status da SEFAZ {response.State}: {response.StatusMessage} ({response.StatusCode})");
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        finally
        {
            consultarStatusButton.Enabled = true;
        }
    }

    private void addDocumentButton_Click(object? sender, EventArgs e)
    {
        var accessKey = accessKeyTextBox.Text.Trim();
        if (accessKey.Length != 44 || !accessKey.All(char.IsDigit))
        {
            MessageBox.Show(this, "A chave de acesso deve conter 44 dígitos.", "Dados inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var emitter = emitterTextBox.Text.Trim();
        var recipient = recipientTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(emitter) || string.IsNullOrWhiteSpace(recipient))
        {
            MessageBox.Show(this, "Informe os nomes do emitente e do destinatário.", "Dados obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var total = totalNumericUpDown.Value;
        var document = CreateDocument(accessKey, emitter, recipient, total);

        try
        {
            _hiperNFe.RegistrarDocumento(document);
            Log($"Documento {document.Number} registrado para {document.Recipient.CorporateName}.");
            RefreshDocumentList();
            accessKeyTextBox.Text = GenerateAccessKey();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private async void authorizeButton_Click(object? sender, EventArgs e)
    {
        var accessKey = GetSelectedAccessKey();
        if (accessKey == null)
        {
            return;
        }

        await ExecuteAsync(authorizeButton, async () =>
        {
            var result = await _hiperNFe.EmitirAsync(accessKey);
            Log($"Resultado da emissão {accessKey}: {(result.IsAuthorized ? "Autorizado" : "Rejeitado")} - {result.StatusMessage}.");
        });
    }

    private async void authorizeAllButton_Click(object? sender, EventArgs e)
    {
        if (!_hiperNFe.Documentos.Any())
        {
            MessageBox.Show(this, "Nenhum documento registrado.", "Fila vazia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        await ExecuteAsync(authorizeAllButton, async () =>
        {
            var results = await _hiperNFe.EmitirTodasAsync();
            foreach (var result in results)
            {
                Log($"NF-e autorizada: {result.ProtocolNumber} - {result.StatusMessage}.");
            }
        });
    }

    private async void cancelButton_Click(object? sender, EventArgs e)
    {
        var accessKey = GetSelectedAccessKey();
        if (accessKey == null)
        {
            return;
        }

        await ExecuteAsync(cancelButton, async () =>
        {
            var result = await _hiperNFe.CancelarAsync(accessKey, "Cancelamento solicitado via aplicativo de teste.");
            Log($"Cancelamento da NF-e {accessKey}: {(result.IsCancelled ? "Sucesso" : "Falha")} - {result.StatusMessage}.");
            RefreshDocumentList();
        });
    }

    private void removeButton_Click(object? sender, EventArgs e)
    {
        var accessKey = GetSelectedAccessKey();
        if (accessKey == null)
        {
            return;
        }

        if (_hiperNFe.RemoverDocumento(accessKey))
        {
            Log($"Documento {accessKey} removido da fila.");
            RefreshDocumentList();
        }
        else
        {
            MessageBox.Show(this, "Documento não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void downloadXmlButton_Click(object? sender, EventArgs e)
    {
        var accessKey = GetSelectedAccessKey();
        if (accessKey == null)
        {
            return;
        }

        await ExecuteAsync(downloadXmlButton, async () =>
        {
            var document = await _hiperNFe.DownloadXmlAsync(accessKey);
            Log($"XML da NF-e {accessKey} recuperado. Emitente: {document.Emitter.TradeName}.");
        });
    }

    private async void printDanfeButton_Click(object? sender, EventArgs e)
    {
        var accessKey = GetSelectedAccessKey();
        if (accessKey == null)
        {
            return;
        }

        await ExecuteAsync(printDanfeButton, async () =>
        {
            var document = _hiperNFe.ObterDocumento(accessKey);
            if (document == null)
            {
                MessageBox.Show(this, "Documento não está mais registrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await using var danfeStream = await _hiperNFe.PrintDanfeAsync(document, new DanfePrintOptions { CustomFooter = "Gerado pelo aplicativo de testes" });
            using var reader = new StreamReader(danfeStream, Encoding.UTF8, leaveOpen: false);
            var danfePreview = await reader.ReadToEndAsync();
            Log($"DANFE gerada para {accessKey}:{Environment.NewLine}{danfePreview}");
        });
    }

    private async void sendEmailButton_Click(object? sender, EventArgs e)
    {
        var accessKey = GetSelectedAccessKey();
        if (accessKey == null)
        {
            return;
        }

        var recipient = emailTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(recipient))
        {
            MessageBox.Show(this, "Informe um endereço de e-mail válido.", "Dados obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var document = _hiperNFe.ObterDocumento(accessKey);
        if (document == null)
        {
            MessageBox.Show(this, "Documento não está mais registrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var email = new EmailMessage
        {
            From = "nfe@empresa.com",
            Subject = $"NF-e {accessKey} disponível",
            Body = $"Prezados, segue a NF-e {accessKey} emitida para {document.Recipient.CorporateName}.",
            IsBodyHtml = false
        };
        email.To.Add(recipient);

        await ExecuteAsync(sendEmailButton, async () =>
        {
            await _hiperNFe.EnviarEmailAsync(accessKey, email);
        });
    }

    private void viewEmailsButton_Click(object? sender, EventArgs e)
    {
        if (!_emailService.SentEmails.Any())
        {
            Log("Nenhum e-mail foi enviado ainda.");
            return;
        }

        foreach (var (message, index) in _emailService.SentEmails.Select((m, i) => (m, i + 1)))
        {
            Log($"E-mail #{index} para {string.Join(", ", message.To)} - Assunto: {message.Subject}. Anexos: {message.Attachments.Count}.");
        }
    }

    private void documentsListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateDocumentButtons();
    }

    private void RefreshDocumentList()
    {
        var selectedKey = GetSelectedAccessKey();
        documentsListBox.BeginUpdate();
        documentsListBox.Items.Clear();
        foreach (var document in _hiperNFe.Documentos.OrderBy(d => d.IssueDate))
        {
            documentsListBox.Items.Add(new DocumentListItem(document));
        }
        documentsListBox.EndUpdate();

        if (selectedKey != null)
        {
            for (var i = 0; i < documentsListBox.Items.Count; i++)
            {
                if (documentsListBox.Items[i] is DocumentListItem item && item.AccessKey == selectedKey)
                {
                    documentsListBox.SelectedIndex = i;
                    break;
                }
            }
        }

        UpdateDocumentButtons();
    }

    private string? GetSelectedAccessKey()
    {
        if (documentsListBox.SelectedItem is DocumentListItem item)
        {
            return item.AccessKey;
        }

        return null;
    }

    private void UpdateDocumentButtons()
    {
        var hasSelection = documentsListBox.SelectedItem is not null;
        authorizeButton.Enabled = hasSelection;
        cancelButton.Enabled = hasSelection;
        removeButton.Enabled = hasSelection;
        downloadXmlButton.Enabled = hasSelection;
        printDanfeButton.Enabled = hasSelection;
        sendEmailButton.Enabled = hasSelection;
    }

    private NFeDocument CreateDocument(string accessKey, string emitterName, string recipientName, decimal totalValue)
    {
        var sequence = _hiperNFe.Documentos.Count + 1;
        return new NFeDocument
        {
            AccessKey = accessKey,
            Series = "1",
            Number = sequence.ToString(CultureInfo.InvariantCulture),
            IssueDate = DateTime.Now,
            Emitter = new NFeEmitter
            {
                CorporateName = emitterName,
                TradeName = emitterName,
                Cnpj = "12345678000190",
                Address = new NFeAddress
                {
                    Street = "Rua do Emitente",
                    Number = "100",
                    Neighborhood = "Centro",
                    City = "São Paulo",
                    State = ufTextBox.Text.Trim().ToUpperInvariant(),
                    ZipCode = "01001000"
                }
            },
            Recipient = new NFeRecipient
            {
                CorporateName = recipientName,
                Cnpj = "10987654000199",
                Email = emailTextBox.Text.Trim(),
                Address = new NFeAddress
                {
                    Street = "Avenida do Cliente",
                    Number = "200",
                    Neighborhood = "Centro",
                    City = "São Paulo",
                    State = ufTextBox.Text.Trim().ToUpperInvariant(),
                    ZipCode = "01002000"
                }
            },
            Items =
            {
                new NFeItem
                {
                    LineNumber = 1,
                    Code = "PROD001",
                    Description = "Produto demonstrativo",
                    Ncm = "61091000",
                    Cfop = "5102",
                    Unit = "UN",
                    Quantity = 1,
                    UnitPrice = decimal.Round(totalValue, 2)
                }
            },
            Totals = new NFeTotals
            {
                TotalProducts = decimal.Round(totalValue, 2),
                TotalInvoice = decimal.Round(totalValue, 2)
            },
            Payment = new NFePayment
            {
                PaymentMethod = "01",
                Amount = decimal.Round(totalValue, 2)
            },
            AdditionalInformation = "Documento criado pelo aplicativo de testes da HiperNFe."
        };
    }

    private async Task ExecuteAsync(Control control, Func<Task> action)
    {
        try
        {
            control.Enabled = false;
            await action();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        finally
        {
            control.Enabled = true;
        }
    }

    private void Log(string message)
    {
        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<string>(Log), message);
            return;
        }

        var formatted = $"[{DateTime.Now:HH:mm:ss}] {message}";
        logTextBox.AppendText(formatted + Environment.NewLine);
    }

    private void ShowError(Exception ex)
    {
        var message = ex.Message;
        Log($"Erro: {message}");
        MessageBox.Show(this, message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private string GenerateAccessKey()
    {
        Span<char> digits = stackalloc char[44];
        for (var i = 0; i < digits.Length; i++)
        {
            digits[i] = (char)('0' + _random.Next(0, 10));
        }

        return new string(digits);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _hiperNFe.Dispose();
        base.OnFormClosed(e);
    }

    private sealed class DocumentListItem
    {
        public DocumentListItem(NFeDocument document)
        {
            AccessKey = document.AccessKey;
            Display = $"{document.Number} - {document.Recipient.CorporateName} ({document.Totals.TotalInvoice:C})";
        }

        public string AccessKey { get; }
        private string Display { get; }

        public override string ToString() => Display;
    }
}
