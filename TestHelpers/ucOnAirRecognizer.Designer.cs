namespace TestHelpers {
    partial class ucOnAirRecognizer {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tmrUI = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrUI
            // 
            this.tmrUI.Interval = 10;
            this.tmrUI.Tick += new System.EventHandler(this.tmrUI_Tick);
            // 
            // ucOnAirRecognizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DoubleBuffered = true;
            this.Name = "ucOnAirRecognizer";
            this.Size = new System.Drawing.Size(133, 45);
            this.Load += new System.EventHandler(this.ucOnAirRecognizer_Load);
            this.DoubleClick += new System.EventHandler(this.ucOnAirRecognizer_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucOnAirRecognizer_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ucOnAirRecognizer_MouseUp);
            this.Resize += new System.EventHandler(this.ucOnAirRecognizer_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrUI;
    }
}
