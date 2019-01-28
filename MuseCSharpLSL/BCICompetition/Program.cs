using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Butterworth;
using LinearTimeInvariantProperties;

namespace BCICompetition
{
    class Program
    {
        private static List<List<bool>> _Artifact;
        private static List<List<int>> _ClassLabel;
        private static List<List<int>> _Trigger;
        private static List<List<double[]>> _Data;
        private static double[][][][] _Subject;
        private static int _SamplingRate = 250;
        private static int _BlankScreen = 2;
        private static int _FixationCross = 1;
        private static int _StimulusShown = 4;
        private static int _epocSizeTesting = _StimulusShown * _SamplingRate;
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _Artifact = new List<List<bool>>();
            _ClassLabel = new List<List<int>>();
            _Trigger = new List<List<int>>();

            foreach (string file in Directory.EnumerateFiles("datasets", "*.HDR_ArtifactSelection"))
            {
                var contents = File.ReadLines(file);
                var newArtifact = new List<bool>();
                foreach (var row in contents)
                {
                    newArtifact.Add(Convert.ToBoolean(Convert.ToInt16(row)));
                }
                _Artifact.Add(newArtifact);
            }
            foreach (string file in Directory.EnumerateFiles("datasets", "*.HDR_Classlabel"))
            {
                var contents = File.ReadLines(file);
                var newLabel = new List<int>();
                foreach (var row in contents)
                {
                    if (row == "NaN") newLabel.Add(5);
                    else newLabel.Add(Convert.ToInt16(row));
                }
                _ClassLabel.Add(newLabel);
            }
            foreach (string file in Directory.EnumerateFiles("datasets", "*.HDR_TRIG"))
            {
                var contents = File.ReadLines(file);
                var newTrigger = new List<int>();
                foreach (var row in contents)
                {
                    newTrigger.Add(Convert.ToInt32(row));
                }
                _Trigger.Add(newTrigger);
            }
            _Data = new List<List<double[]>>();
            foreach(string file in Directory.EnumerateFiles("datasets","*.data")){
                var contents = File.ReadLines(file);
                var newData = new List<double[]>();
                foreach (var row in contents)
                {
                    var timesAllChannels =  Array.ConvertAll(row.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), Double.Parse);
                    var times = new double[] {timesAllChannels[26],timesAllChannels[5],timesAllChannels[9],timesAllChannels[36] };
                    newData.Add(times);
                }
                _Data.Add(newData);
            }
            int countFile = _Data.Count();
            _Subject = new double[countFile][][][];//subject //perStimulus //times //Channels
            Parallel.For(0, countFile, i =>
             {
                 var perData = new double[_Trigger[i].Count][][];
                 Parallel.For(0, _Trigger[i].Count(), j =>
                {
                    var startFrom = _Trigger[i][j] + (_SamplingRate * (_FixationCross + _BlankScreen));
                    var endTo = startFrom + (_SamplingRate * _StimulusShown);
                    var newArrayTemp = new double[endTo - startFrom][];
                    int l = 0;
                    for (int k = startFrom; k < endTo; k++)
                    {
                        newArrayTemp[l] = _Data[i][k];
                        l++;
                    }
                    var pivotArray = new double[newArrayTemp[0].Length][];
                    Parallel.For(0, pivotArray.Length, k =>
                   {
                       var pivotTimes = new double[newArrayTemp.Length];
                       Parallel.For(0,pivotTimes.Length, m=>
                       {
                           pivotTimes[m] = newArrayTemp[m][k];
                       });
                       pivotArray[k] = pivotTimes;
                   });
                    perData[j] = pivotArray;
                });
                 _Subject[i] = perData;
             });

            
            var ApplyingButterworth = new double[countFile][][][];
            Parallel.For(0, ApplyingButterworth.Length, i =>
             {
                 var subjectRow = new double[_Subject[i].Length][][];
                 Parallel.For(0, subjectRow.Length, j =>
                  {
                      var channels = new double[_Subject[i][j].Length][];
                      Parallel.For(0, channels.Length, k =>
                        {
                            var bandpass = new DesignButterworth(Filter.BandPass, 5, 8, 30, _SamplingRate);
                            bandpass.Input = _Subject[i][j][k];
                            bandpass.iirInitialization();
                            bandpass.compute();
                            channels[k] = bandpass.Output;
                        });
                      subjectRow[j] = channels;
                  });
                 Console.WriteLine("Subject-" + i + " Butterworth Done");
                 ApplyingButterworth[i] = subjectRow;
             });

            var ApplyingDFT = new double[countFile][][][];
            Parallel.For(0, ApplyingDFT.Length, i =>
            {
                var subjectRow = new double[ApplyingButterworth[i].Length][][];
                Parallel.For(0, subjectRow.Length, j =>
                {
                    var channels = new double[ApplyingButterworth[i][j].Length][];
                    Parallel.For(0, channels.Length, k =>
                   {
                       var jumlahSample = ApplyingButterworth[i][j][k].Length / 2;
                       channels[k] = new DiscreteFourierTransform().discreteFourierTransform(ApplyingButterworth[i][j][k]).Take(jumlahSample).ToArray();
                   });
                    subjectRow[j] = channels;
                });
                Console.WriteLine("Subject-" + i + " DFT Done");
                ApplyingDFT[i] = subjectRow;
            });

            var ExtractFeature = new double[countFile][][];
            Parallel.For(0, ExtractFeature.Length, i =>
            {
                var subjectRow = new double[ApplyingDFT[i].Length][];
                Parallel.For(0, subjectRow.Length, j =>
                {
                    subjectRow[j] = dftToOneRowFeature(ApplyingDFT[i][j]);
                });
                Console.WriteLine("Subject-" + i + " Extraction Feature Done");
                ExtractFeature[i] = subjectRow;
            });
            Parallel.For(0, ExtractFeature.Length, i =>
            {
                for(int j = 0; j < ExtractFeature[i].Length; j++)
                {
                    if (!_Artifact[i][j])
                    {
                        saveFeature(ExtractFeature[i][j], i, _ClassLabel[i][j]-1);
                    }
                }
                Console.WriteLine("Subject-" + i + " Save Feature Done");
            });
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
            stopwatch.Stop();
        }
        private static double[] dftToOneRowFeature(double[][] dft)
        {
            var temp = new double[dft.Length][];
            Parallel.For(0, dft.Length, j => {
                temp[j] = dft[j].Take(_epocSizeTesting * 30 / _SamplingRate).Skip(_epocSizeTesting * 8 / _SamplingRate).ToArray();
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
        private static bool saveFeature(double[] data, int subject, int arah)
        {
            string filename = "Feature\\" + subject ;

            foreach (var s in data)
            {
                File.AppendAllText(filename + ".csv", s + ";");
            }
            File.AppendAllText(filename + ".csv", arah + Environment.NewLine);
            return true;
        }
    }
}
