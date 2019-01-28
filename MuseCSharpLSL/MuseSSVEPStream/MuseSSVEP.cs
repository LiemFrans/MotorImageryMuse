using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Butterworth;
using LinearTimeInvariantProperties;
using LSL;
namespace MuseSSVEPStream
{
    public partial class MuseSSVEP : Form
    {
        private liblsl.StreamInlet inlet;
        private liblsl.StreamInfo[] results;
        private int _samplingrate;
        private int _epocSizeTesting;
        private int _chunks = 1;
        private int _kanal;
        private bool _stream;
        public MuseSSVEP()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            bgKoneksi.RunWorkerAsync();
        }

        private void btnStream_Click(object sender, EventArgs e)
        {
            bgRekam.RunWorkerAsync();
        }

        private void bgKoneksi_DoWork(object sender, DoWorkEventArgs e)
        {
            mainFlow.Invoke((MethodInvoker)delegate { mainFlow.Enabled = false; });
            results = liblsl.resolve_stream("type", "EEG", 0, 5);
            if (results.Length != 0)
            {
                inlet = new liblsl.StreamInlet(results[0]);
                _kanal = inlet.info().channel_count();
                _samplingrate =(int)inlet.info().nominal_srate();
                _epocSizeTesting = (int)(numDelay.Value * _samplingrate);
                MessageBox.Show("Menghubungkan berhasil");
                mainFlow.Invoke((MethodInvoker)delegate { mainFlow.Enabled = true; });
                this.Invoke((MethodInvoker)delegate {
                    btnConnect.Enabled = false; 
                    btnStream.Enabled = true;
                    numDelay.Enabled = true;
                    rcOutputs.Text = "";
                });
            }
            else
            {
                MessageBox.Show("Menghubungkan Gagal");
                mainFlow.Invoke((MethodInvoker)delegate { mainFlow.Enabled = true; });
            }
        }

        private void bgRekam_DoWork(object sender, DoWorkEventArgs e)
        {
            rekam();
            if (bgRekam.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            _epocSizeTesting = (int)(numDelay.Value * _samplingrate);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _stream = false;
        }

        private void rekam()
        {
            this.Invoke((MethodInvoker)delegate
            {
                rcOutputs.Text = "";
                btnStop.Enabled = true;
                btnStream.Enabled = false;
            });
            _stream = true;
            do
            {
                float[,] buffer = new float[_chunks, _kanal];
            double[] timestamps = new double[_chunks];
            var epoching = new List<float[]>();

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
                        if (_stream != true) break;
                    }
                } while (epoching.Count < _epocSizeTesting);
                if (_stream != true) break;
                var pecah = pecahSinyal(epoching);
                var preprocess = preprocessSignal(pecah);
                var dft = featureExtraction(preprocess);
                var freq = findFreqMaxPower(dft);
                for(int i = 0; i < freq.Length; i++)
                {
                    rcOutputs.Invoke((MethodInvoker)delegate
                    {
                        rcOutputs.Text += freq[i] + "\t";
                    });
                }
                rcOutputs.Invoke((MethodInvoker)delegate
                {
                    rcOutputs.Text += "\n";
                });
                Console.Beep();
            } while (_stream);
            this.Invoke((MethodInvoker)delegate
            {
                btnStop.Enabled = false;
                btnStream.Enabled = true;
            });
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
        private List<double[]> featureExtraction(List<double[]> sinyal)
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
        private double[] findFreqMaxPower(List<double[]>sinyal)
        {
            var freq = new double[sinyal.Count()];
            for(int i = 0; i < freq.Count();i++)
            {
                var maxValue = sinyal[i].Max();
                var maxIndex = sinyal[i].ToList().IndexOf(maxValue);
                freq[i] = maxIndex * _samplingrate / _epocSizeTesting;
            }
                return freq;

        }
    }
}
