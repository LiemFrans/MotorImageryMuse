using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MachineLearning;
using MachineLearning.NeuralNetwork.ExtremeNeuralNetwork;

namespace EvaluationDataMuse
{
    public partial class EvaluationData : Form
    {
        private List<int> _ListKelas;
        private List<double[]>[] _ListFitur;
        private double[][][] _ArrayFitur;
        private double[][][] _ArrayFiturDiskrit;
        private int[] _ChannelsChoose;
        private int _kelas;
        private ExtremeNeuralNetwork enn;
        private bool stop;
        private int indexFungsiAktivasi;
        public EvaluationData()
        {
            InitializeComponent();
            cbFungsiAktivasi.SelectedIndex = 0;
            dgHasilAnalisis.Columns.Add("IterasiKe", "Iterasi Ke-");
            dgHasilAnalisis.Columns.Add("PercobaanKe", "Percobaan Ke-");
            dgHasilAnalisis.Columns.Add("PercentData","Percent Data Training");
            dgHasilAnalisis.Columns.Add("KombinasiChannels","Kombinasi Channels");
            dgHasilAnalisis.Columns.Add("FungsiAktivasi", "Fungsi Aktivasi");
            dgHasilAnalisis.Columns.Add("JumlahNeuron", "Jumlah Neuron");
            dgHasilAnalisis.Columns.Add("RandomAlpha", "Random Alpha");
            dgHasilAnalisis.Columns.Add("NilaiMapeTraining", "Nilai MAPE Data Training");
            dgHasilAnalisis.Columns.Add("AkurasiTraining","Nilai Akurasi Data Training");
            dgHasilAnalisis.Columns.Add("NilaiMapeTesting", "Nilai MAPE Data Testing");
            dgHasilAnalisis.Columns.Add("AkurasiTesting", "Nilai Akurasi Data Testing");
            numRandomAlpha.Increment = 0.1M;
            numRandomAlpha.DecimalPlaces = 1;
            numRandomAlpha.Minimum = -1.0M;
            numRandomAlpha.Maximum = 1.0M;
        }

