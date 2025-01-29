using System;
using System.Drawing;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Sortu : Form
    {
        private string nombreUsuario;

        // Constructor de Sortu.cs que recibe el nombre de usuario
        public Sortu(string usuario)
        {
            InitializeComponent();
            this.nombreUsuario = usuario;

            // Configurar el label de bienvenida
            nombreUsuarioLabel.Text = "Bienvenido, " + nombreUsuario;
            nombreUsuarioLabel.Location = new Point(
                (this.ClientSize.Width - nombreUsuarioLabel.Width) / 2,
                20 // Un poco de margen desde la parte superior
            );

            // Maximizar la ventana cuando se abre
            this.WindowState = FormWindowState.Maximized;
        }

        // Evento Click del botón "Volver"
        private void volverButton_Click(object sender, EventArgs e)
        {
            // Aquí navegas a la pantalla de Eskaera
            Eskaera eskaeraScreen = new Eskaera();  // Cambia esto según cómo sea tu clase de Eskaera
            eskaeraScreen.Show();
            this.Hide(); // Ocultar la pantalla actual (Sortu)
        }
    }
}
