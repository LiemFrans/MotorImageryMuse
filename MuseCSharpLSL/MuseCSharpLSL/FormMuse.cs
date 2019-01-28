using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Butterworth;
using LSL;
using MachineLearning;
using LinearTimeInvariantProperties;
using MachineLearning.SupervisedLearning.InstanceBasedLearning;
using MachineLearning.NeuralNetwork.ExtremeNeuralNetwork;

namespace MuseCSharpLSL
{
    public partial class FormMuse : Form
    {
        private int _kanal;
        private int _chunks = 1;
        private static int _samplingrate = 256;
        private int _epocSizeTesting = 6 * _samplingrate;
        private liblsl.StreamInfo[] results;
        private string _nama;
        private List<double[]> _listFitur;
        private List<double> _listKelas;
        private Stimulus stimulus;
        private StimulusHuruf stimulusHuruf;
        string arah;
        private KNearestNeighbour knn;
        private ExtremeNeuralNetwork enn;
        private double _error;
        private bool _ujikan;

        public FormMuse()
        {
           
            _nama = "";
            DialogResult dialogres;
            do
            {
                dialogres = Tmp.InputBox("Form Nama", "Masukkan nama anda: ", ref _nama);
                if (dialogres == DialogResult.Cancel) break;
            } while ((_nama == "" && dialogres == DialogResult.OK));
            InitializeComponent();
            if (dialogres.ToString() == "Cancel")
            {
                Environment.Exit(0);
            }
            else
            {
                rcAktivitas.Text += DateTime.Now.ToString("HH:mm:ss") + " : \tSelamat Datang " + _nama + "\n";
                this.Text += " - " + _nama + " - " + DateTime.Now.ToString("HH:mm:ss");
            }
            string[] folder = { "RAW_HURUF\\", "BPF_HURUF\\", "DFT_HURUF\\", "FEATURE\\", "RAW_GAMBAR\\", "BPF_HURUF\\", "DFT_HURUF\\", "weight\\" ,"betaHatt\\" }; 
            foreach (var f in folder)
            {
                if (!Directory.Exists(f))
                {
                    Directory.CreateDirectory(f);
                }
            }
        }
        liblsl.StreamInlet inlet;
        private double[,] _arrayFitur;
        private double[] _arrayKelas;
        private int _selectedMachine;
        private int _neuronHidden;
        private double _minRand;
        private double _maxRand;
        private List<double[]> _listFiturUji;
        private List<double> _listKelasUji;
        private double[,] _arrayFiturUji;
        private double[] _arrayKelasUji;

        private void btnKoneksi_Click(object sender, EventArgs e)
        {
            bgKoneksi.RunWorkerAsync();
        }

