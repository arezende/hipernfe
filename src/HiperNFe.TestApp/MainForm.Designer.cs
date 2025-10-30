using System.Windows.Forms;

namespace HiperNFe.TestApp;

partial class MainForm
{
    private TableLayoutPanel mainLayout;
    private FlowLayoutPanel headerPanel;
    private GroupBox documentGroupBox;
    private TableLayoutPanel documentLayout;
    private Label accessKeyLabel;
    private TextBox accessKeyTextBox;
    private Button generateKeyButton;
    private Label emitterLabel;
    private TextBox emitterTextBox;
    private Label recipientLabel;
    private TextBox recipientTextBox;
    private Label totalLabel;
    private NumericUpDown totalNumericUpDown;
    private Button addDocumentButton;
    private GroupBox statusGroupBox;
    private TableLayoutPanel statusLayout;
    private Label ufLabel;
    private TextBox ufTextBox;
    private Button consultarStatusButton;
    private GroupBox emailGroupBox;
    private TableLayoutPanel emailLayout;
    private Label emailLabel;
    private TextBox emailTextBox;
    private Button sendEmailButton;
    private Button viewEmailsButton;
    private GroupBox documentsGroupBox;
    private TableLayoutPanel documentsLayout;
    private ListBox documentsListBox;
    private FlowLayoutPanel documentButtonsPanel;
    private Button authorizeButton;
    private Button authorizeAllButton;
    private Button cancelButton;
    private Button removeButton;
    private Button downloadXmlButton;
    private Button printDanfeButton;
    private GroupBox logGroupBox;
    private TextBox logTextBox;

    /// <summary>
    ///  Limpa os recursos que estão sendo usados.
    /// </summary>
    /// <param name="disposing">true se os recursos gerenciados devem ser descartados; caso contrário, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Código gerado pelo Designer

    private System.ComponentModel.IContainer? components;

