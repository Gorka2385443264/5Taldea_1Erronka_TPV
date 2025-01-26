using System.Drawing;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    partial class Eskaera
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnComanda;

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
            this.SuspendLayout();
            // 
            // btnComanda
            // 
            this.btnComanda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnComanda.Location = new System.Drawing.Point(20, 380); // Posición predeterminada
            this.btnComanda.Name = "btnComanda";
            this.btnComanda.Size = new System.Drawing.Size(150, 50);
            this.btnComanda.TabIndex = 0;
            this.btnComanda.Text = "Volver";
            this.btnComanda.BackColor = ColorTranslator.FromHtml("#E89E47");
            this.btnComanda.UseVisualStyleBackColor = true;
            this.btnComanda.Click += new System.EventHandler(this.BtnVolver_Click);
            // 
            // Eskaera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnComanda);
            this.Name = "Eskaera";
            this.Text = "Eskaera";
            this.Load += new System.EventHandler(this.Eskaera_Load);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
