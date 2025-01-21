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
            // Cambiar el fondo del formulario
            this.BackColor = ColorTranslator.FromHtml("#091725");

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

            int verticalSpacing = 20;
            int cuadroWidth = 200;
            int cuadroHeight = 150;
            int maxCuadrosPorFila = 5;

            int visibleWidth = this.ClientSize.Width;
            int totalRowWidth = maxCuadrosPorFila * cuadroWidth + (maxCuadrosPorFila - 1) * verticalSpacing;
            int leftMargin = Math.Max((visibleWidth - totalRowWidth) / 2, 20);

            int index = 0;
            foreach (Eskaera2 eskaera in listaEskaeras)
            {
                int row = index / maxCuadrosPorFila;
                int column = index % maxCuadrosPorFila;

                Panel panel = new Panel
                {
                    Width = cuadroWidth,
                    Height = cuadroHeight,
                    Left = leftMargin + (column * (cuadroWidth + verticalSpacing)),
                    Top = 20 + (row * (cuadroHeight + verticalSpacing)),
                    BackColor = ColorTranslator.FromHtml("#BA450D"), // Color de los cuadros
                    Padding = new Padding(10)
                };

                panel.Region = new Region(
                    GraphicsPathHelper.CreateRoundedRectangle(
                        new Rectangle(0, 0, cuadroWidth, cuadroHeight),
                        20
                    )
                );

                string langileIzena = langileak.ContainsKey(eskaera.LangileaId)
                    ? langileak[eskaera.LangileaId]
                    : "Desconocido";

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
                    ForeColor = Color.White, // Color del texto blanco
                    BackColor = Color.Transparent,
                    Font = new Font("Arial", 10, FontStyle.Regular)
                };

                panel.Controls.Add(label);
                scrollablePanel.Controls.Add(panel);
                index++;
            }

            // Actualizar el panel de contenido
            this.Controls.Add(scrollablePanel);
        }

        private void BtnComanda_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario Comanda
            Comanda comandaForm = new Comanda();

            // Pasar el nombre del usuario al formulario de Comanda
            comandaForm.NombreUsuario = this.NombreUsuario;

            // Mostrar el formulario de Comanda
            comandaForm.Show();

            // Cerrar el formulario actual (Eskaera)
            this.Close();
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
