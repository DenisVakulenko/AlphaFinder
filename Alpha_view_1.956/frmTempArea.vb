Public Class frmTempArea
    Dim bmp_no_thumbnail As New Bitmap(Application.StartupPath + "\err2.png")
    'Dim bmp_folder As New Bitmap("folder.png")
    Dim bmp_error As New Bitmap(Application.StartupPath + "\err.png")
    Dim bmp_edge As New Bitmap(Application.StartupPath + "\edge.bmp")

    Dim Cl As New Point(10, 10), ClF As Point, moved As Boolean = False, ChosenObj As Long

    Dim files() As String, dirs() As String

    'Dim dir(1000) As dirs_class, n_dirs As Long

    Dim fly(1500) As fly_class
    Dim Surface As Double, SurfaceV As Double, SurfaceMax As Double

    Dim wire As New Point(72, 56), img_wire As New Rectangle(210, 210, 200, 200)

    Dim pic As New Bitmap(10, 10) 'Imaging.PixelFormat.Format32bppArgb)
    'Dim picNull As New Bitmap(W, H, Imaging.PixelFormat.Format24bppRgb)
    Dim graf As Graphics = Graphics.FromImage(pic)
    'Dim graf11Null As Graphics = Graphics.FromImage(picNull)
    Dim Counter As Byte
    Dim time1, time2 As DateTimeOffset, time_delta As TimeSpan
    Dim Fly_X, Fly_Y As Long

    Dim opened_image As Long
    Dim StopOperation As Boolean = False
    Dim IsMouseDown As Boolean
    Dim FlyMode As String
    Dim Resizing As Boolean


    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                StopOperation = True
            Case Keys.PageUp
                PageUp()
            Case Keys.PageDown
                PageDown()
            Case Keys.Back
                'If Not PathLine.txtPath.Focused Then 
                'DirsBox.FolderUp()
                'btnUp.btnDown()
                'End If
            Case Keys.P
                If e.Control Then PathLine.MakeTextMode()
        End Select
    End Sub
    Private Sub frmMain_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        'Surface = Surface + e.Delta / 15 '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    End Sub

#Region "AfterResizing"
    Private Sub ResizeBorder()
        BorderLeft.Height = Me.ClientRectangle.Height - 12
        BorderTop.Width = Me.ClientRectangle.Width - 12
        BorderRight.Height = Me.ClientRectangle.Height - 12
        BorderRight.Left = Me.ClientRectangle.Width - 6
        BorderBottom.Top = Me.ClientRectangle.Height - 6
        BorderBottom.Width = Me.ClientRectangle.Width - 12

        BorderLeftBottom.Top = Me.ClientRectangle.Height - 6
        BorderRightTop.Left = Me.ClientRectangle.Width - 6
        BorderRightBottom.Left = Me.ClientRectangle.Width - 6
        BorderRightBottom.Top = Me.ClientRectangle.Height - 6

        'ImagesBox.Width = Me.ClientRectangle.Width - 12
        'ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - 6
        'ImagesBox.FitImagesLocation()

        'DirsBox.Width = ImagesBox.Width
    End Sub
    Private Sub ResizeCustom()
        ImagesBox.Width = Me.ClientRectangle.Width - 2
        ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - 1

        ResizeBorder()

        btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - 6 + 2

        On Error Resume Next
        Me.Refresh()
    End Sub
    Private Sub ResizeAll()
        ResizeBorder()

        btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - 6 + 2
        btnClose.Top = 4

        ImagesBox.Left = 1
        ImagesBox.Top = 29 + 6 + 4
        ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - 1
        ImagesBox.Width = Me.ClientRectangle.Width - 2

        ImagesBox.OrderImages()

        Me.Refresh()
    End Sub
