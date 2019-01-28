namespace MuseSSVEPStream
{
    partial class MuseSSVEP
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
            this.mainFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnStream = new System.Windows.Forms.Button();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.bgKoneksi = new System.ComponentModel.BackgroundWorker();
            this.bgRekam = new System.ComponentModel.BackgroundWorker();
            this.rcOutputs = new System.Windows.Forms.RichTextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.mainFlow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.68406F));
            this.tableLayoutPanel1.Controls.Add(this.mainFlow, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.65988F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(380, 385);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mainFlow
            // 
            this.mainFlow.Controls.Add(this.btnConnect);
            this.mainFlow.Controls.Add(this.label1);
            this.mainFlow.Controls.Add(this.numDelay);
            this.mainFlow.Controls.Add(this.btnStream);
            this.mainFlow.Controls.Add(this.btnStop);
            this.mainFlow.Controls.Add(this.rcOutputs);
            this.mainFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainFlow.Location = new System.Drawing.Point(3, 3);
            this.mainFlow.Name = "mainFlow";
            this.mainFlow.Size = new System.Drawing.Size(374, 379);
            this.mainFlow.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(3, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Hubungkan";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnStream
            // 
            this.btnStream.Enabled = false;
            this.btnStream.Location = new System.Drawing.Point(213, 3);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(75, 23);
            this.btnStream.TabIndex = 1;
            this.btnStream.Text = "Mulai";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // numDelay
            // 
            this.numDelay.Enabled = false;
            this.numDelay.Location = new System.Drawing.Point(170, 3);
            this.numDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(37, 20);
            this.numDelay.TabIndex = 4;
            this.numDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(84, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Delay (s)";
            // 
            // bgKoneksi
            // 
            this.bgKoneksi.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgKoneksi_DoWork);
            // 
            // bgRekam
            // 
            this.bgRekam.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgRekam_DoWork);
            // 
            // rcOutputs
            // 
            this.rcOutputs.Location = new System.Drawing.Point(3, 32);
            this.rcOutputs.Name = "rcOutputs";
            this.rcOutputs.ReadOnly = true;
            this.rcOutputs.Size = new System.Drawing.Size(371, 347);
            this.rcOutputs.TabIndex = 6;
            this.rcOutputs.Text = "";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(294, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Berhenti";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // MuseSSVEP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 385);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MuseSSVEP";
            this.Text = "Muse SSVEP";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.mainFlow.ResumeLayout(false);
            this.mainFlow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel mainFlow;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rcOutputs;
        private System.ComponentModel.BackgroundWorker bgKoneksi;
        private System.ComponentModel.BackgroundWorker bgRekam;
        private System.Windows.Forms.Button btnStop;
    }
}

