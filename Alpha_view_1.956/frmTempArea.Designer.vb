<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTempArea
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTempArea))
        Me.BorderRight = New System.Windows.Forms.PictureBox()
        Me.BorderRightBottom = New System.Windows.Forms.PictureBox()
        Me.BorderLeftBottom = New System.Windows.Forms.PictureBox()
        Me.BorderBottom = New System.Windows.Forms.PictureBox()
        Me.BorderRightTop = New System.Windows.Forms.PictureBox()
        Me.BorderLeftTop = New System.Windows.Forms.PictureBox()
        Me.tmrDirsBoxAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.BorderTop = New System.Windows.Forms.PictureBox()
        Me.BorderLeft = New System.Windows.Forms.PictureBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.btnSortByName = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.sbView = New photo_view.ucScrollBar()
        Me.ImagesBox = New photo_view.ucImagesBox()
        Me.UcCheckBox1 = New photo_view.ucCheckBox()
        Me.btnClose = New photo_view.ucImagedButton()
        Me.PathLine = New photo_view.ucPathLine()
        CType(Me.BorderRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderRightBottom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderLeftBottom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderBottom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderRightTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderLeftTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BorderLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BorderRight
        '
        Me.BorderRight.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.BorderRight.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.BorderRight.Location = New System.Drawing.Point(646, 6)
        Me.BorderRight.Name = "BorderRight"
        Me.BorderRight.Size = New System.Drawing.Size(6, 41)
        Me.BorderRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.BorderRight.TabIndex = 64
        Me.BorderRight.TabStop = False
        '
        'BorderRightBottom
        '
        Me.BorderRightBottom.BackColor = System.Drawing.Color.Magenta
        Me.BorderRightBottom.Cursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.BorderRightBottom.Location = New System.Drawing.Point(646, 47)
        Me.BorderRightBottom.Name = "BorderRightBottom"
        Me.BorderRightBottom.Size = New System.Drawing.Size(6, 6)
        Me.BorderRightBottom.TabIndex = 68
        Me.BorderRightBottom.TabStop = False
        '
        'BorderLeftBottom
        '
        Me.BorderLeftBottom.BackColor = System.Drawing.Color.Magenta
        Me.BorderLeftBottom.Cursor = System.Windows.Forms.Cursors.SizeNESW
        Me.BorderLeftBottom.Location = New System.Drawing.Point(0, 412)
        Me.BorderLeftBottom.Name = "BorderLeftBottom"
        Me.BorderLeftBottom.Size = New System.Drawing.Size(6, 6)
        Me.BorderLeftBottom.TabIndex = 67
        Me.BorderLeftBottom.TabStop = False
        '
        'BorderBottom
        '
        Me.BorderBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.BorderBottom.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.BorderBottom.Location = New System.Drawing.Point(6, 412)
        Me.BorderBottom.Name = "BorderBottom"
        Me.BorderBottom.Size = New System.Drawing.Size(284, 6)
        Me.BorderBottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.BorderBottom.TabIndex = 66
        Me.BorderBottom.TabStop = False
        '
        'BorderRightTop
        '
        Me.BorderRightTop.BackColor = System.Drawing.Color.Magenta
        Me.BorderRightTop.Cursor = System.Windows.Forms.Cursors.SizeNESW
        Me.BorderRightTop.Location = New System.Drawing.Point(646, 0)
        Me.BorderRightTop.Name = "BorderRightTop"
        Me.BorderRightTop.Size = New System.Drawing.Size(6, 6)
        Me.BorderRightTop.TabIndex = 65
        Me.BorderRightTop.TabStop = False
        '
        'BorderLeftTop
        '
        Me.BorderLeftTop.BackColor = System.Drawing.Color.Magenta
        Me.BorderLeftTop.Cursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.BorderLeftTop.Location = New System.Drawing.Point(0, 0)
        Me.BorderLeftTop.Name = "BorderLeftTop"
        Me.BorderLeftTop.Size = New System.Drawing.Size(6, 6)
        Me.BorderLeftTop.TabIndex = 59
        Me.BorderLeftTop.TabStop = False
        '
        'tmrDirsBoxAnimation
        '
        Me.tmrDirsBoxAnimation.Interval = 10
        '
        'BorderTop
        '
        Me.BorderTop.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.BorderTop.Location = New System.Drawing.Point(6, 0)
        Me.BorderTop.Name = "BorderTop"
        Me.BorderTop.Size = New System.Drawing.Size(634, 6)
        Me.BorderTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.BorderTop.TabIndex = 60
        Me.BorderTop.TabStop = False
        '
        'BorderLeft
        '
        Me.BorderLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.BorderLeft.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.BorderLeft.Location = New System.Drawing.Point(0, 6)
        Me.BorderLeft.Name = "BorderLeft"
        Me.BorderLeft.Size = New System.Drawing.Size(6, 401)
        Me.BorderLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.BorderLeft.TabIndex = 58
        Me.BorderLeft.TabStop = False
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.White
        Me.lblName.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblName.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblName.Location = New System.Drawing.Point(910, 75)
        Me.lblName.Margin = New System.Windows.Forms.Padding(0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(39, 12)
        Me.lblName.TabIndex = 53
        Me.lblName.Text = "Label1"
        Me.lblName.Visible = False
        '
        'btnSortByName
        '
        Me.btnSortByName.Location = New System.Drawing.Point(541, 152)
        Me.btnSortByName.Name = "btnSortByName"
        Me.btnSortByName.Size = New System.Drawing.Size(37, 32)
        Me.btnSortByName.TabIndex = 69
        Me.btnSortByName.TabStop = False
        Me.btnSortByName.Text = "Button3"
        Me.btnSortByName.UseVisualStyleBackColor = True
        Me.btnSortByName.Visible = False
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(6, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(41, 29)
        Me.Button1.TabIndex = 71
        Me.Button1.Text = "clear"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Location = New System.Drawing.Point(53, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(113, 29)
        Me.Button2.TabIndex = 72
        Me.Button2.Text = "All rated music"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Location = New System.Drawing.Point(172, 6)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(102, 29)
        Me.Button3.TabIndex = 73
        Me.Button3.Text = "All music"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'sbView
        '
        Me.sbView.BackColor = System.Drawing.Color.Transparent
        Me.sbView.BackgroundImage = CType(resources.GetObject("sbView.BackgroundImage"), System.Drawing.Image)
        Me.sbView.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.sbView.Location = New System.Drawing.Point(392, 6)
        Me.sbView.Name = "sbView"
        Me.sbView.Size = New System.Drawing.Size(248, 29)
        Me.sbView.TabIndex = 75
        Me.sbView.TabStop = False
        Me.sbView.value = 50
        '
        'ImagesBox
        '
        Me.ImagesBox.AllowDrop = True
        Me.ImagesBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ImagesBox.BackgroundImage = CType(resources.GetObject("ImagesBox.BackgroundImage"), System.Drawing.Image)
        Me.ImagesBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ImagesBox.Location = New System.Drawing.Point(12, 125)
        Me.ImagesBox.Name = "ImagesBox"
        Me.ImagesBox.Size = New System.Drawing.Size(231, 104)
        Me.ImagesBox.TabIndex = 48
        '
        'UcCheckBox1
        '
        Me.UcCheckBox1.BackgroundImage = CType(resources.GetObject("UcCheckBox1.BackgroundImage"), System.Drawing.Image)
        Me.UcCheckBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.UcCheckBox1.caption = "on top"
        Me.UcCheckBox1.Location = New System.Drawing.Point(292, 6)
        Me.UcCheckBox1.Name = "UcCheckBox1"
        Me.UcCheckBox1.Size = New System.Drawing.Size(76, 25)
        Me.UcCheckBox1.TabIndex = 74
        Me.UcCheckBox1.value = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnClose.Location = New System.Drawing.Point(658, 6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(28, 22)
        Me.btnClose.TabIndex = 57
        Me.btnClose.TabStop = False
        '
        'PathLine
        '
        Me.PathLine.Location = New System.Drawing.Point(126, 47)
        Me.PathLine.Name = "PathLine"
        Me.PathLine.Size = New System.Drawing.Size(397, 29)
        Me.PathLine.TabIndex = 70
        Me.PathLine.TabStop = False
        Me.PathLine.Visible = False
        '
        'frmTempArea
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(726, 469)
        Me.Controls.Add(Me.sbView)
        Me.Controls.Add(Me.ImagesBox)
        Me.Controls.Add(Me.UcCheckBox1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnSortByName)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.BorderRight)
        Me.Controls.Add(Me.BorderRightBottom)
        Me.Controls.Add(Me.BorderLeftBottom)
        Me.Controls.Add(Me.BorderBottom)
        Me.Controls.Add(Me.BorderRightTop)
        Me.Controls.Add(Me.BorderLeftTop)
        Me.Controls.Add(Me.BorderTop)
        Me.Controls.Add(Me.BorderLeft)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.PathLine)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmTempArea"
        Me.Text = "temp window"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        CType(Me.BorderRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderRightBottom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderLeftBottom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderBottom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderRightTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderLeftTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BorderLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As photo_view.ucImagedButton
    Friend WithEvents BorderRight As System.Windows.Forms.PictureBox
    Friend WithEvents ImagesBox As photo_view.ucImagesBox
    Friend WithEvents BorderRightBottom As System.Windows.Forms.PictureBox
    Friend WithEvents BorderLeftBottom As System.Windows.Forms.PictureBox
    Friend WithEvents BorderBottom As System.Windows.Forms.PictureBox
    Friend WithEvents BorderRightTop As System.Windows.Forms.PictureBox
    Friend WithEvents BorderLeftTop As System.Windows.Forms.PictureBox
    Friend WithEvents tmrDirsBoxAnimation As System.Windows.Forms.Timer
    Friend WithEvents BorderTop As System.Windows.Forms.PictureBox
    Friend WithEvents BorderLeft As System.Windows.Forms.PictureBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents PathLine As photo_view.ucPathLine
    Friend WithEvents btnSortByName As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents UcCheckBox1 As photo_view.ucCheckBox
    Friend WithEvents sbView As photo_view.ucScrollBar
End Class
