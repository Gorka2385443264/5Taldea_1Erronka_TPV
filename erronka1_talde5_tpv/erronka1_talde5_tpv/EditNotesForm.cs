using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NHibernate;

namespace erronka1_talde5_tpv
{
    public partial class EditNotesForm : Form
    {
        public Dictionary<int, string> UpdatedNotes { get; private set; } = new Dictionary<int, string>();
        private List<Eskaera2> _eskaera2Items;
        private ISession _session; // Instancia de ISession para acceder a la base de datos (NHibernate)

        // Constructor recibe la lista de items y la sesión de NHibernate
        public EditNotesForm(List<Eskaera2> eskaera2Items, ISession session)
        {
            InitializeComponent();
            _eskaera2Items = eskaera2Items;
            _session = session; // Inicializa la sesión de NHibernate
            Load += EditNotesForm_Load; // Cargar los datos al cargar el formulario
        }

        private void EditNotesForm_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Padding = new Padding(10); // Añadir un padding para dar espacio

            // Asegurarse de que los controles solo se agreguen una vez
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                foreach (var item in _eskaera2Items)
                {
                    // Crear un cuadro de texto para mostrar y editar la nota
                    TextBox txtNota = new TextBox
                    {
                        Text = item.NotaGehigarriak, // Asignar la nota actual del plato
                        Tag = item.PlateraId, // Asociar el PlateraId al cuadro de texto (para identificar el plato)
                        Multiline = true,
                        Width = 300,
                        Height = 50
                    };

                    // Añadir el cuadro de texto al panel (sin el nombre del plato)
                    flowLayoutPanel1.Controls.Add(txtNota);
                }

                // Crear los botones de Guardar y Cancelar
                Button btnGuardar = new Button
                {
                    Text = "Guardar",
                    DialogResult = DialogResult.OK,
                    BackColor = Color.Green,
                    ForeColor = Color.White
                };
                btnGuardar.Click += BtnGuardar_Click; // Al hacer clic en Guardar, se procesan las notas

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    DialogResult = DialogResult.Cancel,
                    BackColor = Color.Red,
                    ForeColor = Color.White
                };

                // Añadir los botones al panel solo una vez
                flowLayoutPanel1.Controls.Add(btnGuardar);
                flowLayoutPanel1.Controls.Add(btnCancelar);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    // Iterar a través de los elementos para actualizar solo la nota_gehigarriak
                    foreach (var item in _eskaera2Items)
                    {
                        if (UpdatedNotes.ContainsKey(item.PlateraId)) // Aquí se busca en UpdatedNotes
                        {
                            string nuevaNota = UpdatedNotes[item.PlateraId]; // Se obtiene la nueva nota

                            // Verificar que el Id de la entidad Eskaera2 es válido
                            if (item.Id > 0) // Verifica que el Id sea válido
                            {
                                var eskaera2 = _session.Get<Eskaera2>(item.Id); // Obtener la entidad Eskaera2 con el Id

                                if (eskaera2 != null)
                                {
                                    Console.WriteLine($"Actualizando NotaGehigarriak para Id: {item.Id}, NotaGehigarriak: {nuevaNota}");

                                    // Actualizar la propiedad NotaGehigarriak de la entidad
                                    eskaera2.NotaGehigarriak = nuevaNota;

                                    // Guardar los cambios realizados en la entidad
                                    _session.Update(eskaera2);
                                }
                                else
                                {
                                    Console.WriteLine($"No se encontró la entidad Eskaera2 con Id: {item.Id}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Id no válido: {item.Id}");
                            }
                        }
                    }

                    transaction.Commit(); // Confirmar los cambios en la base de datos
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Deshacer cambios en caso de error
                    MessageBox.Show($"Error al editar las notas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
