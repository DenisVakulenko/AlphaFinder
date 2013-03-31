namespace TestHelpers
{
    partial class ucVoiceRecognizer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrUI = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrUI
            // 
            this.tmrUI.Interval = 14;
            this.tmrUI.Tick += new System.EventHandler(this.tmrUI_Tick);
            // 
            // ucVoiceRecognizer
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DoubleBuffered = true;
            this.Name = "ucVoiceRecognizer";
            this.Load += new System.EventHandler(this.ucVoiceRecognizer_Load_1);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucVoiceRecognizer_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ucVoiceRecognizer_MouseUp);
            this.Resize += new System.EventHandler(this.ucVoiceRecognizer_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrUI;

    }
}
