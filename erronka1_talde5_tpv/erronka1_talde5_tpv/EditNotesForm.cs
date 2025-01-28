using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using erronka1_talde5_tpv;
using System.Drawing;

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
            try
            {
                // Guardar las notas editadas en el diccionario
                foreach (var control in flowLayoutPanel1.Controls.OfType<TextBox>())
                {
                    var plateraId = (int)control.Tag;
                    UpdatedNotes[plateraId] = control.Text; // Guardar la nueva nota
                }

                // Solo actualizar el campo 'nota_gehigarriak' en la tabla 'eskaera_platera'
                using (var session = NHibernateHelper.OpenSession()) // Usando NHibernateHelper para la sesión
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var item in _eskaera2Items)
                        {
                            if (UpdatedNotes.TryGetValue(item.PlateraId, out string nuevaNota))
                            {
                                item.NotaGehigarriak = nuevaNota; // Asignar la nueva nota a la propiedad

                                // Actualizar solo el campo 'nota_gehigarriak' en la base de datos
                                var updateQuery = session.CreateQuery(
                                    "UPDATE Eskaera2 SET NotaGehigarriak = :notaGehigarriak WHERE Id = :id"
                                );

                                updateQuery.SetParameter("notaGehigarriak", item.NotaGehigarriak);
                                updateQuery.SetParameter("id", item.Id);

                                // Ejecutar la actualización
                                updateQuery.ExecuteUpdate();
                            }
                        }
                        transaction.Commit();  // Confirmar la transacción
                        MessageBox.Show("Notas actualizadas correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                DialogResult = DialogResult.OK; // Cerrar el formulario con el resultado de OK
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar las notas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnEditar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int eskaeraId = (int)btn.Tag;

            using (var session = NHibernateHelper.OpenSession()) // Usando NHibernateHelper para la sesión
            {
                try
                {
                    // Obtener los ítems de Eskaera2
                    var eskaera2Items = session.QueryOver<Eskaera2>()
                        .Where(e2 => e2.EskaeraId == eskaeraId)
                        .List();

                    using (var editForm = new EditNotesForm(eskaera2Items.ToList(), session))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            using (var transaction = session.BeginTransaction())
                            {
                                // Solo actualizar las notas
                                foreach (var item in eskaera2Items)
                                {
                                    if (editForm.UpdatedNotes.TryGetValue(item.PlateraId, out string nuevaNota))
                                    {
                                        item.NotaGehigarriak = nuevaNota;

                                        // Actualizar solo el campo 'NotaGehigarriak' en la tabla Eskaera2
                                        var updateQuery = session.CreateQuery(
                                            "UPDATE Eskaera2 SET NotaGehigarriak = :notaGehigarriak WHERE Id = :id"
                                        );

                                        updateQuery.SetParameter("notaGehigarriak", item.NotaGehigarriak);
                                        updateQuery.SetParameter("id", item.Id);

                                        // Ejecutar la actualización
                                        updateQuery.ExecuteUpdate();
                                    }
                                }
                                transaction.Commit();  // Confirmar la transacción
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
            try
            {
                using (var session = NHibernateHelper.OpenSession()) // Usando NHibernateHelper para la sesión
                {
                    var datos = session.QueryOver<Eskaera2>().List<Eskaera2>();

                    // Recargar datos en la interfaz o hacer actualizaciones necesarias
                    // Ejemplo de actualización en los controles
                    foreach (var item in datos)
                    {
                        Console.WriteLine(item.NotaGehigarriak);  // Actualiza los datos como sea necesario
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    ConfigureNHibernate();
                }
                return _sessionFactory;
            }
        }

        // Método para configurar NHibernate
        private static void ConfigureNHibernate()
        {
            try
            {
                var configuration = new NHibernate.Cfg.Configuration();
                configuration.Configure(); // Configuración desde app.config o hibernate.cfg.xml

                _sessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                // Mostrar el error si ocurre algo durante la configuración
                MessageBox.Show($"Error al configurar NHibernate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Método para obtener una nueva sesión
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
