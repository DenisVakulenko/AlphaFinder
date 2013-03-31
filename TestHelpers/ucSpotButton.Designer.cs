namespace TestHelpers {
    partial class ucSpotButton {
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
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 14;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // ucSpotButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Name = "ucSpotButton";
            this.Size = new System.Drawing.Size(93, 88);
            this.Load += new System.EventHandler(this.ucSpotButton_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucSpotButton_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ucSpotButton_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ucSpotButton_MouseUp);
            this.Resize += new System.EventHandler(this.ucSpotButton_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrMain;
    }
}
