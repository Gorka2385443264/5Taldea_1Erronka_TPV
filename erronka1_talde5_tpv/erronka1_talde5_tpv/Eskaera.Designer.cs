using System;
using System.Drawing;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    partial class Eskaera
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnComanda;
        private Label welcomeLabel; // Agregar el Label para el saludo

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.btnComanda = new System.Windows.Forms.Button();
            this.welcomeLabel = new System.Windows.Forms.Label(); // Inicializar el Label
            this.SuspendLayout();

            // welcomeLabel
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.ForeColor = System.Drawing.Color.White;
         //   welcomeLabel.Text = $"¡Hola, {NombreUsuario}!";

            // Posicionamiento inicial
            AjustarPosicionWelcomeLabel();

            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(200, 30); // Ajusta el tamaño según el texto

            // btnComanda
            this.btnComanda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnComanda.Location = new System.Drawing.Point(20, 380); // Posición predeterminada
            this.btnComanda.Name = "btnComanda";
            this.btnComanda.Size = new System.Drawing.Size(150, 50);
            this.btnComanda.TabIndex = 0;
            this.btnComanda.Text = "Volver";
            this.btnComanda.BackColor = ColorTranslator.FromHtml("#E89E47");
            this.btnComanda.UseVisualStyleBackColor = true;
            this.btnComanda.Click += new System.EventHandler(this.BackButton_Click);

            // Eskaera
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.welcomeLabel); // Agregar el Label a los controles
            this.Controls.Add(this.btnComanda);
            this.Name = "Eskaera";
            this.Text = "Eskaera";
            this.Load += new System.EventHandler(this.Eskaera_Load);
            this.Resize += new System.EventHandler(this.Eskaera_Resize);  // Evento para cuando cambie el tamaño de la ventana
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Función para ajustar la posición del welcomeLabel
        private void AjustarPosicionWelcomeLabel()
        {
            this.welcomeLabel.Location = new System.Drawing.Point(
                (this.ClientSize.Width - this.welcomeLabel.Width) / 2, // Centrado horizontal
                70 // Alineado un poco más abajo (antes estaba en 10)
            );
        }


        // Evento que maneja el redimensionamiento de la ventana
        private void Eskaera_Resize(object sender, EventArgs e)
        {
            AjustarPosicionWelcomeLabel();  // Vuelve a ajustar la posición del welcomeLabel cuando se cambia el tamaño de la ventana
        }

        #endregion
    }
}