#End Region

    Private Sub LoadButtons()
        btnClose.LoadImages("buttons\close2.bmp", "buttons\close2_mm.bmp", "buttons\close2_down.bmp", "buttons\close_selection.bmp")
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            LoadButtons()
            LoadBorders()

            ImagesBox.LoadUIImages()
            ImagesBox.Left = 6
            ImagesBox.Width = Me.ClientRectangle.Width - 18
            ImagesBox.Wire.Border = 0

            ResizeAll()
        Catch ex As Exception
            MsgBox("Ooops!  " + ex.ToString)
        End Try
    End Sub

    Dim picResizeY As Long
    Dim DirsBoxHeight As Short

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim a() As System.Diagnostics.Process
        a = Process.GetProcesses()

        For i As Long = 0 To a.Length - 1
            If a(i).MainWindowTitle <> "" Then MsgBox(a(i).ProcessName + "     " + a(i).MainWindowTitle)
        Next
    End Sub


#Region "Buttons events"
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NThumbs = 0
    End Sub
    Private Sub btnStop_Click()
        StopOperation = True
    End Sub
    Private Sub btnOrder_Click()
        ImagesBox.OrderImages()
    End Sub
    Private Sub btnRnd_Click()
        ImagesBox.RandomImages()
    End Sub

    Private Sub btnClose_Click() Handles btnClose.Click
        Me.Close()
    End Sub
#End Region
#Region "Borders"
    Dim bmpBorders(16) As Bitmap, bmpBordersLoaded As Boolean = False

    Private Sub SetNormalBorders()
        Dim MyPath As String = Application.StartupPath + "\resources\"
        BorderTop.Visible = True
        BorderLeft.Visible = True
        BorderRight.Visible = True
        BorderBottom.Visible = True
        BorderLeftTop.Image = Image.FromFile(MyPath + "left-top.gif")
        BorderRightTop.Image = Image.FromFile(MyPath + "right-top.gif")
        BorderLeftBottom.Image = Image.FromFile(MyPath + "left-btm.gif")
        BorderRightBottom.Image = Image.FromFile(MyPath + "right-btm.gif")
        ResizeAll()
    End Sub
    Private Sub SetFullScreenBorders()
        Dim MyPath As String = Application.StartupPath + "\resources\"
        BorderTop.Visible = False
        BorderLeft.Visible = False
        BorderRight.Visible = False
        BorderBottom.Visible = False
        BorderLeftTop.Image = Image.FromFile(MyPath + "left-top-fs.gif")
        BorderRightTop.Image = Image.FromFile(MyPath + "right-top-fs.gif")
        BorderLeftBottom.Image = Image.FromFile(MyPath + "left-btm-fs.gif")
        BorderRightBottom.Image = Image.FromFile(MyPath + "right-btm-fs.gif")
        ResizeAll()
    End Sub