        private void bgKoneksi_DoWork(object sender, DoWorkEventArgs e)
        {
            groupPreparasi.Invoke((MethodInvoker)delegate { groupPreparasi.Enabled = false; });
            updateAktivitas("Menghubungkan ke BlueMuse . . .");
            results = liblsl.resolve_stream("type", "EEG", 0, 5);
            if (results.Length != 0)
            {
                updateAktivitas("Menghubungkan OK");
                this.Invoke((MethodInvoker)delegate
                {
                    btnKoneksi.Enabled = false;
                    groupInfo.Enabled = true;
                    groupLatih.Enabled = true;
                });

                inlet = new liblsl.StreamInlet(results[0]);
                this.Invoke((MethodInvoker)delegate
                {
                    rcInformasi.Text = "Nama Perangkat\t\t: " + inlet.info().name() + "\n" +
                                       "Nomor UID Perangkat\t: " + inlet.info().uid() + "\n" +
                                       "Versi Perangkat\t\t: " + inlet.info().version() + "\n" +
                                       "Sumber Arus EEG\t\t: " + inlet.info().source_id() + "\n" +
                                       "Jumlah Kanal\t\t: " + inlet.info().channel_count() + " Kanal\n" +
                                       "Sampling rate\t\t: " + inlet.info().nominal_srate() + "\n" +
                                       "Bentuk nilai Kanal\t\t: " + inlet.info().channel_format() + "\n";
                    _kanal = inlet.info().channel_count();
                    rbMaju.Checked = true;
                    btnMuatModelWeight.Enabled = true;
                    cbMachineLearning.SelectedIndex = 0;
                });
            }else if (_nama == "debug")
            {
                updateAktivitas("Debug");
                this.Invoke((MethodInvoker)delegate
                {
                    rcInformasi.Text = "debug";
                    rbMaju.Checked = true;
                    btnMuatModelWeight.Enabled = true;
                    groupBox4.Enabled = false;
                    btnRekam.Enabled = false;
                    btnKoneksi.Enabled = false;
                    groupInfo.Enabled = true;
                    groupLatih.Enabled = true;
                    cbMachineLearning.SelectedIndex = 0;
                    cbMachineLearning.Enabled = true;
                });
            }
            else
            {
                MessageBox.Show("Tidak ditemukan arus EEG dari aplikasi BlueMuse atau perangkat EEG tidak terhubung ke komputer! Check kembali", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                updateAktivitas("Menghubungkan ke BlueMuse gagal . . .");
            }
            groupPreparasi.Invoke((MethodInvoker)delegate { groupPreparasi.Enabled = true; });
            if (bgKoneksi.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void btnVerifikasi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sudah terkalibrasi dengan benar di aplikasi Muse Direct?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                btnKoneksi.Enabled = true;
                btnVerifikasi.Enabled = false;
                updateAktivitas("Kalibrasi OK");
            }
            else
            {
                MessageBox.Show("Silahkan Kalibrasi pada aplikasi Muse Direct", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateAktivitas("Kalibrasi NO");
            }
        }

        private void updateAktivitas(string aktivitas)
        {
            rcAktivitas.Invoke((MethodInvoker)delegate { rcAktivitas.Text += DateTime.Now.ToString("HH:mm:ss") + " : \t" + aktivitas + "\n"; });
        }

        private void btnRekam_Click(object sender, EventArgs e)
        {
             bgRekam.RunWorkerAsync();
        }

        private void rbMaju_CheckedChanged(object sender, EventArgs e)
        {
            updateAktivitas("Maju " + rbMaju.Checked + "");
        }

        private void rbMundur_CheckedChanged(object sender, EventArgs e)
        {
            updateAktivitas("Mundur " + rbMundur.Checked + "");
        }

        private void rbBerhenti_CheckedChanged(object sender, EventArgs e)
        {
            updateAktivitas("Berhenti " + rbBerhenti.Checked + "");
        }

        private void rbKiri_CheckedChanged(object sender, EventArgs e)
        {
            updateAktivitas("Kiri " + rbKiri.Checked + "");
        }

        private void rbKanan_CheckedChanged(object sender, EventArgs e)
        {
            updateAktivitas("Kanan" + rbKanan.Checked + "");
        }


        private void bgRekam_DoWork(object sender, DoWorkEventArgs e)
        {
            if (cbRekamHuruf.Checked)
            {
                if (cbOtomatis.Checked == true)
                {
                    stimulusHuruf = new StimulusHuruf();
                    for (int i = 0; i < (int)numPerulangan.Value; i++)
                    {
                        rbMaju.Invoke((MethodInvoker)delegate { rbMaju.Checked = true; });
                        rekam();
                        rbMundur.Invoke((MethodInvoker)delegate { rbMundur.Checked = true; });
                        rekam();
                        rbBerhenti.Invoke((MethodInvoker)delegate { rbBerhenti.Checked = true; });
                        rekam();
                        rbKiri.Invoke((MethodInvoker)delegate { rbKiri.Checked = true; });
                        rekam();
                        rbKanan.Invoke((MethodInvoker)delegate { rbKanan.Checked = true; });
                        rekam();
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        stimulusHuruf.Close();
                    });
                }
                else
                {
                    rekam();
                }
            }
            else
            {
                if (cbOtomatis.Checked == true)
                {
                    stimulus = new Stimulus();
                    for (int i = 0; i < (int)numPerulangan.Value; i++)
                    {
                        rbMaju.Invoke((MethodInvoker)delegate { rbMaju.Checked = true; });
                        rekam();
                        rbMundur.Invoke((MethodInvoker)delegate { rbMundur.Checked = true; });
                        rekam();
                        rbBerhenti.Invoke((MethodInvoker)delegate { rbBerhenti.Checked = true; });
                        rekam();
                        rbKiri.Invoke((MethodInvoker)delegate { rbKiri.Checked = true; });
                        rekam();
                        rbKanan.Invoke((MethodInvoker)delegate { rbKanan.Checked = true; });
                        rekam();
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        stimulus.Close();
                    });
                }
                else
                {
                    rekam();
                }
            }
            cbOtomatis.Invoke((MethodInvoker)delegate { cbOtomatis.Checked = false; }) ;
            if (bgRekam.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void rekam()
        {
            if (cbOtomatis.Checked != true)
            {
                if (cbRekamHuruf.Checked)
                {
                    stimulusHuruf = new StimulusHuruf();
                }
                else
                {
                    stimulus = new Stimulus();
                }
            }
            groupLatih.Invoke((MethodInvoker)delegate { groupLatih.Enabled = false; });
            var checkedButton = tlArah.Controls.OfType<RadioButton>()
                          .FirstOrDefault(r => r.Checked);
            arah = checkedButton.Text;
            var epoching = new List<float[]>();
            var watch = new Stopwatch();
            if (cbRekamHuruf.Checked)
            {
                bgRekam.ReportProgress(1, stimulusHuruf);
            }
            else
            {
                bgRekam.ReportProgress(1, stimulus);
            }
            watch.Start();
            updateAktivitas("REST");
            do
            {

            } while (watch.Elapsed.Seconds != 2);
            watch.Restart();
            if (cbRekamHuruf.Checked)
            {
                bgRekam.ReportProgress(2, stimulusHuruf);
            }
            else
            {
                bgRekam.ReportProgress(2, stimulus);
            }
            // read samples
            float[,] buffer = new float[_chunks, _kanal];
            double[] timestamps = new double[_chunks];
            updateAktivitas("Rekam " + arah);
            inlet = new liblsl.StreamInlet(results[0]);
            do
            {
                int num = inlet.pull_chunk(buffer, timestamps);
                for (int s = 0; s < num; s++)
                {
                    var d = new float[_kanal];
                    for (int i = 0; i < _kanal; i++)
                    {
                        d[i] = buffer[s, i];
                    }
                    epoching.Add(d);
                    if (epoching.Count == _epocSizeTesting) break;
                }
            } while (epoching.Count < _epocSizeTesting);
            updateAktivitas(arah + " pengambilan +- " + watch.ElapsedMilliseconds + " ms");
            watch.Restart();
            if (cbRekamHuruf.Checked)
            {
                bgRekam.ReportProgress(3, stimulusHuruf);
            } 
            else
            {
                bgRekam.ReportProgress(3, stimulus);
            }
            updateAktivitas("REST");
            do
            {
            } while (watch.Elapsed.Seconds != 2);
            watch.Stop();
            if (cbOtomatis.Checked != true)
            {
                if (cbRekamHuruf.Checked)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        stimulusHuruf.Close();
                    });
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        stimulus.Close();
                    });
                }

            }
            var pecah = pecahSinyal(epoching);
            var preprocess = preprocessSignal(pecah);
            var dft = featureExtraction(preprocess);
            var feature = dftToOneRowFeature(dft);
            string dateTime = DateTime.Now.ToString("HH_mm_ss");
            if (cbRekamHuruf.Checked)
            {
                saveFile(pecah, "RAW_HURUF", arah, dateTime);
                saveFile(preprocess, "BPF_HURUF", arah, dateTime);
                saveFile(dft, "DFT_HURUF", arah, dateTime);
                saveFeature(feature, arah,"HURUF");
            }
            else
            {
                saveFile(pecah, "RAW_GAMBAR", arah, dateTime);
                saveFile(preprocess, "BPF_GAMBAR", arah, dateTime);
                saveFile(dft, "DFT_GAMBAR", arah, dateTime);
                saveFeature(feature, arah,"GAMBAR");
            }
                groupLatih.Invoke((MethodInvoker)delegate { groupLatih.Enabled = true; });
        }

        private List<double[]> pecahSinyal(List<float[]> sinyal)
        {
            var jumlahKanal = sinyal.First().Length - 1;
            var jumlahSample = sinyal.Count();
            var pecahSinyal = new List<double[]>();
            for (int i = 0; i < jumlahKanal; i++)
            {
                pecahSinyal.Add(new double[jumlahSample]);
            }
            Parallel.For(0, jumlahKanal, i =>
            {
                for (int j = 0; j < jumlahSample; j++)
                {
                    pecahSinyal[i][j] = sinyal[j][i];
                };
            });
            return pecahSinyal;
        }
        private List<double[]> preprocessSignal(List<double[]> sinyal)
        {
            updateAktivitas("Bandpass Filter");
            var watch = new Stopwatch();
            watch.Start();
            var jumlahKanal = sinyal.Count();
            var jumlahSample = sinyal.First().Length;
            var sinyalFilter = new List<double[]>();
            
            for (int i = 0; i < jumlahKanal; i++)
            {
                sinyalFilter.Add(new double[jumlahSample]);
            }
            Parallel.For(0, jumlahKanal, i => {
                var bandpass = new DesignButterworth(Filter.BandPass, 5, 8, 30, _samplingrate);
                bandpass.Input = sinyal[i];
                bandpass.iirInitialization();
                bandpass.compute();
                sinyalFilter[i] = bandpass.Output;
            });
            watch.Stop();
            updateAktivitas("Selesai Bandpass Filter " + watch.ElapsedMilliseconds + " ms");
            return sinyalFilter;
        }
        private List<double[]> featureExtraction(List<double[]> sinyal)
        {
            updateAktivitas("DFT");
            var watch = new Stopwatch();
            watch.Start();
            var jumlahKanal = sinyal.Count();
            var jumlahSample = sinyal.First().Length / 2;
            var dft = new List<double[]>();
            for (int i = 0; i < jumlahKanal; i++)
            {
                dft.Add(new double[jumlahSample]);
            }
            Parallel.For(0, jumlahKanal, i => {
                dft[i] = new DiscreteFourierTransform().discreteFourierTransform(sinyal[i]).Take(jumlahSample).ToArray();
            });
            watch.Stop();
            updateAktivitas("Selesai DFT " + watch.ElapsedMilliseconds + " ms");
            return dft;
        }
        private double[] dftToOneRowFeature(List<double[]> dft)
        {
            var temp = new double[dft.Count][];
            Parallel.For(0,dft.Count,j=>{
                temp[j] = dft[j].Take(_epocSizeTesting*30/_samplingrate).Skip(_epocSizeTesting*8/_samplingrate).ToArray();
            });
            int lengthFeature = 0;
            
            foreach (var d in temp)
            {
                lengthFeature += d.Length;
            }
            var feature = new double[lengthFeature];
            int i = 0;
            foreach (var d in temp)
            {
                foreach (var s in d)
                {
                    feature[i] = s;
                    i++;
                }
            }
            return feature;
        }

        //private double[] discreteFourierTransform(double[] data)
        //{
        //    int n = data.Length;
        //    int m = n;// I use m = n / 2d;
        //    var real = new double[n];
        //    var imag = new double[n];
        //    var result = new double[m];
        //    double pi_div = 2.0 * Math.PI / n;
        //    Parallel.For(0, m, w =>
        //    {
        //        double a = w * pi_div;
        //        Parallel.For(0, n, t =>
        //        {
        //            real[w] += data[t] * Math.Cos(a * t);
        //            imag[w] += data[t] * Math.Sin(a * t);
        //        });
        //        result[w] = Math.Sqrt(real[w]);
        //    });
        //    Parallel.For(0, m, w =>
        //    {
        //        double a = w * pi_div;
        //        Parallel.For(0, n, t =>
        //        {
        //            real[w] += data[t] * Math.Cos(a * t);
        //            imag[w] += data[t] * Math.Sin(a * t);
        //        });
        //        result[w] = Math.Sqrt(real[w] * real[w] + imag[w] * imag[w]) / n;
        //    });
        //    return result;
        //}

        private void bgRekam_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString() == "MuseCSharpLSL.Stimulus, Text: Stimulus")
            {
                Stimulus stimulus = (Stimulus)e.UserState;
                if (e.ProgressPercentage == 1)
                    stimulus.perbaharuiStimulus("+");
                if (e.ProgressPercentage == 2)
                    stimulus.perbaharuiStimulus(arah);
                if (e.ProgressPercentage == 3)
                    stimulus.perbaharuiStimulus("");
                stimulus.Show();
            }
            else
            {
                StimulusHuruf stimulusHuruf = (StimulusHuruf)e.UserState;
                if (e.ProgressPercentage == 1)
                    stimulusHuruf.perbaharuiStimulus("+");
                if (e.ProgressPercentage == 2)
                    stimulusHuruf.perbaharuiStimulus(arah);
                if (e.ProgressPercentage == 3)
                    stimulusHuruf.perbaharuiStimulus("");
                stimulusHuruf.Show();
            }

        }

        private bool saveFile(List<double[]> data, string context, string arah, string dateTime)
        {
            string filename = context + "\\" + _nama + "_" + arah + "_" + context + "_" + dateTime;

            foreach (var row in data)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (i == row.Length - 1) File.AppendAllText(filename + ".csv", row[i] + Environment.NewLine);
                    else File.AppendAllText(filename + ".csv", row[i] + ";");
                }
            }
            updateAktivitas("simpan selesai " + filename + ".csv");
            return true;
        }
        private bool saveFile(double[,]data, string context, string filename)
        {
            for(int i = 0; i < data.GetLength(0); i++)
            {
                var temp = "";
                for(int j = 0; j < data.GetLength(1)-1; j++)
                {
                    temp += data[i, j]+";";
                }
                temp += data[i, data.GetLength(1)-1]+"\n";
                File.AppendAllText(context+"\\"+filename + "."+context, temp);
            }
            updateAktivitas("simpan selesai " + filename + ".csv");
            return true;
        }
        private bool saveFile(double[] data, string context, string filename)
        {
            for (int i = 0; i < data.Length; i++)
            {
                File.AppendAllText(context + "\\" + filename + "."+context, data[i]+"\n");
            }
            return true;
        }
        private bool saveFeature(double[]data,string arah, string stimulus) 
        {
            string filename = "Feature\\" + _nama+"_"+stimulus;

            foreach(var s in data)
            {
                File.AppendAllText(filename + ".csv", s + ";");
            }
            switch (arah)
            {
                case "Maju":
                    File.AppendAllText(filename + ".csv", 0 + Environment.NewLine);
                    break;
                case "Mundur":
                    File.AppendAllText(filename + ".csv", 1 + Environment.NewLine);
                    break;
                case "Berhenti":
                    File.AppendAllText(filename + ".csv", 2 + Environment.NewLine);
                    break;
                case "Kiri":
                    File.AppendAllText(filename + ".csv", 3 + Environment.NewLine);
                    break;
                case "Kanan":
                    File.AppendAllText(filename + ".csv", 4 + Environment.NewLine);
                    break;
            }
            updateAktivitas("simpan selesai " + filename + ".csv");
            return true;
        }

        private void btnMuatFitur_Click(object sender, EventArgs e)
        {

        }

        private void btnLatih_Click(object sender, EventArgs e)
        {
            bgLatih.RunWorkerAsync();
        }

        private void ELM()
        {
            this.Invoke((MethodInvoker)delegate {
                groupLatih.Enabled = false;
                groupOpsiLatih.Enabled = false;
                groupPemakaian.Enabled = false;
            });
            Console.WriteLine(_neuronHidden);
            Console.WriteLine(_minRand);
            Console.WriteLine(_maxRand);
            Network net = new Network(_neuronHidden, _minRand, _maxRand);
            enn = new ExtremeNeuralNetwork(net, Activation.SigmoidBiner);
            _error = enn.teach(_arrayFitur, _arrayKelas);
            tbMape.Invoke((MethodInvoker)delegate { tbMape.Text = _error+""; });
            DialogResult dialogres;
            string _filename = "";
            do
            {
                dialogres = Tmp.InputBox("Form Nama", "Masukkan nama model Training: ", ref _filename);
                if (dialogres == DialogResult.Cancel) break;
            } while ((_filename == "" && dialogres == DialogResult.OK));
            if (dialogres.ToString() == "Cancel")
            {
                
            }
            else
            {
                saveFile(enn.Weight, "weight", _filename);
                saveFile(enn.BetaHatt, "betaHatt", _filename);
                updateAktivitas("Save File Model");
            }

            this.Invoke((MethodInvoker)delegate {
                groupLatih.Enabled = true;
                groupOpsiLatih.Enabled = true;
                groupPemakaian.Enabled = true;
            });
        }

        private void cbOtomatis_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOtomatis.Checked)
            {
                numPerulangan.Enabled = true;
            }
            else
            {
                numPerulangan.Enabled = false;
            }
        }

        private void bgUji_DoWork(object sender, DoWorkEventArgs e)
        {
            rekam("ujikan");
            if (bgUji.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void bgUji_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("e" + e.ProgressPercentage);
            switch (e.ProgressPercentage)
            {
                case 0:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbMaju.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 1:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbMundur.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 2:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbBerhenti.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 3:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbKiri.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 4:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbKanan.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                default:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbBerhenti.BackColor = System.Drawing.Color.Red;
                    });
                    break;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            do
            {

            } while (watch.Elapsed.Seconds != 2);
            this.Invoke((MethodInvoker)delegate
            {
                lbKanan.BackColor = System.Drawing.Color.Transparent;
                lbKiri.BackColor = System.Drawing.Color.Transparent;
                lbMaju.BackColor= System.Drawing.Color.Transparent;
                lbMundur.BackColor = System.Drawing.Color.Transparent;
                lbBerhenti.BackColor = System.Drawing.Color.Transparent;
            });
        }

        private void btnUji_Click(object sender, EventArgs e)
        {
            bgUji.RunWorkerAsync();
        }

        private void updateArah(int e)
        {
            switch (e)
            {
                case 0:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbMaju.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 1:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbMundur.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 2:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbBerhenti.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 3:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbKiri.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                case 4:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbKanan.BackColor = System.Drawing.Color.Red;
                    });
                    break;
                default:
                    this.Invoke((MethodInvoker)delegate
                    {
                        lbBerhenti.BackColor = System.Drawing.Color.Red;
                    });
                    break;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            do
            {

            } while (watch.Elapsed.Seconds != 2);
            this.Invoke((MethodInvoker)delegate
            {
                lbKanan.BackColor = System.Drawing.Color.Transparent;
                lbKiri.BackColor = System.Drawing.Color.Transparent;
                lbMaju.BackColor = System.Drawing.Color.Transparent;
                lbMundur.BackColor = System.Drawing.Color.Transparent;
                lbBerhenti.BackColor = System.Drawing.Color.Transparent;
            });
        }
        private void btnBerhenti_Click(object sender, EventArgs e)
        {
            _ujikan = false;
        }

        private void rekam(string uji)
        {
            _ujikan = true;
            do
            {
                var epoching = new List<float[]>();
                float[,] buffer = new float[_chunks, _kanal];
                double[] timestamps = new double[_chunks];
                updateAktivitas("Rekam " + arah);
                inlet = new liblsl.StreamInlet(results[0]);
                do
                {
                    int num = inlet.pull_chunk(buffer, timestamps);
                    for (int s = 0; s < num; s++)
                    {
                        var d = new float[_kanal];
                        //Parallel.For(0, _kanal, i =>
                        //{
                        for (int i = 0; i < _kanal; i++)
                        {
                            d[i] = buffer[s, i];
                        }
                        //});
                        epoching.Add(d);
                        if (epoching.Count == _epocSizeTesting) break;
                    }
                } while (epoching.Count < _epocSizeTesting);
                var pecah = pecahSinyal(epoching);
                var preprocess = preprocessSignal(pecah);
                var dft = featureExtraction(preprocess);
                var feature = dftToOneRowFeature(dft);
                var f = new double[1,feature.Length];
                for(int j = 0; j < 1; j++)
                {
                    for(int k = 0; k < f.GetLength(1); k++)
                    {
                        f[j, k] = feature[k];
                    }
                }
                int kelas = (int)Math.Round(enn.testing(f,_modelWeight,_modelbetaHatt,Activation.SigmoidBiner)[0]);
                Console.WriteLine(kelas);
                Console.Beep();
                updateArah(kelas);
            } while (_ujikan);
            
        }
               
        private void cbMachineLearning_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbMachineLearning.SelectedIndex)
            {
                case 0:
                    flpOpsiELM.Enabled = true;
                    numMaxRand.Increment = 0.1M;
                    numMaxRand.DecimalPlaces = 1;
                    numMaxRand.Minimum = -5.0M;
                    numMaxRand.Maximum = 5.0M;
                    numMinRand.Increment = 0.1M;
                    numMinRand.DecimalPlaces = 1;
                    numMinRand.Minimum = -5.0M;
                    numMinRand.Maximum = 5.0M;
                    break;
            }
            _selectedMachine = cbMachineLearning.SelectedIndex;
        }

        private void bgLatih_DoWork(object sender, DoWorkEventArgs e)
        {
           btnLatih.Invoke((MethodInvoker)delegate { btnLatih.Enabled = false; });
            switch (_selectedMachine)
            {
                case 0:
                    ELM();
                    break;
            }
            btnLatih.Invoke((MethodInvoker)delegate { btnLatih.Enabled = true; });
            if (bgLatih.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void numMinRand_ValueChanged(object sender, EventArgs e)
        {
            _minRand = (double)numMinRand.Value;
        }

        private void numMaxRand_ValueChanged(object sender, EventArgs e)
        {
            _maxRand = (double)numMaxRand.Value;
        }

        private void btnMuatFiturTraining_Click(object sender, EventArgs e)
        {
            var openfile = Tmp.OpenFileDialog("Muat Fitur", "Feature");
            var dialogResult = openfile.ShowDialog();
            _listFitur = new List<double[]>();
            _listKelas = new List<double>();
            if (dialogResult == DialogResult.OK)
            {
                foreach (String file in openfile.FileNames)
                {
                    var filecsv = File.ReadAllLines(file);
                    foreach (var row in filecsv)
                    {
                        var temp = Array.ConvertAll(row.Split(';'), double.Parse);
                        var temp1 = temp.Last();
                        _listFitur.Add(temp.Take(temp.Count() - 1).ToArray());
                        _listKelas.Add(temp1 + 1);
                    }
                }
                _arrayFitur = new double[_listFitur.Count, _listFitur[0].Length];
                _arrayKelas = new double[_listKelas.Count];
                Parallel.For(0, _arrayFitur.GetLength(0), i =>
                {
                    Parallel.For(0, _arrayFitur.GetLength(1), j =>
                    {
                        _arrayFitur[i, j] = _util.NormalizedMinMax(_listFitur[i][j],0,0.15);
                    });
                    _arrayKelas[i] = _listKelas[i];
                });
                btnLatih.Enabled = true;
                cbMachineLearning.Enabled = true;
                cbMachineLearning.SelectedIndex = 0;
                groupOpsiLatih.Enabled = true;
            }
        }
        MachineLearning.Util.Util _util = new MachineLearning.Util.Util();
        private double[,] _modelWeight;
        private double[] _modelbetaHatt;

        private void btnMuatFiturTesting_Click(object sender, EventArgs e)
        {
            var openfile = Tmp.OpenFileDialog("Muat Fitur Datasets", "Feature");
            var dialogResult = openfile.ShowDialog();
            _listFiturUji = new List<double[]>();
            _listKelasUji = new List<double>();
            if (dialogResult == DialogResult.OK)
            {
                foreach (String file in openfile.FileNames)
                {
                    var filecsv = File.ReadAllLines(file);
                    foreach (var row in filecsv)
                    {
                        var temp = Array.ConvertAll(row.Split(';'), double.Parse);
                        var temp1 = temp.Last();
                        _listFiturUji.Add(temp.Take(temp.Count() - 1).ToArray());
                        _listKelasUji.Add(temp1 + 1);
                    }
                }
                _arrayFiturUji = new double[_listFiturUji.Count, _listFiturUji[0].Length];
                _arrayKelasUji = new double[_listKelasUji.Count];
                Parallel.For(0, _arrayFiturUji.GetLength(0), i =>
                {
                    Parallel.For(0, _arrayFiturUji.GetLength(1), j =>
                    {
                        _arrayFiturUji[i, j] = _util.NormalizedMinMax(_listFiturUji[i][j],0,0.15);
                    });
                    _arrayKelasUji[i] = _listKelasUji[i];
                });
                btnLatih.Enabled = true;

            }
        }

        private void btnUjiDatasets_Click(object sender, EventArgs e)
        {
            var yHatt = enn.testing(_arrayFiturUji,_modelWeight,_modelbetaHatt, Activation.SigmoidBiner);
            tbMape.Text =  enn.MAPEEval(yHatt, _arrayKelasUji)+"";
            string tempHasil = "";
            for(int i = 0; i < yHatt.Length; i++)
            {
                tempHasil+=yHatt[i] + " <=> " + _arrayKelasUji[i]+"\n";
            }
            rcHasil.Text = tempHasil;

        }

        private void btnMuatModel_Click(object sender, EventArgs e)
        {
            switch (cbMachineLearning.SelectedIndex)
            {

                case 0:
                    var openfile = Tmp.OpenFileDialog("Muat Model", "weight", "MODEL Files (*.weight)|*.weight");
                    var dialogResult = openfile.ShowDialog();
                    var listModelWeight = new List<double[]>();
                    updateAktivitas("Open File Weight " + openfile.FileNames);
                    if (dialogResult == DialogResult.OK)
                    {
                        foreach (String file in openfile.FileNames)
                        {
                            var filecsv = File.ReadAllLines(file);
                            foreach (var row in filecsv)
                            {
                                listModelWeight.Add(Array.ConvertAll(row.Split(';'), double.Parse));
                            }
                        }
                        _modelWeight = new double[listModelWeight.Count(),listModelWeight[0].Length];
                        Parallel.For(0, _modelWeight.GetLength(0) ,i=>{
                            Parallel.For(0, _modelWeight.GetLength(1), j =>
                              {
                                  _modelWeight[i, j] = listModelWeight[i][j];
                              });
                        });
                        btnMuatModelBetaHatt.Enabled = true;
                    }
                    break;
            }
        }

        private void btnMuatModelBetaHatt_Click(object sender, EventArgs e)
        {
            switch (cbMachineLearning.SelectedIndex)
            {
                case 0:
                    var openfile = Tmp.OpenFileDialog("Muat Model", "betahatt", "MODEL Files (*.betaHatt)|*.betaHatt");
                    var dialogResult = openfile.ShowDialog();
                    var listModelbetaHatt = new List<double[]>();
                    updateAktivitas("Open File BetaHatt " + openfile.FileNames);
                    if (dialogResult == DialogResult.OK)
                    {
                        foreach (String file in openfile.FileNames)
                        {
                            var filecsv = File.ReadAllLines(file);
                            foreach (var row in filecsv)
                            {
                                listModelbetaHatt.Add(Array.ConvertAll(row.Split(';'), double.Parse));
                            }
                        }
                        _modelbetaHatt = new double[listModelbetaHatt.Count()];
                        Parallel.For(0, listModelbetaHatt.Count(), i => {
                            Parallel.For(0, listModelbetaHatt[0].Length, j =>
                            {
                                _modelbetaHatt[i] = listModelbetaHatt[i][j];
                            });
                        });
                        groupPemakaian.Enabled = true;
                        btnUji.Enabled = true;
                        btnUjiDatasets.Enabled = true;
                        groupOpsiLatih.Enabled = true;
                        enn = new ExtremeNeuralNetwork();
                    }
                    break;
            }
        }

        private void numNeuron_ValueChanged(object sender, EventArgs e)
        {
            _neuronHidden = (int)numNeuron.Value;
        }
    }
}
