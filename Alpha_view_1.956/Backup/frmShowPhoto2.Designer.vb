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
        Me.components = New System.ComponentModel.Container
        Me.picPhoto = New System.Windows.Forms.PictureBox
        Me.bwLoadingPicture = New System.ComponentModel.BackgroundWorker
        Me.picTail = New System.Windows.Forms.PictureBox
        Me.tmrFading = New System.Windows.Forms.Timer(Me.components)
        Me.bwLP = New System.ComponentModel.BackgroundWorker
        Me.lblNum = New System.Windows.Forms.Label
        Me.ButtonP1 = New photo_view.ucImagedButton
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
        Me.lblNum.BackColor = System.Drawing.Color.White
        Me.lblNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblNum.ForeColor = System.Drawing.Color.Black
        Me.lblNum.Location = New System.Drawing.Point(90, 2)
        Me.lblNum.Name = "lblNum"
        Me.lblNum.Size = New System.Drawing.Size(114, 17)
        Me.lblNum.TabIndex = 18
        Me.lblNum.Text = "( )"
        Me.lblNum.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ButtonP1
        '
        Me.ButtonP1.BackColor = System.Drawing.Color.White
        Me.ButtonP1.Location = New System.Drawing.Point(12, 12)
        Me.ButtonP1.Name = "ButtonP1"
        Me.ButtonP1.Size = New System.Drawing.Size(29, 29)
        Me.ButtonP1.TabIndex = 16
        Me.ButtonP1.Visible = False
        '
        'frmShowPhoto2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Magenta
        Me.ClientSize = New System.Drawing.Size(699, 482)
        Me.Controls.Add(Me.lblNum)
        Me.Controls.Add(Me.ButtonP1)
        Me.Controls.Add(Me.picPhoto)
        Me.Controls.Add(Me.picTail)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmShowPhoto2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "frmShowPhoto2"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.picPhoto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picTail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picPhoto As System.Windows.Forms.PictureBox
    Friend WithEvents bwLoadingPicture As System.ComponentModel.BackgroundWorker
    Friend WithEvents picTail As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonP1 As photo_view.ucImagedButton
    Friend WithEvents tmrFading As System.Windows.Forms.Timer
    Friend WithEvents bwLP As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblNum As System.Windows.Forms.Label
End Class
