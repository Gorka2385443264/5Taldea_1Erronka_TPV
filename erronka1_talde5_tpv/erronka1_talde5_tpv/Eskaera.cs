using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
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

                // ¡No agregues mapeos manualmente!
                // configuration.AddClass(typeof(Platera)); // <-- Elimina esto
                // configuration.AddClass(typeof(Eskaera2)); // <-- Elimina esto

                mySessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mySessionFactory = null;
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
                        // Obtener las comandas
                        var comandas = session.CreateQuery("FROM Eskaera2").List<Eskaera2>();

                        // Desvincular entidades para evitar updates no deseados
                        session.Clear();

                        // Mostrar los datos
                        DisplayComandas(comandas);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar comandas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback(); // Asegúrate de revertir la transacción
                    }
                }
            }
        }
        private void DisplayComandas(IList<Eskaera2> comandas)
        {
            Panel panelContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#091725")
            };

            // Configuración del layout
            int anchoCuadro = 250;
            int altoCuadro = 180;
            int separacion = 20;
            int maxColumnas = 4;

            int margenIzquierdo = (this.ClientSize.Width - (maxColumnas * (anchoCuadro + separacion))) / 2;
            margenIzquierdo = Math.Max(margenIzquierdo, 20);

            for (int i = 0; i < comandas.Count; i++)
            {
                int fila = i / maxColumnas;
                int columna = i % maxColumnas;

                Panel cuadro = new Panel
                {
                    Width = anchoCuadro,
                    Height = altoCuadro,
                    Left = margenIzquierdo + columna * (anchoCuadro + separacion),
                    Top = 20 + fila * (altoCuadro + separacion),
                    BackColor = ColorTranslator.FromHtml("#BA450D"),
                    Padding = new Padding(10)
                };

                // Obtener nombre del plato
                string nombrePlato = ObtenerNombrePlato(comandas[i].PlateraId);
                Label contenido = new Label
                {
                    Text = $"Comanda ID: {comandas[i].Id}\n" +
                           $"Eskaera ID: {comandas[i].EskaeraId}\n" + // <-- ¡Línea añadida!
                           $"Hora pedido: {comandas[i].EskaeraOrdua:HH:mm}\n" +
                           $"Plato: {nombrePlato}\n" +
                           $"Notas: {comandas[i].NotaGehigarriak ?? "Ninguna"}",
                    Dock = DockStyle.Fill,
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Arial", 10)
                };

                cuadro.Controls.Add(contenido);
                panelContenedor.Controls.Add(cuadro);
            }

            this.Controls.Add(panelContenedor);

            // Botón de volver (ajustado para evitar solapamientos)
            Button btnVolver = new Button
            {
                Text = "Volver",
                Size = new Size(120, 40),
                Location = new Point(20, this.ClientSize.Height - 70),
                BackColor = ColorTranslator.FromHtml("#E89E47"),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnVolver.Click += (s, ev) => this.Close();
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