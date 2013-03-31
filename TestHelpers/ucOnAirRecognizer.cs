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

using System.Speech;
using System.Speech.Recognition;
using System.Speech.AudioFormat;

namespace TestHelpers {
    public partial class ucOnAirRecognizer : UserControl {
        SpeechRecognitionEngine sre = new SpeechRecognitionEngine();

        public delegate void SmthRecognizedHandler(string[] text);
        public event SmthRecognizedHandler SmthRecognized;
        public string History = "";

        const string DirectoryForRecords = "C:\\Rec";

        enum UIStates {Normal = 0, Listening, Requesting, Result, RequestError, NoiseError};
        
        private Random rnd = new Random((int)DateTime.Now.Ticks);
        
        private int RequestsFailedCounter = 0;
        private const int MaxRequestFailings = 1;
        
        private int thisID = -1;
        
        private void ucOnAirRecognizer_Load(object sender, EventArgs e) {
            if (!System.IO.Directory.Exists(DirectoryForRecords)) {
                System.IO.DirectoryInfo Inf = System.IO.Directory.CreateDirectory(DirectoryForRecords);
            }
        }
        public ucOnAirRecognizer() {
            InitializeComponent();
            Settings.Instance.wavName = outputFilename;
            SilenceAmplitudes = new double[50];
            SilenceAmplitude = 0.3;
            for (int i = 0; i < SilenceAmplitudes.Length; SilenceAmplitudes[i] = SilenceAmplitude/2, i++) ;
            LoadTiles();

            sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
            sre.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(sre_RecognizeCompleted);
            sre.AudioSignalProblemOccurred += new EventHandler<AudioSignalProblemOccurredEventArgs>(sre_AudioSignalProblemOccurred);
            sre.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(sre_SpeechDetected);
            sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);

            string[] words = { "close", "next page", "previous page", "kate", "play music", "find", "stop music", "music", "back", "play", "open", "home" };
            Choices choices = new Choices(words);
            GrammarBuilder gb = new GrammarBuilder(choices);
            Grammar grammar = new Grammar(gb);
            sre.LoadGrammar(grammar);

            //DictationGrammar dg = new DictationGrammar();
            //sre.LoadGrammar(dg);

            sre.SetInputToDefaultAudioDevice();
            sre.EndSilenceTimeout = new TimeSpan(0, 0, 0, 1, 0);            
        }
        ~ucOnAirRecognizer() {
            StopRecording();
        }


        public void sre_newGrammar(string[] G) {
            string[] words = { "close", "next page", "previous page", "kate", "play music", "find", "stop music", "music", "back", "play", "open", "home" };
            
            Choices choices = new Choices(words); choices.Add(G);
            GrammarBuilder gb = new GrammarBuilder(choices); 
            Grammar grammar = new Grammar(gb);
            sre.LoadGrammar(grammar);
        }

        String sre_text = "";
        void sre_AudioSignalProblemOccurred(object sender, AudioSignalProblemOccurredEventArgs e) {
            //sre_text = e.AudioSignalProblem.ToString() + sre_text;
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e) {
            //sre_text = e.Result.Text + sre_text;
        }

        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e) {
            //sre_text = "Rejected!" + sre_text;
        }

        void sre_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e) {
            //sre_text = "Rec Complete!" + sre_text;
            sre.RecognizeAsync();
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
            sre_text = e.Result.Confidence.ToString()  + " " + e.Result.Text + sre_text;
            string[] ans = { e.Result.Text };
            SmthRecognized(ans);
            RedrawUI();
            this.Refresh();
        }

        void sre_SpeechDetected(object sender, SpeechDetectedEventArgs e) {
            //sre_text = "Speech Detected!" + sre_text;
        }