        private void btnOpenDatasets_Click(object sender, EventArgs e)
        {
            var openfile = Tmp.OpenFileDialog("Muat Fitur", "D:\\Clouds\\Google Drive\\Dari Dropbox\\FILKOM - IF\\Semester 7\\Skripsi\\MuseCSharpLSL\\MuseCSharpLSL\\bin\\Debug\\FEATURE");
            var dialogResult = openfile.ShowDialog();
            _ListKelas = new List<int>();
            if (dialogResult == DialogResult.OK)
            {
                foreach(string file in openfile.FileNames)
                {
                    var filecsv = File.ReadLines(file);
                    foreach (var row in filecsv)
                    {
                        var temp = Array.ConvertAll(row.Split(';'), double.Parse);
                        var temp1 = Convert.ToInt16(temp.Last());
                        //_ListKelas.Add(temp1);
                        if (temp1 == 4)
                        {
                            _ListKelas.Add(1);
                        }else if(temp1 == 5)
                        {
                            _ListKelas.Add(2);
                        }
                    }
                }
                var kelas = _ListKelas.Distinct();
                _kelas = kelas.Count();
                _ListFitur = new List<double[]>[kelas.Count()];
                _ArrayFitur = new double[kelas.Count()][][];
                for (int i = 0; i < kelas.Count(); i++)
                {
                    _ListFitur[i] = new List<double[]>();
                }
                foreach(string file in openfile.FileNames)
                {
                    var filecsv = File.ReadLines(file);
                    foreach (var row in filecsv)
                    {
                        var temp = Array.ConvertAll(row.Split(';'), double.Parse);
                        var temp1 = Convert.ToInt16(temp.Last());
                        if (temp1 == 4) { 
                            _ListFitur[0].Add(temp.Take(temp.Count() - 1).ToArray());
                        }
                        else if(temp1==5)
                        {
                            _ListFitur[1].Add(temp.Take(temp.Count() - 1).ToArray());
                        }
                    }
                }

                var Min = new double[_ListFitur[0][0].Count()];
                var Max = new double[_ListFitur[0][0].Count()];
                for(int i = 0; i < _ListFitur[0][0].Count(); i++)
                {
                    var minMax = getMinMax(_ListFitur, i);
                    Max[i] = minMax.Item1;
                    Min[i] = minMax.Item2;
                }

                Parallel.For(0, kelas.Count(), i =>
                {
                    _ArrayFitur[i]=new double[_ListFitur[i].Count][];
                    Parallel.For(0, _ListFitur[i].Count, j =>
                    {
                        var a = new double[_ListFitur[i][0].Length];
                        Parallel.For(0, _ListFitur[i][j].Length, k =>
                        {
                            //_ArrayFitur[i][j][k] = _ListFitur[i][j][k];
                            a[k] = normalisasi(_ListFitur[i][j][k],Min[k],Max[k]);
                        });
                        _ArrayFitur[i][j] = a;
                    });
                });

                _ArrayFiturDiskrit = new double[kelas.Count()][][];
                Parallel.For(0, kelas.Count(), i =>
                {
                    _ArrayFiturDiskrit[i] = new double[_ListFitur[i].Count][];
                    Parallel.For(0, _ListFitur[i].Count, j =>
                    {
                        var a = new double[((31-8)*_JumlahChannels)+1];
                        int l = 0;
                        for (int k = 0; k < _ListFitur[i][j].Length; k++)
                        {
                            double lngthsinyal = _ListFitur[i][j].Length;
                            double diskrit = (Math.Round(lngthsinyal / (double)_JumlahChannels / 22.0));
                            if ((k/diskrit)% 1 == 0)
                            {
                                a[l] = normalisasi(_ListFitur[i][j][k], Min[k], Max[k]);
                                l++;
                            }
                        }
                        _ArrayFiturDiskrit[i][j] = a;
                    });
                });

                MessageBox.Show("Done");
                cbOtomatis.Enabled = true;
                btnProses.Enabled = true;
                tlpOpsi.Enabled = true;
                numKali.Enabled = true;
            }
        }

        private double[] ChannelsCombination(double[]data,int[]combination)
        {
            var n = data.Length / _JumlahChannels;
            var splitting = new double[_JumlahChannels][];
            for(int i = 0;  i < _JumlahChannels; i++)
            {
                splitting[i] = data.Skip(n * i).Take(n).ToArray();
            }
            var temp = new List<double>();
            for (int i = 0; i < combination.Length; i++)
            {
                for (int j = 0; j < splitting[i].Length; j++)
                {
                    temp.Add(splitting[i][j]);
                }
            }
            return temp.ToArray<double>();
        }
        private double[,] ChannelsCombination(double[][] data, int[] combination)
        {
            var temp = new double[data.Length,(data[0].Length/_JumlahChannels)*combination.Length];
            Parallel.For(0, data.Length, i =>
            {
                var temp1 = ChannelsCombination(data[i], combination);
                Parallel.For(0, temp1.Length, j =>
                {
                    temp[i, j] = temp1[j];
                });
            });
            return temp;
        }

