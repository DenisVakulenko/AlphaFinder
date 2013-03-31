Public Class frmMusicPlayer
    'Private Const CS_DROPSHADOW As Integer = 131072
    '' Override the CreateParams property
    'Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
    '    Get
    '        Dim cp As CreateParams = MyBase.CreateParams
    '        cp.ClassStyle = cp.ClassStyle Or CS_DROPSHADOW
    '        Return cp
    '    End Get
    'End Property
    Enum PlayerStates As Short
        Pause = 2
        Playing = 1
        Stoped = 0
    End Enum

    Dim WithEvents Player As New WMPLib.WindowsMediaPlayer
    Dim BmpBG As Bitmap
    Dim PlayerState As PlayerStates = PlayerStates.Stoped

    Private Sub frmMusicPlayer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnPlay.LoadImages("player\play_mini.png")
        btnStop.LoadImages("player\stop_mini.png")

        PlayList.ResizeEnded()
        PlayList.LoadUIImages()
        playList.Arrangement = ucImagesBox.ArrangmentTypes.Нorizontal
        PlayList.SetWire(PlayList.Width - 10, 16, 0, 5)
        playList.Sorting = "user"
        'playList.DrawFrame = False
        'playList.DrawBorders = False

        sbPos.b3.Color = Me.BackColor
        sbVolume.b3.Color = Me.BackColor
        sbVolume.ShowText = True
        sbVolume.value = 50
        Player.settings.volume = 50

    End Sub

    Private Sub btnPlay_Clicked() Handles btnPlay.Clicked
        If Player.playState = WMPLib.WMPPlayState.wmppsPlaying Then
            Player.controls.pause()
            btnPlay.LoadImages("player\play_mini.png")
        ElseIf Player.playState = WMPLib.WMPPlayState.wmppsPaused Then
            Player.controls.play()
            btnPlay.LoadImages("player\pause_mini.png")
        Else
            If PlayList.NImages > 0 Then
                Player.URL = playList.Image(playList.SelectedImageIndex).FileName
                Player.controls.play()
                If Player.currentMedia.duration > 0 Then
                    With sbPos
                        .max = CInt(Player.currentMedia.duration)
                        .value = 0
                    End With
                End If
            End If
            btnPlay.LoadImages("player\pause_mini.png")
        End If
    End Sub

    Dim IndexOfRandomlyPlayed As Long = 0
    Private Sub NextRandomSong()
        Dim k As Long = 0
        Dim r As New Random(DateTime.Now.Millisecond)
        While k = 0
            k = r.Next(1, FileTags.N)
            If FileTags.Tags(k).Type <> "music" Or Not IO.File.Exists(FileTags.Files(k)) Or Mid(FileTags.Files(k), FileTags.Files(k).Length - 2, 3) <> "mp3" Then
                k = 0
            End If
        End While
        IndexOfRandomlyPlayed = k
        Me.Text = FileTags.Tags(k).Song.Name + " (" + FileTags.Tags(k).Song.Singer + ")"
        Player.URL = FileTags.Files(k)
        Player.controls.play()
        If Player.currentMedia.duration > 0 Then
            With sbPos
                .max = CInt(Player.currentMedia.duration)
                .value = 0
            End With
        End If
        playList.AddImage(FileTags.Files(k))
        playList.SelectedImageIndex = playList.NImages
        playList.SetImageLocation(playList.NImages)
        playList.RecalculateCanvasParameters()
        playList.SetCanvas()
        playList.StopLoading = False
        playList.IsNotEverithingLoaded = True
        playList.RedrawOnce = True
    End Sub

    Private Sub NextSong()
        If playList.SelectedImageIndex >= playList.NImages Then
            If IndexOfRandomlyPlayed > 0 Then
                FileTags.Tags(IndexOfRandomlyPlayed).LaunchingTimes += 1
            End If
            NextRandomSong()
        ElseIf playList.NImages > 0 Then
            playList.SelectedImageIndex += 1
            playList.CorrectSelectedImageIndex()
            playList.RedrawOnce = True
            Player.URL = playList.Image(playList.SelectedImageIndex).FileName
            Player.controls.play()
            'If Player.currentMedia.duration > 0 Then
            '    With sbPos
            '        .max = CInt(Player.currentMedia.duration)
            '        .value = 0
            '    End With
            'End If
        End If
    End Sub

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        If Player.playState = WMPLib.WMPPlayState.wmppsStopped Then
            NextSong()
        Else
            If Player.URL <> "" Then
                If Player.currentMedia.duration > 0 Then
                    'lblInfo.Text = (Math.Round(Player.controls.currentPosition * 100) / 100).ToString '+ " | " + Player.currentMedia.durationString
                    lblInfo.Text = Player.controls.currentPositionString + "  /  " + Player.currentMedia.durationString
                    With sbPos
                        .max = CInt(Player.currentMedia.duration)
                        .value = CInt(Player.controls.currentPosition)
                    End With
                End If
            End If
        End If
    End Sub

    Private Sub btnStop_Clicked() Handles btnStop.Clicked
        Player.controls.stop()
        btnPlay.LoadImages("player\play_mini.png")
    End Sub


    Private Sub sbVolume_ValueChanged(ByVal Value As Long) Handles sbVolume.ValueChanged
        Player.settings.volume = Value
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        NextSong()
        btnPlay.LoadImages("player\pause_mini.png")
    End Sub



    Dim frmPoint As Point


    Private Sub Me_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, lblInfo.MouseDown
        frmPoint = e.Location
    End Sub
    Private Sub frmMusicPlayer_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove, lblInfo.MouseMove
        If e.Button Then
            Dim newLeft, newTop As Short
            newLeft = Me.Left - frmPoint.X + e.X
            newTop = Me.Top - frmPoint.Y + e.Y

            'If Me.Width = Screen.PrimaryScreen.WorkingArea.Width And Me.Height = Screen.PrimaryScreen.WorkingArea.Height Then
            '    Me.Size = FrmRect.Size
            '    frmPoint.X = Me.Width / 2
            '    SetNormalBorders()
            'End If

            Dim d As Short = 10
            If Math.Abs(newTop) < d Then newTop = 0
            If Math.Abs(newLeft) < d Then newLeft = 0
            If Math.Abs(newLeft - (Screen.PrimaryScreen.WorkingArea.Width - Me.Width)) < d Then newLeft = Screen.PrimaryScreen.WorkingArea.Width - Me.Width
            If Math.Abs(newTop - (Screen.PrimaryScreen.WorkingArea.Height - Me.Height)) < d Then newTop = Screen.PrimaryScreen.WorkingArea.Height - Me.Height


            If newTop = 0 Then newTop = -1
            If newLeft = 0 Then newLeft = -1

            Me.Left = newLeft
            Me.Top = newTop
        End If
    End Sub

    Private Sub frmMusicPlayer_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        BmpBG = New Bitmap(284, 395)
        Using g As Graphics = Graphics.FromImage(BmpBG)
            g.Clear(Color.FromArgb(200, 200, 200))
            g.DrawRectangle(New Pen(Color.FromArgb(66, 66, 66)), New Rectangle(0, 0, Me.Width - 1, Me.Height - 1))
        End Using
        With BmpBG
            Dim ColorExternal As Color = Color.FromArgb(255, 0, 255)
            Dim ColorInternal As Color = Color.FromArgb(150, 0, 0, 0) '.FromArgb(19 * 0.7, 130 * 0.7, 206 * 0.7)
            .SetPixel(0, 0, ColorExternal)
            .SetPixel(1, 1, ColorInternal)
            .SetPixel(.Width - 1, .Height - 1, ColorExternal)
            .SetPixel(.Width - 2, .Height - 2, ColorInternal)
            .SetPixel(.Width - 1, 0, ColorExternal)
            .SetPixel(.Width - 2, 1, ColorInternal)
            .SetPixel(0, .Height - 1, ColorExternal)
            .SetPixel(1, .Height - 2, ColorInternal)
        End With
        Me.BackgroundImage = BmpBG
        Me.Refresh() : Me.Refresh() : Me.Refresh() : Me.Refresh()
    End Sub

    Private Sub btnClose_Clicked() Handles btnClose.Clicked
        FileTags.Save()
        Player.close()
        Me.Close()
        Me.Dispose()
    End Sub

    Public Sub New()
        InitializeComponent()
        btnClose.LoadImages("buttons\close.png")
    End Sub

    Private Sub sbPos_Load(sender As Object, e As EventArgs) Handles sbPos.Load

    End Sub

    Private Sub sbPos_ValueChanged(Value As Long) Handles sbPos.ValueChanged
        Player.controls.currentPosition = Value
    End Sub

    Private Sub playList_FileClick(Image As ucImagesBox.ImageStruct) Handles playList.FileClick
        'If Player.playState = WMPLib.WMPPlayState.wmppsPlaying Then
        '    Player.controls.pause()
        '    btnPlay.LoadImages("player\play_mini.png")
        'ElseIf Player.playState = WMPLib.WMPPlayState.wmppsPaused Then
        '    Player.controls.play()
        '    btnPlay.LoadImages("player\pause_mini.png")
        'Else
        If playList.NImages > 0 Then
            Player.URL = Image.FileName
            Player.controls.play()
            If Player.currentMedia.duration > 0 Then
                With sbPos
                    .max = CInt(Player.currentMedia.duration)
                    .value = 0
                End With
            End If
        End If
        btnPlay.LoadImages("player\pause_mini.png")
        'End If
    End Sub

    Private Sub btnClose_Load(sender As Object, e As EventArgs) Handles btnClose.Load

    End Sub
End Class