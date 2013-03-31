namespace TestHelpers {
    partial class frmMusicPlayer {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMusicPlayer));
            this.lblTime = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tmrMousePos = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.sbNext = new UserControls.ucSkinButton();
            this.sbPlayPause = new UserControls.ucSkinButton();
            this.btnNext = new TestHelpers.ucSpotButton();
            this.btnPlayPause = new TestHelpers.ucSpotButton();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblTime.Location = new System.Drawing.Point(102, 12);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(133, 29);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "   ";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblName.Location = new System.Drawing.Point(12, 48);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(297, 29);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "label2";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrMousePos
            // 
            this.tmrMousePos.Enabled = true;
            this.tmrMousePos.Tick += new System.EventHandler(this.tmrMousePos_Tick);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(241, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "50";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // sbNext
            // 
            this.sbNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.sbNext.Location = new System.Drawing.Point(57, 12);
            this.sbNext.Name = "sbNext";
            this.sbNext.Size = new System.Drawing.Size(39, 29);
            this.sbNext.TabIndex = 9;
            this.sbNext.Load += new System.EventHandler(this.sbNext_Load);
            this.sbNext.Click += new System.EventHandler(this.sbNext_Click);
            // 
            // sbPlayPause
            // 
            this.sbPlayPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.sbPlayPause.Location = new System.Drawing.Point(12, 12);
            this.sbPlayPause.Name = "sbPlayPause";
            this.sbPlayPause.Size = new System.Drawing.Size(39, 29);
            this.sbPlayPause.TabIndex = 8;
            this.sbPlayPause.Load += new System.EventHandler(this.sbPlayPause_Load);
            this.sbPlayPause.Click += new System.EventHandler(this.sbPlayPause_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Black;
            this.btnNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNext.BackgroundImage")));
            this.btnNext.Location = new System.Drawing.Point(263, 49);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(29, 29);
            this.btnNext.TabIndex = 7;
            this.btnNext.Visible = false;
            this.btnNext.Load += new System.EventHandler(this.btnNext_Load);
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.BackColor = System.Drawing.Color.Black;
            this.btnPlayPause.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlayPause.BackgroundImage")));
            this.btnPlayPause.Location = new System.Drawing.Point(228, 49);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(29, 29);
            this.btnPlayPause.TabIndex = 6;
            this.btnPlayPause.Visible = false;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // frmMusicPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 90);
            this.Controls.Add(this.sbNext);
            this.Controls.Add(this.sbPlayPause);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPlayPause);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMusicPlayer";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Timer tmrMousePos;
        private System.Windows.Forms.Label label1;
        private UserControls.ucSkinButton sbPlayPause;
        private ucSpotButton btnNext;
        private ucSpotButton btnPlayPause;
        private UserControls.ucSkinButton sbNext;
    }
}