#End Region
#Region "Form moving and FullScreaning"
    Dim frmPoint As Point
    Dim FrmRect As Rectangle
    Private Sub Me_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
            Me.Location = FrmRect.Location
            Me.Size = FrmRect.Size
            SetNormalBorders()
        Else
            FrmRect.Location = Me.Location
            FrmRect.Size = Me.Size
            Me.Left = 0
            Me.Top = 0
            Me.Width = Screen.PrimaryScreen.WorkingArea.Width
            Me.Height = Screen.PrimaryScreen.WorkingArea.Height
            SetFullScreenBorders()
            ResizeAll()
            'btnClose.Left += 6 + 1 : btnClose.Top = -2 - 1
            Me.Refresh()
        End If
    End Sub
    Private Sub Me_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        frmPoint = e.Location
    End Sub
    Private Sub LoadBorders()
        Dim MyPath As String = Application.StartupPath + "\resources\"

        bmpBorders(0) = Image.FromFile(MyPath + "top.bmp")
        bmpBorders(1) = Image.FromFile(MyPath + "right.bmp")
        bmpBorders(2) = Image.FromFile(MyPath + "bottom.bmp")
        bmpBorders(3) = Image.FromFile(MyPath + "left.bmp")

        bmpBorders(4) = Image.FromFile(MyPath + "left-top.gif")
        bmpBorders(5) = Image.FromFile(MyPath + "right-top.gif")
        bmpBorders(6) = Image.FromFile(MyPath + "left-btm.gif")
        bmpBorders(7) = Image.FromFile(MyPath + "right-btm.gif")

        bmpBorders(8) = Image.FromFile(MyPath + "left-top-fs.gif")
        bmpBorders(9) = Image.FromFile(MyPath + "right-top-fs.gif")
        bmpBorders(10) = Image.FromFile(MyPath + "left-btm-fs.gif")
        bmpBorders(11) = Image.FromFile(MyPath + "right-btm-fs.gif")

        bmpBorders(12) = Image.FromFile(MyPath + "top-line.gif")
        bmpBorders(13) = Image.FromFile(MyPath + "right-line.gif")
        bmpBorders(14) = Image.FromFile(MyPath + "btm-line.gif")
        bmpBorders(15) = Image.FromFile(MyPath + "left-line.gif")

        BorderTop.Image = bmpBorders(0)
        BorderLeft.Image = bmpBorders(3)
        BorderRight.Image = bmpBorders(1)
        BorderBottom.Image = bmpBorders(2)

        BorderRightBottom.Image = bmpBorders(7)
        BorderRightTop.Image = bmpBorders(5)
        BorderLeftBottom.Image = bmpBorders(6)
        BorderLeftTop.Image = bmpBorders(4)

        bmpBordersLoaded = True
    End Sub
    Private Sub Me_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button Then
            Dim l, t As Long
            l = Me.Left - frmPoint.X + e.X
            t = Me.Top - frmPoint.Y + e.Y
            If l > -5 And l < 5 Then l = 0
            If t > -5 And t < 5 Then t = 0
            If t < Screen.PrimaryScreen.WorkingArea.Height - Me.Height + 5 And t > Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 5 Then t = Screen.PrimaryScreen.WorkingArea.Height - Me.Height
            If l < Screen.PrimaryScreen.WorkingArea.Width - Me.Width + 5 And l > Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 5 Then l = Screen.PrimaryScreen.WorkingArea.Width - Me.Width

            BorderRightBottom.Image = bmpBorders(7)
            BorderRightTop.Image = bmpBorders(5)
            BorderLeftBottom.Image = bmpBorders(6)
            BorderLeftTop.Image = bmpBorders(4)

            If l = 0 Then
                BorderLeft.Visible = False : BorderLeftTop.Image = bmpBorders(12) : BorderLeftBottom.Image = bmpBorders(14)
            Else
                BorderLeft.Visible = True
            End If
            If t = 0 Then
                BorderTop.Visible = False : BorderLeftTop.Image = bmpBorders(15) : BorderRightTop.Image = bmpBorders(13)
                If l = 0 Then BorderLeftTop.Image = bmpBorders(8)
            Else
                BorderTop.Visible = True
            End If
            If t >= Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                BorderBottom.Visible = False : BorderLeftBottom.Image = bmpBorders(15) : BorderRightBottom.Image = bmpBorders(13)
                If l = 0 Then BorderLeftBottom.Image = bmpBorders(10)
                If l = Screen.PrimaryScreen.WorkingArea.Width - Me.Width Then BorderRightBottom.Image = bmpBorders(11)
            Else
                BorderBottom.Visible = True
            End If
            If l = Screen.PrimaryScreen.WorkingArea.Width - Me.Width Then
                BorderRight.Visible = False : BorderRightTop.Image = bmpBorders(12) : BorderRightBottom.Image = bmpBorders(14)
                If t = 0 Then BorderRightTop.Image = bmpBorders(9)
                If t = Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then BorderRightBottom.Image = bmpBorders(11)
            Else
                BorderRight.Visible = True
            End If

            If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
                Me.Size = FrmRect.Size
                SetNormalBorders()
            End If
            Me.Left = l
            Me.Top = t
        End If
    End Sub
