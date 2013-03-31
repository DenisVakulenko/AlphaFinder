<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucMusicBox
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
        Me.bwLoadOne = New System.ComponentModel.BackgroundWorker
        Me.SuspendLayout()
        '
        'tmrAnimation
        '
        Me.tmrAnimation.Interval = 14
        '
        'bwLoadOne
        '
        '
        'ucMusicBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.DoubleBuffered = True
        Me.Name = "ucMusicBox"
        Me.Size = New System.Drawing.Size(346, 337)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tmrAnimation As System.Windows.Forms.Timer
    Friend WithEvents bwLoadOne As System.ComponentModel.BackgroundWorker

End Class
