using System;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Caja : Form
    {
        // Propiedad para recibir el nombre del usuario
        public string NombreUsuario { get; set; }

        public Caja()
        {
            InitializeComponent();
            // Establecer el formulario en pantalla completa
            this.WindowState = FormWindowState.Maximized;
        }

        private void Caja_Load(object sender, EventArgs e)
        {
            // Establecer el texto de bienvenida con el nombre del usuario
            welcomeLabel.Text = $"¡Hola, {NombreUsuario}! (Caja)";

            // Centrar el Label de bienvenida en la parte superior central
            welcomeLabel.Left = (this.ClientSize.Width - welcomeLabel.Width) / 2;
            welcomeLabel.Top = 50; // 50 píxeles desde el borde superior

            // Colocar el botón "Volver" en la parte inferior izquierda
            backButton.Left = 20; // 20 píxeles desde el borde izquierdo
            backButton.Top = this.ClientSize.Height - backButton.Height - 20; // 20 píxeles desde el borde inferior
        }

        // Método para manejar el clic en el botón "Volver"
        private void BackButton_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la pantalla Comanda.cs
            Comanda comandaForm = new Comanda
            {
                NombreUsuario = NombreUsuario // Pasar el nombre del usuario
            };

            // Mostrar la pantalla Comanda.cs
            comandaForm.Show();

            // Ocultar la pantalla actual (Caja.cs)
            this.Hide();
        }
    }
}