#End Region
#Region "Resizing"
    Dim ResizingPoint As Point

    Private Sub BorderTop_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BorderTop.DoubleClick
        Me.Top = 0
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height
    End Sub
    Private Sub Border_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeft.MouseDown, BorderRightTop.MouseDown, BorderLeftTop.MouseDown, BorderTop.MouseDown, BorderRight.MouseDown, BorderBottom.MouseDown, BorderRightBottom.MouseDown
        ResizingPoint = e.Location
        'tmrGraphics.Interval = 100
        Me.TransparencyKey = Nothing
    End Sub
    Private Sub BorderLeft_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeft.MouseMove
        If e.Button Then
            Me.Left += -ResizingPoint.X + e.X
            Me.Width -= -ResizingPoint.X + e.X

            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    Private Sub BorderLeftTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeftTop.MouseMove
        If e.Button Then
            Me.Left += -ResizingPoint.X + e.X
            Me.Top += -ResizingPoint.Y + e.Y
            Me.Width -= -ResizingPoint.X + e.X
            Me.Height -= -ResizingPoint.Y + e.Y

            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    Private Sub BorderTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderTop.MouseMove
        If e.Button Then
            Me.Top += -ResizingPoint.Y + e.Y
            Me.Height -= -ResizingPoint.Y + e.Y

            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    'BorderRightBottom
    Private Sub BorderRightTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderRightTop.MouseMove
        If e.Button Then
            Me.Top += -ResizingPoint.Y + e.Y
            Me.Width += -ResizingPoint.X + e.X
            Me.Height -= -ResizingPoint.Y + e.Y
            'BorderRightTop.Left = Me.ClientRectangle.Width - 6

            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    Private Sub BorderRightBottom_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderRightBottom.MouseMove
        If e.Button Then
            'Me.Top += -ResizingPoint.Y + e.Y
            Me.Width += -ResizingPoint.X + e.X
            Me.Height += -ResizingPoint.Y + e.Y
            'BorderRightTop.Left = Me.ClientRectangle.Width - 6

            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    Private Sub BorderRight_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderRight.MouseMove
        If e.Button Then
            Me.Width += -ResizingPoint.X + e.X
            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    Private Sub BorderBottom_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderBottom.MouseMove
        If e.Button Then
            Me.Height += -ResizingPoint.Y + e.Y
            ResizeBorder()
            'ResizeCustom()
            Me.Refresh()
        End If
    End Sub
    Private Sub BorderAny_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeft.MouseUp, BorderRightTop.MouseUp, BorderLeftTop.MouseUp, BorderTop.MouseUp, BorderRight.MouseUp, BorderBottom.MouseUp, BorderRightBottom.MouseUp
        'tmrGraphics.Interval = 10
        ResizeAll()
        Me.TransparencyKey = Color.Magenta
    End Sub
#End Region
#Region "SurfacePageUpPageDown"
    Dim SurfaceDest As Long = 1
    Dim SurfaceFly As Boolean
    Private Sub PageDown()
        If Not SurfaceFly Then SurfaceDest = Surface
        SurfaceDest = SurfaceDest - Int(ImagesBox.Height / img_wire.Y) * img_wire.Y
        SurfaceFly = True
    End Sub
    Private Sub PageUp()
        If Not SurfaceFly Then SurfaceDest = Surface
        SurfaceDest = SurfaceDest + Int(ImagesBox.Height / img_wire.Y) * img_wire.Y
        If SurfaceDest > 0 Then SurfaceDest = 0
        SurfaceFly = True
    End Sub
