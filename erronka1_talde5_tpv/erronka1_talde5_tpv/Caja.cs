using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Cfg;

namespace erronka1_talde5_tpv
{
    public partial class Caja : Form
    {
        public string NombreUsuario { get; set; } // Propiedad para almacenar el nombre de usuario
        private ISessionFactory mySessionFactory; // SessionFactory de NHibernate

        public Caja()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; // Maximizar la ventana al iniciar
            ConfigureNHibernate(); // Configurar NHibernate al iniciar
        }

        private void ConfigureNHibernate()
        {
            try
            {
                var configuration = new Configuration();
                configuration.Configure(); // Lee el app.config (incluye <mapping assembly>)
                mySessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Lanzar la excepción para detener la ejecución
            }
        }

        private void Caja_Load(object sender, EventArgs e)
        {
            // Establecer el texto de bienvenida
            welcomeLabel.Text = $"¡Hola, {NombreUsuario}! (Caja)";

            // Centrar el Label de bienvenida
            welcomeLabel.Left = (this.ClientSize.Width - welcomeLabel.Width) / 2;
            welcomeLabel.Top = 50;

            // Colocar el botón "Volver" en la parte inferior izquierda
            backButton.Left = 20;
            backButton.Top = this.ClientSize.Height - backButton.Height - 20;

            // Cargar y mostrar las eskaeras pagadas
            MostrarEskaerasPagadas();
        }

        private void MostrarEskaerasPagadas()
        {
            using (var session = mySessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Obtener los detalles de las eskaeras pagadas
                        var detalles = session.Query<Eskaera2>()
                            .Where(e => e.Ordainduta == 1)
                            .Join(
                                session.Query<EskaeraDetalle>(),
                                e => e.Id,
                                ep => ep.EskaeraId,
                                (e, ep) => new EskaeraDetalle
                                {
                                    EskaeraId = e.Id,
                                    PlatoId = ep.PlateraId,
                                    Nota = ep.NotaGehigarriak,
                                    Hora = ep.EskaeraOrdua,
                                    Precio = session.Query<Platera>()
                                                   .Where(p => p.Id == ep.PlateraId)
                                                   .Select(p => p.Prezioa)
                                                   .FirstOrDefault()
                                })
                            .ToList();

                        // Agrupar los detalles por EskaeraId
                        var eskaerasAgrupadas = detalles
                            .GroupBy(d => d.EskaeraId)
                            .Select(g => new
                            {
                                EskaeraId = g.Key,
                                Platos = g.ToList(),
                                PrecioTotal = g.Sum(p => p.Precio)
                            })
                            .ToList();

                        // Mostrar las eskaeras pagadas en la interfaz
                        DisplayEskaerasPagadas(eskaerasAgrupadas);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar las eskaeras pagadas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback();
                    }
                }
            }
        }

        private void DisplayEskaerasPagadas(List<dynamic> eskaerasAgrupadas)
        {
            Panel panelContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#091725")
            };

            // Configuración del layout
            int anchoCuadro = 400;  // Ancho fijo del cuadro
            int altoCuadro = 200;   // Altura fija del cuadro
            int separacion = 15;    // Espacio entre cuadros
            int maxColumnas = 2;    // Máximo de columnas por fila

            int margenIzquierdo = (this.ClientSize.Width - (maxColumnas * (anchoCuadro + separacion))) / 2;
            margenIzquierdo = Math.Max(margenIzquierdo, 15);

            for (int i = 0; i < eskaerasAgrupadas.Count; i++)
            {
                int fila = i / maxColumnas;
                int columna = i % maxColumnas;

                // Panel principal del cuadro
                Panel cuadro = new Panel
                {
                    Width = anchoCuadro,
                    Height = altoCuadro,
                    Left = margenIzquierdo + columna * (anchoCuadro + separacion),
                    Top = 15 + fila * (altoCuadro + separacion),
                    BackColor = ColorTranslator.FromHtml("#BA450D"),
                    Padding = new Padding(10)
                };

                // ---- Eskaera ID ----
                Label lblEskaeraId = new Label
                {
                    Text = $"Eskaera ID: {eskaerasAgrupadas[i].EskaeraId}",
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 30
                };

                // ---- Contenedor para los platos ----
                Panel panelPlatos = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.Transparent
                };

                // ---- Agregar cada plato y sus detalles ----
                foreach (var plato in eskaerasAgrupadas[i].Platos)
                {
                    Label lblPlato = new Label
                    {
                        Text = $"Plato: {plato.PlatoId}\nNota: {plato.Nota ?? "Ninguna"}\nHora: {plato.Hora.ToString("HH:mm")}\nPrecio: {plato.Precio.ToString("C")}",
                        Font = new Font("Arial", 10),
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        Margin = new Padding(0, 0, 0, 10)
                    };

                    panelPlatos.Controls.Add(lblPlato);
                }

                // ---- Precio Total ----
                Label lblPrecioTotal = new Label
                {
                    Text = $"Precio Total: {eskaerasAgrupadas[i].PrecioTotal.ToString("C")}",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Bottom,
                    Height = 25
                };

                cuadro.Controls.Add(lblEskaeraId);
                cuadro.Controls.Add(panelPlatos);
                cuadro.Controls.Add(lblPrecioTotal);

                panelContenedor.Controls.Add(cuadro);
            }

            this.Controls.Add(panelContenedor);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // Abrir la ventana Comanda y pasar el nombre de usuario
            Comanda comandaForm = new Comanda
            {
                NombreUsuario = NombreUsuario
            };
            comandaForm.Show();
            this.Hide(); // Ocultar la ventana actual
        }
    }


}