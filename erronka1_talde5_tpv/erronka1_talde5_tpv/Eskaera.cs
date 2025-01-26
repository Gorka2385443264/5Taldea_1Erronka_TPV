using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Eskaera : Form
    {
        private ISessionFactory mySessionFactory;
        private string metodoPagoSeleccionado; // Variable para guardar el método de pago seleccionado
        public static List<EskaeraPagada> EskaerasPagadas = new List<EskaeraPagada>();

        public string NombreUsuario { get; set; }

        public Eskaera()
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
                configuration.Configure(); // Lee el app.config (incluye <mapping assembly>)

                mySessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Lanzar la excepción para detener la ejecución
            }
        }

        private void Eskaera_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#091725");

            if (mySessionFactory == null)
            {
                MessageBox.Show("NHibernate no se configuró correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var session = mySessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Obtener todas las comandas (solo lectura)
                        var comandas = session.CreateQuery("FROM Eskaera2")
                                              .SetReadOnly(true)
                                              .List<Eskaera2>();

                        // Agrupar comandas por eskaera_id
                        var comandasAgrupadas = comandas
                            .GroupBy(c => c.EskaeraId)
                            .Select(g => (
                                EskaeraId: g.Key,
                                Platos: g.Select(c => (
                                    PlatoId: c.PlateraId,
                                    Notas: c.NotaGehigarriak,
                                    Hora: c.EskaeraOrdua
                                )).ToList()
                            )).ToList();

                        // Mostrar los datos agrupados
                        DisplayComandasAgrupadas(comandasAgrupadas);
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

        private void DisplayComandasAgrupadas(List<(int EskaeraId, List<(int PlatoId, string Notas, DateTime Hora)> Platos)> comandasAgrupadas)
        {
            Panel panelContenedor = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#091725")
            };

            // Configuración del layout
            int anchoCuadro = 350;  // Ancho fijo del cuadro
            int altoCuadro = 200;   // Altura fija del cuadro
            int separacion = 15;    // Espacio entre cuadros
            int maxColumnas = 3;    // Máximo de columnas por fila

            int margenIzquierdo = (this.ClientSize.Width - (maxColumnas * (anchoCuadro + separacion))) / 2;
            margenIzquierdo = Math.Max(margenIzquierdo, 15);

            for (int i = 0; i < comandasAgrupadas.Count; i++)
            {
                int fila = i / maxColumnas;
                int columna = i % maxColumnas;

                // Panel principal del cuadro
                Panel cuadro = new Panel
                {
                    Width = anchoCuadro,
                    Height = altoCuadro,
                    Left = margenIzquierdo + columna * (anchoCuadro + separacion),
                    Top = 15 + fila * (altoCuadro + separacion + 140), // +140 para los botones y el precio total
                    BackColor = ColorTranslator.FromHtml("#BA450D"),
                    Padding = new Padding(10)
                };

                // ---- Comanda ID (Grande, arriba a la izquierda) ----
                Label lblComandaId = new Label
                {
                    Text = $"Comanda ID: {comandasAgrupadas[i].EskaeraId}",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // ---- Espacio entre Comanda ID y Plato ----
                Panel espacio = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 10,
                    BackColor = Color.Transparent
                };

                // ---- Contenedor para los platos ----
                Panel panelPlatos = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.Transparent
                };

                // ---- Calcular el precio total de la comanda ----
                decimal precioTotal = 0;

                // ---- Agregar cada plato y sus notas ----
                foreach (var plato in comandasAgrupadas[i].Platos)
                {
                    // Obtener el nombre y el precio del plato
                    var platoInfo = ObtenerNombreYPrecioPlato(plato.PlatoId);
                    string nombrePlato = platoInfo.Nombre;
                    decimal precioPlato = platoInfo.Precio;

                    // Sumar al precio total
                    precioTotal += precioPlato;

                    // Crear el texto del plato con el precio
                    Label lblPlato = new Label
                    {
                        Text = $"PLATO: {nombrePlato}\nNOTAS: {plato.Notas ?? "Ninguna"}\nHORA: {plato.Hora.ToString("HH:mm")}",
                        Font = new Font("Arial", 10),
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        Margin = new Padding(0, 0, 0, 10)  // Margen inferior entre platos
                    };

                    // Crear el label para el precio (más pequeño y menos llamativo)
                    Label lblPrecio = new Label
                    {
                        Text = $"Precio: {precioPlato.ToString("C")}", // Formato de moneda
                        Font = new Font("Arial", 8, FontStyle.Italic),
                        ForeColor = Color.LightGray, // Color gris claro
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        Margin = new Padding(0, 0, 0, 5)  // Margen inferior pequeño
                    };

                    panelPlatos.Controls.Add(lblPlato);
                    panelPlatos.Controls.Add(lblPrecio);

                    // Agregar un separador entre platos
                    Panel separador = new Panel
                    {
                        Height = 1,
                        Dock = DockStyle.Top,
                        BackColor = Color.White
                    };
                    panelPlatos.Controls.Add(separador);
                }

                // Añadir controles al cuadro principal
                cuadro.Controls.Add(panelPlatos);  // Platos y notas
                cuadro.Controls.Add(espacio);      // Espacio entre Comanda ID y Plato
                cuadro.Controls.Add(lblComandaId); // Comanda ID en la parte superior

                panelContenedor.Controls.Add(cuadro);

                // ---- Mostrar el Precio Total debajo del cuadro ----
                Label lblPrecioTotal = new Label
                {
                    Text = $"Precio Total: {precioTotal.ToString("C")}", // Formato de moneda
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(cuadro.Left, cuadro.Bottom + 5) // Debajo del cuadro
                };
                panelContenedor.Controls.Add(lblPrecioTotal);

                // ---- Botón "Editar" debajo del Precio Total ----
                Button btnEditar = new Button
                {
                    Text = $"Editar Comanda {comandasAgrupadas[i].EskaeraId}",
                    Size = new Size(anchoCuadro, 40),
                    Location = new Point(cuadro.Left, lblPrecioTotal.Bottom + 5), // Debajo del Precio Total
                    BackColor = ColorTranslator.FromHtml("#E89E47"),
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Tag = comandasAgrupadas[i].EskaeraId // Asignar el ID de la comanda al botón
                };
                btnEditar.Click += BtnEditar_Click; // Asignar el evento de clic
                panelContenedor.Controls.Add(btnEditar);

                // ---- Botón "Pagar" debajo del botón "Editar" ----
                Button btnPagar = new Button
                {
                    Text = $"Pagar Comanda {comandasAgrupadas[i].EskaeraId}",
                    Size = new Size(anchoCuadro, 40),
                    Location = new Point(cuadro.Left, btnEditar.Bottom + 5), // Debajo del botón "Editar"
                    BackColor = ColorTranslator.FromHtml("#4CAF50"), // Verde para el botón "Pagar"
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Tag = comandasAgrupadas[i].EskaeraId // Asignar el ID de la comanda al botón
                };
                btnPagar.Click += BtnPagar_Click; // Asignar el evento de clic
                panelContenedor.Controls.Add(btnPagar);
            }

            this.Controls.Add(panelContenedor);

            // Botón de volver
            Button btnVolver = new Button
            {
                Text = "Volver",
                Size = new Size(120, 40),
                Location = new Point(20, this.ClientSize.Height - 70),
                BackColor = ColorTranslator.FromHtml("#E89E47"),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnVolver.Click += BtnVolver_Click;
            this.Controls.Add(btnVolver);
        }

        // Evento para manejar el clic en el botón "Editar"
        private void BtnEditar_Click(object sender, EventArgs e)
        {
            Button btnEditar = sender as Button;
            if (btnEditar != null)
            {
                int eskaeraId = (int)btnEditar.Tag; // Obtener el ID de la comanda desde el Tag
                MessageBox.Show($"Editar Comanda ID: {eskaeraId}", "Editar Comanda", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Aquí puedes implementar la lógica para editar la comanda
                // Por ejemplo, abrir un formulario de edición con el ID de la comanda
            }
        }

        // Evento para manejar el clic en el botón "Pagar"
        // Evento para manejar el clic en el botón "Pagar"
        private void BtnPagar_Click(object sender, EventArgs e)
        {
            Button btnPagar = sender as Button;
            if (btnPagar != null)
            {
                int eskaeraId = (int)btnPagar.Tag;

                // Mostrar un MessageBox con opciones de pago
                var result = MessageBox.Show(
                    "Seleccione el método de pago:\n\n¿Desea pagar en efectivo?",
                    "Método de Pago",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1
                );

                // Guardar la opción seleccionada
                if (result == DialogResult.Yes)
                {
                    metodoPagoSeleccionado = "Efectivo";
                }
                else if (result == DialogResult.No)
                {
                    metodoPagoSeleccionado = "Tarjeta";
                }

                // Calcular el precio total de la eskaera
                decimal precioTotal = CalcularPrecioTotal(eskaeraId);

                // Guardar la eskaera pagada en la lista estática
                EskaerasPagadas.Add(new EskaeraPagada
                {
                    EskaeraId = eskaeraId,
                    PrecioTotal = precioTotal,
                    MetodoPago = metodoPagoSeleccionado,
                    FechaPago = DateTime.Now
                });

                MessageBox.Show($"Eskaera {eskaeraId} pagada con {metodoPagoSeleccionado}.", "Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private decimal CalcularPrecioTotal(int eskaeraId)
        {
            using (var session = mySessionFactory.OpenSession())
            {
                var comandas = session.CreateQuery("FROM Eskaera2 WHERE EskaeraId = :eskaeraId")
                                      .SetParameter("eskaeraId", eskaeraId)
                                      .List<Eskaera2>();

                decimal precioTotal = 0;
                foreach (var comanda in comandas)
                {
                    var plato = ObtenerNombreYPrecioPlato(comanda.PlateraId);
                    precioTotal += plato.Precio;
                }

                return precioTotal;
            }
        }        // Método para obtener el nombre y el precio de un plato
        private (string Nombre, decimal Precio) ObtenerNombreYPrecioPlato(int plateraId)
        {
            try
            {
                using (var session = mySessionFactory.OpenSession())
                {
                    var plato = session.Get<Platera>(plateraId);
                    return (plato?.Izena ?? "Plato desconocido", plato?.Prezioa ?? 0); // Devuelve nombre y precio
                }
            }
            catch
            {
                return ("Error al cargar el nombre", 0); // Devuelve un valor predeterminado en caso de error
            }
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            // Cerrar la ventana actual (Eskaera.cs)
            this.Close();

            // Abrir la ventana Comanda.cs y pasar el nombre de usuario
            Comanda comandaForm = new Comanda
            {
                NombreUsuario = this.NombreUsuario // Pasar el nombre de usuario actual
            };
            comandaForm.Show();
        }
    }
}