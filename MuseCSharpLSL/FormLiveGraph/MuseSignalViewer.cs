using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSL;
namespace FormLiveGraph
{
    public partial class MuseSignalViewer : Form
    {
        private int _sizeX = 256;
        public MuseSignalViewer()
        {
            InitializeComponent();
            chStream.ChartAreas[0].AxisX.Minimum = 0;
            chStream.ChartAreas[0].AxisX.Maximum = _sizeX;
            chStream.ChartAreas[0].AxisY.Minimum = -100;
            chStream.ChartAreas[0].AxisY.Maximum = 600;
        }

        private void btnVerifikasi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sudah terkalibrasi dengan benar di aplikasi Muse Direct?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                btnKoneksi.Enabled = true;
                btnVerifikasi.Enabled = false;
            }
            else
            {
                MessageBox.Show("Silahkan Kalibrasi pada aplikasi Muse Direct", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        liblsl.StreamInlet inlet;
        private liblsl.StreamInfo[] results;
        private int _kanal;
        private int _chunks = 1;
        private static int _samplingrate = 256;
        private int _epocSizeTesting = 6 * _samplingrate;
        private void bgKoneksi_DoWork(object sender, DoWorkEventArgs e)
        {
            groupPreparasi.Invoke((MethodInvoker)delegate { groupPreparasi.Enabled = false; });
            results = liblsl.resolve_stream("type", "EEG", 0, 5);
            if (results.Length != 0)
            {

                this.Invoke((MethodInvoker)delegate
                {
                    btnKoneksi.Enabled = false;
                    btnPindai.Enabled = true;
                });
                inlet = new liblsl.StreamInlet(results[0]);
                _kanal = inlet.info().channel_count();
                MessageBox.Show("Telah Terhubung", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Tidak ditemukan arus EEG dari aplikasi BlueMuse atau perangkat EEG tidak terhubung ke komputer! Check kembali", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            groupPreparasi.Invoke((MethodInvoker)delegate { groupPreparasi.Enabled = true; });
        }

        private void btnPindai_Click(object sender, EventArgs e)
        {
            bgPindai.RunWorkerAsync();
        }

        private void btnKoneksi_Click(object sender, EventArgs e)
        {
            bgKoneksi.RunWorkerAsync();
        }
        private bool stop;
        private Stopwatch watch;

        private void bgPindai_DoWork(object sender, DoWorkEventArgs e)
        {
            stop = true;
            btnKoneksi.Invoke((MethodInvoker)delegate { btnPindai.Enabled = false; });
            btnBerhenti.Invoke((MethodInvoker)delegate { btnBerhenti.Enabled = true; });
            inlet = new liblsl.StreamInlet(results[0]);
            float[,] buffer = new float[_chunks, _kanal];
            double[] timestamps = new double[_chunks];
            watch = new Stopwatch();
            watch.Start();
            do
            {
                int num = inlet.pull_chunk(buffer, timestamps);
                //for (int s = 0; s < num; s++)
                //{
                    //var d = new float[_kanal];
                    //Parallel.For(0, _kanal-1, i =>
                    //{
                        chStream.Invoke((MethodInvoker)delegate
                        {
                            if (chStream.Series[0].Points.Count==_sizeX)
                            {
                                Console.WriteLine(watch.Elapsed.Milliseconds+" ms");
                                chStream.Series[0].Points.Clear();
                                chStream.Series[1].Points.Clear();
                                chStream.Series[2].Points.Clear();
                                chStream.Series[3].Points.Clear();
                                watch.Restart();
                            } 
                            chStream.Series[0].Points.AddY(buffer[0,0]+(150*(0)));
                            chStream.Series[1].Points.AddY(buffer[0, 1] + (150 * (1)));
                            chStream.Series[2].Points.AddY(buffer[0, 2] + (150 * (2)));
                            chStream.Series[3].Points.AddY(buffer[0, 3] + (150 * (3)));
                        });
                        //d[i] = buffer[s, i];
                    //});
                //}

            } while (stop);
        }

        private void btnBerhenti_Click(object sender, EventArgs e)
        {
            stop = false;
            btnPindai.Enabled = true;
            btnBerhenti.Enabled = false;
        }
    }
}
