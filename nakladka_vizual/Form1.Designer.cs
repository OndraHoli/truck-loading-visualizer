﻿namespace nakladka_vizual
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_truckBed = new System.Windows.Forms.Panel();
            this.button_addPallet = new System.Windows.Forms.Button();
            this.comboBox_Bedsize = new System.Windows.Forms.ComboBox();
            this.numericUpDown_PalletX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_PalletY = new System.Windows.Forms.NumericUpDown();
            this.comboBox_Pallet = new System.Windows.Forms.ComboBox();
            this.checkBox_CustomPallet = new System.Windows.Forms.CheckBox();
            this.label_bedSize = new System.Windows.Forms.Label();
            this.label_PalletSize = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button_export = new System.Windows.Forms.Button();
            this.checkBox_overlap = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PalletX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PalletY)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_truckBed
            // 
            this.panel_truckBed.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel_truckBed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_truckBed.Location = new System.Drawing.Point(49, 35);
            this.panel_truckBed.Name = "panel_truckBed";
            this.tableLayoutPanel1.SetRowSpan(this.panel_truckBed, 5);
            this.panel_truckBed.Size = new System.Drawing.Size(884, 550);
            this.panel_truckBed.TabIndex = 0;
            this.panel_truckBed.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel_truckBed.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_truckBed_MouseMove);
            // 
            // button_addPallet
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.button_addPallet, 3);
            this.button_addPallet.Location = new System.Drawing.Point(939, 165);
            this.button_addPallet.Name = "button_addPallet";
            this.button_addPallet.Size = new System.Drawing.Size(308, 49);
            this.button_addPallet.TabIndex = 1;
            this.button_addPallet.Text = "Přidat paletu";
            this.button_addPallet.UseVisualStyleBackColor = true;
            this.button_addPallet.Click += new System.EventHandler(this.button_addPallet_Click);
            // 
            // comboBox_Bedsize
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBox_Bedsize, 2);
            this.comboBox_Bedsize.FormattingEnabled = true;
            this.comboBox_Bedsize.Items.AddRange(new object[] {
            "300x100",
            "400x200",
            "600x200",
            "720x240",
            "820x240"});
            this.comboBox_Bedsize.Location = new System.Drawing.Point(1071, 35);
            this.comboBox_Bedsize.Name = "comboBox_Bedsize";
            this.comboBox_Bedsize.Size = new System.Drawing.Size(176, 28);
            this.comboBox_Bedsize.TabIndex = 2;
            this.comboBox_Bedsize.SelectedIndexChanged += new System.EventHandler(this.comboBox_Bedsize_SelectedIndexChanged);
            // 
            // numericUpDown_PalletX
            // 
            this.numericUpDown_PalletX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_PalletX.Location = new System.Drawing.Point(1071, 127);
            this.numericUpDown_PalletX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_PalletX.Name = "numericUpDown_PalletX";
            this.numericUpDown_PalletX.Size = new System.Drawing.Size(91, 26);
            this.numericUpDown_PalletX.TabIndex = 3;
            // 
            // numericUpDown_PalletY
            // 
            this.numericUpDown_PalletY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_PalletY.Location = new System.Drawing.Point(1168, 127);
            this.numericUpDown_PalletY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_PalletY.Name = "numericUpDown_PalletY";
            this.numericUpDown_PalletY.Size = new System.Drawing.Size(79, 26);
            this.numericUpDown_PalletY.TabIndex = 4;
            // 
            // comboBox_Pallet
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBox_Pallet, 2);
            this.comboBox_Pallet.FormattingEnabled = true;
            this.comboBox_Pallet.Items.AddRange(new object[] {
            "120x80",
            "80x60",
            "60x40",
            "120x100"});
            this.comboBox_Pallet.Location = new System.Drawing.Point(1071, 78);
            this.comboBox_Pallet.Name = "comboBox_Pallet";
            this.comboBox_Pallet.Size = new System.Drawing.Size(176, 28);
            this.comboBox_Pallet.TabIndex = 5;
            // 
            // checkBox_CustomPallet
            // 
            this.checkBox_CustomPallet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_CustomPallet.AutoSize = true;
            this.checkBox_CustomPallet.Location = new System.Drawing.Point(939, 128);
            this.checkBox_CustomPallet.Name = "checkBox_CustomPallet";
            this.checkBox_CustomPallet.Size = new System.Drawing.Size(126, 24);
            this.checkBox_CustomPallet.TabIndex = 6;
            this.checkBox_CustomPallet.Text = "vlastní rozměr";
            this.checkBox_CustomPallet.UseVisualStyleBackColor = true;
            // 
            // label_bedSize
            // 
            this.label_bedSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_bedSize.AutoSize = true;
            this.label_bedSize.Location = new System.Drawing.Point(939, 43);
            this.label_bedSize.Name = "label_bedSize";
            this.label_bedSize.Size = new System.Drawing.Size(126, 20);
            this.label_bedSize.TabIndex = 7;
            this.label_bedSize.Text = "rozměr kamionu";
            // 
            // label_PalletSize
            // 
            this.label_PalletSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_PalletSize.AutoSize = true;
            this.label_PalletSize.Location = new System.Drawing.Point(939, 87);
            this.label_PalletSize.Name = "label_PalletSize";
            this.label_PalletSize.Size = new System.Drawing.Size(126, 20);
            this.label_PalletSize.TabIndex = 8;
            this.label_PalletSize.Text = "rozměr palet";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.004634F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94.99537F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel1.Controls.Add(this.label_bedSize, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Bedsize, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_addPallet, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_PalletY, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_CustomPallet, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_PalletX, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Pallet, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_PalletSize, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel_truckBed, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_overlap, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.button_export, 3, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 369F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1250, 699);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // button_export
            // 
            this.button_export.Location = new System.Drawing.Point(1071, 222);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(91, 52);
            this.button_export.TabIndex = 17;
            this.button_export.Text = "EXPORT";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // checkBox_overlap
            // 
            this.checkBox_overlap.AutoSize = true;
            this.checkBox_overlap.Location = new System.Drawing.Point(939, 222);
            this.checkBox_overlap.Name = "checkBox_overlap";
            this.checkBox_overlap.Size = new System.Drawing.Size(86, 24);
            this.checkBox_overlap.TabIndex = 18;
            this.checkBox_overlap.Text = "překrytí";
            this.checkBox_overlap.UseVisualStyleBackColor = true;
            this.checkBox_overlap.CheckedChanged += new System.EventHandler(this.checkBox_overlap_CheckedChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1898, 1024);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PalletX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PalletY)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_truckBed;
        private System.Windows.Forms.Button button_addPallet;
        private System.Windows.Forms.ComboBox comboBox_Bedsize;
        private System.Windows.Forms.NumericUpDown numericUpDown_PalletX;
        private System.Windows.Forms.NumericUpDown numericUpDown_PalletY;
        private System.Windows.Forms.ComboBox comboBox_Pallet;
        private System.Windows.Forms.CheckBox checkBox_CustomPallet;
        private System.Windows.Forms.Label label_bedSize;
        private System.Windows.Forms.Label label_PalletSize;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.CheckBox checkBox_overlap;
    }
}