#region "UI"
        private clsWaitingIndicator WaitingIndicator = new clsWaitingIndicator(27,27);

        private Bitmap[] Tile;
        private Boolean TilesLoaded = false;

        private UIStates UIState = UIStates.Normal;
        private double UITransition = 0.99;

        private void LoadTiles() {
            TilesLoaded = true;
            Tile = new Bitmap[3];
            if (System.IO.File.Exists(Application.StartupPath + "\\frame_left.png") == true) Tile[0] = new Bitmap(Application.StartupPath + "\\frame_left.png"); else TilesLoaded = false;
            if (System.IO.File.Exists(Application.StartupPath + "\\frame_right.png") == true) Tile[1] = new Bitmap(Application.StartupPath + "\\frame_right.png"); else TilesLoaded = false;
            if (System.IO.File.Exists(Application.StartupPath + "\\mic1.png") == true) Tile[2] = new Bitmap(Application.StartupPath + "\\mic1.png"); else TilesLoaded = false;
        }
        private void DrawFrame(Graphics graph) {
            if (TilesLoaded) {
                if (this.Height == Tile[0].Height) {
                    graph.DrawLine(new Pen(Color.FromArgb(70, 70, 70)), 1, this.Height - 1, this.Width - 2, this.Height - 1);
                    graph.DrawLine(new Pen(Color.FromArgb(70, 70, 70)), 1, 0, this.Width - 2, 0);
                    graph.DrawImageUnscaled(Tile[0], 0, 0);
                    graph.DrawImageUnscaled(Tile[1], this.Width - 2, 0);
                }
                else {
                    graph.DrawRectangle(new Pen(Color.FromArgb(50,50,50)), 0, 0, this.Width - 1, this.Height - 1);
                }
            }
            else {
                graph.DrawRectangle(new Pen(Color.FromArgb(70, 70, 70)), 0, 0, this.Width - 1, this.Height - 1);
            }
            //Printing  failings
            graph.DrawString(SilenceAmplitude.ToString("0.00"), new Font("arial", 6), Brushes.Gray, 2, 2);
            graph.DrawString(sre_text, new Font("arial", 6), Brushes.Gray, 2, this.Height - 12);
        }
        private void RedrawUI() {
            if (UITransition <= 1 && UIState != UIStates.Listening) {
                SolidBrush TextBrush;
                using (Graphics gr = Graphics.FromImage(bmp)) {
                    gr.DrawRectangle(new Pen(Color.FromArgb(245, 245, 245)), 0, 0, bmp.Width - 1, bmp.Height - 1);
                    gr.FillRectangle(new SolidBrush(Color.FromArgb((int)((UITransition) * 255), 245, 245, 245)), 1, 1, bmp.Width - 2, bmp.Height - 2);
                    if (UIState == UIStates.Normal) {
                        WaitingIndicator.Enabled = false;
                        
                        var att = new System.Drawing.Imaging.ImageAttributes();
                        var cm = new System.Drawing.Imaging.ColorMatrix(new Single[][] 
                           {new Single[] {1, 0, 0, 0, 0}, 
                            new Single[] {0, 1, 0, 0, 0},
                            new Single[] {0, 0, 1, 0, 0}, 
                            new Single[] {0, 0, 0, (Single)UITransition, 0}, 
                            new Single[] {0, 0, 0, 0, 1}});
                        att.SetColorMatrix(cm);

                        Color TextColor = Color.FromArgb(150, 150, 150);

                        if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, TextColor));
                        else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), TextColor));

                        if (TilesLoaded) {
                            if (this.Width > 110) {
                                gr.DrawString("Click and speak", new Font("arial", 8), TextBrush, 6, 9);
                                gr.DrawImage(Tile[2], new Rectangle(this.Width - 22, 2, 27, 27), 0, 0, 27, 27, GraphicsUnit.Pixel, att);
                            }
                            else if (this.Width > 84) {
                                gr.DrawString("Click and..", new Font("arial", 8), TextBrush, 6, 9);
                                gr.DrawImage(Tile[2], new Rectangle(this.Width - 22, 2, 27, 27), 0, 0, 27, 27, GraphicsUnit.Pixel, att);
                            }
                            else if (this.Width > 60) {
                                gr.DrawString("Click..", new Font("arial", 8), TextBrush, 6, 9);
                                gr.DrawImage(Tile[2], new Rectangle(this.Width - 22, 2, 27, 27), 0, 0, 27, 27, GraphicsUnit.Pixel, att);
                            }
                            else {
                                gr.DrawImage(Tile[2], new Rectangle((this.Width - Tile[2].Width) / 2, 2, 27, 27), 0, 0, 27, 27, GraphicsUnit.Pixel, att);
                            }
                        }
                    }
                    else if (UIState == UIStates.Listening) {
                        //WaitingIndicator.Enabled = false;
                    }
                    else if (UIState == UIStates.Requesting) {
                        WaitingIndicator.Enabled = true;
                        if (bmp.Width > 90) {
                            if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, 50, 150, 150));
                            else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), 50, 50, 50));
                            gr.DrawString("requesting", new Font("arial", 8), TextBrush, 6, 9);
                        }
                    }
                    else if (UIState == UIStates.RequestError) {
                        WaitingIndicator.Enabled = false;

                        Color TextColor = Color.FromArgb(170, 0, 0);

                        if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, TextColor));
                        else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), TextColor));
                        
                        gr.DrawString("Request error..", new Font("arial", 8), TextBrush, 6, 9);
                        
                        if (UITransition == 1) { UITransition = 0; UIState = UIStates.Normal; }
                        if (UITransition > 0.9) UITransition -= 0.014;
                    }
                    else if (UIState == UIStates.NoiseError) {
                        WaitingIndicator.Enabled = false;

                        Color TextColor = Color.FromArgb(170, 0, 0);

                        if (UITransition < 0.2) TextBrush = new SolidBrush(Color.FromArgb(0, TextColor));
                        else TextBrush = new SolidBrush(Color.FromArgb((int)(((UITransition - 0.2) / 0.8) * 255), TextColor));

                        gr.DrawString("Noise error..", new Font("arial", 8), TextBrush, 6, 9);

                        if (UITransition == 1) { UITransition = 0; UIState = UIStates.Normal; }
                        if (UITransition > 0.9) UITransition -= 0.014;
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
                UITransition += 0.015;
            }
            else { UITransition = 1; }
        }

        private void tmrUI_Tick(object sender, EventArgs e) {
            RedrawUI();
        }

        private void ucOnAirRecognizer_Resize(object sender, EventArgs e) {
            bmp = new Bitmap(this.Width, this.Height);
            UITransition = 0.99;
            RedrawUI();
            this.BackgroundImage = bmp;
            this.Refresh();
        }
        private void ucOnAirRecognizer_DoubleClick(object sender, EventArgs e) {
            MessageBox.Show(History.Substring(0, Math.Min(400, History.Length)));
        }

        private DateTime MouseDownTime;
        private void ucOnAirRecognizer_MouseDown(object sender, MouseEventArgs e) {
            MouseDownTime = DateTime.Now;
            if (Recording == false) {
                StartListening();
            }
            else {
                tmrUI.Enabled = true;
                UITransition = 0;
                UIState = UIStates.Normal;
                StopRecording();
                SendFileRun(Settings.Instance.wavName);
            }
        }
        private void ucOnAirRecognizer_MouseUp(object sender, MouseEventArgs e) {
            if ((DateTime.Now - MouseDownTime).TotalSeconds > 0.5 && Recording == true) {
                tmrUI.Enabled = true;
                UITransition = 0;
                UIState = UIStates.Normal;
                StopRecording();
                SendFileRun(Settings.Instance.wavName);
            }
        }
