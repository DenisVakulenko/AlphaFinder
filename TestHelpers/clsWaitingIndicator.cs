using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestHelpers {
    class clsWaitingIndicator {
        public class Ring {
            public float Angle;
            public float AngleDelta;
            public float Lenght;
            public float ColorState;
            public float ColorStateDelta;
            public Color BaseColor = Color.FromArgb(19, 130, 206);
            public Rectangle Rect;

            public Ring(float NewAngleDelta, Rectangle NewRect, float NewColorStateDelta = 0.01F, float NewColorState = 0) {
                Angle = 0;
                AngleDelta = NewAngleDelta;
                Lenght = 240;
                ColorState = NewColorStateDelta;
                ColorStateDelta = NewColorState;
                Rect = NewRect;
            }
            public Ring(float NewAngleDelta, Rectangle NewRect, Random R) {
                Angle = (float)(R.NextDouble() * 360);
                AngleDelta = NewAngleDelta;
                Lenght = 200 + (float)(R.NextDouble() * 80);
                ColorState = (float)(R.NextDouble() * Math.PI * 2);
                ColorStateDelta = (float)((R.NextDouble() / 2 + 0.5) / 100);
                Rect = NewRect;
            }
            public void CountNextState(double Fade) {
                Angle += (float)(AngleDelta * Fade);
                if (Angle > 360) Angle -= 360;
                if (Angle < 0) Angle += 360;

                ColorState += ColorStateDelta;
                if (ColorState > Math.PI) ColorState -= (float)Math.PI;
                if (ColorState < 0) ColorState += (float)Math.PI;
            }
            public Color GetColor(double Fade) {
                double k = (Math.Sin(ColorState) + 1) / 2;
                return Color.FromArgb((int)(Fade * 255), (int)(BaseColor.R * k), (int)(BaseColor.G * k), (int)(BaseColor.B * k));
            }
        }

        public Boolean Enabled = false;

        private Bitmap bmpMain;
        private Graphics gMain;
        private Rectangle rectMain;

        public Ring[] Rings;
        public Pen penMain = new Pen(Color.FromArgb(19, 130, 206), 3); //= new Pen(Color.FromArgb(66,66,66), 3);
        public double Fade = 0;

        public Rectangle GetRect(int R) {
            return new Rectangle((int)bmpMain.Width / 2 - R, (int)bmpMain.Height / 2 - R, R * 2, R * 2);
        }
        public void InitRings(float[] Ad, int[] Rs) {
            Random R = new Random((int)DateTime.Now.Ticks);
            Rings = new Ring[Rs.Length];
            for (int i = 0; i < Rings.Length; i++) {
                Rings[i] = new Ring(Ad[i], GetRect(Rs[i]), R);
            }
        }
        public void InitRings(int[] Rs) {
            Random R = new Random((int)DateTime.Now.Ticks);
            Rings = new Ring[Rs.Length];
            for (int i = 0; i < Rings.Length; i++) {
                Rings[i] = new Ring((float)(R.NextDouble() * 2 - 1), GetRect(Rs[i]), R);
            }
        }
        public void InitRings(int N) {
            Random R = new Random((int)DateTime.Now.Millisecond);
            Rings = new Ring[N];
            for (int i = 0; i < N; i++) {
                Rings[i] = new Ring((float)(R.NextDouble() * 4 - 2), GetRect((int)((i + 2) * (penMain.Width + 1))), R);
            }
        }



        public clsWaitingIndicator(int Width, int Height) {
            bmpMain = new Bitmap(Width, Height);
            gMain = Graphics.FromImage(bmpMain);
            gMain.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            rectMain = new Rectangle(0 + (int)(penMain.Width / 2), 0 + (int)(penMain.Width / 2), bmpMain.Width - (int)penMain.Width, bmpMain.Height - (int)penMain.Width);
            penMain.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            penMain.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            if (Height < 30) {
                Random R = new Random((int)DateTime.Now.Ticks);
                Rings = new Ring[3];
                Rings[0] = new Ring(2.9F, GetRect(3), R);
                Rings[1] = new Ring(-1.5F, GetRect(6), R);
                Rings[2] = new Ring(1.9F, GetRect(9), R);
                penMain.Width = 2;
            }
            else {
                Random R = new Random((int)DateTime.Now.Ticks);
                Rings = new Ring[3];
                Rings[0] = new Ring(3.8F, GetRect(3), R);
                Rings[1] = new Ring(-1.3F, GetRect(7), R);
                Rings[2] = new Ring(1.7F, GetRect(11), R);
            }
        }

        public Bitmap GetNextFrame() {
            if (Enabled && Fade < 1) Fade += 0.015;
            if (!Enabled && Fade > 0) Fade -= 0.025;
            if (Fade < 0) Fade = 0;
            if (Fade > 1) Fade = 1;


            if (Fade > 0) {
                gMain.Clear(Color.FromArgb(0, 0, 0, 0));
                if (Rings != null) {
                    for (int i = 0; i < Rings.Length; i++) {
                        //if (Enabled) Rings[i].CountNextState(Math.Sin(Fade*Math.PI/2));
                        if (Enabled) Rings[i].CountNextState((2 - Math.Sin(Fade * Math.PI/2))*4-3);
                        else Rings[i].CountNextState(Fade);
                        penMain.Color = Rings[i].GetColor(Fade);
                        gMain.DrawArc(penMain, Rings[i].Rect, Rings[i].Angle, Rings[i].Lenght);
                    }
                }
            }
            else {
                gMain.Clear(Color.FromArgb(0, 0, 0, 0));
            }
            return bmpMain;
        }
    }
}
