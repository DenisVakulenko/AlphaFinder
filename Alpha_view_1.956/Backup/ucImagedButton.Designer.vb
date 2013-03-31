<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucImagedButton
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
        Me.pic = New System.Windows.Forms.PictureBox
        Me.picSel = New System.Windows.Forms.PictureBox
        CType(Me.pic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pic
        '
        Me.pic.Location = New System.Drawing.Point(0, 0)
        Me.pic.Name = "pic"
        Me.pic.Size = New System.Drawing.Size(60, 39)
        Me.pic.TabIndex = 0
        Me.pic.TabStop = False
        '
        'picSel
        '
        Me.picSel.Location = New System.Drawing.Point(0, 0)
        Me.picSel.Name = "picSel"
        Me.picSel.Size = New System.Drawing.Size(83, 58)
        Me.picSel.TabIndex = 1
        Me.picSel.TabStop = False
        '
        'ucImagedButton
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.pic)
        Me.Controls.Add(Me.picSel)
        Me.DoubleBuffered = True
        Me.Name = "ucImagedButton"
        Me.Size = New System.Drawing.Size(115, 93)
        CType(Me.pic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pic As System.Windows.Forms.PictureBox
    Friend WithEvents picSel As System.Windows.Forms.PictureBox

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
