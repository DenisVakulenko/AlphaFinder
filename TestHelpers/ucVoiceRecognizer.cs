using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;

using CUETools.Codecs;
using CUETools.Codecs.FLAKE;

using GoogleSpeech;

namespace TestHelpers {
    public partial class ucVoiceRecognizer : UserControl {
        enum UIStates {Normal = 0, Listening, Requesting, Result};
        private string LastAnsver = "";

#region "UI"
        private clsWaitingIndicator WaitingIndicator = new clsWaitingIndicator(27,27);

        private Bitmap[] Tile;
        private Boolean TilesLoaded = false;

        private void LoadTiles() {
            TilesLoaded = true;
            Tile = new Bitmap[3];
            if (System.IO.File.Exists(Application.StartupPath + "\\frame_left.png") == true) Tile[0] = new Bitmap(Application.StartupPath + "\\frame_left.png"); else TilesLoaded = false;
            if (System.IO.File.Exists(Application.StartupPath + "\\frame_right.png") == true) Tile[1] = new Bitmap(Application.StartupPath + "\\frame_right.png"); else TilesLoaded = false;
            if (System.IO.File.Exists(Application.StartupPath + "\\mic1.png") == true) Tile[2] = new Bitmap(Application.StartupPath + "\\mic1.png"); else TilesLoaded = false;
        }
        void DrawFrame(Graphics graph) {
            graph.DrawLine(new Pen(Color.FromArgb(70, 70, 70)), 1, this.Height - 1, this.Width - 2, this.Height - 1);
            graph.DrawLine(new Pen(Color.FromArgb(70, 70, 70)), 1, 0, this.Width - 2, 0);
            if (TilesLoaded) {
                graph.DrawImageUnscaled(Tile[0], 0, 0);
                graph.DrawImageUnscaled(Tile[1], this.Width - 2, 0);
            }
        }

        private UIStates UIState = UIStates.Normal;
        private double UITransition = 0.99;

