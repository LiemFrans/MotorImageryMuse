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
            pbStimulus.Visible = true;
            pbStimulus.BackColor = Color.Transparent; 
            switch (stimulus)
            {
                case "+":
                    pbStimulus.Image = Image.FromFile("Stimulus/plus.png");
                    break;
                case "Maju":
                    pbStimulus.Image = Image.FromFile("Stimulus/Maju.png");
                    break;
                case "Mundur":
                    pbStimulus.Image = Image.FromFile("Stimulus/Mundur.png");
                    break;
                case "Berhenti":
                    pbStimulus.Image = Image.FromFile("Stimulus/Berhenti.png");
                    break;
                case "Kiri":
                    pbStimulus.Image = Image.FromFile("Stimulus/Kiri.png");
                    break;
                case "Kanan":
                    pbStimulus.Image = Image.FromFile("Stimulus/Kanan.png");
                    break;
                case "":
                    pbStimulus.Visible = false;
                    break;
            }
        }
    }
}
