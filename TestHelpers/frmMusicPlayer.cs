using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestHelpers {
    public partial class frmMusicPlayer : Form {
        public delegate void IntValue(long Volume);
        public event IntValue VolumeChanged;

        public delegate void SimpleEvent();
        public event SimpleEvent PlayPause, NextSong;
        
        public void SetState(String Time, String Name) {
            this.lblTime.Text = Time;
            this.lblName.Text = Name;
        }
        
        public frmMusicPlayer() {
            InitializeComponent();

            Bitmap BmpBG = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(BmpBG);

            g.Clear(Color.FromArgb(200, 200, 200));
            g.DrawRectangle(new Pen(Color.FromArgb(66, 66, 66)), new Rectangle(0, 0, this.Width - 1, this.Height - 1));

            Color ColorExternal = Color.FromArgb(255, 0, 255);
            Color ColorInternal = Color.FromArgb(150, 0, 0, 0); //.FromArgb(19 * 0.7, 130 * 0.7, 206 * 0.7)
            BmpBG.SetPixel(0, 0, ColorExternal);
            BmpBG.SetPixel(1, 1, ColorInternal);
            BmpBG.SetPixel(BmpBG.Width - 1, BmpBG.Height - 1, ColorExternal);
            BmpBG.SetPixel(BmpBG.Width - 2, BmpBG.Height - 2, ColorInternal);
            BmpBG.SetPixel(BmpBG.Width - 1, 0, ColorExternal);
            BmpBG.SetPixel(BmpBG.Width - 2, 1, ColorInternal);
            BmpBG.SetPixel(0, BmpBG.Height - 1, ColorExternal);
            BmpBG.SetPixel(1, BmpBG.Height - 2, ColorInternal);
        
            this.BackgroundImage = BmpBG;
            this.Refresh();

            btnNext.LoadIcon("player\\next_mini_dark.png");
            btnPlayPause.LoadIcon(Application.StartupPath+"\\player\\play_mini_dark.png");

            sbNext.LoadImages("player\\next_mini.png", Application.StartupPath);
            sbPlayPause.LoadImages("player\\play_mini.png", Application.StartupPath);
        }


        private void tmrMousePos_Tick(object sender, EventArgs e) {
            Screen[] Screens =  Screen.AllScreens;
            foreach (Screen S in Screens) {
                if (MousePosition.X > S.Bounds.Left + S.Bounds.Width - 10 && MousePosition.Y > S.Bounds.Top + S.Bounds.Height - 10 &&
                    MousePosition.X <= S.Bounds.Left + S.Bounds.Width && MousePosition.Y <= S.Bounds.Top + S.Bounds.Height) {
                    this.Left = S.Bounds.Left + S.Bounds.Width - this.Width + 1;
                    this.Top = S.Bounds.Top + S.Bounds.Height - this.Height + 1;
                    this.Show();
                }
            }

            int Border = 5;
            if (MousePosition.X < this.Left - Border || MousePosition.Y < this.Top - Border ||
            MousePosition.X > this.Left + this.Width + Border || MousePosition.Y > this.Top + this.Height + Border) {
                this.Hide();
            }
        }

        Point PrevPos;
        private void label1_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button != System.Windows.Forms.MouseButtons.None) {
                VolumeChanged(Convert.ToInt32(label1.Text) + e.X - e.Y - PrevPos.X + PrevPos.Y);
                label1.Text = (Convert.ToInt32(label1.Text) + e.X - e.Y - PrevPos.X + PrevPos.Y).ToString();
                PrevPos = e.Location;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e) {
            PrevPos = e.Location;
        }


        private void btnPlayPause_Click(object sender, EventArgs e) {
            PlayPause();
        }

        private void ucSpotButton2_Load(object sender, EventArgs e) {

        }

        private void ucSpotButton1_Load(object sender, EventArgs e) {

        }

        private void btnNext_Click(object sender, EventArgs e) {
            NextSong();
        }

        private void btnNext_Load(object sender, EventArgs e) {

        }

        private void sbPlayPause_Load(object sender, EventArgs e) {
            
        }

        private void sbNext_Load(object sender, EventArgs e) {
            
        }

        private void sbPlayPause_Click(object sender, EventArgs e) {
            PlayPause();
        }

        private void sbNext_Click(object sender, EventArgs e) {
            NextSong();
        }

    }
}
