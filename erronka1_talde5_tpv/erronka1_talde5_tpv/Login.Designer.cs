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
            this.emailText.Location = new System.Drawing.Point(0, 0);
            this.emailText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.emailText.Multiline = true;
            this.emailText.Name = "emailText";
            this.emailText.Size = new System.Drawing.Size(391, 56);
            this.emailText.TabIndex = 0;
            // 
            // pasahitzaText
            // 
            this.pasahitzaText.Font = new System.Drawing.Font("Myanmar Text", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasahitzaText.ForeColor = System.Drawing.Color.Black;
            this.pasahitzaText.Location = new System.Drawing.Point(0, 0);
            this.pasahitzaText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pasahitzaText.Multiline = true;
            this.pasahitzaText.Name = "pasahitzaText";
            this.pasahitzaText.PasswordChar = '*';
            this.pasahitzaText.Size = new System.Drawing.Size(391, 57);
            this.pasahitzaText.TabIndex = 1;
            // 
            // logInButton
            // 
            this.logInButton.Font = new System.Drawing.Font("Myanmar Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logInButton.ForeColor = System.Drawing.Color.White;
            this.logInButton.Location = new System.Drawing.Point(597, 470);
            this.logInButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logInButton.Name = "logInButton";
            this.logInButton.Size = new System.Drawing.Size(311, 65);
            this.logInButton.TabIndex = 3;
            this.logInButton.Text = "HASI SAIOA";
            this.logInButton.Click += new System.EventHandler(this.logInButton_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = Properties.Resources.saboreame_sin_background_1_;
            this.logoPictureBox.Location = new System.Drawing.Point(186, 13);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(533, 492);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 4;
            this.logoPictureBox.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.emailText);
            this.Controls.Add(this.pasahitzaText);
            this.Controls.Add(this.logInButton);
            this.Controls.Add(this.logoPictureBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Login";
            this.Text = "Hasi Saioa";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
