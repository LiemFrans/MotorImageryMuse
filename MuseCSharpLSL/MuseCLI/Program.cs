using Butterworth;
using LinearTimeInvariantProperties;
using LSL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuseCLI
{
    class Program
    {
        static Stimulus stimulus;
        static int _kanal;
        static int _chunks = 1;
        static int _samplingrate = 256;
        static int _epocSizeTesting = 6 * _samplingrate;
        private static liblsl.StreamInlet inlet;
        private static liblsl.StreamInfo[] results;
        private static string nama;

        static void Main(string[] args)
        {
            string[] folder = { "RAW_HURUF\\", "BPF_HURUF\\", "DFT_HURUF\\", "FEATURE\\" };
            foreach (var f in folder)
            {
                if (!Directory.Exists(f))
                {
                    Directory.CreateDirectory(f);
                }
            }
            var kalibrasi = "y";
            do
            {
                Console.WriteLine("Sudah terkalibrasi dengan benar di aplikasi Muse Direct?");
               kalibrasi =  Console.ReadLine();
            } while (kalibrasi =="n");
            do
            {

            } while (!koneksi());
            var lagi = "y";
            do
            {
                Console.WriteLine("Nama : ");
                nama = Console.ReadLine();
                var arah = new string[] { "Maju", "Mundur", "Berhenti", "Kiri", "Kanan" };
                Console.WriteLine("Perulangan : ");
                var perulangan = Convert.ToInt16(Console.ReadLine());
                for (int i = 0; i < perulangan; i++)
                {
                    stimulus = new Stimulus();
                    stimulus.Show();
                    for (int j = 0; j < arah.Length; j++)
                    {
                        rekam(arah[j]);
                    }
                }
                stimulus.Close();
                Console.WriteLine("Lagi?(y/n)");
                lagi = Console.ReadLine();

            } while (lagi == "y");
        }
        static bool koneksi()
        {
            Console.WriteLine("Menghubungkan ke BlueMuse . . .");
            results = liblsl.resolve_stream("type", "EEG", 0, 5);
            if (results.Length != 0)
            {
                Console.WriteLine("Menghubungkan OK");
                inlet = new liblsl.StreamInlet(results[0]);
                Console.WriteLine("Nama Perangkat\t\t: " + inlet.info().name() + "\n" +
                                       "Nomor UID Perangkat\t: " + inlet.info().uid() + "\n" +
                                       "Versi Perangkat\t\t: " + inlet.info().version() + "\n" +
                                       "Sumber Arus EEG\t\t: " + inlet.info().source_id() + "\n" +
                                       "Jumlah Kanal\t\t: " + inlet.info().channel_count() + " Kanal\n" +
                                       "Sampling rate\t\t: " + inlet.info().nominal_srate() + "\n" +
                                       "Bentuk nilai Kanal\t\t: " + inlet.info().channel_format() + "\n");
                _kanal = inlet.info().channel_count();
                return true;
            }
            else
            {
                Console.WriteLine("Tidak ditemukan arus EEG dari aplikasi BlueMuse atau perangkat EEG tidak terhubung ke komputer! Check kembali");
                Console.WriteLine("Menghubungkan ke BlueMuse gagal . . .");
                return false;
            }

        }
        static void rekam(string arah)
        {
            var epoching = new List<float[]>();
            var watch = new Stopwatch();
            watch.Start();
            Console.WriteLine("Fokus");
            stimulus.perbaharuiStimulus("+");
            stimulus.Update();
            do
            {

            } while (watch.Elapsed.Seconds != 2);
            Console.WriteLine(watch.ElapsedMilliseconds + " ms");
            watch.Restart();
            stimulus.perbaharuiStimulus(arah);
            stimulus.Update();
            float[,] buffer = new float[_chunks, _kanal];
            double[] timestamps = new double[_chunks];
            inlet = new liblsl.StreamInlet(results[0]);
            Console.WriteLine("Rekam " + arah);
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
            watch.Stop();
            Console.WriteLine(Convert.ToString(watch.ElapsedMilliseconds)+ " ms");
            watch.Restart();
            stimulus.perbaharuiStimulus("");
            stimulus.Update();
            Console.WriteLine("Rest");
            do
            {
            } while (watch.Elapsed.Seconds != 2);
            watch.Stop();
            string dateTime = DateTime.Now.ToString("HH_mm_ss");
            var pecah = pecahSinyal(epoching);
            var preprocess = preprocessSignal(pecah);
            var dft = featureExtraction(preprocess);
            var feature = dftToOneRowFeature(dft);

            saveFile(pecah, "RAW_HURUF", arah, dateTime);
            saveFile(preprocess, "BPF_HURUF", arah, dateTime);
            saveFile(dft, "DFT_HURUF", arah, dateTime);
            saveFeature(feature, arah, "HURUF");
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
            Console.WriteLine("Bandpass Filter");
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
            Console.WriteLine("Selesai Bandpass Filter " + watch.ElapsedMilliseconds + " ms");
            return sinyalFilter;
        }
        private static List<double[]> featureExtraction(List<double[]> sinyal)
        {
            Console.WriteLine("DFT");
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
            Console.WriteLine("Selesai DFT " + watch.ElapsedMilliseconds + " ms");
            return dft;
        }
        private static double[] dftToOneRowFeature(List<double[]> dft)
        {
            var temp = new double[dft.Count][];
            Parallel.For(0, dft.Count, j => {
                temp[j] = dft[j].Take(_epocSizeTesting * 30 / _samplingrate).Skip(_epocSizeTesting * 8 / _samplingrate).ToArray();
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
        private static bool saveFile(List<double[]> data, string context, string arah, string dateTime)
        {
            string filename = context + "\\" + nama + "_" + arah + "_" + context + "_" + dateTime;

            foreach (var row in data)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (i == row.Length - 1) File.AppendAllText(filename + ".csv", row[i] + Environment.NewLine);
                    else File.AppendAllText(filename + ".csv", row[i] + ";");
                }
            }
            Console.WriteLine("simpan selesai " + filename + ".csv");
            return true;
        }
        private static bool saveFile(double[,] data, string context, string filename)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                var temp = "";
                for (int j = 0; j < data.GetLength(1) - 1; j++)
                {
                    temp += data[i, j] + ";";
                }
                temp += data[i, data.GetLength(1) - 1] + "\n";
                File.AppendAllText(context + "\\" + filename + "." + context, temp);
            }
            Console.WriteLine("simpan selesai " + filename + ".csv");
            return true;
        }
        private static bool saveFile(double[] data, string context, string filename)
        {
            for (int i = 0; i < data.Length; i++)
            {
                File.AppendAllText(context + "\\" + filename + "." + context, data[i] + "\n");
            }
            return true;
        }
        private static bool saveFeature(double[] data, string arah, string stimulus)
        {
            string filename = "Feature\\" + nama + "_" + stimulus;

            foreach (var s in data)
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
            Console.WriteLine("simpan selesai " + filename + ".csv");
            return true;
        }
    }
}
