using Aplication.DTO;
using Aplication.Interfaces;
using Aplication.Service;
using Aplication.UseCase;
using Infra.Repository;

namespace RoboConferenciaSat
{
    public partial class Form1 : Form
    {
        private readonly EmpresasRepository _empresasRepository;
        private readonly LoginUseCase2 _login;
        private readonly SeleniumService _selenium;
        private readonly PostLoginUseCase _postLoginUseCase;
        private readonly IApiService _apiService;
        private DadosConferencia _dados;

        public Form1(EmpresasRepository empresasRepository, SeleniumService selenium,
                      LoginUseCase2 login, PostLoginUseCase postLoginUseCase, IApiService apiService)
        {
            InitializeComponent();
            _empresasRepository = empresasRepository;
            _login = login;
            _selenium = selenium;
            _postLoginUseCase = postLoginUseCase;
            _apiService = apiService;

            dgvEmpresasDfe.AutoGenerateColumns = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var dados = new DadosConferencia()
            {
                DataInicial = mtxtDataInicial.Text,
                DataFinal = mtxtDataFinal.Text,
                AnoReferencia = txtAnoReferencia.Text,
                MesReferencia = txtMesReferencia.Text,
                Cnpj = txtCnpj.Text,
                Email = txtEmail.Text,
                Senha = txtSenha.Text,
                NomeEmpresa = txtNomeEmpresa.Text,
            };

            var usuario = await _postLoginUseCase.Execute(dados);
            dgvEmpresasDfe.DataSource = usuario.Empresas;
            dgvEmpresasDfe.Refresh();

            var realizarConferencia = new RealizarConferenciaUseCase(dados);
            _ = Task.Run(async () => await realizarConferencia.ExecuteAsync(dados));
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
                DataInicial = mtxtDataInicial.Text,
                DataFinal = mtxtDataFinal.Text,
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
    }
}
