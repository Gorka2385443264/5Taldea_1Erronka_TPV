namespace erronka1_talde5_tpv
{
    partial class Comanda
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.questionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.Location = new System.Drawing.Point(300, 20);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(0, 32);
            this.welcomeLabel.TabIndex = 0;
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Font = new System.Drawing.Font("Arial", 14F);
            this.questionLabel.Location = new System.Drawing.Point(300, 80);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(239, 27);
            this.questionLabel.TabIndex = 1;
            this.questionLabel.Text = "¿Qué quieres hacer?";
            // 
            // Comanda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 481);
            this.Controls.Add(this.welcomeLabel);
            this.Controls.Add(this.questionLabel);
            this.Name = "Comanda";
            this.Text = "Comanda";
            this.Load += new System.EventHandler(this.Comanda_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label welcomeLabel; // Declarar el Label de bienvenida
        private System.Windows.Forms.Label questionLabel; // Declarar el Label "¿Qué quieres hacer?"
    }
}
