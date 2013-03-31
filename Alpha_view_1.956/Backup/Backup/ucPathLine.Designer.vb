<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucPathLine
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
        Me.lblName = New System.Windows.Forms.Label
        Me.picMain = New System.Windows.Forms.PictureBox
        Me.txtPath = New System.Windows.Forms.TextBox
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblName.Location = New System.Drawing.Point(113, 75)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(39, 12)
        Me.lblName.TabIndex = 3
        Me.lblName.Text = "Label1"
        Me.lblName.Visible = False
        '
        'picMain
        '
        Me.picMain.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.picMain.Location = New System.Drawing.Point(0, 0)
        Me.picMain.Name = "picMain"
        Me.picMain.Size = New System.Drawing.Size(157, 25)
        Me.picMain.TabIndex = 2
        Me.picMain.TabStop = False
        '
        'txtPath
        '
        Me.txtPath.BackColor = System.Drawing.Color.FromArgb(CType(CType(229, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(229, Byte), Integer))
        Me.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPath.Location = New System.Drawing.Point(44, 6)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(279, 13)
        Me.txtPath.TabIndex = 4
        Me.txtPath.Visible = False
        '
        'ucPathLine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.picMain)
        Me.Name = "ucPathLine"
        Me.Size = New System.Drawing.Size(471, 118)
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents picMain As System.Windows.Forms.PictureBox
    Friend WithEvents txtPath As System.Windows.Forms.TextBox

End Class
