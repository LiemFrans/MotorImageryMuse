using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MuseCSharpLSL
{
    public partial class StimulusHuruf : Form
    {
        public StimulusHuruf()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
        public void perbaharuiStimulus(string stimulus)
        {
            lbStimulus.Visible = true;
            lbStimulus.Text = stimulus;
            switch (stimulus)
            {
                case "Maju":
                    lbMaju.Text = "\u25B2";
                    break;
                case "Mundur":
                    lbMundur.Text = "\u25BC";
                    break;
                case "Kiri":
                    lbKiri.Text = "\u25C0";
                    break;
                case "Kanan":
                    lbKanan.Text = "\u25B6";
                    break;
                default:
                    lbMaju.Text = "";
                    lbMundur.Text = "";
                    lbKiri.Text = "";
                    lbKanan.Text = "";
                    break;
            }
        }
    }
}
