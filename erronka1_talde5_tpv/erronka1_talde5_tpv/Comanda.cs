using System;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Comanda : Form
    {
        // Propiedad para recibir el nombre del usuario
        public string NombreUsuario { get; set; }

        public Comanda()
        {
            InitializeComponent();
            // Establecer el formulario en pantalla completa
            this.WindowState = FormWindowState.Maximized;
        }

        private void Comanda_Load(object sender, EventArgs e)
        {
            // Centrar los controles
            CenterControls();

            // Establecer el texto del label de bienvenida
            welcomeLabel.Text = $"¡Hola, {NombreUsuario}!";
        }

        // Método para centrar los controles
        private void CenterControls()
        {
            // Centrar el formulario
            this.StartPosition = FormStartPosition.CenterScreen;

            // Colocar el label "¡Hola, NombreUsuario!" un poco más a la derecha
            welcomeLabel.Left = 670; // Ajuste del valor para moverlo más a la derecha
            welcomeLabel.Top = 30;  // Un poco de espacio desde el borde superior

            // Centrar el label "¿Qué quieres hacer?" debajo del anterior
            questionLabel.Left = (this.ClientSize.Width - questionLabel.Width) / 2;
            questionLabel.Top = welcomeLabel.Bottom + 20;  // Justo debajo del label de bienvenida

            // Crear los botones con textos y acciones diferentes
            CreateButtons();

            // Crear el botón "Volver" en la parte inferior izquierda
            CreateBackButton();
        }

        // Método para crear los botones con texto y acciones diferentes
        private void CreateButtons()
        {
            int buttonWidth = 150;
            int buttonHeight = 50;
            int horizontalSpacing = 20;

            // Crear el botón "Eskaera"
            Button buttonEskaera = new Button
            {
                Text = "Eskaera",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = (this.ClientSize.Width - 2 * buttonWidth - horizontalSpacing) / 2, // Centrado horizontalmente
                Top = questionLabel.Bottom + 20  // Justo debajo del label "¿Qué quieres hacer?"
            };
            buttonEskaera.Click += ButtonEskaera_Click;
            this.Controls.Add(buttonEskaera);

            // Crear el botón "Caja" a la derecha de "Eskaera"
            Button buttonCaja = new Button
            {
                Text = "Caja",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonEskaera.Right + horizontalSpacing, // A la derecha de "Eskaera"
                Top = buttonEskaera.Top // Mismo nivel vertical
            };
            buttonCaja.Click += ButtonCaja_Click;
            this.Controls.Add(buttonCaja);
        }

        // Método para manejar el clic en el botón "Eskaera"
        private void ButtonEskaera_Click(object sender, EventArgs e)
        {
            Eskaera eskaeraForm = new Eskaera
            {
                NombreUsuario = NombreUsuario // Pasar el nombre del usuario
            };
            eskaeraForm.Show();
            this.Hide();
        }

        // Método para manejar el clic en el botón "Caja"
        private void ButtonCaja_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la pantalla Caja.cs
            Caja cajaForm = new Caja
            {
                NombreUsuario = NombreUsuario // Pasar el nombre del usuario si es necesario
            };

            // Mostrar la pantalla Caja.cs
            cajaForm.Show();

            // Ocultar la pantalla actual (Comanda.cs)
            this.Hide();
        }

        // Método para crear el botón "Volver"
        private void CreateBackButton()
        {
            Button backButton = new Button
            {
                Text = "Volver",
                Width = 100,
                Height = 40,
                Left = 20,  // Posición en el borde izquierdo
                Top = this.ClientSize.Height - 80  // Posición en la parte inferior
            };

            backButton.Click += (sender, e) => {
                // Al hacer clic, se abre el formulario de Login
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();  // Oculta el formulario actual
            };

            this.Controls.Add(backButton);
        }
    }
}