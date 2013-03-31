Public Class frmShowPhoto2
    Dim FileNames() As String
    Dim CurrentIndex As Long, CurrentImage, ResizedCurrentImage As Bitmap
    Dim Zoom As Long = 3


    Public Sub Init2(ByVal Thumbnail As Bitmap, ByVal Index As Long, ByVal ImageFileNamesArray() As String)
        CurrentIndex = Index
        FileNames = ImageFileNamesArray
        CurrentImage = Thumbnail
        ResizedCurrentImage = New Bitmap(CurrentImage.Width * Zoom, CurrentImage.Height * Zoom, Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
            graph.DrawImage(CurrentImage, 0, 0, ResizedCurrentImage.Width, ResizedCurrentImage.Height)
        End Using
        ResizedCurrentImage.SetPixel(1, 1, Color.White)
        ResizedCurrentImage.SetPixel(1, 2, Color.White)
        ResizedCurrentImage.SetPixel(1, 3, Color.White)
        ResizedCurrentImage.SetPixel(1, 4, Color.White)
        ResizedCurrentImage.SetPixel(1, 5, Color.White)
        ResizedCurrentImage.SetPixel(2, 5, Color.White)
        ResizedCurrentImage.SetPixel(3, 5, Color.White)

        picPhoto.Width = ResizedCurrentImage.Width
        picPhoto.Height = ResizedCurrentImage.Height
        picPhoto.Image = ResizedCurrentImage
        picPhoto.Left = Cursor.Position.X - ResizedCurrentImage.Width / 2
        picPhoto.Top = Cursor.Position.Y - ResizedCurrentImage.Height / 2

        Me.Left = 0
        Me.Top = 0
        Me.Size = Screen.PrimaryScreen.WorkingArea.Size
        picPhoto.Refresh()
        bwLP.RunWorkerAsync()
    End Sub
    Public Sub Init3(ByVal Thumbnail As Bitmap, ByVal Index As Long, ByVal ImageFileNamesArray() As String, ByVal x As Long, ByVal y As Long)
        CurrentIndex = Index
        FileNames = ImageFileNamesArray
        CurrentImage = Thumbnail
        If Math.Max(CurrentImage.Width, CurrentImage.Height) < 100 Then Zoom = Zoom * 4
        ResizedCurrentImage = New Bitmap(CurrentImage.Width * Zoom, CurrentImage.Height * Zoom, Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
            graph.DrawImage(CurrentImage, 0, 0, ResizedCurrentImage.Width, ResizedCurrentImage.Height)
        End Using
        ResizedCurrentImage.SetPixel(1, 1, Color.White)
        ResizedCurrentImage.SetPixel(1, 2, Color.White)
        ResizedCurrentImage.SetPixel(1, 3, Color.White)
        ResizedCurrentImage.SetPixel(1, 4, Color.White)
        ResizedCurrentImage.SetPixel(1, 5, Color.White)
        ResizedCurrentImage.SetPixel(2, 5, Color.White)
        ResizedCurrentImage.SetPixel(3, 5, Color.White)

        picPhoto.Width = ResizedCurrentImage.Width
        picPhoto.Height = ResizedCurrentImage.Height
        If Not (picPhoto.Image Is Nothing) Then picPhoto.Image.Dispose()
        picPhoto.Image = ResizedCurrentImage
        picPhoto.Left = x - ResizedCurrentImage.Width / Zoom
        picPhoto.Top = y - ResizedCurrentImage.Height / Zoom

        Me.Left = 0
        Me.Top = 0
        Me.Size = Screen.PrimaryScreen.WorkingArea.Size
        picPhoto.Refresh()
        bwLP.RunWorkerAsync()
    End Sub
    Public Sub InitFullScreened(ByVal Thumbnail As Bitmap, ByVal Index As Long, ByVal ImageFileNamesArray() As String)
        CurrentIndex = Index
        FileNames = ImageFileNamesArray
        CurrentImage = Thumbnail

        isFullScreen = True
        Dim w As Long = CurrentImage.Width * 100
        Dim h As Long = CurrentImage.Height * 100
        CorrectSize(w, h, Screen.PrimaryScreen.WorkingArea.Size)

        ResizedCurrentImage = New Bitmap(w, h, Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
            If bwLoadingPicture.IsBusy Then
                'graph.DrawImage(bmpPictsTh(ImageIndex), 0, 0, w, h)
            Else
                graph.DrawImage(CurrentImage, 0, 0, w, h)
            End If
        End Using

        ResizedCurrentImage.SetPixel(1, 1, Color.White)
        ResizedCurrentImage.SetPixel(1, 2, Color.White)
        ResizedCurrentImage.SetPixel(1, 3, Color.White)
        ResizedCurrentImage.SetPixel(1, 4, Color.White)
        ResizedCurrentImage.SetPixel(1, 5, Color.White)
        ResizedCurrentImage.SetPixel(2, 5, Color.White)
        ResizedCurrentImage.SetPixel(3, 5, Color.White)

        'picPhoto.Width = ResizedCurrentImage.Width
        'picPhoto.Height = ResizedCurrentImage.Height
        'picPhoto.Image = ResizedCurrentImage
        'picPhoto.Left = x - ResizedCurrentImage.Width / Zoom
        'picPhoto.Top = y - ResizedCurrentImage.Height / Zoom

        Me.Left = 0
        Me.Top = 0
        Me.Size = Screen.PrimaryScreen.WorkingArea.Size

        picPhoto.Width = w
        picPhoto.Height = h
        picPhoto.Left = (Screen.PrimaryScreen.WorkingArea.Width - w) / 2
        picPhoto.Top = (Screen.PrimaryScreen.WorkingArea.Height - h) / 2

        If Not (picPhoto.Image Is Nothing) Then picPhoto.Image.Dispose()
        picPhoto.Image = ResizedCurrentImage
        Me.BackColor = Color.Black

        'picPhoto.Refresh()
        Me.Refresh()
        bwLP.RunWorkerAsync()
    End Sub

    Private Sub bwLP_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLP.DoWork
        If Not CurrentImage Is Nothing Then CurrentImage.Dispose()
        If IO.File.Exists(FileNames(CurrentIndex)) Then
            CurrentImage = Image.FromFile(FileNames(CurrentIndex)).Clone()
            'If Not (ResizedCurrentImage Is Nothing) Then ResizedCurrentImage.Dispose()
            If isFullScreen Then
                Dim w As Long = CurrentImage.Width
                Dim h As Long = CurrentImage.Height
                CorrectSize(w, h, Screen.PrimaryScreen.WorkingArea.Size)

                ResizedCurrentImage = New Bitmap(w, h, Drawing.Imaging.PixelFormat.Format32bppRgb)
                Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
                    graph.DrawImage(CurrentImage, 0, 0, w, h)
                End Using
            Else
                Dim w As Long = (CurrentImage.Width / CurrentImage.Height) * picPhoto.Height
                ResizedCurrentImage = New Bitmap(w, picPhoto.Height, Drawing.Imaging.PixelFormat.Format32bppRgb)
                Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
                    graph.DrawImage(CurrentImage, 0, 0, w, picPhoto.Height)
                End Using
            End If
        End If
    End Sub
    Private Sub bwLP_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLP.RunWorkerCompleted
        'If Not (picPhoto.Image Is Nothing) Then picPhoto.Image.Dispose()
        If isFullScreen Then
            picPhoto.Left = (Screen.PrimaryScreen.WorkingArea.Width - ResizedCurrentImage.Width) / 2
            picPhoto.Top = (Screen.PrimaryScreen.WorkingArea.Height - ResizedCurrentImage.Height) / 2
        Else
            picPhoto.Left += (picPhoto.Width - ResizedCurrentImage.Width) / 2
        End If
        picPhoto.Width = ResizedCurrentImage.Width
        picPhoto.Height = ResizedCurrentImage.Height
        picPhoto.Image = ResizedCurrentImage
        lblNum.Text = "( " + CurrentIndex.ToString + " | " + (FileNames.Length - 1).ToString + " )"
        Me.Text = GetFileName(FileNames(CurrentIndex))
    End Sub

#Region "Form moving and FullScreening"
    Dim frmPoint As Point
    Dim FrmRect As Rectangle
    Private Sub Me_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles picPhoto.DoubleClick, Me.DoubleClick
        FullScreen()
        ''If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
        ''Me.Location = FrmRect.Location

        ''Me.Size = FrmRect.Size
        ''Else
        ''FrmRect.Location = Me.Location
        ''FrmRect.Size = Me.Size
        'picPhoto.Left = 0
        'picPhoto.Top = 0
        'Dim w As Long = CurrentImage.Width
        'Dim h As Long = CurrentImage.Height
        'CorrectSize(w, h, Screen.PrimaryScreen.WorkingArea.Size)
        'picPhoto.Width = w
        'picPhoto.Height = h
        'picPhoto.Left = (Screen.PrimaryScreen.WorkingArea.Width - w) / 2

        'ResizedCurrentImage = New Bitmap(w, h, Drawing.Imaging.PixelFormat.Format32bppRgb)
        'Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
        '    If bwLoadingPicture.IsBusy Then
        '        'graph.DrawImage(bmpPictsTh(ImageIndex), 0, 0, w, h)
        '    Else
        '        graph.DrawImage(CurrentImage, 0, 0, w, h)
        '    End If
        'End Using
        'picPhoto.Image = ResizedCurrentImage

        'Me.BackColor = Color.Black
        ''picPhoto.Width = Screen.PrimaryScreen.WorkingArea.Width
        ''picPhoto.Height = Screen.PrimaryScreen.WorkingArea.Height
        ''SetFullScreenBorders()
        ''ResizeAll()
        'Me.Refresh()
        ''End If
    End Sub

    Private Sub picPhoto_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picPhoto.MouseClick, Me.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then Me.Dispose()
    End Sub
    Private Sub Me_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picPhoto.MouseDown, Me.MouseDown
        frmPoint = e.Location
        picTail.Size = picPhoto.Size
    End Sub
    Private Sub Me_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picPhoto.MouseMove, Me.MouseMove
        If e.Button Then
            picTail.Location = picPhoto.Location
            picPhoto.Left += -frmPoint.X + e.X
            picPhoto.Top += -frmPoint.Y + e.Y
            'lblNum.Left = 0 'picPhoto.Left
            'lblNum.Top = 0 'picPhoto.Top - lblNum.Height - 6
            picTail.Refresh()
            picPhoto.Refresh()
            'Me.Refresh()
            'If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
            '    Me.Size = FrmRect.Size
            '    'SetNormalBorders()
            'End If
        End If
    End Sub
#End Region

    Private Sub frmShowPhoto_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        picTail.Size = picPhoto.Size
        picTail.Location = picPhoto.Location

        picPhoto.Width = picPhoto.Width + e.Delta
        If picPhoto.Width < 50 Then picPhoto.Width = 50
        picPhoto.Height = CurrentImage.Height / CurrentImage.Width * picPhoto.Width
        picPhoto.Left = picPhoto.Left - e.Delta / 2
        picPhoto.Top = picPhoto.Top - e.Delta / 2

        If Not (ResizedCurrentImage Is Nothing) Then ResizedCurrentImage.Dispose()
        ResizedCurrentImage = New Bitmap(picPhoto.Width, picPhoto.Height, Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
            graph.FillRectangle(Brushes.White, 1, 1, picPhoto.Width - 2, picPhoto.Height - 2)
            Dim a As Long = picPhoto.Width / 30
            If bwLoadingPicture.IsBusy Then
                'graph.DrawImage(bmpPictsTh(ImageIndex), a, a, picPhoto.Width - a * 2, picPhoto.Height - a * 2)
            Else
                graph.DrawImage(CurrentImage, a, a, picPhoto.Width - a * 2, picPhoto.Height - a * 2)
            End If
        End Using
        If Not (picPhoto.Image Is Nothing) Then picPhoto.Image.Dispose()
        picPhoto.Image = ResizedCurrentImage

        If picPhoto.Width >= Screen.PrimaryScreen.WorkingArea.Width - 200 Or picPhoto.Height >= Screen.PrimaryScreen.WorkingArea.Height - 200 Then
            If Me.BackColor = Color.Magenta Then Me.BackColor = Color.Black : Me.Refresh()
        Else
            If Me.BackColor = Color.Black Then Me.BackColor = Color.Magenta : Me.Refresh()
        End If
        picTail.Refresh()
        picPhoto.Refresh()
    End Sub

    Private Sub frmShowPhoto_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        'picPhoto.Size = Me.ClientRectangle.Size
    End Sub

    Private Sub tmrFading_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrFading.Tick
        Me.Opacity += 0.2
        Me.Refresh()
        If Me.Opacity = 1 Then tmrFading.Enabled = False
    End Sub

    Private Sub frmShowPhoto2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Left
                CurrentIndex -= 1
                RefreshImage()
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.Up
                CurrentIndex -= 1
                RefreshImage()
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.PageUp
                CurrentIndex -= 1
                RefreshImage()
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.Right
                CurrentIndex += 1
                RefreshImage()
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.Down
                CurrentIndex += 1
                RefreshImage()
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.PageDown
                CurrentIndex += 1
                RefreshImage()
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.Space
                CurrentIndex += 1
                If tmrSlideShow.Enabled = True Then tmrSlideShow.Stop() : tmrSlideShow.Start()
            Case Keys.S
                tmrSlideShow.Enabled = True
            Case Keys.R
                tmrSlideShow.Enabled = False
            Case Keys.O
                tmrSlideShow.Interval *= 1.2
            Case Keys.I
                tmrSlideShow.Interval *= 0.8
            Case Keys.Escape
                Me.Dispose()
            Case Keys.Back
                Me.Dispose()
            Case Keys.P
                Process.Start("C:\Program Files (x86)\Adobe\Adobe Photoshop CS4\Photoshop.exe", Chr(34) + FileNames(CurrentIndex) + Chr(34))
            Case Keys.Enter
                FullScreen()
            Case Keys.Add
                ResizeImage(+100)
            Case Keys.Oemplus
                ResizeImage(+100)
            Case Keys.Subtract
                ResizeImage(-100)
            Case Keys.OemMinus
                ResizeImage(-100)
        End Select
        If tmrSlideShow.Enabled Then
            lblSlideShow.Text = "Show on  |  "
        Else
            lblSlideShow.Text = "Show off  |  "
        End If
        lblSlideShow.Text += (tmrSlideShow.Interval / 1000).ToString + " sec"
    End Sub
    Sub ResizeImage(ByVal d As Integer)
        picTail.Size = picPhoto.Size
        picTail.Location = picPhoto.Location

        picPhoto.Width = picPhoto.Width + d
        If picPhoto.Width < 50 Then picPhoto.Width = 50
        picPhoto.Height = CurrentImage.Height / CurrentImage.Width * picPhoto.Width
        picPhoto.Left = picPhoto.Left - d / 2
        picPhoto.Top = picPhoto.Top - d / 2

        ResizedCurrentImage.Dispose()
        ResizedCurrentImage = New Bitmap(picPhoto.Width, picPhoto.Height, Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
            graph.FillRectangle(Brushes.White, 1, 1, picPhoto.Width - 2, picPhoto.Height - 2)
            Dim a As Long = picPhoto.Width / 30
            If bwLoadingPicture.IsBusy Then
                'graph.DrawImage(bmpPictsTh(ImageIndex), a, a, picPhoto.Width - a * 2, picPhoto.Height - a * 2)
            Else
                graph.DrawImage(CurrentImage, a, a, picPhoto.Width - a * 2, picPhoto.Height - a * 2)
            End If
        End Using
        If Not (picPhoto.Image Is Nothing) Then picPhoto.Image.Dispose()
        picPhoto.Image = ResizedCurrentImage

        If picPhoto.Width >= Screen.PrimaryScreen.WorkingArea.Width - 200 Or picPhoto.Height >= Screen.PrimaryScreen.WorkingArea.Height - 200 Then
            If Me.BackColor = Color.Magenta Then Me.BackColor = Color.Black : Me.Refresh()
        Else
            If Me.BackColor = Color.Black Then Me.BackColor = Color.Magenta : Me.Refresh()
        End If
        picTail.Refresh()
        picPhoto.Refresh()
    End Sub


    Sub RefreshImage()
        If CurrentIndex < 1 Then CurrentIndex = 1
        If CurrentIndex > FileNames.Length - 1 Then CurrentIndex = FileNames.Length - 1 : Me.Close()

        If bwLP.IsBusy Then bwLP.CancelAsync()
        While bwLP.IsBusy
            Application.DoEvents()
        End While
        bwLP.RunWorkerAsync()
    End Sub

    Dim isFullScreen As Boolean = False
    Public Sub FullScreen()
        isFullScreen = True
        Dim w As Long = CurrentImage.Width
        Dim h As Long = CurrentImage.Height
        CorrectSize(w, h, Screen.PrimaryScreen.WorkingArea.Size)
        picPhoto.Width = w
        picPhoto.Height = h
        picPhoto.Left = (Screen.PrimaryScreen.WorkingArea.Width - w) / 2
        picPhoto.Top = (Screen.PrimaryScreen.WorkingArea.Height - h) / 2

        ResizedCurrentImage = New Bitmap(w, h, Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graph As Graphics = Graphics.FromImage(ResizedCurrentImage)
            If bwLoadingPicture.IsBusy Then
                'graph.DrawImage(bmpPictsTh(ImageIndex), 0, 0, w, h)
            Else
                graph.DrawImage(CurrentImage, 0, 0, w, h)
            End If
        End Using
        If Not (picPhoto.Image Is Nothing) Then picPhoto.Image.Dispose()
        picPhoto.Image = ResizedCurrentImage

        Me.BackColor = Color.Black
        'picPhoto.Width = Screen.PrimaryScreen.WorkingArea.Width
        'picPhoto.Height = Screen.PrimaryScreen.WorkingArea.Height
        'SetFullScreenBorders()
        'ResizeAll()
        Me.Refresh()
    End Sub


    Private Sub picPhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picPhoto.Click

    End Sub

    Private Sub frmShowPhoto2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UcOnAirRecognizer1.StartListening(100)
    End Sub

    Private Sub tmrSlideShow_Tick(sender As Object, e As EventArgs) Handles tmrSlideShow.Tick
        CurrentIndex += 1
        If CurrentIndex > FileNames.Length - 1 Then CurrentIndex = 1
        RefreshImage()
    End Sub



    Private Sub UcOnAirRecognizer1_SmthRecognized(text() As String) Handles UcOnAirRecognizer1.SmthRecognized
        Dim rec As String

        For i As Long = 0 To Math.Min(5, text.Length - 1)
            rec = LCase(text(i))
            If rec = "close" Or rec = "exit" Then Me.Dispose() : Exit For
            If rec = "next" Or rec = "right" Or rec = "down" Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.Right)) : Exit For
            If rec = "previous" Or rec = "up" Or rec = "left" Or rec = "back" Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.Left)) : Exit For
            If rec.IndexOf("photoshop") >= 0 Or (rec.IndexOf("photo") >= 0 And rec.IndexOf("shop") >= 0) Or rec.IndexOf("edit") >= 0 Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.P)) : Exit For
            If rec.IndexOf("start") >= 0 Or (rec.IndexOf("slideshow") >= 0 And rec.IndexOf("stop") < 0) Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.S)) : Exit For
            If rec.IndexOf("stop") >= 0 Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.R)) : Exit For
            If rec = "pasta" Or rec = "fast" Or rec.IndexOf("faster") >= 0 Or (rec.IndexOf("too") >= 0 And rec.IndexOf("slow") >= 0) Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.I)) : Exit For
            If rec = "slow" Or rec.IndexOf("slower") >= 0 Or ((rec.IndexOf("too") >= 0 Or rec.IndexOf("very") >= 0) And rec.IndexOf("fast") >= 0) Then frmShowPhoto2_KeyDown(New Object(), New KeyEventArgs(Keys.O)) : Exit For
        Next
    End Sub

    Private Sub frmShowPhoto2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not picPhoto.Image Is Nothing Then picPhoto.Image.Dispose()
        If Not CurrentImage Is Nothing Then CurrentImage.Dispose()
        UcOnAirRecognizer1.StopRecording()
    End Sub
End Class