#End Region


    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadThumbsInfo()
        ImagesBox.ResizeEnded()

        sbView.b3 = New SolidBrush(Color.FromArgb(240, 240, 240))
        sbView.max = 200
        sbView.value = 30
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ImagesBox.ClearImages()
    End Sub


    Private Sub Button2_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            ImagesBox.StopLoading = True
            ImagesBox.DrawShadow = False
            ImagesBox.ShowImagesName = True
            ImagesBox.Wire.Border = 5
            ImagesBox.ClearImages()

            For i As Long = 1 To FileTags.N
                If (FileTags.Tags(i).Rating >= 1 And IO.File.Exists(FileTags.Files(i))) Then
                    ImagesBox.AddImage(FileTags.Files(i))
                End If
                If i Mod 100 = 0 Then Button2.Text = "All rated music ( " + Math.Round(i / FileTags.N * 100).ToString + "% )" : Button2.Refresh()
            Next
            Button2.Text = "All rated music"
            Me.Text = "all rated music"

            ImagesBox.SetImagesLocation()
            ImagesBox.OrderImages()

            ImagesBox.MakeAllThumbnails()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            ImagesBox.StopLoading = True
            'files = IO.Directory.GetFiles(DirsBox.Path)

            'ImagesBox.SetWire(txtW.Text, txtH.Text, 8, 8)
            'ImagesBox.BGColor = Color.FromArgb(bar_bg_color.Value, bar_bg_color.Value, bar_bg_color.Value)
            ImagesBox.DrawShadow = False
            ImagesBox.ShowImagesName = True
            ImagesBox.Wire.Border = 5
            ImagesBox.ClearImages()

            For i As Long = 1 To FileTags.N
                If (FileTags.Tags(i).Type = "music" And IO.File.Exists(FileTags.Files(i))) Then
                    ImagesBox.AddImage(FileTags.Files(i))
                End If
                If i Mod 100 = 0 Then Button3.Text = "All music ( " + Math.Round(i / FileTags.N * 100).ToString + "% )" : Button3.Refresh()
            Next
            Button3.Text = "All music"
            Me.Text = "all music"

            ImagesBox.SetImagesLocation()
            ImagesBox.OrderImages()

            ImagesBox.MakeAllThumbnails()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub DirsBox_SendFocusToTheBottom(ByRef Done As System.Boolean)

    End Sub

    Private Sub ImagesBox_SendFocusToTheTop(ByRef Done As System.Boolean) Handles ImagesBox.SendFocusToTheTop

    End Sub

    Private Sub UcCheckBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UcCheckBox1.Load

    End Sub

    Private Sub UcCheckBox1_ValueChanged(ByVal Value As Boolean) Handles UcCheckBox1.ValueChanged
        Me.TopMost = UcCheckBox1.value
    End Sub

    Private Sub BorderLeftBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderLeftBottom.Click

    End Sub

    Private Sub BorderLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderLeft.Click

    End Sub

    Private Sub BorderTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderTop.Click

    End Sub

    Private Sub BorderRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderRight.Click

    End Sub

    Private Sub BorderRightTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderRightTop.Click

    End Sub

    Private Sub BorderBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderBottom.Click

    End Sub

    Private Sub BorderLeftTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderLeftTop.Click

    End Sub

    Private Sub BorderRightBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BorderRightBottom.Click

    End Sub

    Private Sub sbView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbView.Load

    End Sub

    Private Sub sbView_ValueChanged(ByVal Value As Long) Handles sbView.ValueChanged
        Dim prev_y As Long = ImagesBox.Wire.Y
        If Value < 100 Then
            If Value >= 50 Then
                ImagesBox.SetWire(Value * 4, 32, 5, 5)
            Else
                ImagesBox.SetWire((Value + 50) * 6 - 150, 16, 5, 5)
            End If
        Else
            Dim t As Long = (Value - 100) * 3 + 60
            ImagesBox.SetWire(t, t, 10, 10)
        End If
        If prev_y <> ImagesBox.Wire.Y Then
            ImagesBox.ReloadAllThumbs()
        Else
            ImagesBox.ReloadWidthWithText = True
        End If
        ImagesBox.OrderImages()

        ImagesBox.Select()
    End Sub
End Class
