using NHibernate;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SelectPdf; 

namespace erronka1_talde5_tpv
{
    public partial class Caja : Form
    {
        private ISessionFactory mySessionFactory;
        private string metodoPagoSeleccionado; // Variable para guardar el método de pago seleccionado
        public static List<EskaeraPagada> EskaerasPagadas = new List<EskaeraPagada>();

        public string NombreUsuario { get; set; }

        public Caja()
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
                configuration.Configure(); // Configuración desde app.config

                mySessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void Caja_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#091725");

            Label tituloLabel = new Label
            {
                Text = "CAJA",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = ColorTranslator.FromHtml("#BA450D")
            };

            this.Controls.Add(tituloLabel); // Agregar el título a los controles de la forma

            if (mySessionFactory == null)
            {
                MessageBox.Show("NHibernate no se configuró correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            welcomeLabel.Text = $"¡Hola, {NombreUsuario}!";
            AjustarPosicionWelcomeLabel();

            using (var session = mySessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // 1
                        var comandas = session.CreateQuery("FROM EskaeraEntity WHERE Ordainduta = 1 ORDER BY Id ASC")
                                                                      .SetReadOnly(true)
                                              .List<EskaeraEntity>();

                        var comandasDetalles = new List<(int EskaeraId, List<(int PlatoId, string Notas, DateTime Hora, int Precio, string NombrePlato)>)>();

                        foreach (var comanda in comandas)
                        {
                            var platos = session.CreateQuery("FROM Eskaera2 WHERE EskaeraId = :eskaeraId")
                                                .SetParameter("eskaeraId", comanda.Id)
                                                .SetReadOnly(true)
                                                .List<Eskaera2>();

                            var platosDetalles = platos.Select(p => (
                                PlatoId: p.PlateraId,
                                Notas: p.NotaGehigarriak,
                                Hora: p.EskaeraOrdua,
                                Precio: ObtenerPrecioPlato(session, p.PlateraId),
                                NombrePlato: ObtenerNombrePlato(session, p.PlateraId)
                            )).ToList();

                            comandasDetalles.Add((EskaeraId: comanda.Id, Platos: platosDetalles));
                        }

                        // Mostrar los datos agrupados
                        DisplayComandasAgrupadas(comandasDetalles);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar comandas: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback();
                    }
                }
            }
        }

        private int ObtenerPrecioPlato(ISession session, int platoId)
        {
            var plato = session.Get<Platera>(platoId);
            return plato?.Prezioa ?? 0;
        }

        private string ObtenerNombrePlato(ISession session, int platoId)
        {
            if (session == null) return "Plato no encontrado";

            // Obtener el plato desde la base de datos
            var plato = session.Get<Platera>(platoId);

            // Verificar si el plato existe
            return plato?.Izena ?? "Plato no encontrado";
        }

        private void DisplayComandasAgrupadas(List<(int EskaeraId, List<(int PlatoId, string Notas, DateTime Hora, int Precio, string NombrePlato)> Platos)> comandasAgrupadas)
        {
            Panel panelContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#091725")
            };

            int anchoCuadro = 350;
            int altoCuadro = 200;
            int separacion = 15;
            int maxColumnas = 3;

            int margenIzquierdo = (this.ClientSize.Width - (maxColumnas * (anchoCuadro + separacion))) / 2;
            margenIzquierdo = Math.Max(margenIzquierdo, 15);

            for (int i = 0; i < comandasAgrupadas.Count; i++)
            {
                int fila = i / maxColumnas;
                int columna = i % maxColumnas;

                Panel cuadro = new Panel
                {
                    Width = anchoCuadro,
                    Height = altoCuadro,
                    Left = margenIzquierdo + columna * (anchoCuadro + separacion),
                    Top = welcomeLabel.Bottom + separacion + fila * (altoCuadro + separacion + 140),
                    BackColor = ColorTranslator.FromHtml("#BA450D"),
                    Padding = new Padding(10)
                };

                Label lblComandaId = new Label
                {
                    Text = $"Comanda ID: {comandasAgrupadas[i].EskaeraId}",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Panel espacio = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 10,
                    BackColor = Color.Transparent
                };

                Panel panelPlatos = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.Transparent
                };

