﻿using System;
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
            this.comboBox_mahaia = new System.Windows.Forms.ComboBox();
            this.label_mahaia = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
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
            this.volverButton.Location = new System.Drawing.Point(12, 680);
            this.volverButton.Name = "volverButton";
            this.volverButton.Size = new System.Drawing.Size(100, 40);
            this.volverButton.TabIndex = 1;
            this.volverButton.Text = "Volver";
            this.volverButton.Click += new System.EventHandler(this.volverButton_Click);
            // 
            // comboBox_mahaia
            // 
            this.comboBox_mahaia.FormattingEnabled = true;
            this.comboBox_mahaia.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.comboBox_mahaia.Location = new System.Drawing.Point(540, 428);
            this.comboBox_mahaia.Name = "comboBox_mahaia";
            this.comboBox_mahaia.Size = new System.Drawing.Size(121, 24);
            this.comboBox_mahaia.TabIndex = 3;
            this.comboBox_mahaia.SelectedIndexChanged += new System.EventHandler(this.comboBox_mahaia_SelectedIndexChanged);
            // 
            // label_mahaia
            // 
            this.label_mahaia.AutoSize = true;
            this.label_mahaia.Location = new System.Drawing.Point(569, 409);
            this.label_mahaia.Name = "label_mahaia";
            this.label_mahaia.Size = new System.Drawing.Size(58, 16);
            this.label_mahaia.TabIndex = 5;
            this.label_mahaia.Text = "MAHAIA";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(471, 458);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(259, 75);
            this.button1.TabIndex = 6;
            this.button1.Text = "Eskaera sortu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Sortu
            // 
            this.ClientSize = new System.Drawing.Size(1189, 741);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_mahaia);
            this.Controls.Add(this.comboBox_mahaia);
            this.Controls.Add(this.nombreUsuarioLabel);
            this.Controls.Add(this.volverButton);
            this.Name = "Sortu";
            this.Load += new System.EventHandler(this.Sortu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private ComboBox comboBox_mahaia;
        private Label label_mahaia;
        private Button button1;
    }
}
