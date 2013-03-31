<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucDirsBox
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
        Me.picMain = New System.Windows.Forms.PictureBox
        Me.tmrScrolling = New System.Windows.Forms.Timer(Me.components)
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picMain
        '
        Me.picMain.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.picMain.Location = New System.Drawing.Point(79, 41)
        Me.picMain.Name = "picMain"
        Me.picMain.Size = New System.Drawing.Size(112, 137)
        Me.picMain.TabIndex = 0
        Me.picMain.TabStop = False
        '
        'tmrScrolling
        '
        Me.tmrScrolling.Interval = 10
        '
        'ucDirsBox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange
        Me.Controls.Add(Me.picMain)
        Me.DoubleBuffered = True
        Me.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.Name = "ucDirsBox"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Size = New System.Drawing.Size(337, 266)
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picMain As System.Windows.Forms.PictureBox
    Friend WithEvents tmrScrolling As System.Windows.Forms.Timer

End Class
