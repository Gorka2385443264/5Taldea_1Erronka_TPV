using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class txat : Form
    {
        private TextBox textBoxMessage;
        private Button buttonSend;
        private ListBox listBoxChat;

        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread listenerThread;

        private String izena;

        public txat(String izena)
        {
            this.izena = izena;
            Console.WriteLine(this.izena);
            InitializeComponent();
            InitializeChatComponents();
        }

        private void txat_Load(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void InitializeChatComponents()
        {
            // Panel para mensajes de chat
            FlowLayoutPanel panelChat = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 70),
                AutoScroll = true,
                BackColor = Color.White,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            textBoxMessage = new TextBox
            {
                Location = new Point(10, this.ClientSize.Height - 60),
                Size = new Size(this.ClientSize.Width - 120, 50),
                Multiline = true,
                Name = "textBoxMessage",
                Font = new Font("Segoe UI", 12),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None
            };

            buttonSend = new Button
            {
                Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 60),
                Size = new Size(90, 50),
                Name = "buttonSend",
                Text = "Enviar",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            buttonSend.Click += new EventHandler(buttonSend_Click);

            this.Controls.Add(panelChat);
            this.Controls.Add(textBoxMessage);
            this.Controls.Add(buttonSend);

            this.Resize += (s, e) =>
            {
                panelChat.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 70);
                textBoxMessage.Location = new Point(10, this.ClientSize.Height - 60);
                textBoxMessage.Size = new Size(this.ClientSize.Width - 120, 50);
                buttonSend.Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 60);
            };

            panelChat.SizeChanged += (s, e) => AdjustMessagePanelMargins(panelChat);
        }
        private void AdjustMessagePanelMargins(FlowLayoutPanel panelChat) { 
            foreach (FlowLayoutPanel messagePanel in panelChat.Controls.OfType<FlowLayoutPanel>()) { 
                bool isUser = messagePanel.Controls.OfType<Label>().FirstOrDefault()?.BackColor == Color.LightBlue; 
                int marginLeft = isUser ? panelChat.Width - messagePanel.PreferredSize.Width - 30 : 0; 
                int marginRight = isUser ? 0 : 0; 
                messagePanel.Margin = new Padding(marginLeft, 0, marginRight, 10); 
            } 
        }

        private void AddMessageToPanel(string message, bool isUserMessage)
        {
            FlowLayoutPanel panelChat = this.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();
            if (panelChat == null) return;

            // Separar el nombre del remitente del mensaje
            var parts = message.Split(new char[] { '>' }, 2);
            if (parts.Length < 2) return;
            string senderName = parts[0];
            string msg = parts[1];

            bool isUser = (senderName.Trim() == izena.Trim());

            Label labelMessage = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(panelChat.Width - 30, 0),
                Text = message,  // Mostrar solo el contenido del mensaje
                Font = new Font("Segoe UI", 12),
                BackColor = isUser ? Color.LightBlue : Color.LightGray,
                ForeColor = Color.Black,
                Padding = new Padding(10),
                Margin = new Padding(0),
                BorderStyle = BorderStyle.FixedSingle,
            };

            FlowLayoutPanel messagePanel = new FlowLayoutPanel
            {
                MaximumSize = new Size(panelChat.Width - 30, 0),
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(5),
                Margin = isUser ? new Padding(panelChat.Width - labelMessage.Width - 30, 0, 0, 0) : new Padding(0),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Anchor = isUser ? AnchorStyles.Right : AnchorStyles.Left,
                Dock = isUser ? DockStyle.Right : DockStyle.Left
            };

            messagePanel.Controls.Add(labelMessage);
            panelChat.Controls.Add(messagePanel);
            panelChat.ScrollControlIntoView(messagePanel);
            AdjustMessagePanelMargins(panelChat);

        }



        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = textBoxMessage.Text;

            if (!string.IsNullOrEmpty(message))
            {
                string formattedMessage = izena + ">" + message;
                AddMessageToPanel(formattedMessage, true);
                writer.WriteLine(formattedMessage);
                textBoxMessage.Clear();
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un mensaje.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("192.168.115.155", 5555);
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;

                listenerThread = new Thread(ListenForMessages);
                listenerThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListenForMessages()
        {
            try
            {
                string message;
                while ((message = reader.ReadLine()) != null)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        AddMessageToPanel(message, false);
                    }));
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show("Error al escuchar mensajes del servidor: " + ioEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ThreadInterruptedException)
            {
                // El hilo fue interrumpido, salir del bucle de escucha
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al escuchar mensajes del servidor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DisconnectFromServer();
            }
        }


        private void DisconnectFromServer()
        {
            try
            {
                if (listenerThread != null && listenerThread.IsAlive)
                {
                    listenerThread.Interrupt();
                    listenerThread.Join();
                }

                if (writer != null)
                {
                    writer.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (client != null)
                {
                    client.Close();
                }

                MessageBox.Show("Desconectado del servidor.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desconectar del servidor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.Hide();
        }
    }
}
