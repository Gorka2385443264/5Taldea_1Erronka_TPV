using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox emailText;
        private System.Windows.Forms.TextBox pasahitzaText;
        private System.Windows.Forms.Button logInButton;
        private System.Windows.Forms.PictureBox logoPictureBox;

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
            this.emailText = new System.Windows.Forms.TextBox();
            this.pasahitzaText = new System.Windows.Forms.TextBox();
            this.logInButton = new System.Windows.Forms.Button();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // emailText
            // 
            this.emailText.Font = new System.Drawing.Font("Myanmar Text", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.emailText.ForeColor = System.Drawing.Color.Black;
            this.emailText.Location = new System.Drawing.Point(424, 274);
            this.emailText.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.emailText.Multiline = true;
            this.emailText.Name = "emailText";
            this.emailText.Size = new System.Drawing.Size(222, 38);
            this.emailText.TabIndex = 0;

            this.emailText.Location = new System.Drawing.Point(0, 0); // Dinámico en CenterControls
            this.emailText.Multiline = true;
            this.emailText.Size = new System.Drawing.Size(294, 46);

            this.pasahitzaText.Font = new System.Drawing.Font("Myanmar Text", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasahitzaText.ForeColor = System.Drawing.Color.Black;
            this.pasahitzaText.Location = new System.Drawing.Point(424, 324);
            this.pasahitzaText.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pasahitzaText.Multiline = true;
            this.pasahitzaText.Name = "pasahitzaText";
            this.pasahitzaText.Size = new System.Drawing.Size(222, 39);
            this.pasahitzaText.TabIndex = 1;

            this.pasahitzaText.Location = new System.Drawing.Point(0, 0); // Dinámico en CenterControls
            this.pasahitzaText.Multiline = true;
            this.pasahitzaText.Size = new System.Drawing.Size(294, 47);
            this.pasahitzaText.PasswordChar = '*';

            // 
            // logInButton
            // 
            this.logInButton.Font = new System.Drawing.Font("Myanmar Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.logInButton.ForeColor = System.Drawing.Color.White;
            this.logInButton.Location = new System.Drawing.Point(448, 382);
            this.logInButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.logInButton.Name = "logInButton";
            this.logInButton.Size = new System.Drawing.Size(175, 43);
            this.logInButton.TabIndex = 3;

            this.logInButton.Text = "HASI SAIOA";
            this.logInButton.Size = new System.Drawing.Size(233, 53);
            this.logInButton.Click += new System.EventHandler(this.logInButton_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 544);
            this.Controls.Add(this.logInButton);
            this.Controls.Add(this.pasahitzaText);
            this.Controls.Add(this.emailText);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Login";
            this.Text = "Hasi Saioa";
            // logoPictureBox
            // 
            this.logoPictureBox.Location = new System.Drawing.Point(0, 0); // Dinámico en CenterControls
            this.logoPictureBox.Size = new System.Drawing.Size(400, 400);
            this.logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            // 
            // Login
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.emailText);
            this.Controls.Add(this.pasahitzaText);
            this.Controls.Add(this.logInButton);
            this.Controls.Add(this.logoPictureBox);
            this.Name = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
