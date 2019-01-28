namespace FormLiveGraph
{
    partial class MuseSignalViewer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupPreparasi = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnVerifikasi = new System.Windows.Forms.Button();
            this.btnKoneksi = new System.Windows.Forms.Button();
            this.btnPindai = new System.Windows.Forms.Button();
            this.btnBerhenti = new System.Windows.Forms.Button();
            this.groupStream = new System.Windows.Forms.GroupBox();
            this.chStream = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.bgKoneksi = new System.ComponentModel.BackgroundWorker();
            this.bgPindai = new System.ComponentModel.BackgroundWorker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupPreparasi.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupStream.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chStream)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPreparasi
            // 
            this.groupPreparasi.Controls.Add(this.flowLayoutPanel1);
            this.groupPreparasi.Location = new System.Drawing.Point(13, 13);
            this.groupPreparasi.Name = "groupPreparasi";
            this.groupPreparasi.Size = new System.Drawing.Size(1085, 62);
            this.groupPreparasi.TabIndex = 0;
            this.groupPreparasi.TabStop = false;
            this.groupPreparasi.Text = "Preparasi";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnVerifikasi);
            this.flowLayoutPanel1.Controls.Add(this.btnKoneksi);
            this.flowLayoutPanel1.Controls.Add(this.btnPindai);
            this.flowLayoutPanel1.Controls.Add(this.btnBerhenti);
            this.flowLayoutPanel1.Controls.Add(this.textBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1079, 43);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnVerifikasi
            // 
            this.btnVerifikasi.Location = new System.Drawing.Point(3, 3);
            this.btnVerifikasi.Name = "btnVerifikasi";
            this.btnVerifikasi.Size = new System.Drawing.Size(159, 40);
            this.btnVerifikasi.TabIndex = 3;
            this.btnVerifikasi.Text = "Verifikasi Kalibrasi dengan Muse Direct";
            this.btnVerifikasi.UseVisualStyleBackColor = true;
            this.btnVerifikasi.Click += new System.EventHandler(this.btnVerifikasi_Click);
            // 
            // btnKoneksi
            // 
            this.btnKoneksi.Enabled = false;
            this.btnKoneksi.Location = new System.Drawing.Point(168, 3);
            this.btnKoneksi.Name = "btnKoneksi";
            this.btnKoneksi.Size = new System.Drawing.Size(159, 40);
            this.btnKoneksi.TabIndex = 2;
            this.btnKoneksi.Text = "Hubungkan ke BlueMuse";
            this.btnKoneksi.UseVisualStyleBackColor = true;
            this.btnKoneksi.Click += new System.EventHandler(this.btnKoneksi_Click);
            // 
            // btnPindai
            // 
            this.btnPindai.Enabled = false;
            this.btnPindai.Location = new System.Drawing.Point(333, 3);
            this.btnPindai.Name = "btnPindai";
            this.btnPindai.Size = new System.Drawing.Size(143, 40);
            this.btnPindai.TabIndex = 4;
            this.btnPindai.Text = "Pindai Sinyal";
            this.btnPindai.UseVisualStyleBackColor = true;
            this.btnPindai.Click += new System.EventHandler(this.btnPindai_Click);
            // 
            // btnBerhenti
            // 
            this.btnBerhenti.Enabled = false;
            this.btnBerhenti.Location = new System.Drawing.Point(482, 3);
            this.btnBerhenti.Name = "btnBerhenti";
            this.btnBerhenti.Size = new System.Drawing.Size(143, 40);
            this.btnBerhenti.TabIndex = 5;
            this.btnBerhenti.Text = "Berhenti";
            this.btnBerhenti.UseVisualStyleBackColor = true;
            this.btnBerhenti.Click += new System.EventHandler(this.btnBerhenti_Click);
            // 
            // groupStream
            // 
            this.groupStream.Controls.Add(this.chStream);
            this.groupStream.Location = new System.Drawing.Point(13, 82);
            this.groupStream.Name = "groupStream";
            this.groupStream.Size = new System.Drawing.Size(1085, 548);
            this.groupStream.TabIndex = 1;
            this.groupStream.TabStop = false;
            this.groupStream.Text = "Live Stream";
            // 
            // chStream
            // 
            chartArea1.Name = "ChartArea1";
            this.chStream.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chStream.Legends.Add(legend1);
            this.chStream.Location = new System.Drawing.Point(7, 20);
            this.chStream.Name = "chStream";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Channel TP9";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Channel AF7";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Legend = "Legend1";
            series3.Name = "Channel AF8";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series4.Legend = "Legend1";
            series4.Name = "Channel TP10";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series5.Legend = "Legend1";
            series5.Name = "AUX";
            this.chStream.Series.Add(series1);
            this.chStream.Series.Add(series2);
            this.chStream.Series.Add(series3);
            this.chStream.Series.Add(series4);
            this.chStream.Series.Add(series5);
            this.chStream.Size = new System.Drawing.Size(1072, 522);
            this.chStream.TabIndex = 0;
            this.chStream.Text = "Live Stream";
            // 
            // bgKoneksi
            // 
            this.bgKoneksi.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgKoneksi_DoWork);
            // 
            // bgPindai
            // 
            this.bgPindai.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgPindai_DoWork);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(631, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 6;
            // 
            // MuseSignalViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 642);
            this.Controls.Add(this.groupStream);
            this.Controls.Add(this.groupPreparasi);
            this.Name = "MuseSignalViewer";
            this.Text = "Muse Signal Viewer";
            this.groupPreparasi.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupStream.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chStream)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupPreparasi;
        private System.Windows.Forms.GroupBox groupStream;
        private System.Windows.Forms.DataVisualization.Charting.Chart chStream;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnVerifikasi;
        private System.Windows.Forms.Button btnKoneksi;
        private System.ComponentModel.BackgroundWorker bgKoneksi;
        private System.Windows.Forms.Button btnPindai;
        private System.ComponentModel.BackgroundWorker bgPindai;
        private System.Windows.Forms.Button btnBerhenti;
        private System.Windows.Forms.TextBox textBox1;
    }
}

