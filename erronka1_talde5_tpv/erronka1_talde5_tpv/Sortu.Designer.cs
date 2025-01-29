using System;
using System.Drawing;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    partial class Sortu
    {
        private System.ComponentModel.IContainer components = null;
        private Button volverButton;  // Declaración del botón
        private Label nombreUsuarioLabel;

        // Aquí va el código de inicialización
        private void InitializeComponent()
        {
            this.nombreUsuarioLabel = new System.Windows.Forms.Label();
            this.volverButton = new System.Windows.Forms.Button();  // Crear el botón

            // 
            // nombreUsuarioLabel
            // 
            this.nombreUsuarioLabel.AutoSize = true;
            this.nombreUsuarioLabel.Font = new System.Drawing.Font("Arial", 16F);
            this.nombreUsuarioLabel.ForeColor = System.Drawing.Color.Black; // Color negro
            this.nombreUsuarioLabel.Location = new System.Drawing.Point(0, 0);  // Se actualizará dinámicamente en el código
            this.nombreUsuarioLabel.Name = "nombreUsuarioLabel";
            this.nombreUsuarioLabel.Size = new System.Drawing.Size(0, 26);
            this.nombreUsuarioLabel.TabIndex = 0;

            // 
            // volverButton
            // 
            this.volverButton.Text = "Volver";
            this.volverButton.Font = new Font("Arial", 12);
            this.volverButton.Size = new Size(100, 40); // Tamaño del botón
            this.volverButton.Location = new System.Drawing.Point(20, 380); // Posición predeterminada
            this.volverButton.TabIndex = 1;

            // Aquí se le agrega el evento Click al botón
            this.volverButton.Click += new System.EventHandler(this.volverButton_Click);

            // 
            // Sortu
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nombreUsuarioLabel);
            this.Controls.Add(this.volverButton);  // Añadir el botón al formulario
            this.Name = "Sortu";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
