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
                configuration.Configure(); // Configuración desde app.config

                mySessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void Eskaera_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#091725");

     
            // Botón "Sortu"
            Button sortuButton = new Button
            {
                Text = "Sortu",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.DarkOrange,
                Width = 100,
                Height = 40,
                Top = 10
            };
            sortuButton.Click += SortuButton_Click;
            this.Controls.Add(sortuButton);
            // Título "Las eskaeras de tu restaurante"
            Label tituloLabel = new Label
            {
                Text = "ESKAERAK",
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
                        // O

                        var comandas = session.CreateQuery("FROM EskaeraEntity WHERE Ordainduta = 0 ORDER BY Id DESC")
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

                Button btnEditar = new Button
                {
                    Text = "Editar",
                    Tag = comandasAgrupadas[i].EskaeraId, // Asignar el ID de la comanda como Tag
                    Width = anchoCuadro / 2 - 5,
                    Height = 30,
                    BackColor = Color.Orange,
                    ForeColor = Color.White,
                    Left = 0 // Alineado a la izquierda
                };
                btnEditar.Click += BtnEditar_Click;

                Button btnPagar = new Button
                {
                    Text = "Pagar",
                    Tag = comandasAgrupadas[i].EskaeraId, // Asignar el ID de la comanda como Tag
                    Width = anchoCuadro / 2 - 5,
                    Height = 30,
                    BackColor = Color.Green,
                    ForeColor = Color.White,
                    Left = anchoCuadro / 2 + 5 // Alineado a la derecha
                };
                btnPagar.Click += BtnPagar_Click;

                panelBotones.Controls.Add(btnEditar);
                panelBotones.Controls.Add(btnPagar);

                panelContenedor.Controls.Add(cuadro);
                panelContenedor.Controls.Add(panelBotones); // Añadimos los botones al panel contenedor
            }

            this.Controls.Add(panelContenedor);
        }

        private void BtnPagar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int eskeeraId = (int)btn.Tag;  // Obtener el ID de la comanda desde el Tag

            using (var session = mySessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Obtener la comanda por su ID
                        var eskeera = session.Get<EskaeraEntity>(eskeeraId);

                        if (eskeera != null)
                        {
                            // Cambiar el estado a pagado
                            eskeera.Ordainduta = 1;  // 1 significa "pagada"
                            session.Update(eskeera); // Actualizar la comanda en la base de datos

                            transaction.Commit(); // Confirmar la transacción

                            MessageBox.Show($"Comanda {eskeeraId} ha sido pagada con éxito.", "Pago realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Comanda con ID {eskeeraId} no encontrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar el estado de la comanda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        transaction.Rollback(); // Revertir la transacción en caso de error
                    }
                }
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int eskeeraId = (int)btn.Tag;

            using (var session = mySessionFactory.OpenSession())
            {
                try
                {
                    // Obtener los ítems de Eskaera2
                    var eskaera2Items = session.QueryOver<Eskaera2>()
                        .Where(e2 => e2.EskaeraId == eskeeraId)
                        .List();

                    using (var editForm = new EditNotesForm(eskaera2Items.ToList(), session))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            using (var transaction = session.BeginTransaction())
                            {
                                foreach (var item in eskaera2Items)
                                {
                                    if (editForm.UpdatedNotes.TryGetValue(item.PlateraId, out string nuevaNota))
                                    {
                                        item.NotaGehigarriak = nuevaNota;

                                        // Usar un query más directo para actualizar solo el campo 'NotaGehigarriak'
                                        var updateQuery = session.CreateQuery(
                                            "UPDATE Eskaera2 SET NotaGehigarriak = :nuevaNota WHERE Id = :id"
                                        );
                                        updateQuery.SetParameter("nuevaNota", item.NotaGehigarriak);
                                        updateQuery.SetParameter("id", item.Id);

                                        // Ejecutar la actualización
                                        updateQuery.ExecuteUpdate();
                                    }
                                }
                                transaction.Commit();
                                MessageBox.Show("Notas actualizadas correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshFormData(); // Actualizar la interfaz
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al editar las notas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshFormData()
        {
            // Eliminar el panel actual
            var panelContenedor = this.Controls.OfType<Panel>().FirstOrDefault(p => p.AutoScroll);
            if (panelContenedor != null)
            {
                this.Controls.Remove(panelContenedor);
                panelContenedor.Dispose();
            }

            // Recargar los datos
            Eskaera_Load(this, EventArgs.Empty);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Comanda comandaForm = new Comanda { NombreUsuario = NombreUsuario };
            comandaForm.Show();
            this.Hide();
        }

        private void SortuButton_Click(object sender, EventArgs e)
        {
            // Usar el nombre de usuario desde la propiedad de la clase Eskaera
            string nombreUsuario = this.NombreUsuario;

            // Crear una nueva instancia de Sortu.cs y pasar el nombre de usuario
            Sortu sortuForm = new Sortu(nombreUsuario);

            // Mostrar la nueva pantalla
            sortuForm.Show();

            // Opcional: Ocultar la pantalla actual si ya no se necesita
            this.Hide();
        }



    }
}
