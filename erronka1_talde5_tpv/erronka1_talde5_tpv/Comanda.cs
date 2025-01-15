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
            int verticalSpacing = 20;

            // Fila 1 de botones
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

            Button buttonHacerPedido = new Button
            {
                Text = "Hacer Pedido",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonEskaera.Right + horizontalSpacing, // A la derecha del primero
                Top = buttonEskaera.Top
            };
            buttonHacerPedido.Click += ButtonHacerPedido_Click;
            this.Controls.Add(buttonHacerPedido);

            // Fila 2 de botones
            Button buttonVerFacturas = new Button
            {
                Text = "Ver Facturas",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = (this.ClientSize.Width - 2 * buttonWidth - horizontalSpacing) / 2, // Centrado horizontalmente
                Top = buttonEskaera.Bottom + verticalSpacing // Justo debajo de la fila 1
            };
            buttonVerFacturas.Click += ButtonVerFacturas_Click;
            this.Controls.Add(buttonVerFacturas);

            Button buttonCancelarPedido = new Button
            {
                Text = "Cancelar Pedido",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonVerFacturas.Right + horizontalSpacing, // A la derecha del primero
                Top = buttonVerFacturas.Top
            };
            buttonCancelarPedido.Click += ButtonCancelarPedido_Click;
            this.Controls.Add(buttonCancelarPedido);

            // Fila 3 de botones
            Button buttonVerMesas = new Button
            {
                Text = "Ver Mesas",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = (this.ClientSize.Width - 2 * buttonWidth - horizontalSpacing) / 2, // Centrado horizontalmente
                Top = buttonVerFacturas.Bottom + verticalSpacing // Justo debajo de la fila 2
            };
            buttonVerMesas.Click += ButtonVerMesas_Click;
            this.Controls.Add(buttonVerMesas);

            // Botón Ajustes
            Button buttonAjustes = new Button
            {
                Text = "Ajustes",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = buttonVerMesas.Right + horizontalSpacing, // A la derecha del primero
                Top = buttonVerMesas.Top
            };
            buttonAjustes.Click += ButtonAjustes_Click;
            this.Controls.Add(buttonAjustes);

            // Nuevo botón "Txat" debajo de los otros botones
            Button buttonTxat = new Button
            {
                Text = "Txat",
                Width = buttonWidth,
                Height = buttonHeight,
                Left = (this.ClientSize.Width - buttonWidth) / 2, // Centrado horizontalmente
                Top = buttonAjustes.Bottom + verticalSpacing + 20 // Justo debajo de los botones anteriores
            };
            buttonTxat.Click += ButtonTxat_Click;
            this.Controls.Add(buttonTxat);
        }

        private void ButtonTxat_Click(object sender, EventArgs e)
        {
            txat txatForm = new txat();
            txatForm.Show();
            this.Hide(); // Oculta la pantalla actual
        }

        // Métodos de eventos para los botones
        private void ButtonEskaera_Click(object sender, EventArgs e)
        {
            Eskaera eskaeraForm = new Eskaera
            {
                NombreUsuario = NombreUsuario // Pasar el nombre del usuario
            };
            eskaeraForm.Show();
            this.Hide();
        }


        private void ButtonHacerPedido_Click(object sender, EventArgs e)
        {
            // Lógica para abrir la pantalla de hacer un pedido
            MessageBox.Show("Abriendo pantalla para hacer un pedido...");
        }

        private void ButtonVerFacturas_Click(object sender, EventArgs e)
        {
            // Lógica para abrir la pantalla de ver facturas
            MessageBox.Show("Abriendo pantalla para ver las facturas...");
        }

        private void ButtonCancelarPedido_Click(object sender, EventArgs e)
        {
            // Lógica para abrir la pantalla de cancelar un pedido
            MessageBox.Show("Abriendo pantalla para cancelar un pedido...");
        }

        private void ButtonVerMesas_Click(object sender, EventArgs e)
        {
            // Lógica para abrir la pantalla de ver mesas
            MessageBox.Show("Abriendo pantalla para ver las mesas...");
        }

        private void ButtonAjustes_Click(object sender, EventArgs e)
        {
            // Lógica para abrir la pantalla de ajustes
            MessageBox.Show("Abriendo pantalla de ajustes...");
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
