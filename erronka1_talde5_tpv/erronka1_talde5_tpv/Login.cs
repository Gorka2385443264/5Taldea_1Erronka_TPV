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
        }

        private NHibernate.Cfg.Configuration miConfiguracion;
        private ISessionFactory mySessionFactory;
        private ISession mySession;

        private void Login_Load(object sender, EventArgs e)
        {
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
                        // Usuario encontrado
                        MessageBox.Show($"¡Hola, {emailText.Text}!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
