using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuseCLI
{
    public partial class Stimulus : Form
    {
        public Stimulus()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
        public void perbaharuiStimulus(string stimulus)
        {
            lbStimulus.Invoke((MethodInvoker)delegate { lbStimulus.Visible = true;  lbStimulus.Text = stimulus; });
            switch (stimulus)
            {
                case "Maju":
                    lbMaju.Invoke((MethodInvoker)delegate { lbMaju.Text = "\u25B2"; });
                    break;
                case "Mundur":
                    lbMundur.Invoke((MethodInvoker)delegate { lbMundur.Text = "\u25BC"; });
                    break;
                case "Kiri":
                    lbKiri.Invoke((MethodInvoker)delegate {  lbKiri.Text= "\u25C0"; });
                    break;
                case "Kanan":
                    lbKanan.Invoke((MethodInvoker)delegate { lbKanan.Text = "\u25B6"; });
                    break;
                default:
                    lbMaju.Invoke((MethodInvoker)delegate { lbMaju.Text = ""; });
                    lbMundur.Invoke((MethodInvoker)delegate { lbMundur.Text = ""; });
                    lbKiri.Invoke((MethodInvoker)delegate { lbKiri.Text = ""; });
                    lbKanan.Invoke((MethodInvoker)delegate { lbKanan.Text = ""; });
                    break;
            }
        }
    }
}
