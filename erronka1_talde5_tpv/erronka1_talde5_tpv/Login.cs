﻿using NHibernate;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private NHibernate.Cfg.Configuration miConfiguracion;
        private ISessionFactory mySessionFactory;

        private void Login_Load(object sender, EventArgs e)
        {
            CenterControls();

            // Cambiar colores según lo solicitado
            logInButton.BackColor = ColorTranslator.FromHtml("#BA450D");
            logInButton.ForeColor = Color.White;
            this.BackColor = ColorTranslator.FromHtml("#091725");
            emailText.BackColor = ColorTranslator.FromHtml("#E89E47");
            pasahitzaText.BackColor = ColorTranslator.FromHtml("#E89E47");

            emailText.ForeColor = Color.Black;
            pasahitzaText.ForeColor = Color.Black;

            pasahitzaText.PasswordChar = '*';

            // Asignar la imagen desde los recursos
            
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailText.Text) || string.IsNullOrWhiteSpace(pasahitzaText.Text))
            {
                MessageBox.Show("Por favor, rellena ambos campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            miConfiguracion = new NHibernate.Cfg.Configuration();
            miConfiguracion.Configure();
            mySessionFactory = miConfiguracion.BuildSessionFactory();

            using (var mySession = mySessionFactory.OpenSession())
            {
                using (var transaccion = mySession.BeginTransaction())
                {
                    try
                    {
                        // Consulta HQL para obtener el trabajador por email y contraseña
                        string hql = @"SELECT l FROM Langilea l WHERE l.Email = :emailParam AND l.Pasahitza = :pasahitzaParam";
                        var query = mySession.CreateQuery(hql);
                        query.SetParameter("emailParam", emailText.Text);
                        query.SetParameter("pasahitzaParam", pasahitzaText.Text);

                        var resultado = query.UniqueResult<Langilea>();

                        if (resultado != null)
                        {
                            // Crear y mostrar el formulario Comanda con el ID y nombre del usuario
                            Comanda comandaForm = new Comanda
                            {
                                IdUsuario = resultado.Id, // Guardar el ID del trabajador
                                NombreUsuario = resultado.Izena // Guardar el nombre del trabajador
                            };
                            comandaForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Credenciales incorrectas. ¡Adiós!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        transaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void CenterControls()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            // Posicionar el logo
            logoPictureBox.Left = (this.ClientSize.Width - logoPictureBox.Width) / 2;
            logoPictureBox.Top = 50;

            // Posicionar los campos de texto
            emailText.Left = (this.ClientSize.Width - emailText.Width) / 2;
            emailText.Top = logoPictureBox.Top + logoPictureBox.Height + 20;

            pasahitzaText.Left = emailText.Left;
            pasahitzaText.Top = emailText.Top + emailText.Height + 10;

            // Centrar el botón debajo de los TextBox
            logInButton.Left = (this.ClientSize.Width - logInButton.Width) / 2;
            logInButton.Top = pasahitzaText.Top + pasahitzaText.Height + 20;
        }
    }
}