    /// <summary>
    ///  Método necessário para suporte ao Designer - não modifique
    ///  o conteúdo deste método com o editor de código.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        mainLayout = new TableLayoutPanel();
        headerPanel = new FlowLayoutPanel();
        documentGroupBox = new GroupBox();
        documentLayout = new TableLayoutPanel();
        accessKeyLabel = new Label();
        accessKeyTextBox = new TextBox();
        generateKeyButton = new Button();
        emitterLabel = new Label();
        emitterTextBox = new TextBox();
        recipientLabel = new Label();
        recipientTextBox = new TextBox();
        totalLabel = new Label();
        totalNumericUpDown = new NumericUpDown();
        addDocumentButton = new Button();
        statusGroupBox = new GroupBox();
        statusLayout = new TableLayoutPanel();
        ufLabel = new Label();
        ufTextBox = new TextBox();
        consultarStatusButton = new Button();
        emailGroupBox = new GroupBox();
        emailLayout = new TableLayoutPanel();
        emailLabel = new Label();
        emailTextBox = new TextBox();
        sendEmailButton = new Button();
        viewEmailsButton = new Button();
        documentsGroupBox = new GroupBox();
        documentsLayout = new TableLayoutPanel();
        documentsListBox = new ListBox();
        documentButtonsPanel = new FlowLayoutPanel();
        authorizeButton = new Button();
        authorizeAllButton = new Button();
        cancelButton = new Button();
        removeButton = new Button();
        downloadXmlButton = new Button();
        printDanfeButton = new Button();
        logGroupBox = new GroupBox();
        logTextBox = new TextBox();
        mainLayout.SuspendLayout();
        headerPanel.SuspendLayout();
        documentGroupBox.SuspendLayout();
        documentLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)totalNumericUpDown).BeginInit();
        statusGroupBox.SuspendLayout();
        statusLayout.SuspendLayout();
        emailGroupBox.SuspendLayout();
        emailLayout.SuspendLayout();
        documentsGroupBox.SuspendLayout();
        documentsLayout.SuspendLayout();
        documentButtonsPanel.SuspendLayout();
        logGroupBox.SuspendLayout();
        SuspendLayout();
        // 
        // mainLayout
        // 
        mainLayout.ColumnCount = 1;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(headerPanel, 0, 0);
        mainLayout.Controls.Add(documentsGroupBox, 0, 1);
        mainLayout.Controls.Add(logGroupBox, 0, 2);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new System.Drawing.Point(0, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.RowCount = 3;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        mainLayout.Size = new System.Drawing.Size(984, 661);
        mainLayout.TabIndex = 0;
        // 
        // headerPanel
        // 
        headerPanel.AutoSize = true;
        headerPanel.Controls.Add(documentGroupBox);
        headerPanel.Controls.Add(statusGroupBox);
        headerPanel.Controls.Add(emailGroupBox);
        headerPanel.Dock = DockStyle.Fill;
        headerPanel.Location = new System.Drawing.Point(3, 3);
        headerPanel.Name = "headerPanel";
        headerPanel.Size = new System.Drawing.Size(978, 214);
        headerPanel.TabIndex = 0;
        headerPanel.WrapContents = false;
        // 
        // documentGroupBox
        // 
        documentGroupBox.Controls.Add(documentLayout);
        documentGroupBox.Location = new System.Drawing.Point(3, 3);
        documentGroupBox.Name = "documentGroupBox";
        documentGroupBox.Size = new System.Drawing.Size(420, 208);
        documentGroupBox.TabIndex = 0;
        documentGroupBox.TabStop = false;
        documentGroupBox.Text = "Novo documento";
        // 
        // documentLayout
        // 
        documentLayout.ColumnCount = 3;
        documentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        documentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        documentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        documentLayout.Controls.Add(accessKeyLabel, 0, 0);
        documentLayout.Controls.Add(accessKeyTextBox, 1, 0);
        documentLayout.Controls.Add(generateKeyButton, 2, 0);
        documentLayout.Controls.Add(emitterLabel, 0, 1);
        documentLayout.Controls.Add(emitterTextBox, 1, 1);
        documentLayout.SetColumnSpan(emitterTextBox, 2);
        documentLayout.Controls.Add(recipientLabel, 0, 2);
        documentLayout.Controls.Add(recipientTextBox, 1, 2);
        documentLayout.SetColumnSpan(recipientTextBox, 2);
        documentLayout.Controls.Add(totalLabel, 0, 3);
        documentLayout.Controls.Add(totalNumericUpDown, 1, 3);
        documentLayout.SetColumnSpan(totalNumericUpDown, 2);
        documentLayout.Controls.Add(addDocumentButton, 1, 4);
        documentLayout.SetColumnSpan(addDocumentButton, 2);
        documentLayout.Dock = DockStyle.Fill;
        documentLayout.Location = new System.Drawing.Point(3, 19);
        documentLayout.Name = "documentLayout";
        documentLayout.RowCount = 5;
        documentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        documentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        documentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        documentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        documentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        documentLayout.Size = new System.Drawing.Size(414, 186);
        documentLayout.TabIndex = 0;
        // 
        // accessKeyLabel
        // 
        accessKeyLabel.AutoSize = true;
        accessKeyLabel.Dock = DockStyle.Fill;
        accessKeyLabel.Location = new System.Drawing.Point(3, 0);
        accessKeyLabel.Name = "accessKeyLabel";
        accessKeyLabel.Size = new System.Drawing.Size(84, 32);
        accessKeyLabel.TabIndex = 0;
        accessKeyLabel.Text = "Chave de acesso";
        accessKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // accessKeyTextBox
        // 
        accessKeyTextBox.Dock = DockStyle.Fill;
        accessKeyTextBox.Location = new System.Drawing.Point(93, 3);
        accessKeyTextBox.Name = "accessKeyTextBox";
        accessKeyTextBox.Size = new System.Drawing.Size(208, 23);
        accessKeyTextBox.TabIndex = 1;
        // 
        // generateKeyButton
        // 
        generateKeyButton.Dock = DockStyle.Fill;
        generateKeyButton.Location = new System.Drawing.Point(307, 3);
        generateKeyButton.Name = "generateKeyButton";
        generateKeyButton.Size = new System.Drawing.Size(104, 26);
        generateKeyButton.TabIndex = 2;
        generateKeyButton.Text = "Gerar chave";
        generateKeyButton.UseVisualStyleBackColor = true;
        generateKeyButton.Click += generateKeyButton_Click;
        // 
        // emitterLabel
        // 
        emitterLabel.AutoSize = true;
        emitterLabel.Dock = DockStyle.Fill;
        emitterLabel.Location = new System.Drawing.Point(3, 32);
        emitterLabel.Name = "emitterLabel";
        emitterLabel.Size = new System.Drawing.Size(84, 32);
        emitterLabel.TabIndex = 3;
        emitterLabel.Text = "Emitente";
        emitterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // emitterTextBox
        // 
        emitterTextBox.Dock = DockStyle.Fill;
        emitterTextBox.Location = new System.Drawing.Point(93, 35);
        emitterTextBox.Name = "emitterTextBox";
        emitterTextBox.Size = new System.Drawing.Size(318, 23);
        emitterTextBox.TabIndex = 4;
        // 
        // recipientLabel
        // 
        recipientLabel.AutoSize = true;
        recipientLabel.Dock = DockStyle.Fill;
        recipientLabel.Location = new System.Drawing.Point(3, 64);
        recipientLabel.Name = "recipientLabel";
        recipientLabel.Size = new System.Drawing.Size(84, 32);
        recipientLabel.TabIndex = 5;
        recipientLabel.Text = "Destinatário";
        recipientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // recipientTextBox
        // 
        recipientTextBox.Dock = DockStyle.Fill;
        recipientTextBox.Location = new System.Drawing.Point(93, 67);
        recipientTextBox.Name = "recipientTextBox";
        recipientTextBox.Size = new System.Drawing.Size(318, 23);
        recipientTextBox.TabIndex = 6;
        // 
        // totalLabel
        // 
        totalLabel.AutoSize = true;
        totalLabel.Dock = DockStyle.Fill;
        totalLabel.Location = new System.Drawing.Point(3, 96);
        totalLabel.Name = "totalLabel";
        totalLabel.Size = new System.Drawing.Size(84, 32);
        totalLabel.TabIndex = 7;
        totalLabel.Text = "Valor total";
        totalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // totalNumericUpDown
        // 
        totalNumericUpDown.DecimalPlaces = 2;
        totalNumericUpDown.Dock = DockStyle.Fill;
        totalNumericUpDown.Location = new System.Drawing.Point(93, 99);
        totalNumericUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        totalNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
        totalNumericUpDown.Name = "totalNumericUpDown";
        totalNumericUpDown.Size = new System.Drawing.Size(318, 23);
        totalNumericUpDown.TabIndex = 8;
        totalNumericUpDown.Value = new decimal(new int[] { 1500, 0, 0, 65536 });
        // 
        // addDocumentButton
        // 
        addDocumentButton.Dock = DockStyle.Fill;
        addDocumentButton.Location = new System.Drawing.Point(93, 131);
        addDocumentButton.Margin = new Padding(3, 6, 3, 0);
        addDocumentButton.Name = "addDocumentButton";
        addDocumentButton.Size = new System.Drawing.Size(318, 32);
        addDocumentButton.TabIndex = 9;
        addDocumentButton.Text = "Adicionar documento";
        addDocumentButton.UseVisualStyleBackColor = true;
        addDocumentButton.Click += addDocumentButton_Click;
        // 
        // statusGroupBox
        // 
        statusGroupBox.Controls.Add(statusLayout);
        statusGroupBox.Location = new System.Drawing.Point(429, 3);
        statusGroupBox.Name = "statusGroupBox";
        statusGroupBox.Size = new System.Drawing.Size(180, 208);
        statusGroupBox.TabIndex = 1;
        statusGroupBox.TabStop = false;
        statusGroupBox.Text = "Status da SEFAZ";
        // 
        // statusLayout
        // 
        statusLayout.ColumnCount = 2;
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        statusLayout.Controls.Add(ufLabel, 0, 0);
        statusLayout.Controls.Add(ufTextBox, 1, 0);
        statusLayout.Controls.Add(consultarStatusButton, 0, 1);
        statusLayout.SetColumnSpan(consultarStatusButton, 2);
        statusLayout.Dock = DockStyle.Fill;
        statusLayout.Location = new System.Drawing.Point(3, 19);
        statusLayout.Name = "statusLayout";
        statusLayout.RowCount = 2;
        statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        statusLayout.Size = new System.Drawing.Size(174, 186);
        statusLayout.TabIndex = 0;
        // 
        // ufLabel
        // 
        ufLabel.AutoSize = true;
        ufLabel.Dock = DockStyle.Fill;
        ufLabel.Location = new System.Drawing.Point(3, 0);
        ufLabel.Name = "ufLabel";
        ufLabel.Size = new System.Drawing.Size(54, 32);
        ufLabel.TabIndex = 0;
        ufLabel.Text = "UF";
        ufLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // ufTextBox
        // 
        ufTextBox.CharacterCasing = CharacterCasing.Upper;
        ufTextBox.Dock = DockStyle.Fill;
        ufTextBox.Location = new System.Drawing.Point(63, 3);
        ufTextBox.MaxLength = 2;
        ufTextBox.Name = "ufTextBox";
        ufTextBox.Size = new System.Drawing.Size(108, 23);
        ufTextBox.TabIndex = 1;
        // 
        // consultarStatusButton
        // 
        consultarStatusButton.Dock = DockStyle.Fill;
        consultarStatusButton.Location = new System.Drawing.Point(3, 35);
        consultarStatusButton.Margin = new Padding(3, 3, 3, 0);
        consultarStatusButton.Name = "consultarStatusButton";
        consultarStatusButton.Size = new System.Drawing.Size(168, 32);
        consultarStatusButton.TabIndex = 2;
        consultarStatusButton.Text = "Consultar status";
        consultarStatusButton.UseVisualStyleBackColor = true;
        consultarStatusButton.Click += consultarStatusButton_Click;
        // 
        // emailGroupBox
        // 
        emailGroupBox.Controls.Add(emailLayout);
        emailGroupBox.Location = new System.Drawing.Point(615, 3);
        emailGroupBox.Name = "emailGroupBox";
        emailGroupBox.Size = new System.Drawing.Size(261, 208);
        emailGroupBox.TabIndex = 2;
        emailGroupBox.TabStop = false;
        emailGroupBox.Text = "Envio de e-mail";
        // 
        // emailLayout
        // 
        emailLayout.ColumnCount = 1;
        emailLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        emailLayout.Controls.Add(emailLabel, 0, 0);
        emailLayout.Controls.Add(emailTextBox, 0, 1);
        emailLayout.Controls.Add(sendEmailButton, 0, 2);
        emailLayout.Controls.Add(viewEmailsButton, 0, 3);
        emailLayout.Dock = DockStyle.Fill;
        emailLayout.Location = new System.Drawing.Point(3, 19);
        emailLayout.Name = "emailLayout";
        emailLayout.RowCount = 4;
        emailLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        emailLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        emailLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        emailLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        emailLayout.Size = new System.Drawing.Size(255, 186);
        emailLayout.TabIndex = 0;
        // 
        // emailLabel
        // 
        emailLabel.AutoSize = true;
        emailLabel.Dock = DockStyle.Fill;
        emailLabel.Location = new System.Drawing.Point(3, 0);
        emailLabel.Name = "emailLabel";
        emailLabel.Size = new System.Drawing.Size(249, 15);
        emailLabel.TabIndex = 0;
        emailLabel.Text = "Destinatário";
        // 
        // emailTextBox
        // 
        emailTextBox.Dock = DockStyle.Fill;
        emailTextBox.Location = new System.Drawing.Point(3, 18);
        emailTextBox.Name = "emailTextBox";
        emailTextBox.PlaceholderText = "cliente@empresa.com";
        emailTextBox.Size = new System.Drawing.Size(249, 23);
        emailTextBox.TabIndex = 1;
        // 
        // sendEmailButton
        // 
        sendEmailButton.Dock = DockStyle.Fill;
        sendEmailButton.Location = new System.Drawing.Point(3, 49);
        sendEmailButton.Margin = new Padding(3, 5, 3, 0);
        sendEmailButton.Name = "sendEmailButton";
        sendEmailButton.Size = new System.Drawing.Size(249, 32);
        sendEmailButton.TabIndex = 2;
        sendEmailButton.Text = "Enviar e-mail";
        sendEmailButton.UseVisualStyleBackColor = true;
        sendEmailButton.Click += sendEmailButton_Click;
        // 
        // viewEmailsButton
        // 
        viewEmailsButton.Dock = DockStyle.Fill;
        viewEmailsButton.Location = new System.Drawing.Point(3, 86);
        viewEmailsButton.Margin = new Padding(3, 5, 3, 0);
        viewEmailsButton.Name = "viewEmailsButton";
        viewEmailsButton.Size = new System.Drawing.Size(249, 32);
        viewEmailsButton.TabIndex = 3;
        viewEmailsButton.Text = "Ver e-mails armazenados";
        viewEmailsButton.UseVisualStyleBackColor = true;
        viewEmailsButton.Click += viewEmailsButton_Click;
        // 
        // documentsGroupBox
        // 
        documentsGroupBox.Controls.Add(documentsLayout);
        documentsGroupBox.Dock = DockStyle.Fill;
        documentsGroupBox.Location = new System.Drawing.Point(3, 223);
        documentsGroupBox.Name = "documentsGroupBox";
        documentsGroupBox.Size = new System.Drawing.Size(978, 239);
        documentsGroupBox.TabIndex = 1;
        documentsGroupBox.TabStop = false;
        documentsGroupBox.Text = "Documentos registrados";
        // 
        // documentsLayout
        // 
        documentsLayout.ColumnCount = 2;
        documentsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        documentsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        documentsLayout.Controls.Add(documentsListBox, 0, 0);
        documentsLayout.Controls.Add(documentButtonsPanel, 1, 0);
        documentsLayout.Dock = DockStyle.Fill;
        documentsLayout.Location = new System.Drawing.Point(3, 19);
        documentsLayout.Name = "documentsLayout";
        documentsLayout.RowCount = 1;
        documentsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        documentsLayout.Size = new System.Drawing.Size(972, 217);
        documentsLayout.TabIndex = 0;
        // 
        // documentsListBox
        // 
        documentsListBox.Dock = DockStyle.Fill;
        documentsListBox.FormattingEnabled = true;
        documentsListBox.ItemHeight = 15;
        documentsListBox.Location = new System.Drawing.Point(3, 3);
        documentsListBox.Name = "documentsListBox";
        documentsListBox.Size = new System.Drawing.Size(746, 211);
        documentsListBox.TabIndex = 0;
        documentsListBox.SelectedIndexChanged += documentsListBox_SelectedIndexChanged;
        // 
        // documentButtonsPanel
        // 
        documentButtonsPanel.Controls.Add(authorizeButton);
        documentButtonsPanel.Controls.Add(authorizeAllButton);
        documentButtonsPanel.Controls.Add(cancelButton);
        documentButtonsPanel.Controls.Add(removeButton);
        documentButtonsPanel.Controls.Add(downloadXmlButton);
        documentButtonsPanel.Controls.Add(printDanfeButton);
        documentButtonsPanel.Dock = DockStyle.Fill;
        documentButtonsPanel.FlowDirection = FlowDirection.TopDown;
        documentButtonsPanel.Location = new System.Drawing.Point(755, 3);
        documentButtonsPanel.Name = "documentButtonsPanel";
        documentButtonsPanel.Size = new System.Drawing.Size(214, 211);
        documentButtonsPanel.TabIndex = 1;
        documentButtonsPanel.WrapContents = false;
        // 
        // authorizeButton
        // 
        authorizeButton.Location = new System.Drawing.Point(3, 3);
        authorizeButton.Name = "authorizeButton";
        authorizeButton.Size = new System.Drawing.Size(206, 30);
        authorizeButton.TabIndex = 0;
        authorizeButton.Text = "Emitir selecionado";
        authorizeButton.UseVisualStyleBackColor = true;
        authorizeButton.Click += authorizeButton_Click;
        // 
        // authorizeAllButton
        // 
        authorizeAllButton.Location = new System.Drawing.Point(3, 39);
        authorizeAllButton.Name = "authorizeAllButton";
        authorizeAllButton.Size = new System.Drawing.Size(206, 30);
        authorizeAllButton.TabIndex = 1;
        authorizeAllButton.Text = "Emitir todos";
        authorizeAllButton.UseVisualStyleBackColor = true;
        authorizeAllButton.Click += authorizeAllButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Location = new System.Drawing.Point(3, 75);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new System.Drawing.Size(206, 30);
        cancelButton.TabIndex = 2;
        cancelButton.Text = "Cancelar selecionado";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // removeButton
        // 
        removeButton.Location = new System.Drawing.Point(3, 111);
        removeButton.Name = "removeButton";
        removeButton.Size = new System.Drawing.Size(206, 30);
        removeButton.TabIndex = 3;
        removeButton.Text = "Remover da fila";
        removeButton.UseVisualStyleBackColor = true;
        removeButton.Click += removeButton_Click;
        // 
        // downloadXmlButton
        // 
        downloadXmlButton.Location = new System.Drawing.Point(3, 147);
        downloadXmlButton.Name = "downloadXmlButton";
        downloadXmlButton.Size = new System.Drawing.Size(206, 30);
        downloadXmlButton.TabIndex = 4;
        downloadXmlButton.Text = "Baixar XML";
        downloadXmlButton.UseVisualStyleBackColor = true;
        downloadXmlButton.Click += downloadXmlButton_Click;
        // 
        // printDanfeButton
        // 
        printDanfeButton.Location = new System.Drawing.Point(3, 183);
        printDanfeButton.Name = "printDanfeButton";
        printDanfeButton.Size = new System.Drawing.Size(206, 30);
        printDanfeButton.TabIndex = 5;
        printDanfeButton.Text = "Imprimir DANFE";
        printDanfeButton.UseVisualStyleBackColor = true;
        printDanfeButton.Click += printDanfeButton_Click;
        // 
        // logGroupBox
        // 
        logGroupBox.Controls.Add(logTextBox);
        logGroupBox.Dock = DockStyle.Fill;
        logGroupBox.Location = new System.Drawing.Point(3, 468);
        logGroupBox.Name = "logGroupBox";
        logGroupBox.Size = new System.Drawing.Size(978, 190);
        logGroupBox.TabIndex = 2;
        logGroupBox.TabStop = false;
        logGroupBox.Text = "Registro de operações";
        // 
        // logTextBox
        // 
        logTextBox.Dock = DockStyle.Fill;
        logTextBox.Location = new System.Drawing.Point(3, 19);
        logTextBox.Multiline = true;
        logTextBox.Name = "logTextBox";
        logTextBox.ReadOnly = true;
        logTextBox.ScrollBars = ScrollBars.Vertical;
        logTextBox.Size = new System.Drawing.Size(972, 168);
        logTextBox.TabIndex = 0;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(984, 661);
        Controls.Add(mainLayout);
        MinimumSize = new System.Drawing.Size(1000, 700);
        Name = "MainForm";
        Text = "HiperNFe - Aplicativo de Testes";
        Load += MainForm_Load;
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        headerPanel.ResumeLayout(false);
        documentGroupBox.ResumeLayout(false);
        documentLayout.ResumeLayout(false);
        documentLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)totalNumericUpDown).EndInit();
        statusGroupBox.ResumeLayout(false);
        statusLayout.ResumeLayout(false);
        statusLayout.PerformLayout();
        emailGroupBox.ResumeLayout(false);
        emailLayout.ResumeLayout(false);
        emailLayout.PerformLayout();
        documentsGroupBox.ResumeLayout(false);
        documentsLayout.ResumeLayout(false);
        documentButtonsPanel.ResumeLayout(false);
        logGroupBox.ResumeLayout(false);
        logGroupBox.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
}
