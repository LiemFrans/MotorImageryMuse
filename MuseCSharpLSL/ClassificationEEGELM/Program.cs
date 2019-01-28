using MachineLearning.NeuralNetwork.ExtremeNeuralNetwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassificationEEGELMCLI
{
    class Program
    {
        private static ExtremeNeuralNetwork enn;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Load Feature in Folder");
            var showDialog = true;
            do
            {
                var openfiles = OpenFileDialog("Muat Fitur", "");
                var files = new string[0];
                if (openfiles.ShowDialog() == DialogResult.OK)
                {
                    files = openfiles.FileNames;
                    Console.WriteLine("Persen Data Training (Contoh Input : 90,80,70,...,N) : ");
                    var PersenData = Array.ConvertAll(Console.ReadLine().Split(','),int.Parse);
                    Console.WriteLine("Jumlah Channels : (Contoh Input : 4)");
                    var JumlahChannels = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Kombinasi Channels : (Contoh Input : 0,1,...N;0,3,...,M;Y,...,Z) : ");
                    var KombinasiChannels = (Console.ReadLine()).Split(';').Select(t=>t.Split(',').Select(st=>int.Parse(st)).ToArray()).ToArray();
                    Console.WriteLine("Jumlah Neuron Uji (Contoh Input : 0,1,2,3,...,N) : ");
                    var JumlahNeuron = Array.ConvertAll(Console.ReadLine().Split(','),int.Parse);
                    Console.WriteLine("Masukkan Kelas apa saja yang ingin diklasifikasikan, lihat dokumentasi datasets (Contoh Input : (0,1,...,N)) : ");
                    var Kelas = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);
                    Console.WriteLine("Berapa kali anda ingin melakukan percobaan?");
                    var percobaan = Convert.ToInt32(Console.ReadLine());
                    var prepareData = PrepareData(files, Kelas, JumlahChannels);
                    var fiturKontinu = prepareData.Item1;
                    var fiturDiskrit = prepareData.Item2;
                    var kelas = prepareData.Item3;
                    var date = DateTime.Now.ToString("dd_MM_yyyy")+"_Kontinu";
                    var FungsiAktivasi = new Activation[] { Activation.SigmoidBiner };
                    var NilaiRandomAlpha = new double[] { 1.0 };
                    Percobaan(PersenData, kelas, fiturKontinu, KombinasiChannels, JumlahChannels, FungsiAktivasi, JumlahNeuron, NilaiRandomAlpha, percobaan, date,kelas);
                    date = DateTime.Now.ToString("dd_MM_yyyy")+"_Diskrit";
                    Percobaan(PersenData, kelas, fiturDiskrit, KombinasiChannels, JumlahChannels, FungsiAktivasi, JumlahNeuron, NilaiRandomAlpha, percobaan, date,kelas);
                    Console.WriteLine("Enter untuk melanjutkan");
                    Console.ReadLine();
                }
                else
                {
                    showDialog = ExitBox();
                }
            } while (showDialog);
        }
        static Tuple<double[][][],double[][][],int[]> PrepareData(string[] files, int[]KelasData, int JumlahChannels)
        {
            var fitur = new List<double[]>();
            var listKelas = new List<int>();
            foreach(var file in files)
            {
                var filecsv = File.ReadLines(file);
                foreach (var row in filecsv)
                {
                    var temp = Array.ConvertAll(row.Split(';'), double.Parse);
                    var temp1 = Convert.ToInt16(temp.Last());
                    //_ListKelas.Add(temp1);
                    foreach(var k in KelasData)
                    {
                        if (temp1 == k)
                        {
                            listKelas.Add((temp1));
                        }
                    }
                }
            }
            var kelas = listKelas.Distinct().ToArray();
            //_kelas = kelas.Count();
            var listFiturKontinu = new List<double[]>[kelas.Count()];
            var arrayFiturKontinu = new double[kelas.Count()][][];
            for (int i = 0; i < kelas.Count(); i++)
            {
                listFiturKontinu[i] = new List<double[]>();
            }
            foreach (string file in files)
            {
                var filecsv = File.ReadLines(file);
                foreach (var row in filecsv)
                {
                    var temp = Array.ConvertAll(row.Split(';'), double.Parse);
                    var temp1 = Convert.ToInt16(temp.Last());
                    foreach(var k in KelasData)
                    {
                        if(temp1 == k)
                        {
                            listFiturKontinu[Array.IndexOf(kelas,temp1)].Add(temp.Take(temp.Count() - 1).ToArray());
                        }
                    }
                }
            }
            var Min = new double[listFiturKontinu[0][0].Count()];
            var Max = new double[listFiturKontinu[0][0].Count()];
            for (int i = 0; i < listFiturKontinu[0][0].Count(); i++)
            {
                var minMax = getMinMax(listFiturKontinu, i);
                Max[i] = minMax.Item1;
                Min[i] = minMax.Item2;
            }
            Parallel.For(0, kelas.Count(), i =>
            {
                arrayFiturKontinu[i] = new double[listFiturKontinu[i].Count][];
                Parallel.For(0, listFiturKontinu[i].Count, j =>
                {
                    var a = new double[listFiturKontinu[i][0].Length];
                    Parallel.For(0, listFiturKontinu[i][j].Length, k =>
                    {
                        //_ArrayFitur[i][j][k] = _ListFitur[i][j][k];
                        a[k] = normalisasi(listFiturKontinu[i][j][k], Min[k], Max[k]);
                    });
                    arrayFiturKontinu[i][j] = a;
                });
            });
            var arrayFiturDiskrit = new double[kelas.Count()][][];
            Parallel.For(0, kelas.Count(), i =>
            {
                arrayFiturDiskrit[i] = new double[listFiturKontinu[i].Count][];
                Parallel.For(0, listFiturKontinu[i].Count, j =>
                {
                    var a = new double[((31 - 8) * JumlahChannels) + 1];
                    int l = 0;
                    for (int k = 0; k < listFiturKontinu[i][j].Length; k++)
                    {
                        double lngthsinyal = listFiturKontinu[i][j].Length;
                        double diskrit = (Math.Round(lngthsinyal / (double)JumlahChannels / 22.0));
                        if ((k / diskrit) % 1 == 0)
                        {
                            a[l] = normalisasi(listFiturKontinu[i][j][k], Min[k], Max[k]);
                            l++;
                        }
                    }
                    arrayFiturDiskrit[i][j] = a;
                });
            });
            Console.WriteLine("Done Preparation!");
            return new Tuple<double[][][], double[][][], int[]>(arrayFiturKontinu, arrayFiturDiskrit, kelas);
        }
        static OpenFileDialog OpenFileDialog(string title, string initialDIrectory)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = Path.Combine(Application.StartupPath, initialDIrectory);
            openfile.Filter =
                "CSV Files (*.csv)|*.csv";
            openfile.Multiselect = true;
            openfile.Title = title;
            return openfile;
        }
        static bool ExitBox()
        {
            DialogResult dialogResult = MessageBox.Show("Yakin?", "Keluar", MessageBoxButtons.YesNo);
            return dialogResult != DialogResult.Yes;
        }
        static Tuple<double, double> getMinMax(List<double[]>[] array, int index)
        {
            var column = new List<double>();
            int k = 0;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Count; j++)
                {
                    column.Add(array[i][j][index]);
                    k++;
                }
            }
            return new Tuple<double, double>(column.Max(), column.Min());
        }
        static double normalisasi(double x, double min, double max)
        {
            return (x - min) / (max - min);
        }
        static Tuple<double[][], double[][]> GetRatioData(int ratioDataTraining, double[][] featureData)
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
            for (int i = featureData.Length - dataTesting.Length; i < featureData.Length; i++)
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
        static double[] ChannelsCombination(double[] data, int[] combination, int JumlahChannels)
        {
            var n = data.Length / JumlahChannels;
            var splitting = new double[JumlahChannels][];
            for (int i = 0; i < JumlahChannels; i++)
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
        static double[,] ChannelsCombination(double[][] data, int[] combination, int JumlahChannels)
        {
            var temp = new double[data.Length, (data[0].Length / JumlahChannels) * combination.Length];
            Parallel.For(0, data.Length, i =>
            {
                var temp1 = ChannelsCombination(data[i], combination, JumlahChannels);
                Parallel.For(0, temp1.Length, j =>
                {
                    temp[i, j] = temp1[j];
                });
            });
            return temp;
        }
        static Tuple<double[,], double[]> Join(double[][,] data, int[] kelas)
        {
            int banyakData = data.Length * data[0].GetLength(0);
            var kelasTemp = new double[banyakData];
            var dataTemp = new double[banyakData, data[0].GetLength(1)];
            int l = 0;
            for (int i = 0; i < kelas.Length; i++)
            {
                for (int j = 0; j < data[i].GetLength(0); j++)
                {
                    kelasTemp[l] = i + 1;
                    for (int k = 0; k < data[i].GetLength(1); k++)
                    {
                        dataTemp[l, k] = data[i][j, k];
                    }
                    l++;
                }
            }
            return new Tuple<double[,], double[]>(dataTemp, kelasTemp);
        }
        static bool saveFile(double[,] data, string context, string filename)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                var temp = "";
                for (int j = 0; j < data.GetLength(1) - 1; j++)
                {
                    temp += data[i, j] + ";";
                }
                temp += data[i, data.GetLength(1) - 1] + "\n";
                File.AppendAllText( context + "\\" + filename + "." + context, temp);
            }
            return true;
        }
        static bool saveFile(double[] data, string context, string filename)
        {
            for (int i = 0; i < data.Length; i++)
            {
                File.AppendAllText(  context + "\\" + filename + "." + context, data[i] + "\n");
            }
            return true;
        }
        static bool saveFile(double[,] YHatt, double[,] output, string context, string filename)
        {
            for (int i = 0; i < YHatt.GetLength(0); i++)
            {
                File.AppendAllText( context + "\\" + filename + "." + context, printRowsArray2D(output,i) + ";" + printRowsArray2D(YHatt,i) + "\n");
            }
            return true;
        }
        static string printRowsArray2D(double[,]array, int rows)
        {
            string temp = "";
            for (int j = 0; j < array.GetLength(1); j++)
            {
                temp += array[rows, j];
            }
            return temp;
        }
        static void Percobaan(int[]PersenData, int[]kelas, double[][][]fitur, int[][]KombinasiChannels, int JumlahChannels,Activation[] FungsiAktivasi, int[] JumlahNeuron, double[]NilaiRandomAlpha, int percobaan, string date, int[]kelasNonDupe)
        {
            int iterasi = 0;
            int total = PersenData.Length * KombinasiChannels.Length * FungsiAktivasi.Length * JumlahNeuron.Length * NilaiRandomAlpha.Length * percobaan;
            for (int i = 0; i < PersenData.Length; i++)
            {
                var tuple = new Tuple<double[][], double[][]>[kelas.Length];
                Parallel.For(0, tuple.Length, j =>
                {
                    tuple[j] = GetRatioData(PersenData[i], fitur[j]);
                });
                var trainingDataKontinu = new double[kelas.Length][][];
                var testingDataKontinu = new double[kelas.Length][][];
                for (int j = 0; j < kelas.Length; j++)
                {
                    trainingDataKontinu[j] = tuple[j].Item1;
                    Console.WriteLine("Training-" + j + ":" + trainingDataKontinu[j].Length);
                    testingDataKontinu[j] = tuple[j].Item2;
                    Console.WriteLine("Testing-" + j + ":" + testingDataKontinu[j].Length);
                }
                for (int j = 0; j < KombinasiChannels.Length; j++)
                {
                    var combinationTrainingData = new double[kelas.Length][,];
                    var combinationTestingData = new double[kelas.Length][,];
                    for (int k = 0; k < kelas.Length; k++)
                    {
                        combinationTrainingData[k] = ChannelsCombination(trainingDataKontinu[k], KombinasiChannels[j], JumlahChannels);
                        combinationTestingData[k] = ChannelsCombination(testingDataKontinu[k], KombinasiChannels[j], JumlahChannels);
                    }
                    var training = Join(combinationTrainingData,kelas);
                    var testing = Join(combinationTestingData,kelas);
                    var trainingFeature = training.Item1;
                    var trainingKelas = training.Item2;
                    var testingFeature = testing.Item1;
                    var testingKelas = testing.Item2;
                    for(int k = 0; k< FungsiAktivasi.Length; k++)
                    {
                        for (int l = 0; l < JumlahNeuron.Length; l++)
                        {
                            for(int m = 0; m < NilaiRandomAlpha.Length; m++)
                            {
                                for(int n = 0; n < percobaan; n++)
                                {
                                    MainProsesAuto(iterasi, i, j, k, l, m, n, PersenData, KombinasiChannels, FungsiAktivasi, JumlahNeuron, NilaiRandomAlpha, trainingFeature, trainingKelas, testingFeature, testingKelas, total, date, kelasNonDupe);
                                    iterasi++;
                                }
                            }
                        }
                    }
                }
            }
        }
        static void MainProsesAuto(int iterasi, int i, int j, int k, int l, int m, int n,
            int[]PersenData,int[][]KombinasiChannels,Activation[] FungsiAktivasi, int[]JumlahNeuron,double[]NilaiRandomAlpha, double[,] trainingFeature, double[] trainingKelas,
            double[,] testingFeature, double[] testingKelas, int total, string date, int[] kelasNonDupe)
        {
            string name = iterasi + "_" + "ke-" + n + ", percentTraining " + PersenData[i] + ", KombinasiChannels " + string.Join(", ", KombinasiChannels[j]) + ", Neuron-" + JumlahNeuron[l] + ", Alpha-" + NilaiRandomAlpha[m] + ", " + DateTime.Now.ToString("hh_mm_ss");
            Console.WriteLine(name);
            Network net = new Network(JumlahNeuron[l], -NilaiRandomAlpha[m], NilaiRandomAlpha[m]);
            enn = new ExtremeNeuralNetwork(net, FungsiAktivasi[k]);
            var kelas2DTraining = enn.ConvertTo2DArrayKelas(kelasNonDupe, trainingKelas);
            var kelas2DTesting = enn.ConvertTo2DArrayKelas(kelasNonDupe, testingKelas);
            var accTrainingData = enn.teach(trainingFeature, kelas2DTraining);
            var YHattTraining = enn.YHattTraining2D;
            var forecast = enn.testing(testingFeature,enn.Weight,enn.BetaHatt2D, FungsiAktivasi[k]);
            var accTesting = enn.Accuration(forecast, kelas2DTesting);
            //var errorTrainingData = enn.teach(trainingFeature, trainingKelas);
            //var accurationTrainingData = enn.Acc;
            //var YHattTraining = enn.YHattTraining;

            saveFile(enn.Weight, "weight", name);
            saveFile(enn.BetaHatt2D, "betaHatt", name);

            saveFile(YHattTraining, kelas2DTraining, "comparasionTraining",name);

            saveFile(forecast, kelas2DTesting, "comparasionTesting", name);
            //var forecast = enn.testing(testingFeature, FungsiAktivasi[k]);
            //var errorTestingData = enn.MAPEEval(forecast, testingKelas);
            //var accurationTestingData = enn.Accuration(forecast, testingKelas);
            //saveFile(YHattTraining, trainingKelas, "comparasionTraining", name);
            //saveFile(forecast, testingKelas, "comparasionTesting", name);

            var rows = new string[] { iterasi + ";" + n + ";" + PersenData[i] + ";" + string.Join(", ", KombinasiChannels[j]) + ";" + JumlahNeuron[l] + ";" + NilaiRandomAlpha[m] + ";" + accTrainingData + ";" + accTesting+"\n" };

            Console.WriteLine("Iterasi Ke-"+iterasi + "; Percobaan Ke-" + n + "; PersenDataTraining : " + PersenData[i] + "; Kombinasi Channels" + string.Join(", ", KombinasiChannels[j]) + "; Jumlah Neuron " + JumlahNeuron[l] + ";" + NilaiRandomAlpha[m] + "; Akurasi Kepada Training Data " + accTrainingData+ "; Akurasi Kepada Testing Data " + accTesting);
            File.AppendAllText(date+".csv", rows[0]);
        }
    }
}
