using Butterworth;
using LinearTimeInvariantProperties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCICompetitionCLI
{
    class Program
    {
        private static List<string> _dataPath;
        private static List<string> _artifactPath;
        private static List<string> _classLabelPath;
        private static List<string> _triggerPath;
        private static List<List<int>> _ClassLabel;
        private static List<List<int>> _Trigger;
        private static List<List<double[]>> _Data;
        private static int _AllChannelsHave;
        private static int _TriggerTimeStart;
        private static int _TriggerTimeStop;
        private static int _EpocSizeTraining;
        private static List<List<bool>> _Artifact;
        private static double[][][][] _Subject;
        private static int _SamplingRate;
        private static int[] _ChannelsChoose;

        [STAThread]
        static void Main(string[] args)
        {
            opendatasets();
            Console.WriteLine("Masukkan Samplingrate : ");
            _SamplingRate = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Channels (Contoh Input : 0,1,2,..,N (Lihat Dokumentasi Datasets)) : ");
            _ChannelsChoose = Array.ConvertAll(Console.ReadLine().Split(','),int.Parse);
            Console.WriteLine("Masukkan Times Triggers Start : ");
            _TriggerTimeStart = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Masukkan Times Triggers Stop : ");
            _TriggerTimeStop = Convert.ToInt32(Console.ReadLine());
            _EpocSizeTraining = (_TriggerTimeStop - _TriggerTimeStart) * _SamplingRate;
            _Artifact = new List<List<bool>>();
            _ClassLabel = new List<List<int>>();
            _Trigger = new List<List<int>>();
            _Data = new List<List<double[]>>();

            for (int i = 0; i < _dataPath.Count; i++)
            {
                ReadArtifactSelection(_artifactPath[i]);
                Console.WriteLine("Datasets " + Path.GetFileNameWithoutExtension(_dataPath[i]) + " List Artifact Load Done");
            }
            for (int i = 0; i < _dataPath.Count(); i++)
            {
                ReadClassLabels(_classLabelPath[i]);
            }
            for (int i = 0; i < _dataPath.Count; i++)
            {
                ReadTrigger(_triggerPath[i]);
                Console.WriteLine("Datasets " + Path.GetFileNameWithoutExtension(_dataPath[i]) + " List Trigger Load Done");
            }
            for (int i = 0; i < _dataPath.Count; i++)
            {
                ReadData(_dataPath[i]);
                Console.WriteLine("Datasets " + Path.GetFileNameWithoutExtension(_dataPath[i]) + " List Data EEG Load Done");
            }
            int countFile = _Data.Count();
            _Subject = new double[countFile][][][];//subject //perStimulus //times //Channels
            Parallel.For(0, countFile, i =>
            {
                var perData = new double[_Trigger[i].Count][][];
                Parallel.For(0, _Trigger[i].Count(), j =>
                {
                    var startFrom = _Trigger[i][j] + (_SamplingRate * _TriggerTimeStart);
                    var endTo = startFrom + (_SamplingRate * (_TriggerTimeStop - _TriggerTimeStart));
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
                        Parallel.For(0, pivotTimes.Length, m =>
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
                for (int j = 0; j < ExtractFeature[i].Length; j++)
                {
                    if (!_Artifact[i][j])
                    {
                        saveFeature(ExtractFeature[i][j], Path.GetFileNameWithoutExtension(_dataPath[i]), _ClassLabel[i][j]);
                    }
                }
                Console.WriteLine("Subject-" + i + " Save Feature Done");
            });
        }
        static OpenFileDialog OpenFileDialog(string title, string initialDirectory, string format, bool MultiSelect)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = Path.Combine(Application.StartupPath, initialDirectory);
            // Set the file dialog to filter for graphics files.
            openfile.Filter =
                format;
            openfile.Multiselect = MultiSelect;
            openfile.Title = title;
            return openfile;
        }
        static void opendatasets()
        {
            var openfile = OpenFileDialog("Muat Fitur", "Datasets", "DATASETS ASCII Files (*.data)|*.data", true);
            var dialogResult = openfile.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _dataPath = new List<string>();
                _artifactPath = new List<string>();
                _classLabelPath = new List<string>();
                _triggerPath = new List<string>();
                foreach (string file in openfile.FileNames)
                {
                    string directoryPath = Path.GetDirectoryName(file);
                    string[] namesOfFiles = Directory.GetFiles(directoryPath);
                    var fileGroups = from f in namesOfFiles
                                     group f by Path.GetFileNameWithoutExtension(f) into g
                                     select new { Name = g.Key, FileNames = g };
                    foreach (var g in fileGroups)
                    {
                        foreach (var fname in g.FileNames)
                        {
                            if (Path.GetFileNameWithoutExtension(fname) == Path.GetFileNameWithoutExtension(file))
                            {
                                switch (Path.GetExtension(fname))
                                {
                                    case ".HDR_ArtifactSelection":
                                        _artifactPath.Add(fname);
                                        break;
                                    case ".HDR_Classlabel":
                                        _classLabelPath.Add(fname);
                                        break;
                                    case ".HDR_TRIG":
                                        _triggerPath.Add(fname);
                                        break;
                                    case ".data":
                                        _dataPath.Add(fname);
                                        break;
                                }
                            }
                        }
                    }
                }
                _ClassLabel = new List<List<int>>();
                for (int i = 0; i < _dataPath.Count(); i++)
                {
                    ReadClassLabels(_classLabelPath[i]);
                    _AllChannelsHave = getChannelsCounts(_dataPath[i]);
                    Console.WriteLine("Datasets " + Path.GetFileNameWithoutExtension(_dataPath[i]) + ", Jumlah Data : " + _ClassLabel[i].Count() + ", Jumlah Channels : " + _AllChannelsHave);
                }
            }
        }
        static void ReadClassLabels(string path)
        {
            var contents = File.ReadLines(path);
            var newLabel = new List<int>();
            foreach (var row in contents)
            {
                if (row.Split('\t')[0] == "NaN") newLabel.Add(0);
                else newLabel.Add(Convert.ToInt16(row.Split('\t')[0]));
            }
            _ClassLabel.Add(newLabel);
        }

        static void ReadArtifactSelection(string path)
        {
            var contents = File.ReadLines(path);
            var newArtifact = new List<bool>();
            foreach (var row in contents)
            {
                newArtifact.Add(Convert.ToBoolean(Convert.ToInt16(row)));
            }
            _Artifact.Add(newArtifact);
        }

        static void ReadTrigger(string path)
        {
            var contents = File.ReadLines(path);
            var newTrigger = new List<int>();
            foreach (var row in contents)
            {
                newTrigger.Add(Convert.ToInt32(row));
            }
            _Trigger.Add(newTrigger);
        }

        static void ReadData(string path)
        {
            var contents = File.ReadLines(path);
            var newData = new List<double[]>();
            foreach (var row in contents)
            {
                var timesAllChannels = row.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //var times = new double[] { timesAllChannels[26], timesAllChannels[5], timesAllChannels[9], timesAllChannels[36] };
                var times = new List<double>();
                foreach (var ch in _ChannelsChoose)
                {
                    times.Add(double.Parse(timesAllChannels[ch], CultureInfo.InvariantCulture));
                }
                newData.Add(times.ToArray());
            }
            _Data.Add(newData);
        }

        static int getChannelsCounts(string path)
        {
            int count = 0;
            using (var reader = new StreamReader(path))
            {
                for (int i = 0; i < 1; i++)
                {
                    var timesAllChannels = Array.ConvertAll(reader.ReadLine().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), Double.Parse);
                    count = timesAllChannels.Length;
                }
            }
            return count;
        }

        static double[] dftToOneRowFeature(double[][] dft)
        {
            var temp = new double[dft.Length][];
            Parallel.For(0, dft.Length, j => {
                temp[j] = dft[j].Take(_EpocSizeTraining * (30 + 1) / _SamplingRate).Skip(_EpocSizeTraining * 8 / _SamplingRate).ToArray();
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

        static bool saveFeature(double[] data, string subject, int arah)
        {
            string filename = "Feature\\" + subject;

            foreach (var s in data)
            {
                File.AppendAllText(filename + ".csv", s + ";");
            }
            File.AppendAllText(filename + ".csv", arah + Environment.NewLine);
            return true;
        }
    }
}
