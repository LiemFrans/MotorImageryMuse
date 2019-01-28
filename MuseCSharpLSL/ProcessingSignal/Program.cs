using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Butterworth;
using LinearTimeInvariantProperties;
namespace ProcessingSignal
{
    class Program
    {
        private static int _samplingrate = 256;
        private static int _epocSizeTesting = 6 * _samplingrate;

        static void Main(string[] args)
        {
            var nama = new string[] { "Ramda", "Nanda", "Veny", "Col", "Ekky", "Adri", "Yoga", "RefriR", "RefriR_Test", "Della", "Della_Test", "Galang", "Galang_Test", "Arif", "Arif_Test", "Dian", "Dian_Test", "Chen", "Chen_Test", "Rizky", "Rizky_Test" };
            var arah = new string[] { "Maju", "Mundur", "Berhenti", "Kiri", "Kanan" };
            double total = nama.Length * arah.Length*5;
            var now = 0;
            foreach (var n in nama)
            {
                foreach (var a in arah)
                {
                    var folderPath = "Sinyal\\" + n + "\\" + a;
                    foreach (string file in Directory.EnumerateFiles(folderPath, "*.csv"))
                    {
                        string dateTime = DateTime.Now.ToString("HH_mm_ss");
                        var channels = File.ReadLines(file);
                        var sinyalFilter = new double[channels.Count()][];
                        int i = 0;
                        foreach (var c in channels)
                        {
                            var signal = Array.ConvertAll(c.Split(';'), double.Parse);
                            var bandpass = new DesignButterworth(Filter.BandPass, 5, 8, 30, _samplingrate);
                            bandpass.Input = signal;
                            bandpass.iirInitialization();
                            bandpass.compute();
                            sinyalFilter[i] = bandpass.Output;
                            i++;
                        }
                        var dft = featureExtraction(sinyalFilter);
                        var feature = dftToOneRowFeature(dft);
                        saveFile(sinyalFilter,n, "BPF", a, dateTime);
                        saveFile(dft, n, "DFT", a, dateTime);
                        saveFeature(feature, n, a);
                        now++;
                        Console.WriteLine("DONE " + n+" "+a+" "+now+"/"+total);
                    }
                }
            }
            Console.ReadKey();

        }
        private static bool saveFile(double[][] data, string nama, string context, string arah, string dateTime)
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
            return true;
        }
        private static double[][] featureExtraction(double[][] sinyal)
        {
            var jumlahKanal = sinyal.Count();
            var jumlahSample = sinyal.First().Length / 2;
            var dft = new double[jumlahKanal][];
            for (int i = 0; i < jumlahKanal; i++)
            {
                dft[i]= new double[jumlahSample];
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
                temp[j] = dft[j].Take((_epocSizeTesting * (30+1) / _samplingrate)).Skip(_epocSizeTesting * 8 / _samplingrate).ToArray();
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
        private static bool saveFeature(double[] data, string nama, string arah)
        {
            string filename = "Feature\\" + nama;

            foreach (var s in data)
            {
                File.AppendAllText(filename + ".csv", s + ";");
            }
            switch (arah)
            {
                case "Maju":
                    File.AppendAllText(filename + ".csv", 1 + Environment.NewLine);
                    break;
                case "Mundur":
                    File.AppendAllText(filename + ".csv", 2 + Environment.NewLine);
                    break;
                case "Berhenti":
                    File.AppendAllText(filename + ".csv", 3 + Environment.NewLine);
                    break;
                case "Kiri":
                    File.AppendAllText(filename + ".csv", 4 + Environment.NewLine);
                    break;
                case "Kanan":
                    File.AppendAllText(filename + ".csv", 5 + Environment.NewLine);
                    break;
            }
            return true;
        }
        //private List<double[]> preprocessSignal(List<double[]> sinyal)
        //{
        //    updateAktivitas("Bandpass Filter");
        //    var watch = new Stopwatch();
        //    watch.Start();
        //    var jumlahKanal = sinyal.Count();
        //    var jumlahSample = sinyal.First().Length;
        //    var sinyalFilter = new List<double[]>();

        //    for (int i = 0; i < jumlahKanal; i++)
        //    {
        //        sinyalFilter.Add(new double[jumlahSample]);
        //    }
        //    Parallel.For(0, jumlahKanal, i => {
        //        var bandpass = new DesignButterworth(Filter.BandPass, 5, 8, 30, _samplingrate);
        //        bandpass.Input = sinyal[i];
        //        bandpass.iirInitialization();
        //        bandpass.compute();
        //        sinyalFilter[i] = bandpass.Output;
        //    });
        //    watch.Stop();
        //    updateAktivitas("Selesai Bandpass Filter " + watch.ElapsedMilliseconds + " ms");
        //    return sinyalFilter;
        //}
    }
}
