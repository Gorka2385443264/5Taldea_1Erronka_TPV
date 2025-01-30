using System;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using System.Collections.Generic;
using System.Drawing;
using MySqlX.XDevAPI;
using System.Transactions;
using NHibernate.Mapping;

namespace erronka1_talde5_tpv
{
    public partial class Sortu : Form
    {
        private string nombreUsuario;
        private int IdUsuario;

        // Constructor de Sortu.cs que recibe el nombre de usuario
        public Sortu(string usuario, int idUsuario)
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
                            Text = $"{plato.Id} - {plato.Izena} - {plato.Prezioa}€",
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
                            AutoSize = true,
                            Name = $"{plato.Id}Label"
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
                            int platoId = plato.Id;

                            using (ISession nuevaSession = NHibernateHelper.OpenSession())
                            using (var transaction = nuevaSession.BeginTransaction())
                            {
                                try
                                {
                                    var almazenaPlateraLista = nuevaSession.Query<AlmazenaPlatera>()
                                        .Where(ap => ap.PlateraId == platoId)
                                        .ToList();

                                    bool stockSuficiente = true;

                                    if (almazenaPlateraLista.Any())
                                    {
                                        foreach (var almazena in almazenaPlateraLista)
                                        {
                                            // Verificar y restar el stock
                                            bool result = almazenetikKendu(nuevaSession, almazena.Kantitatea, almazena.AlmazenaId);

                                            if (!result)
                                            {
                                                stockSuficiente = false;
                                                break; // Salir del bucle si falta stock
                                            }
                                        }

                                        if (stockSuficiente)
                                        {
                                            transaction.Commit(); // Confirmar cambios en la base de datos
                                            cantidad++; // Incrementar solo si el stock se actualizó
                                            cantidadLabel.Text = cantidad.ToString();
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            MessageBox.Show("No hay suficiente stock para uno de los ingredientes");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show($"No se encontró ningún AlmazenaId para el Plato ID: {platoId}",
                                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show($"Error: {ex.Message}");
                                }
                            }
                        };



                        // Evento para el botón "-"
                        botonMenos.Click += (sender, e) =>
                        {
                            if (cantidad > 0)
                            {
                                cantidad--;
                                cantidadLabel.Text = cantidad.ToString();
                                int platoId = plato.Id;

                                using (ISession nuevaSession = NHibernateHelper.OpenSession())
                                using (var transaction = nuevaSession.BeginTransaction())
                                {
                                    try
                                    {
                                        var almazenaPlateraLista = nuevaSession.Query<AlmazenaPlatera>()
                                            .Where(ap => ap.PlateraId == platoId)
                                            .ToList();

                                        

                                        if (almazenaPlateraLista.Any())
                                        {
                                            foreach (var almazena in almazenaPlateraLista)
                                            {
                                                // Verificar y restar el stock
                                                almazenetikGehitu(nuevaSession, almazena.Kantitatea, almazena.AlmazenaId);

                                                
                                            }
                                            transaction.Commit(); // Confirmar cambios en la base de datos

                                        }
                                        else
                                        {
                                            MessageBox.Show($"No se encontró ningún AlmazenaId para el Plato ID: {platoId}",
                                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show($"Error: {ex.Message}");
                                    }
                                }
                            }
                        };

                        yPos += 40; // Separar los platos entre sí
                    }

                    yPos += 20; // Separar las categorías entre sí
                }
            }
        }

        private void almazenetikGehitu(ISession nuevaSession, int kantitatea, int almazenaId)
        {
            var almacen = nuevaSession.Get<Stock>(almazenaId);


            // Actualizar cantidad
            almacen.Stock_Kant += kantitatea;
            nuevaSession.Update(almacen); // Actualizar en la sesión

        }

        private bool almazenetikKendu(ISession session, int cantidadRequerida, int almacenId)
        {
            var almacen = session.Get<Stock>(almacenId);

            if (almacen.Stock_Kant < cantidadRequerida)
            {
                return false; // No hay suficiente stock
            }

            // Actualizar cantidad
            almacen.Stock_Kant -= cantidadRequerida;
            session.Update(almacen); // Actualizar en la sesión

            return true;
        }

        private void Sortu_Load(object sender, EventArgs e)
        {

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_mahaia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int numeroMesa = Convert.ToInt32(comboBox_mahaia.SelectedItem?.ToString() ?? "0");
                if (numeroMesa == 0)
                {
                    MessageBox.Show("Selecciona una mesa válida");
                    return;
                }





                List<int> idsPlatos = new List<int>();

                foreach (Control control in this.Controls)
                {

                    if (control is Panel panelCantidad && panelCantidad.Controls.Count > 0)
                    {

                        Label lblCantidad = panelCantidad.Controls.OfType<Label>().FirstOrDefault();
                        if (lblCantidad != null && int.TryParse(lblCantidad.Text, out int cantidad) && cantidad > 0)
                        {
                            // Obtener el Label del plato (asumiendo que está 20px arriba del panel)
                            Label lblPlato = this.Controls.OfType<Label>()
                                .FirstOrDefault(l => l.Location.Y == panelCantidad.Location.Y - 20);

                            if (lblPlato != null)
                            {
                                // Extraer ID del plato
                                int idPlato = Convert.ToInt32(lblPlato.Text.Split('-')[0].Trim());

                                // Añadir el ID tantas veces como la cantidad
                                for (int i = 0; i < cantidad; i++)
                                {
                                    idsPlatos.Add(idPlato);
                                }
                            }
                        }
                    }
                }



                if (idsPlatos.Count == 0)
                {
                    MessageBox.Show("Errore bat gertatu da mapeoarekin");
                    return;
                }



                using (ISession session = NHibernateHelper.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Crear pedido principal
                        var pedido = new EskaeraEntity
                        {
                            MahaiaId = numeroMesa,
                            LangileaId = IdUsuario // Asegúrate de que esta variable existe
                        };
                        session.Save(pedido);

                        // Crear registros individuales para cada plato
                        foreach (int idPlato in idsPlatos)
                        {
                            session.Save(new Eskaera2
                            {
                                EskaeraId = pedido.Id, // Asume que EskaeraEntity tiene Id
                                PlateraId = idPlato
                            });
                        }

                        transaction.Commit();
                        MessageBox.Show("Pedido guardado");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarInterfaz()
        {
            throw new NotImplementedException();
        }
    }
}

        
    