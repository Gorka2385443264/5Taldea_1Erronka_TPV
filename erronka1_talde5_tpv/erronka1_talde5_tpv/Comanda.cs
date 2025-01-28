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
                Left = (this.ClientSize.Width - 4 * buttonWidth - 3 * horizontalSpacing) / 2, // Centrado horizontalmente para 4 botones
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

            // Crear el botón "Txat" a la derecha de "Caja"
            Button buttonTxat = new Button
            {
                Text = "Txat",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonCaja.Right + horizontalSpacing, // A la derecha de "Caja"
                Top = buttonCaja.Top // Mismo nivel vertical
            };
            buttonTxat.Click += ButtonTxat_Click;
            this.Controls.Add(buttonTxat);

  
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
            Caja cajaForm = new Caja
            {
                NombreUsuario = NombreUsuario // Pasar el nombre del usuario si es necesario
            };
            cajaForm.Show();
            this.Hide();
        }

        // Método para manejar el clic en el botón "Txat"
        private void ButtonTxat_Click(object sender, EventArgs e)
        {
            txat txatForm = new txat(NombreUsuario); // Pasar el nombre del usuario
            txatForm.Show();
            this.Hide();
        }

        // Método para manejar el clic en el botón "Sortu Eskaerak"
    

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
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            };

            this.Controls.Add(backButton);
        }
    }
}
