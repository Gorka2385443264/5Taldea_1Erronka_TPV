namespace erronka1_talde5_tpv
{
    partial class Caja
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Button backButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.backButton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // welcomeLabel
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.ForeColor = System.Drawing.Color.White;
            this.welcomeLabel.Location = new System.Drawing.Point(300, 50);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(200, 32);
            this.welcomeLabel.TabIndex = 0;
            this.welcomeLabel.Text = "¡Hola, Usuario!";

            // backButton
            this.backButton.Location = new System.Drawing.Point(20, 500);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(120, 40);
            this.backButton.TabIndex = 1;
            this.backButton.Text = "Volver";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);

            // Caja
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(23)))), ((int)(((byte)(37)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.welcomeLabel);
            this.Name = "Caja";
            this.Text = "Caja";
            this.Load += new System.EventHandler(this.Caja_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}