                decimal precioTotal = 0;

                foreach (var plato in comandasAgrupadas[i].Platos)
                {
                    precioTotal += plato.Precio;

                    Label lblPlato = new Label
                    {
                        Text = $"PLATO: {plato.NombrePlato}\nNOTAS: {plato.Notas ?? "Ninguna"}\nHORA: {plato.Hora:HH:mm}",
                        Font = new Font("Arial", 10),
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        AutoSize = true
                    };

                    Label lblPrecio = new Label
                    {
                        Text = $"Precio: {plato.Precio.ToString("C")}",
                        Font = new Font("Arial", 8, FontStyle.Italic),
                        ForeColor = Color.LightGray,
                        Dock = DockStyle.Top,
                        AutoSize = true
                    };

                    panelPlatos.Controls.Add(lblPlato);
                    panelPlatos.Controls.Add(lblPrecio);
                }

                cuadro.Controls.Add(panelPlatos);
                cuadro.Controls.Add(espacio);
                cuadro.Controls.Add(lblComandaId);

                // Crear un panel para los botones, fuera del cuadro
                Panel panelBotones = new Panel
                {
                    Width = anchoCuadro,
                    Height = 60, // Un poco de espacio para los botones
                    Left = cuadro.Left,
                    Top = cuadro.Bottom + separacion,
                    BackColor = Color.Transparent
                };

                Button btnCrearPDF = new Button
                {
                    Text = "Crear PDF",
                    Tag = comandasAgrupadas[i].EskaeraId, // Asignar el ID de la comanda como Tag
                    Width = anchoCuadro,
                    Height = 30,
                    BackColor = Color.Blue,
                    ForeColor = Color.White
                };
                btnCrearPDF.Click += BtnCrearPDF_Click;

                panelBotones.Controls.Add(btnCrearPDF);

