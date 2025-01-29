using System;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using System.Collections.Generic;
using System.Drawing;

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

            // Llamar a la función para cargar y mostrar los platos
            MostrarPlatos();
        }

        // Evento Click del botón "Volver"
        private void volverButton_Click(object sender, EventArgs e)
        {
            // Aquí navegas a la pantalla de Eskaera
            Eskaera eskaeraScreen = new Eskaera();  // Cambia esto según cómo sea tu clase de Eskaera
            eskaeraScreen.Show();
            this.Hide(); // Ocultar la pantalla actual (Sortu)
        }

        // Función para cargar y mostrar los platos desde la base de datos
        private void MostrarPlatos()
        {
            // Obtener la sesión de NHibernate
            using (ISession session = NHibernateHelper.OpenSession())
            {
                // Obtener todos los platos desde la base de datos
                var platos = session.Query<Platera>().ToList();

                // Agrupar los platos por categoría
                var categorias = platos.GroupBy(p => p.PlateraMota)
                                       .OrderBy(g => g.Key == "Edariak" ? 1 :
                                                     g.Key == "Lehen_Platera" ? 2 :
                                                     g.Key == "Bigarren_Platera" ? 3 :
                                                     g.Key == "Postrea" ? 4 : 5) // Orden específico: Edariak, Lehen, Bigarren, Postrea
                                       .ToList();

                int yPos = 60; // Posición inicial para mostrar los platos

                foreach (var categoria in categorias)
                {
                    // Crear un Label para cada categoría
                    Label categoriaLabel = new Label
                    {
                        Text = categoria.Key,
                        Font = new Font("Arial", 16F, FontStyle.Bold),
                        Location = new Point(20, yPos),
                        AutoSize = true
                    };
                    this.Controls.Add(categoriaLabel);

                    yPos += 30; // Separar un poco las categorías

                    // Mostrar los platos dentro de cada categoría
                    foreach (var plato in categoria)
                    {
                        // Crear un Label para cada plato
                        Label platoLabel = new Label
                        {
                            Text = $"{plato.Izena} - {plato.Prezioa}€",
                            Font = new Font("Arial", 12F),
                            Location = new Point(40, yPos),
                            AutoSize = true
                        };
                        this.Controls.Add(platoLabel);

                        // Crear un contenedor de botones para "+" y "-" y la cantidad
                        Panel cantidadPanel = new Panel
                        {
                            Location = new Point(250, yPos), // Ajustar según sea necesario
                            Size = new Size(120, 30) // Tamaño del contenedor
                        };
                        this.Controls.Add(cantidadPanel);

                        // Crear el botón "+"
                        Button botonMas = new Button
                        {
                            Text = "+",
                            Size = new Size(30, 30),
                            Location = new Point(0, 0),
                            Font = new Font("Arial", 12F)
                        };
                        cantidadPanel.Controls.Add(botonMas);

                        // Crear el número (Label) entre los botones
                        Label cantidadLabel = new Label
                        {
                            Text = "0", // Inicialmente 0
                            Font = new Font("Arial", 12F),
                            Location = new Point(35, 0), // Ajustar según sea necesario
                            AutoSize = true
                        };
                        cantidadPanel.Controls.Add(cantidadLabel);

                        // Crear el botón "-"
                        Button botonMenos = new Button
                        {
                            Text = "-",
                            Size = new Size(30, 30),
                            Location = new Point(70, 0),
                            Font = new Font("Arial", 12F)
                        };
                        cantidadPanel.Controls.Add(botonMenos);

                        // Variable para manejar la cantidad
                        int cantidad = 0;

                     // Evento para el botón "+"
                        botonMas.Click += (sender, e) =>
                        {
                            cantidad++;
                            cantidadLabel.Text = cantidad.ToString();
    
                            // Obtener el ID del plato (equivalente a PlateraId)
                            int platoId = plato.Id;

                            // Obtener el AlmazenaId correspondiente a ese plato
                            using (ISession nuevaSession = NHibernateHelper.OpenSession())
                            {
                                var almazenaPlateraLista = nuevaSession.Query<AlmazenaPlatera>()
                                                                       .Where(ap => ap.PlateraId == platoId)
                                                                       .ToList();

                                if (almazenaPlateraLista.Any())
                                {
                                    string mensaje = $"Plato ID (PlateraId): {platoId}\nAlmacenes encontrados:\n";
                                    foreach (var almazena in almazenaPlateraLista)
                                    {
                                        mensaje += $"- Almacén ID (AlmazenaId): {almazena.AlmazenaId}\n";
                                    }

                                    MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show($"No se encontró ningún AlmazenaId para el Plato ID: {platoId}",
                                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        };



                        // Evento para el botón "-"
                        botonMenos.Click += (sender, e) =>
                        {
                            if (cantidad > 0) // No permitir que la cantidad sea negativa
                            {
                                cantidad--;
                                cantidadLabel.Text = cantidad.ToString();
                            }
                        };

                        yPos += 25; // Separar los platos entre sí
                    }

                    yPos += 20; // Separar las categorías entre sí
                }
            }
        }
    }
}
