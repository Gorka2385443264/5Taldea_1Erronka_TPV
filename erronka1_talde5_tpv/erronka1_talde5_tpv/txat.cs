using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class txat : Form
    {
        private TextBox textBoxMessage;
        private Button buttonSend;
        private ListBox listBoxChat;

        public txat()
        {
            InitializeComponent();
            InitializeChatComponents();
        }

        private void txat_Load(object sender, EventArgs e)
        {

        }

        private void InitializeChatComponents()
        {
            // Inicializar y configurar el TextBox para mensajes
            textBoxMessage = new TextBox
            {
                Location = new Point(12, 328),
                Size = new Size(326, 20),
                Name = "textBoxMessage"
            };

            // Inicializar y configurar el Button para enviar mensajes
            buttonSend = new Button
            {
                Location = new Point(344, 326),
                Size = new Size(75, 23),
                Name = "buttonSend",
                Text = "Enviar"
            };
            buttonSend.Click += new EventHandler(buttonSend_Click);

            // Inicializar y configurar el ListBox para mostrar el chat
            listBoxChat = new ListBox
            {
                Location = new Point(12, 12),
                Size = new Size(407, 303),
                Name = "listBoxChat"
            };

            // Agregar los controles al formulario
            Controls.Add(textBoxMessage);
            Controls.Add(buttonSend);
            Controls.Add(listBoxChat);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Obtener el mensaje del TextBox
            string message = textBoxMessage.Text;

            // Validar que el mensaje no esté vacío
            if (!string.IsNullOrEmpty(message))
            {
                // Agregar el mensaje al ListBox
                listBoxChat.Items.Add("Tú: " + message);

                // Limpiar el TextBox después de enviar el mensaje
                textBoxMessage.Clear();
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un mensaje.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