        private Tuple<double[,],double[]> Join(double[][,] data)
        {
            int banyakData = data.Length * data[0].GetLength(0);
            var kelasTemp = new double[banyakData];
            var dataTemp = new double[banyakData, data[0].GetLength(1)];
            int l= 0;
            for(int i = 0; i < _kelas; i++)
            {
                for(int j = 0; j < data[i].GetLength(0); j++)
                {
                    kelasTemp[l] = i + 1;
                    for (int k = 0; k < data[i].GetLength(1); k++)
                    {
                        dataTemp[l, k] = data[i][j, k];
                    }
                    l++;
                }
            }
            return new Tuple<double[,],double[]>(dataTemp, kelasTemp);
        }
        private void cbOtomatis_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbOtomatis.Checked) { tlpOpsi.Enabled = true; numKali.Enabled = true; }
            else { tlpOpsi.Enabled = false; numKali.Enabled = true; }
        }

        private void updateAktivitas(string aktivitas)
        {
            rcAktivitas.Invoke((MethodInvoker)delegate { rcAktivitas.Text += DateTime.Now.ToString("HH:mm:ss") + " : \t" + aktivitas + "\n"; });
        }

        private void btnProses_Click(object sender, EventArgs e)
        {
            if(!cbOtomatis.Checked)indexFungsiAktivasi = cbFungsiAktivasi.SelectedIndex;
            if (cbOtomatis.Checked)btnStopAuto.Enabled = true;
            bgProses.RunWorkerAsync();
        }

        private int _JumlahChannels = 4;

        private void AutoProses()
        {
            var logFile = File.ReadAllLines("History.log");
            var temp = logFile[0].Split(',');
            var log = new int[7];
            for(int i = 0; i < temp.Length; i++)
            {
                log[i] = Convert.ToInt32(temp[i]);
            }

            stop = false;
            var persenDataTraining = new int[] { 50,60,70, 80, 90 };
            var kombinasiChannels = new int[][] { new int[] { 0, 1 },new int[] {1,2 },new int[] { 0, 1, 2, 3 } };
            var fungsiAktivasi = new Activation[] { Activation.SigmoidBiner };
            var jumlahNeuron = new int[] {10,20,30,40,50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
            var nilaiRandomAlpha = new double[] { 1.0 };
            int total = persenDataTraining.Length * kombinasiChannels.Length * fungsiAktivasi.Length * jumlahNeuron.Length * nilaiRandomAlpha.Length*(int)numKali.Value;
            int iterasi = log[0];
            lbProgress.Invoke((MethodInvoker)delegate
            {
                lbProgress.Text = iterasi + "/" + total;
            });
            if (log[1] != 0)
            {
                for (int i = log[1]; i < persenDataTraining.Length; i++)
                {
                    var tuple = new Tuple<double[][], double[][]>[_kelas];
                    Parallel.For(0, tuple.Length, j =>
                    {
                        tuple[j] = GetRatioData(persenDataTraining[i], _ArrayFiturDiskrit[j]);
                    });
                    var trainingData = new double[_kelas][][];
                    var testingData = new double[_kelas][][];
                    for (int j = 0; j < _kelas; j++)
                    {
                        trainingData[j] = tuple[j].Item1;
                        Console.WriteLine("Training-" + j + ":" + trainingData[j].Length);
                        testingData[j] = tuple[j].Item2;
                        Console.WriteLine("Testing-" + j + ":" + testingData[j].Length);
                    }
                    if (log[2] != 0)
                    {
                        for (int j = log[2]; j < kombinasiChannels.Length; j++)
                        {
                            var combinationTrainingData = new double[_kelas][,];
                            var combinationTestingData = new double[_kelas][,];
                            for (int k = 0; k < _kelas; k++)
                            {
                                combinationTrainingData[k] = ChannelsCombination(trainingData[k], kombinasiChannels[j]);
                                combinationTestingData[k] = ChannelsCombination(testingData[k], kombinasiChannels[j]);
                            }
                            var training = Join(combinationTrainingData);
                            var testing = Join(combinationTestingData);
                            var trainingFeature = training.Item1;
                            var trainingKelas = training.Item2;
                            var testingFeature = testing.Item1;
                            var testingKelas = testing.Item2;
                            if (log[3]!=0) {
                                for (int k = log[3]; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron, 
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }else if (log[3] == 0)
                            {
                                for (int k = 0; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                                nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            log[3] = 0;
                        }
                    }else if (log[2] == 0)
                    {
                        for (int j = 0; j < kombinasiChannels.Length; j++)
                        {
                            var combinationTrainingData = new double[_kelas][,];
                            var combinationTestingData = new double[_kelas][,];
                            for (int k = 0; k < _kelas; k++)
                            {

                                combinationTrainingData[k] = ChannelsCombination(trainingData[k], kombinasiChannels[j]);
                                combinationTestingData[k] = ChannelsCombination(testingData[k], kombinasiChannels[j]);
                            }
                            var training = Join(combinationTrainingData);
                            var testing = Join(combinationTestingData);
                            var trainingFeature = training.Item1;
                            var trainingKelas = training.Item2;
                            var testingFeature = testing.Item1;
                            var testingKelas = testing.Item2;
                            if (log[3] != 0)
                            {
                                for (int k = log[3]; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            else if (log[3] == 0)
                            {
                                for (int k = 0; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            log[3] = 0;
                        }
                    }
                    log[2] = 0;
                }
            } else  if (log[1] == 0)
            {
                for (int i = 0; i < persenDataTraining.Length; i++)
                {
                    var tuple = new Tuple<double[][], double[][]>[_kelas];
                    Parallel.For(0, tuple.Length, j =>
                    {
                        tuple[j] = GetRatioData(persenDataTraining[i], _ArrayFitur[j]);
                    });
                    var trainingData = new double[_kelas][][];
                    var testingData = new double[_kelas][][];
                    for (int j = 0; j < _kelas; j++)
                    {
                        trainingData[j] = tuple[j].Item1;
                        Console.WriteLine("Training-" + j + ":" + trainingData[j].Length);
                        testingData[j] = tuple[j].Item2;
                        Console.WriteLine("Testing-" + j + ":" + testingData[j].Length);
                    }
                    if (log[2] != 0)
                    {
                        for (int j = log[2]; j < kombinasiChannels.Length; j++)
                        {
                            var combinationTrainingData = new double[_kelas][,];
                            var combinationTestingData = new double[_kelas][,];
                            for (int k = 0; k < _kelas; k++)
                            {

                                combinationTrainingData[k] = ChannelsCombination(trainingData[k], kombinasiChannels[j]);
                                combinationTestingData[k] = ChannelsCombination(testingData[k], kombinasiChannels[j]);
                            }
                            var training = Join(combinationTrainingData);
                            var testing = Join(combinationTestingData);
                            var trainingFeature = training.Item1;
                            var trainingKelas = training.Item2;
                            var testingFeature = testing.Item1;
                            var testingKelas = testing.Item2;
                            if (log[3] != 0)
                            {
                                for (int k = log[3]; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            else if (log[3] == 0)
                            {
                                for (int k = 0; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            log[3] = 0;
                        }
                    }
                    else if (log[2] == 0)
                    {
                        for (int j = 0; j < kombinasiChannels.Length; j++)
                        {
                            var combinationTrainingData = new double[_kelas][,];
                            var combinationTestingData = new double[_kelas][,];
                            for (int k = 0; k < _kelas; k++)
                            {

                                combinationTrainingData[k] = ChannelsCombination(trainingData[k], kombinasiChannels[j]);
                                combinationTestingData[k] = ChannelsCombination(testingData[k], kombinasiChannels[j]);
                            }
                            var training = Join(combinationTrainingData);
                            var testing = Join(combinationTestingData);
                            var trainingFeature = training.Item1;
                            var trainingKelas = training.Item2;
                            var testingFeature = testing.Item1;
                            var testingKelas = testing.Item2;
                            if (log[3] != 0)
                            {
                                for (int k = log[3]; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            else if (log[3] == 0)
                            {
                                for (int k = 0; k < fungsiAktivasi.Length; k++)
                                {
                                    if (log[4] != 0)
                                    {
                                        for (int l = log[4]; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    else if (log[4] == 0)
                                    {
                                        for (int l = 0; l < jumlahNeuron.Length; l++)
                                        {
                                            if (log[5] != 0)
                                            {
                                                for (int m = log[5]; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            else if (log[5] == 0)
                                            {
                                                for (int m = 0; m < nilaiRandomAlpha.Length; m++)
                                                {
                                                    if (log[6] != 0)
                                                    {
                                                        for (int n = log[6]; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    else if (log[6] == 0)
                                                    {
                                                        for (int n = 0; n < numKali.Value; n++)
                                                        {
                                                            MainProsesAuto(iterasi, i, j, k, l, m, n, persenDataTraining, kombinasiChannels, jumlahNeuron,
                                                               nilaiRandomAlpha, fungsiAktivasi, trainingFeature, trainingKelas, testingFeature, testingKelas, total);
                                                            iterasi++;
                                                            if (stop)
                                                            {
                                                                saveHistory(iterasi, i, j, k, l, m, n);
                                                                MessageBox.Show("Stop in iteration-" + iterasi);
                                                                goto Done;
                                                            }
                                                        }
                                                    }
                                                    log[6] = 0;
                                                }
                                            }
                                            log[5] = 0;
                                        }
                                    }
                                    log[4] = 0;
                                }
                            }
                            log[3] = 0;
                        }
                    }
                    log[2] = 0;
                }
            }
            log[1] = 0;
            Done: ExtractDataToCSV();
        }

        private void ManualProses()
        {
            _JumlahChannels = (int)numJumlahChannels.Value;
            var persenDataTraining = (int)numTrainingPercent.Value;
            _ChannelsChoose = tbChannels.Text.Split(',').Select(Int32.Parse).ToArray();
            Activation fungsiAktivasi = Activation.Linear;
            switch (indexFungsiAktivasi)
            {
                case 0:
                    fungsiAktivasi = Activation.SigmoidBiner;
                    break;
                case 1:
                    fungsiAktivasi = Activation.Linear;
                    break;
                case 2:
                    fungsiAktivasi = Activation.Sin;
                    break;
                case 3:
                    fungsiAktivasi = Activation.RadialBasis;
                    break;
                case 4:
                    fungsiAktivasi = Activation.SigmoidBiner;
                    break;
            }
            var jumlahNeuron = (int)numNeuron.Value;
            var nilaiRandomAlpha = (double)numRandomAlpha.Value;
            var tuple = new Tuple<double[][], double[][]>[_kelas];
            Parallel.For(0, tuple.Length, i =>
            {
                tuple[i] = GetRatioData(persenDataTraining, _ArrayFitur[i]);
            });
            var trainingData = new double[_kelas][][];
            var testingData = new double[_kelas][][];
            for (int i = 0; i < _kelas; i++)
            {
                trainingData[i] = tuple[i].Item1;
                testingData[i] = tuple[i].Item2;
            }
            var combinationTrainingData = new double[_kelas][,];
            var combinationTestingData = new double[_kelas][,];
            for (int i = 0; i < _kelas; i++)
            {
                combinationTrainingData[i] = ChannelsCombination(trainingData[i], _ChannelsChoose);
                combinationTestingData[i] = ChannelsCombination(testingData[i], _ChannelsChoose);
            }
            var training = Join(combinationTrainingData);
            var testing = Join(combinationTestingData);
            var trainingFeature = training.Item1;
            var trainingKelas = training.Item2;
            var testingFeature = testing.Item1;
            var testingKelas = testing.Item2;
            for (int i = 0; i < numKali.Value; i++)
            {
                string name = i  + "_" + "ke-" + i+ ", percentTraining " + persenDataTraining  + ", KombinasiChannels " + string.Join(", ", _ChannelsChoose) + ", Neuron-" + jumlahNeuron + ", Alpha-" + nilaiRandomAlpha + ", " + DateTime.Now.ToString("dd_MM_yyyy");
                updateAktivitas(name); 
                Network net = new Network(jumlahNeuron, -nilaiRandomAlpha, nilaiRandomAlpha);
                enn = new ExtremeNeuralNetwork(net, fungsiAktivasi);
                var errorTrainingData = enn.teach(trainingFeature, trainingKelas);
                var accurationTrainingData = enn.Acc;
                var YHattTraining = enn.YHattTraining;

                saveFile(enn.Weight, "weight", name);
                saveFile(enn.BetaHatt, "betaHatt", name);
                var forecast = enn.testing(testingFeature, fungsiAktivasi);
                var errorTestingData = enn.MAPEEval(forecast, testingKelas);
                var accurationTestingData = enn.Accuration(forecast, testingKelas);


                saveFile(YHattTraining, trainingKelas, "comparasionTraining", name);
                saveFile(forecast, testingKelas, "comparasionTesting", name);

                var rows = new string[] { i+ "", i + "", persenDataTraining + "", string.Join(", ", _ChannelsChoose) + "", fungsiAktivasi + "", jumlahNeuron + "", nilaiRandomAlpha + "", errorTrainingData + "", accurationTrainingData + "", errorTestingData + "", accurationTestingData + "" };
                this.Invoke((MethodInvoker)delegate
                {
                    dgHasilAnalisis.Rows.Add(rows);
                });
            }
            ExtractDataToCSV();
        }

        private Tuple<double[][],double[][]> GetRatioData(int ratioDataTraining, double[][]featureData)
        {
            double temp1 = (featureData.Length * (double)((double)ratioDataTraining / 100));
            int jumlahDataTraining = (int)temp1;
            double temp2 = (featureData.Length * (double)((double)10 / 100));
            int jumlahDataTesting = (int)temp2;
            var dataTraining = new double[jumlahDataTraining][];
            var dataTesting = new double[jumlahDataTesting][];
            Parallel.For(0, featureData.Length, i =>
            {
                if (i < jumlahDataTraining)
                {
                    var temp = new double[featureData[0].Length];
                    Parallel.For(0, featureData[0].Length, j =>
                     {
                         temp[j] = featureData[i][j];
                     });
                    dataTraining[i] = temp;
                }
                //else
                //{
                //    var temp = new double[featureData[0].Length];
                //    Parallel.For(0, featureData[0].Length, j =>
                //    {
                //        temp[j] = featureData[i][j];
                //        //dataTesting[i-dataTraining.GetLength(0), j] = featureData[i, j];
                //    });
                //    dataTesting[i - dataTraining.Length] = temp;
                //}
            });
            int k = 0; 
            for(int i = featureData.Length-dataTesting.Length; i<featureData.Length;i++)
            {
                var temp = new double[featureData[0].Length];
                Parallel.For(0, featureData[0].Length, j =>
                {
                    temp[j] = featureData[i][j];
                });
                dataTesting[k] = temp;
                k++;
            }
            return new Tuple<double[][], double[][]>(dataTraining, dataTesting);
        }

        string alatDir = "Evaluasi_Muse_Diskrit_KiriKanan";
        private void bgProses_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                btnOpenDatasets.Enabled = false;
                cbOtomatis.Enabled = false;
                numKali.Enabled = false;
                btnProses.Enabled = false;
                numJumlahChannels.Enabled = false;
                tbChannels.Enabled = false;
            });
            if (cbOtomatis.Checked == true)
            {
                AutoProses();
            }
            else if(cbOtomatis.Checked == false)
            {
                ManualProses();
            }
            this.Invoke((MethodInvoker)delegate {
                btnOpenDatasets.Enabled = true;
                cbOtomatis.Enabled = true;
                numKali.Enabled = true;
                btnProses.Enabled = true;
                numJumlahChannels.Enabled = true;
                tbChannels.Enabled = true;
                btnStopAuto.Enabled = false;
            });
            if (bgProses.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }
        private bool saveFile(double[,] data, string context, string filename)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                var temp = "";
                for (int j = 0; j < data.GetLength(1) - 1; j++)
                {
                    temp += data[i, j] + ";";
                }
                temp += data[i, data.GetLength(1) - 1] + "\n";
                File.AppendAllText("D:\\Evaluasi\\" + alatDir + "\\" + context + "\\" + filename + "." + context, temp);
            }
            updateAktivitas("simpan selesai " + filename + ".csv");
            return true;
        }
        private bool saveFile(double[] data, string context, string filename)
        {
            for (int i = 0; i < data.Length; i++)
            {
                File.AppendAllText("D:\\Evaluasi\\"+alatDir+"\\" + context + "\\" + filename + "." + context, data[i] + "\n");
            }
            return true;
        }
        private bool saveFile(double[] YHatt, double[] output, string context, string filename)
        {
            for(int i = 0; i < YHatt.Length; i++)
            {
                File.AppendAllText("D:\\Evaluasi\\" + alatDir + "\\" + context + "\\" + filename + "." + context,output[i]+";"+YHatt[i]+"\n");
            }
            return true;
        }
        private void ExtractDataToCSV()
        {
            DataGridView dgv = dgHasilAnalisis;
            // Don't save if no data is returned
            if (dgv.Rows.Count == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            // Column headers
            string columnsHeader = "";
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                columnsHeader += dgv.Columns[i].Name + ";";
            }
            sb.Append(columnsHeader + Environment.NewLine);
            // Go through each cell in the datagridview
            foreach (DataGridViewRow dgvRow in dgv.Rows)
            {
                // Make sure it's not an empty row.
                if (!dgvRow.IsNewRow)
                {
                    for (int c = 0; c < dgvRow.Cells.Count; c++)
                    {
                        // Append the cells data followed by a comma to delimit.

                        sb.Append(dgvRow.Cells[c].Value + ";");
                    }
                    // Add a new line in the text file.
                    sb.Append(Environment.NewLine);
                }
            }
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(DateTime.Now.ToString("HH_mm_ss")+ "_DataGridview.csv", false))
                {
                    sw.WriteLine(sb.ToString());
                }
            MessageBox.Show("CSV file saved.");
        }
        private void saveHistory(int iterasi, int i, int j, int k, int l, int m, int n)
        {
            string log = iterasi + "," + i + "," + j + "," + k + "," +  l+ "," + m +"," + n ;
            System.IO.File.WriteAllText(@"History.log", log);
        }


        private void btnStopAuto_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private Tuple<double,double> getMinMax(List<double[]>[] array, int index)
        {
            var column = new List<double>();
            int k = 0;
            for(int i = 0; i < array.Length; i++)
            {
                for(int j = 0; j < array[i].Count; j++)
                {
                    column.Add(array[i][j][index]);
                    k++;
                }
            }
            return new Tuple<double, double>(column.Max(),column.Min());
        }
        private double normalisasi(double x, double min, double max)
        {
            return (x - min) / (max - min);
        }

        private void MainProsesAuto(int iterasi, int i, int j, int k, int l, int m, int n,
            int[] persenDataTraining, int[][] kombinasiChannels, int[] jumlahNeuron, double[] nilaiRandomAlpha, 
            Activation[] fungsiAktivasi, double[,] trainingFeature, double[] trainingKelas,
            double[,]testingFeature,double[]testingKelas, int total)
        {
            string name = iterasi + "_" + "ke-" + n + ", percentTraining " + persenDataTraining[i] + ", KombinasiChannels " + string.Join(", ", kombinasiChannels[j]) + ", Neuron-" + jumlahNeuron[k] + ", Alpha-" + nilaiRandomAlpha[m] + ", " + DateTime.Now.ToString("dd_MM_yyyy");
            updateAktivitas(name);
            Network net = new Network(jumlahNeuron[l], -nilaiRandomAlpha[m], nilaiRandomAlpha[m]);
            enn = new ExtremeNeuralNetwork(net, fungsiAktivasi[k]);
            var errorTrainingData = enn.teach(trainingFeature, trainingKelas);
            var accurationTrainingData = enn.Acc;
            var YHattTraining = enn.YHattTraining;

            saveFile(enn.Weight, "weight", name);
            saveFile(enn.BetaHatt, "betaHatt", name);
            var forecast = enn.testing(testingFeature, fungsiAktivasi[k]);
            var errorTestingData = enn.MAPEEval(forecast, testingKelas);
            var accurationTestingData = enn.Accuration(forecast, testingKelas);

            saveFile(YHattTraining, trainingKelas, "comparasionTraining", name);
            saveFile(forecast, testingKelas, "comparasionTesting", name);

            var rows = new string[] { iterasi + "", n + "", persenDataTraining[i] + "", string.Join(", ", kombinasiChannels[j]) + "", fungsiAktivasi[k] + "", jumlahNeuron[l] + "", nilaiRandomAlpha[m] + "", errorTrainingData + "", accurationTrainingData + "", errorTestingData + "", accurationTestingData + "" };
            this.Invoke((MethodInvoker)delegate
            {
                dgHasilAnalisis.Rows.Add(rows);
            });
            lbProgress.Invoke((MethodInvoker)delegate
            {
                lbProgress.Text = iterasi + "/" + total;
            });

        }
    }
}
