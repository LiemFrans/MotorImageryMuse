namespace BCICompetitionUI
{
    partial class FormProcess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOpenDatasets = new System.Windows.Forms.Button();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.numTriggerStop = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbChannels = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProses = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numSamplingRate = new System.Windows.Forms.NumericUpDown();
            this.numTriggerStart = new System.Windows.Forms.NumericUpDown();
            this.rcAktivitas = new System.Windows.Forms.RichTextBox();
            this.bgProses = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSamplingRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerStart)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tlpMain, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.66491F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.33509F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(893, 379);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnOpenDatasets);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(887, 42);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnOpenDatasets
            // 
            this.btnOpenDatasets.Location = new System.Drawing.Point(3, 3);
            this.btnOpenDatasets.Name = "btnOpenDatasets";
            this.btnOpenDatasets.Size = new System.Drawing.Size(107, 34);
            this.btnOpenDatasets.TabIndex = 0;
            this.btnOpenDatasets.Text = "Open File Datasets BCI Competition";
            this.btnOpenDatasets.UseVisualStyleBackColor = true;
            this.btnOpenDatasets.Click += new System.EventHandler(this.btnOpenDatasets_Click);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.74295F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.25705F));
            this.tlpMain.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tlpMain.Controls.Add(this.rcAktivitas, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Enabled = false;
            this.tlpMain.Location = new System.Drawing.Point(3, 51);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(887, 325);
            this.tlpMain.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.54331F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.45669F));
            this.tableLayoutPanel3.Controls.Add(this.numTriggerStop, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tbChannels, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnProses, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.numSamplingRate, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.numTriggerStart, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(381, 319);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // numTriggerStop
            // 
            this.numTriggerStop.Location = new System.Drawing.Point(207, 192);
            this.numTriggerStop.Name = "numTriggerStop";
            this.numTriggerStop.Size = new System.Drawing.Size(100, 20);
            this.numTriggerStop.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Times Trigger Start (s)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Channels";
            // 
            // tbChannels
            // 
            this.tbChannels.Location = new System.Drawing.Point(207, 66);
            this.tbChannels.Name = "tbChannels";
            this.tbChannels.Size = new System.Drawing.Size(100, 20);
            this.tbChannels.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sampling Rate";
            // 
            // btnProses
            // 
            this.btnProses.Location = new System.Drawing.Point(207, 255);
            this.btnProses.Name = "btnProses";
            this.btnProses.Size = new System.Drawing.Size(75, 23);
            this.btnProses.TabIndex = 3;
            this.btnProses.Text = "Proses";
            this.btnProses.UseVisualStyleBackColor = true;
            this.btnProses.Click += new System.EventHandler(this.btnProses_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Timees Trigger Stop (s)";
            // 
            // numSamplingRate
            // 
            this.numSamplingRate.Location = new System.Drawing.Point(207, 3);
            this.numSamplingRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSamplingRate.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numSamplingRate.Name = "numSamplingRate";
            this.numSamplingRate.Size = new System.Drawing.Size(100, 20);
            this.numSamplingRate.TabIndex = 9;
            this.numSamplingRate.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // numTriggerStart
            // 
            this.numTriggerStart.Location = new System.Drawing.Point(207, 129);
            this.numTriggerStart.Name = "numTriggerStart";
            this.numTriggerStart.Size = new System.Drawing.Size(100, 20);
            this.numTriggerStart.TabIndex = 10;
            // 
            // rcAktivitas
            // 
            this.rcAktivitas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rcAktivitas.Location = new System.Drawing.Point(390, 3);
            this.rcAktivitas.Name = "rcAktivitas";
            this.rcAktivitas.ReadOnly = true;
            this.rcAktivitas.Size = new System.Drawing.Size(494, 319);
            this.rcAktivitas.TabIndex = 1;
            this.rcAktivitas.Text = "";
            // 
            // bgProses
            // 
            this.bgProses.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgProses_DoWork);
            // 
            // FormProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 379);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormProcess";
            this.Text = "Process BCI Datasets";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSamplingRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerStart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnOpenDatasets;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.NumericUpDown numTriggerStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbChannels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSamplingRate;
        private System.Windows.Forms.NumericUpDown numTriggerStart;
        private System.Windows.Forms.RichTextBox rcAktivitas;
        private System.ComponentModel.BackgroundWorker bgProses;
    }
}

