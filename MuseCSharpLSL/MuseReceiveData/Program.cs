using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSL;
namespace MuseReceiveData
{
    class Program
    {
        static void Main(string[] args)
        {
            // wait until an EEG stream shows up
            liblsl.StreamInfo[] results = liblsl.resolve_stream("type", "EEG",0,5);
            // open an inlet and print some interesting info about the stream (meta-data, etc.)
            if (results.Length !=0)
            {
                liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);
                System.Console.Write(inlet.info().as_xml());
                Console.WriteLine(inlet.info().channel_count());
                Console.WriteLine(inlet.info().channel_format());
                Console.WriteLine(inlet.info().created_at());
                Console.WriteLine(inlet.info().desc());
                Console.WriteLine(inlet.info().handle());
                Console.WriteLine(inlet.info().hostname());
                Console.WriteLine(inlet.info().name());
                Console.WriteLine(inlet.info().nominal_srate());
                Console.WriteLine(inlet.info().session_id());
                Console.WriteLine(inlet.info().source_id());
                Console.WriteLine(inlet.info().uid());
                Console.WriteLine(inlet.info().version());
                // read samples
                Stopwatch watch = new Stopwatch();
                watch.Start();
                float[,] buffer = new float[1536,5];
                var epoching = new List<float[]>();
                int _kanal = 5;
                double[] timestamps = new double[1536];
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
                        //d[_kanal + 1] = (float)timestamps[s];
                        //for (int c = 0; c < 5; c++)
                        //    Console.Write("\t{0}", buffer[s, c]);
                        //Console.WriteLine();
                        epoching.Add(d);
                        if (epoching.Count == 1536)
                        {
                            Console.WriteLine("Masuk");
                            break;
                        }
                    }
                } while (epoching.Count <= 1536);
                Console.WriteLine(watch.ElapsedMilliseconds);
                Console.WriteLine(epoching.Count());
                watch.Reset();
            }
            else
            {
                Console.WriteLine("Kosong");
            }

            //// read samples
            //var listsampling = new List<float[]>();
            //float[] sample = new float[5];
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //Console.WriteLine("Wait");
            //while (watch.Elapsed.Seconds != 2)
            //{
            //}
            //var watch1 = new Stopwatch();
            //watch1.Start();
            //while (listsampling.Count != 1536)
            //{
            //    //TP9 AF7 AF8 TP10 RIGHT AUX
            //    inlet.pull_sample(sample);
            //    var d = new float[5];
            //    Parallel.For(0, 5, i =>
            //    {
            //        d[i] = sample[i];
            //    });
            //    listsampling.Add(d);
            //}
            //watch1.Stop();
            //watch.Restart();
            //Console.WriteLine("REST");
            //Console.WriteLine(watch1.ElapsedMilliseconds);
            //watch.Start();
            //while (watch.Elapsed.Seconds != 2)
            //{
            //}
            //Console.WriteLine(listsampling.Count);
            //Console.WriteLine(watch.ElapsedMilliseconds);
            //foreach(var l in listsampling)
            //{
            //    foreach( var s in l)
            //    {
            //        File.AppendAllText("coba.csv",s+"; ");
            //    }
            //    File.AppendAllText("coba.csv",Environment.NewLine);
            //}
            
            System.Console.ReadKey();

        }
    }
}
