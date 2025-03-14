using Aplication.DTO;
using Aplication.Interfaces;
using Aplication.Service;
using Aplication.UseCase;
using Infra.Repository;

namespace RoboConferenciaSat
{
    public partial class Form1 : Form
    {


        private readonly SeleniumService _selenium;
        private readonly PostLoginUseCase _postLoginUseCase;
        private readonly IApiService _apiService;
        private DadosConferencia _dados;
        private readonly RodarConferenciaTodasEmpresas _rodarConferencia;

        public Form1(SeleniumService selenium, 
                      PostLoginUseCase postLoginUseCase, 
                      IApiService apiService,
                      RodarConferenciaTodasEmpresas rodarConferencia)
        {
            InitializeComponent();
            _selenium = selenium;
            _postLoginUseCase = postLoginUseCase;
            _apiService = apiService;
            _rodarConferencia = rodarConferencia;

            dgvEmpresasDfe.AutoGenerateColumns = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var dados = new DadosConferencia()
            {
                DataInicialDfe = mtxtDataInicialDfe.Text,
                DataFinalDfe = mtxtDataFinalDfe.Text,
                DataInicialSefaz = mtxtDataInicialSefaz.Text,
                DataFinalSefaz = mtxtDataFinalSefaz.Text,
                AnoReferencia = txtAnoReferencia.Text,
                MesReferencia = txtMesReferencia.Text,
                Cnpj = txtCnpj.Text,
                Email = txtEmail.Text,
                Senha = txtSenha.Text,
                NomeEmpresa = txtNomeEmpresa.Text,
            };

            var usuario = await _postLoginUseCase.ExecuteAsync(dados);
            dgvEmpresasDfe.DataSource = usuario.Empresas;
            dgvEmpresasDfe.Refresh();
        }

        private void dgvEmpresasDfe_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvEmpresasDfe.Rows[e.RowIndex];
                txtCnpj.Text = row.Cells[0].Value.ToString();
                txtNomeEmpresa.Text = row.Cells[1].Value.ToString();
            }
        }

        private void btnIniciarConferencia_Click(object sender, EventArgs e)
        {
            var dados = new DadosConferencia()
            {
                DataInicialDfe = mtxtDataInicialDfe.Text,
                DataFinalDfe = mtxtDataFinalDfe.Text,
                DataInicialSefaz = mtxtDataInicialSefaz.Text,
                DataFinalSefaz = mtxtDataFinalSefaz.Text,
                AnoReferencia = txtAnoReferencia.Text,
                MesReferencia = txtMesReferencia.Text,
                Cnpj = txtCnpj.Text,
                Email = txtEmail.Text,
                Senha = txtSenha.Text,
                NomeEmpresa = txtNomeEmpresa.Text,
            };
            var realizarConferencia = new RealizarConferenciaUseCase(dados);
            _ = realizarConferencia.ExecuteAsync(dados);
        }

        private async void btnConferenciaTodasEmpresas_Click(object sender, EventArgs e)
        {
            var dados = new DadosConferencia()
            {
                DataInicialDfe = mtxtDataInicialDfe.Text,
                DataFinalDfe = mtxtDataFinalDfe.Text,
                DataInicialSefaz = mtxtDataInicialSefaz.Text,
                DataFinalSefaz = mtxtDataFinalSefaz.Text,
                AnoReferencia = txtAnoReferencia.Text,
                MesReferencia = txtMesReferencia.Text,
                Cnpj = txtCnpj.Text,
                Email = txtEmail.Text,
                Senha = txtSenha.Text,
                NomeEmpresa = txtNomeEmpresa.Text,
            };

            var usuario = await _postLoginUseCase.ExecuteAsync(dados);
            dgvEmpresasDfe.DataSource = usuario.Empresas;
            dgvEmpresasDfe.Refresh();

            btnConferenciaTodasEmpresas.Enabled = false;
            try
            {
                await _rodarConferencia.ExecuteAsync(usuario.Empresas, dados);
                MessageBox.Show("Conferências finalizadas!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao rodar conferências: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConferenciaTodasEmpresas.Enabled = true; // Habilita o botão novamente
            }
        }
    }
}
