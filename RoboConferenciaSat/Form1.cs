using RoboConferenciaSat.Aplicacao;

namespace RoboConferenciaSat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var login = new Login();
            login.LoginDfe();
        }
    }
}
