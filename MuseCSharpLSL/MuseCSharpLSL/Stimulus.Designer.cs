namespace MuseCSharpLSL
{
    partial class Stimulus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbStimulus = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbStimulus)).BeginInit();
            this.SuspendLayout();
            // 
            // pbStimulus
            // 
            this.pbStimulus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbStimulus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbStimulus.Location = new System.Drawing.Point(0, 0);
            this.pbStimulus.Name = "pbStimulus";
            this.pbStimulus.Size = new System.Drawing.Size(800, 450);
            this.pbStimulus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStimulus.TabIndex = 1;
            this.pbStimulus.TabStop = false;
            // 
            // Stimulus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbStimulus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Stimulus";
            this.Text = "Stimulus";
            ((System.ComponentModel.ISupportInitialize)(this.pbStimulus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbStimulus;
    }
}