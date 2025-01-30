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
            // Cambiar el color de fondo de la pantalla
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#091725");

            // Centrar los controles
            CenterControls();

            // Establecer el texto del label de bienvenida
            welcomeLabel.Text = $"¡Hola, {NombreUsuario}!";
            welcomeLabel.ForeColor = System.Drawing.Color.White; // Color del texto blanco

            questionLabel.ForeColor = System.Drawing.Color.White; // Color del texto blanco
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
            int buttonWidth = (int)(this.ClientSize.Width * 0.3);
            int buttonHeight = (int)(this.ClientSize.Height * 0.2);
            int horizontalSpacing = (int)(this.ClientSize.Width * 0.05);

            int totalWidth = (3 * buttonWidth) + (2 * horizontalSpacing);
            int startLeft = (this.ClientSize.Width - totalWidth) / 2;
            int startTop = (this.ClientSize.Height - buttonHeight) / 2;

            System.Drawing.Color buttonColor = System.Drawing.ColorTranslator.FromHtml("#BA450D");

            Button buttonEskaera = new Button
            {
                Text = "Eskaera",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = startLeft,
                Top = startTop,
                Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold),
                BackColor = buttonColor,
                ForeColor = System.Drawing.Color.White
            };
            buttonEskaera.Click += ButtonEskaera_Click;
            this.Controls.Add(buttonEskaera);

            Button buttonCaja = new Button
            {
                Text = "Caja",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonEskaera.Right + horizontalSpacing,
                Top = startTop,
                Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold),
                BackColor = buttonColor,
                ForeColor = System.Drawing.Color.White
            };
            buttonCaja.Click += ButtonCaja_Click;
            this.Controls.Add(buttonCaja);

            Button buttonTxat = new Button
            {
                Text = "Txat",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonCaja.Right + horizontalSpacing,
                Top = startTop,
                Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold),
                BackColor = buttonColor,
                ForeColor = System.Drawing.Color.White
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
                Left = 20,
                Top = this.ClientSize.Height - 80,
                BackColor = System.Drawing.ColorTranslator.FromHtml("#E89E47"),
                ForeColor = System.Drawing.Color.White
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
