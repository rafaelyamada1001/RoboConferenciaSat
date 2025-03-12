﻿namespace RoboConferenciaSat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblUsuario = new Label();
            lblSenha = new Label();
            txtSenha = new TextBox();
            txtEmail = new TextBox();
            btnLogin = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtMesReferencia = new TextBox();
            txtAnoReferencia = new TextBox();
            grpDadosConferencia = new GroupBox();
            label6 = new Label();
            txtNomeEmpresa = new TextBox();
            mtxtDataFinal = new MaskedTextBox();
            mtxtDataInicial = new MaskedTextBox();
            label5 = new Label();
            txtCnpj = new TextBox();
            dgvEmpresasDfe = new DataGridView();
            Cnpj = new DataGridViewTextBoxColumn();
            NomeEmpresa = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            grpDadosConferencia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpresasDfe).BeginInit();
            SuspendLayout();
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(6, 332);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(53, 15);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuário :";
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Location = new Point(12, 361);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(45, 15);
            lblSenha.TabIndex = 1;
            lblSenha.Text = "Senha :";
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(63, 353);
            txtSenha.Name = "txtSenha";
            txtSenha.ReadOnly = true;
            txtSenha.Size = new Size(185, 23);
            txtSenha.TabIndex = 2;
            txtSenha.Text = "raf1713";
            txtSenha.UseSystemPasswordChar = true;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(63, 324);
            txtEmail.Name = "txtEmail";
            txtEmail.ReadOnly = true;
            txtEmail.Size = new Size(185, 23);
            txtEmail.TabIndex = 3;
            txtEmail.Text = "rafael.yamada@fourlions.com.br";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(179, 393);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 23);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 21);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 7;
            label1.Text = "Data Inicial :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(154, 21);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 8;
            label2.Text = "Data Final :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 77);
            label3.Name = "label3";
            label3.Size = new Size(110, 15);
            label3.TabIndex = 9;
            label3.Text = "Mês De Referência :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(154, 77);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 10;
            label4.Text = "Ano Referência :";
            // 
            // txtMesReferencia
            // 
            txtMesReferencia.Location = new Point(25, 95);
            txtMesReferencia.Name = "txtMesReferencia";
            txtMesReferencia.Size = new Size(100, 23);
            txtMesReferencia.TabIndex = 11;
            txtMesReferencia.Text = "03";
            // 
            // txtAnoReferencia
            // 
            txtAnoReferencia.Location = new Point(154, 95);
            txtAnoReferencia.Name = "txtAnoReferencia";
            txtAnoReferencia.Size = new Size(100, 23);
            txtAnoReferencia.TabIndex = 12;
            txtAnoReferencia.Text = "25";
            // 
            // grpDadosConferencia
            // 
            grpDadosConferencia.Controls.Add(label6);
            grpDadosConferencia.Controls.Add(txtNomeEmpresa);
            grpDadosConferencia.Controls.Add(mtxtDataFinal);
            grpDadosConferencia.Controls.Add(mtxtDataInicial);
            grpDadosConferencia.Controls.Add(label5);
            grpDadosConferencia.Controls.Add(txtCnpj);
            grpDadosConferencia.Controls.Add(txtMesReferencia);
            grpDadosConferencia.Controls.Add(btnLogin);
            grpDadosConferencia.Controls.Add(txtEmail);
            grpDadosConferencia.Controls.Add(txtSenha);
            grpDadosConferencia.Controls.Add(label2);
            grpDadosConferencia.Controls.Add(lblSenha);
            grpDadosConferencia.Controls.Add(txtAnoReferencia);
            grpDadosConferencia.Controls.Add(lblUsuario);
            grpDadosConferencia.Controls.Add(label3);
            grpDadosConferencia.Controls.Add(label1);
            grpDadosConferencia.Controls.Add(label4);
            grpDadosConferencia.Location = new Point(633, 16);
            grpDadosConferencia.Name = "grpDadosConferencia";
            grpDadosConferencia.Size = new Size(259, 422);
            grpDadosConferencia.TabIndex = 13;
            grpDadosConferencia.TabStop = false;
            grpDadosConferencia.Text = "Dados Conferência";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(25, 199);
            label6.Name = "label6";
            label6.Size = new Size(94, 15);
            label6.TabIndex = 18;
            label6.Text = "Nome Empresa :";
            // 
            // txtNomeEmpresa
            // 
            txtNomeEmpresa.Location = new Point(25, 217);
            txtNomeEmpresa.Name = "txtNomeEmpresa";
            txtNomeEmpresa.ReadOnly = true;
            txtNomeEmpresa.Size = new Size(229, 23);
            txtNomeEmpresa.TabIndex = 17;
            // 
            // mtxtDataFinal
            // 
            mtxtDataFinal.Location = new Point(154, 39);
            mtxtDataFinal.Mask = "00/00/0000";
            mtxtDataFinal.Name = "mtxtDataFinal";
            mtxtDataFinal.Size = new Size(100, 23);
            mtxtDataFinal.TabIndex = 16;
            mtxtDataFinal.Text = "09032025";
            mtxtDataFinal.ValidatingType = typeof(DateTime);
            // 
            // mtxtDataInicial
            // 
            mtxtDataInicial.Location = new Point(25, 39);
            mtxtDataInicial.Mask = "00/00/0000";
            mtxtDataInicial.Name = "mtxtDataInicial";
            mtxtDataInicial.Size = new Size(100, 23);
            mtxtDataInicial.TabIndex = 15;
            mtxtDataInicial.Text = "01032025";
            mtxtDataInicial.ValidatingType = typeof(DateTime);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 146);
            label5.Name = "label5";
            label5.Size = new Size(38, 15);
            label5.TabIndex = 14;
            label5.Text = "Cnpj :";
            // 
            // txtCnpj
            // 
            txtCnpj.Location = new Point(25, 164);
            txtCnpj.Name = "txtCnpj";
            txtCnpj.Size = new Size(229, 23);
            txtCnpj.TabIndex = 13;
            // 
            // dgvEmpresasDfe
            // 
            dgvEmpresasDfe.AllowUserToAddRows = false;
            dgvEmpresasDfe.AllowUserToDeleteRows = false;
            dgvEmpresasDfe.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmpresasDfe.Columns.AddRange(new DataGridViewColumn[] { Cnpj, NomeEmpresa, Status });
            dgvEmpresasDfe.Location = new Point(12, 12);
            dgvEmpresasDfe.Name = "dgvEmpresasDfe";
            dgvEmpresasDfe.ReadOnly = true;
            dgvEmpresasDfe.Size = new Size(589, 426);
            dgvEmpresasDfe.TabIndex = 14;
            dgvEmpresasDfe.CellMouseClick += dgvEmpresasDfe_CellMouseClick;
            // 
            // Cnpj
            // 
            Cnpj.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Cnpj.DataPropertyName = "CNPJ";
            Cnpj.HeaderText = "Cnpj";
            Cnpj.Name = "Cnpj";
            Cnpj.ReadOnly = true;
            Cnpj.Width = 57;
            // 
            // NomeEmpresa
            // 
            NomeEmpresa.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NomeEmpresa.DataPropertyName = "Nome";
            NomeEmpresa.HeaderText = "Nome Empresa";
            NomeEmpresa.Name = "NomeEmpresa";
            NomeEmpresa.ReadOnly = true;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(904, 450);
            Controls.Add(dgvEmpresasDfe);
            Controls.Add(grpDadosConferencia);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            grpDadosConferencia.ResumeLayout(false);
            grpDadosConferencia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmpresasDfe).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblUsuario;
        private Label lblSenha;
        private TextBox txtSenha;
        private TextBox txtEmail;
        private Button btnLogin;
        private TextBox txtDataInicialDfe;
        private TextBox txtDataFinalDfe;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtMesReferencia;
        private TextBox txtAnoReferencia;
        private GroupBox grpDadosConferencia;
        private DataGridView dgvEmpresasDfe;
        private TextBox txtCnpj;
        private Label label5;
        private MaskedTextBox mtxtDataInicial;
        private MaskedTextBox mtxtDataFinal;
        private Label label6;
        private TextBox txtNomeEmpresa;
        private DataGridViewTextBoxColumn Cnpj;
        private DataGridViewTextBoxColumn NomeEmpresa;
        private DataGridViewTextBoxColumn Status;
    }
}
