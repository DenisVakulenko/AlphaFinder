<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMusicPlayer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMusicPlayer))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.playList = New photo_view.ucImagesBox()
        Me.btnClose = New photo_view.ucSkinButton()
        Me.btnPlay = New photo_view.ucSkinButton()
        Me.btnStop = New photo_view.ucSkinButton()
        Me.sbPos = New photo_view.ucScrollBar()
        Me.sbVolume = New photo_view.ucScrollBar()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblInfo.ForeColor = System.Drawing.Color.Black
        Me.lblInfo.Location = New System.Drawing.Point(20, 19)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(98, 15)
        Me.lblInfo.TabIndex = 2
        Me.lblInfo.Text = "       "
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'tmrUpdate
        '
        Me.tmrUpdate.Enabled = True
        Me.tmrUpdate.Interval = 50
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.ForeColor = System.Drawing.Color.Black
        Me.Button2.Location = New System.Drawing.Point(131, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(97, 29)
        Me.Button2.TabIndex = 8
        Me.Button2.Text = "-->"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'playList
        '
        Me.playList.AllowDrop = True
        Me.playList.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.playList.Location = New System.Drawing.Point(1, 115)
        Me.playList.Name = "playList"
        Me.playList.Size = New System.Drawing.Size(282, 279)
        Me.playList.TabIndex = 6
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.btnClose.Location = New System.Drawing.Point(236, 9)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(39, 29)
        Me.btnClose.TabIndex = 11
        '
        'btnPlay
        '
        Me.btnPlay.BackColor = System.Drawing.Color.Transparent
        Me.btnPlay.Location = New System.Drawing.Point(27, 43)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(39, 29)
        Me.btnPlay.TabIndex = 3
        '
        'btnStop
        '
        Me.btnStop.BackColor = System.Drawing.Color.Transparent
        Me.btnStop.Location = New System.Drawing.Point(72, 43)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(39, 29)
        Me.btnStop.TabIndex = 4
        '
        'sbPos
        '
        Me.sbPos.BackColor = System.Drawing.Color.Transparent
        Me.sbPos.BackgroundImage = CType(resources.GetObject("sbPos.BackgroundImage"), System.Drawing.Image)
        Me.sbPos.Location = New System.Drawing.Point(13, 79)
        Me.sbPos.Name = "sbPos"
        Me.sbPos.Size = New System.Drawing.Size(260, 29)
        Me.sbPos.TabIndex = 1
        Me.sbPos.value = 50
        '
        'sbVolume
        '
        Me.sbVolume.BackColor = System.Drawing.Color.Transparent
        Me.sbVolume.BackgroundImage = CType(resources.GetObject("sbVolume.BackgroundImage"), System.Drawing.Image)
        Me.sbVolume.Location = New System.Drawing.Point(125, 43)
        Me.sbVolume.Name = "sbVolume"
        Me.sbVolume.Size = New System.Drawing.Size(139, 29)
        Me.sbVolume.TabIndex = 5
        Me.sbVolume.value = 15
        '
        'frmMusicPlayer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(284, 395)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.playList)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnPlay)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.sbPos)
        Me.Controls.Add(Me.sbVolume)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMusicPlayer"
        Me.Text = "MusicPlayer"
        Me.TransparencyKey = System.Drawing.Color.Magenta
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sbPos As photo_view.ucScrollBar
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents btnPlay As photo_view.ucSkinButton
    Friend WithEvents btnStop As photo_view.ucSkinButton
    Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
    Friend WithEvents sbVolume As photo_view.ucScrollBar
    Friend WithEvents playList As photo_view.ucImagesBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnClose As photo_view.ucSkinButton
End Class
