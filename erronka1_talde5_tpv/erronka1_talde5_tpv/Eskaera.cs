using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Eskaera : Form
    {
        private NHibernate.Cfg.Configuration miConfiguracion;
        private ISessionFactory mySessionFactory;

        public string NombreUsuario { get; set; } // Propiedad pública para el nombre de usuario

        public Eskaera()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Eskaera_Load(object sender, EventArgs e)
        {
            // Configurar NHibernate
            miConfiguracion = new NHibernate.Cfg.Configuration();
            miConfiguracion.Configure();
            mySessionFactory = miConfiguracion.BuildSessionFactory();

            // Consulta para obtener todas las comandes (eskaera)
            using (var mySession = mySessionFactory.OpenSession())
            {
                using (var transaccion = mySession.BeginTransaction())
                {
                    try
                    {
                        // Obtener todos los langiles para evitar múltiples consultas en el bucle
                        string hqlLangiles = "FROM Langilea";
                        var langileak = mySession.CreateQuery(hqlLangiles).List<Langilea>()
                            .ToDictionary(l => l.Id, l => l.Izena);

                        // Obtener todas las comandes (eskaeras)
                        string hqlEskaeras = "FROM Eskaera2";
                        var listaEskaeras = mySession.CreateQuery(hqlEskaeras).List<Eskaera2>();

                        // Evitar que NHibernate intente actualizar las entidades no deseadas
                        mySession.Clear();  // Limpia la sesión para evitar actualizaciones no deseadas

                        // Mostrar los resultados en la pantalla
                        DisplayEskaerasAsText(listaEskaeras, langileak);

                        transaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al obtener las comandes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DisplayEskaerasAsText(IList<Eskaera2> listaEskaeras, Dictionary<int, string> langileak)
        {
            // Crear un panel contenedor con desplazamiento
            Panel scrollablePanel = new Panel
            {
                Dock = DockStyle.Fill, // Ocupa toda la ventana
                AutoScroll = true,    // Habilitar desplazamiento
                BackColor = this.BackColor // Igualar fondo con el formulario
            };

            int verticalSpacing = 10;
            int horizontalSpacing = 10;
            int topMargin = 20;
            int leftMargin = 20;
            int maxCuadrosPorFila = 5;
            int cuadroWidth = 200;
            int cuadroHeight = 150;

            int index = 0;
            foreach (Eskaera2 eskaera in listaEskaeras)
            {
                // Calcular la posición de cada cuadro
                int row = index / maxCuadrosPorFila;
                int column = index % maxCuadrosPorFila;

                // Crear un cuadro para cada eskaera
                Panel panel = new Panel
                {
                    Width = cuadroWidth,
                    Height = cuadroHeight,
                    Left = leftMargin + (column * (cuadroWidth + horizontalSpacing)),
                    Top = topMargin + (row * (cuadroHeight + verticalSpacing)),
                    BackColor = Color.Red,
                    Padding = new Padding(10) // Agregar margen interno
                };

                // Esquinas redondeadas
                panel.Region = new Region(
                    GraphicsPathHelper.CreateRoundedRectangle(
                        new Rectangle(0, 0, cuadroWidth, cuadroHeight),
                        20
                    )
                );

                // Obtener el nombre del langile
                string langileIzena = langileak.ContainsKey(eskaera.LangileaId)
                    ? langileak[eskaera.LangileaId]
                    : "Desconocido";

                // Crear el texto dinámico
                Label label = new Label
                {
                    Text = $"ID: {eskaera.Id}\n" +
                           $"Langilea: {langileIzena}\n" +
                           $"Mahaila ID: {eskaera.MahailaId}\n" +
                           $"Platera ID: {eskaera.Platera}\n" +
                           $"Nota: {(string.IsNullOrWhiteSpace(eskaera.Nota) ? "Sin nota" : eskaera.Nota)}\n",
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Arial", 10, FontStyle.Regular)
                };

                // Agregar el texto al panel y el panel al contenedor
                panel.Controls.Add(label);
                scrollablePanel.Controls.Add(panel);

                index++;
            }

            // Agregar el panel contenedor con desplazamiento al formulario
            this.Controls.Clear(); // Limpiar controles anteriores
            this.Controls.Add(scrollablePanel);
        }

        // Clase para crear rectángulos con esquinas redondeadas
        public static class GraphicsPathHelper
        {
            public static GraphicsPath CreateRoundedRectangle(Rectangle rect, int cornerRadius)
            {
                GraphicsPath path = new GraphicsPath();

                int diameter = cornerRadius * 2;

                // Crear arcos para las esquinas
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90); // Esquina superior izquierda
                path.AddArc(rect.X + rect.Width - diameter, rect.Y, diameter, diameter, 270, 90); // Esquina superior derecha
                path.AddArc(rect.X + rect.Width - diameter, rect.Y + rect.Height - diameter, diameter, diameter, 0, 90); // Esquina inferior derecha
                path.AddArc(rect.X, rect.Y + rect.Height - diameter, diameter, diameter, 90, 90); // Esquina inferior izquierda

                path.CloseFigure();
                return path;
            }
        }
    }
}
