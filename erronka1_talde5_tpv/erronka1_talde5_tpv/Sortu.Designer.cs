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
            this.volverButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nombreUsuarioLabel
            // 
            this.nombreUsuarioLabel.AutoSize = true;
            this.nombreUsuarioLabel.Font = new System.Drawing.Font("Arial", 16F);
            this.nombreUsuarioLabel.ForeColor = System.Drawing.Color.Black;
            this.nombreUsuarioLabel.Location = new System.Drawing.Point(0, 0);
            this.nombreUsuarioLabel.Name = "nombreUsuarioLabel";
            this.nombreUsuarioLabel.Size = new System.Drawing.Size(0, 32);
            this.nombreUsuarioLabel.TabIndex = 0;
            // 
            // volverButton
            // 
            this.volverButton.Font = new System.Drawing.Font("Arial", 12F);
            this.volverButton.Location = new System.Drawing.Point(20, 430);
            this.volverButton.Name = "volverButton";
            this.volverButton.Size = new System.Drawing.Size(100, 40);
            this.volverButton.TabIndex = 1;
            this.volverButton.Text = "Volver";
            this.volverButton.Click += new System.EventHandler(this.volverButton_Click);
            // 
            // Sortu
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nombreUsuarioLabel);
            this.Controls.Add(this.volverButton);
            this.Name = "Sortu";
            this.Load += new System.EventHandler(this.Sortu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
