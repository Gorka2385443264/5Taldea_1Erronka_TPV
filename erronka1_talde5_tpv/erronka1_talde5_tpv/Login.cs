using NHibernate;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            // Establecer el formulario en pantalla completa
            this.WindowState = FormWindowState.Maximized;
        }

        private NHibernate.Cfg.Configuration miConfiguracion;
        private ISessionFactory mySessionFactory;
        private ISession mySession;

        private void Login_Load(object sender, EventArgs e)
        {
            // Centrar los controles
            CenterControls();

            logInButton.BackColor = Color.FromArgb(118, 138, 153);
            this.BackColor = Color.FromArgb(52, 90, 123);

            pasahitzaText.PasswordChar = '*';
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailText.Text) || string.IsNullOrWhiteSpace(pasahitzaText.Text))
            {
                MessageBox.Show("Por favor, rellena ambos campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            miConfiguracion = new NHibernate.Cfg.Configuration();
            miConfiguracion.Configure();
            mySessionFactory = miConfiguracion.BuildSessionFactory();
            mySession = mySessionFactory.OpenSession();

            using (var transaccion = mySession.BeginTransaction())
            {
                try
                {
                    // Consulta HQL para verificar el email y la contraseña
                    string hql = @"FROM Langilea WHERE Email = :emailParam AND Pasahitza = :pasahitzaParam";

                    var query = mySession.CreateQuery(hql);
                    query.SetParameter("emailParam", emailText.Text);
                    query.SetParameter("pasahitzaParam", pasahitzaText.Text);

                    var resultado = query.UniqueResult<erronka1_talde5_tpv.Langilea>();

                    if (resultado != null)
                    {
                        // Usuario encontrado, abrir la pantalla Comanda
                        Comanda comandaForm = new Comanda
                        {
                            NombreUsuario = resultado.Izena // Pasar el nombre al formulario de Comanda
                        };
                        comandaForm.Show();
                        this.Hide(); // Ocultar el formulario de login
                    }
                    else
                    {
                        // Credenciales incorrectas
                        MessageBox.Show("Credenciales incorrectas. ¡Adiós!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    mySession.Close();
                }
            }
        }

        // Método para centrar los controles
        private void CenterControls()
        {
            // Centrar el formulario
            this.StartPosition = FormStartPosition.CenterScreen;

            // Centrar el botón de login
            logInButton.Left = (this.ClientSize.Width - logInButton.Width) / 2;
            logInButton.Top = (this.ClientSize.Height - logInButton.Height) / 2 + 50; // Centrado vertical y desplazado hacia abajo

            // Centrar los campos de texto
            emailText.Left = (this.ClientSize.Width - emailText.Width) / 2;
            emailText.Top = logInButton.Top - 60;

            pasahitzaText.Left = (this.ClientSize.Width - pasahitzaText.Width) / 2;
            pasahitzaText.Top = emailText.Top + 40;
        }
    }
}
