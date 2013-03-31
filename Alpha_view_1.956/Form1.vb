Imports System
Imports System.Threading
Imports TestHelpers
Imports System.Management
Imports System.Threading.Tasks

Public Class frmMain
    Dim WithEvents Player As New WMPLib.WindowsMediaPlayer

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
    Private Sub SetOnlyMusicSettings()
        ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Vertical
        ImagesBox.SetWire(200, 26, 7, 5)

        ImagesBox.Wire.Border = 0 : sbBorder.value = 0
        ImagesBox.DrawShadow = False : cbShadow.value = False
        ImagesBox.ShowImagesName = False

        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.Simple
    End Sub
    Private Sub SetOnlyImagesSettings()
        ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Нorizontal
        ImagesBox.Wire.Border = 0 : sbBorder.value = 0
        ImagesBox.DrawShadow = True : cbShadow.value = True

        ImagesBox.ShowImagesName = False
        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.Simple
        ImagesBox.SetWire(100, 100, 20, 20)
    End Sub
    Private Sub SetCommonSettings()
        ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Vertical
        ImagesBox.SetWire(200, 16, 7, 5)

        ImagesBox.Wire.Border = 0 : sbBorder.value = 0
        ImagesBox.DrawShadow = False : cbShadow.value = False
        ImagesBox.ShowImagesName = False

        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.Simple
    End Sub
    Sub SortImagesBoxFiles()
        If FilesFilter.value = 0 Then ImagesBox.SortImagesByName() : ImagesBox.SortImagesBySinger() : ImagesBox.SortImagesByType()
        If FilesFilter.value = 1 Then ImagesBox.SortImagesByDate() : ImagesBox.SortImagesByType()
        If FilesFilter.value = 2 Then ImagesBox.SortImagesByName() : ImagesBox.SortImagesBySinger()
    End Sub

    Public Sub make_sid()
        txtSearchWhat.Text = ""
        Dim str As String = PathLine.MaxPath
        'MsgBox(str)
        If Mid(str, str.Length) = "\" Then str = Mid(str, 1, str.Length - 1)
        While InStr(str, "\") > 0
            str = Mid(str, InStr(str, "\") + 1)
        End While
        If str.Length = 2 Then str += "\"
        Me.Text = "SID " + str

        Dim name As String
        If PathLine.MaxPath <> "home" Then
            Try
                ImagesBox.StopLoading = True
                'files = IO.Directory.GetFiles(DirsBox.Path, "*.*", IO.SearchOption.AllDirectories)
                Dim files As Collections.ObjectModel.ReadOnlyCollection(Of String)

                With ImagesBox
                    '.SetWire(txtW.Text, txtH.Text, 5, 5)
                    .BGColor = Color.FromArgb(sbBGColor.value, sbBGColor.value, sbBGColor.value)
                    .DrawShadow = cbShadow.value
                    .ShowImagesName = cbNames.value
                    '.FrameSize = txtBorder.Text
                    .ClearImages()
                    .Path = DirsBox.Path
                End With

                If FilesFilter.value = 0 Then
                    Try
                        files = FileIO.FileSystem.GetFiles(PathLine.MaxPath, 3, "*.*")
                    Catch
                        files = FileIO.FileSystem.GetFiles(PathLine.MaxPath, 2, "*.*")
                    End Try

                    Dim WasMusic, WasImage, WasSmthElse As Boolean
                    Dim CMusic, CImage, CSmthElse As Long
                    WasImage = False : WasMusic = False : WasSmthElse = False

                    If cbDirs.value Then
                        Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                        dirs = FileIO.FileSystem.GetDirectories(PathLine.MaxPath)
                        For Each name In dirs
                            ImagesBox.AddImage(name)
                        Next
                    End If

                    For Each name In files
                        If IsMusicFile(name) Then
                            'If FilesFilter.value = 0 Or FilesFilter.value = 2 Then 
                            ImagesBox.AddImage(name)
                            WasMusic = True
                            CMusic += 1
                        ElseIf IsImageFile(name) Then
                            'If FilesFilter.value = 0 Or FilesFilter.value = 1 Then 
                            ImagesBox.AddImage(name)
                            WasImage = True
                            CImage += 1
                        Else
                            'If FilesFilter.value = 0 Then 
                            ImagesBox.AddImage(name)
                            WasSmthElse = True
                        End If
                        CSmthElse += 1
                    Next
                    FilesFilter.MusicCount = CMusic
                    FilesFilter.AllCount = CSmthElse
                    FilesFilter.ImagesCount = CImage
                    'If ImagesBox.NImages = 0 And FilesFilter.value = 0 Then
                    '    If cbDirs.value Then
                    '        Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    '        dirs = FileIO.FileSystem.GetDirectories(DirsBox.Path)
                    '        For Each name In dirs
                    '            ImagesBox.AddImage(name)
                    '            ImagesBox.Image(ImagesBox.NImages).Type = "folder"
                    '        Next
                    '    End If
                    '    For Each name In files
                    '        ImagesBox.AddImage(name)
                    '    Next
                    '    If ImagesBox.NImages > 0 Then FilesFilter.value = 0
                    'End If 'Else
                    If WasImage And WasMusic = False And WasSmthElse = False Then
                        'If FilesFilter.value <> 1 Then
                        FilesFilter.value = 1

                        SetOnlyImagesSettings()
                        'End If
                    ElseIf WasMusic And WasImage = False And WasSmthElse = False Then
                        'If FilesFilter.value <> 1 Then
                        FilesFilter.value = 2

                        SetOnlyMusicSettings()
                        'End If
                    Else
                        FilesFilter.value = 0

                        SetCommonSettings()
                    End If
                    'End If
                    If FilesFilter.value = 2 Then
                        ImagesBox.SortImagesByName()
                        ImagesBox.SortImagesBySinger()
                    End If
                    If FilesFilter.value = 0 Then
                        ImagesBox.SortImagesByType()
                    End If
                ElseIf FilesFilter.value = 2 Then
                    files = FileIO.FileSystem.GetFiles(PathLine.MaxPath, 3, "*.*")

                    Dim WasMusic, WasImage, WasSmthElse As Boolean
                    Dim CMusic, CImage, CSmthElse As Long
                    WasImage = False : WasMusic = False : WasSmthElse = False

                    If cbDirs.value Then
                        Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                        dirs = FileIO.FileSystem.GetDirectories(PathLine.MaxPath)
                        For Each name In dirs
                            ImagesBox.AddImage(name)
                        Next
                    End If

                    For Each name In files
                        If IsMusicFile(name) Then
                            If FilesFilter.value = 0 Or FilesFilter.value = 2 Then ImagesBox.AddImage(name)
                            WasMusic = True
                            CMusic += 1
                        ElseIf IsImageFile(name) Then
                            If FilesFilter.value = 0 Or FilesFilter.value = 1 Then ImagesBox.AddImage(name)
                            WasImage = True
                            CImage += 1
                        Else
                            If FilesFilter.value = 0 Then ImagesBox.AddImage(name)
                            WasSmthElse = True
                        End If
                        CSmthElse += 1
                    Next
                    FilesFilter.MusicCount = CMusic
                    FilesFilter.AllCount = CSmthElse
                    FilesFilter.ImagesCount = CImage

                    If WasImage And WasMusic = False And WasSmthElse = False Then
                        'If FilesFilter.value <> 1 Then
                        FilesFilter.value = 1

                        SetOnlyImagesSettings()
                        'End If
                    ElseIf WasMusic And WasImage = False And WasSmthElse = False Then
                        'If FilesFilter.value <> 1 Then
                        'FilesFilter.value = 2

                        SetOnlyMusicSettings()
                        'End If
                    Else
                        'FilesFilter.value = 0

                        SetCommonSettings()
                    End If
                    'End If
                    SortImagesBoxFiles()
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
                    If DirsBox.NDirs <= 1 Then
                        HideDirsBox() : ImagesBox.Select()
                    Else
                        ShowDirsBox()
                    End If
                End If
                'ImagesBox.SetWire(ImagesBox.Wire.X, txtH.Text, 5, 5)
                ImagesBox.SetCanvas()
            Catch ex As Exception
                MsgBox("err! file_click" + ex.ToString)
            End Try
        Else
            ImagesBox.ClearImages()
            ImagesBox.Path = ""
            ShowDirsBox()
        End If
    End Sub


    Public Function GetDirectoriesAsync(ByVal Path As String) As Task(Of Collections.ObjectModel.ReadOnlyCollection(Of String))
        Return Task.Run(
            Function()
                Return FileIO.FileSystem.GetDirectories(Path)
            End Function)
    End Function
    Public Function GetFilesAsync(ByVal Path As String, ByVal Opt As FileIO.SearchOption, WC As String) As Task(Of Collections.ObjectModel.ReadOnlyCollection(Of String))
        Return Task.Run(
            Function()
                Try
                    Return FileIO.FileSystem.GetFiles(Path, Opt, WC)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                    'Dim n As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    Return Nothing
                End Try
            End Function)
    End Function
    Public Function DirectoryExistsAsync(ByVal Path As String) As Task(Of Boolean)
        Return Task.Run(
            Function()
                Return IO.Directory.Exists(Path)
            End Function)
    End Function
    Structure Artist
        Public Name As String
        Public Songs As Long
        Public Pop As Long
    End Structure
    Private Function CompareArtists_BySongs(x As Artist, y As Artist) As Integer
        Return y.Songs.CompareTo(x.Songs)
    End Function
    Private Function CompareArtists_ByPop(x As Artist, y As Artist) As Integer
        Return y.Pop.CompareTo(x.Pop)
    End Function
    Public Function BaseGetArtistsAsync() As Task(Of List(Of String))
        Return Task.Run(
            Function()
                Dim Tmp As Artist
                Dim Ret As New List(Of Artist)
                For i As Long = 1 To FileTags.N
                    If FileTags.Tags(i).Type = "music" Then
                        Dim f As Boolean = True
                        For j As Long = 0 To Ret.Count - 1
                            If Ret(j).Name = FileTags.Tags(i).Song.Singer.ToLower Then
                                f = False
                                Tmp.Name = Ret(j).Name
                                Tmp.Songs = Ret(j).Songs + 1
                                Tmp.Pop = Ret(j).Pop + FileTags.Tags(i).LaunchingTimes
                                Ret(j) = Tmp
                                Exit For
                            End If
                        Next
                        If f Then
                            Tmp.Name = FileTags.Tags(i).Song.Singer.ToLower
                            Tmp.Songs = 1
                            Tmp.Pop = FileTags.Tags(i).LaunchingTimes
                            Ret.Add(Tmp)
                        End If
                    End If
                Next

                Ret.Sort(AddressOf CompareArtists_ByPop)

                Dim Ret2 As New List(Of String)
                For Each i As Artist In Ret
                    Ret2.Add(i.Name)
                Next
                Return Ret2
            End Function)
    End Function
    Public Function BaseGetSongsAsync(Artist As String) As Task(Of List(Of String))
        Return Task.Run(
            Function()
                Artist = Artist.ToLower
                Dim Ret As New List(Of String)
                For i As Long = 1 To FileTags.N
                    If FileTags.Tags(i).Type = "music" Then
                        If FileTags.Tags(i).Song.Singer.ToLower = Artist Then
                            If Not Ret.Contains(FileTags.Tags(i).Song.Singer) Then
                                If IO.File.Exists(FileTags.Files(i)) Then Ret.Add(FileTags.Files(i))
                            End If
                        End If
                    End If
                Next
                Return Ret
            End Function)
    End Function

    Public Async Function refresh_files() As Tasks.Task
        UcWaitingIndicator1.Enabled = True

        txtSearchWhat.Text = ""
        Dim str As String = PathLine.MaxPath
        'MsgBox(str)
        If Mid(str, str.Length) = "\" Then str = Mid(str, 1, str.Length - 1)
        While InStr(str, "\") > 0
            str = Mid(str, InStr(str, "\") + 1)
        End While
        If str.Length = 2 Then str += "\"
        Me.Text = str

        Dim name As String
        Dim NewPath As String = PathLine.MaxPath
        If NewPath <> "home" And NewPath.IndexOf("music") <> 0 Then
            Try
                If Await DirectoryExistsAsync(PathLine.MaxPath) Then
                    ImagesBox.StopLoading = True
                    Dim files As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    If cbSID.value Then
                        files = Await GetFilesAsync(PathLine.MaxPath, 3, "*.*")
                    Else
                        files = Await GetFilesAsync(PathLine.MaxPath, 2, "*.*")
                    End If
                    If files Is Nothing Then UcWaitingIndicator1.Enabled = False : Exit Function
                    Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    If cbDirs.value Then dirs = Await GetDirectoriesAsync(PathLine.MaxPath)

                    With ImagesBox
                        .ClearImages()
                        .Path = PathLine.MaxPath
                    End With

                    Dim WasSmthElse As Boolean
                    Dim CMusic, CImage, CSmthElse As Long
                    WasSmthElse = False

                    If cbDirs.value And Not cbSID.value Then
                        For Each name In dirs
                            ImagesBox.AddImage(name, ucImagesBox.FileTypes.Folder)
                            If ImagesBox.Image(ImagesBox.NImages).Name = PathLine.Path(PathLine.NDirs + 1).name Then ImagesBox.SelectedImageIndex = ImagesBox.NImages
                        Next
                    End If

                    Dim F As Boolean = False
                    For Each name In files
                        If name.Length > 9 Then
                            If Mid(name, name.Length - 8).ToLower = "thumbs.db" Then
                                F = True
                            End If
                        End If
                        If Not F Then
                            ImagesBox.AddImage(name)

                            If IsMusicFile(name) Then
                                CMusic += 1
                            ElseIf IsImageFile(name) Then
                                CImage += 1
                            Else
                                WasSmthElse = True
                            End If
                            CSmthElse += 1
                        Else
                            F = False
                        End If
                    Next

                    If cbDirs.value Then
                        With ImagesBox
                            If .SelectedImageIndex = 0 Then
                                For i As Short = 1 To dirs.Count
                                    'If (.Image(i).Type = ucImagesBox.FileTypes.Folder) Then
                                    .Image(i).InTagsIngex = DirTags.FindByName(.Image(i).FileName)
                                    If .Image(i).InTagsIngex = 0 Then
                                        .Image(i).InTagsIngex = DirTags.Add(.Image(i).FileName)
                                    End If
                                    'End If
                                Next
                                Dim maxI As Long = 1, max As Long = DirTags.Tags(.Image(1).InTagsIngex).LaunchingTimes
                                For i As Short = 2 To dirs.Count
                                    'If (.Image(i).Type = ucImagesBox.FileTypes.Folder) Then
                                    If DirTags.Tags(.Image(i).InTagsIngex).LaunchingTimes > max Then
                                        max = DirTags.Tags(.Image(i).InTagsIngex).LaunchingTimes
                                        maxI = i
                                    End If
                                    'End If
                                Next
                                .SelectedImageIndex = maxI
                                For i As Short = 1 To .NImages
                                    .Image(i).InTagsIngex = 0
                                Next
                            End If
                        End With
                    End If

                    FilesFilter.MusicCount = CMusic
                    FilesFilter.AllCount = CSmthElse
                    FilesFilter.ImagesCount = CImage

                    If CImage > 0 And CMusic = 0 And Not WasSmthElse Then
                        If FilesFilter.value <> 1 Then
                            FilesFilter.value = 1
                            SetOnlyImagesSettings()
                        End If
                    ElseIf CMusic > 0 And CImage = 0 And WasSmthElse = False Then
                        If FilesFilter.value <> 2 Then
                            FilesFilter.value = 2
                            SetOnlyMusicSettings()
                        End If
                    Else
                        If FilesFilter.value <> 0 Then
                            FilesFilter.value = 0
                            SetCommonSettings()
                        End If
                    End If

                    SortImagesBoxFiles()

                    If ImagesBox.SelectedImageIndex = 0 Then ImagesBox.SelectedImageIndex = 1

                    ImagesBox.MakeAllThumbnails()
                    If Not ImagesBox.FlyMode Then ImagesBox.SetImagesLocation()
                    ImagesBox.OrderImages()

                    ImagesBox.SetCanvas(False)
                End If
            Catch ex As Exception
                MsgBox("err! refresh file" + ex.ToString())
            End Try
            UcWaitingIndicator1.Enabled = False
        ElseIf PathLine.MaxPath.ToLower.IndexOf("music") = 0 Then
            ImagesBox.ClearImages()
            ImagesBox.Path = NewPath
            ImagesBox.SelectedImageIndex = 0
            Dim arr As List(Of String)
            If PathLine.MaxPath.Length <= 6 Then
                arr = Await BaseGetArtistsAsync()
                For Each s As String In arr
                    s = s.Trim
                    ImagesBox.AddImage("music\" + s, ucImagesBox.FileTypes.Command)
                    If s = PathLine.Path(PathLine.NDirs + 1).name Then ImagesBox.SelectedImageIndex = ImagesBox.NImages
                Next
                ImagesBox.SetWire(150, 32, 16, 16)
            Else
                arr = Await BaseGetSongsAsync(PathLine.MaxPath.Substring(5).Replace("\", ""))
                For Each s As String In arr
                    ImagesBox.AddImage(s, ucImagesBox.FileTypes.Music)
                Next
                ImagesBox.SetWire(150, 32, 5, 5)
                ImagesBox.SortImagesByName()
            End If
            If ImagesBox.SelectedImageIndex = 0 Then ImagesBox.SelectedImageIndex = 1

            ImagesBox.MakeAllThumbnails()
            'ImagesBox.OrderImages()
            ImagesBox.SetWire(150, ImagesBox.Wire.Y, ImagesBox.Wire.dX, ImagesBox.Wire.dY)
            ImagesBox.SetImagesLocation()
            ImagesBox.SetCanvas()
            UcWaitingIndicator1.Enabled = False
            UcWaitingIndicator1.Enabled = False
        Else
            FilesFilter.value = 3
            ImagesBox.SetWire(150, 50, 20, 20)

            ImagesBox.ClearImages()
            ImagesBox.Path = ""

            Dim Drives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
            Dim Info As IO.DriveInfo, ent As String = Chr(13) + Chr(10)
            Dim dr(Drives.Length) As String, i As Long = 0

            For Each Info In Drives
                If Info.RootDirectory.ToString <> "A:\" Then
                    If Info.IsReady Then
                        i += 1
                        ImagesBox.AddImage(Info.RootDirectory.ToString(), ucImagesBox.FileTypes.Drive)
                        ImagesBox.Image(ImagesBox.NImages).OriginalName = Info.VolumeLabel + " (" + Mid(Info.Name, 1, 2) + ")"
                        ImagesBox.Image(ImagesBox.NImages).Singer = SizeToString(Info.TotalFreeSpace) + " / " + SizeToString(Info.TotalSize)
                        'Dir(i).Path = Info.RootDirectory.ToString 'Dir(i).Type = Info.DriveType.ToString
                    End If
                End If
            Next
            Array.Clear(Drives, 0, Drives.Length)

            ImagesBox.AddImage("D:\_University")
            ImagesBox.AddImage("Music", ucImagesBox.FileTypes.Command)
            ImagesBox.AddImage("C:\Users\" + System.Environment.UserName + "\Downloads")
            ImagesBox.AddImage("C:\Users\" + System.Environment.UserName + "\Desktop")

            ImagesBox.AddImage("C:\Program Files (x86)\Safari\Safari.exe")
            ImagesBox.AddImage("C:\Program Files\Adobe\Adobe Photoshop CS4 (64 Bit)\Photoshop.exe")
            ImagesBox.AddImage("C:\Program Files (x86)\Skype\Phone\Skype.exe")

            Dim RecentArray(1) As Integer
            Dim PosInRA As Integer = 0
            Dim Max As Long = 0
            For k As Long = 1 To FileTags.N
                Max = RecentArray(PosInRA)
                If FileTags.Tags(k).LaunchingTimes > FileTags.Tags(Max).LaunchingTimes Then
                    If IO.Directory.Exists(FileTags.Files(k)) Or IO.File.Exists(FileTags.Files(k)) Then
                        If ImagesBox.IsInListIgnoreCase(FileTags.Files(k)) = 0 Then
                            RecentArray(PosInRA) = k
                            'Max = k
                            PosInRA += 1
                            If PosInRA > RecentArray.Length - 1 Then PosInRA = 0
                        End If
                    End If
                End If
            Next
            For k As Long = 0 To RecentArray.Length - 1
                If (RecentArray(k) <> 0) Then ImagesBox.AddImage(FileTags.Files(RecentArray(k)))
            Next

            ImagesBox.SelectedImageIndex = 1
            For j As Long = 1 To ImagesBox.NImages
                If ImagesBox.Image(j).FileName = PathLine.Path(PathLine.NDirs + 1).name Then ImagesBox.SelectedImageIndex = j
            Next

            ImagesBox.SetCanvas()
            ImagesBox.MakeAllThumbnails()
            ImagesBox.SetImagesLocation()
            ImagesBox.OrderImages()

            UcWaitingIndicator1.Enabled = False
        End If
        ImagesBox.Focus()
        'Try
        '    Dim G As New List(Of String)
        '    For i As Long = 1 To ImagesBox.NImages
        '        If Not ImagesBox.Image(i).OriginalName Is Nothing Then G.Add(ImagesBox.Image(i).OriginalName.Replace("""", ""))
        '    Next
        '    OnAirVoiceRecognizer.sre_newGrammar(G.ToArray)
        'Catch ex As Exception
        '    MsgBox("err! G" + ex.ToString())
        'End Try
    End Function

    Public Function SizeToString(ByVal Size As Double) As String
        Dim Pref As String = "b"
        If (Size > 1024) Then Size /= 1024 : Pref = "Kb"
        If (Size > 1024) Then Size /= 1024 : Pref = "Mb"
        If (Size > 1024) Then Size /= 1024 : Pref = "Gb"
        Return (Math.Round(Size * 10) / 10).ToString + Pref
    End Function

    Public Async Function refine_files() As Task
        UcWaitingIndicator1.Enabled = True

        txtSearchWhat.Text = ""
        Dim str As String = PathLine.MaxPath
        If Mid(str, str.Length) = "\" Then str = Mid(str, 1, str.Length - 1)
        While InStr(str, "\") > 0
            str = Mid(str, InStr(str, "\") + 1)
        End While
        If str.Length = 2 Then str += "\"
        Me.Text = str

        If FilesFilter.value = 0 Then SetCommonSettings()
        If FilesFilter.value = 1 Then SetOnlyImagesSettings()
        If FilesFilter.value = 2 Then SetCommonSettings()

        Dim name As String
        If PathLine.MaxPath <> "home" Then
            Try
                ImagesBox.StopLoading = True
                Dim files As Collections.ObjectModel.ReadOnlyCollection(Of String)
                If cbSID.value Then
                    files = FileIO.FileSystem.GetFiles(PathLine.MaxPath, 3, "*.*")
                Else
                    files = FileIO.FileSystem.GetFiles(PathLine.MaxPath, 2, "*.*")
                End If

                ImagesBox.Path = PathLine.MaxPath

                Dim WasMusic, WasImage, WasSmthElse As Boolean
                WasImage = False
                WasMusic = False
                WasSmthElse = False

                Dim NewList(10000) As String
                Dim i As Long = 0
                If cbDirs.value Then
                    Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                    dirs = FileIO.FileSystem.GetDirectories(PathLine.MaxPath)
                    For Each name In dirs
                        i += 1
                        NewList(i) = name
                    Next
                End If

                For Each name In files
                    If IsMusicFile(name) Then
                        If FilesFilter.value = 0 Or FilesFilter.value = 2 Then i += 1 : NewList(i) = name 'ImagesBox.AddImage(name)
                        WasMusic = True
                    ElseIf IsImageFile(name) Then
                        If FilesFilter.value = 0 Or FilesFilter.value = 1 Then i += 1 : NewList(i) = name 'ImagesBox.AddImage(name)
                        WasImage = True
                    Else
                        If FilesFilter.value = 0 Then i += 1 : NewList(i) = name 'ImagesBox.AddImage(name)
                        WasSmthElse = True
                    End If
                Next
                ReDim Preserve NewList(i)

                ImagesBox.Refine(NewList)

                SortImagesBoxFiles()

                ImagesBox.ReloadAllThumbs()
                ImagesBox.MakeAllThumbnails()
                ImagesBox.OrderImages()
            Catch ex As Exception
                MsgBox("err! !!!" + ex.ToString)
            End Try
        Else
            Await refresh_files()
        End If

        UcWaitingIndicator1.Enabled = False
        ImagesBox.Focus()
    End Function

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Visible = False
        Player.close()
        frmMusicPlayer.Close()
        frmWireWidth.Dispose()
        OnAirVoiceRecognizer.StopRecording()
        SaveThumbsInfo()
        FileTags.Save()
    End Sub


    Private Async Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        'MsgBox(e.KeyCode)
        Select Case e.KeyCode
            Case Keys.F5
                Await refine_files()
                btnRefresh.btnDown()
            Case Keys.Escape
                If ImagesBox.Focused Then
                    ImagesBox.StopLoading = True
                Else
                    ImagesBox.Focus()
                    pnl_settings.Visible = False
                    pnl_35photo.Visible = False
                End If
            Case Keys.E
                If e.Control Then
                    pnl_settings.Visible = True
                    txtW.Select()
                End If
            Case Keys.P
                If e.Control Then PathLine.MakeTextMode()
            Case Keys.F
                If e.Control Then
                    If txtSearchWhat.Focused = False Then txtSearchWhat.Select()
                    txtSearchWhat.SelectionStart = 0
                    txtSearchWhat.SelectionLength = txtSearchWhat.Text.Length
                    e.Handled = True
                End If
            Case Keys.Add
                'If e.Control Then 
                'UcScrollBar1.value += 10'!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Case 109
                'If e.Control Then 
                'UcScrollBar1.value -= 10
            Case Keys.Oemplus
                'If e.Control Then 
                'UcScrollBar1.value += 1
            Case Keys.OemMinus
                'If e.Control Then 
                'UcScrollBar1.value -= 1
            Case Keys.Q
                If e.Control Then
                    If picResizeDest = picResizeMinY Then
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
                    Await refresh_files()
                    cbSID.value = False
                    Cursor = Cursors.Arrow
                End If
        End Select
    End Sub
    Private Sub frmMain_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        btnRefresh.btnNormal()
        btnUp.btnNormal()
    End Sub


    Private Sub LoadButtons()
        btnClose2.LoadImages("buttons\close.bmp", "buttons\close_mm.bmp", "buttons\close_down.bmp", "")
        btn35photo.LoadImages("buttons\35photo.bmp", "buttons\35photo_mm.bmp", "buttons\35photo_down.bmp", "") ', "buttons\selection.gif")
        btnLoad35photo.LoadImages("buttons\load.bmp", "buttons\load_mm.bmp", "buttons\load_down.bmp", "")

        btnOrderBigThumbs.LoadImages("buttons\order_big_thumbs4.png")
        btnOrderList.LoadImages("buttons\order_min_thumbs3.png")
        btnMakeSID.LoadImages("buttons\sid.png")
        btnSeach.LoadImages("buttons\search.png")
        btnShowSeach.LoadImages("buttons\search.png")

        btnUp.LoadImages("buttons\up.png")
        btnRefresh.LoadImages("buttons\reload.png")
        btnTempArea.LoadImages("buttons\temp.png")
        btnSettings.LoadImages("buttons\settings.png")

        btnClose.LoadImages("buttons\close.png")
        btnMinimize.LoadImages("buttons\minim.png")

        cbDirs.LoadCustomTiles("folders")
        cbSelectionMode.LoadCustomTiles("selection_mode")
    End Sub
    Public StartupPath = "home"
    Private Async Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim r As New Random(DateTime.Now.Millisecond)
        Me.Left += r.Next(-20, 20)
        Me.Top += r.Next(-20, 20)
        Try
            If Not UcCheckBox1.value Then DirsBox.Select() Else ImagesBox.Select()

            PathLine.NewMaxPath("D:\_univer\")
            PathLine.NewMaxPath(StartupPath)


            DirsBox.Init()
            DirsBox.NewPath(StartupPath, "") '"C:\Documents\")

            LoadButtons()
            LoadBorders()
            sbWireWidth.Pic = btnOrderList.GetMDPic() : sbWireWidth.IsPic = True
            sbWireWidthBig.Pic = btnOrderBigThumbs.GetMDPic() : sbWireWidthBig.IsPic = True

            ResizeAll() '----

            ImagesBox.LoadUIImages()

            LoadThumbsInfo()
            FileTags.Load()
            DirTags.Load()

            If StartupPath <> "home" Then
                PathLine.NewMaxPath(StartupPath)
            End If
            Await refresh_files()

            OnAirVoiceRecognizer.StartListening()
        Catch ex As Exception
            MsgBox("Ooops!  " + ex.ToString)
        End Try
    End Sub

#Region "Editing divider line"
    Dim DirsBoxHeight As Short
    Dim picResizeY As Long
    Dim picResizeDest As Integer
    Dim picResizeMinY As Long = 29 + 6
    Dim picResizeMaxY As Long
    Dim picResizeMinDelta As Long = 100
    Private Sub picResize_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles picResize.DoubleClick
        If picResize.Top = picResizeMinY Then ShowDirsBox() Else HideDirsBox()
    End Sub
    Private Sub picResize_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picResize.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            ChangePicResizeSize()
        Else
            tmrDirsBoxAnimation.Enabled = False
            If picResize.Top > picResizeMinY And picResize.Top < picResizeMaxY Then DirsBoxHeight = picResize.Top
            picResizeY = e.Y
            ImagesBox.ResizeStarted()
            Resizing = True
        End If
    End Sub
    Private Sub picResize_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picResize.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            If picResizeY <> e.Y Or Divider_PrevPoint.X <> e.X Then
                If Math.Abs(picResizeY - e.Y) > Math.Abs(Divider_PrevPoint.X - e.X) Then Divider_LastY = e.Y - picResizeY Else Divider_LastY = 0
                Divider_LastTimeOfMoving = DateTime.Now
                Divider_PrevPoint = e.Location
            End If

            If picResizeY <> e.Y Then
                Dim NewPRTop = picResize.Top - (picResizeY - e.Y)
                If NewPRTop < picResizeMinY Then NewPRTop = picResizeMinY
                If NewPRTop > picResizeMaxY Then NewPRTop = picResizeMaxY
                If NewPRTop > picResizeMinY + 11 Then DirsBox.Height = NewPRTop - PathLine.Height - 4 - 6 Else DirsBox.Height = 0
                picResize.Top = NewPRTop

                Dim H As Integer = Me.ClientRectangle.Height - picResize.Top - picResize.Height - delta

                ImagesBox.Resizing(ImagesBox.Width, H)
                ImagesBox.Top = picResize.Top + picResize.Height
                ImagesBox.Height = H

                ImagesBox.Refresh()

                picResize.Refresh()
                DirsBox.Refresh()
                BorderBottom.Refresh()
            End If
        End If
    End Sub
    Private Sub picResize_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picResize.MouseUp
        If (DateTime.Now - Divider_LastTimeOfMoving).TotalSeconds < 0.05 Then
            If (Divider_LastY < -2) Then
                If DirsBoxHeight > picResize.Top Then HideDirsBox() Else ShowDirsBox()
            ElseIf (Divider_LastY > 2) Then
                If DirsBoxHeight > picResize.Top Then ShowDirsBox() Else HideImagesBox()
            Else
                If picResize.Top < picResizeMinY + picResizeMinDelta Then
                    If tmrDirsBoxAnimation.Enabled <> True Then HideDirsBox()
                ElseIf picResize.Top > picResizeMaxY - picResizeMinDelta Then
                    If tmrDirsBoxAnimation.Enabled <> True Then HideImagesBox()
                Else
                    DirsBoxHeight = picResize.Top
                End If
            End If
        Else
            If picResize.Top < picResizeMinY + picResizeMinDelta Then
                If tmrDirsBoxAnimation.Enabled <> True Then HideDirsBox()
            ElseIf picResize.Top > picResizeMaxY - picResizeMinDelta Then
                If tmrDirsBoxAnimation.Enabled <> True Then HideImagesBox()
            Else
                DirsBoxHeight = picResize.Top
            End If
        End If

        If tmrDirsBoxAnimation.Enabled = False Then
            ImagesBox.ResizeEnded()
            Resizing = False
            ResizeAll()
        End If
    End Sub
    Dim Divider_LastTimeOfMoving As Date
    Dim Divider_LastY As Short
    Dim Divider_PrevPoint As Point

    Private Sub HideDirsBox()
        ImagesBox.ResizeStarted()
        Resizing = True
        picResizeDest = picResizeMinY
        ImagesBox.Select()
        tmrDirsBoxAnimation.Enabled = True
    End Sub
    Private Sub ShowDirsBox()
        ImagesBox.ResizeStarted()
        Resizing = True
        picResizeDest = DirsBoxHeight
        DirsBox.Select()
        tmrDirsBoxAnimation.Enabled = True
    End Sub
    Private Sub HideImagesBox()
        ImagesBox.ResizeStarted()
        Resizing = True
        picResizeDest = picResizeMaxY
        DirsBox.Select()
        tmrDirsBoxAnimation.Enabled = True
    End Sub
    Private Sub tmrDirsBoxAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDirsBoxAnimation.Tick
        If picResize.Top <> picResizeDest Then
            Dim delta1 As Long = (-picResizeDest + picResize.Top) * 0.21 + Math.Sign(-picResizeDest + picResize.Top)
            Dim NewPRTop As Long = picResize.Top - delta1
            If NewPRTop < picResizeMinY Then NewPRTop = picResizeMinY
            If NewPRTop > picResizeMaxY Then NewPRTop = picResizeMaxY
            If NewPRTop > PathLine.Height + 6 + 5 + 6 Then DirsBox.Height = NewPRTop - DirsBox.Top Else DirsBox.Height = 0
            picResize.Top = NewPRTop

            Dim H As Integer = Me.ClientRectangle.Height - picResize.Top - picResize.Height - delta
            ImagesBox.Resizing(ImagesBox.Width, H)
            ImagesBox.Top = picResize.Top + picResize.Height

            picResize.Refresh()
            DirsBox.RedrawComposed()
            DirsBox.Refresh()
            BorderBottom.Refresh()

            If NewPRTop = picResizeDest Then tmrDirsBoxAnimation.Enabled = False : ImagesBox.ResizeEnded() : Resizing = False : DirsBox.CorrectCanvas() : DirsBox.Redraw()
        Else
            tmrDirsBoxAnimation.Enabled = False : ImagesBox.ResizeEnded() : Resizing = False : DirsBox.CorrectCanvas() : DirsBox.Redraw()
        End If
    End Sub
#End Region

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
    '    'Dim wc As New Net.WebClient 'Ñîçäà¸ì WebClient   
    '    ''Ñîçäà¸ì ïîòîê è BinaryWriter äëÿ çàïèñè äàííûõ â ôàéë   
    '    'Dim fs As New IO.FileStream("35photo\main.txt", IO.FileMode.Create)
    '    'Dim bw As New IO.BinaryWriter(fs)
    '    'Dim b() As Byte
    '    ''Êîïèðóåì ôàéë â áàéòîâûé ìàññèâ   
    '    'b = wc.DownloadData("http://35photo.ru/")
    '    ''Ïèøåì áàéòîâûé ìàññèâ â FileStream   
    '    'bw.Write(b)
    '    ''Çàêðûâàåì îáúåêòû   
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
            'ProgressBar1.Value = 0

            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(url)
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
                    'Label1.Text = "( " + n.ToString + " | 39 ) Ñêîðîñòü " + CStr(Math.Round((i / 1024) / gl_elapsed_time.TotalSeconds, 0)) + " Êá/ñ. "
                    picSpeed.Image = GenerateStatusBar(picSpeed.Width, i / length)
                    Application.DoEvents()
                End If
            Next
            'ProgressBar1.Value = ProgressBar1.Maximum

            Using output As IO.Stream = System.IO.File.Create(FilePath)
                output.Write(bytes, 0, bytes.Length)
            End Using
            gl_stop_time = Now
            gl_elapsed_time = gl_stop_time.Subtract(gl_start_time)
            'Label1.Text = CStr(Math.Round((length - 1) / 1024, 2)) + " Êá. çà " + CStr(Math.Round(gl_elapsed_time.TotalSeconds, 1)) + " ñ." : Application.DoEvents()
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "a1")
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
        If w > 50 And h >= 16 Then ImagesBox.SetWire(w, h, Val(txtdx.Text), Val(txtdy.Text))
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
    Private Async Sub btnUp_Click() Handles btnUp.Clicked
        PathLine.GoUp()
        Await refresh_files()
        'If DirsBox.Visible Then DirsBox.Select() Else ImagesBox.Select()
    End Sub
    Private Async Sub btnReload_Click() Handles btnRefresh.Clicked
        Await refresh_files()
    End Sub
    Private Sub btnSettings_Click()
        pnl_settings.Visible = True
        txtW.Select()
    End Sub
    Private Sub btnClose1_Click()
        pnl_settings.Visible = False
    End Sub
    Private Sub btn35photo_Click() Handles btn35photo.Click
        pnl_35photo.Visible = True
    End Sub
    Private Sub btnClose2_Click() Handles btnClose2.Click
        pnl_35photo.Visible = False
    End Sub
    Private Sub btnClose_Click() Handles btnClose.Clicked
        HideInTray()
        'Me.Close()
    End Sub
#End Region
#Region "Borders"
    Private Sub LoadBorders()
        Dim MyPath As String = Application.StartupPath + "\resources\"
        BorderTop.Image = Image.FromFile(MyPath + "top.gif")
        BorderLeft.Image = Image.FromFile(MyPath + "left.gif")
        BorderRight.Image = Image.FromFile(MyPath + "right.gif")
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
            FullScreenMe()
        End If
    End Sub
    Private Sub SaveFormRect()
        FrmRect.Location = Me.Location
        FrmRect.Size = Me.Size
    End Sub
    Private Sub FullScreenMe()
        SaveFormRect()
        Me.Left = 0
        Me.Top = 0
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height
        SetFullScreenBorders()
        IsCustomFormRect = True
        ResizeAll()
        'btnClose.Left += 6 + 1 : btnClose.Top = -2 - 1
        Me.Refresh()
    End Sub
    Private Sub Me_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        frmPoint = e.Location
    End Sub
    Private Sub Me_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button Then
            Dim newLeft, newTop As Short
            newLeft = Me.Left - frmPoint.X + e.X
            newTop = Me.Top - frmPoint.Y + e.Y

            Dim dx As Short = 10
            Dim dy As Short = 10
            If IsCustomFormRect Then
                If Me.Width = Screen.PrimaryScreen.WorkingArea.Width Then
                    dx = 30
                Else
                    dy = 30
                End If
            End If

            Dim WasMagnet As Boolean = False
            If Math.Abs(newTop) < dy Then newTop = 0 : WasMagnet = True
            If Math.Abs(newLeft) < dx Then newLeft = 0 : WasMagnet = True
            If Math.Abs(newLeft - (Screen.PrimaryScreen.WorkingArea.Width - Me.Width)) < dx Then newLeft = Screen.PrimaryScreen.WorkingArea.Width - Me.Width : WasMagnet = True
            If Math.Abs(newTop - (Screen.PrimaryScreen.WorkingArea.Height - Me.Height)) < dy Then newTop = Screen.PrimaryScreen.WorkingArea.Height - Me.Height : WasMagnet = True

            If IsCustomFormRect Then
                If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height And _
                    (newLeft <> 0 Or newTop <> 0) Then
                    Me.Size = FrmRect.Size
                    frmPoint.X = Me.Width * (MousePosition.X / Screen.PrimaryScreen.WorkingArea.Width)
                    SetNormalBorders()
                    Me.Left = newLeft : Me.Top = newTop
                    IsCustomFormRect = False
                ElseIf Me.Height = Screen.PrimaryScreen.WorkingArea.Height And newTop = 0 Then
                    Me.Left = newLeft
                ElseIf Me.Width = Screen.PrimaryScreen.WorkingArea.Width And newLeft = 0 Then
                    Me.Top = newTop
                Else
                    Me.Size = FrmRect.Size
                    frmPoint.X = Me.Width * (MousePosition.X / Screen.PrimaryScreen.WorkingArea.Width)
                    SetNormalBorders()
                    IsCustomFormRect = False

                    Me.Left = newLeft : Me.Top = newTop
                End If
            Else
                Me.Left = newLeft : Me.Top = newTop
            End If
        End If
    End Sub
    Dim IsCustomFormRect As Boolean = False
    Private Sub frmMain_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If MousePosition.X = 0 Then
            SaveFormRect()
            Me.Left = 0
            Me.Top = 0
            Me.Height = Screen.PrimaryScreen.Bounds.Height
            Me.Width = Screen.PrimaryScreen.Bounds.Width / 2
            IsCustomFormRect = True
            ResizeAll()
        End If
        If MousePosition.X = Screen.PrimaryScreen.Bounds.Width - 1 Then
            SaveFormRect()
            Me.Left = Screen.PrimaryScreen.Bounds.Width / 2
            Me.Top = 0
            Me.Height = Screen.PrimaryScreen.Bounds.Height
            Me.Width = Screen.PrimaryScreen.Bounds.Width / 2
            IsCustomFormRect = True
            ResizeAll()
        End If
        If MousePosition.Y = 0 Then
            FullScreenMe()
        End If
        If MousePosition.Y = Screen.PrimaryScreen.Bounds.Height - 1 Then
            SaveFormRect()
            Me.Left = 0
            Me.Top = Screen.PrimaryScreen.Bounds.Height / 2
            Me.Height = Screen.PrimaryScreen.Bounds.Height / 2
            Me.Width = Screen.PrimaryScreen.Bounds.Width
            IsCustomFormRect = True
            ResizeAll()
        End If
    End Sub
#End Region
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
    End Sub
    Dim delta As Long = 1
    Private Sub ResizeCustom()
        DirsBox.Width = Me.ClientRectangle.Width - delta * 2
        If picResize.Top > PathLine.Height + delta + 5 + delta Then DirsBox.Height = picResize.Top - PathLine.Height - 4 - 6 Else DirsBox.Height = 0
        ImagesBox.Top = picResize.Top + picResize.Height
        ImagesBox.Width = Me.ClientRectangle.Width - delta * 2
        ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - delta
        picResize.Width = Me.Width - picResize.Left * 2 'DirsBox.Width

        ResizeBorder()

        btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - delta + 2
        btnMinimize.Left = Me.ClientRectangle.Width - btnClose.Left - btnMinimize.Width - 4

        On Error Resume Next
        Me.Refresh()
    End Sub
    Private Sub ResizeAll()
        If True Then
            If IsCustomFormRect And Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
                PathLine.Width = Me.Width - 400
                btnRefresh.Left = PathLine.Width + PathLine.Left + 5
                btnSettings.Left = btnRefresh.Width + btnRefresh.Left + 6 + 6
                btnTempArea.Left = btnSettings.Width + btnSettings.Left + 6

                DirsBox.Width = Me.ClientRectangle.Width '- delta * 2
                DirsBox.Left = 0 'delta
                If picResize.Top > PathLine.Height + 5 + delta Then DirsBox.Height = picResize.Top - PathLine.Height - 4 - 6 Else DirsBox.Height = 0
                picResize.Width = Me.Width - picResize.Left * 2 'DirsBox.Width

                ResizeBorder()

                btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - 6 'delta
                btnClose.Top = 6 'delta
                btnMinimize.Left = btnClose.Left - btnMinimize.Width - 5
                btnMinimize.Top = 6 'delta

                With ImagesBox
                    .Left = 0
                    .Top = picResize.Top + picResize.Height
                    .Height = Me.ClientRectangle.Height - picResize.Top - picResize.Height
                    .Width = Me.ClientRectangle.Width
                End With

                ImagesBox.OrderImages()
            Else
                PathLine.Width = Me.Width - 400
                btnRefresh.Left = PathLine.Width + PathLine.Left + 5
                btnSettings.Left = btnRefresh.Width + btnRefresh.Left + 6 + 6
                btnTempArea.Left = btnSettings.Width + btnSettings.Left + 6

                DirsBox.Width = Me.ClientRectangle.Width - delta * 2
                DirsBox.Left = delta
                If picResize.Top > PathLine.Height + 5 + delta Then DirsBox.Height = picResize.Top - PathLine.Height - 4 - 6 Else DirsBox.Height = 0
                picResize.Width = Me.Width - picResize.Left * 2 'DirsBox.Width

                Dim Tmp As Long = (Me.Width - (btnOrderBigThumbs.Left + 200))
                VoiceRecognizer.Width = Math.Max(Math.Min(Tmp * 0.4, 120), 39)
                OnAirVoiceRecognizer.Width = Math.Max(Math.Min(Tmp * 0.4, 120), 39)
                pnlSearchText.Width = Math.Max(Math.Min(Tmp * 0.6, 153), 80)
                VoiceRecognizer.Left = Me.ClientRectangle.Width - VoiceRecognizer.Width - 7
                OnAirVoiceRecognizer.Left = Me.ClientRectangle.Width - VoiceRecognizer.Width - 7
                cbSelectionMode.Left = VoiceRecognizer.Left - cbSelectionMode.Width - 5
                pnlSearchText.Left = cbSelectionMode.Left - pnlSearchText.Width - 5
                btnShowSeach.Left = pnlSearchText.Left - btnShowSeach.Width - 5

                ResizeBorder()

                btnClose.Left = Me.ClientRectangle.Width - btnClose.Width - 6 'delta
                btnClose.Top = 6 'delta
                btnMinimize.Left = btnClose.Left - btnMinimize.Width - 5
                btnMinimize.Top = 6 'delta

                With ImagesBox
                    .Left = delta
                    .Top = picResize.Top + picResize.Height
                    .Height = Me.ClientRectangle.Height - picResize.Top - picResize.Height - delta
                    .Width = Me.ClientRectangle.Width - delta * 2
                    .OrderImages()
                End With
            End If
        End If

        picResizeMaxY = Me.Height - picResize.Height - 1
        'MessageBox.Show(picResize.Height.ToString() + picResizeMinY.ToString())
        If picResize.Top > picResizeMinY Then
            If Me.Height - DirsBox.Top - picResize.Height < picResizeMinDelta * 2 Then
                If DirsBox.Path = "home" And Not cbDirs.value Then HideImagesBox() Else HideDirsBox()
                DirsBoxHeight = picResizeMaxY / 2 + DirsBox.Top
            Else
                If picResize.Top > picResizeMaxY Then
                    picResize.Top = picResizeMaxY
                Else
                    If picResize.Top > picResizeMaxY - picResizeMinDelta Or picResize.Top < picResizeMinY + picResizeMinDelta Then
                        DirsBoxHeight = picResizeMaxY / 2 + DirsBox.Top
                        If Not cbDirs.value Then ShowDirsBox()
                    End If
                End If
            End If
            Me.Refresh()
        End If
    End Sub
#End Region

#Region "Frame events (Resizing)"
    Dim ResizingPoint As Point
    Private Sub UserResizingRefresh()
        ImagesBox.Resizing(Me.ClientRectangle.Width - delta * 2, Me.ClientRectangle.Height - ImagesBox.Top - delta)
        ImagesBox.Refresh()
        ResizeAll()
    End Sub
    Private Sub Border_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeft.MouseDown, BorderRightTop.MouseDown, BorderLeftTop.MouseDown, BorderTop.MouseDown, BorderRight.MouseDown, BorderBottom.MouseDown, BorderRightBottom.MouseDown
        ResizingPoint = e.Location
        ImagesBox.ResizeStarted()
        'Me.TransparencyKey = Nothing
    End Sub
    Private Sub BorderLeft_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeft.MouseMove
        If e.Button Then
            Me.Left += -ResizingPoint.X + e.X
            Me.Width -= -ResizingPoint.X + e.X

            UserResizingRefresh()
        End If
    End Sub
    Private Sub BorderLeftTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeftTop.MouseMove
        If e.Button Then
            Me.Left += -ResizingPoint.X + e.X
            Me.Top += -ResizingPoint.Y + e.Y
            Me.Width -= -ResizingPoint.X + e.X
            Me.Height -= -ResizingPoint.Y + e.Y

            UserResizingRefresh()
        End If
    End Sub
    Private Sub BorderTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderTop.MouseMove
        If e.Button Then
            Me.Top += -ResizingPoint.Y + e.Y
            Me.Height -= -ResizingPoint.Y + e.Y

            UserResizingRefresh()
        End If
    End Sub
    'BorderRightBottom
    Private Sub BorderRightTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderRightTop.MouseMove
        If e.Button Then
            Me.Top += -ResizingPoint.Y + e.Y
            Me.Width += -ResizingPoint.X + e.X
            Me.Height -= -ResizingPoint.Y + e.Y
            'BorderRightTop.Left = Me.ClientRectangle.Width - 6

            UserResizingRefresh()
        End If
    End Sub
    Private Sub BorderRightBottom_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderRightBottom.MouseMove
        If e.Button Then
            'Me.Top += -ResizingPoint.Y + e.Y
            Me.Width += -ResizingPoint.X + e.X
            Me.Height += -ResizingPoint.Y + e.Y
            'BorderRightTop.Left = Me.ClientRectangle.Width - 6

            UserResizingRefresh()
        End If
    End Sub
    Private Sub BorderRight_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderRight.MouseMove
        If e.Button Then
            Me.Width += -ResizingPoint.X + e.X

            UserResizingRefresh()
        End If
    End Sub
    Private Sub BorderBottom_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderBottom.MouseMove
        If e.Button Then
            Me.Height += -ResizingPoint.Y + e.Y

            UserResizingRefresh()
        End If
    End Sub
    Private Sub BorderAny_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BorderLeft.MouseUp, BorderRightTop.MouseUp, BorderLeftTop.MouseUp, BorderTop.MouseUp, BorderRight.MouseUp, BorderBottom.MouseUp, BorderRightBottom.MouseUp
        ImagesBox.ResizeEnded()
        ResizeAll()
        'Me.TransparencyKey = Color.Magenta
    End Sub
#End Region

    Private Sub DirsBox_BackPressed() Handles DirsBox.BackPressed
        btnUp.btnDown()
        btnUp.Refresh()
    End Sub

    Private Async Sub DirsBox_DirChanged(ByVal Up As Boolean) Handles DirsBox.DirChanged
        PathLine.NewMaxPath(DirsBox.Path)
        Await refresh_files()
    End Sub

    Private Async Sub PathLine_PathChaged(ByVal Up As Boolean) Handles PathLine.PathChaged
        'DirsBox.NewPath(PathLine.MaxPath, PathLine.MaxPath)
        Await refresh_files()
        If DirsBox.Visible Then DirsBox.Focus() Else ImagesBox.Focus()
    End Sub




    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Player.settings.volume = 50


        ImagesBox.Left = delta
        ImagesBox.Top = picResize.Top + picResize.Height
        ImagesBox.Width = Me.ClientRectangle.Width - delta * 2
        ImagesBox.Height = Me.ClientRectangle.Height - ImagesBox.Top - delta
        SetCommonSettings()

        DirsBoxHeight = picResize.Top
        DirsBox.Width = Me.ClientRectangle.Width - DirsBox.Left * 2
        DirsBox.Height = picResize.Top - PathLine.Height - 5 - 6
        DirsBox.SetWire(76, 60, 5, 5)

        sbWireWidth.bar_width = 39
        sbWireWidth.b1 = New SolidBrush(Color.FromArgb(240, 240, 240))
        sbWireWidth.b2 = New SolidBrush(Color.FromArgb(19, 130, 206))
        sbWireWidth.b3 = New SolidBrush(Color.FromArgb(240, 240, 240))
        sbWireWidthBig.b1 = New SolidBrush(Color.FromArgb(240, 240, 240))
        sbWireWidthBig.b2 = New SolidBrush(Color.FromArgb(19, 130, 206))
        sbWireWidthBig.b3 = New SolidBrush(Color.FromArgb(240, 240, 240))
        'UcScrollBar1.b3 = New SolidBrush(Color.FromArgb(240, 240, 240))
        'UcScrollBar1.max = 200
        'UcScrollBar1.value = 30
        sbBGColor.min = 0
        sbBGColor.max = 255
        sbBGColor.value = 240
        sbBGColor.b3 = New SolidBrush(Color.FromArgb(215, 215, 215))
        sbBGColor.ShowText = True
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

        ImagesBox.ResizeEnded()
    End Sub



    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ImagesBox.DrawShadow = cbShadow.value
        ImagesBox.NextFrame(True)
    End Sub

    Private Async Sub ImagesBox_BackSpaceKey(TurnOffTheButtonAutomatically As Boolean) Handles ImagesBox.BackSpaceKey
        btnUp.btnDown()
        btnUp.Refresh()

        PathLine.GoUp()
        Await refresh_files()

        If TurnOffTheButtonAutomatically Then
            btnUp.btnNormal()
            btnUp.Refresh()
        End If
    End Sub

    Private Sub ImagesBox_SendFocusToTheTop(ByRef Done As System.Boolean) Handles ImagesBox.SendFocusToTheTop
        If cbDirs.value = False Then
            If picResize.Top = picResizeMinY Then
                If DirsBox.NDirs > 0 Then ShowDirsBox() Else Done = False
            Else
                DirsBox.Select()
            End If
        End If
    End Sub
    Private Sub DirsBox_SendFocusToTheBottom(ByRef Done As Boolean) Handles DirsBox.SendFocusToTheBottom
        If ImagesBox.NImages > 0 Then ImagesBox.Select() : Done = True Else Done = False
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ImagesBox.ShowImagesName = cbNames.value
    End Sub

    Private Sub btnMinimize_Click() Handles btnMinimize.Clicked
        Me.WindowState = FormWindowState.Minimized
    End Sub


    Private Declare Function timeBeginPeriod Lib "winmm.dll" (ByVal uPeriod As Long) As Long
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        timeBeginPeriod(1)
    End Sub

    Sub ChangePicResizeSize()
        If picResize.Height = 4 Then
            picResize.Height = 37
        Else
            picResize.Height = 4
        End If
        ResizeAll()
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


    'Private Sub UcScrollBar1_ValueChanged(ByVal Value As Long) Handles UcScrollBar1.ValueChanged
    '    Dim prev_y As Long = ImagesBox.Wire.Y
    '    If Value < 100 Then
    '        If Value >= 50 Then
    '            ImagesBox.SetWire(Value * 4, 32, 5, 5)
    '        Else
    '            ImagesBox.SetWire((Value + 50) * 6 - 150, 16, 5, 5)
    '        End If
    '    Else
    '        Dim t As Long = (Value - 100) * 3 + 60
    '        ImagesBox.SetWire(t, t, 10, 10)
    '    End If
    '    If prev_y <> ImagesBox.Wire.Y Then
    '        ImagesBox.ReloadAllThumbs()
    '    Else
    '        ImagesBox.ReloadWidthWithText = True
    '    End If
    '    ImagesBox.OrderImages()

    '    txtH.Text = ImagesBox.Wire.Y.ToString
    '    txtW.Text = ImagesBox.Wire.X.ToString
    '    ImagesBox.Select()
    'End Sub

    'Private Sub frmMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
    '    Me.BackColor = Color.Green
    '    MsgBox("L")
    'End Sub

    'Private Sub frmMain_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
    '    Me.BackColor = Color.FromArgb(200, 200, 200)
    'End Sub

    Private Sub ImagesBox_FillScreenMe() Handles ImagesBox.FillScreenMe
        If picResize.Top = picResizeMinY Then
            ShowDirsBox()
        Else
            HideDirsBox()
        End If
    End Sub
    Private Sub UcScrollBar2_ValueChanged(ByVal Value As Long) Handles sbBGColor.ValueChanged
        ImagesBox.BGColor = Color.FromArgb(Value, Value, Value)
        DirsBox.BackColor = Color.FromArgb(Value, Value, Value)
        DirsBox.RedrawFolders()
        DirsBox.RedrawComposed()
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
        pnl_settings.Height = 500
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ImagesBox.Animation = Not ImagesBox.Animation
    End Sub

#Region "In HOME mode files showing"
    Dim Files() As String
    Public Sub SearchSomeRecentFiles()
        Dim MaxFilesIn As Short = 99
        Dim N As Short = -1
        Dim Path As String
        ReDim Files(MaxFilesIn)
        For i As Long = FileTags.N To 1 Step -1
            Path = FileTags.Files(i)
            If (IO.File.Exists(Path)) Then
                N += 1
                Files(N) = Path
            End If
            If i Mod 100 = 1 Then RefreshProgress(N / MaxFilesIn, "Searching recent files")
            If N = MaxFilesIn Then Exit For
        Next
        If N < MaxFilesIn Then ReDim Preserve Files(N)
    End Sub
    Private Sub AfterSearchSomeRecentFiles(workID As IAsyncResult)
        If (InvokeRequired) Then
            Invoke(New AsyncCallback(AddressOf AfterSearchSomeRecentFiles), New Object() {workID})
            Return
        Else
            SetCommonSettings()
            ImagesBox.Refine(Files)
            ImagesBox.Focus()
            FilesFilter.MusicCount = 0 : FilesFilter.ImagesCount = 0 : FilesFilter.CustomCount = 0
            FilesFilter.AllCount = ImagesBox.NImages

            Me.Text = "recent files"

            UcWaitingIndicator1.Enabled = False
        End If
        Dim worker As DoWorkDelegate = workID.AsyncState
        worker.EndInvoke(workID)
    End Sub

    Public Sub SearchSomeImages()
        Dim MaxFilesIn As Short = 99
        Dim N As Short = -1
        ReDim Files(MaxFilesIn)
        For i As Long = FileTags.N To 1 Step -1
            If (FileTags.Tags(i).Type = "image" And IO.File.Exists(FileTags.Files(i))) Then
                N += 1
                Files(N) = FileTags.Files(i)
            End If
            If i Mod 100 = 1 Then RefreshProgress(N / MaxFilesIn, "Searching images")
            If N = MaxFilesIn Then Exit For
        Next
        If N < MaxFilesIn Then ReDim Preserve Files(N)
    End Sub
    Private Sub AfterSearchSomeImages(workID As IAsyncResult)
        If (InvokeRequired) Then
            Invoke(New AsyncCallback(AddressOf AfterSearchSomeImages), New Object() {workID})
            Return
        Else
            'ImagesBox.SortImagesByLaunchingCount()
            'ImagesBox.MakeAllThumbnails()
            'ImagesBox.SetImagesLocation()
            'ImagesBox.OrderImages()
            SetOnlyImagesSettings()
            ImagesBox.Refine(Files)
            ImagesBox.Focus()
            FilesFilter.MusicCount = 0 : FilesFilter.AllCount = 0 : FilesFilter.CustomCount = 0
            FilesFilter.ImagesCount = ImagesBox.NImages

            Me.Text = "Last images"

            UcWaitingIndicator1.Enabled = False
        End If
        Dim worker As DoWorkDelegate = workID.AsyncState
        worker.EndInvoke(workID)
    End Sub

    Public Sub SearchSomeMusic()
        Dim MaxFilesIn As Short = 99
        Dim N As Short = -1
        ReDim Files(MaxFilesIn)
        For i As Long = FileTags.N To 1 Step -1
            If (FileTags.Tags(i).Type = "music" And IO.File.Exists(FileTags.Files(i))) Then
                N += 1
                Files(N) = FileTags.Files(i)
            End If
            If i Mod 30 = 1 Then RefreshProgress(N / MaxFilesIn, "Searching music")
            If N = MaxFilesIn Then Exit For
        Next
        If N < MaxFilesIn Then ReDim Preserve Files(N)
    End Sub
    Private Sub AfterSearchSomeMusic(workID As IAsyncResult)
        If (InvokeRequired) Then
            Invoke(New AsyncCallback(AddressOf AfterSearchSomeMusic), New Object() {workID})
            Return
        Else
            'ImagesBox.SortImagesByLaunchingCount()
            'ImagesBox.MakeAllThumbnails()
            'ImagesBox.SetImagesLocation()
            'ImagesBox.OrderImages()
            SetOnlyMusicSettings()
            ImagesBox.Refine(Files)
            ImagesBox.Focus()
            FilesFilter.ImagesCount = 0 : FilesFilter.AllCount = 0 : FilesFilter.CustomCount = 0
            FilesFilter.MusicCount = ImagesBox.NImages

            Me.Text = "Last music files"

            UcWaitingIndicator1.Enabled = False
        End If
        Dim worker As DoWorkDelegate = workID.AsyncState
        worker.EndInvoke(workID)
    End Sub

    Private Sub RefreshProgress(Progress As Double, FormText As String)
        If (InvokeRequired) Then
            Invoke(New ReportProgressDelegate(AddressOf RefreshProgress), New Object() {Progress, FormText})
        Else
            UcWaitingIndicator1.Rings(0).Lenght = Progress * 320 + 10
            UcWaitingIndicator1.Rings(1).Lenght = Progress * 320 + 10
            UcWaitingIndicator1.Rings(2).Lenght = Progress * 320 + 10
            Me.Text = Math.Round(Progress * 100).ToString + "% " + FormText
        End If
    End Sub
    Private Sub AddFiles(Path() As String)
        If (InvokeRequired) Then
            Invoke(New AddFilesDelegate(AddressOf AddFiles), New Object() {Path})
        Else
            'For Each s As String In Path
            '    ImagesBox.AddImage(s)
            'Next
            ImagesBox.Refine(Path)
        End If
    End Sub
    Private Delegate Sub ReportProgressDelegate(Progress As Double, FormText As String)
    Private Delegate Sub AddFilesDelegate(Path() As String)
    Private Delegate Sub DoWorkDelegate()
#End Region

    Private Async Sub FilesFilter_ValueChanged() Handles FilesFilter.ValueChanged
        If (txtSearchWhat.Text = "") Then
            If PathLine.MaxPath = "home" Then
                HideDirsBox()

                UcWaitingIndicator1.Enabled = True
                ImagesBox.StopLoading = True
                'ImagesBox.ClearImages()

                If FilesFilter.value = 3 Then
                    ImagesBox.Refine(RecentFiles.GetAllItems())
                    UcWaitingIndicator1.Enabled = False
                ElseIf FilesFilter.value = 2 Then
                    Dim worker As DoWorkDelegate = New DoWorkDelegate(AddressOf SearchSomeMusic)
                    worker.BeginInvoke(New AsyncCallback(AddressOf AfterSearchSomeMusic), worker)
                ElseIf FilesFilter.value = 1 Then
                    Dim worker As DoWorkDelegate = New DoWorkDelegate(AddressOf SearchSomeImages)
                    worker.BeginInvoke(New AsyncCallback(AddressOf AfterSearchSomeImages), worker)
                ElseIf FilesFilter.value = 0 Then
                    Dim worker As DoWorkDelegate = New DoWorkDelegate(AddressOf SearchSomeRecentFiles)
                    worker.BeginInvoke(New AsyncCallback(AddressOf AfterSearchSomeRecentFiles), worker)
                End If
            Else
                Await refine_files()
                ImagesBox.Select()
            End If
        Else
            txtSearchWhat_TextChanged(New Object(), New System.EventArgs())
        End If
    End Sub




    Private Sub UcSkinButton1_Clicked() Handles btnOrderBigThumbs.Clicked
        ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Нorizontal
        'ImagesBox.Canvas.X = 0
        If txtH.Text <> 60 Then
            txtH.Text = 60
            txtW.Text = 60
            ImagesBox.SetWire(60, 60, 10, 10)
            'UcScrollBar1.value = (60 - 60) / 3 + 100 'UcScrollBar1.value = (K - 60) / 3 + 100
            sbBorder.value = 3
        Else
            txtH.Text = 180
            txtW.Text = 180
            ImagesBox.SetWire(180, 180, 10, 10)
            'UcScrollBar1.value = (180 - 60) / 3 + 100
            sbBorder.value = 6
        End If

        cbShadow.value = False
        cbNames.value = False

        ImagesBox.DrawShadow = False
        ImagesBox.Wire.Border = sbBorder.value
        ImagesBox.ShowImagesName = False

        ImagesBox.StopLoading = True
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()

        ImagesBox.Select()
    End Sub

    Private Sub btnOrderList_Clicked() Handles btnOrderList.Clicked
        ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Vertical
        'ImagesBox.Canvas.Y = 0
        If txtH.Text <> 16 Then
            txtH.Text = 16
            txtW.Text = 360
            ImagesBox.SetWire(300, 16, 5, 5)
            'UcScrollBar1.value = 35
        Else
            txtH.Text = 32
            txtW.Text = 300
            ImagesBox.SetWire(300, 32, 5, 5)
            'UcScrollBar1.value = 75
        End If
        sbBorder.value = 1
        cbShadow.value = False
        cbNames.value = False

        ImagesBox.DrawShadow = False
        ImagesBox.Wire.Border = 1
        ImagesBox.ShowImagesName = False

        ImagesBox.StopLoading = True
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()
        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.NotSimple

        ImagesBox.Select()
    End Sub


    Private Async Sub cbDirs_ValueChanged(ByVal Value As Boolean) Handles cbDirs.ValueChanged
        Try
            If Value = True Then
                HideDirsBox()
                Await refine_files() 'refresh_files()
            End If
        Catch
        End Try
    End Sub

    Private Sub btnShowSeach_Clicked() Handles btnShowSeach.Clicked
        pnlSearch.Top = picResize.Top - 1 '- pnlSearch.Height + 40
        'txtSearchWhere.Text = DirsBox.Path
        txtSearchWhat.Text = ""
        pnlSearch.Visible = Not pnlSearch.Visible
        txtSearchWhat.Select()
    End Sub

    Private Sub btnMakeSID_Clicked() Handles btnMakeSID.Clicked
        Cursor = Cursors.WaitCursor
        cbSID.value = True
        make_sid()
        cbSID.value = False
        Cursor = Cursors.Arrow
    End Sub

    Private Sub btnSeach_Clicked() Handles btnSeach.Clicked
        pnlSearch.Visible = False
    End Sub

    Private Sub btnSortByName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSortByName.Click
        frmMusicPlayer.Show()
    End Sub

    Private Sub btnSettings_Clicked() Handles btnSettings.Clicked
        pnl_settings.Left = btnSettings.Left ' - pnl_settings.Width / 2
        pnl_settings.Visible = Not pnl_settings.Visible
    End Sub

    Private Sub btnTempArea_Clicked() Handles btnTempArea.Clicked
        LoadTempArea()
    End Sub

    Private Sub LoadTempArea()
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

    Dim WireWidthStart As Long
    Dim WireWidthStartPoint As Point
    Dim frmWireWidth As frmSelect = New frmSelect()
    Dim frmWireWidthShown As Boolean = False
    Private Sub btnOrderList_MouseDown(sender As Object, e As MouseEventArgs) Handles btnOrderList.MouseDown
        WireWidthStartPoint = e.Location
        sbWireWidth.value = (ImagesBox.Wire.X - 100) / 3
        frmWireWidth.Value = 78 - ImagesBox.Wire.Y
        'ImagesBox.FlyingAlgorithm = "simple"
        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.NotSimple
    End Sub
    Private Sub btnOrderList_MouseMove(sender As Object, e As MouseEventArgs) Handles btnOrderList.MouseMove
        If e.Button Then
            If sbWireWidth.Visible = False And frmWireWidthShown = False Then
                If (WireWidthStartPoint.X - e.X) ^ 2 + (WireWidthStartPoint.Y - e.Y) ^ 2 > 10 Then
                    If Math.Abs(WireWidthStartPoint.X - e.X) > Math.Abs(WireWidthStartPoint.Y - e.Y) Then
                        pnlWireWidth.Left = -sbWireWidth.Left + btnOrderList.Left - sbWireWidth.value
                        pnlWireWidth.Top = btnOrderList.Top + btnOrderList.Parent.Top - sbWireWidth.Top - 1

                        WireWidthStart = sbWireWidth.value - e.X
                        pnlWireWidth.Visible = True
                    Else
                        With frmWireWidth
                            .Left = btnOrderList.Left + btnOrderList.Parent.Left + btnOrderList.Parent.Parent.Left - 5
                            .Top = btnOrderList.Top + btnOrderList.Parent.Top + btnOrderList.Parent.Parent.Top - 5 - frmWireWidth.Value '- frmWireWidth.Height
                            .Width = 10 + btnOrderList.Width
                            .Height = 120 - 3

                            .BackgroundImage = New Bitmap("scroll_bar\bg.png")  'New Bitmap(.Width, .Height)
                            Dim g As Graphics = Graphics.FromImage(.BackgroundImage)
                            'g.FillRectangle(New SolidBrush(Color.FromArgb(200, 200, 200)), 1, 1, .Width - 2, .Height - 2)
                            'g.DrawRectangle(Pens.Black, 0, 0, .Width - 1, .Height - 1)

                            'g.FillRectangle(New SolidBrush(Color.FromArgb(240, 240, 240)), 8, 8, .Width - 16, .Height - 16)
                            'g.DrawImage(btnOrderList.GetMDPic, 7, 7 + frmWireWidth.Value)
                            'g.DrawRectangle(Pens.Black, 7, 7, .Width - 1 - 14, .Height - 1 - 14)
                            .Show()

                            .Width = 10 + btnOrderList.Width
                            .Height = 120 - 7 - 4
                        End With
                        frmWireWidthShown = True
                        WireWidthStart = frmWireWidth.Value - e.Y
                    End If
                End If
            End If
            If sbWireWidth.Visible Then
                sbWireWidth.value = WireWidthStart + e.X

                ImagesBox.SetWire(sbWireWidth.value * 3 + 100, ImagesBox.Wire.Y, 7, 5)

                ImagesBox.ReloadWidthWithText = True
                ImagesBox.OrderImages()

                txtH.Text = ImagesBox.Wire.Y.ToString : txtW.Text = ImagesBox.Wire.X.ToString
                ImagesBox.Select()
            End If
            If frmWireWidthShown Then
                With frmWireWidth
                    frmWireWidth.Value = WireWidthStart + e.Y
                    If frmWireWidth.Value < 0 Then frmWireWidth.Value = 0
                    If frmWireWidth.Value > 70 Then frmWireWidth.Value = 70

                    Dim dx As Short = 13
                    Dim dy As Short = 6

                    .BackgroundImage = New Bitmap("scroll_bar\bg.png")
                    Dim g As Graphics = Graphics.FromImage(.BackgroundImage)
                    Dim IndicatorColor As Color = Color.FromArgb(110, 110, 110) 'Color.FromArgb(19, 130, 206) 

                    'Dim StartOfNoborder As Short = btnOrderList.Top + btnOrderList.Parent.Top + btnOrderList.Parent.Parent.Top - 4 - frmWireWidth.Top - 1
                    'Dim HeightOfNoborder As Short = picResize.Height + 1
                    'If picResize.Top = picResizeMinY Then StartOfNoborder -= 34 : HeightOfNoborder += 34
                    'StartOfNoborder += 1 : HeightOfNoborder -= 2
                    'g.DrawLine(New Pen(Color.FromArgb(100, 200, 200, 200)), 0, StartOfNoborder, 0, HeightOfNoborder + StartOfNoborder)
                    'g.DrawLine(New Pen(Color.FromArgb(100, 200, 200, 200)), .Width - 1, StartOfNoborder, .Width - 1, HeightOfNoborder + StartOfNoborder)

                    Dim yPos As Short = frmWireWidth.Value + dy + 30 - 2
                    If yPos > .Height - 13 Then yPos = .Height - 13
                    g.FillRectangle(New SolidBrush(IndicatorColor), 7 + dx, yPos, .Width - 16 + 2 - dx * 2, .Height - frmWireWidth.Value - 46)
                    g.DrawLine(New Pen(Color.FromArgb(140, 0, 0, 0)), 7 + dx, yPos, 7 + dx + 8, yPos)
                    g.DrawLine(New Pen(Color.FromArgb(50, 0, 0, 0)), 7 + dx, yPos + 1, 7 + dx + 8, yPos + 1)
                    g.DrawLine(New Pen(Color.FromArgb(70, 0, 0, 0)), 7 + dx, yPos, 7 + dx, .Height - 13)
                    g.DrawLine(New Pen(Color.FromArgb(70, 0, 0, 0)), 7 + dx + 8, yPos, 7 + dx + 8, .Height - 13)

                    g.DrawImage(btnOrderList.GetMDPic, 5, 5 + frmWireWidth.Value)

                    .Refresh()

                    ImagesBox.Wire.X /= ImagesBox.font_filename.Size / 100
                    If frmWireWidth.Value < 15 Then
                        ImagesBox.font_filename = New Font("Lucida Sans Unicode", 24, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_singer = New Font("Lucida Sans Unicode", 22, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_bold = New Font(ImagesBox.font_singer, FontStyle.Bold)
                    ElseIf frmWireWidth.Value < 25 Then
                        ImagesBox.font_filename = New Font("Lucida Sans Unicode", 15, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_singer = New Font("Lucida Sans Unicode", 14, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_bold = New Font(ImagesBox.font_singer, FontStyle.Bold)
                    ElseIf frmWireWidth.Value < 35 Then
                        ImagesBox.font_filename = New Font("Lucida Sans Unicode", 13, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_singer = New Font("Lucida Sans Unicode", 12, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_bold = New Font(ImagesBox.font_singer, FontStyle.Bold)
                    Else
                        ImagesBox.font_filename = New Font("Lucida Sans Unicode", 11, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_singer = New Font("Lucida Sans Unicode", 10, FontStyle.Regular, GraphicsUnit.Pixel)
                        ImagesBox.font_bold = New Font(ImagesBox.font_singer, FontStyle.Bold)
                    End If
                    ImagesBox.Wire.X /= 100 / ImagesBox.font_filename.Size
                    ImagesBox.SetWire(ImagesBox.Wire.X, 78 - frmWireWidth.Value, ImagesBox.Wire.dX, ImagesBox.Wire.dY)
                    ImagesBox.ReloadAllThumbs()
                    ImagesBox.OrderImages()

                    ImagesBox.Select()
                End With
            End If
        End If
    End Sub
    Private Sub btnOrderList_MouseUp(sender As Object, e As MouseEventArgs) Handles btnOrderList.MouseUp
        If pnlWireWidth.Visible Then pnlWireWidth.Visible = False
        If frmWireWidthShown Then frmWireWidth.Hide() : frmWireWidthShown = False
    End Sub




    Dim WireWidthBigStart As Long
    Dim WireWidthBigStartPoint As Point
    Private Sub btnOrderBigThumbs_MouseDown(sender As Object, e As MouseEventArgs) Handles btnOrderBigThumbs.MouseDown
        WireWidthBigStartPoint = e.Location
        sbWireWidthBig.value = (ImagesBox.Wire.Y - 60) / 3
        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.Simple
    End Sub
    Private Sub btnOrderBigThumbs_MouseMove(sender As Object, e As MouseEventArgs) Handles btnOrderBigThumbs.MouseMove
        If e.Button Then
            If sbWireWidthBig.Visible = False Then
                If (WireWidthBigStartPoint.X - e.X) ^ 2 + (WireWidthBigStartPoint.Y - e.Y) ^ 2 > 10 Then
                    pnlWireWidthBig.Left = -sbWireWidthBig.Left + sender.Left - sbWireWidthBig.value
                    pnlWireWidthBig.Top = btnOrderList.Top + btnOrderList.Parent.Top - sbWireWidthBig.Top - 1

                    WireWidthBigStart = sbWireWidthBig.value - e.X
                    pnlWireWidthBig.Visible = True
                End If
            Else
                sbWireWidthBig.value = WireWidthBigStart + e.X

                ImagesBox.SetWire(sbWireWidthBig.value * 3 + 60, sbWireWidthBig.value * 3 + 60, 5 + sbWireWidthBig.value / 5, 5 + sbWireWidthBig.value / 5)
                ImagesBox.ReloadAllThumbs()
                ImagesBox.OrderImages()

                txtH.Text = ImagesBox.Wire.Y.ToString
                txtW.Text = ImagesBox.Wire.X.ToString
                ImagesBox.Select()
            End If
        End If
    End Sub
    Private Sub btnOrderBigThumbs_MouseUp(sender As Object, e As MouseEventArgs) Handles btnOrderBigThumbs.MouseUp
        pnlWireWidthBig.Visible = False
    End Sub

    Private Async Sub ImagesBox_ChangeDir(ByVal Path As String, ByVal a As Boolean) Handles ImagesBox.ChangeDir
        If Mid(Path, Path.Length() - 1, 1) <> "\" Then Path += "\"
        'DirsBox.NewPath(Path, Path)
        PathLine.NewMaxPath(Path)
        Await refresh_files()
        If a Then ImagesBox.Select()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ImagesBox.OrderImagesDifferentWidth()
        'Dim r As Microsoft.Win32.RegistryKey
        'r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".mp3")
        'Dim Alias1 As String = r.GetValue("")
        'r.Close()
        'r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Alias1) 'r.OpenSubKey(Alias1 + "\DefaultIcon")
        'r = r.OpenSubKey("DefaultIcon")
        'MsgBox(r.GetValue(""))
        'r.Close()
    End Sub



    Private Sub txtSearchWhat_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearchWhat.KeyDown
        If e.KeyCode = Keys.Down Then
            ImagesBox.Select()
        ElseIf e.KeyCode = Keys.Up Then
            DirsBox.Select()
        ElseIf e.KeyCode = Keys.F And e.Control = True Then
            e.Handled = True
        End If
    End Sub
    Private Sub cbRounded_ValueChanged(Value As Boolean) Handles cbRounded.ValueChanged
        DirsBox.RoundedElements = Value
        ImagesBox.RoundedElements = Value
    End Sub

    Private Sub ImagesBox_GoForvard(TurnOffTheButtonAutomatically As Boolean) Handles ImagesBox.GoForvard
        PathLine.NewPathByIndex(PathLine.NDirs + 1)
    End Sub

    Private Sub cbMakeThumbs_ValueChanged(Value As Boolean) Handles cbMakeThumbs.ValueChanged
        ImagesBox.MakeThumbnails = Value
    End Sub

    Private Sub UcCheckBox1_ValueChanged(Value As Boolean) Handles UcCheckBox1.ValueChanged
        DirsBox.DrawBorders = Value
        ImagesBox.DrawBorders = Value
    End Sub

#Region "Finding"
    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles pnlSearchText.Click
        txtSearchWhat.Select()
    End Sub
    Private Sub txtSearchWhat_GotFocus(sender As Object, e As EventArgs) Handles txtSearchWhat.GotFocus
        txtSearchWhat.BackColor = Color.White
        pnlSearchText.BackColor = Color.White
    End Sub
    Private Sub txtSearchWhat_LostFocus(sender As Object, e As EventArgs) Handles txtSearchWhat.LostFocus
        txtSearchWhat.BackColor = Color.WhiteSmoke
        pnlSearchText.BackColor = Color.WhiteSmoke
    End Sub
    Private Sub txtSearchWhat_TextChanged(sender As Object, e As EventArgs) Handles txtSearchWhat.TextChanged
        If IndexReady = False And Not bgwMakeIndex.IsBusy Then
            With UcWaitingIndicator1
                .Enabled = True
                .InitRings({1.1, -2, 3.2, 1.9, -1.4}, {3, 6, 9, 12, 15})
                .Rings(0).BaseColor = Color.FromArgb(70, 70, 70)
                .Rings(1).BaseColor = Color.FromArgb(80, 80, 80)
                .Rings(2).BaseColor = Color.FromArgb(50, 50, 50)
                .Rings(3).BaseColor = Color.FromArgb(90, 90, 90)
                .Rings(4).BaseColor = Color.FromArgb(60, 60, 60)
                .penMain.Width = 2
                .Enabled = True
            End With
            picBuildingIndexProgress.Width = 0
            picBuildingIndexProgress.Visible = True
            bgwMakeIndex.RunWorkerAsync()
        End If
        picProgressSearch.Visible = False
        UcWaitingIndicator2.Enabled = False
        While bgwSearch.IsBusy
            bgwSearch.CancelAsync()
            Application.DoEvents()
        End While

        If txtSearchWhat.Text <> "" Then
            ImagesBox.StopLoading = True

            NameForSearch = txtSearchWhat.Text.ToLower
            NameForSearch += " " + TranslateToRus(NameForSearch) + " " + TranslateToEng(NameForSearch)

            'MsgBox(NameForSearch)
            NamesPlus = NameForSearch.Split({CChar("."), CChar(","), CChar("_"), CChar(" "), CChar("&")})
            'For j As Long = 0 To NamesPlus.Length - 1
            'Next

            SearchFailed = False
            SearchCancelled = False
            picProgressSearch.Width = 0
            picProgressSearch.BackColor = Color.FromArgb(19, 130, 206)
            picProgressSearch.Visible = True
            If Not UcWaitingIndicator2.Enabled Then
                With UcWaitingIndicator2
                    If Not UcWaitingIndicator2.Visible Then .InitRings({3.8, -1.3, 1.7}, {3, 6, 9}) : .penMain.Width = 2
                    '.InitRings({1.1, -2, 3.2, 1.9}, {2, 4, 6, 8}) : .penMain.Width = 1.2
                    .Enabled = True
                End With
            End If
            Me.Text = Chr(34) + txtSearchWhat.Text + Chr(34) + " - searching"
            bgwSearch.RunWorkerAsync()
        End If
    End Sub

    Private Function TranslateToRus(S As String) As String
        Dim strEng As String = "qwertyuiop[]asdfghjkl;'zxcvbnm,./"
        Dim strRus As String = "йцукенгшщзхъфывапролджэячсмитьбю."
        Dim ch As String
        Dim pos As Short
        For i As Short = 1 To S.Length
            ch = Mid(S, i, 1)
            pos = InStr(strEng, ch)
            If pos > 0 Then S = Mid(S, 1, i - 1) + Mid(strRus, pos, 1) + Mid(S, i + 1)
        Next
        Return S
    End Function
    Private Function TranslateToEng(S As String) As String
        Dim strEng As String = "йцукенгшщзхъфывапролджэячсмитьбю."
        Dim strRus As String = "qwertyuiop[]asdfghjkl;'zxcvbnm,./"
        Dim ch As String
        Dim pos As Short
        For i As Short = 1 To S.Length
            ch = Mid(S, i, 1)
            pos = InStr(strEng, ch)
            If pos > 0 Then S = Mid(S, 1, i - 1) + Mid(strRus, pos, 1) + Mid(S, i + 1)
        Next
        Return S
    End Function

    Dim NameForSearch As String
    Dim NamesPlus() As String, NamesMinus() As String
    Dim FoundedIndexes(100000) As Long, FoundedRelevance(100000) As Double, NFounded As Long
    Dim FoundedFiles(100000) As String, NFoundedFiles As Long
    Dim SearchFailed As Boolean, SearchCancelled As Boolean

    Private Sub bgwSearch_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwSearch.ProgressChanged
        picProgressSearch.Width = e.ProgressPercentage / 100 * (pnlSearchText.Width - 2)
        Me.Text = Chr(34) + txtSearchWhat.Text + Chr(34) + " - searching (" + e.ProgressPercentage.ToString() + ")"
        picProgress.Refresh()
    End Sub
    Dim FinderAns As String
    Private Sub bgwSearch_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwSearch.DoWork
        Try
            NFounded = -1
            If IndexReady Then
                Dim n As Long
                Dim pos As Short
                Dim Delta As Long = IndexCount / 10
                For n = 1 To IndexCount
                    For j As Long = 0 To NamesPlus.Length - 1
                        'If Index(n).Keyword = NamesPlus(j) And NamesPlus(j) <> "" Then
                        If NamesPlus(j) <> "" Then
                            pos = InStr(Index(n).Keyword, NamesPlus(j))
                            If pos > 0 Then
                                For i As Long = 1 To Index(n).Count
                                    If pos = 1 Then
                                        Accumulate(Index(n).Files(i), NamesPlus(j).Length / Index(n).Keyword.Length)
                                        'If NamesPlus(j) = "curs" Then MsgBox(NFounded.ToString())
                                    Else
                                        Accumulate(Index(n).Files(i), NamesPlus(j).Length / Index(n).Keyword.Length / Index(n).Keyword.Length)
                                    End If
                                Next
                            End If
                        End If
                    Next
                    If n Mod Delta = 0 Then bgwSearch.ReportProgress(60 * n / IndexCount)
                    If bgwSearch.CancellationPending Then SearchCancelled = True : Exit Sub
                Next
                bgwSearch.ReportProgress(60)
                'Dim tmp As Long
                'For j As Long = 0 To NFounded
                '    For i As Long = 0 To NFounded - 1
                '        If FoundedRelevance(i) < FoundedRelevance(i + 1) Then
                '            tmp = FoundedRelevance(i)
                '            FoundedRelevance(i) = FoundedRelevance(i + 1)
                '            FoundedRelevance(i + 1) = tmp

                '            tmp = FoundedIndexes(i)
                '            FoundedIndexes(i) = FoundedIndexes(i + 1)
                '            FoundedIndexes(i + 1) = tmp
                '        End If
                '    Next
                '    If j Mod 100 = 0 Then bgwSearch.ReportProgress(60 + 30 * j / (NFounded + 1))
                '    If bgwSearch.CancellationPending Then SearchCancelled = True : Exit Sub
                'Next
                bgwSearch.ReportProgress(90)
                Dim Path As String
                For i As Long = 0 To NFounded
                    Path = FileTags.Files(FoundedIndexes(i))
                    If Not IO.File.Exists(Path) Then
                        FoundedIndexes(i) = -1
                    End If
                    If i Mod 100 = 0 Then bgwSearch.ReportProgress(90 + 10 * i / (NFounded + 1))
                    If bgwSearch.CancellationPending Then SearchCancelled = True : Exit Sub
                Next
                bgwSearch.ReportProgress(100)
            Else
                NFoundedFiles = 0
                Dim Delta As Long = FileTags.N / 100
                For i As Long = 1 To FileTags.N
                    If InStr(FileTags.Files(i), NameForSearch) > 0 Then
                        If IO.File.Exists(FileTags.Files(i)) Then
                            NFoundedFiles += 1
                            FoundedFiles(NFoundedFiles) = FileTags.Files(i)
                        End If
                    ElseIf FileTags.Tags(i).Type = "music" Then
                        If InStr(FileTags.Tags(i).Song.Name.ToLower, NameForSearch) > 0 Or InStr(FileTags.Tags(i).Song.Singer.ToLower, NameForSearch) > 0 Then
                            If IO.File.Exists(FileTags.Files(i)) Then
                                NFoundedFiles += 1
                                FoundedFiles(NFoundedFiles) = FileTags.Files(i)
                            End If
                        End If
                    End If
                    If i Mod Delta = 0 Then bgwSearch.ReportProgress(100 * i / FileTags.N)
                    If bgwSearch.CancellationPending Then SearchCancelled = True : Exit Sub
                Next
                bgwSearch.ReportProgress(100)
            End If
        Catch ex As Exception
            SearchFailed = True
            txtSearchWhat.Text = ex.ToString
        End Try
    End Sub
    Private Sub Accumulate(ByRef NewIndex As Long, Relevance As Double)
        Dim AlreadyExists As Long = -1
        For i As Long = 0 To NFounded
            If FoundedIndexes(i) = NewIndex Then AlreadyExists = i : Exit For
        Next
        If AlreadyExists = -1 Then
            NFounded += 1
            FoundedRelevance(NFounded) = Relevance
            FoundedIndexes(NFounded) = NewIndex
        Else
            FoundedRelevance(AlreadyExists) += Relevance
        End If
    End Sub
    Private Sub Accumulate(ByRef NewIndex As Long)
        Dim AlreadyExists As Long = -1
        For i As Long = 0 To NFounded
            If FoundedIndexes(i) = NewIndex Then AlreadyExists = i : Exit For
        Next
        If AlreadyExists = -1 Then
            NFounded += 1
            FoundedRelevance(NFounded) = 1
            FoundedIndexes(NFounded) = NewIndex
        Else
            FoundedRelevance(AlreadyExists) += 1
        End If
    End Sub
    Private Sub bgwSearch_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwSearch.RunWorkerCompleted
        If bgwSearch.CancellationPending = False And e.Cancelled = False And SearchFailed = False And SearchCancelled = False Then
            'TextBox1.Text = FinderAns
            'Button5.Text += "^"
            Me.Text = Chr(34) + txtSearchWhat.Text + Chr(34) + " - search results"
            UcWaitingIndicator2.Enabled = False
            picProgressSearch.Visible = False

            ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Vertical
            ImagesBox.Path = ""
            ImagesBox.StopLoading = True
            ImagesBox.DrawShadow = False
            ImagesBox.ShowImagesName = True
            ImagesBox.Wire.Border = 5
            ImagesBox.SetWire(Me.Width / 2, 16, 7, 5)
            ImagesBox.SetWire(200, 16, 5, 5)

            If IndexReady Then
                Dim Path As String
                Dim NFoundedToShow As Long = 0
                Dim FoundedList(201) As String

                'If NFounded = 0 Then MsgBox("NOTHING")


                If FilesFilter.value = 1 Then
                    For i As Long = 0 To NFounded
                        If FoundedIndexes(i) <> -1 Then
                            If FileTags.Tags(FoundedIndexes(i)).Type <> "image" Then
                                FoundedIndexes(i) = -1
                            End If
                        End If
                    Next i
                    SetOnlyImagesSettings()
                End If
                If FilesFilter.value = 2 Then
                    For i As Long = 0 To NFounded
                        If FoundedIndexes(i) <> -1 Then
                            If FileTags.Tags(FoundedIndexes(i)).Type <> "music" Then
                                FoundedIndexes(i) = -1
                            End If
                        End If
                    Next i
                    SetOnlyMusicSettings()
                End If


                Dim MaxI, MaxRelev As Long
                For j As Long = 0 To 200
                    MaxI = -1
                    MaxRelev = 0
                    For i As Long = 0 To NFounded
                        If FoundedIndexes(i) <> -1 And FoundedRelevance(i) > MaxRelev Then
                            MaxRelev = FoundedRelevance(i)
                            MaxI = i
                        End If
                    Next
                    If MaxI = -1 Then Exit For
                    Path = FileTags.Files(FoundedIndexes(MaxI))
                    FoundedIndexes(MaxI) = -1
                    NFoundedToShow += 1
                    FoundedList(NFoundedToShow) = Path
                Next
                ReDim Preserve FoundedList(NFoundedToShow)

                ImagesBox.Canvas.DestY = 0
                ImagesBox.Canvas.DestX = 0
                ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.Simple 'NotSimple
                ImagesBox.Refine(FoundedList)
                If ImagesBox.NImages > 0 Then ImagesBox.SelectedImageIndex = 1
                ImagesBox.SetCanvas()
                ImagesBox.MakeAllThumbnails()
            End If

        ElseIf SearchFailed Then
            picProgressSearch.BackColor = Color.Red
            picProgressSearch.Width = (pnlSearchText.Width - 2)
        End If
    End Sub
#End Region
#Region "INDEX"
    Structure IndexItem
        Dim Keyword As String
        Dim Count As Long
        Dim Files() As Long
    End Structure
    Dim IndexReady As Boolean = False
    Dim Index() As IndexItem
    Dim IndexCount As Long = 0


    Private Sub bgwMakeIndex_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwMakeIndex.ProgressChanged
        picBuildingIndexProgress.Width = (pnlSearchText.Width - 2) * e.ProgressPercentage / 10000
        Dim D As Double = e.ProgressPercentage / 2000
        UcWaitingIndicator1.Rings(0).Lenght = Math.Min(Math.Max(360 * D, 45), 360 - 60)
        UcWaitingIndicator1.Rings(1).Lenght = Math.Min(Math.Max(360 * (D - 1), 45), 360 - 45)
        UcWaitingIndicator1.Rings(2).Lenght = Math.Min(Math.Max(360 * (D - 2), 45), 360 - 45)
        UcWaitingIndicator1.Rings(3).Lenght = Math.Min(Math.Max(360 * (D - 3), 45), 360 - 45)
        UcWaitingIndicator1.Rings(4).Lenght = Math.Min(Math.Max(360 * (D - 4), 30), 360 - 30)
    End Sub
    Private Sub bgwMakeIndex_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwMakeIndex.DoWork
        For i As Long = 1 To FileTags.N
            Dim s As String = GetFileName(FileTags.Files(i)).ToLower
            If FileTags.Tags(i).Type = "music" Then
                s += " " + FileTags.Tags(i).Song.Name.ToLower + " " + FileTags.Tags(i).Song.Singer.ToLower
            End If
            Dim ss() As String = s.Split({CChar("."), CChar(","), CChar("-"), CChar("_"), CChar(" "), CChar("&")})
            For j As Long = 0 To ss.Length - 1
                Dim k As Long = 0 'FindInIndex(ss(j))
                For ii As Long = 1 To IndexCount
                    If Index(ii).Keyword = ss(j) Then k = ii : Exit For
                Next
                If k > 0 Then
                    Dim kk As Long = 0
                    For ii As Long = 1 To Index(k).Count
                        If Index(k).Files(ii) = i Then kk = ii : Exit For
                    Next
                    If kk = 0 Then
                        Index(k).Count += 1
                        ReDim Preserve Index(k).Files(Index(k).Count)
                        Index(k).Files(Index(k).Count) = i
                    End If
                Else
                    IndexCount += 1
                    ReDim Preserve Index(IndexCount)
                    Index(IndexCount).Keyword = ss(j)
                    Index(IndexCount).Count = 1
                    ReDim Preserve Index(IndexCount).Files(1)
                    Index(IndexCount).Files(1) = i
                End If
            Next
            If (i Mod 50) = 0 Then
                bgwMakeIndex.ReportProgress(10000 * i / FileTags.N)
            End If
            If bgwMakeIndex.CancellationPending Then Exit For
        Next
        IndexReady = True
    End Sub
    Private Sub bgwMakeIndex_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwMakeIndex.RunWorkerCompleted
        picBuildingIndexProgress.Visible = False
        UcWaitingIndicator1.Enabled = False
        txtSearchWhat_TextChanged(sender, e)
    End Sub

    Private Function FindInIndex(Keyword As String) As Long
        For i As Long = 1 To IndexCount
            If Index(i).Keyword = Keyword Then Return i : Exit Function
        Next
        Return 0
    End Function
#End Region

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        With FileTags
            Dim DeletedFromBase As Long = 0
            For i As Long = 1 To .N
                If Not IO.File.Exists(.Files(i)) Then .Files(i) = "" : DeletedFromBase += 1
                If i Mod 500 Then Button9.Text = Math.Round(100 * i / .N).ToString() : Button9.Refresh()
            Next

            Dim SimilarInBase As Long = 0
            Dim F As Boolean = False, S As String
            For i As Long = 1 To .N
                F = False
                S = LCase(.Files(i))
                For j As Long = 1 To i - 1
                    If S = LCase(.Files(j)) Then F = True
                Next
                If F Then .Files(i) = "" : SimilarInBase += 1
                If i Mod 500 Then Button9.Text = Math.Round(100 * i / .N).ToString() : Button9.Refresh()
            Next
            Button9.Text = "deleted:" + DeletedFromBase.ToString() + " similar:" + SimilarInBase.ToString() + " total:" + .N.ToString()
        End With
    End Sub

    'Private Sub ShowPhoto(i As Long)
    '    Dim a As New frmShowPhoto2
    '    Dim FN(NImages) As String
    '    For j As Long = 1 To NImages
    '        FN(j) = Image(j).FileName
    '    Next
    '    If Image(i).Loaded = True Then
    '        a.InitFullScreened(Thumbnail(i), i, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
    '    Else
    '        a.InitFullScreened(bmp_error, i, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
    '    End If
    '    a.Show()
    'End Sub


    Dim RecentFiles As New TestHelpers.clsRecentItems("RecentFiles.txt", 500)
    Private Async Sub ImagesBox_FileClick(Image As ucImagesBox.ImageStruct) Handles ImagesBox.FileClick
        Try
            Dim Path As String = Image.FileName
            If Image.Type = ucImagesBox.FileTypes.Command Then
                If (Mid(Path, 1, 5).ToLower = "music") Then
                    PathLine.NewMaxPath(Path.ToLower)
                    Await refresh_files()
                End If
            ElseIf Image.Type = ucImagesBox.FileTypes.Music Then
                Player_PlaySong(Path)
                FileTags.Tags(ImagesBox.Image(ImagesBox.SelectedImageIndex).InTagsIngex).LaunchingTimes -= 1
            Else
                If (Mid(Path, Path.Length - 3)) = ".exe" Then
                    Dim SP As New System.Threading.Thread(AddressOf SubShell)
                    SP.Start(Path)
                Else
                    Process.Start(Path, AppWinStyle.NormalFocus)
                End If
                RecentFiles.AddItem(Path)
                RecentFiles.Save()
            End If
            'MsgBox("file click")
        Catch
            'MsgBox("FileExecutingProblem")
        End Try
    End Sub
    Private Sub SubShell(Path As Object)
        Try
            Shell(Chr(34) + Path + Chr(34), AppWinStyle.NormalFocus)
        Catch ex As Exception
            Try
                System.Diagnostics.Process.Start(Path)
            Catch ex2 As Exception
                'MsgBox(ex.ToString() + vbNewLine + "_____________________" + vbNewLine + ex2.ToString())
            End Try
        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Vertical Then ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Нorizontal Else ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.Vertical

        ImagesBox.RecalculateCanvasParameters()
        ImagesBox.StopLoading = True
        ImagesBox.ReloadAllThumbs()
        ImagesBox.OrderImages()
        ImagesBox.FlyingAlgorithm = ucImagesBox.FlyingAlgorithms.Simple

        ImagesBox.Select()
    End Sub


    Private Sub cbSelectionMode_ValueChanged(Value As Boolean) Handles cbSelectionMode.ValueChanged
        ImagesBox.SelectionMode = Value
    End Sub

    Private Async Sub ImagesBox_NeedToRefine() Handles ImagesBox.NeedToRefine
        Await refine_files()
    End Sub

    Friend Declare Auto Function GetWindowText Lib "user32.dll" (ByVal hwnd As Int32, <Runtime.InteropServices.Out()> ByVal lpString As System.Text.StringBuilder, ByVal cch As Int32) As Int32
    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Long
    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal Hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Const WM_COMMAND = &H111
    Private Const WM_USER = &H400

    Public Class Win32
        Public Sub KillProcess(ByVal hwnd As Long)
            Dim pID As Long
            Dim hProc As Long
            'MsgBox(hwnd)
            GetWindowThreadProcessId(hwnd, pID)
            'MsgBox(pID)
            hProc = OpenProcess(PROCESS_TERMINATE, False, pID)
            SendMessage(hwnd, WM_QUERYENDSESSION, 0, 1)
            SendMessage(hwnd, WM_ENDSESSION, -1, 1)
            TerminateProcess(hProc, 0)
            CloseHandle(hProc)
        End Sub

        <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto, ExactSpelling:=True)> _
        Public Shared Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
        End Function

        <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto, ExactSpelling:=True)> _
        Public Shared Function BringWindowToTop(hWnd As IntPtr) As Boolean
        End Function

        <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto, ExactSpelling:=True)> _
        Public Shared Function GetWindowThreadProcessId(hWnd As IntPtr, ByRef ProcessId As Integer) As Boolean
        End Function

        Public Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)

        Public Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
        Public Declare Function TerminateProcess Lib "kernel32" (ByVal hProcess As Long, ByVal uExitCode As Long) As Long
        Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
        Const PROCESS_TERMINATE = &H1
        Const WM_QUERYENDSESSION = &H11
        Const WM_ENDSESSION = &H16

        <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto, ExactSpelling:=True)> _
        Public Shared Function MoveWindow(hWnd As IntPtr, X As Integer, Y As Integer, nWidth As Integer, nHeight As Integer, bRepaint As Boolean) As Boolean
        End Function

        Public Const EWX_HYBRID_SHUTDOWN = &H400000 'Beginning with Windows 8:  You can prepare the system for a faster startup by combining the EWX_HYBRID_SHUTDOWN flag with the EWX_SHUTDOWN flag.
        Public Const EWX_LOGOFF = 0 'Shuts down all processes running in the logon session of the process that called the ExitWindowsEx function. Then it logs the user off.
        'This flag can be used only by processes running in an interactive user's logon session.
        Public Const EWX_POWEROFF = &H8 'Shuts down the system and turns off the power. The system must support the power-off feature.
        'The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
        Public Const EWX_REBOOT = &H2 'Shuts down the system and then restarts the system.
        'The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
        Public Const EWX_RESTARTAPPS = &H40 'Shuts down the system and then restarts it, as well as any applications that have been registered for restart using the RegisterApplicationRestart function. These application receive the WM_QUERYENDSESSION message with lParam set to the ENDSESSION_CLOSEAPP value. For more information, see Guidelines for Applications.
        Public Const EWX_SHUTDOWN = &H1 'Shuts down the system to a point at which it is safe to turn off the power. All file buffers have been flushed to disk, and all running processes have stopped.
        'The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
        'Specifying this flag will not turn off the power even if the system supports the power-off feature. You must specify EWX_POWEROFF to do this.

        <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto, ExactSpelling:=True)> _
        Public Shared Function GetForegroundWindow() As IntPtr
        End Function

        Declare Function ExitWindowsEx Lib "user32.dll" (ByVal uFlags As Long, ByVal dwReserved As Long) As Long
    End Class

    Private Sub OnAirVoiceRecognizer_SizeChanged(sender As Object, e As EventArgs) Handles OnAirVoiceRecognizer.SizeChanged

    End Sub

    Private Function IntToMonth(M As Integer) As String
        Dim months(12) As String
        months(0) = "January"
        months(1) = "February"
        months(2) = "March"
        months(3) = "April"
        months(4) = "May"
        months(5) = "June"
        months(6) = "July"
        months(7) = "August"
        months(8) = "September"
        months(9) = "October"
        months(10) = "November"
        months(11) = "December"
        Return (months(M - 1))
    End Function

    Dim KateVoice As New WMPLib.WindowsMediaPlayer
    Private Sub KateSay(text As String)
        KateVoice.controls.stop()
        KateVoice.URL = ""
        Loadfile("http://translate.google.com/translate_tts?tl=en&q=" + text, "tts.mp3", 0)
        KateVoice.URL = "tts.mp3"
        KateVoice.controls.play()
    End Sub
    Private Enum SpeechThemes As Short
        Main = 0
        Music = 1
    End Enum
    Dim SpeechTheme As SpeechThemes = SpeechThemes.Main
    Private Async Sub ucVoiceRecognizer_SmthRecognized(Vars() As String) Handles VoiceRecognizer.SmthRecognized, OnAirVoiceRecognizer.SmthRecognized
        Dim Variants(vars.Length - 1)() As String
        For i As Long = 0 To vars.Length - 1
            Variants(i) = vars(i).Split(" ")
        Next

        Dim hwndWinamp As Long = FindWindow("Winamp v1.x", vbNullString)
        Dim hwndMPHC As Long = FindWindow("MediaPlayerClassicW", vbNullString)
        Dim FocusWasOnMPHC As Boolean = Win32.GetForegroundWindow() = hwndMPHC

        'Dim st As System.Text.StringBuilder = New Text.StringBuilder(256)
        'Dim a As Integer = 256
        'Dim h As IntPtr = Win32.GetForegroundWindow()
        'GetWindowText(h, st, 256)

        For Each VV As String() In Variants
            For Each V As String In VV
                If LCase(V) = "kate" Or LCase(V) = "cate" Or LCase(V) = "key" Or LCase(V) = "gate" Or LCase(V) = "катя" Or LCase(V) = "кать" Then
                    If FocusWasOnMPHC Then SendMessage(hwndMPHC, WM_COMMAND, 888, 0) 'Pause

                    If Me.Visible = False Then Me.Visible = True

                    Me.Activate()
                    AppActivate(Text)
                    Win32.ShowWindow(Me.Handle, 1)
                    Win32.BringWindowToTop(Me.Handle)
                    AppActivate(Text)
                    Me.Activate()
                    System.Threading.Thread.Sleep(10)
                    ImagesBox.Focus()

                    If FocusWasOnMPHC Then System.Threading.Thread.Sleep(100) : Win32.BringWindowToTop(Me.Handle)
                    GoTo metka
                End If
            Next
        Next
metka:
        If Win32.GetForegroundWindow() = Me.Handle Then
            If txtSearchWhat.Focused = True Then
                If Variants(0)(0) = "results" Or Variants(0)(0) = "result" Then
                    ImagesBox.Focus()
                ElseIf (vars(0)(0) = "запустить" Or Variants(0)(0) = "run" Or _
                       Variants(0)(0) = "start" Or Variants(0)(0) = "open" Or _
                       Variants(0)(0) = "play" Or Variants(0)(0) = "launch") And _
                       Variants(0).Length = 1 Then
                    ImagesBox.Focus()
                    ImagesBox_FileClick(ImagesBox.Image(1))
                ElseIf Variants(0)(0) = "clear" Or Variants(0)(0) = "delete" Or vars(0) = "clear request" Or vars(0) = "delete request" Then
                    txtSearchWhat.Text = ""
                Else
                    If (txtSearchWhat.Text <> "") Then txtSearchWhat.Text += " "
                    txtSearchWhat.SelectionStart = txtSearchWhat.Text.Length
                    txtSearchWhat.SelectedText = vars(0)

                    ImagesBox.Focus()
                End If
                'ElseIf Variants(0)(0) = "find" Then
                '    txtSearchWhat.Focus()
                '    txtSearchWhat.Text = Mid(vars(0), 6)
                '    If txtSearchWhat.Text <> "" Then ImagesBox.Focus()
            Else
                ImagesBox.Focus()
                For Each s As String In vars
                    Dim Text As String = LCase(s)
                    If Text.IndexOf("kate ") = 0 Then Text = Text.Substring(5)

                    If Text = "домой" Or Text = "home" Or Text = "go home" Then
                        PathLine.NewMaxPath("home")
                        PathLine_PathChaged(False) : Exit Sub
                    ElseIf Text = "рабочий стол" Or Text = "desktop" Then
                        PathLine.NewMaxPath("ws")
                        PathLine_PathChaged(False) : Exit Sub
                    ElseIf Text = "downloads" Or Text = "загрузки" Then
                        PathLine.NewMaxPath("C:\Users\" + System.Environment.UserName + "\Downloads")
                        PathLine_PathChaged(False) : Exit Sub
                    ElseIf Text = "refresh" Or Text = "обнови" Then
                        Await refresh_files()
                        Exit Sub
                    ElseIf Text = "exit" Or Text = "close" Or Text = "die" Or Text = "закройся" Or Text = "вон" Or Text = "умри" Or _
                        Text = "сгинь" Or Text = "иди к чёрту" Or Text = "свали" Or Text = "закрыть" Or Text = "пошла вон" Then
                        Application.Exit()

                    ElseIf Text.IndexOf("shut down") = 0 Or Text.IndexOf("shutdown") = 0 Or Text.IndexOf("выключить компьютер") = 0 Then
                        'System.Diagnostics.Process.Start("ShutDown", "-s -f -t 00") 'MsgBox(Win32.ExitWindowsEx(Win32.EWX_SHUTDOWN, 0)) '0-sign out; 4-force sign out; 1-0;
                        Dim Troll As New frmShutDown()
                        Troll.ShowDialog()
                        Exit Sub

                    ElseIf Text.IndexOf("reboot") = 0 Or Text.IndexOf("re boot") = 0 Or Text.IndexOf("re start") = 0 Or Text.IndexOf("restart") = 0 Or Text.IndexOf("перезагрузи") = 0 Then
                        'System.Diagnostics.Process.Start("ShutDown", "-r -f -t 00") 'MsgBox(Win32.ExitWindowsEx(Win32.EWX_SHUTDOWN, 0)) '0-sign out; 4-force sign out; 1-0;
                        'Exit Sub

                    ElseIf (Text = "музыка" Or Text = "music" Or Text = "musica" Or Text = "songs") And DirsBox.Path = "home" Then
                        PathLine.NewMaxPath("music")
                        SpeechTheme = SpeechThemes.Music
                        PathLine_PathChaged(False) : Exit Sub
                    ElseIf Text = "документы" Or Text = "docs" Or Text = "documents" Or Text = "document" Then
                        PathLine.NewMaxPath("docs")
                        PathLine_PathChaged(False) : Exit Sub
                    ElseIf Text = "35 photo" Or Text = "new photo" Or Text = "new photoes" Then
                        btnLoad35photo_Click() : Exit Sub

                        'ElseIf Text = "поиск" Or Text = "найти" Or Text = "найди" Or Text = "find" Or Text = "search" Then
                        '    txtSearchWhat.Focus()
                        '    txtSearchWhat.Text = "" : Exit Sub
                        'ElseIf Text = "boobs" Or Text = "сиськи" Or Text = "сосок" Or Text = "титька" Or Text = "код красный" Then
                        '    Process.Start("http://www.google.ru/search?q=" + "картинки сиськи" + "&ie=UTF-8&oe=UTF-8")
                        '    Exit Sub

                    ElseIf Text.IndexOf("дура") >= 0 Or Text.IndexOf("тупая") >= 0 Or _
                        Text.IndexOf("лупень") >= 0 Or Text.IndexOf("срань") >= 0 Or _
                        Text.IndexOf("коза") >= 0 Or Text.IndexOf("козел") >= 0 Or _
                        Text.IndexOf("лох") >= 0 Or Text.IndexOf("олень") >= 0 Or _
                        Text.IndexOf("осёл") >= 0 Or Text.IndexOf("сволочь") >= 0 Or _
                        Text.IndexOf("скотина") >= 0 Or Text.IndexOf("жопа") >= 0 Or _
                        Text.IndexOf("дерьмо") >= 0 Or Text.IndexOf("сука") >= 0 Or _
                        Text.IndexOf("пидор") >= 0 Or Text.IndexOf("хуй") >= 0 Or _
                        Text.IndexOf("глупая") >= 0 Or Text.IndexOf("говно") >= 0 Then
                        ImagesBox.ShowPhoto("C:\Users\Denis\Downloads\fuck-off.jpg") ' Text.IndexOf("#") >= 0 Or
                        Exit Sub


                    ElseIf Text = "find" Or Text = "search" Or Text = "найти" Or Text = "поиск" Then
                        txtSearchWhat.Focus()
                        txtSearchWhat.Text = Mid(Text, 6)
                        Exit Sub
                    ElseIf Text.IndexOf("find ") = 0 And Text.IndexOf("in web") < 0 And Text.IndexOf("in the web") < 0 Then
                        txtSearchWhat.Focus()
                        txtSearchWhat.Text = Mid(Text, 6)
                        If txtSearchWhat.Text <> "" Then ImagesBox.Focus()
                        Exit Sub
                    ElseIf Text.IndexOf("найди ") = 0 Or Text.IndexOf("найти ") = 0 And Text.IndexOf("в интернете") < 0 Then
                        txtSearchWhat.Focus()
                        txtSearchWhat.Text = Mid(Text, 7)
                        If txtSearchWhat.Text <> "" Then ImagesBox.Focus()
                        Exit Sub

                    ElseIf Text.IndexOf("hello") >= 0 Or Text.IndexOf("привет") >= 0 Then
                        KateSay("Hi!") : Exit Sub
                    ElseIf Text.IndexOf("date") >= 0 Or Text.IndexOf("день") >= 0 Then
                        KateSay("Today is " + DateAndTime.Now.Day.ToString() + " of " + IntToMonth(DateAndTime.Now.Month) + ". It's" + DateAndTime.Now.DayOfWeek.ToString())
                        Exit Sub
                    ElseIf Text.IndexOf("time") >= 0 Or Text.IndexOf("врем") >= 0 Then
                        KateSay(DateAndTime.Now.Hour.ToString() + "." + DateAndTime.Now.Minute.ToString() + " a.m.")
                        Exit Sub

                    ElseIf Text = "открыть" Or Text = "открой" Or Text = "uh huh" Or Text = "включай" Or Text = "включить" Or Text = "запуск" Or Text = "запустить" Or Text = "run" Or Text = "start" Or Text = "open" Or Text = "play" Or Text = "launch" Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Enter)
                        ImagesBox.ucImagesBox_KeyDown(Me, args)
                        If ImagesBox.Image(ImagesBox.SelectedImageIndex).Type = ucImagesBox.FileTypes.Music Then SpeechTheme = SpeechThemes.Music
                        Exit Sub

                    ElseIf Text = "to playlist" Or Text = "add to play list" Or Text = "add to playlist" Or Text = "play list" Or Text = "playlist" Or Text = "to list" Or
                        Text = "в плейлист" Or Text = "добавить в плейлист" Or Text = "список воспроизведения" Then
                        SpeechTheme = SpeechThemes.Music
                        ImagesBox.AddToPlaylist() : Exit Sub

                    ElseIf Text = "назад" Or Text = "back" Then
                        btnUp.btnDown()
                        btnUp.Refresh()
                        PathLine.GoUp()
                        Await refresh_files()
                        btnUp.btnNormal()
                        btnUp.Refresh() : Exit Sub

                    ElseIf Text = "page down" Or Text = "next page" Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.PageDown)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text = "page up" Or Text = "prev page" Or Text = "previous page" Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.PageUp)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text = "page home" Or Text.IndexOf("начало") >= 0 Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Home)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text = "page end" Or Text.IndexOf("конец") >= 0 Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.End)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub

                    ElseIf Text = "next file" Then
                        ImagesBox.SelectedImageIndex += 1 : ImagesBox.SetCanvas() : ImagesBox.RedrawOnce = True : SpeechTheme = SpeechThemes.Main : Exit Sub
                    ElseIf Text = "previous file" Then
                        ImagesBox.SelectedImageIndex -= 1 : ImagesBox.SetCanvas() : ImagesBox.RedrawOnce = True : SpeechTheme = SpeechThemes.Main : Exit Sub

                    ElseIf Text = "next" Then
                        If SpeechTheme = SpeechThemes.Music Then
                            Player_NextSong() : Exit Sub
                        Else
                            ImagesBox.SelectedImageIndex += 1 : ImagesBox.SetCanvas() : ImagesBox.RedrawOnce = True : Exit Sub
                        End If
                    ElseIf Text = "previous" Then
                        ImagesBox.SelectedImageIndex -= 1 : ImagesBox.SetCanvas() : ImagesBox.RedrawOnce = True : Exit Sub


                    ElseIf Text = "down" Or Text = "вниз" Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Down)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text.IndexOf("up") >= 0 Or Text.IndexOf("верх") >= 0 Or Text.IndexOf("выше") >= 0 Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Up)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text.IndexOf("left") >= 0 Or Text.IndexOf("лево") >= 0 Or Text.IndexOf("левее") >= 0 Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Left)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text.IndexOf("right") >= 0 Or Text.IndexOf("право") >= 0 Or Text.IndexOf("правее") >= 0 Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Right)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub

                    ElseIf Text = "delete" Or Text = "удалить" Or Text = "удали" Then
                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Delete)
                        ImagesBox.ucImagesBox_KeyDown(Me, args) : Exit Sub
                    ElseIf Text = "cut" Or Text = "вырезать" Or Text = "вырежь" Then
                        ImagesBox.CutToClipboard() : Exit Sub
                    ElseIf Text = "copy" Or Text = "копировать" Or Text = "скопируй" Then
                        ImagesBox.CopyToClipboard() : Exit Sub
                    ElseIf Text = "paste" Or Text = "вставить" Or Text = "вставь" Then
                        ImagesBox.PasteFromClipboard() : Exit Sub


                    ElseIf Text = "select all" Or Text = "выдели всё" Then
                        ImagesBox.SelectAll() : ImagesBox.RedrawOnce = True : Exit Sub
                    ElseIf Text = "deselect all" Or Text = "clear selection" Or Text = "unselect all" Or Text = "снять всё выделение" Then
                        ImagesBox.DeselectAll() : ImagesBox.RedrawOnce = True : Exit Sub
                    ElseIf Text = "select" Or Text = "select this" Or Text = "mark" Or Text = "select this" Or Text = "выдели" Then
                        ImagesBox.SelectCurrentItem() : ImagesBox.RedrawOnce = True : Exit Sub
                    ElseIf Text = "deselect" Or Text = "des elect" Or Text = "dis elect" Or Text = "dissect" Or Text = "d select" Or Text = "unselect" Or Text = "unselect this" Or Text = "deselect this" Or Text = "снять выделение" Then
                        ImagesBox.DeselectCurrentItem() : ImagesBox.RedrawOnce = True : Exit Sub
                    ElseIf Text = "invert selection" Or Text = "инвертировать выделение" Then
                        ImagesBox.InvertSelection() : ImagesBox.RedrawOnce = True : Exit Sub

                    ElseIf Text = "hide folders" Or Text = "hide dirs" Or Text = "hide directories" Then
                        HideDirsBox() : Exit Sub
                    ElseIf Text = "show folders" Or Text = "show dirs" Or Text = "show directories" Then
                        ShowDirsBox() : Exit Sub
                    ElseIf Text = "add folders" Or Text = "add dirs" Or Text = "add directories" Then
                        cbDirs.value = True : Exit Sub

                    ElseIf Text = "only images" Or Text = "only pictures" Or Text = "только картинки" Then
                        FilesFilter.value = 1 : FilesFilter_ValueChanged() : Exit Sub
                    ElseIf Text = "only music" Or Text = "only songs" Or Text = "только песни" Then
                        FilesFilter.value = 2 : FilesFilter_ValueChanged() : Exit Sub
                    ElseIf Text = "all" Or Text = "all files" Or Text = "все файлы" Then
                        FilesFilter.value = 0 : FilesFilter_ValueChanged() : Exit Sub
                    ElseIf Text = "sid" Or Text = "depth" Or Text = "deep" Or Text = "deep search" Or Text = "d search" Then
                        Cursor = Cursors.WaitCursor
                        cbSID.value = True
                        make_sid()
                        cbSID.value = False
                        Cursor = Cursors.Arrow : Exit Sub
                    Else
                        Dim name As String

                        'If DirsBox.Path <> "home" And DirsBox.Height <> 0 Then
                        '    Dim dirs As Collections.ObjectModel.ReadOnlyCollection(Of String)
                        '    dirs = FileIO.FileSystem.GetDirectories(DirsBox.Path)
                        '    For Each name In dirs
                        '        Dim Compare As Boolean = True
                        '        For Each word As String In Text.Split(" ")
                        '            If InStr(LCase(Mid(name, InStrRev(name, "\") + 1)), word) = 0 Then Compare = False
                        '        Next
                        '        If Compare = True Then
                        '            PathLine.NewMaxPath(name)
                        '            PathLine_PathChaged(False)
                        '            Exit Sub
                        '        End If
                        '    Next
                        'End If

                        Dim OpenFlag As Boolean = False
                        Dim RunFlag As Boolean = False
                        Dim PlayFlag As Boolean = False

                        If (Text.IndexOf("play ") = 0) Then PlayFlag = True : Text = Text.Substring(5)
                        If (Text.IndexOf("open ") = 0) Then OpenFlag = True : Text = Text.Substring(5)
                        If (Text.IndexOf("run ") = 0) Then RunFlag = True : Text = Text.Substring(4)
                        If (Text.IndexOf("открой ") = 0) Then OpenFlag = True : Text = Text.Substring(6)
                        If (Text.IndexOf("открыть ") = 0) Then OpenFlag = True : Text = Text.Substring(7)
                        If (Text.IndexOf("включить ") = 0) Then RunFlag = True : Text = Text.Substring(8)
                        If (Text.IndexOf("включи ") = 0) Then RunFlag = True : Text = Text.Substring(6)

                        With ImagesBox
                            For i As Long = 1 To .NImages
                                Dim Compare As Boolean = True
                                For Each word As String In Text.Split(" ")
                                    If InStr(LCase(.Image(i).OriginalName), word) = 0 And InStr(LCase(.Image(i).Singer), word) = 0 Then Compare = False
                                Next
                                If Compare = True Then
                                    .SelectedImageIndex = i
                                    .SetCanvas()
                                    .RedrawOnce = True
                                    If (.Image(i).Type = ucImagesBox.FileTypes.Music And PlayFlag) Or RunFlag Or OpenFlag Then
                                        Dim args As New Windows.Forms.KeyEventArgs(Keys.Enter)
                                        ImagesBox.ucImagesBox_KeyDown(Me, args)
                                        If ImagesBox.Image(ImagesBox.SelectedImageIndex).Type = ucImagesBox.FileTypes.Music Then SpeechTheme = SpeechThemes.Music
                                    End If
                                    Exit Sub
                                End If
                            Next
                        End With
                    End If
                Next
            End If
        End If

        For Each s As String In vars
            Dim Text As String = " " + LCase(s) + " "

            If Text.IndexOf(" mail ") >= 0 Or Text.IndexOf(" почта ") >= 0 Or Text.IndexOf(" мыло ") >= 0 Then
                Process.Start("http://mail.google.com") : Exit Sub
            ElseIf Text.IndexOf(" contact ") >= 0 Or Text.IndexOf(" контакт ") >= 0 Or Text.IndexOf(" новости ") >= 0 Then
                Process.Start("http://vk.com") : Exit Sub
            ElseIf Text.IndexOf(" habr ") >= 0 Or Text.IndexOf(" habrahabr ") >= 0 Or Text.IndexOf(" хабр ") >= 0 Or Text.IndexOf(" хабрахабр ") >= 0 Then
                Process.Start("http://habrahabr.ru") : Exit Sub
            ElseIf Text.IndexOf(" translate ") >= 0 Or Text.IndexOf(" перевод ") >= 0 Or Text.IndexOf(" переводчик ") >= 0 Then
                Process.Start("http://translate.google.com") : Exit Sub
            ElseIf Text.IndexOf(" metro ") >= 0 Or Text.IndexOf(" метро ") >= 0 Or Text.IndexOf(" underground ") >= 0 Then
                Process.Start("http://www.metromap.ru/") : Exit Sub
            ElseIf Text.IndexOf(" maps ") >= 0 Or Text.IndexOf(" пробки ") >= 0 Or Text.IndexOf(" карта ") >= 0 Or Text.IndexOf(" карты ") >= 0 Or Text.IndexOf(" карту ") >= 0 Then
                Process.Start("http://maps.yandex.ru/?ll=38.054923%2C55.793221&spn=0.988770%2C0.256863&z=11&l=map%2Ctrf%2Ctrfe&trfm=cur") : Exit Sub
            End If

            If Text.IndexOf("song name") >= 0 Or Text.IndexOf("что игра") >= 0 Or Text.IndexOf("кто игра") >= 0 Or ((Text.IndexOf("песн") >= 0 Or SpeechTheme = SpeechThemes.Music) And (Text.IndexOf("название") >= 0 Or Text.IndexOf("называется") >= 0)) Then
                If Player.URL <> "" Then
                    If Player.currentMedia.duration > 0 Then
                        Dim SongName As String = FileTags.Tags(FileTags.FindByFileName(CurrentSongFileName)).Song.Name
                        If (SongName.LastIndexOf("(") > 1) Then SongName = SongName.Substring(1, SongName.LastIndexOf("(") - 1)
                        KateSay(FileTags.Tags(FileTags.FindByFileName(CurrentSongFileName)).Song.Singer + ". " + SongName)
                        Exit Sub
                    End If
                End If
                KateSay("Nothing is playing")
                Exit Sub
            End If

            Text = LCase(s)
            If hwndWinamp <> 0 And (Player.playState <> WMPLib.WMPPlayState.wmppsPlaying And Player.playState <> WMPLib.WMPPlayState.wmppsPaused) Then
                If Text = "pause" Or Text = "stop" Or Text = "stop music" Or Text = "music stop" Or Text = "пауза" Or Text = "стоп" Or Text = "выключи музыку" Then
                    SendMessage(hwndWinamp, WM_COMMAND, 40046, 0) : Exit Sub 'Пауза 40046
                ElseIf Text = "play" Or Text = "start playing" Or Text = "music play" Or Text = "play music" Or Text = "включи музыку" Or Text = "включить музыку" Or Text = "музыку" Then
                    SendMessage(hwndWinamp, WM_COMMAND, 40045, 0) : Exit Sub 'Играть 40045
                ElseIf Text = "next song" Or Text = "следующая песня" Then
                    SendMessage(hwndWinamp, WM_COMMAND, 40048, 0) : Exit Sub 'Следующая кнопка трека 40048
                ElseIf Text = "prev song" Or Text = "previous song" Or Text = "предидущая песня" Then
                    SendMessage(hwndWinamp, WM_COMMAND, 40048, 0) : Exit Sub 'Предыдущая кнопка трека 40044 
                ElseIf Text = "loud" Or Text = "louder" Or Text = "turn up" Or Text = "громче" Or Text = "погромче" Then
                    For m As Long = 1 To 7
                        SendMessage(hwndWinamp, WM_COMMAND, 40058, 0) 'Увеличить звук на 1% 40058 
                    Next : Exit Sub
                ElseIf Text = "quiet" Or Text = "quieter" Or Text = "turn down" Or Text = "тише" Or Text = "потише" Then
                    For m As Long = 1 To 7
                        SendMessage(hwndWinamp, WM_COMMAND, 40059, 0) 'Уменьшить звук на 1% 40059
                    Next : Exit Sub
                ElseIf Text = "more loud" Or Text = "ещё громче" Or Text = "еще громче" Or Text = "хочу громко" Then
                    For m As Long = 1 To 15
                        SendMessage(hwndWinamp, WM_COMMAND, 40058, 0) 'Увеличить звук на 1% 40058 
                    Next : Exit Sub
                ElseIf Text = "more quiet" Or Text = "ещё тише" Or Text = "еще тише" Or Text = "хочу тихо" Then
                    For m As Long = 1 To 15
                        SendMessage(hwndWinamp, WM_COMMAND, 40059, 0) 'Уменьшить звук на 1% 40059
                    Next : Exit Sub
                End If '122 Настройка звука в соответствии с 'данными', которые могут быть между 0 (тишина) и 255 (максимум). (data=0-255) Закрыть Winamp 40001 5
            Else
                If Text = "stop music" Or Text = "music stop" Or Text = "выключи музыку" Or Text = "тихо" Or Text = "silence" Or Text = "пауза" Or Text = "выключи" Then
                    Player_Pause() : SpeechTheme = SpeechThemes.Music : Exit Sub
                ElseIf Text = "start playing" Or Text = "music play" Or Text = "play music" Or Text = "включи музыку" Or Text = "включить музыку" Or Text = "музыку" Then
                    Player_Play() : SpeechTheme = SpeechThemes.Music : Exit Sub
                ElseIf Text = "next song" Or Text = "следующая песня" Then
                    Player_NextSong() : SpeechTheme = SpeechThemes.Music : Exit Sub

                    'ElseIf Text = "prev song" Or Text = "prev" Or Text = "previous" Or Text = "previous song" Or Text = "предидущая" Or Text = "предидущая песня" Then
                    '    'SendMessage(hwndWinamp, WM_COMMAND, 40048, 0) : Exit Sub 'Предыдущая кнопка трека 40044 

                ElseIf Text = "more loud" Or Text = "ещё громче" Or Text.IndexOf("еще громче") >= 0 Or Text = "хочу громко" Then
                    SetNewVolume(GetVolume() + 20) : SpeechTheme = SpeechThemes.Music : Exit Sub
                ElseIf Text = "more quiet" Or Text = "ещё тише" Or Text = "еще тихо" Or Text.IndexOf("еще тише") >= 0 Or Text = "хочу тихо" Then
                    SetNewVolume(GetVolume() - 20) : SpeechTheme = SpeechThemes.Music : Exit Sub
                ElseIf Text = "loud" Or Text = "louder" Or Text = "turn up" Or Text = "громче" Or Text = "погромче" Then
                    SetNewVolume(GetVolume() + 10) : SpeechTheme = SpeechThemes.Music : Exit Sub
                    'VolumeUp()
                ElseIf Text = "quiet" Or Text = "quieter" Or Text = "turn down" Or Text = "тише" Or Text = "потише" Then
                    SetNewVolume(GetVolume() - 10) : SpeechTheme = SpeechThemes.Music : Exit Sub
                    'VolumeDown() ' : VolumeDown() : VolumeDown()

                ElseIf SpeechTheme = SpeechThemes.Music Then
                    If Text = "pause" Or Text = "stop" Or Text = "stop" Or Text = "stop" Or Text = "пауза" Or Text = "стоп" Or Text = "выключи" Or Text = "отключи" Then
                        Player_Pause() : Exit Sub
                    ElseIf Text = "play" Or Text = "start" Or Text = "turn on" Or Text = "включи" Or Text = "включай" Or Text = "включить" Then
                        Player_Play() : Exit Sub
                    ElseIf Text = "next" Or Text = "следующая" Or Text = "следующую" Or Text = "другую" Or Text = "дальше" Then
                        Player_NextSong() : Exit Sub
                    ElseIf Text = "сначала" Or Text = "включи сначала" Or Text = "в начало" Or Text = "на начало" Or Text = "дальше" Then
                        Player.controls.currentPosition = 0 : Exit Sub
                    End If
                End If
            End If

            If hwndMPHC <> 0 Then
                If Win32.GetForegroundWindow() <> hwndMPHC Then
                    If Text = "movie" Or Text = "film" Or Text = "фильм" Or Text = "кино" Or Text = "окно фильм" Or Text = "окно кино" Then
                        Win32.ShowWindow(hwndMPHC, 1)
                        Win32.BringWindowToTop(hwndMPHC)
                    End If
                End If
                If Win32.GetForegroundWindow() = hwndMPHC Then
                    If Text = "pause" Or Text = "pause movie" Or Text = "stop" Or Text = "stop movie" Or _
                    Text = "movie stop" Or Text = "пауза" Or Text = "стоп" Or Text = "останови фильм" Or Text = "остановить фильм" Or _
                    Text = "остановить" Or Text = "останови" Then
                        SendMessage(hwndMPHC, WM_COMMAND, 888, 0) : Exit Sub 'Pause
                    ElseIf Text = "play" Or Text = "start movie" Or Text = "movie play" Or Text = "play movie" Or _
                    Text = "включи фильм" Or Text = "включить фильм" Or Text = "продолжить фильм" Or Text = "продолжить" Or _
                    Text = "включить" Or Text = "включи" Or Text = "включай" Then
                        SendMessage(hwndMPHC, WM_COMMAND, 887, 0) : Exit Sub 'Play
                    ElseIf Text = "movie" Or Text = "film" Or Text = "фильм" Or Text = "кино" Then
                        SendMessage(hwndMPHC, WM_COMMAND, 889, 0) : Exit Sub 'Play/Pause
                    ElseIf Text = "full screen" Or Text = "expand" Or Text = "full screen movie" Or Text = "на весь экран" Or _
                    Text = "весь экран" Or Text = "развернуть" Or Text = "развернуть на весь экран" Then
                        SendMessage(hwndMPHC, WM_COMMAND, 830, 0) : Exit Sub 'FullScreen
                    ElseIf Text = "close movie" Or Text = "exit movie" Or Text = "закрыть фильм" Or Text = "выключить фильм" Or _
                    Text = "close" Or Text = "exit" Or Text = "закрыть" Or Text = "выключить" Then
                        SendMessage(hwndMPHC, WM_COMMAND, 816, 0) : Exit Sub 'FullScreen
                    End If
                End If
            End If


            If Text.IndexOf("найди в интернете ") = 0 Or Text.IndexOf("найти в интернете ") = 0 Then
                Process.Start("http://www.google.ru/search?q=" + Mid(Text, "найди в интернете ".Length) + "&ie=UTF-8&oe=UTF-8")
                Exit Sub
            ElseIf Text.IndexOf("найди интернете ") = 0 Then
                Process.Start("http://www.google.ru/search?q=" + Mid(Text, "найди интернете ".Length) + "&ie=UTF-8&oe=UTF-8")
                Exit Sub
            ElseIf Text.IndexOf("find in web ") = 0 Then
                Process.Start("http://www.google.ru/search?q=" + Mid(Text, "find in web ".Length) + "&ie=UTF-8&oe=UTF-8")
                Exit Sub
            ElseIf Text.IndexOf("google ") = 0 Then
                Process.Start("http://www.google.ru/search?q=" + Mid(Text, "google ".Length) + "&ie=UTF-8&oe=UTF-8")
                Exit Sub
            ElseIf Text.IndexOf("гугл ") = 0 Then
                Process.Start("http://www.google.ru/search?q=" + Mid(Text, "гул ".Length) + "&ie=UTF-8&oe=UTF-8")
                Exit Sub
            ElseIf Text.IndexOf("yandex ") = 0 Then
                Process.Start("http://www.google.ru/search?q=" + Mid(Text, "yandex ".Length) + "&ie=UTF-8&oe=UTF-8")
                Exit Sub
            End If

            If Text = "close window" Then
                If Win32.GetForegroundWindow() <> 0 Then
                    Try
                        Dim a As New Win32()
                        a.KillProcess(Win32.GetForegroundWindow())
                    Catch ex As Exception
                        MsgBox(ex.ToString())
                    End Try
                    Exit Sub
                End If
            End If
            If Text = "close tab" Or Text = "закрыть вкладку" Then
                SendHotKey(Keys.W, True) : Exit Sub
            End If
            If Text = "new tab" Or Text = "новая вкладка" Then
                SendHotKey(Keys.T, True) : Exit Sub
            End If
            If Text = "refresh" Or Text = "обновить" Then
                SendHotKey(Keys.F5) : Exit Sub
            End If
            If Text = "next page" Or Text = "page down" Or Text = "вниз" Or Text = "дальше" Or Text = "next" Or Text = "вперед" Or Text = "следующая" Then
                SendHotKey(Keys.PageDown) : Exit Sub
            End If
            If Text = "previous page" Or Text = "previous" Or Text = "page up" Or Text = "вверх" Or Text = "назад" Or Text = "предыдущая" Then
                SendHotKey(Keys.PageUp) : Exit Sub
            End If
            If Text = "tab" Or Text = "таб" Then
                SendHotKey(Keys.Tab) : Exit Sub
            End If
            If Text = "enter" Or Text = "перейти" Then
                SendHotKey(Keys.Enter) : Exit Sub
            End If
            If Text = "change tab" Then
                SendHotKey(Keys.Tab, True) : Exit Sub
            End If
            If Text = "change window" Then
                SendHotKey(Keys.Tab, False, True) : Exit Sub
            End If
            If Text.IndexOf("напечатай ") = 0 Or Text.IndexOf("напиши ") = 0 Or Text.IndexOf("ввести ") = 0 Then
                Try
                    SendStringKeys(Text.Substring(Text.IndexOf(" ") + 1)) : Exit Sub
                Catch ex As Exception
                    MsgBox(ex.ToString())
                End Try
            End If
        Next
        'If vars.Length > 3 Then
        '    Dim a As New Form

        '    a.Text = vars(0) + " " + vars(1) + " " + vars(2)
        '    a.Show()
        'End If
        'If Win32.GetForegroundWindow() <> Me.Handle Then SendStringKeys(Vars(0))
    End Sub
    Private Sub FindIn(ByVal Variants As String, ByVal Synonyms As String())

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ImagesBox.FlyMode = Not ImagesBox.FlyMode
    End Sub

    Private Sub OnAirVoiceRecognizer_Load(sender As Object, e As EventArgs) Handles OnAirVoiceRecognizer.Load

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ImagesBox.Arrangement = ucImagesBox.ArrangmentTypes.No
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim hwndMPHC As Long = FindWindow(TextBox2.Text, vbNullString)
        If hwndMPHC <> 0 Then MsgBox(hwndMPHC)
    End Sub





    Private contextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents menuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents notifyIcon1 As System.Windows.Forms.NotifyIcon

    Private Sub HideInTray()
        Me.Visible = False

        If (notifyIcon1 Is Nothing) Then
            Me.components = New System.ComponentModel.Container
            Me.contextMenu1 = New System.Windows.Forms.ContextMenu
            Me.menuItem1 = New System.Windows.Forms.MenuItem

            ' Initialize contextMenu1
            Me.contextMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem1})

            ' Initialize menuItem1
            Me.menuItem1.Index = 0
            Me.menuItem1.Text = "E&xit"

            ' Set up how the form should be displayed.
            'Me.ClientSize = New System.Drawing.Size(292, 266)

            ' Create the NotifyIcon.
            Me.notifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)

            notifyIcon1.Icon = Me.Icon ' New Icon("appicon.ico")

            ' The ContextMenu property sets the menu that will
            ' appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = Me.contextMenu1

            ' The Text property sets the text that will be displayed,
            ' in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Kate"
            notifyIcon1.Visible = True
        End If
    End Sub

    Private Sub notifyIcon1_DoubleClick(Sender As Object, e As EventArgs) Handles notifyIcon1.Click
        If Me.Visible = False Then
            Me.Visible = True
            If (Me.WindowState = FormWindowState.Minimized) Then Me.WindowState = FormWindowState.Normal
            Me.Activate()
        Else
            Me.Visible = False
        End If
    End Sub
    Private Sub menuItem1_Click(Sender As Object, e As EventArgs) Handles menuItem1.Click
        Me.Close()
    End Sub

    Dim CurrentSongFileName As String = ""
    Dim WithEvents frmMusicPlayer As New TestHelpers.frmMusicPlayer
    Function GetVolume() As Long
        Return Player.settings.volume ' Adjust_System_Volume.AudioMixerHelper.GetVolume() 'Int(Val(myMixer(1).Devices.VolumeControl.Controls(1).Value / 65535 * 100))
    End Function
    Sub SetNewVolume(ByVal NewVal As Long) Handles frmMusicPlayer.VolumeChanged
        Player.settings.volume = NewVal
        'Adjust_System_Volume.AudioMixerHelper.SetVolume(NewVal)
        'myMixer(1).Devices.VolumeControl.Controls(1).Value = NewVal / 100 * 65536
    End Sub

    'Sub VolumeUp()
    '    keybd_event(System.Windows.Forms.Keys.VolumeUp, 0, 0, 0)
    'End Sub
    'Sub VolumeDown()
    '    keybd_event(System.Windows.Forms.Keys.VolumeDown, 0, 0, 0)
    'End Sub

    Private Sub UcSpotButton1_Click(sender As Object, e As EventArgs) Handles UcSpotButton1.Click
        Player_PlayPause()
    End Sub

    Public Sub Player_PlayPause() Handles frmMusicPlayer.PlayPause
        If Player.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            Player_Pause()
        ElseIf Player.playState = WMPLib.WMPPlayState.wmppsPaused Then
            Player_Play()
        Else
            Player_Play()
        End If
    End Sub
    Private Sub tmrPlayer_Tick(sender As Object, e As EventArgs) Handles tmrPlayer.Tick
        If Player.playState = WMPLib.WMPPlayState.wmppsStopped Then
            FileTags.Tags(FileTags.FindByFileName(CurrentSongFileName)).LaunchingTimes += 1

            Player_NextSong()
        Else
            If Player.URL <> "" Then
                If Player.currentMedia.duration > 0 Then
                    UpdatePlayerInfo()
                    'With sbPos
                    '    .max = CInt(Player.currentMedia.duration)
                    '    .value = CInt(Player.controls.currentPosition)
                    'End With
                End If
            End If
        End If
        ImagesBox.RedrawOnce = True
    End Sub
    Private Sub Player_Pause()
        Player.controls.pause()
        tmrPlayer.Stop()
        ImagesBox.PlayerState = ucImagesBox.PlayerStates.Pause
        ImagesBox.RedrawOnce = True
    End Sub
    Private Sub Player_Play()
        If Player.URL = "" Then
            Player_NextSong()
        Else
            Player.controls.play()
            ImagesBox.PlayerState = ucImagesBox.PlayerStates.Playing
            tmrPlayer.Start()
            ImagesBox.RedrawOnce = True
        End If
    End Sub
    Private Sub Player_PlaySong(FileName As String)
        CurrentSongFileName = FileName
        Player.URL = CurrentSongFileName
        Player.controls.play()
        tmrPlayer.Start()
    End Sub
    Private Sub Player_NextSong() Handles frmMusicPlayer.NextSong
        Try
            Dim i As Long = ImagesBox.IsInListIgnoreCase(CurrentSongFileName)
            Dim F As Boolean = False
            If i <> 0 Then
                i += 1
                While i <= ImagesBox.NImages And ImagesBox.Image(i).Type <> ucImagesBox.FileTypes.Music
                    i += 1
                End While
                If (i <= ImagesBox.NImages) Then
                    CurrentSongFileName = ImagesBox.Image(i).FileName
                    F = True
                End If
            End If
            If Not F Then
                CurrentSongFileName = GetRandomSong()
            End If
            Player.URL = CurrentSongFileName
            Player.controls.play()
            tmrPlayer.Start()
            UpdatePlayerInfo()
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub
    Private Function GetRandomSong() As String
        Dim k As Long = 0
        Dim r As New Random(DateTime.Now.Millisecond)
        While k = 0
            k = r.Next(1, FileTags.N)
            If FileTags.Tags(k).Type <> "music" Or Not IO.File.Exists(FileTags.Files(k)) Or Mid(FileTags.Files(k), FileTags.Files(k).Length - 2, 3) <> "mp3" Then
                k = 0
            End If
        End While
        Return FileTags.Files(k)
    End Function



    Private Sub UcSpotButton1_MouseMove(sender As Object, e As MouseEventArgs) Handles UcSpotButton1.MouseMove
        UpdatePlayerInfo()
        frmMusicPlayer.Location = UcSpotButton1.Location + Me.Location + picResize.Location - New Point(12, 12)
        frmMusicPlayer.Show()
    End Sub
    Private Sub UpdatePlayerInfo()
        If frmMusicPlayer.Visible Then
            If Player.URL <> "" Then
                If Player.currentMedia.duration > 0 Then
                    frmMusicPlayer.SetState(Player.controls.currentPositionString + "  /  " + Player.currentMedia.durationString, FileTags.Tags(FileTags.FindByFileName(CurrentSongFileName)).Song.Singer + " - " + FileTags.Tags(FileTags.FindByFileName(CurrentSongFileName)).Song.Name)
                End If
            End If
        End If
        If Player.playState = WMPLib.WMPPlayState.wmppsPlaying Then ImagesBox.PlayerState = ucImagesBox.PlayerStates.Playing
        If Player.playState = WMPLib.WMPPlayState.wmppsPaused Then ImagesBox.PlayerState = ucImagesBox.PlayerStates.Pause
        ImagesBox.InPlayerFileName = CurrentSongFileName
        ImagesBox.RedrawOnce = True
    End Sub

    Private Sub btnClose_Load(sender As Object, e As EventArgs) Handles btnClose.Load

    End Sub

    Private Sub UcSpotButton1_Load(sender As Object, e As EventArgs) Handles UcSpotButton1.Load

    End Sub

    Private Sub Button12_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnRefresh_Load(sender As Object, e As EventArgs) Handles btnRefresh.Load

    End Sub

    Private Sub btnOrderBigThumbs_Load(sender As Object, e As EventArgs) Handles btnOrderBigThumbs.Load

    End Sub
End Class
