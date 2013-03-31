using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestHelpers {
    public partial class ucSpotButton : UserControl {
        public ucSpotButton() {
            InitializeComponent();
        }
        private Bitmap bmpMain;
        private Graphics gMain;
        private Bitmap bmpIcon;

        private class Spot {
            public PointF FirstMouseLocation;
            public float State;
            public bool Enabled;
            public float Speed;
            private float LastDR;
            private float MaxState;
            public Spot(float NewSpeed = 0.35F) {
                State = 0;
                Speed = NewSpeed;
                Enabled = false;
                FirstMouseLocation = new PointF(0, 0);
                LastDR = 0;
                MaxState = 0.95F;
            }
            public RectangleF GetSpotRect(PointF Center) {
                PointF CurrentLocation = new PointF(FirstMouseLocation.X + (Center.X - FirstMouseLocation.X) * State, FirstMouseLocation.Y + (Center.Y - FirstMouseLocation.Y) * State);
                return new RectangleF(CurrentLocation.X - State * Center.X, CurrentLocation.Y - State * Center.Y, State * Center.X * 2, State * Center.Y * 2);
            }
            public void NextState() {
                if (Enabled) {
                    if (State < MaxState * 0.999) { LastDR = (MaxState - State) * Speed; State += LastDR; } else State = MaxState; 
                }
                else {
                    if (State > 0.001) { LastDR *= 0.1F; State -= Speed / 40 - LastDR; } else State = 0;
                    //if (State > 0.001) State -= (State) * Speed / 6; else State = 0;
                }
            }
        }
        private class DinamicDouble {
            private double Value;
            private double destValue;
            
        }
        private Spot SpotMD = new Spot(0.9F), SpotMU = new Spot(0.15F);

        Color ColorBG = Color.FromArgb(200, 200, 200);
        Color ColorButtonBG = Color.FromArgb(215, 215, 215);
        Color ColorButtonMouseDown = Color.FromArgb(235, 235, 235);
        bool IsMouseDown = false;

        private void Redraw() {
            PointF Center = new PointF(this.Width / 2, this.Height / 2);

            //Color ColorBG = Color.FromArgb(80, 80, 80);
            //Color ColorButtonBG = Color.FromArgb(40, 40, 40);
            //Color ColorButtonMouseDown = Color.FromArgb(120, 120, 120);

            //Color ColorBG = Color.FromArgb(0, 0, 0);
            //Color ColorButtonBG = Color.FromArgb(60, 60, 60);
            //Color ColorButtonMouseDown = Color.FromArgb(120, 120, 120);

            //Color ColorBG = Color.FromArgb(245, 245, 245);
            //Color ColorButtonBG = Color.FromArgb(255,255,255);
            //Color ColorButtonMouseDown = Color.FromArgb(220, 220, 220);

            //Color ColorBG = Color.FromArgb(245, 245, 245);
            //Color ColorButtonBG = Color.FromArgb(245, 245, 245);
            //Color ColorButtonMouseDown = Color.FromArgb(255, 255, 255); //+//Color.FromArgb(19, 130, 206);

            ////Color ColorBG = Color.FromArgb(200,200,200);
            ////Color ColorButtonBG = Color.FromArgb(215, 215, 215);
            //////Color ColorButtonBG = Color.FromArgb(200, 200, 200); //Color.FromArgb(180, 180, 180);
            ////Color ColorButtonMouseDown = Color.FromArgb(235, 235, 235);

            //Color ColorBG = Color.FromArgb(245, 245, 245);
            //Color ColorButtonBG = Color.FromArgb(220, 220, 220);
            //Color ColorButtonMouseDown = Color.FromArgb(200, 200, 200);

            //Color ColorBG = Color.FromArgb(245, 245, 245);
            //Color ColorButtonBG = Color.FromArgb(220, 220, 220);
            //Color ColorButtonMouseDown = Color.FromArgb(245, 245, 245); -

            gMain.Clear(ColorBG);

            if (!IsMouseDown) gMain.FillEllipse(new SolidBrush(ColorButtonBG), 0, 0, bmpMain.Width-1, bmpMain.Height-1);
            else gMain.FillEllipse(new SolidBrush(Color.White), 0, 0, bmpMain.Width - 1, bmpMain.Height - 1);

            //gMain.FillEllipse(new SolidBrush(ColorButtonMouseDown), SpotMD.GetSpotRect(Center));
            
            //gMain.FillEllipse(new SolidBrush(Color.FromArgb((int)(SpotMD.State*255), 19, 130, 206)), SpotMD.GetSpotRect(Center));
            //gMain.FillEllipse(new SolidBrush(Color.FromArgb(40, 40, 40)), SpotMU.GetSpotRect(Center));
            //gMain.FillEllipse(new SolidBrush(Color.FromArgb(255, 200, 200, 200)), SpotMU.GetSpotRect(Center));

            gMain.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            int sqrSize = 8;

            if (bmpIcon != null) gMain.DrawImage(bmpIcon, (this.Width - bmpIcon.Width) / 2+1, (this.Height - bmpIcon.Height) / 2);
            else gMain.FillRectangle(new SolidBrush(Color.FromArgb(190, 0,0,0)), Center.X - sqrSize / 2+1, Center.Y - sqrSize / 2+1, sqrSize, sqrSize);
            //gMain.FillRectangle(new SolidBrush(Color.FromArgb(235,255,255,255)), Center.X - sqrSize/2, Center.Y - sqrSize/2, sqrSize, sqrSize);
            //gMain.DrawRectangle(new Pen(Color.Black, 6), Center.X - 30, Center.Y - 30, 60, 60);
            gMain.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //gMain.DrawEllipse(new Pen(Color.White, 6), Center.X - 72, Center.Y - 72, 144, 144);
            
            this.BackgroundImage = bmpMain;
            this.Refresh();
        }

        private void tmrMain_Tick(object sender, EventArgs e) {
            SpotMD.NextState();
            SpotMU.NextState();
            if (SpotMU.State == 0 && SpotMD.State == 0) {
                tmrMain.Stop();
                //if (SpotMU.State == 1) {
                //    SpotMD.State = 0;
                //}
            }
            Redraw();
        }

        private void ucSpotButton_MouseDown(object sender, MouseEventArgs e) {
            SpotMD.FirstMouseLocation = e.Location;
            //SpotMD.State = 0;
            SpotMU.State = 0;
            SpotMD.Enabled = true;
            SpotMU.Enabled = false;
            tmrMain.Start();

            IsMouseDown = true;
        }
        private void ucSpotButton_MouseUp(object sender, MouseEventArgs e) {
            SpotMU.FirstMouseLocation = e.Location;
            SpotMU.State = 0;
            SpotMD.Enabled = false;
            //SpotMU.Enabled = true;
            IsMouseDown = false;

            tmrMain.Start();
            Redraw();
        }

        private void ucSpotButton_Load(object sender, EventArgs e)
        {

        }

        public void LoadIcon(string Path) {
            if (System.IO.File.Exists(Path)) SetIcon(new Bitmap(Path)); 
        }
        public void SetIcon(Bitmap newIcon) {
            if (bmpIcon != null) bmpIcon.Dispose();
            bmpIcon = newIcon;
            Redraw();
        }

        private void ucSpotButton_Resize(object sender, EventArgs e) {
            //this.BackgroundImage = bmpMain;
            bmpMain = new Bitmap(this.Width, this.Height);
            gMain = Graphics.FromImage(bmpMain);
            gMain.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            SpotMD.State = 0;
            this.BackgroundImage = bmpMain;
            Redraw();
        }

        private void ucSpotButton_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button != System.Windows.Forms.MouseButtons.None) SpotMD.FirstMouseLocation = e.Location;
        }
    }
}
