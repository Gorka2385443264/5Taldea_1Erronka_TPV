using NHibernate;
using System;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Eskaera : Form
    {
        private NHibernate.Cfg.Configuration miConfiguracion;
        private ISessionFactory mySessionFactory;
        private ISession mySession;

        public Eskaera()
        {
            InitializeComponent();
        }

        private void Eskaera_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Formulario Eskaera cargado correctamente");

            // Configurar NHibernate
            miConfiguracion = new NHibernate.Cfg.Configuration();
            miConfiguracion.Configure();
            mySessionFactory = miConfiguracion.BuildSessionFactory();
            mySession = mySessionFactory.OpenSession();

            // Consulta para obtener todas las comandes (eskaera)
            using (var transaccion = mySession.BeginTransaction())
            {
                try
                {
                    string hql = "FROM Eskaera2"; // Consulta HQL para obtener todas las comandes
                    var query = mySession.CreateQuery(hql);

                    // Ejecutar la consulta
                    var listaEskaeras = query.List<Eskaera2>();

                    // Mostrar los resultados en el DataGridView
                    dataGridViewEskaera.DataSource = listaEskaeras;

                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener las comandes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    mySession.Close();
                }
            }
        }

    }
}
