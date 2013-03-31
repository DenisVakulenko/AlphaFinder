Public Class frmMain
    'Private Const CS_DROPSHADOW As Integer = 131072
    '' Override the CreateParams property
    'Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
    '    Get
    '        Dim cp As CreateParams = MyBase.CreateParams
    '        cp.ClassStyle = cp.ClassStyle Or CS_DROPSHADOW
    '        Return cp
    '    End Get
    'End Property

    Dim Resizing As Boolean

    Private Sub F5()
        'FlyMode = "one-point"
        refresh_files()
    End Sub
    Private Sub F5_Up()
        'FlyMode = "zoom"
        refresh_files()
    End Sub

    Public Sub refresh_files()
        Dim str As String = DirsBox.Path
        'MsgBox(str)
        If Mid(str, str.Length) = "\" Then str = Mid(str, 1, str.Length - 1)
        While InStr(str, "\") > 0
            str = Mid(str, InStr(str, "\") + 1)
        End While
        If str.Length = 2 Then str += "\"
        Me.Text = str

        Dim name As String
        'MsgBox("YEP")
        If DirsBox.Path <> "home" Then
            Try
                ImagesBox.StopLoading = True
                'files = IO.Directory.GetFiles(DirsBox.Path, "*.*", IO.SearchOption.AllDirectories)
                Dim files As Collections.ObjectModel.ReadOnlyCollection(Of String)
                If cbSID.value Then
                    files = FileIO.FileSystem.GetFiles(DirsBox.Path, 3, "*.*")
                Else
                    files = FileIO.FileSystem.GetFiles(DirsBox.Path, 2, "*.*")
                End If
                

                With ImagesBox
                    .SetWire(txtW.Text, txtH.Text, 5, 5)
                    .BGColor = Color.FromArgb(sbBGColor.value, sbBGColor.value, sbBGColor.value)
                    .DrawShadow = cbShadow.value
                    .ShowImagesName = cbNames.value
                    '.FrameSize = txtBorder.Text
                    .ClearImages()
                    .Path = DirsBox.Path
                End With
                Dim WasMusic, WasImage, WasSmthElse As Boolean
                WasImage = False
                WasMusic = False
                WasSmthElse = False

                If cbDirs.value And FilesFilter.value = 0 Then
                    Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    dirs = FileIO.FileSystem.GetDirectories(DirsBox.Path)
                    For Each name In dirs
                        ImagesBox.AddImage(name)
                        ImagesBox.Image(ImagesBox.NImages).Type = "folder"
                    Next
                End If

                For Each name In files
                    If InStr(" mp3 flac ", " " + LCase(GetFileExtention(name)) + " ") Then
                        If FilesFilter.value = 0 Or FilesFilter.value = 2 Then ImagesBox.AddImage(name)
                        WasMusic = True
                    ElseIf InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(name)) + " ") Then
                        If FilesFilter.value = 0 Or FilesFilter.value = 1 Then ImagesBox.AddImage(name)
                        WasImage = True
                    Else
                        If FilesFilter.value = 0 Then ImagesBox.AddImage(name)
                        WasSmthElse = True
                    End If
                Next
                If ImagesBox.NImages = 0 And FilesFilter.value = 0 Then
                    If cbDirs.value Then
                        Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                        dirs = FileIO.FileSystem.GetDirectories(DirsBox.Path)
                        For Each name In dirs
                            ImagesBox.AddImage(name)
                            ImagesBox.Image(ImagesBox.NImages).Type = "folder"
                        Next
                    End If
                    For Each name In files
                        ImagesBox.AddImage(name)
                    Next
                    If ImagesBox.NImages > 0 Then FilesFilter.value = 0
                End If 'Else
                If WasImage And WasMusic = False And WasSmthElse = False Then
                    If FilesFilter.value <> 1 Then
                        FilesFilter.value = 1

                        ImagesBox.Arrangement = "gorizontal"
                        ImagesBox.SetWire(100, 100, 10, 10)
                    End If
                ElseIf WasMusic And WasImage = False And WasSmthElse = False Then
                    If FilesFilter.value <> 1 Then
                        FilesFilter.value = 2

                        ImagesBox.Arrangement = "vertical"
                        ImagesBox.SetWire(100, 16, 5, 5)
                    End If
                Else
                    FilesFilter.value = 0
                End If
                'End If
                If FilesFilter.value = 2 Then
                    ImagesBox.SortImagesByName()
                    ImagesBox.SortImagesBySinger()
                End If
                If FilesFilter.value = 0 Then
                    ImagesBox.SortImagesByType()
                End If
                'End If

                'With FilesBox
                '    .SetImagesLocation()
                '    .OrderImages()
                '    .MakeAllThumbnails()
                'End With

                ImagesBox.MakeAllThumbnails()
                ImagesBox.SetImagesLocation()
                ImagesBox.OrderImages()

                If Not cbDirs.value Then
                    If DirsBox.NDirs <= 0 Then
                        HideDirsBox() : ImagesBox.Select()
                    Else
                        ShowDirsBox()
                    End If
                End If
                'ImagesBox.SetWire(ImagesBox.Wire.X, txtH.Text, 5, 5)
                ImagesBox.SetCanvas()
            Catch ex As Exception
                MsgBox("err!")
            End Try
        Else
            ImagesBox.ClearImages()
            ShowDirsBox()
        End If
    End Sub

    Public Sub refine_files()
        Dim str As String = DirsBox.Path
        If Mid(str, str.Length) = "\" Then str = Mid(str, 1, str.Length - 1)
        While InStr(str, "\") > 0
            str = Mid(str, InStr(str, "\") + 1)
        End While
        If str.Length = 2 Then str += "\"
        Me.Text = str

        Dim name As String
        'MsgBox("YEP")
        If DirsBox.Path <> "home" Then
            Try
                ImagesBox.StopLoading = True
                'files = IO.Directory.GetFiles(DirsBox.Path, "*.*", IO.SearchOption.AllDirectories)
                Dim files As Collections.ObjectModel.ReadOnlyCollection(Of String)
                If cbSID.value Then
                    files = FileIO.FileSystem.GetFiles(DirsBox.Path, 3, "*.*")
                Else
                    files = FileIO.FileSystem.GetFiles(DirsBox.Path, 2, "*.*")
                End If

                With ImagesBox
                    .SetWire(txtW.Text, txtH.Text, 5, 5)
                    .BGColor = Color.FromArgb(sbBGColor.value, sbBGColor.value, sbBGColor.value)
                    .DrawShadow = cbShadow.value
                    .ShowImagesName = cbNames.value
                    '.FrameSize = txtBorder.Text
                    .ClearImages()
                    .Path = DirsBox.Path
                End With
                Dim WasMusic, WasImage, WasSmthElse As Boolean
                WasImage = False
                WasMusic = False
                WasSmthElse = False

                If cbDirs.value Then
                    Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    dirs = FileIO.FileSystem.GetDirectories(DirsBox.Path)
                    For Each name In dirs
                        ImagesBox.AddImage(name)
                        ImagesBox.Image(ImagesBox.NImages).Type = "folder"
                    Next
                End If

                For Each name In files
                    If InStr(" mp3 flac ", " " + LCase(GetFileExtention(name)) + " ") Then
                        If FilesFilter.value = 0 Or FilesFilter.value = 2 Then ImagesBox.AddImage(name)
                        WasMusic = True
                    ElseIf InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(name)) + " ") Then
                        If FilesFilter.value = 0 Or FilesFilter.value = 1 Then ImagesBox.AddImage(name)
                        WasImage = True
                    Else
                        If FilesFilter.value = 0 Then ImagesBox.AddImage(name)
                        WasSmthElse = True
                    End If
                Next
                If FilesFilter.value = 2 Then
                    ImagesBox.SortImagesByName()
                    ImagesBox.SortImagesBySinger()
                End If
                ImagesBox.SetImagesLocation()
                ImagesBox.OrderImages()
                'End If

                'With FilesBox
                '    .SetImagesLocation()
                '    .OrderImages()
                '    .MakeAllThumbnails()
                'End With

                ImagesBox.MakeAllThumbnails()
                If DirsBox.NDirs <= 0 Then
                    HideDirsBox() : ImagesBox.Select()
                Else
                    ShowDirsBox()
                End If
            Catch ex As Exception
                MsgBox("err!")
            End Try
        Else
            ImagesBox.ClearImages()
            ShowDirsBox()
        End If
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Visible = False
        SaveThumbsInfo()
        FileTags.Save()
    End Sub

    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F5
                F5_Up()
                btnReload.btnDown()
            Case Keys.Escape
                ImagesBox.StopLoading = True
                pnl_settings.Visible = False
                pnl_35photo.Visible = False
            Case Keys.E
                If e.Control Then
                    pnl_settings.Visible = True
                    txtW.Select()
                End If
            Case Keys.P
                If e.Control Then PathLine.MakeTextMode()
            Case Keys.Q
                If e.Control Then
                    If picResizeDest = 31 Then
                        'picResize.Top = DirsBoxHeight : ResizeAll() : DirsBox.Select()
                        ShowDirsBox()
                    Else
                        If tmrDirsBoxAnimation.Enabled = False And picResize.Top > DirsBox.Top + DirsBox.Wire.Y + DirsBox.Wire.dY Then DirsBoxHeight = picResize.Top
                        HideDirsBox()
                    End If
                End If
            Case Keys.D
                If e.Control And e.Shift Then
                    Cursor = Cursors.WaitCursor
                    cbSID.value = True
                    refresh_files()
                    cbSID.value = False
                    Cursor = Cursors.Arrow
                End If
        End Select
    End Sub
    Private Sub frmMain_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        btnReload.btnNormal()
        btnUp.btnNormal()
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
        DirsBox.Width = Me.ClientRectangle.Width - 12
        If picResize.Top > 31 + 5 + 6 Then DirsBox.Height = picResize.Top - PathLine.Height - 5 - 6 Else DirsBox.Height = 0
        ImagesBox.Top = picResize.Top + picResize.Height
        ImagesBox.Width = Me.ClientRectangle.Width - 12
        ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - 6
        picResize.Width = DirsBox.Width

        ResizeBorder()

        btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - 6 + 2
        btnMinimize.Left = Me.ClientRectangle.Width - btnClose.Left - btnMinimize.Width - 4
        'ImagesBox.Size = ImagesBox.Size
        'ImagesBox.Left = ImagesBox.Left
        'ImagesBox.Top = ImagesBox.Top
        'ImagesBox.FitImagesLocation()

        On Error Resume Next
        Me.Refresh()
    End Sub
    Private Sub ResizeAll()
        DirsBox.Width = Me.ClientRectangle.Width - 12
        If picResize.Top > 31 + 5 + 6 Then DirsBox.Height = picResize.Top - PathLine.Height - 5 - 6 Else DirsBox.Height = 0
        picResize.Width = DirsBox.Width

        ResizeBorder()

        btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - 6
        btnClose.Top = 6
        btnMinimize.Left = btnClose.Left - btnMinimize.Width - 5
        btnMinimize.Top = 6

        With ImagesBox
            .Left = 6
            .Top = picResize.Top + picResize.Height
            .Height = Me.ClientRectangle.Height - picResize.Top - picResize.Height - 6
            .Width = Me.ClientRectangle.Width - 12
        End With
        'With FilesBox
        '    .Left = ImagesBox.Left
        '    .Top = ImagesBox.Top
        '    .Height = ImagesBox.Height
        '    .Width = ImagesBox.Width
        'End With

        ImagesBox.OrderImages()

        Me.Refresh()
    End Sub
#End Region

    Private Sub LoadButtons()
        btnUp.LoadImages("buttons\up.bmp", "buttons\up_mm.bmp", "buttons\up_down.bmp", "") ', "buttons\selection.gif")
        btnReload.LoadImages("buttons\reload.bmp", "buttons\reload_mm.bmp", "buttons\reload_down.bmp", "") ', "buttons\selection.gif")
        btnSettings.LoadImages("buttons\settings.bmp", "buttons\settings_mm.bmp", "buttons\settings_down.bmp", "") ', "buttons\selection.gif")
        btnOnlyMusic.LoadImages("buttons\mus.bmp", "buttons\mus_mm.bmp", "buttons\mus_down.bmp", "") '"buttons\stop_selection.bmp")
        btnOnlyImg.LoadImages("buttons\img.bmp", "buttons\img_mm.bmp", "buttons\img_down.bmp", "") '"buttons\stop_selection.bmp")
        btnSid.LoadImages("buttons\sid.bmp", "buttons\sid_mm.bmp", "buttons\sid_down.bmp", "")
        'btnClose1.LoadImages("buttons\close.bmp", "buttons\close_mm.bmp", "buttons\close_down.bmp", "")
        btnClose2.LoadImages("buttons\close.bmp", "buttons\close_mm.bmp", "buttons\close_down.bmp", "")
        btnClose1.LoadImages("buttons\close2.bmp", "buttons\close2_mm.bmp", "buttons\close2_down.bmp", "")
        btnTempArea.LoadImages("buttons\temp.bmp", "buttons\temp_mm.bmp", "buttons\temp_down.bmp", "")
        btnClose.LoadImages("buttons\close2.bmp", "buttons\close2_mm.bmp", "buttons\close2_down.bmp", "") ', "buttons\close_selection.bmp")
        btnMinimize.LoadImages("buttons\minim.bmp", "buttons\minim_mm.bmp", "buttons\minim_down.bmp", "") ', "buttons\close_selection.bmp")
        btn35photo.LoadImages("buttons\35photo.bmp", "buttons\35photo_mm.bmp", "buttons\35photo_down.bmp", "") ', "buttons\selection.gif")
        btnLoad35photo.LoadImages("buttons\load.bmp", "buttons\load_mm.bmp", "buttons\load_down.bmp", "")
    End Sub
    Public StartupPath = "home"
    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            LoadBGPicturies()
            pnl_settings.BackgroundImage = BG_Settings
            pnl_settings.Size = BG_Settings.Size
            pnl_35photo.BackgroundImage = BG_35photo
            pnl_35photo.Size = BG_35photo.Size

            DirsBox.Init()
            DirsBox.NewPath(StartupPath, "")

            LoadButtons()
            LoadBorders()

            ResizeAll() '----

            ImagesBox.LoadUIImages()
            'FilesBox.LoadUIImages()

            LoadThumbsInfo()
            FileTags.Load()
            DirTags.Load()

            If StartupPath <> "home" Then
                PathLine.NewMaxPath(StartupPath)
                refresh_files()
            End If
        Catch ex As Exception
            MsgBox("Ooops!  " + ex.ToString)
        End Try
    End Sub

    Dim picResizeY As Long
    Dim DirsBoxHeight As Short
    Private Sub picResize_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles picResize.DoubleClick
        If picResize.Top = 31 Then
            ShowDirsBox()
        Else
            HideDirsBox()
        End If
    End Sub
    Private Sub picResize_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picResize.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ChangePicResizeSize()
        Else
            If picResize.Top > DirsBox.Top + DirsBox.Wire.Y + DirsBox.Wire.dY Then DirsBoxHeight = picResize.Top
            picResizeY = e.Y
            ImagesBox.ResizeStarted()
            'FilesBox.ResizeStarted()
            Resizing = True
        End If
    End Sub
    Private Sub picResize_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picResize.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            Dim NewPRTop = picResize.Top - (picResizeY - e.Y)
            If NewPRTop < 31 Then NewPRTop = 31
            If NewPRTop > 31 + 5 + 6 Then DirsBox.Height = NewPRTop - PathLine.Height - 5 - 6 Else DirsBox.Height = 0
            picResize.Top = NewPRTop

            Dim H As Integer = Me.ClientRectangle.Height - picResize.Top - picResize.Height - 6
            ImagesBox.Resizing(ImagesBox.Width, H)
            ImagesBox.Top = picResize.Top + picResize.Height
            ImagesBox.Refresh()
            'FilesBox.Resizing(FilesBox.Width, H)
            'FilesBox.Top = picResize.Top + picResize.Height
            'FilesBox.Refresh()


            picResize.Refresh()
            DirsBox.Refresh()
            BorderBottom.Refresh()
        End If
    End Sub
    Private Sub picResize_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picResize.MouseUp
        If picResize.Top > DirsBox.Top + DirsBox.Wire.Y + DirsBox.Wire.dY Then
            DirsBoxHeight = picResize.Top
        ElseIf tmrDirsBoxAnimation.Enabled <> True Then
            HideDirsBox()
        End If

        If tmrDirsBoxAnimation.Enabled = False Then
            ImagesBox.ResizeEnded()
            'FilesBox.ResizeEnded()
            Resizing = False
            ResizeAll()
        End If
    End Sub

#Region "35photo"
    Dim bar_bg_color_fly As Long = 140
    Private Sub btnLoad35photo_Click() Handles btnLoad35photo.Click
        'Button1.Enabled = False : Button1.Text = "wait" : Button1.Refresh()
        'Timer1.Enabled = False : tmrGraphics.Enabled = False

        Dim txt_url As String = ""

        Try
            If IO.File.Exists(Application.StartupPath + "\35photo" + txtPage.Text + "\main.txt") Then IO.File.Delete(Application.StartupPath + "\35photo" + txtPage.Text + "\main.txt")
            If txtPage.Text = "1" Then
                My.Computer.Network.DownloadFile("http://35photo.ru/new/", Application.StartupPath + "\35photo" + txtPage.Text + "\main.txt", False, 1000)
            Else
                My.Computer.Network.DownloadFile("http://35photo.ru/new/list_" + txtPage.Text + "/", Application.StartupPath + "\35photo" + txtPage.Text + "\main.txt", False, 1000)
            End If

            Dim htm, adress, url As String, i, start As Long
            htm = IO.File.ReadAllText(Application.StartupPath + "\35photo" + txtPage.Text + "\main.txt")

            Dim pattern1 As String
            '<img class="prevr2" 
            ' <img class="photo_preview3" 
            'pattern = "<div class=" + Chr(34) + "photos_div2" + Chr(34) + ">"'pattern = "<img class=" + Chr(34) + "prevr2" + Chr(34)
            '"<img nude=" + Chr(34) + "1" + Chr(34) + " class=" + Chr(34) + "prevr2" + Chr(34)

            pattern1 = "<td valign=" + Chr(34) + "top" + Chr(34) + " width=" + Chr(34) + "25%" + Chr(34) + ">" ' "<img alt=" + Chr(34) + Chr(34) + " class=" + Chr(34) + "photo_preview3" + Chr(34)


            i = 1
            For ii As Long = 1 To 13 * 3
                picProgress.Image = GenerateStatusBar(picProgress.Width, ii / 39)
                While i < htm.Length - pattern1.Length And (Mid(htm, i, pattern1.Length) <> pattern1) 'And Mid(htm, i, pattern2.Length) <> pattern2)
                    i += 1
                End While
                If i < htm.Length Then
                    While i < htm.Length And Mid(htm, i, 9) <> "<a href=" + Chr(34) : i += 1 : End While
                    start = i + 9 : i += 10
                    While (i < htm.Length And Mid(htm, i, 1) <> Chr(34)) : i += 1 : End While
                    url = Mid(htm, start, i - start)
                    txt_url += url + vbNewLine
                    'MsgBox(url)

                    While i < htm.Length And Mid(htm, i, 5) <> "src=" + Chr(34) : i += 1 : End While
                    start = i + 5 : i += 8
                    While i < htm.Length And Mid(htm, i, 1) <> Chr(34) : i += 1 : End While
                    adress = Mid(htm, start, i - start)
                    'MsgBox(adress)

                    'Button1.Text = "photo " + ii.ToString : Button1.Refresh()
                    'MsgBox(start.ToString + " " + htm.Length.ToString)
                    If ii < 10 Then
                        Loadfile(adress, Application.StartupPath + "\35photo" + txtPage.Text + "\0" + ii.ToString + ".jpg", ii)
                    Else
                        Loadfile(adress, Application.StartupPath + "\35photo" + txtPage.Text + "\" + ii.ToString + ".jpg", ii)
                    End If

                    Application.DoEvents()
                Else
                    Exit For
                End If
            Next

            FileOpen(1, Application.StartupPath + "\35photo" + txtPage.Text + "\urls.txt", OpenMode.Output)
            Print(1, txt_url)
            FileClose(1)

            'Button1.Text = "done" : Button1.Refresh()
        Catch ex As Exception
            MsgBox(ex.ToString)
            'Button1.Text = "no connection" : Button1.Refresh()
        End Try

        bar_bg_color_fly = 60
        'txtBorder.Text = "0"

        txtW.Text = "240"
        txtH.Text = "240"
        ImagesBox.SetWire(240, 240, 20, 20)

        PathLine.NewMaxPath(Application.StartupPath + "\35photo" + txtPage.Text + "\")
        DirsBox.NewPath(PathLine.MaxPath, PathLine.MaxPath)

        'FlyMode = "one-point"
        'ClearThumbs()
        refresh_files()

        pnl_35photo.Visible = False
    End Sub

    '    '        alternative downloading
    '    'Dim wc As New Net.WebClient 'Создаём WebClient   
    '    ''Создаём поток и BinaryWriter для записи данных в файл   
    '    'Dim fs As New IO.FileStream("35photo\main.txt", IO.FileMode.Create)
    '    'Dim bw As New IO.BinaryWriter(fs)
    '    'Dim b() As Byte
    '    ''Копируем файл в байтовый массив   
    '    'b = wc.DownloadData("http://35photo.ru/")
    '    ''Пишем байтовый массив в FileStream   
    '    'bw.Write(b)
    '    ''Закрываем объекты   
    '    'bw.Close()
    '    'fs.Close()


    '    ''Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    ''    Button1.Text = "wait" : Button1.Refresh()

    '    ''    IO.File.Delete("35photo\main.txt")
    '    ''    'My.Computer.Network.DownloadFile("http://35photo.ru/", "35photo\main.txt", False, 650)
    '    ''    Try
    '    ''        My.Computer.Network.DownloadFile("http://35photo.ru/new/", "35photo\main.txt", False, 650)

    '    ''        'FIRST
    '    ''        Dim htm, adress As String, i, start As Long
    '    ''        htm = IO.File.ReadAllText("35photo\main.txt")

    '    ''        Dim pattern1 As String
    '    ''        Dim pattern2 As String
    '    ''        'pattern = "<div class=" + Chr(34) + "photos_div2" + Chr(34) + ">"
    '    ''        pattern1 = Chr(34) + "photo_preview3" + Chr(34) '"<img alt=" + Chr(34) + Chr(34) + " class=" + Chr(34) + "photo_preview3" + Chr(34)
    '    ''        pattern2 = "<img nude=" + Chr(34) + "1" + Chr(34) + " class=" + Chr(34) + "prevr2" + Chr(34)
    '    ''        'pattern = "<img class=" + Chr(34) + "prevr2" + Chr(34)

    '    ''        i = 1
    '    ''        For ii As Long = 1 To 13 * 3
    '    ''            While i < htm.Length - pattern2.Length And (Mid(htm, i, pattern1.Length) <> pattern1 And Mid(htm, i, pattern2.Length) <> pattern2)
    '    ''                i += 1
    '    ''            End While
    '    ''            If i <> htm.Length Then
    '    ''                While i < htm.Length And Mid(htm, i, 5) <> "src=" + Chr(34) : i += 1 : End While
    '    ''                start = i + 5 : i += 6
    '    ''                While i < htm.Length And Mid(htm, i, 1) <> Chr(34) : i += 1 : End While
    '    ''                adress = Mid(htm, start, i - start)
    '    ''                Button1.Text = "photo " + ii.ToString : Button1.Refresh()

    '    ''                Loadfile(adress, "35photo\" + ii.ToString + ".jpg")

    '    ''                Application.DoEvents()
    '    ''            Else
    '    ''                Exit For
    '    ''            End If
    '    ''        Next
    '    ''        Button1.Text = "done" : Button1.Refresh()
    '    ''    Catch ex As Exception
    '    ''        Button1.Text = "no connection" : Button1.Refresh()
    '    ''    End Try
    '    ''    bar_bg_color.Value = 60
    '    ''    txtBorder.Text = "0"

    '    ''    'txtPath.Text = Mid(Application.ExecutablePath, 1, Application.ExecutablePath.Length - 15) + "\35photo" '!!!!!!!!!!!!!!!!!!!!

    '    ''    Timer1.Enabled = False
    '    ''    tmrGraphics.Enabled = False
    '    ''    refresh_files()
    '    ''    'refresh_dirs()

    '    ''    img_wire.X = 260
    '    ''    img_wire.Y = img_wire.X
    '    ''    img_wire.Width = img_wire.X - 10
    '    ''    img_wire.Height = img_wire.Width

    '    ''    'draw_folders()

    '    ''    StopFlying()
    '    ''    refresh_picturies()
    '    ''    draw_picturies()
    '    ''    tmrGraphics.Enabled = True
    '    ''    Timer1.Enabled = True
    '    ''    '<img class="prevr2" 
    '    ''    ' <img class="photo_preview3" 
    '    ''End Sub

    '    '####################################

    Public Sub Loadfile(ByVal url As String, ByVal FilePath As String, ByVal n As Long)
        Dim gl_start_time As DateTime
        Dim gl_stop_time As DateTime
        Dim gl_elapsed_time As TimeSpan
        Try
            Label1.Text = "( " + n.ToString + " | 39 ) Запрос" : Application.DoEvents()
            'ProgressBar1.Value = 0

            Dim file As String = url
            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(file)
            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            Dim stream As System.IO.Stream = response.GetResponseStream()

            Dim length As Integer = response.ContentLength
            'ProgressBar1.Maximum = length

            Dim bytes(length) As Byte
            gl_start_time = Now
            For i As Integer = 0 To length - 1
                bytes(i) = stream.ReadByte()

                If i Mod 500 = 0 Then
                    gl_stop_time = Now
                    gl_elapsed_time = gl_stop_time.Subtract(gl_start_time)
                    Label1.Text = "( " + n.ToString + " | 39 ) Скорость " + CStr(Math.Round((i / 1024) / gl_elapsed_time.TotalSeconds, 0)) + " Кб/с. "
                    picSpeed.Image = GenerateStatusBar(picSpeed.Width, i / length)
                    Application.DoEvents()
                End If
            Next
            'ProgressBar1.Value = ProgressBar1.Maximum
            Label1.Text = "( " + n.ToString + " | 39 ) Сохранение" : Application.DoEvents()

            '--Записывам в файл--
            Using output As IO.Stream = System.IO.File.Create(FilePath)
                output.Write(bytes, 0, bytes.Length)
            End Using
            gl_stop_time = Now
            gl_elapsed_time = gl_stop_time.Subtract(gl_start_time)
            Label1.Text = CStr(Math.Round((length - 1) / 1024, 2)) + " Кб. за " + CStr(Math.Round(gl_elapsed_time.TotalSeconds, 1)) + " с." : Application.DoEvents()
            '##Записывам в файл##
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Ошибка")
        End Try
    End Sub
#End Region



    Private Sub txtW1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtW.KeyUp
        Dim w As Short = Val(txtW.Text)
        Dim h As Short = Val(txtH.Text)
        If w > 50 And h >= 16 Then ImagesBox.SetWire(w, h, 5, 5)
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()
    End Sub
    Private Sub txtH1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtH.KeyUp
        Dim w As Short = Val(txtW.Text)
        Dim h As Short = Val(txtH.Text)
        If w > 50 And h >= 16 Then ImagesBox.SetWire(w, h, 5, 5)
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim a() As System.Diagnostics.Process
        a = Process.GetProcesses()

        For i As Long = 0 To a.Length - 1
            If a(i).MainWindowTitle <> "" Then MsgBox(a(i).ProcessName + "     " + a(i).MainWindowTitle)
        Next
        'Dim b As Devices.Mouse
        'b.WheelScrollLines
    End Sub


#Region "Buttons events"
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        NThumbs = 0
        SaveThumbsInfo()
    End Sub
    Private Sub btnUp_Click() Handles btnUp.Click
        DirsBox.FolderUp()
    End Sub
    Private Sub btnReload_Click() Handles btnReload.Click
        F5_Up()
    End Sub
    Private Sub btnSettings_Click() Handles btnSettings.Click
        pnl_settings.Visible = True
        txtW.Select()
    End Sub
    Private Sub btnClose1_Click() Handles btnClose1.Click
        pnl_settings.Visible = False
    End Sub
    Private Sub btn35photo_Click() Handles btn35photo.Click
        pnl_35photo.Visible = True
    End Sub
    Private Sub btnClose2_Click() Handles btnClose2.Click
        pnl_35photo.Visible = False
    End Sub
    Private Sub btnClose_Click() Handles btnClose.Click
        Me.Close()
    End Sub
#End Region
#Region "Borders"
    Private Sub LoadBorders()
        Dim MyPath As String = Application.StartupPath + "\resources\"
        BorderTop.Image = Image.FromFile(MyPath + "top.bmp")
        BorderLeft.Image = Image.FromFile(MyPath + "left.bmp")
        BorderRight.Image = Image.FromFile(MyPath + "right.bmp")
        BorderBottom.Image = Image.FromFile(MyPath + "bottom.bmp")
        BorderLeftTop.Image = Image.FromFile(MyPath + "left-top.gif")
        BorderRightTop.Image = Image.FromFile(MyPath + "right-top.gif")
        BorderLeftBottom.Image = Image.FromFile(MyPath + "left-btm.gif")
        BorderRightBottom.Image = Image.FromFile(MyPath + "right-btm.gif")
    End Sub
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
    Private Sub Me_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button Then
            Me.Left += -frmPoint.X + e.X
            Me.Top += -frmPoint.Y + e.Y

            If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
                Me.Size = FrmRect.Size
                frmPoint.X = Me.Width / 2
                SetNormalBorders()
            End If
        End If
    End Sub
#End Region
#Region "Resizing"
    Dim ResizingPoint As Point
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


    Private Sub DirsBox_DirChanged(ByVal Up As Boolean) Handles DirsBox.DirChanged
        If Up Then
            F5_Up()
        Else
            F5()
        End If
        PathLine.NewMaxPath(DirsBox.Path)
    End Sub

    Private Sub PathLine_PathChaged(ByVal Up As Boolean) Handles PathLine.PathChaged
        DirsBox.NewPath(PathLine.MaxPath, PathLine.MaxPath)
        If Up Then
            F5_Up()
        Else
            F5()
        End If
        DirsBox.Focus()
    End Sub



    Dim picResizeDest As Integer = 31
    Private Sub HideDirsBox()
        'Dim H As Integer = Me.ClientRectangle.Height - 31 - picResize.Height - 6
        ImagesBox.ResizeStarted() 'ImagesBox.Width, H)
        Resizing = True
        picResizeDest = 31
        If ImagesBox.Visible = True Then ImagesBox.Select() 'Else FilesBox.Select()
        tmrDirsBoxAnimation.Enabled = True
    End Sub
    Private Sub ShowDirsBox()
        'Dim H As Integer = Me.ClientRectangle.Height - DirsBoxHeight - picResize.Height - 6
        ImagesBox.ResizeStarted() 'ImagesBox.Width, H)
        Resizing = True
        picResizeDest = DirsBoxHeight
        DirsBox.Select()
        tmrDirsBoxAnimation.Enabled = True
    End Sub
    Private Sub tmrDirsBoxAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDirsBoxAnimation.Tick
        If picResize.Top <> picResizeDest Then
            Dim delta As Long = (-picResizeDest + picResize.Top) * 0.21 + Math.Sign(-picResizeDest + picResize.Top)
            Dim NewPRTop As Long = picResize.Top - delta
            If NewPRTop < 31 Then NewPRTop = 31
            If NewPRTop > DirsBoxHeight Then NewPRTop = DirsBoxHeight
            If NewPRTop > 31 + 5 + 6 Then DirsBox.Height = NewPRTop - DirsBox.Top Else DirsBox.Height = 0
            picResize.Top = NewPRTop

            Dim H As Integer = Me.ClientRectangle.Height - picResize.Top - picResize.Height - 6
            If ImagesBox.Visible = True Then
                ImagesBox.Resizing(ImagesBox.Width, H)
                ImagesBox.Top = picResize.Top + picResize.Height
                ImagesBox.Refresh()
            Else
                'FilesBox.Resizing(FilesBox.Width, H)
                'FilesBox.Top = picResize.Top + picResize.Height
                'FilesBox.Refresh()
            End If

            picResize.Refresh()
            DirsBox.RedrawComposed()
            DirsBox.Refresh()
            BorderBottom.Refresh()
            'If NewPRTop = picResizeDest Then tmrDirsBoxAnimation.Enabled = False : ImagesBox.ResizeEnded() : FilesBox.ResizeEnded() : Resizing = False : DirsBox.CorrectCanvas() : DirsBox.Redraw()
            If NewPRTop = picResizeDest Then tmrDirsBoxAnimation.Enabled = False : ImagesBox.ResizeEnded() : Resizing = False : DirsBox.CorrectCanvas() : DirsBox.Redraw()
        Else
            'tmrDirsBoxAnimation.Enabled = False : ImagesBox.ResizeEnded() : FilesBox.ResizeEnded() : Resizing = False : DirsBox.CorrectCanvas() : DirsBox.Redraw()
            tmrDirsBoxAnimation.Enabled = False : ImagesBox.ResizeEnded() : Resizing = False : DirsBox.CorrectCanvas() : DirsBox.Redraw()
        End If
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ImagesBox.Left = 6
        ImagesBox.Top = picResize.Top + picResize.Height
        ImagesBox.Width = Me.ClientRectangle.Width - 12
        ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - 6

        'FilesBox.Left = 6
        'FilesBox.Top = picResize.Top + picResize.Height
        'FilesBox.Width = Me.ClientRectangle.Width - 12
        'FilesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - 6

        DirsBoxHeight = picResize.Top
        DirsBox.Width = Me.ClientRectangle.Width - DirsBox.Left * 2
        DirsBox.Height = picResize.Top - PathLine.Height - 5 - 6
        DirsBox.SetWire(76, 59, 5, 5)

        UcScrollBar1.max = 200
        UcScrollBar1.value = 30
        sbBGColor.min = 0
        sbBGColor.max = 255
        sbBGColor.value = 200
        sbBGColor.b3 = New SolidBrush(Color.FromArgb(215, 215, 215))
        sbBorder.min = 0
        sbBorder.max = 20
        sbBorder.value = 0
        sbBorder.b3 = New SolidBrush(Color.FromArgb(215, 215, 215))
        sbBorder.ShowText = True

        sbHeight.b3 = New SolidBrush(Color.FromArgb(215, 215, 215))
        sbWidth.b3 = New SolidBrush(Color.FromArgb(215, 215, 215))

        sbWidth.max = 400
        sbHeight.max = 200
        sbWidth.ShowText = True
        sbHeight.ShowText = True
        'cbAll.Text = "All"
        'cbMusic.Text = "Music"
        'cbImg.Text = "Images"

        ImagesBox.ResizeEnded()
        'FilesBox.ResizeEnded()
    End Sub

    Private Sub ImagesBox_BackSpaceKey() Handles ImagesBox.BackSpaceKey
        DirsBox.FolderUp()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ImagesBox.DrawShadow = cbShadow.value
        ImagesBox.NextFrame(True)
    End Sub

    Private Sub ImagesBox_SendFocusToTheTop(ByRef Done As System.Boolean) Handles ImagesBox.SendFocusToTheTop
        If picResize.Top = 31 Then
            If DirsBox.NDirs > 0 Then ShowDirsBox() Else Done = False
        Else
            DirsBox.Select()
        End If
    End Sub
    Private Sub DirsBox_SendFocusToTheBottom(ByRef Done As Boolean) Handles DirsBox.SendFocusToTheBottom
        'If ImagesBox.Visible = True Then
        If ImagesBox.NImages > 0 Then ImagesBox.Select() : Done = True Else Done = False
        'Else
        '    If FilesBox.NImages > 0 Then FilesBox.Select() : Done = True Else Done = False
        'End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ImagesBox.ShowImagesName = cbNames.value
    End Sub

    Private Sub btnMinimize_Click() Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub btnTempArea_Click() Handles btnTempArea.Click
        Dim NF As New frmTempArea
        NF.Show()
        NF.ImagesBox.Sorting = "user"
        NF.ImagesBox.SetWire(Val(txtW.Text), Val(txtH.Text), 10, 10)
        NF.ImagesBox.Wire.Border = Val(sbBorder.value)
        NF.ImagesBox.BGColor = ImagesBox.BGColor
        NF.Left = Me.Left + btnTempArea.Left - NF.Width / 2
        NF.Top = Me.Top + btnTempArea.Top - 10
        NF.ImagesBox.Arrangement = ImagesBox.Arrangement
    End Sub


    Private Declare Function timeBeginPeriod Lib "winmm.dll" (ByVal uPeriod As Long) As Long
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        timeBeginPeriod(1)
    End Sub

    Sub ChangePicResizeSize()
        If picResize.Height = 5 Then
            picResize.Height = 31
        Else
            picResize.Height = 5
        End If
        ResizeAll()
    End Sub




    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'FilesBox.Visible = Not FilesBox.Visible
        ImagesBox.Visible = Not ImagesBox.Visible
        refresh_files()
    End Sub


    'http://500px.com/popular
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        'Try
        If IO.File.Exists(Application.StartupPath + "\500px\main.txt") Then IO.File.Delete(Application.StartupPath + "\500px\main.txt")
        My.Computer.Network.DownloadFile("http://500px.com/", Application.StartupPath + "\500px\main.txt", False, 1000)

        Dim htm, url As String, i As Long
        htm = IO.File.ReadAllText(Application.StartupPath + "\500px\main.txt")

        Dim pattern1 As String

        pattern1 = "<div id=" + Chr(34) + "new_pic" + Chr(34) + " style=" + Chr(34) + "background: url('"
        i = InStr(htm, pattern1)
        url = Mid(htm, i + pattern1.Length, InStr(Mid(htm, i + pattern1.Length), "'") - 1)
        Loadfile(url, Application.StartupPath + "\500px\" + Mid(url, 23, 6) + ".jpg", 1)

        Dim a As New frmShowPhoto2
        Dim FN(0) As String
        FN(0) = Application.StartupPath + "\500px\" + Mid(url, 23, 6) + ".jpg"
        Dim b As New Bitmap(10, 600)
        a.InitFullScreened(b, 0, FN)
        a.Show()
        pnl_35photo.Visible = False
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If IO.File.Exists(Application.StartupPath + "\500px\main.txt") Then IO.File.Delete(Application.StartupPath + "\500px\main.txt")
        My.Computer.Network.DownloadFile("http://500px.com/popular", Application.StartupPath + "\500px\main.txt", False, 1000)

        Dim htm, url As String, i As Long
        htm = IO.File.ReadAllText(Application.StartupPath + "\500px\main.txt")

        Dim pattern1 As String

        pattern1 = "class=" + Chr(34) + "image" + Chr(34) + "><img src=" + Chr(34)

        i = InStr(htm, pattern1)
        While i <> 0
            url = Mid(htm, i + pattern1.Length, InStr(Mid(htm, i + pattern1.Length), Chr(34)) - 1)
            htm = Mid(htm, i + url.Length)
            Loadfile(url, Application.StartupPath + "\500px\" + Mid(url, 23, 6) + ".jpg", 1)

            Dim a As New frmShowPhoto2
            Dim FN(0) As String
            FN(0) = Application.StartupPath + "\500px\" + Mid(url, 23, 6) + ".jpg"
            Dim b As New Bitmap(10, 600)
            a.InitFullScreened(b, 0, FN)
            a.Show()

            i = InStr(htm, pattern1) 'id="mainphoto" rel="Shadowbox" src=" !!
        End While

        pnl_35photo.Visible = False
    End Sub

    Private Sub chkInDirs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        refresh_files()
    End Sub


    Private Sub UcScrollBar1_ValueChanged(ByVal Value As Long) Handles UcScrollBar1.ValueChanged
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

        txtH.Text = ImagesBox.Wire.Y.ToString
        txtW.Text = ImagesBox.Wire.X.ToString
        ImagesBox.Select()
    End Sub

    'Private Sub frmMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
    '    Me.BackColor = Color.Green
    '    MsgBox("L")
    'End Sub

    'Private Sub frmMain_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
    '    Me.BackColor = Color.FromArgb(200, 200, 200)
    'End Sub

    Private Sub ImagesBox_FillScreenMe() Handles ImagesBox.FillScreenMe
        If picResize.Top = 31 Then
            ShowDirsBox()
        Else
            HideDirsBox()
        End If
    End Sub
    Private Sub UcScrollBar2_ValueChanged(ByVal Value As Long) Handles sbBGColor.ValueChanged
        ImagesBox.BGColor = Color.FromArgb(Value, Value, Value)
        ImagesBox.NextFrame(True)
    End Sub
    Private Sub sbBorder_ValueChanged(ByVal Value As Long) Handles sbBorder.ValueChanged
        ImagesBox.Wire.Border = Val(Value)
        ImagesBox.NextFrame(True)
    End Sub
    Private Sub cbShadow_ValueChanged(ByVal Value As Boolean) Handles cbShadow.ValueChanged
        ImagesBox.DrawShadow = Value
        ImagesBox.NextFrame(True)
    End Sub
    Private Sub cbNames_ValueChanged(ByVal Value As Boolean) Handles cbNames.ValueChanged
        ImagesBox.ShowImagesName = Value
        ImagesBox.ReloadAllThumbs()
    End Sub

    Private Sub btnOnlyMusic_Click() Handles btnOnlyMusic.Click
        'cbAll0.value = False
        'cbMusic0.value = True
        'cbImg0.value = False
        'FilesFilter.value = 2
        ImagesBox.Arrangement = "vertical"
        ImagesBox.Canvas.Y = 0
        If txtH.Text <> 16 Then
            txtH.Text = 16
            txtW.Text = 360
            ImagesBox.SetWire(360, 16, 5, 5)
            UcScrollBar1.value = 35
        Else
            txtH.Text = 32
            txtW.Text = 300
            ImagesBox.SetWire(300, 32, 5, 5)
            UcScrollBar1.value = 75
        End If
        sbBorder.value = 0
        cbShadow.value = False
        cbNames.value = False

        ImagesBox.DrawShadow = False
        ImagesBox.Wire.Border = 0
        ImagesBox.ShowImagesName = False

        ImagesBox.StopLoading = True
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()

        ImagesBox.Select()
    End Sub

    Private Sub btnOnlyImg_Click() Handles btnOnlyImg.Click
        'cbAll0.value = False
        'cbMusic0.value = False
        'cbImg0.value = True
        'FilesFilter.value = 1

        ImagesBox.Arrangement = "gorizontal"
        ImagesBox.Canvas.X = 0
        If txtH.Text <> 100 Then
            txtH.Text = 100
            txtW.Text = 100
            ImagesBox.SetWire(100, 100, 10, 10)
            UcScrollBar1.value = (100 - 60) / 3 + 100
        Else
            txtH.Text = 180
            txtW.Text = 180
            ImagesBox.SetWire(180, 180, 10, 10)
            UcScrollBar1.value = (180 - 60) / 3 + 100
        End If
        sbBorder.value = 2
        cbShadow.value = True
        cbNames.value = False

        ImagesBox.DrawShadow = True
        ImagesBox.Wire.Border = 2
        ImagesBox.ShowImagesName = False

        ImagesBox.StopLoading = True
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()

        ImagesBox.Select()
    End Sub



    Private Sub sbWidth_ValueChanged(ByVal Value As Long) Handles sbWidth.ValueChanged
        txtW.Text = Value.ToString
        Dim w As Short = Val(txtW.Text)
        Dim h As Short = Val(txtH.Text)
        ImagesBox.SetWire(w, h, 5, 5)
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()
    End Sub

    Private Sub sbHeight_ValueChanged(ByVal Value As Long) Handles sbHeight.ValueChanged
        txtH.Text = Value
        Dim w As Short = Val(txtW.Text)
        Dim h As Short = Val(txtH.Text)
        ImagesBox.SetWire(w, h, 5, 5)
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()
    End Sub

    Private Sub pnl_settings_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnl_settings.MouseDoubleClick
        pnl_settings.Height = 375
    End Sub

    
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ImagesBox.Animation = Not ImagesBox.Animation
    End Sub

    Private Sub btnSid_Click() Handles btnSid.Click
        Cursor = Cursors.WaitCursor
        cbSID.value = True
        refresh_files()
        cbSID.value = False
        Cursor = Cursors.Arrow
    End Sub


    Private Sub FilesFilter_ValueChanged() Handles FilesFilter.ValueChanged
        refine_files()
    End Sub

    Private Sub ImagesBox_ChangeDir(ByRef Path As String) Handles ImagesBox.ChangeDir
        DirsBox.NewPath(Path, Path)
        refresh_files()
        PathLine.NewMaxPath(Path)
    End Sub

    Private Sub pnl_settings_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnl_settings.Paint

    End Sub

    Private Sub btnOnlyImg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOnlyImg.Load

    End Sub

    Private Sub btnOnlyMusic_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOnlyMusic.Load

    End Sub
End Class