#endregion
 
        private delegate void DoWorkDelegate(String filePath);
        private void SendFileRun(String filePath) {
            DoWorkDelegate worker = new DoWorkDelegate(SendFile);
            worker.BeginInvoke(filePath, new AsyncCallback(SendFileCompleted), worker);
        }
        private void SendFileCompleted(IAsyncResult workID) {
            DoWorkDelegate worker = workID.AsyncState as DoWorkDelegate;
            worker.EndInvoke(workID);
            worker = null;
        }

        private delegate void ReportAboutErrorDelegate(string Path);
        private void ReportAboutError(string Path) {
            if (InvokeRequired) {
                Invoke(new ReportAboutErrorDelegate(ReportAboutError), new object[] { Path });
                return;
            }
            else {
                RequestsFailedCounter++;
                if (RequestsFailedCounter > MaxRequestFailings) {
                    tmrUI.Enabled = true;
                    UITransition = 0;
                    UIState = UIStates.RequestError;
                    StopRecording();
                }
                DeleteTempRecords(Path);
            }
        }

        private void SendFile(String filePath) {
            try {
                string s = "";
                int sampleRate = SoundTools.Wav2Flac(filePath, filePath + ".flac");
                String responseFromServer;
                
                responseFromServer = GoogleVoice.GoogleSpeechRequest(filePath + ".flac", sampleRate);
                JSon.RecognitionResult resultEng = JSon.Parse(responseFromServer);
                if (resultEng.hypotheses.Length > 0) {
                    JSon.RecognizedItem item = resultEng.hypotheses.First();

                    int l = resultEng.hypotheses.Length;
                    foreach (JSon.RecognizedItem hypot in resultEng.hypotheses) {
                        if (s != "") s += "/";
                        s += hypot.utterance;
                    }
                }

                responseFromServer = GoogleVoice.GoogleSpeechRequest(filePath + ".flac", sampleRate, "lang=ru-RU&maxresults=10");
                JSon.RecognitionResult resultRus = JSon.Parse(responseFromServer);
                if (resultRus.hypotheses.Length > 0) {
                    JSon.RecognizedItem item = resultRus.hypotheses.First();

                    if (resultRus.hypotheses.Length > 0 && resultEng.hypotheses.Length > 0)
                        if (resultRus.hypotheses[0].confidence > resultEng.hypotheses[0].confidence) {
                            foreach (JSon.RecognizedItem hypot in resultRus.hypotheses) {
                                if (s != "") s = "/"+s;
                                s = hypot.utterance+s;
                            }
                        }
                        else {
                            foreach (JSon.RecognizedItem hypot in resultRus.hypotheses) {
                                if (s != "") s += "/";
                                s += hypot.utterance;
                            }
                        }
                }

                SendFileAnsver(s);

                while (!DeleteTempRecords(filePath)) {
                    System.Threading.Thread.Sleep(500);
                };
            }
            catch {
                ReportAboutError(filePath);
            }
        }
        private Boolean DeleteTempRecords(string filePath) {
            try {
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                if (System.IO.File.Exists(filePath + ".flac")) System.IO.File.Delete(filePath + ".flac");
                return true;
            }
            catch {
                return false;
            }
        }

        //(!IsAccess(filePath + ".flac"))
        private Boolean IsAccess(string filapath) {
            var perm = new System.Security.Permissions.FileIOPermission(
                System.Security.Permissions.FileIOPermissionAccess.Write |
                System.Security.Permissions.FileIOPermissionAccess.Read,
                filapath);
            try {
                perm.Demand();
                return true;
            }
            catch {
                return false;
            }
        }

        private delegate void AddDelegate(String item);
        private void SendFileAnsver(String item) {
            if (InvokeRequired) {
                Invoke(new AddDelegate(SendFileAnsver), new object[] { item });
                return;
            }
            else {
                History = item + (char)(13) + (char)(10) + (char)(13) + (char)(10) + History;
                if (item != "") SmthRecognized(item.Split('/'));
                RequestsFailedCounter = 0;
            }
        }

        DateTime timeRecordingStarted;
        DateTime timeSilenceStarted;

        WaveIn waveIn;
        WaveFileWriter writer;
        
        string outputFilename = "record1.wav";
        
        Bitmap bmp = new Bitmap(10, 10);

        Boolean WaitingForSound = true;
        private byte[] PBuffer;
        private byte[] PPBuffer;
        private byte[] PPPBuffer;
        private byte[] PPPPBuffer;

        private double SilenceAmplitude = 0.2F;
        private double[] SilenceAmplitudes;
        private int SilenceAmplitudesPointer = 0;

        void waveIn_DataAvailable(object sender, WaveInEventArgs e) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else {
                //Записываем данные из буфера в файл
                if (Recording) {
                    double max = -10000000;
                    double min = 10000000;
                    double Sum2 = 0;

                    int Count = e.BytesRecorded / 2;
                    for (int index = 0; index < e.BytesRecorded; index += 2) {
                        double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                        Tmp /= 32768.0;
                        Sum2 += Tmp * Tmp;
                        if (max < Tmp) max = Tmp;
                        if (min > Tmp) min = Tmp;
                    }
                    Sum2 /= Count;

                    if (Sum2 > 0.01 * SilenceAmplitude && (max - min) > SilenceAmplitude) {
                        if (WaitingForSound == true) {
                            WaitingForSound = false;
                            MakeNewFileForRecording();

                            if (PPPPBuffer != null) writer.Write(PPPPBuffer, 0, e.BytesRecorded);
                            if (PPPBuffer != null) writer.Write(PPPBuffer, 0, e.BytesRecorded);
                            if (PPBuffer != null) writer.Write(PPBuffer, 0, e.BytesRecorded);
                            if (PBuffer != null) writer.Write(PBuffer, 0, e.BytesRecorded);
                        }
                        timeSilenceStarted = DateTime.Now;
                    }

                    if ((max - min) < SilenceAmplitude * 2) {
                        SilenceAmplitudes[SilenceAmplitudesPointer] = (max - min);
                        SilenceAmplitudesPointer = (SilenceAmplitudesPointer + 1) % SilenceAmplitudes.Length;
                        //if (SilenceAmplitude > SilenceAmplitudes.Average()) 
                        SilenceAmplitude = SilenceAmplitudes.Average()*2;
                        SilenceAmplitude = Math.Min(SilenceAmplitude, 1.5);
                    }

                    if (WaitingForSound == false) {
                        writer.Write(e.Buffer, 0, e.BytesRecorded);
                    }
                    else {
                        if (PPPBuffer != null) PPPPBuffer = (byte[])PPPBuffer.Clone();
                        if (PPBuffer != null) PPPBuffer = (byte[])PPBuffer.Clone();
                        if (PBuffer != null) PPBuffer = (byte[])PBuffer.Clone();
                        PBuffer = (byte[])e.Buffer.Clone();
                    }
                    if (WaitingForSound != true) {
                        if ((DateTime.Now - timeSilenceStarted).TotalSeconds > 0.5) {
                            CloseAndAnaliseRecordered();
                            WaitingForSound = true;
                        }
                        if ((DateTime.Now - timeRecordingStarted).TotalSeconds > 50) {
                            CloseWriter();
                            System.IO.File.Delete(outputFilename);

                            StopRecording();
                            tmrUI.Enabled = true;
                            UITransition = 0;
                            UIState = UIStates.NoiseError;
                        }
                    }


                    using (Graphics gr = Graphics.FromImage(bmp)) {
                        gr.DrawRectangle(new Pen(Color.FromArgb(245, 245, 245)), 0, 0, bmp.Width - 1, bmp.Height - 1);
                        gr.FillRectangle(new SolidBrush(Color.FromArgb((int)(UITransition * 200), 245, 245, 245)), 1, 1, bmp.Width - 2, bmp.Height - 2);
                        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        int PrevY = (bmp.Height - 1) / 2;
                        int PrevX = 0;
                        Color MainWaveColor;
                        if (WaitingForSound == true) MainWaveColor = Color.FromArgb(50, 150, 150, 150);
                        else MainWaveColor = Color.FromArgb(240, 19, 130, 206);

                        if (max > 0.0001) {
                            for (int index = 0; index < e.BytesRecorded; index += 8) {
                                double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]) / 32768.0;
                                int x = (int)(index * (bmp.Width - 1) / e.BytesRecorded);
                                int y = (int)(Tmp * ((bmp.Height - 1) / 2) / (max) + ((bmp.Height - 1) / 2));

                                y = (int)(Tmp * ((bmp.Height - 1) / 2) + ((bmp.Height - 1) / 2));
                                gr.DrawLine(new Pen(MainWaveColor), PrevX, PrevY, x, y);
                                PrevX = x; PrevY = y;
                            }
                        }
                        //gr.DrawString(min.ToString(), new Font("arial", 8), Brushes.Black, 5, 15);

                        DrawFrame(gr);
                        this.BackgroundImage = bmp;
                        this.Refresh();
                    }
                }
            }
        }

        Boolean Recording = false;
        //Завершаем запись
        public void StopRecording() {
            if (waveIn != null) waveIn.StopRecording();
            Recording = false;
        }

        void CloseAndAnaliseRecordered() {
            if (writer != null) {
                writer.Close();
                writer = null;
                SendFileRun(outputFilename);
            }
        }
        void CloseWriter() {
            if (writer != null) {
                writer.Close();
                writer = null;
            }
        }
        public void MakeNewFileForRecording() {
            outputFilename = DirectoryForRecords + "\\" + thisID.ToString() + ") " + DateTime.Now.Ticks.ToString() + rnd.Next().ToString() + ".wav";
            
            Settings.Instance.wavName = outputFilename;
            timeRecordingStarted = DateTime.Now;
            timeSilenceStarted = DateTime.Now;
            writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
        }
        //Окончание записи
        private void waveIn_RecordingStopped(object sender, EventArgs e) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else {
                if (waveIn != null) {
                    waveIn.Dispose();
                    waveIn = null;
                }
                if (writer != null) {
                    writer.Close();
                    writer = null;
                }
            }
        }

        public void StartListening(int ID = -1) {
            if (ID != -1) thisID = ID; else if (thisID == -1) thisID = rnd.Next() % 100;

            tmrUI.Enabled = true;
            UITransition = 0;
            RequestsFailedCounter = 0;
            UIState = UIStates.Listening;
            try {
                timeRecordingStarted = DateTime.Now;
                timeSilenceStarted = DateTime.Now;

                WaitingForSound = true;

                waveIn = new WaveIn();

                waveIn.DeviceNumber = 0;//Дефолтное устройство для записи (если оно имеется)
                waveIn.DataAvailable += waveIn_DataAvailable;//Прикрепляем к событию DataAvailable обработчик, возникающий при наличии записываемых данных
                waveIn.RecordingStopped += new EventHandler(waveIn_RecordingStopped);//Прикрепляем обработчик завершения записи

                waveIn.WaveFormat = new WaveFormat(16000, 1);//Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)

                waveIn.StartRecording();
                Recording = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            //sre.RecognizeAsync();
        }
    }
}
