using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSL;
namespace TestGrabEEGData
{
    class Program
    {
        private static int _samplingrate = 256;
        private static int _epocSizeTesting = 6 * _samplingrate;
        static void Main(string[] args)
        {
            DateTime now = DateTime.Now;
            liblsl.StreamInfo[] results= liblsl.resolve_stream("type", "EEG", 0, 5);
            Console.WriteLine("Found inverse in " + DateTime.Now.Subtract(now).TotalSeconds.ToString() + " seconds");
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);
            Console.WriteLine("Nama Perangkat\t\t: " + inlet.info().name() + "\n" +
                                       "Nomor UID Perangkat\t: " + inlet.info().uid() + "\n" +
                                       "Versi Perangkat\t\t: " + inlet.info().version() + "\n" +
                                       "Sumber Arus EEG\t\t: " + inlet.info().source_id() + "\n" +
                                       "Jumlah Kanal\t\t: " + inlet.info().channel_count() + " Kanal\n" +
                                       "Sampling rate\t\t: " + inlet.info().nominal_srate() + "\n" +
                                       "Bentuk nilai Kanal\t\t: " + inlet.info().channel_format() + "\n");
            int _kanal = inlet.info().channel_count();
            var epoching = new List<float[]>();
            float[,] buffer = new float[1, _kanal];
            double[] timestamps = new double[1];
            now = DateTime.Now;
            do
            {
                int num = inlet.pull_chunk(buffer, timestamps);
                for (int s = 0; s < num; s++)
                {
                    var d = new float[_kanal];
                    Parallel.For(0, _kanal, i =>
                    {
                        d[i] = buffer[s, i];
                    });
                    epoching.Add(d);
                    if (epoching.Count == _epocSizeTesting) break;
                }
            } while (epoching.Count < _epocSizeTesting);
            Console.WriteLine(epoching.Count);
            Console.WriteLine("Found inverse in " + DateTime.Now.Subtract(now).TotalSeconds.ToString() + " seconds");
            Console.ReadKey();
        }
    }
}
