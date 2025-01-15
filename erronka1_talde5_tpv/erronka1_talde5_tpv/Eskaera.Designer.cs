namespace erronka1_talde5_tpv
{
    partial class Eskaera
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewEskaera;

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
            this.dataGridViewEskaera = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEskaera)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewEskaera
            // 
            this.dataGridViewEskaera.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEskaera.Location = new System.Drawing.Point(12, 40);
            this.dataGridViewEskaera.Name = "dataGridViewEskaera";
            this.dataGridViewEskaera.RowTemplate.Height = 24;
            this.dataGridViewEskaera.Size = new System.Drawing.Size(760, 400);
            this.dataGridViewEskaera.TabIndex = 0;
            // 
            // Eskaera
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewEskaera);
            this.Name = "Eskaera";
            this.Load += new System.EventHandler(this.Eskaera_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEskaera)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
