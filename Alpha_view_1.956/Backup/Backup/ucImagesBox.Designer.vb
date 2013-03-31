<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucImagesBox
    Inherits System.Windows.Forms.UserControl

    'Пользовательский элемент управления (UserControl) переопределяет метод Dispose для очистки списка компонентов.
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
        Me.tmrAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.bwLoadImages = New System.ComponentModel.BackgroundWorker
        Me.picMain = New System.Windows.Forms.PictureBox
        Me.picForDrag = New System.Windows.Forms.PictureBox
        Me.bwLoadOne = New System.ComponentModel.BackgroundWorker
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picForDrag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tmrAnimation
        '
        Me.tmrAnimation.Interval = 14
        '
        'bwLoadImages
        '
        Me.bwLoadImages.WorkerReportsProgress = True
        Me.bwLoadImages.WorkerSupportsCancellation = True
        '
        'picMain
        '
        Me.picMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.picMain.Location = New System.Drawing.Point(0, 0)
        Me.picMain.Name = "picMain"
        Me.picMain.Size = New System.Drawing.Size(103, 114)
        Me.picMain.TabIndex = 0
        Me.picMain.TabStop = False
        '
        'picForDrag
        '
        Me.picForDrag.Location = New System.Drawing.Point(176, 52)
        Me.picForDrag.Name = "picForDrag"
        Me.picForDrag.Size = New System.Drawing.Size(66, 45)
        Me.picForDrag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picForDrag.TabIndex = 1
        Me.picForDrag.TabStop = False
        Me.picForDrag.Visible = False
        '
        'bwLoadOne
        '
        '
        'ucImagesBox
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.Controls.Add(Me.picForDrag)
        Me.Controls.Add(Me.picMain)
        Me.DoubleBuffered = True
        Me.Name = "ucImagesBox"
        Me.Size = New System.Drawing.Size(471, 172)
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picForDrag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tmrAnimation As System.Windows.Forms.Timer
    Friend WithEvents bwLoadImages As System.ComponentModel.BackgroundWorker
    Friend WithEvents picMain As System.Windows.Forms.PictureBox
    Friend WithEvents picForDrag As System.Windows.Forms.PictureBox
    Friend WithEvents bwLoadOne As System.ComponentModel.BackgroundWorker

End Class