        private void RedrawUI() {
            if (UITransition <= 1 && UIState != UIStates.Listening) {
                SolidBrush TextBrush;
                using (Graphics gr = Graphics.FromImage(bmp)) {
                    gr.DrawRectangle(new Pen(Color.FromArgb(245, 245, 245)), 0, 0, bmp.Width - 1, bmp.Height - 1);
                    gr.FillRectangle(new SolidBrush(Color.FromArgb((int)((UITransition) * 255), 245, 245, 245)), 1, 1, bmp.Width - 2, bmp.Height - 2);
                    if (UIState == UIStates.Normal) {
                        WaitingIndicator.Enabled = false;

                        if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, 150, 150, 150));
                        else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition-0.2)/0.8) * 255), 150, 150, 150));
                        
                        var att = new System.Drawing.Imaging.ImageAttributes();
                        var cm = new System.Drawing.Imaging.ColorMatrix(new Single[][] 
                           {new Single[] {1, 0, 0, 0, 0}, 
                            new Single[] {0, 1, 0, 0, 0},
                            new Single[] {0, 0, 1, 0, 0}, 
                            new Single[] {0, 0, 0, (Single)UITransition, 0}, 
                            new Single[] {0, 0, 0, 0, 1}});
                        att.SetColorMatrix(cm);
                        
                        if (TilesLoaded) {
                            if (this.Width > 110) {
                                gr.DrawString("Click and speak", new Font("arial", 8), TextBrush, 6, 9);
                                gr.DrawImage(Tile[2], new Rectangle(this.Width - 22, 2,27,27), 0, 0, 27,27, GraphicsUnit.Pixel, att);
                            }
                            else if (this.Width > 84) {
                                gr.DrawString("Click and..", new Font("arial", 8), TextBrush, 6, 9);
                                gr.DrawImage(Tile[2], new Rectangle(this.Width - 22, 2,27,27), 0, 0, 27,27, GraphicsUnit.Pixel, att);
                            }
                            else if (this.Width > 60) {
                                gr.DrawString("Click..", new Font("arial", 8), TextBrush, 6, 9);
                                gr.DrawImage(Tile[2], new Rectangle(this.Width - 22, 2,27,27), 0, 0, 27,27, GraphicsUnit.Pixel, att);
                            }
                            else {
                                gr.DrawImage(Tile[2], new Rectangle((this.Width - Tile[2].Width) / 2, 2, 27, 27), 0, 0, 27, 27, GraphicsUnit.Pixel, att);
                            }
                        }
                    }
                    else if (UIState == UIStates.Listening) {
                        WaitingIndicator.Enabled = false;
                    }
                    else if (UIState == UIStates.Requesting) {
                        WaitingIndicator.Enabled = true;
                        if (bmp.Width > 90) {
                            if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, 50, 150, 150));
                            else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), 50, 50, 50));
                            gr.DrawString("requesting", new Font("arial", 8), TextBrush, 6, 9);
                        }
                    }
                    else {
                        WaitingIndicator.Enabled = false;

                        if (LastAnsver != "") {
                            if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, 150, 150, 150));
                            else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), 0, 0, 0));
                            gr.DrawString(LastAnsver, new Font("arial", 8), TextBrush, 6, 9);
                            gr.DrawLine(new Pen(Color.FromArgb(245, 245, 245)), this.Width - 2, 1, this.Width - 2, this.Height - 2);
                            gr.DrawLine(new Pen(Color.FromArgb(150, 245, 245, 245)), this.Width - 3, 1, this.Width - 3, this.Height - 2);
                        }
                        else {
                            if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, 150, 150, 150));
                            else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), 100, 50, 50));
                            gr.DrawString("none", new Font("arial", 8), TextBrush, 6, 9);
                        }
                        if (UITransition == 1) { UITransition = 0; UIState = UIStates.Normal; }
                    }
                    if (WaitingIndicator.Fade > 0 || WaitingIndicator.Enabled == true) {
                        //if (WaitingIndicator.Enabled == false) gr.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), bmp.Width - 28, 1, 27, 27);
                        if (UITransition > 0.3 && UIState == UIStates.Requesting || UIState != UIStates.Requesting) {
                            if (bmp.Width > 60) {
                                gr.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), bmp.Width - 28, 1, 27, 27);
                                gr.DrawImageUnscaled(WaitingIndicator.GetNextFrame(), bmp.Width - 29, 1);
                            }
                            else {
                                gr.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), (this.Width - 27) / 2, 1, 27, 27);
                                gr.DrawImageUnscaled(WaitingIndicator.GetNextFrame(), (this.Width - 27) / 2, 1);
                            }
                        }
                    }
                    DrawFrame(gr);
                }
                this.BackgroundImage = bmp;
                this.Refresh();
                UITransition += 0.02;
            }
            else { UITransition = 1; }
        }

        private void tmrUI_Tick(object sender, EventArgs e) {
            RedrawUI();
        }

        private void ucVoiceRecognizer_MouseDown(object sender, MouseEventArgs e) {
            tmrUI.Enabled = true;
            UITransition = 0;
            UIState = UIStates.Listening;
            if (Recording == false) {
                try {
                    timeRecordingStarted = DateTime.Now;
                    timeSilenceStarted = DateTime.Now;

                    waveIn = new WaveIn();
                    //Дефолтное устройство для записи (если оно имеется)
                    waveIn.DeviceNumber = 0;
                    //Прикрепляем к событию DataAvailable обработчик, возникающий при наличии записываемых данных
                    waveIn.DataAvailable += waveIn_DataAvailable;
                    //Прикрепляем обработчик завершения записи
                    waveIn.RecordingStopped += new EventHandler(waveIn_RecordingStopped);
                    //Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)
                    waveIn.WaveFormat = new WaveFormat(16000, 1);
                    //Инициализируем объект WaveFileWriter
                    writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                    //Начало записи

                    waveIn.StartRecording();
                    Recording = true;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void ucVoiceRecognizer_MouseUp(object sender, MouseEventArgs e) {
            if ((DateTime.Now - timeRecordingStarted).TotalSeconds > 0.5) {
                if (waveIn != null) {
                    StopRecording();
                }
                SendFileRun(Settings.Instance.wavName);
            }
        }

        private void ucVoiceRecognizer_Resize(object sender, EventArgs e) {
            bmp = new Bitmap(this.Width, this.Height);
            RedrawUI();
            this.BackgroundImage = bmp;
        }
#endregion

        public ucVoiceRecognizer() {
            InitializeComponent();
            Settings.Instance.wavName = outputFilename;
            LoadTiles();
        }


        private delegate void DoWorkDelegate(String filePath);
        private void SendFileRun(String filePath) {
            // Disable the button
            DisableStartButtons();
            // Create delegate and make async call
            DoWorkDelegate worker = new DoWorkDelegate(SendFile);
            worker.BeginInvoke(filePath, new AsyncCallback(DoWorkComplete), worker);
        }

        private void EnableStartButtons() {
            if (InvokeRequired) {
                Invoke(new MethodInvoker(EnableStartButtons));
            }
            else {
                UITransition = 0;
                UIState = UIStates.Result;
            }
        }

        private void DisableStartButtons() {
            UITransition = 0;
            UIState = UIStates.Requesting;
        }


        private void DoWorkComplete(IAsyncResult workID) {
            EnableStartButtons();
            DoWorkDelegate worker = workID.AsyncState as DoWorkDelegate;
            worker.EndInvoke(workID);
        }


        private delegate void ReportOnProgressDelegate(int progress, string msg);

        private void ReportOnProgress(int progress, string msg) {
            if (InvokeRequired) {
                Invoke(new ReportOnProgressDelegate(ReportOnProgress), new object[] { progress, msg });
                return;
            }
        }

        private void SendFile(String filePath) {
            try {
                ReportOnProgress(10, "Идет запрос");
                String responseFromServer = GoogleVoice.GoogleSpeechRequest(filePath, Settings.Instance.tmpName);

                JSon.RecognitionResult result = JSon.Parse(responseFromServer);
                if (result.hypotheses.Length > 0) {
                    JSon.RecognizedItem item = result.hypotheses.First();

                    string s = "";
                    int l = result.hypotheses.Length;
                    foreach (JSon.RecognizedItem hypot in result.hypotheses) {
                        if (s != "") s += "/";
                        s += hypot.utterance;
                    }
                    AddToList(s);
                }
                else {
                    AddToList(String.Format(""));
                }

                ReportOnProgress(100, "Запрос успешно выполнен");
            }
            catch (Exception e) {
                ReportOnProgress(100, "Ошибка запроса: " + e.Message);
            }
        }

        public delegate void SmthRecognizedHandler(string[] text);
        public event SmthRecognizedHandler SmthRecognized;

        private delegate void AddDelegate(String log);
        private void AddToList(String item) {
            if (InvokeRequired) {
                Invoke(new AddDelegate(AddToList), new object[] { item });
                return;
            }
            else {
                LastAnsver = item;
                if (item != "") SmthRecognized(item.Split('/'));
            }
        }




        DateTime timeRecordingStarted;
        DateTime timeSilenceStarted;

        // WaveIn - поток для записи
        WaveIn waveIn;
        //Класс для записи в файл
        WaveFileWriter writer;
        //Имя файла для записи
        string outputFilename = "record1.wav";
        //Получение данных из входного буфера и обработка полученных с микрофона данных
        Bitmap bmp;
        void waveIn_DataAvailable(object sender, WaveInEventArgs e) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else {
                //Записываем данные из буфера в файл
                if (Recording) {
                    writer.WriteData(e.Buffer, 0, e.BytesRecorded);

                    double max = 0;
                    double Sum2 = 0;
                    using (Graphics gr = Graphics.FromImage(bmp)) {
                        gr.DrawRectangle(new Pen(Color.FromArgb(245, 245, 245)), 0,0, bmp.Width - 1, bmp.Height - 1);
                        gr.FillRectangle(new SolidBrush(Color.FromArgb((int)(UITransition*100), 245, 245, 245)), 1, 1, bmp.Width - 2, bmp.Height - 2);
                        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        int Count = e.BytesRecorded / 2;
                        for (int index = 0; index < e.BytesRecorded; index += 2) {
                            double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                            Tmp /= 32768.0;
                            Sum2 += Tmp * Tmp;
                            if (max < Math.Abs(Tmp)) max = Math.Abs(Tmp);
                        }
                        Sum2 /= Count;

                        int PrevY = (bmp.Height - 1) / 2;
                        int PrevX = 0;
                        Color MainWaveColor; 
                        if (Sum2 < 0.1 && max < 0.4) MainWaveColor = Color.FromArgb(70, 19, 130, 206); else MainWaveColor = Color.FromArgb(230, 19, 130, 206);
                        if (max > 0.0001) {
                            for (int index = 0; index < e.BytesRecorded; index += 8) {
                                double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]) / 32768.0;
                                int x = (int)(index * (bmp.Width - 1) / e.BytesRecorded);
                                int y = (int)(Tmp * ((bmp.Height - 1) / 2) / (max) + ((bmp.Height - 1) / 2));
                                bmp.SetPixel(x, y, Color.FromArgb(220, 220, 220));

                                y = (int)(Tmp * ((bmp.Height - 1) / 2) + ((bmp.Height - 1) / 2));
                                gr.DrawLine(new Pen(MainWaveColor), PrevX, PrevY, x, y);
                                PrevX = x; PrevY = y;
                            }
                        }
                        if (Sum2 > 0.1 || max > 0.4) timeSilenceStarted = DateTime.Now;
                        if ((DateTime.Now - timeSilenceStarted).TotalSeconds > 1.2 || (DateTime.Now - timeRecordingStarted).TotalSeconds > 12) {
                            if (waveIn != null) {
                                StopRecording();
                            }

                            SendFileRun(Settings.Instance.wavName);
                        }

                        DrawFrame(gr);
                        this.BackgroundImage = bmp;
                        this.Refresh();
                    }
                }
            }
        }

        Boolean Recording = false;
        //Завершаем запись
        void StopRecording() {
            waveIn.StopRecording();
            Recording = false;
        }
        //Окончание записи
        private void waveIn_RecordingStopped(object sender, EventArgs e) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }


        private void ucVoiceRecognizer_Load(object sender, EventArgs e) {

        }

        private void ucVoiceRecognizer_Load_1(object sender, EventArgs e) {
            //bmp = new Bitmap(100,10);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            //if (checkBox1.Checked) {

            //}
        }

    }
}
