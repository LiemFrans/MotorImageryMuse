using Butterworth;
using LinearTimeInvariantProperties;
using LSL;
using MachineLearning.NeuralNetwork.ExtremeNeuralNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExperimentalControlling
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        private static liblsl.StreamInfo[] results;
        private static liblsl.StreamInlet inlet;
        private static int _kanal;
        private static int _chunks = 1;
        private static int _samplingrate = 256;
        private static int _epocSizeTesting = 6 * _samplingrate;
        private static ExtremeNeuralNetwork enn;
        static void Main(string[] args)
        {
            var Min = Array.ConvertAll(File.ReadAllLines("Min.csv")[0].Split(';'),double.Parse);
            var Max = Array.ConvertAll(File.ReadAllLines("Max.csv")[0].Split(';'), double.Parse) ;
            var fileWeight = File.ReadAllLines("model.weight");
            var betaHatt = Array.ConvertAll(File.ReadAllLines("model.betaHatt"),double.Parse);
            var weight = new double[fileWeight.Length, fileWeight[0].Split(';').Count()];
            for(int i = 0; i < fileWeight.Length; i++)
            {
                var temp = Array.ConvertAll(fileWeight[i].Split(';'), double.Parse);
                for(int j = 0; j < temp.Length; j++)
                {
                    weight[i, j] = temp[j];
                }
            }
            do
            {
                results = liblsl.resolve_stream("type", "EEG", 0, 5);
                if (results.Length != 0)
                {
                    Console.WriteLine("Menghubungkan Oke");
                    inlet = new liblsl.StreamInlet(results[0]);
                    Console.WriteLine("Nama Perangkat\t\t: " + inlet.info().name() + "\n" +
                                       "Nomor UID Perangkat\t: " + inlet.info().uid() + "\n" +
                                       "Versi Perangkat\t\t: " + inlet.info().version() + "\n" +
                                       "Sumber Arus EEG\t\t: " + inlet.info().source_id() + "\n" +
                                       "Jumlah Kanal\t\t: " + inlet.info().channel_count() + " Kanal\n" +
                                       "Sampling rate\t\t: " + inlet.info().nominal_srate() + "\n" +
                                       "Bentuk nilai Kanal\t\t: " + inlet.info().channel_format() + "\n");
                    _kanal = inlet.info().channel_count();
                }
                else
                {
                    Console.WriteLine("Tidak ditemukan arus EEG dari aplikasi BlueMuse atau perangkat EEG tidak terhubung ke komputer! Check kembali");
                }
            } while (results.Length == 0);
            Console.WriteLine("==============================================================");
            Console.WriteLine("=========================Mulai Stream=========================");
            do
            {
                enn = new ExtremeNeuralNetwork();
                var epoching = new List<float[]>();
                float[,] buffer = new float[_chunks, _kanal];
                double[] timestamps = new double[_chunks];
                inlet = new liblsl.StreamInlet(results[0]);
                Stopwatch watch = new Stopwatch();
                watch.Start();
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
                Console.WriteLine(watch.ElapsedMilliseconds + "ms");
                watch.Stop();
                var pecah = pecahSinyal(epoching);
                var preprocess = preprocessSignal(pecah);
                var dft = featureExtraction(preprocess).ToArray();
                var feature = dftToOneRowFeature(dft);
                var norm = new double[1,feature.Length];
                for (int s = 0; s < feature.Length; s++)
                {
                    norm[0,s] = Normalisasi(feature[s], Min[s], Max[s]);
                }
                var kelas = enn.testing(norm, weight, betaHatt, Activation.SigmoidBiner);
                var keyboard = new Keyboard();
                switch (Convert.ToInt16(Math.Round(kelas[0])))
                {
                    case 1:
                        Console.WriteLine("Kiri");
                        for (int loop = 0; loop < 2; loop++)
                        {
                            keyboard.Send(Keyboard.ScanCodeShort.KEY_A);
                            Thread.Sleep(500);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Kanan");
                        for (int loop = 0; loop < 2; loop++)
                        {
                            keyboard.Send(Keyboard.ScanCodeShort.KEY_D);
                            Thread.Sleep(500);
                        }
                        break;
                    default:
                        Console.WriteLine("Bukan Keduanya");
                        keyboard.Send(Keyboard.ScanCodeShort.KEY_W); break;
                }
            } while (true);

        }
        private static List<double[]> pecahSinyal(List<float[]> sinyal)
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
        private static List<double[]> preprocessSignal(List<double[]> sinyal)
        {
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
            return sinyalFilter;
        }
        private static List<double[]> featureExtraction(List<double[]> sinyal)
        {
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
            return dft;
        }
        private static double[] dftToOneRowFeature(double[][] dft)
        {
            var temp = new double[dft.Length][];
            Parallel.For(0, dft.Length, j => {
                temp[j] = dft[j].Take((_epocSizeTesting * (30 + 1) / _samplingrate)).Skip(_epocSizeTesting * 8 / _samplingrate).ToArray();
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
        private static double Normalisasi(double x, double min, double max)
        {
            return (x - min) / (max - min);
        }
    }
}
