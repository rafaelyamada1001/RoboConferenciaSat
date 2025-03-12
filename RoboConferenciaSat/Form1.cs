using Aplication.DTO;
using Aplication.LoginUseCase;
using Aplication.UseCase;
using Infra.Repository;

namespace RoboConferenciaSat
{
    public partial class Form1 : Form
    {
        private readonly EmpresasRepository _empresasRepository;
        private readonly LoginUseCase2 _login;
        private readonly RealizarConferenciaUseCase _realizarConferenciaUseCase;
        public Form1()
        {
            InitializeComponent();
            _empresasRepository = new EmpresasRepository();
            _login = new LoginUseCase2();
            _realizarConferenciaUseCase = new RealizarConferenciaUseCase();

            dgvEmpresasDfe.AutoGenerateColumns = false;
            dgvEmpresasDfe.DataSource = _empresasRepository.ObterEmpresas().ToList();
            dgvEmpresasDfe.Refresh();
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
            };

            //_login.Execute(dados);
            var realizarConferencia = new RealizarConferenciaUseCase();
            _ = Task.Run(async () => await _realizarConferenciaUseCase.ExecuteAsync(dados));
        }

        private void dgvEmpresasDfe_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cnpj = dgvEmpresasDfe.Rows[e.RowIndex];
                txtCnpj.Text = cnpj.Cells[0].Value.ToString();              
            }
        }
    }
}
