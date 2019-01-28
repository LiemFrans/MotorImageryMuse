namespace MuseCSharpLSL
{
    partial class FormMuse
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
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.rcInformasi = new System.Windows.Forms.RichTextBox();
            this.groupPreparasi = new System.Windows.Forms.GroupBox();
            this.btnVerifikasi = new System.Windows.Forms.Button();
            this.btnKoneksi = new System.Windows.Forms.Button();
            this.groupAktivitas = new System.Windows.Forms.GroupBox();
            this.rcAktivitas = new System.Windows.Forms.RichTextBox();
            this.groupLatih = new System.Windows.Forms.GroupBox();
            this.flTombol = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRekam = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnMuatFiturTraining = new System.Windows.Forms.Button();
            this.btnMuatFiturTesting = new System.Windows.Forms.Button();
            this.cbMachineLearning = new System.Windows.Forms.ComboBox();
            this.btnMuatModelWeight = new System.Windows.Forms.Button();
            this.btnMuatModelBetaHatt = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tlArah = new System.Windows.Forms.TableLayoutPanel();
            this.rbKanan = new System.Windows.Forms.RadioButton();
            this.rbKiri = new System.Windows.Forms.RadioButton();
            this.rbBerhenti = new System.Windows.Forms.RadioButton();
            this.rbMundur = new System.Windows.Forms.RadioButton();
            this.rbMaju = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cbOtomatis = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numPerulangan = new System.Windows.Forms.NumericUpDown();
            this.cbRekamHuruf = new System.Windows.Forms.CheckBox();
            this.btnLatih = new System.Windows.Forms.Button();
            this.bgRekam = new System.ComponentModel.BackgroundWorker();
            this.bgKoneksi = new System.ComponentModel.BackgroundWorker();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbMundur = new System.Windows.Forms.Label();
            this.lbKanan = new System.Windows.Forms.Label();
            this.lbBerhenti = new System.Windows.Forms.Label();
            this.lbMaju = new System.Windows.Forms.Label();
            this.lbKiri = new System.Windows.Forms.Label();
            this.groupOpsiLatih = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpOpsiELM = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.numNeuron = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numMinRand = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numMaxRand = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.tbMape = new System.Windows.Forms.TextBox();
            this.btnUjiDatasets = new System.Windows.Forms.Button();
            this.rcHasil = new System.Windows.Forms.RichTextBox();
            this.groupPemakaian = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBerhenti = new System.Windows.Forms.Button();
            this.btnUji = new System.Windows.Forms.Button();
            this.bgUji = new System.ComponentModel.BackgroundWorker();
            this.bgLatih = new System.ComponentModel.BackgroundWorker();
            this.groupInfo.SuspendLayout();
            this.groupPreparasi.SuspendLayout();
            this.groupAktivitas.SuspendLayout();
            this.groupLatih.SuspendLayout();
            this.flTombol.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tlArah.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPerulangan)).BeginInit();
            this.groupStatus.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupOpsiLatih.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flpOpsiELM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNeuron)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinRand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRand)).BeginInit();
            this.groupPemakaian.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.rcInformasi);
            this.groupInfo.Enabled = false;
            this.groupInfo.Location = new System.Drawing.Point(12, 88);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(339, 180);
            this.groupInfo.TabIndex = 0;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "Informasi Perangkat";
            // 
            // rcInformasi
            // 
            this.rcInformasi.Enabled = false;
            this.rcInformasi.Location = new System.Drawing.Point(6, 19);
            this.rcInformasi.Name = "rcInformasi";
            this.rcInformasi.Size = new System.Drawing.Size(327, 155);
            this.rcInformasi.TabIndex = 0;
            this.rcInformasi.Text = "";
            // 
            // groupPreparasi
            // 
            this.groupPreparasi.Controls.Add(this.btnVerifikasi);
            this.groupPreparasi.Controls.Add(this.btnKoneksi);
            this.groupPreparasi.Location = new System.Drawing.Point(12, 12);
            this.groupPreparasi.Name = "groupPreparasi";
            this.groupPreparasi.Size = new System.Drawing.Size(339, 70);
            this.groupPreparasi.TabIndex = 1;
            this.groupPreparasi.TabStop = false;
            this.groupPreparasi.Text = "Preparasi";
            // 
            // btnVerifikasi
            // 
            this.btnVerifikasi.Location = new System.Drawing.Point(7, 19);
            this.btnVerifikasi.Name = "btnVerifikasi";
            this.btnVerifikasi.Size = new System.Drawing.Size(159, 40);
            this.btnVerifikasi.TabIndex = 1;
            this.btnVerifikasi.Text = "Verifikasi Kalibrasi dengan Muse Direct";
            this.btnVerifikasi.UseVisualStyleBackColor = true;
            this.btnVerifikasi.Click += new System.EventHandler(this.btnVerifikasi_Click);
            // 
            // btnKoneksi
            // 
            this.btnKoneksi.Enabled = false;
            this.btnKoneksi.Location = new System.Drawing.Point(172, 19);
            this.btnKoneksi.Name = "btnKoneksi";
            this.btnKoneksi.Size = new System.Drawing.Size(159, 40);
            this.btnKoneksi.TabIndex = 0;
            this.btnKoneksi.Text = "Hubungkan ke BlueMuse";
            this.btnKoneksi.UseVisualStyleBackColor = true;
            this.btnKoneksi.Click += new System.EventHandler(this.btnKoneksi_Click);
            // 
            // groupAktivitas
            // 
            this.groupAktivitas.Controls.Add(this.rcAktivitas);
            this.groupAktivitas.Location = new System.Drawing.Point(12, 275);
            this.groupAktivitas.Name = "groupAktivitas";
            this.groupAktivitas.Size = new System.Drawing.Size(339, 442);
            this.groupAktivitas.TabIndex = 2;
            this.groupAktivitas.TabStop = false;
            this.groupAktivitas.Text = "Log Aktivitas";
            // 
            // rcAktivitas
            // 
            this.rcAktivitas.Location = new System.Drawing.Point(7, 20);
            this.rcAktivitas.Name = "rcAktivitas";
            this.rcAktivitas.ReadOnly = true;
            this.rcAktivitas.Size = new System.Drawing.Size(326, 416);
            this.rcAktivitas.TabIndex = 0;
            this.rcAktivitas.Text = "";
            // 
            // groupLatih
            // 
            this.groupLatih.Controls.Add(this.flTombol);
            this.groupLatih.Controls.Add(this.groupBox4);
            this.groupLatih.Enabled = false;
            this.groupLatih.Location = new System.Drawing.Point(357, 12);
            this.groupLatih.Name = "groupLatih";
            this.groupLatih.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupLatih.Size = new System.Drawing.Size(609, 259);
            this.groupLatih.TabIndex = 3;
            this.groupLatih.TabStop = false;
            this.groupLatih.Text = "Kolom Latih";
            // 
            // flTombol
            // 
            this.flTombol.Controls.Add(this.btnRekam);
            this.flTombol.Controls.Add(this.flowLayoutPanel3);
            this.flTombol.Controls.Add(this.cbMachineLearning);
            this.flTombol.Controls.Add(this.btnMuatModelWeight);
            this.flTombol.Controls.Add(this.btnMuatModelBetaHatt);
            this.flTombol.Location = new System.Drawing.Point(6, 203);
            this.flTombol.Name = "flTombol";
            this.flTombol.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flTombol.Size = new System.Drawing.Size(597, 53);
            this.flTombol.TabIndex = 6;
            // 
            // btnRekam
            // 
            this.btnRekam.Location = new System.Drawing.Point(3, 3);
            this.btnRekam.Name = "btnRekam";
            this.btnRekam.Size = new System.Drawing.Size(143, 44);
            this.btnRekam.TabIndex = 0;
            this.btnRekam.Text = "Rekam Sinyal";
            this.btnRekam.UseVisualStyleBackColor = true;
            this.btnRekam.Click += new System.EventHandler(this.btnRekam_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.btnMuatFiturTraining);
            this.flowLayoutPanel3.Controls.Add(this.btnMuatFiturTesting);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(152, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(159, 50);
            this.flowLayoutPanel3.TabIndex = 5;
            // 
            // btnMuatFiturTraining
            // 
            this.btnMuatFiturTraining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMuatFiturTraining.Location = new System.Drawing.Point(3, 3);
            this.btnMuatFiturTraining.Name = "btnMuatFiturTraining";
            this.btnMuatFiturTraining.Size = new System.Drawing.Size(76, 41);
            this.btnMuatFiturTraining.TabIndex = 4;
            this.btnMuatFiturTraining.Text = "Muat Fitur Training";
            this.btnMuatFiturTraining.UseVisualStyleBackColor = true;
            this.btnMuatFiturTraining.Click += new System.EventHandler(this.btnMuatFiturTraining_Click);
            // 
            // btnMuatFiturTesting
            // 
            this.btnMuatFiturTesting.Location = new System.Drawing.Point(85, 3);
            this.btnMuatFiturTesting.Name = "btnMuatFiturTesting";
            this.btnMuatFiturTesting.Size = new System.Drawing.Size(69, 41);
            this.btnMuatFiturTesting.TabIndex = 5;
            this.btnMuatFiturTesting.Text = "Muat Fitur Testing";
            this.btnMuatFiturTesting.UseVisualStyleBackColor = true;
            this.btnMuatFiturTesting.Click += new System.EventHandler(this.btnMuatFiturTesting_Click);
            // 
            // cbMachineLearning
            // 
            this.cbMachineLearning.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMachineLearning.Enabled = false;
            this.cbMachineLearning.FormattingEnabled = true;
            this.cbMachineLearning.Items.AddRange(new object[] {
            "Extreme Learning"});
            this.cbMachineLearning.Location = new System.Drawing.Point(317, 3);
            this.cbMachineLearning.Name = "cbMachineLearning";
            this.cbMachineLearning.Size = new System.Drawing.Size(125, 21);
            this.cbMachineLearning.TabIndex = 4;
            this.cbMachineLearning.SelectedIndexChanged += new System.EventHandler(this.cbMachineLearning_SelectedIndexChanged);
            // 
            // btnMuatModelWeight
            // 
            this.btnMuatModelWeight.Enabled = false;
            this.btnMuatModelWeight.Location = new System.Drawing.Point(448, 3);
            this.btnMuatModelWeight.Name = "btnMuatModelWeight";
            this.btnMuatModelWeight.Size = new System.Drawing.Size(66, 44);
            this.btnMuatModelWeight.TabIndex = 1;
            this.btnMuatModelWeight.Text = "Muat Weight";
            this.btnMuatModelWeight.UseVisualStyleBackColor = true;
            this.btnMuatModelWeight.Click += new System.EventHandler(this.btnMuatModel_Click);
            // 
            // btnMuatModelBetaHatt
            // 
            this.btnMuatModelBetaHatt.Enabled = false;
            this.btnMuatModelBetaHatt.Location = new System.Drawing.Point(520, 3);
            this.btnMuatModelBetaHatt.Name = "btnMuatModelBetaHatt";
            this.btnMuatModelBetaHatt.Size = new System.Drawing.Size(66, 44);
            this.btnMuatModelBetaHatt.TabIndex = 6;
            this.btnMuatModelBetaHatt.Text = "Muat BetaHatt";
            this.btnMuatModelBetaHatt.UseVisualStyleBackColor = true;
            this.btnMuatModelBetaHatt.Click += new System.EventHandler(this.btnMuatModelBetaHatt_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tlArah);
            this.groupBox4.Location = new System.Drawing.Point(6, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox4.Size = new System.Drawing.Size(597, 181);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Arah Gerakan";
            // 
            // tlArah
            // 
            this.tlArah.ColumnCount = 2;
            this.tlArah.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlArah.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlArah.Controls.Add(this.rbKanan, 1, 1);
            this.tlArah.Controls.Add(this.rbKiri, 1, 0);
            this.tlArah.Controls.Add(this.rbBerhenti, 0, 2);
            this.tlArah.Controls.Add(this.rbMundur, 0, 1);
            this.tlArah.Controls.Add(this.rbMaju, 0, 0);
            this.tlArah.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tlArah.Location = new System.Drawing.Point(6, 18);
            this.tlArah.Name = "tlArah";
            this.tlArah.RowCount = 3;
            this.tlArah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlArah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlArah.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlArah.Size = new System.Drawing.Size(585, 156);
            this.tlArah.TabIndex = 0;
            // 
            // rbKanan
            // 
            this.rbKanan.AutoSize = true;
            this.rbKanan.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbKanan.Location = new System.Drawing.Point(295, 54);
            this.rbKanan.Name = "rbKanan";
            this.rbKanan.Size = new System.Drawing.Size(116, 37);
            this.rbKanan.TabIndex = 4;
            this.rbKanan.TabStop = true;
            this.rbKanan.Text = "Kanan";
            this.rbKanan.UseVisualStyleBackColor = true;
            this.rbKanan.CheckedChanged += new System.EventHandler(this.rbKanan_CheckedChanged);
            // 
            // rbKiri
            // 
            this.rbKiri.AutoSize = true;
            this.rbKiri.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbKiri.Location = new System.Drawing.Point(295, 3);
            this.rbKiri.Name = "rbKiri";
            this.rbKiri.Size = new System.Drawing.Size(76, 37);
            this.rbKiri.TabIndex = 3;
            this.rbKiri.TabStop = true;
            this.rbKiri.Text = "Kiri";
            this.rbKiri.UseVisualStyleBackColor = true;
            this.rbKiri.CheckedChanged += new System.EventHandler(this.rbKiri_CheckedChanged);
            // 
            // rbBerhenti
            // 
            this.rbBerhenti.AutoSize = true;
            this.rbBerhenti.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBerhenti.Location = new System.Drawing.Point(3, 106);
            this.rbBerhenti.Name = "rbBerhenti";
            this.rbBerhenti.Size = new System.Drawing.Size(141, 37);
            this.rbBerhenti.TabIndex = 2;
            this.rbBerhenti.TabStop = true;
            this.rbBerhenti.Text = "Berhenti";
            this.rbBerhenti.UseVisualStyleBackColor = true;
            this.rbBerhenti.CheckedChanged += new System.EventHandler(this.rbBerhenti_CheckedChanged);
            // 
            // rbMundur
            // 
            this.rbMundur.AutoSize = true;
            this.rbMundur.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMundur.Location = new System.Drawing.Point(3, 54);
            this.rbMundur.Name = "rbMundur";
            this.rbMundur.Size = new System.Drawing.Size(131, 37);
            this.rbMundur.TabIndex = 1;
            this.rbMundur.TabStop = true;
            this.rbMundur.Text = "Mundur";
            this.rbMundur.UseVisualStyleBackColor = true;
            this.rbMundur.CheckedChanged += new System.EventHandler(this.rbMundur_CheckedChanged);
            // 
            // rbMaju
            // 
            this.rbMaju.AutoSize = true;
            this.rbMaju.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMaju.Location = new System.Drawing.Point(3, 3);
            this.rbMaju.Name = "rbMaju";
            this.rbMaju.Size = new System.Drawing.Size(96, 37);
            this.rbMaju.TabIndex = 0;
            this.rbMaju.TabStop = true;
            this.rbMaju.Text = "Maju";
            this.rbMaju.UseVisualStyleBackColor = true;
            this.rbMaju.CheckedChanged += new System.EventHandler(this.rbMaju_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.76655F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.23345F));
            this.tableLayoutPanel3.Controls.Add(this.cbOtomatis, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.numPerulangan, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cbRekamHuruf, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(295, 106);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.06383F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.93617F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(287, 47);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // cbOtomatis
            // 
            this.cbOtomatis.AutoSize = true;
            this.cbOtomatis.Location = new System.Drawing.Point(3, 3);
            this.cbOtomatis.Name = "cbOtomatis";
            this.cbOtomatis.Size = new System.Drawing.Size(104, 17);
            this.cbOtomatis.TabIndex = 7;
            this.cbOtomatis.Text = "Otomatis Rekam";
            this.cbOtomatis.UseVisualStyleBackColor = true;
            this.cbOtomatis.CheckedChanged += new System.EventHandler(this.cbOtomatis_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Perulangan";
            // 
            // numPerulangan
            // 
            this.numPerulangan.Enabled = false;
            this.numPerulangan.Location = new System.Drawing.Point(119, 27);
            this.numPerulangan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPerulangan.Name = "numPerulangan";
            this.numPerulangan.Size = new System.Drawing.Size(92, 20);
            this.numPerulangan.TabIndex = 9;
            this.numPerulangan.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbRekamHuruf
            // 
            this.cbRekamHuruf.AutoSize = true;
            this.cbRekamHuruf.Checked = true;
            this.cbRekamHuruf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRekamHuruf.Location = new System.Drawing.Point(119, 3);
            this.cbRekamHuruf.Name = "cbRekamHuruf";
            this.cbRekamHuruf.Size = new System.Drawing.Size(94, 17);
            this.cbRekamHuruf.TabIndex = 10;
            this.cbRekamHuruf.Text = "Stimulus Huruf";
            this.cbRekamHuruf.UseVisualStyleBackColor = true;
            // 
            // btnLatih
            // 
            this.btnLatih.Enabled = false;
            this.btnLatih.Location = new System.Drawing.Point(3, 121);
            this.btnLatih.Name = "btnLatih";
            this.btnLatih.Size = new System.Drawing.Size(143, 44);
            this.btnLatih.TabIndex = 2;
            this.btnLatih.Text = "Latih";
            this.btnLatih.UseVisualStyleBackColor = true;
            this.btnLatih.Click += new System.EventHandler(this.btnLatih_Click);
            // 
            // bgRekam
            // 
            this.bgRekam.WorkerReportsProgress = true;
            this.bgRekam.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgRekam_DoWork);
            this.bgRekam.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgRekam_ProgressChanged);
            // 
            // bgKoneksi
            // 
            this.bgKoneksi.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgKoneksi_DoWork);
            // 
            // groupStatus
            // 
            this.groupStatus.Controls.Add(this.tableLayoutPanel1);
            this.groupStatus.Location = new System.Drawing.Point(357, 277);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupStatus.Size = new System.Drawing.Size(609, 440);
            this.groupStatus.TabIndex = 4;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "Status Gerakan";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.lbMundur, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbKanan, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbBerhenti, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbMaju, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbKiri, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(593, 418);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbMundur
            // 
            this.lbMundur.AutoSize = true;
            this.lbMundur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMundur.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMundur.Location = new System.Drawing.Point(200, 278);
            this.lbMundur.Name = "lbMundur";
            this.lbMundur.Size = new System.Drawing.Size(191, 140);
            this.lbMundur.TabIndex = 12;
            this.lbMundur.Text = "Mundur";
            this.lbMundur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbKanan
            // 
            this.lbKanan.AutoSize = true;
            this.lbKanan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbKanan.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbKanan.Location = new System.Drawing.Point(397, 139);
            this.lbKanan.Name = "lbKanan";
            this.lbKanan.Size = new System.Drawing.Size(193, 139);
            this.lbKanan.TabIndex = 11;
            this.lbKanan.Text = "Kanan";
            this.lbKanan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBerhenti
            // 
            this.lbBerhenti.AutoSize = true;
            this.lbBerhenti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBerhenti.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBerhenti.Location = new System.Drawing.Point(200, 139);
            this.lbBerhenti.Name = "lbBerhenti";
            this.lbBerhenti.Size = new System.Drawing.Size(191, 139);
            this.lbBerhenti.TabIndex = 10;
            this.lbBerhenti.Text = "Berhenti";
            this.lbBerhenti.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbMaju
            // 
            this.lbMaju.AutoSize = true;
            this.lbMaju.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMaju.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMaju.Location = new System.Drawing.Point(200, 0);
            this.lbMaju.Name = "lbMaju";
            this.lbMaju.Size = new System.Drawing.Size(191, 139);
            this.lbMaju.TabIndex = 6;
            this.lbMaju.Text = "Maju";
            this.lbMaju.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbKiri
            // 
            this.lbKiri.AutoSize = true;
            this.lbKiri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbKiri.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbKiri.Location = new System.Drawing.Point(3, 139);
            this.lbKiri.Name = "lbKiri";
            this.lbKiri.Size = new System.Drawing.Size(191, 139);
            this.lbKiri.TabIndex = 2;
            this.lbKiri.Text = "Kiri";
            this.lbKiri.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupOpsiLatih
            // 
            this.groupOpsiLatih.Controls.Add(this.tableLayoutPanel2);
            this.groupOpsiLatih.Enabled = false;
            this.groupOpsiLatih.Location = new System.Drawing.Point(972, 12);
            this.groupOpsiLatih.Name = "groupOpsiLatih";
            this.groupOpsiLatih.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupOpsiLatih.Size = new System.Drawing.Size(366, 438);
            this.groupOpsiLatih.TabIndex = 5;
            this.groupOpsiLatih.TabStop = false;
            this.groupOpsiLatih.Text = "Opsi Latih";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.00499F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.995013F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(354, 401);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.flpOpsiELM);
            this.flowLayoutPanel1.Controls.Add(this.btnLatih);
            this.flowLayoutPanel1.Controls.Add(this.btnUjiDatasets);
            this.flowLayoutPanel1.Controls.Add(this.rcHasil);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(348, 387);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // flpOpsiELM
            // 
            this.flpOpsiELM.Controls.Add(this.label4);
            this.flpOpsiELM.Controls.Add(this.numNeuron);
            this.flpOpsiELM.Controls.Add(this.label5);
            this.flpOpsiELM.Controls.Add(this.numMinRand);
            this.flpOpsiELM.Controls.Add(this.label6);
            this.flpOpsiELM.Controls.Add(this.numMaxRand);
            this.flpOpsiELM.Controls.Add(this.label7);
            this.flpOpsiELM.Controls.Add(this.tbMape);
            this.flpOpsiELM.Enabled = false;
            this.flpOpsiELM.Location = new System.Drawing.Point(3, 3);
            this.flpOpsiELM.Name = "flpOpsiELM";
            this.flpOpsiELM.Size = new System.Drawing.Size(345, 112);
            this.flpOpsiELM.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(172, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Neuron di Hidden Layer";
            // 
            // numNeuron
            // 
            this.numNeuron.Location = new System.Drawing.Point(181, 3);
            this.numNeuron.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numNeuron.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNeuron.Name = "numNeuron";
            this.numNeuron.Size = new System.Drawing.Size(113, 20);
            this.numNeuron.TabIndex = 19;
            this.numNeuron.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNeuron.ValueChanged += new System.EventHandler(this.numNeuron_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.label5.Location = new System.Drawing.Point(3, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 29);
            this.label5.TabIndex = 14;
            this.label5.Text = "Minimum Random";
            // 
            // numMinRand
            // 
            this.numMinRand.Location = new System.Drawing.Point(217, 29);
            this.numMinRand.Name = "numMinRand";
            this.numMinRand.Size = new System.Drawing.Size(77, 20);
            this.numMinRand.TabIndex = 13;
            this.numMinRand.ValueChanged += new System.EventHandler(this.numMinRand_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.label6.Location = new System.Drawing.Point(3, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 29);
            this.label6.TabIndex = 16;
            this.label6.Text = "Maximum Random";
            // 
            // numMaxRand
            // 
            this.numMaxRand.Location = new System.Drawing.Point(222, 58);
            this.numMaxRand.Name = "numMaxRand";
            this.numMaxRand.Size = new System.Drawing.Size(72, 20);
            this.numMaxRand.TabIndex = 15;
            this.numMaxRand.ValueChanged += new System.EventHandler(this.numMaxRand_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 20);
            this.label7.TabIndex = 18;
            this.label7.Text = "MAPE";
            // 
            // tbMape
            // 
            this.tbMape.Location = new System.Drawing.Point(67, 87);
            this.tbMape.Name = "tbMape";
            this.tbMape.ReadOnly = true;
            this.tbMape.Size = new System.Drawing.Size(100, 20);
            this.tbMape.TabIndex = 17;
            // 
            // btnUjiDatasets
            // 
            this.btnUjiDatasets.Location = new System.Drawing.Point(152, 121);
            this.btnUjiDatasets.Name = "btnUjiDatasets";
            this.btnUjiDatasets.Size = new System.Drawing.Size(143, 44);
            this.btnUjiDatasets.TabIndex = 5;
            this.btnUjiDatasets.Text = "Uji Menggunakan Datasets";
            this.btnUjiDatasets.UseVisualStyleBackColor = true;
            this.btnUjiDatasets.Click += new System.EventHandler(this.btnUjiDatasets_Click);
            // 
            // rcHasil
            // 
            this.rcHasil.Location = new System.Drawing.Point(3, 171);
            this.rcHasil.Name = "rcHasil";
            this.rcHasil.ReadOnly = true;
            this.rcHasil.Size = new System.Drawing.Size(338, 216);
            this.rcHasil.TabIndex = 6;
            this.rcHasil.Text = "";
            // 
            // groupPemakaian
            // 
            this.groupPemakaian.Controls.Add(this.tableLayoutPanel4);
            this.groupPemakaian.Enabled = false;
            this.groupPemakaian.Location = new System.Drawing.Point(972, 432);
            this.groupPemakaian.Name = "groupPemakaian";
            this.groupPemakaian.Size = new System.Drawing.Size(359, 285);
            this.groupPemakaian.TabIndex = 6;
            this.groupPemakaian.TabStop = false;
            this.groupPemakaian.Text = "Opsi Pemakaian";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.41148F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.58852F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(347, 418);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnBerhenti);
            this.flowLayoutPanel2.Controls.Add(this.btnUji);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(341, 187);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // btnBerhenti
            // 
            this.btnBerhenti.Location = new System.Drawing.Point(3, 3);
            this.btnBerhenti.Name = "btnBerhenti";
            this.btnBerhenti.Size = new System.Drawing.Size(75, 23);
            this.btnBerhenti.TabIndex = 2;
            this.btnBerhenti.Text = "Berhenti";
            this.btnBerhenti.UseVisualStyleBackColor = true;
            this.btnBerhenti.Click += new System.EventHandler(this.btnBerhenti_Click);
            // 
            // btnUji
            // 
            this.btnUji.Location = new System.Drawing.Point(84, 3);
            this.btnUji.Name = "btnUji";
            this.btnUji.Size = new System.Drawing.Size(75, 23);
            this.btnUji.TabIndex = 1;
            this.btnUji.Text = "Uji";
            this.btnUji.UseVisualStyleBackColor = true;
            this.btnUji.Click += new System.EventHandler(this.btnUji_Click);
            // 
            // bgUji
            // 
            this.bgUji.WorkerReportsProgress = true;
            this.bgUji.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgUji_DoWork);
            this.bgUji.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgUji_ProgressChanged);
            // 
            // bgLatih
            // 
            this.bgLatih.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgLatih_DoWork);
            // 
            // FormMuse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.groupPemakaian);
            this.Controls.Add(this.groupOpsiLatih);
            this.Controls.Add(this.groupStatus);
            this.Controls.Add(this.groupLatih);
            this.Controls.Add(this.groupAktivitas);
            this.Controls.Add(this.groupPreparasi);
            this.Controls.Add(this.groupInfo);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMuse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MuseCSharp";
            this.groupInfo.ResumeLayout(false);
            this.groupPreparasi.ResumeLayout(false);
            this.groupAktivitas.ResumeLayout(false);
            this.groupLatih.ResumeLayout(false);
            this.flTombol.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tlArah.ResumeLayout(false);
            this.tlArah.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPerulangan)).EndInit();
            this.groupStatus.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupOpsiLatih.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flpOpsiELM.ResumeLayout(false);
            this.flpOpsiELM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNeuron)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinRand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRand)).EndInit();
            this.groupPemakaian.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupInfo;
        private System.Windows.Forms.GroupBox groupPreparasi;
        private System.Windows.Forms.Button btnVerifikasi;
        private System.Windows.Forms.Button btnKoneksi;
        private System.Windows.Forms.RichTextBox rcInformasi;
        private System.Windows.Forms.GroupBox groupAktivitas;
        private System.Windows.Forms.RichTextBox rcAktivitas;
        private System.Windows.Forms.GroupBox groupLatih;
        private System.Windows.Forms.FlowLayoutPanel flTombol;
        private System.Windows.Forms.Button btnRekam;
        private System.Windows.Forms.Button btnMuatModelWeight;
        private System.Windows.Forms.Button btnLatih;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tlArah;
        private System.Windows.Forms.RadioButton rbKanan;
        private System.Windows.Forms.RadioButton rbKiri;
        private System.Windows.Forms.RadioButton rbBerhenti;
        private System.Windows.Forms.RadioButton rbMundur;
        private System.Windows.Forms.RadioButton rbMaju;
        private System.ComponentModel.BackgroundWorker bgRekam;
        private System.ComponentModel.BackgroundWorker bgKoneksi;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbMundur;
        private System.Windows.Forms.Label lbKanan;
        private System.Windows.Forms.Label lbBerhenti;
        private System.Windows.Forms.Label lbMaju;
        private System.Windows.Forms.Label lbKiri;
        private System.Windows.Forms.GroupBox groupOpsiLatih;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox cbOtomatis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numPerulangan;
        private System.Windows.Forms.CheckBox cbRekamHuruf;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupPemakaian;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.ComponentModel.BackgroundWorker bgUji;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnUji;
        private System.Windows.Forms.Button btnBerhenti;
        private System.Windows.Forms.ComboBox cbMachineLearning;
        private System.Windows.Forms.FlowLayoutPanel flpOpsiELM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numMinRand;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMaxRand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbMape;
        private System.ComponentModel.BackgroundWorker bgLatih;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button btnMuatFiturTraining;
        private System.Windows.Forms.Button btnMuatFiturTesting;
        private System.Windows.Forms.Button btnUjiDatasets;
        private System.Windows.Forms.Button btnMuatModelBetaHatt;
        private System.Windows.Forms.NumericUpDown numNeuron;
        private System.Windows.Forms.RichTextBox rcHasil;
    }
}

