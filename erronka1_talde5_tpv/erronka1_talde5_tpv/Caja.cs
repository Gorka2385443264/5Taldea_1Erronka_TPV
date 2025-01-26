using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Caja : Form
    {
        public string NombreUsuario { get; set; }

        public Caja()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Caja_Load(object sender, EventArgs e)
        {
            // Establecer el texto de bienvenida
            welcomeLabel.Text = $"¡Hola, {NombreUsuario}! (Caja)";

            // Centrar el Label de bienvenida
            welcomeLabel.Left = (this.ClientSize.Width - welcomeLabel.Width) / 2;
            welcomeLabel.Top = 50;

            // Colocar el botón "Volver"
            backButton.Left = 20;
            backButton.Top = this.ClientSize.Height - backButton.Height - 20;

            // Mostrar las eskaeras pagadas
            MostrarEskaerasPagadas();
        }

        private void MostrarEskaerasPagadas()
        {
            Panel panelContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#091725")
            };

            int anchoCuadro = 350;
            int altoCuadro = 150;
            int separacion = 15;
            int maxColumnas = 3;

            int margenIzquierdo = (this.ClientSize.Width - (maxColumnas * (anchoCuadro + separacion))) / 2;
            margenIzquierdo = Math.Max(margenIzquierdo, 15);

            for (int i = 0; i < Eskaera.EskaerasPagadas.Count; i++)
            {
                int fila = i / maxColumnas;
                int columna = i % maxColumnas;

                Panel cuadro = new Panel
                {
                    Width = anchoCuadro,
                    Height = altoCuadro,
                    Left = margenIzquierdo + columna * (anchoCuadro + separacion),
                    Top = 15 + fila * (altoCuadro + separacion),
                    BackColor = ColorTranslator.FromHtml("#BA450D"),
                    Padding = new Padding(10)
                };

                Label lblEskaeraId = new Label
                {
                    Text = $"Eskaera ID: {Eskaera.EskaerasPagadas[i].EskaeraId}",
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 30
                };

                Label lblPrecioTotal = new Label
                {
                    Text = $"Precio Total: {Eskaera.EskaerasPagadas[i].PrecioTotal.ToString("C")}",
                    Font = new Font("Arial", 12),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 25
                };

                Label lblMetodoPago = new Label
                {
                    Text = $"Método de Pago: {Eskaera.EskaerasPagadas[i].MetodoPago}",
                    Font = new Font("Arial", 12),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 25
                };

                Label lblFechaPago = new Label
                {
                    Text = $"Fecha: {Eskaera.EskaerasPagadas[i].FechaPago.ToString("dd/MM/yyyy HH:mm")}",
                    Font = new Font("Arial", 10),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 20
                };

                cuadro.Controls.Add(lblEskaeraId);
                cuadro.Controls.Add(lblPrecioTotal);
                cuadro.Controls.Add(lblMetodoPago);
                cuadro.Controls.Add(lblFechaPago);

                panelContenedor.Controls.Add(cuadro);
            }

            this.Controls.Add(panelContenedor);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Comanda comandaForm = new Comanda
            {
                NombreUsuario = NombreUsuario
            };
            comandaForm.Show();
            this.Hide();
        }
    }
}