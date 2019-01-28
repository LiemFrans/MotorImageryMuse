using Butterworth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinearTimeInvariantProperties;
using System.Threading;
using System.Diagnostics;

namespace TestButterworth
{
    public partial class TestButterworth : Form
    {
        private double[] yf2;
        private double[] y;

        public TestButterworth()
        {
            InitializeComponent();
            var row = File.ReadLines("D:\\Clouds\\Google Drive\\Dari Dropbox\\FILKOM - IF\\Semester 7\\Skripsi\\MuseCSharpLSL\\MuseCSharpLSL\\bin\\Debug\\RAW_HURUF\\Adri_Berhenti_RAW_HURUF_16_56_20.csv");

            y = Array.ConvertAll(row.First().Split(';'), double.Parse);

            var bandpass = new DesignButterworth(Filter.BandPass, 5, 8, 30, 256);
            bandpass.iirInitialization();
            bandpass.Input = y;
            bandpass.compute();
            yf2 = bandpass.Output;
            for (int i = 0; i < y.Length; i++)
            {
                //chart1.Series[0].Points.AddY(y[i]);
                chart1.Series[2].Points.AddY(y[i]);
            }
        }

        private void btnDFT_Click(object sender, EventArgs e)
        {
            ////signal + noise
            //double fs = 256; //sampling rate
            //double fw = 5; //signal frequency
            //double fn = 50; //noise frequency
            //double n = 5; //number of periods to show
            //double A = 10; //signal amplitude
            //double N = 1; //noise amplitude
            //int size = (int)(n * fs / fw); //sample size

            //var t = Enumerable.Range(1, size).Select(p => p * 1 / fs).ToArray();

            chart1.Series[2].Points.Clear();
            var yf3 = new DiscreteFourierTransform().discreteFourierTransform(yf2).Take(y.Length / 2).ToArray();


            for (int i = 0; i < yf3.Length; i++)
            {
                //chart1.Series[0].Points.AddY(y[i]);
                chart1.Series[2].Points.AddY(yf3[i]);
            }
            //double[] yf3 = bandpassnarrow.ProcessSamples(y); //Bandpass Narrow
        }

        private void btnBPF_Click(object sender, EventArgs e)
        {
            chart1.Series[2].Points.Clear();
            for (int i = 0; i < yf2.Length; i++)
            {
                //chart1.Series[0].Points.AddY(y[i]);
                chart1.Series[2].Points.AddY(yf2[i]);
            }
        }
    }
}
