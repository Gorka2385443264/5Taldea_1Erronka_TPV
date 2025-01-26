using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Eskaera : Form
    {
        private NHibernate.Cfg.Configuration miConfiguracion;
        private ISessionFactory mySessionFactory;

        public string NombreUsuario { get; set; }

        public Eskaera()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Eskaera_Load(object sender, EventArgs e)
        {
            // Cambiar el fondo del formulario
            this.BackColor = ColorTranslator.FromHtml("#091725");

            // Configurar NHibernate
            miConfiguracion = new NHibernate.Cfg.Configuration();
            miConfiguracion.Configure();
            mySessionFactory = miConfiguracion.BuildSessionFactory();

            // Consulta para obtener los platos de una comanda específica
            using (var mySession = mySessionFactory.OpenSession())
            {
                using (var transaccion = mySession.BeginTransaction())
                {
                    try
                    {
                        // Obtener los platos de la tabla eskaera_platera
                        string hqlEskaeraPlatera = "FROM EskaeraPlatera";
                        var listaEskaeraPlatera = mySession.CreateQuery(hqlEskaeraPlatera).List<EskaeraPlatera>();

                        // Evitar que NHibernate intente actualizar las entidades no deseadas
                        mySession.Clear();

                        // Mostrar los resultados en la pantalla
                        DisplayEskaeraPlateraAsText(listaEskaeraPlatera);

                        transaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al obtener los platos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DisplayEskaeraPlateraAsText(IList<EskaeraPlatera> listaEskaeraPlatera)
        {
            // Crear un panel contenedor con desplazamiento
            Panel scrollablePanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = this.BackColor
            };

            int verticalSpacing = 20;
            int cuadroWidth = 200;
            int cuadroHeight = 150;
            int maxCuadrosPorFila = 5;

            int visibleWidth = this.ClientSize.Width;
            int totalRowWidth = maxCuadrosPorFila * cuadroWidth + (maxCuadrosPorFila - 1) * verticalSpacing;
            int leftMargin = Math.Max((visibleWidth - totalRowWidth) / 2, 20);

            int index = 0;
            foreach (EskaeraPlatera platera in listaEskaeraPlatera)
            {
                int row = index / maxCuadrosPorFila;
                int column = index % maxCuadrosPorFila;

                Panel panel = new Panel
                {
                    Width = cuadroWidth,
                    Height = cuadroHeight,
                    Left = leftMargin + (column * (cuadroWidth + verticalSpacing)),
                    Top = 20 + (row * (cuadroHeight + verticalSpacing)),
                    BackColor = ColorTranslator.FromHtml("#BA450D"),
                    Padding = new Padding(10)
                };

                Label label = new Label
                {
                    Text = $"ID: {platera.Id}\n" +
                           $"Eskaera ID: {platera.EskaeraId}\n" +
                           $"Platera ID: {platera.PlateraId}\n" +
                           $"Nota: {(string.IsNullOrWhiteSpace(platera.NotaGehigarriak) ? "Sin nota" : platera.NotaGehigarriak)}\n" +
                           $"Eskaera Ordua: {platera.EskaeraOrdua}\n" +
                           $"Ateratze Ordua: {platera.AteratzeOrdua}",
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Arial", 10, FontStyle.Regular)
                };

                panel.Controls.Add(label);
                scrollablePanel.Controls.Add(panel);
                index++;
            }

            // Actualizar el panel de contenido
            this.Controls.Add(scrollablePanel);

            // Agregar botón de volver
            Button btnVolver = new Button
            {
                Text = "Volver",
                Location = new Point(20, this.ClientSize.Height - 60),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White
            };
            btnVolver.Click += BtnVolver_Click;
            this.Controls.Add(btnVolver);
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close(); // Esto cierra la ventana actual
        }


    }
}