                panelContenedor.Controls.Add(cuadro);
                panelContenedor.Controls.Add(panelBotones); // Añadimos el botón al panel contenedor
            }

            this.Controls.Add(panelContenedor);

            // Agregar botón "Volver" al panel de la parte inferior de la pantalla
            Button backButton = new Button
            {
                Text = "Volver",
                Width = 100,
                Height = 40,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Left = (this.ClientSize.Width - 100) / 2, // Centrado en la parte inferior
                Top = this.ClientSize.Height - 60, // Posición en la parte inferior
            };
            backButton.Click += BackButton_Click;

            this.Controls.Add(backButton);
        }



        private void BtnCrearPDF_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int eskeeraId = (int)btn.Tag;

            try
            {
                // 1. Ruta del logo
                string logoPath = @"C:\Users\barto\OneDrive\Escritorio\erronka1_talde5_tpv\erronka1_talde5_tpv\Resources\saboreame.png";

                // Verificar si existe el logo
                if (!File.Exists(logoPath))
                {
                    MessageBox.Show("El archivo del logo no se encontró en la ruta especificada",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertir ruta a URI válido
                string logoUri = new Uri(logoPath).AbsoluteUri;
                logoUri = logoUri.Replace(" ", "%20");

                using (var session = mySessionFactory.OpenSession())
                {
                    // 2. Obtener datos de la comanda
                    var eskaera = session.Get<EskaeraEntity>(eskeeraId);
                    var platos = session.CreateQuery("FROM Eskaera2 WHERE EskaeraId = :id")
                                       .SetParameter("id", eskeeraId)
                                       .List<Eskaera2>();

                    // 3. Construir HTML
                    StringBuilder html = new StringBuilder();
                    html.AppendLine("<!DOCTYPE html>");
                    html.AppendLine("<html>");
                    html.AppendLine("<head>");
                    html.AppendLine("<meta charset='UTF-8'>");
                    html.AppendLine("<style>");
                    html.AppendLine("body { font-family: Arial, sans-serif; margin: 25px; }");
                    html.AppendLine(".header { text-align: center; margin-bottom: 25px; border-bottom: 2px solid #BA450D; padding-bottom: 15px; }");
                    html.AppendLine(".logo { max-width: 200px; height: auto; margin-bottom: 10px; }");
                    html.AppendLine("h1 { color: #BA450D; font-size: 24px; margin: 15px 0; }");
                    html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
                    html.AppendLine("th { background-color: #f8f9fa; color: #091725; padding: 12px; text-align: left; }");
                    html.AppendLine("td { padding: 10px; border-bottom: 1px solid #eee; }");
                    html.AppendLine(".total { font-size: 18px; color: #091725; margin-top: 25px; font-weight: bold; }");
                    html.AppendLine(".info-empresa { color: #666; font-size: 14px; margin-top: 5px; }");
                    html.AppendLine("</style>");
                    html.AppendLine("</head>");
                    html.AppendLine("<body>");

                    // Cabecera con logo y fecha/hora
                    string fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    html.AppendLine($@"<div class='header'>
                <img src='{logoUri}' class='logo'/>
                <h1>{fechaHora}</h1>
                <div class='info-empresa'>
                    <p>Saboreame Restaurante</p>
                    <p>C/Gran Vía, 12 - 48001 Bilbao</p>
                    <p>Tel: 944 123 456 | CIF: B12345678</p>
                </div>
              </div>");

                    html.AppendLine("<table>");
                    html.AppendLine("<tr><th>Plato</th><th>Precio</th></tr>");

                    decimal total = 0;
                    foreach (var plato in platos)
                    {
                        string nombre = ObtenerNombrePlato(session, plato.PlateraId);
                        int precio = ObtenerPrecioPlato(session, plato.PlateraId);
                        total += precio;

                        html.AppendLine("<tr>");
                        html.AppendLine($"<td>{nombre}</td>");
                        html.AppendLine($"<td>{precio:C}</td>");
                        html.AppendLine("</tr>");
                    }

                    html.AppendLine("</table>");
                    html.AppendLine($"<p class='total'>Total: {total:C}</p>");
                    html.AppendLine("</body></html>");

                    // 4. Configuración PDF
                    HtmlToPdf converter = new HtmlToPdf();
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    converter.Options.WebPageWidth = 1200;

                    // 5. Generar PDF
                    PdfDocument doc = converter.ConvertHtmlString(html.ToString());

                    // 6. Guardar PDF
                    string documentosPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string pdfFileName = $"Comanda_{eskeeraId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                    string pdfPath = Path.Combine(documentosPath, pdfFileName);

                    doc.Save(pdfPath);
                    doc.Close();

                    // 7. Mostrar confirmación
                    MessageBox.Show($"PDF generado con éxito:\n{pdfPath}",
                                      "PDF Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btn.Enabled = false;

                    // Get the parent panel (panelBotones)
                    Panel parentPanel = (Panel)btn.Parent;

                    // Check if "TODO LISTO" button already exists to avoid duplicates
                    var existingTodoListo = parentPanel.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "TODO LISTO");
                    if (existingTodoListo == null)
                    {
                        // Create the "TODO LISTO" button
                        Button btnTodoListo = new Button
                        {
                            Text = "TODO LISTO",
                            Width = btn.Width,
                            Height = 30, // Adjusted height for better fit
                            BackColor = Color.Green,
                            ForeColor = Color.White,
                            Top = btn.Bottom + 10, // Position below "Crear PDF"
                            Left = btn.Left
                        };

                        btnTodoListo.Click += (s, ev) =>
                        {
                            
                        };

                        // Adjust the parent panel's height to fit the new button
                        parentPanel.Height = btnTodoListo.Top + btnTodoListo.Height + 10; // Add padding

                        // Add the button to the parent panel
                        parentPanel.Controls.Add(btnTodoListo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}\n\nDetalles técnicos:\n{ex.StackTrace}",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BackButton_Click(object sender, EventArgs e)
    {
        Comanda comandaForm = new Comanda { NombreUsuario = NombreUsuario };
        comandaForm.Show();
        this.Hide();
    }
}
}