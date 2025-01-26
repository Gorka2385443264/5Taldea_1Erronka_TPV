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
        private ISessionFactory mySessionFactory;

        public string NombreUsuario { get; set; }

        public Eskaera()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            ConfigureNHibernate(); // Configurar NHibernate al iniciar
        }

        private void ConfigureNHibernate()
        {
            try
            {
                var configuration = new NHibernate.Cfg.Configuration();
                configuration.Configure(); // Lee el app.config (incluye <mapping assembly>)

                mySessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Lanzar la excepción para detener la ejecución
            }
        }

        private void Eskaera_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#091725");

            if (mySessionFactory == null)
            {
                MessageBox.Show("NHibernate no se configuró correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var session = mySessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Obtener todas las comandas (solo lectura)
                        var comandas = session.CreateQuery("FROM Eskaera2")
                                              .SetReadOnly(true)
                                              .List<Eskaera2>();

                        // Agrupar comandas por eskaera_id
                        var comandasAgrupadas = comandas
                            .GroupBy(c => c.EskaeraId)
                            .Select(g => (
                                EskaeraId: g.Key,
                                Platos: g.Select(c => (
                                    PlatoId: c.PlateraId,
                                    Notas: c.NotaGehigarriak,
                                    Hora: c.EskaeraOrdua
                                )).ToList()
                            )).ToList();

                        // Mostrar los datos agrupados
                        DisplayComandasAgrupadas(comandasAgrupadas);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar comandas: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback();
                    }
                }
            }
        }
        private void DisplayComandasAgrupadas(List<(int EskaeraId, List<(int PlatoId, string Notas, DateTime Hora)> Platos)> comandasAgrupadas)
        {
            Panel panelContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#091725")
            };

            // Configuración del layout
            int anchoCuadro = 350;  // Ancho fijo del cuadro
            int altoCuadro = 200;   // Altura fija del cuadro
            int separacion = 15;    // Espacio entre cuadros
            int maxColumnas = 3;    // Máximo de columnas por fila

            int margenIzquierdo = (this.ClientSize.Width - (maxColumnas * (anchoCuadro + separacion))) / 2;
            margenIzquierdo = Math.Max(margenIzquierdo, 15);

            for (int i = 0; i < comandasAgrupadas.Count; i++)
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

                // ---- Comanda ID (Grande, arriba a la izquierda) ----
                Label lblComandaId = new Label
                {
                    Text = $"Comanda ID: {comandasAgrupadas[i].EskaeraId}",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // ---- Espacio entre Comanda ID y Plato ----
                Panel espacio = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 10,
                    BackColor = Color.Transparent
                };

                // ---- Contenedor para los platos ----
                Panel panelPlatos = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.Transparent
                };

                // ---- Agregar cada plato y sus notas ----
                foreach (var plato in comandasAgrupadas[i].Platos)
                {
                    Label lblPlato = new Label
                    {
                        Text = $"PLATO: {ObtenerNombrePlato(plato.PlatoId)}\nNOTAS: {plato.Notas ?? "Ninguna"}\nHORA: {plato.Hora.ToString("HH:mm")}",
                        Font = new Font("Arial", 10),
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        Margin = new Padding(0, 0, 0, 10)  // Margen inferior entre platos
                    };

                    panelPlatos.Controls.Add(lblPlato);

                    // Agregar un separador entre platos
                    Panel separador = new Panel
                    {
                        Height = 1,
                        Dock = DockStyle.Top,
                        BackColor = Color.White
                    };
                    panelPlatos.Controls.Add(separador);
                }

                // Añadir controles al cuadro principal
                cuadro.Controls.Add(panelPlatos);  // Platos y notas
                cuadro.Controls.Add(espacio);      // Espacio entre Comanda ID y Plato
                cuadro.Controls.Add(lblComandaId); // Comanda ID en la parte superior

                panelContenedor.Controls.Add(cuadro);
            }

            this.Controls.Add(panelContenedor);

            // Botón de volver
            Button btnVolver = new Button
            {
                Text = "Volver",
                Size = new Size(120, 40),
                Location = new Point(20, this.ClientSize.Height - 70),
                BackColor = ColorTranslator.FromHtml("#E89E47"),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnVolver.Click += BtnVolver_Click;
            this.Controls.Add(btnVolver);
        }
        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra la ventana actual
        }

        private string ObtenerNombrePlato(int plateraId)
        {
            try
            {
                using (var session = mySessionFactory.OpenSession())
                {
                    var plato = session.Get<Platera>(plateraId);
                    return plato?.Izena ?? "Plato desconocido";
                }
            }
            catch
            {
                return "Error al cargar el nombre";
            }
        }
    }
}