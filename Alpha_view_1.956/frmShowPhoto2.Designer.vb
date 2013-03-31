<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowPhoto2
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShowPhoto2))
        Me.picPhoto = New System.Windows.Forms.PictureBox()
        Me.bwLoadingPicture = New System.ComponentModel.BackgroundWorker()
        Me.picTail = New System.Windows.Forms.PictureBox()
        Me.tmrFading = New System.Windows.Forms.Timer(Me.components)
        Me.bwLP = New System.ComponentModel.BackgroundWorker()
        Me.lblNum = New System.Windows.Forms.Label()
        Me.tmrSlideShow = New System.Windows.Forms.Timer(Me.components)
        Me.lblSlideShow = New System.Windows.Forms.Label()
        Me.UcOnAirRecognizer1 = New TestHelpers.ucOnAirRecognizer()
        Me.ButtonP1 = New photo_view.ucImagedButton()
        CType(Me.picPhoto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picTail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picPhoto
        '
        Me.picPhoto.BackColor = System.Drawing.Color.White
        Me.picPhoto.Location = New System.Drawing.Point(130, 59)
        Me.picPhoto.Name = "picPhoto"
        Me.picPhoto.Size = New System.Drawing.Size(227, 230)
        Me.picPhoto.TabIndex = 14
        Me.picPhoto.TabStop = False
        '
        'picTail
        '
        Me.picTail.Location = New System.Drawing.Point(414, 186)
        Me.picTail.Name = "picTail"
        Me.picTail.Size = New System.Drawing.Size(100, 50)
        Me.picTail.TabIndex = 15
        Me.picTail.TabStop = False
        '
        'tmrFading
        '
        Me.tmrFading.Interval = 10
        '
        'bwLP
        '
        Me.bwLP.WorkerSupportsCancellation = True
        '
        'lblNum
        '
        Me.lblNum.BackColor = System.Drawing.Color.Black
        Me.lblNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblNum.ForeColor = System.Drawing.Color.Transparent
        Me.lblNum.Location = New System.Drawing.Point(-1, -1)
        Me.lblNum.Name = "lblNum"
        Me.lblNum.Size = New System.Drawing.Size(110, 17)
        Me.lblNum.TabIndex = 18
        Me.lblNum.Text = "( )"
        Me.lblNum.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'tmrSlideShow
        '
        Me.tmrSlideShow.Interval = 10000
        '
        'lblSlideShow
        '
        Me.lblSlideShow.AutoSize = True
        Me.lblSlideShow.ForeColor = System.Drawing.Color.White
        Me.lblSlideShow.Location = New System.Drawing.Point(7, 18)
        Me.lblSlideShow.Name = "lblSlideShow"
        Me.lblSlideShow.Size = New System.Drawing.Size(0, 13)
        Me.lblSlideShow.TabIndex = 19
        '
        'UcOnAirRecognizer1
        '
        Me.UcOnAirRecognizer1.BackColor = System.Drawing.Color.White
        Me.UcOnAirRecognizer1.BackgroundImage = CType(resources.GetObject("UcOnAirRecognizer1.BackgroundImage"), System.Drawing.Image)
        Me.UcOnAirRecognizer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.UcOnAirRecognizer1.Location = New System.Drawing.Point(10, 79)
        Me.UcOnAirRecognizer1.Name = "UcOnAirRecognizer1"
        Me.UcOnAirRecognizer1.Size = New System.Drawing.Size(92, 45)
        Me.UcOnAirRecognizer1.TabIndex = 20
        Me.UcOnAirRecognizer1.TabStop = False
        Me.UcOnAirRecognizer1.Visible = False
        '
        'ButtonP1
        '
        Me.ButtonP1.BackColor = System.Drawing.Color.White
        Me.ButtonP1.Location = New System.Drawing.Point(39, 33)
        Me.ButtonP1.Name = "ButtonP1"
        Me.ButtonP1.Size = New System.Drawing.Size(29, 29)
        Me.ButtonP1.TabIndex = 16
        Me.ButtonP1.TabStop = False
        Me.ButtonP1.Visible = False
        '
        'frmShowPhoto2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Magenta
        Me.ClientSize = New System.Drawing.Size(699, 482)
        Me.Controls.Add(Me.UcOnAirRecognizer1)
        Me.Controls.Add(Me.lblSlideShow)
        Me.Controls.Add(Me.lblNum)
        Me.Controls.Add(Me.ButtonP1)
        Me.Controls.Add(Me.picPhoto)
        Me.Controls.Add(Me.picTail)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmShowPhoto2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "frmShowPhoto2"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.picPhoto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picTail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picPhoto As System.Windows.Forms.PictureBox
    Friend WithEvents bwLoadingPicture As System.ComponentModel.BackgroundWorker
    Friend WithEvents picTail As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonP1 As photo_view.ucImagedButton
    Friend WithEvents tmrFading As System.Windows.Forms.Timer
    Friend WithEvents bwLP As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblNum As System.Windows.Forms.Label
    Friend WithEvents tmrSlideShow As System.Windows.Forms.Timer
    Friend WithEvents lblSlideShow As System.Windows.Forms.Label
    Friend WithEvents UcOnAirRecognizer1 As TestHelpers.ucOnAirRecognizer
End Class
