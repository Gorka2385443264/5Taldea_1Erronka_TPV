namespace erronka1_talde5_tpv
{
    partial class Login
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
            this.emailText = new System.Windows.Forms.TextBox();
            this.pasahitzaText = new System.Windows.Forms.TextBox();
            this.logInButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // emailText
            // 
            this.emailText.Font = new System.Drawing.Font("Myanmar Text", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailText.ForeColor = System.Drawing.Color.Black;
            this.emailText.Location = new System.Drawing.Point(565, 337);
            this.emailText.Multiline = true;
            this.emailText.Name = "emailText";
            this.emailText.Size = new System.Drawing.Size(294, 46);
            this.emailText.TabIndex = 0;
            // 
            // pasahitzaText
            // 
            this.pasahitzaText.Font = new System.Drawing.Font("Myanmar Text", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasahitzaText.ForeColor = System.Drawing.Color.Black;
            this.pasahitzaText.Location = new System.Drawing.Point(565, 399);
            this.pasahitzaText.Multiline = true;
            this.pasahitzaText.Name = "pasahitzaText";
            this.pasahitzaText.Size = new System.Drawing.Size(294, 47);
            this.pasahitzaText.TabIndex = 1;
            // 
            // logInButton
            // 
            this.logInButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logInButton.Font = new System.Drawing.Font("Myanmar Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logInButton.ForeColor = System.Drawing.Color.White;
            this.logInButton.Location = new System.Drawing.Point(598, 470);
            this.logInButton.Name = "logInButton";
            this.logInButton.Size = new System.Drawing.Size(233, 53);
            this.logInButton.TabIndex = 3;
            this.logInButton.Text = "HASI SAIOA";
            this.logInButton.UseVisualStyleBackColor = true;
            this.logInButton.Click += new System.EventHandler(this.logInButton_Click);
            // 
            // hasiSaioa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1417, 670);
            this.Controls.Add(this.logInButton);
            this.Controls.Add(this.pasahitzaText);
            this.Controls.Add(this.emailText);
            this.Name = "hasiSaioa";
            this.Text = "Hasi Saioa";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox emailText;
        private System.Windows.Forms.TextBox pasahitzaText;
        private System.Windows.Forms.Button logInButton;
    }
}
