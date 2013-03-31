Public Class ucFilesBox
    Event BackSpaceKey()
    Event SendFocusToTheTop(ByRef Done As Boolean)
#Region "--------------------------|  STRUCTURES            |--------------------------"
    Structure ButtonStruct
        Dim X As Short
        Dim Y As Short
        Dim Image As Bitmap
        Dim Visible As Boolean
    End Structure
    Structure WireStruct
        Dim X As Short
        Dim Y As Short
        Dim min_dX As Short
        Dim dX As Short
        Dim dY As Short
        Dim Border As Short
    End Structure
    Structure CanvasStruct
        Dim LinesInBox As Short
        Dim Lines As Short
        Dim Columns As Short
        Dim Height As Long
        Dim MinY As Long
        Dim Y As Double
        Dim DestY As Long
        Dim V As Double
        Dim Animate As Boolean
        Dim Plus As Short
    End Structure
    Public Structure ImageStruct
        Dim X As Double
        Dim Y As Double
        Dim DestX As Integer
        Dim DestY As Integer

        Dim WidthWithText As Short
        Dim Animate As Boolean

        Dim Name As String
        Dim FileName As String

        Dim Loaded As Boolean
        Dim Loading As Boolean
        Dim ReLoaded As Boolean

        Dim Transparency As Double
        Dim DestTransparency As Double

        Dim Selected As Boolean
        Dim InTagsIngex As Long

        Dim Visible As Boolean
    End Structure
#End Region
    Public SelectedImageIndex As Long = 0
    Public ShowImagesName As Boolean = False
    Public View As String ' "big" "ave" "min"
    Private VScrolling As Boolean ' true - vertical; false - gorizontal
    Dim IsNotEverithingLoaded As Boolean = False
    Dim IsEnterDown As Boolean
    Dim DraggingFilesList() As Long
#Region "BUTTONS"
    Dim Buttons(1) As ButtonStruct
    Public Sub InitButtons()
        Try
            ReDim Buttons(11)
            Buttons(6).X = 2
            Buttons(6).Y = 2
            Buttons(6).Image = New Bitmap(Application.StartupPath + "\order.png")
            Buttons(6).Visible = False

            Buttons(0).X = 2
            Buttons(0).Y = 2
            Buttons(0).Image = New Bitmap(Application.StartupPath + "\sortbyname.gif")
            Buttons(0).Visible = False
            Buttons(1).X = 2
            Buttons(1).Y = 21
            Buttons(1).Image = New Bitmap(Application.StartupPath + "\sortbydate.gif")
            Buttons(1).Visible = False
            Buttons(7).X = 2
            Buttons(7).Y = 19 * 2 + 2
            Buttons(7).Image = New Bitmap(Application.StartupPath + "\sortby_type.bmp")
            Buttons(7).Visible = False
            Buttons(8).X = 2
            Buttons(8).Y = 19 * 3 + 2
            Buttons(8).Image = New Bitmap(Application.StartupPath + "\sortbyPopularity.bmp")
            Buttons(8).Visible = False


            Buttons(9).X = 2
            Buttons(9).Y = 19 * 3 + 2 + 5 + 19
            Buttons(9).Image = New Bitmap(Application.StartupPath + "\rate1.bmp")
            Buttons(9).Visible = False
            Buttons(10).X = 2
            Buttons(10).Y = 19 * 3 + 2 + 5 + 19 * 2
            Buttons(10).Image = New Bitmap(Application.StartupPath + "\rate2.bmp")
            Buttons(10).Visible = False
            Buttons(11).X = 2
            Buttons(11).Y = 19 * 3 + 2 + 5 + 19 * 3
            Buttons(11).Image = New Bitmap(Application.StartupPath + "\rate3.bmp")
            Buttons(11).Visible = False

            Buttons(5).X = 2
            Buttons(5).Y = 2
            Buttons(5).Image = New Bitmap(Application.StartupPath + "\sort.png")
            Buttons(5).Visible = True

            Buttons(2).X = 55
            Buttons(2).Y = 17
            Buttons(2).Image = New Bitmap(Application.StartupPath + "\scroller\scroll_thing.png")
            Buttons(2).Visible = False

            Buttons(3).X = 55
            Buttons(3).Y = 17
            Buttons(3).Image = New Bitmap(Application.StartupPath + "\scroller\scroll_up.png")
            Buttons(3).Visible = False
            Buttons(4).X = 55
            Buttons(4).Y = 17
            Buttons(4).Image = New Bitmap(Application.StartupPath + "\scroller\scroll_down.png")
            Buttons(4).Visible = False
        Catch EX As Exception
        End Try
    End Sub

#End Region
#Region "--------------------------|  UI IMAGES             |-------------------------- GOOD"
    'Dim BmpScrollThing, BmpScrollThingMD, BmpScrollUp, BmpScrollDown As Bitmap
    Dim bmp_error, bmp_star, bmp_muz_ico As Bitmap
    Public Sub LoadUIImages()
        bmp_error = New Bitmap(Application.StartupPath + "\err--.png")
        bmp_star = New Bitmap(Application.StartupPath + "\star2--.png")
        bmp_muz_ico = New Bitmap(Application.StartupPath + "\muz--.png")
        'BmpScrollThing = New Bitmap(Application.StartupPath + "\scroller\scroll_thing.png")
        'BmpScrollUp = New Bitmap(Application.StartupPath + "\scroller\scroll_up.gif")
        'BmpScrollDown = New Bitmap(Application.StartupPath + "\scroller\scroll_down.gif")
        'BmpScrollThingMD = New Bitmap(Application.StartupPath + "\scroller\scroll_thing_md.bmp")
    End Sub
#End Region
#Region "--------------------------|  WIRE & PARAMETERS     |--------------------------"
    Public Wire As WireStruct, Canvas As CanvasStruct

    Private Sub CalculateCanvasHeight()
        If Canvas.Columns > 0 Then
            Canvas.Height = (Math.Truncate((NImages - 1) / Canvas.Columns) + 1) * (Wire.Y + Wire.dY) + Wire.dY
        Else
            Canvas.Height = 0
            'MsgBox("really!")
        End If
    End Sub
    Private Sub CalculateCanvasMinY()
        'Canvas.Lines = Math.Truncate(NImages / Canvas.Columns) '!!!!!!!!!!!!!!!!!!!!!!!!!
        Canvas.MinY = Me.Height - Canvas.Height
        If Canvas.MinY > 0 Then Canvas.MinY = 0
    End Sub
    Private Sub RecalculateCanvasParameters()
        If Wire.Y <> 0 Then
            Canvas.LinesInBox = Math.Truncate(BmpMain.Height / (Wire.Y + Wire.dY))
            Canvas.Columns = Math.Truncate((BmpMain.Width - Wire.min_dX) / (Wire.X + Wire.min_dX))
            If Canvas.Columns < 1 Then Canvas.Columns = 1
            Wire.dX = (BmpMain.Width - Canvas.Columns * Wire.X) / (Canvas.Columns + 1)
            Canvas.Plus = Math.Truncate((BmpMain.Width - ((Wire.X + Wire.dX) * Canvas.Columns - Wire.dX)) / 2)
        End If
    End Sub
    Public Sub SetWire(ByVal x As Short, ByVal y As Short, ByVal dx As Short, ByVal dy As Short)
        If x > 500 Then x = 500
        If y > 500 Then y = 500
        Wire.X = x
        Wire.Y = y
        Wire.min_dX = dx
        Wire.dY = dy

        RecalculateCanvasParameters()
        'ClearThumbs()
        'MakeAllThumbnails()
    End Sub
#End Region
#Region "--------------------------|  IMAGE LIST            |-------------------------- GOOD"
    Public Image(50000) As ImageStruct, NImages As Long, IsAnimatedImages As Boolean

    Public Sub ClearThumbs()
        For i As Short = 1 To NImages
            If Image(i).Loaded Then
                Image(i).Loaded = False
                Image(i).Loading = False
                Image(i).ReLoaded = False
                Image(i).Transparency = 0
                Image(i).Selected = False
                Thumbnail(i).Dispose()
            End If
        Next
    End Sub
    Public Sub ClearImages()
        tmrAnimation.Enabled = False
        ClearThumbs()
        NImages = 0
        SelectedImageIndex = 0
        Canvas.Y = 0
        Canvas.V = 0
        NextFrameInAnyWay()
        SelectedImageIndex = 0
        tmrAnimation.Enabled = True
    End Sub
    Public Sub Add(ByVal Filename As String)
        NImages += 1
        Image(NImages).FileName = Filename
        Image(NImages).Name = GetFileName(Filename)
        SelectedImageIndex = 1
    End Sub
    Public Sub SortImagesByName()
        Dim i, j As Long
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If Image(i).FileName > Image(i + 1).FileName Then
                    SwapImages(i, i + 1)
                End If
            Next
        Next
        OrderImages()
    End Sub
    Public Sub SortImagesByType()
        Dim i, j As Long
        'My.Computer.Audio.Play

        Dim Temp(NImages), t As String
        For i = 1 To NImages - 1
            Temp(i) = LCase(IO.Path.GetExtension(Image(i).FileName))
        Next

        For j = 1 To NImages
            For i = 1 To NImages - 1
                If Temp(i) > Temp(i + 1) Then
                    SwapImages(i, i + 1)
                    t = Temp(i)
                    Temp(i) = Temp(i + 1)
                    Temp(i + 1) = t
                End If
            Next
        Next
        OrderImages()
    End Sub
    Public Sub SortImagesByRating()
        Dim i, j As Long
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If FileTags.Tags(Image(i).InTagsIngex).Rating < FileTags.Tags(Image(i + 1).InTagsIngex).Rating Then
                    SwapImages(i, i + 1)
                End If
            Next
        Next
        OrderImages()
    End Sub
    Public Sub SortImagesByLaunchingCount()
        Dim i, j As Long
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If FileTags.Tags(Image(i).InTagsIngex).LaunchingTimes < FileTags.Tags(Image(i + 1).InTagsIngex).LaunchingTimes Then
                    SwapImages(i, i + 1)
                End If
            Next
        Next
        OrderImages()
    End Sub
    Public Sub SortImagesByDate()
        Dim i, j As Long
        Dim dates(NImages), d As Long
        For i = 1 To NImages
            dates(i) = System.IO.File.GetCreationTime(Image(i).FileName).Ticks
        Next
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If dates(i) > dates(i + 1) Then
                    SwapImages(i, i + 1)
                    d = dates(i)
                    dates(i) = dates(i + 1)
                    dates(i + 1) = d
                End If
            Next
        Next
        OrderImages()
    End Sub
    Private Sub SwapImages(ByVal a As Long, ByVal b As Long)
        Dim temp As ImageStruct
        temp = Image(b)
        Image(b) = Image(a)
        Image(a) = temp

        Dim tempBmp As Bitmap
        tempBmp = Thumbnail(a)
        Thumbnail(a) = Thumbnail(b)
        Thumbnail(b) = tempBmp
    End Sub
#End Region
#Region "--------------------------|  LOCATION & ANIMATING  |-------------------------- GOOD"
    Private Sub SetImageLocation(ByVal i As Long)
        Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
        Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
        Image(i).X = Image(i).DestX
        Image(i).Y = Image(i).DestY
        Image(i).Transparency = 0
        Image(i).Animate = True
    End Sub
    Private Sub SetImageDestination(ByVal i As Long)
        Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
        Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
        Image(i).Animate = True
    End Sub
    Public Sub SetImagesLocation()
        For i As Long = 1 To NImages
            SetImageLocation(i)
        Next
        CalculateCanvasHeight()
        CalculateCanvasMinY()
        IsAnimatedImages = True
    End Sub
    Public Sub OrderImages()
        For i As Long = 1 To NImages
            With Image(i)
                .DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                .DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
                .DestTransparency = 1
                .Animate = True
            End With
        Next
        CalculateCanvasHeight()
        CalculateCanvasMinY()
        IsAnimatedImages = True
    End Sub
    Public Sub RandomImages()
        For i As Long = 1 To NImages
            If Image(i).Loaded Then
                Image(i).Animate = True
                Image(i).DestX = Rnd() * (BmpMain.Width - Thumbnail(i).Width) '(Wire.X - Thumbnail(i).Width) / 2 + ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                Image(i).DestY = Rnd() * (BmpMain.Height - Thumbnail(i).Height) '(Wire.Y - Thumbnail(i).Height) / 2 + Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
            End If
        Next
        CalculateCanvasHeight()
        CalculateCanvasMinY()
        IsAnimatedImages = True
    End Sub
    Private Sub StopAnimatingImages()
        For i As Long = 1 To NImages
            Image(i).Animate = False
        Next
        IsAnimatedImages = False
    End Sub
    Private Sub StopCanvas()
        Canvas.Y = 0 : Canvas.V = 0
        Canvas.Animate = False
    End Sub
#End Region
#Region "--------------------------|  THUMBNAILS            |--------------------------"
    Dim Thumbnail(50000) As Bitmap
    Dim LoadingProgress As Double, Str As String
    Public StopLoading As Boolean = False

    Public Sub ReloadAllThumbs()
        StopLoading = False
        For i As Long = 1 To NImages
            Image(i).ReLoaded = False
        Next
        IsNotEverithingLoaded = True
    End Sub

    Public Sub MakeAllThumbnails()
        tmrAnimation.Enabled = False

        IsNotEverithingLoaded = True

        Cursor = Cursors.Arrow
        StopLoading = False
        RecalculateCanvasParameters()
        CurrentLoadingImgIndex = 0

        CalculateCanvasHeight()
        CalculateCanvasMinY()

        tmrAnimation.Enabled = True
    End Sub
    Private Sub bwLoadImages_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadImages.ProgressChanged
        LoadingProgress = e.ProgressPercentage / NImages
    End Sub
    Private Sub bwLoadImages_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadImages.DoWork
        Try
            'MsgBox("F")
        Catch ex As Exception
        End Try

        Str = ""

        For i As Long = 1 To NImages
            If StopLoading Then
                SetImageLocation(i)
            Else
                'CurrentLoadingImg = i
                'While bwLoadOne.IsBusy
                '    Application.DoEvents()
                'End While
                Application.DoEvents()
                Dim w As Boolean

                MakeThumbnail(i) ', w)
                'SetImageLocation(i)
                Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
                Image(i).X = Image(i).DestX
                Image(i).Y = Image(i).DestY
                If w Then Image(i).Y = Image(i).DestY - 20 : Image(i).Transparency = 0 Else Image(i).Transparency = 1
                Image(i).Animate = True
                If i > 1 Then Image(i - 1).Loaded = True
                Application.DoEvents()
                bwLoadOne.RunWorkerAsync()

                IsAnimatedImages = True
                bwLoadImages.ReportProgress(i)
            End If
        Next
        'Image(i).Loaded = True
        SaveThumbsInfo()
    End Sub

    Dim LoadedThumbnail As Bitmap, LoadingStrInfo As String
    Public CurrentLoadingImgIndex As Long, CurrentLoadingFileName As String
    Private Sub bwLoadOne_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadOne.DoWork
        MakeThumbnail(CurrentLoadingFileName)
    End Sub
    Private Sub MakeThumbnail(ByVal FileName As String) ', ByRef wait As Boolean)
        Dim wait As Boolean = True
        If InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(FileName)) + " ") Then
            Dim ThNumber As Long = SearchInThumbs(FileName)
            Dim bmp, bmp1 As Bitmap
            'Dim s As New Point(Wire.X, Wire.Y - 14)
            Dim s As New Point(Wire.Y - Wire.Border * 2, Wire.Y - Wire.Border * 2) 'Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)

            If Wire.Y < 40 Then
                s.Y = Wire.Y
            Else
                If ShowImagesName Then s.Y -= 14
            End If


            If ThNumber <> 0 And IO.File.Exists(Application.StartupPath + "\config\th\" + ThNumber.ToString + ".jpg") Then                                     'IF THUMBNAIL EXISTS                
                bmp = New Bitmap(Application.StartupPath + "\config\th\" + ThNumber.ToString + ".jpg")

                Dim W As Short = bmp.Width
                Dim H As Short = bmp.Height

                If (W > s.X Or H > s.Y) And ThumbIsNotOld(ThNumber) Then     'IF THUMBNAIL SIZE IS BIGGER THAN NECESSARY
                    LoadingStrInfo = "thumb. is bigger"
                    CorrectSize(W, H, s)

                    bmp1 = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                    Using graf As Graphics = Graphics.FromImage(bmp1)
                        graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                        graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                        graf.DrawImage(bmp, -1, -1, W + 2, H + 2)
                        graf.DrawImage(bmp, 0, 0, W, H) 'graf.DrawImage(bmp1, -1, -1, W + 1, H + 1)   'graf.DrawImage(bmp1, 0, 0, W, H)
                        LoadedThumbnail = bmp1.Clone
                    End Using
                    bmp1.Dispose()
                    bmp.Dispose()
                    wait = False
                Else
                    If (W = s.X Or H = s.Y) And ThumbIsNotOld(ThNumber) Then  'IF THUMBNAIL SIZE IS RIGHT
                        LoadingStrInfo = "thumb. is ideal"
                        bmp1 = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                        Using graf As Graphics = Graphics.FromImage(bmp1)
                            'graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                            'graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                            'graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                            graf.DrawImage(bmp, 0, 0, W, H) 'graf.DrawImage(bmp1, -1, -1, W + 1, H + 1)   'graf.DrawImage(bmp1, 0, 0, W, H)
                            LoadedThumbnail = bmp1.Clone
                        End Using
                        bmp1.Dispose()
                        bmp.Dispose()
                        bmp = Nothing
                        wait = False
                    Else                                               'IF THUMBNAIL SIZE IS BAD
                        'MsgBox(W.ToString + "  " + H.ToString)
                        LoadingStrInfo = "thumb. is smaller"
                        bmp.Dispose()
                        If Not ReMakeThumb(ThNumber, s, LoadedThumbnail) Then LoadedThumbnail = bmp_error.Clone
                    End If
                End If
                'LoadingStrInfo = "wait please..." + "   |   " + Str + "   |   " + "( " + i.ToString + " / " + NImages.ToString + " )"
            Else                                               'IF THERE IS NO THUMBNAIL
                LoadingStrInfo = "thumb. does not exists"
                'LoadingStrInfo = "wait please..." + "   |   " + Str + "   |   " + "( " + i.ToString + " / " + NImages.ToString + " )"

                If Not MakeThumb(FileName, s, LoadedThumbnail) Then LoadedThumbnail = bmp_error.Clone
            End If
        Else
            wait = False
            '1. Ищем в реестре расширение: «HKEY_CLASSES_ROOT\.doc». Берём значение из «HKEY_CLASSES_ROOT\.doc\(Default)» (например, «Word.Document.8»).
            '2. Ищем «HKEY_CLASSES_ROOT\Word.Document.8», берём значение из «HKEY_CLASSES_ROOT\Word.Document.8\DefaultIcon\(Default)» (например, «C:\WINDOWS\Installer\{90110419-6000-11D3-8CFE-0150048383C9}\wordicon.exe,1»).
            'MsgBox(Mid(Image(i).FileName, Image(i).FileName.Length - 2, 3))
            'If Mid(FileName, FileName.Length - 2, 3) <> "mp3" And Mid(FileName, FileName.Length - 3, 4) <> "flac" Then
            'LoadedThumbnail = Icon.ExtractAssociatedIcon(FileName).ToBitmap

            LoadedThumbnail = New Bitmap(16, 16)
            Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                Dim r As New Rectangle(0, 0, 16, 16)
                graf.DrawIcon(Icon.ExtractAssociatedIcon(FileName), r)
            End Using
            'Else
            'LoadedThumbnail = bmp_muz_ico.Clone
            'End If
        End If
    End Sub
#End Region
#Region "-----|  RESIZING "
    Public Sub ResizeStarted()
        DoResizeEvent = False
        tmrAnimation.Enabled = False
        picMain.Visible = False
        Me.BackgroundImage = BmpMain
    End Sub
    Public Sub Resizing(ByVal w As Integer, ByVal h As Integer)
        If h > 35 Then
            BmpMain = New Bitmap(w, h)
            GraphicsMain = Graphics.FromImage(BmpMain)
            NextFrameInAnyWay() '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Me.BackgroundImage = BmpMain
            Me.Height = h
        End If
        If h > 0 And h <= 35 Then
            BmpMain = New Bitmap(w, h)
            GraphicsMain = Graphics.FromImage(BmpMain)
            NextFrameInAnyWay()

            If h < 35 Then
                Dim a2 As New SolidBrush(Color.AliceBlue)
                If h < 10 Then a2.Color = Color.FromArgb(255, 200, 200, 200) Else a2.Color = Color.FromArgb(350 - h * 10, 200, 200, 200)
                GraphicsMain.FillRectangle(a2, 0, 0, w, h)
            End If

            Me.BackgroundImage = BmpMain

            Me.Height = h
        End If
    End Sub
    Public Sub ResizeEnded()
        tmrAnimation.Enabled = True

        picMain.Width = Me.Width
        picMain.Height = Me.Height
        picMain.Image = BmpMain
        picMain.Visible = True
        DoResizeEvent = True
    End Sub
#End Region
#Region "-----|  Mouse Movings"
    Dim IsMouseDown As Boolean
    Dim Cl As New Point(10, 10), ClF As Point, moved As Boolean = False, ChosenObj As Long
    Dim MDTime As Long = 0

    Dim IsSmthDragging As Boolean = False
    Dim DraggingCount As Long = 0
    Private Sub picMain_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picMain.DragEnter, Me.DragEnter
        e.Effect = DragDropEffects.Copy
        'Cl.X = e.X
        'Cl.Y = e.Y
        'IsSmthDragging = True
    End Sub
    Private Sub ucImagesBox_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragOver
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)

        NextFrameInAnyWay()

        Dim x, y As Long
        x = e.X - Me.Left - Me.Parent.Left
        y = e.Y - Me.Top - Me.Parent.Top

        Dim ii As Short
        'ChosenObj = NImages

        GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim b As New SolidBrush(Color.FromArgb(150, 120, 120, 120))
        Dim w As Long = GraphicsMain.MeasureString(files.Length, font_filename).Width - 1
        If w < 10 Then w = 10
        GraphicsMain.FillEllipse(b, x - 17, y - 17, 10 + w, 20)
        GraphicsMain.DrawEllipse(Pens.Black, x - 17, y - 17, 10 + w, 20)
        GraphicsMain.DrawString(files.Length, font_filename, Brushes.Black, x - 12, y - 13)
        GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.None

        Image(ChosenObj).X += x - Cl.X : Cl.X = x
        Image(ChosenObj).Y += y - Cl.Y : Cl.Y = y

        x = x - Canvas.Plus : y = y - Wire.dY - Canvas.Y
        ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        If ii > NImages Or (Math.Truncate(x / (Wire.X + Wire.dX)) + 1) > Canvas.Columns Then ii = NImages
        If ii > ChosenObj Then
            For i As Long = ChosenObj To ii - 1
                SwapImages(i, i + 1)
                SetImageDestination(i)
                IsAnimatedImages = True
            Next
        Else
            For i As Long = ChosenObj To ii + 1 Step -1
                SwapImages(i, i - 1)
                SetImageDestination(i)
                IsAnimatedImages = True
            Next
        End If
        ChosenObj = ii
        SelectedImageIndex = ChosenObj

        picMain.Image = BmpMain
        picMain.Refresh()
    End Sub
    Private Sub picMain_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picMain.DragDrop, Me.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
        Dim t As String
        StopLoading = False
        For Each t In files
            Dim f As Boolean = True
            For i As Long = 1 To NImages
                If Image(i).FileName = t Then f = False : Exit For
            Next
            If f Then
                Add(t)

                Image(NImages).Loaded = False
                IsNotEverithingLoaded = True

                Image(NImages).X = e.X - Me.Left - Me.Parent.Left
                Image(NImages).Y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y
                Image(NImages).DestX = Image(NImages).X
                Image(NImages).DestY = Image(NImages).Y

                Dim x = Image(NImages).X
                Dim y = Image(NImages).Y
                Dim ii As Short
                ChosenObj = NImages

                x = x - Canvas.Plus : y = y - Wire.dY
                ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
                If ii > NImages Or (Math.Truncate(x / (Wire.X + Wire.dX)) + 1) > Canvas.Columns Then ii = NImages
                'If x Mod (Wire.X + Wire.dX) - Wire.X > 0 Or y Mod (Wire.Y + Wire.dY) - Wire.Y > 0 Then ii = 0
                'If ii <> 0 And ii <> ChosenObj Then
                If ii > ChosenObj Then
                    For i As Long = ChosenObj To ii - 1
                        SwapImages(i, i + 1)
                        SetImageDestination(i)
                        IsAnimatedImages = True
                    Next
                Else
                    For i As Long = ChosenObj To ii + 1 Step -1
                        SwapImages(i, i - 1)
                        SetImageDestination(i)
                        IsAnimatedImages = True
                    Next
                End If
                ChosenObj = ii
                SelectedImageIndex = 0 'ChosenObj
                'For i As Long = 1 To NImages
                '    Image(i).Transparency = 1
                'Next
                'Image(ii).Transparency = 0.5
                'End If

                'Image(NImages).Transparency = 0
                'Image(NImages).Animate = True
                'Image(NImages).Loaded = True
                IsAnimatedImages = True
                OrderImages()
            End If
        Next
        OrderImages()
        IsAnimatedImages = True
        IsSmthDragging = False
    End Sub
    Private Sub ucImagesBox_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DragLeave
        'NextFrameInAnyWay()
        IsSmthDragging = False
        OrderImages()
        IsAnimatedImages = True
    End Sub

    Private Sub CorrectCanvasDestY()
        If Canvas.DestY > 0 Then Canvas.DestY = 0
        If Canvas.DestY < Canvas.MinY Then Canvas.DestY = Canvas.MinY
    End Sub
    Private Sub picPhoto_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDown 'Me.MouseDown
        'If e.Button = Windows.Forms.MouseButtons.Right Then MDTime = 1
        Me.Select()
        moved = False
        ChosenObj = 0
        Dim a As ButtonStruct
        Dim i, ii As Long
        For i = 0 To Buttons.Length - 1
            a = Buttons(i)
            If e.X >= a.X And e.X <= a.X + a.Image.Width And e.Y >= a.Y And e.Y <= a.Y + a.Image.Height And a.Visible = True Then
                Select Case i
                    Case 0
                        SortImagesByName()
                        Buttons(5).Visible = 1
                        Buttons(0).Visible = 0
                        Buttons(1).Visible = 0
                        Buttons(7).Visible = 0
                        Buttons(8).Visible = 0
                    Case 1
                        SortImagesByDate()
                        Buttons(5).Visible = 1
                        Buttons(0).Visible = 0
                        Buttons(1).Visible = 0
                        Buttons(7).Visible = 0
                        Buttons(8).Visible = 0
                    Case 7
                        SortImagesByType()
                        Buttons(5).Visible = 1
                        Buttons(0).Visible = 0
                        Buttons(1).Visible = 0
                        Buttons(7).Visible = 0
                        Buttons(8).Visible = 0
                    Case 8
                        SortImagesByLaunchingCount()
                        Buttons(5).Visible = 1
                        Buttons(0).Visible = 0
                        Buttons(1).Visible = 0
                        Buttons(7).Visible = 0
                        Buttons(8).Visible = 0
                    Case 2
                        IsMouseDown = True
                        Canvas.Animate = False
                    Case 3
                        If Canvas.Y >= 0 Then
                            Canvas.V = 10
                        Else
                            Canvas.DestY = Canvas.Y + (Wire.Y + Wire.dY)
                            CorrectCanvasDestY()
                            Canvas.Animate = True
                        End If
                    Case 4
                        Canvas.DestY = Canvas.Y - (Wire.Y + Wire.dY)
                        CorrectCanvasDestY()
                        Canvas.Animate = True
                    Case 5
                        Buttons(5).Visible = 0
                        Buttons(0).Visible = 1
                        Buttons(1).Visible = 1
                        Buttons(7).Visible = 1
                        Buttons(8).Visible = 1
                    Case 6
                        OrderImages()

                    Case 9
                        FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 1
                    Case 10
                        FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 2
                    Case 11
                        FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 3
                End Select
                ChosenObj = -i - 1
                Exit For
            End If
        Next

        MDTime = 1

        Dim p As New Point(e.X, e.Y - Canvas.Y)
        Cl.X = e.X : Cl.Y = e.Y
        ClF.X = Cl.X : ClF.Y = Cl.Y

        If ChosenObj = 0 Then
            Canvas.V = 0
            Canvas.Animate = False
            IsMouseDown = True
            For ii = 1 To NImages
                i = ii 'foto_p(ii)
                If Image(i).Loaded Then
                    If p.X >= Image(i).X And p.X < Image(i).X + Image(i).WidthWithText And p.Y >= Image(i).Y And p.Y < Image(i).Y + Wire.Y Then
                        Image(i).Animate = False
                        If e.Button = Windows.Forms.MouseButtons.Right Then
                            Image(i).Selected = Not (Image(i).Selected)
                        End If
                        'For a As Long = ii - 1 To 1 Step -1
                        '    foto_p(a + 1) = foto_p(a)
                        'Next a
                        'foto_p(1) = i
                        ChosenObj = i
                        SelectedImageIndex = i
                        Exit For
                    End If
                Else
                    If p.X >= Image(i).X And p.X < Image(i).X + Wire.X And p.Y >= Image(i).Y And p.Y < Image(i).Y + Wire.Y Then
                        Image(i).Animate = False
                        ChosenObj = i
                        SelectedImageIndex = i
                        Exit For
                    End If
                End If
            Next
        End If
        NextFrameInAnyWay()
    End Sub
    Private Sub picPhoto_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseMove 'Me.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            If ChosenObj = 0 Then
                Canvas.Y += e.Y - Cl.Y
                ClF.X = Cl.X : ClF.Y = Cl.Y
                Cl.X = e.X : Cl.Y = e.Y
            Else
                If ChosenObj > 0 And (Cl.X <> e.X Or Cl.Y <> e.Y) Then
                    Image(ChosenObj).X += e.X - Cl.X
                    Image(ChosenObj).Y += e.Y - Cl.Y

                    Dim x = Image(ChosenObj).X
                    Dim y = Image(ChosenObj).Y
                    Dim ii As Short
                    'MsgBox(Canvas.Plus)
                    'If ChosenObj > CurrentLoadingImgIndex + 1 Or ChosenObj < CurrentLoadingImgIndex - 1 Then
                    If Image(ChosenObj).Loading = False Then 'ChosenObj < CurrentLoadingImgIndex Then
                        '    x = x - Canvas.Plus + Thumbnail(ChosenObj).Width / 2 : y = y - Wire.dY + Thumbnail(ChosenObj).Height / 2 '- Canvas.Y
                        '    ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
                        '    If ii > NImages Or (Math.Truncate(x / (Wire.X + Wire.dX)) + 1) > Canvas.Columns Then ii = NImages
                        '    If x Mod (Wire.X + Wire.dX) - Wire.X > 0 Or y Mod (Wire.Y + Wire.dY) - Wire.Y > 0 Then ii = 0
                        '    If ii <> 0 And ii <> ChosenObj Then    
                        '        If ii > ChosenObj Then
                        '            For i As Long = ChosenObj To ii - 1
                        '                SwapImages(i, i + 1)
                        '                SetImageDestination(i)
                        '                IsAnimatedImages = True
                        '            Next
                        '        Else
                        '            For i As Long = ChosenObj To ii + 1 Step -1
                        '                SwapImages(i, i - 1)
                        '                SetImageDestination(i)
                        '                IsAnimatedImages = True
                        '            Next
                        '        End If
                        '        ChosenObj = ii
                        '        SelectedImageIndex = ChosenObj
                        '        'For i As Long = 1 To NImages
                        '        '    Image(i).Transparency = 1
                        '        'Next
                        '        'Image(ChosenObj).Transparency = 0.7
                        '    End If
                        If e.X < 0 Or e.Y < 0 Or e.X > Me.Width Or e.Y > Me.Height Then
                            'For i As Long = ChosenObj To NImages - 1
                            '    SwapImages(i, i + 1)
                            'Next
                            'NImages = NImages - 1

                            NextFrameInAnyWay()
                            OrderImages()

                            '                            StartDragDrop(ChosenObj)
                        End If
                    End If
                    If moved = False Then
                        If (ClF.X - e.X) ^ 2 + (ClF.Y - e.Y) ^ 2 >= 9 Then
                            moved = True
                            MDTime = 0

                            If Image(ChosenObj).Selected Then
                                Dim n As Long = 0
                                For j As Long = 1 To NImages
                                    If Image(j).Selected Then n = n + 1
                                Next
                                ReDim DraggingFilesList(n)
                                n = 0
                                For j As Long = 1 To NImages
                                    If Image(j).Selected Then
                                        n = n + 1
                                        DraggingFilesList(n) = j
                                        If ChosenObj <> j Then
                                            Image(j).DestX = Image(ChosenObj).X
                                            Image(j).DestY = Image(ChosenObj).Y
                                            Image(j).DestTransparency = 0.6
                                            Image(j).Animate = True
                                        End If
                                    End If
                                Next
                                IsAnimatedImages = True
                                'StartDragDrop(ChosenObj)
                            End If
                            StartDragDrop(ChosenObj)
                        End If
                        'Cl.X = e.X : Cl.Y = e.Y
                        'Else
                        'ClF.X = Cl.X : ClF.Y = Cl.Y
                        'Cl.X = e.X : Cl.Y = e.Y
                    End If
                    Cl.X = e.X : Cl.Y = e.Y
                Else
                    If ChosenObj = -3 Then
                        Canvas.Y += (e.Y - Cl.Y) / ((Me.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) / Canvas.MinY)
                        Cl.Y = e.Y
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub picPhoto_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseUp 'Me.MouseUp
        If ChosenObj = 0 Then Canvas.V = e.Y - ClF.Y
        If moved = False And e.Button = Windows.Forms.MouseButtons.Left Then 'And ChosenObj > 0 Then
            If ChosenObj > 0 Then
                If SelectedImageIndex > 0 Then
                    If InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(Image(SelectedImageIndex).FileName)) + " ") Then
                        Dim a As New frmShowPhoto2
                        Dim FN(NImages) As String
                        For j As Long = 1 To NImages
                            FN(j) = Image(j).FileName
                        Next
                        If Image(SelectedImageIndex).Loaded = True Then
                            a.Init3(Thumbnail(SelectedImageIndex), SelectedImageIndex, FN, Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
                        Else
                            a.Init3(bmp_error, SelectedImageIndex, FN, Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
                        End If
                        a.Show()
                    Else
                        Process.Start(Image(SelectedImageIndex).FileName, AppWinStyle.NormalFocus)
                    End If
                    FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).LaunchingTimes += 1
                End If
            End If
        Else
        End If
        ChosenObj = 0
        IsMouseDown = False
        MDTime = 0
        NextFrameInAnyWay()
        OrderImages()
    End Sub
#End Region

    Public BmpMain As Bitmap = New Bitmap(10, 10), GraphicsMain As Graphics = Graphics.FromImage(BmpMain)

    Dim font_filename As New Font("Verdana", 11, FontStyle.Regular, GraphicsUnit.Pixel)
    Dim font_singer = New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Pixel)

    Dim TextColor As New SolidBrush(Color.Black)
    Dim b4 As New SolidBrush(Color.FromArgb(140, 0, 0, 0))

    Dim SelRectColor As New SolidBrush(Color.FromArgb(110, 255, 255, 255))
    Dim SelRectFrameColor As New Pen(Color.FromArgb(70, 0, 0, 0))

    Public BGColor As Color = Color.Gray
    Public FrameSize As Short = 1
    Public DrawShadow As Boolean = True


    Private Sub Draw_FreeAreas()
        If Canvas.Y > 0 Then
            Dim a2 As New SolidBrush(Color.FromArgb(100, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B))
            Dim ii As Long = Canvas.Y
            GraphicsMain.FillRectangle(a2, 0, 0, BmpMain.Width, ii)

            Dim c As New Pen(Color.FromArgb(0, 255, 255, 255))
            GraphicsMain.DrawLine(c, 0, ii, BmpMain.Width, ii)
            c = New Pen(Color.FromArgb(160, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 1, BmpMain.Width, ii - 1)
            c = New Pen(Color.FromArgb(80, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 2, BmpMain.Width, ii - 2)
            c = New Pen(Color.FromArgb(30, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 3, BmpMain.Width, ii - 3)
            c = New Pen(Color.FromArgb(10, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 4, BmpMain.Width, ii - 4)
        End If
        If Canvas.Y < Canvas.MinY Then
            Dim a2 As New SolidBrush(Color.FromArgb(100, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B))
            Dim ii As Long = Canvas.MinY - Canvas.Y
            GraphicsMain.FillRectangle(a2, 0, BmpMain.Height - ii, BmpMain.Width, ii)
            ii = -ii + BmpMain.Height + 4

            Dim c As New Pen(Color.FromArgb(180, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 4, BmpMain.Width, ii - 4)
            c = New Pen(Color.FromArgb(120, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 3, BmpMain.Width, ii - 3)
            c = New Pen(Color.FromArgb(60, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 2, BmpMain.Width, ii - 2)
            c = New Pen(Color.FromArgb(25, 0, 0, 0))
            GraphicsMain.DrawLine(c, 0, ii - 1, BmpMain.Width, ii - 1)
        End If
    End Sub
    Public Sub Draw_Picturies()
        If BmpMain.Height > 5 Then
            GraphicsMain.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            If BGColor.R < 127 Then
                TextColor.Color = Color.White
                SelRectColor.Color = Color.FromArgb(110, 0, 0, 0)
                SelRectFrameColor.Color = Color.FromArgb(70, 255, 255, 255)
            Else
                TextColor.Color = Color.Black
                SelRectColor.Color = Color.FromArgb(110, 255, 255, 255)
                SelRectFrameColor.Color = Color.FromArgb(70, 0, 0, 0)
            End If

            GraphicsMain.Clear(BGColor)

            Draw_FreeAreas()
            DrawSelections()

            Dim I As Long = SelectedImageIndex
            Dim x, y As Long
            Dim II As Long
            For II = NImages To 1 Step -1
                If II <> SelectedImageIndex And Image(II).Y + Canvas.Y > -Wire.Y And Image(II).Y + Canvas.Y < Me.Height Then
                    If Image(II).Loaded Then
                        DrawPicture(II)
                        If Wire.Y > 40 And InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(Image(II).FileName)) + " ") Then
                            If ShowImagesName Then DrawUnderImageText(II)
                        Else
                            DrawSimpleText(II)
                        End If
                    Else
                        If Canvas.Columns <> 0 Then
                            x = Image(II).X
                            y = Image(II).Y + Canvas.Y + (Wire.Y - font_filename.Size) / 2
                            GraphicsMain.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                            GraphicsMain.DrawString(Image(II).Name, font_filename, TextColor, x, y)
                            GraphicsMain.DrawString("(no thumb)", font_filename, b4, x + GraphicsMain.MeasureString(Image(II).Name, font_filename).Width, y)
                        End If
                    End If
                End If
            Next

            If Me.Focused And I > 0 And I <= NImages Then
                Dim sm As Long = Wire.dY / 2

                x = Image(I).X
                y = Image(I).Y + Canvas.Y

                SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 120, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B)
                SelRectFrameColor.Color = Color.FromArgb(Image(I).Transparency * 120, SelRectFrameColor.Color.R, SelRectFrameColor.Color.G, SelRectFrameColor.Color.B)
                If (IsMouseDown And SelectedImageIndex = ChosenObj) Or IsEnterDown Then
                    SelRectColor.Color = Color.FromArgb(SelRectColor.Color.A * 2.0, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B)
                    SelRectFrameColor.Color = Color.FromArgb(SelRectFrameColor.Color.A * 2.0, SelRectFrameColor.Color.R, SelRectFrameColor.Color.G, SelRectFrameColor.Color.B)
                End If
                GraphicsMain.FillRectangle(SelRectColor, x - sm, y - sm, Image(I).WidthWithText + sm * 2, Wire.Y + sm * 2)
                GraphicsMain.DrawRectangle(SelRectFrameColor, x - sm - 1, y - sm - 1, Image(I).WidthWithText + sm * 2 + 1, Wire.Y + sm * 2 + 1)
            End If

            If SelectedImageIndex <> 0 Then
                II = SelectedImageIndex
                If Image(II).Loaded Then
                    DrawPicture(II)
                    If Wire.Y > 40 And InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(Image(II).FileName)) + " ") Then
                        If ShowImagesName Then DrawUnderImageText(II)
                    Else
                        DrawSimpleText(II)
                    End If
            Else
                If Canvas.Columns <> 0 Then
                    x = Image(II).X
                    y = Image(II).Y + Canvas.Y + (Wire.Y - font_filename.Size) / 2
                    GraphicsMain.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                    GraphicsMain.DrawString(Image(II).Name, font_filename, TextColor, x, y)
                    GraphicsMain.DrawString("(no thumb)", font_filename, b4, x + GraphicsMain.MeasureString(Image(II).Name, font_filename).Width, y)
                End If
            End If
            End If
            DrawFrame(GraphicsMain, BmpMain)
        End If
    End Sub

    Private Sub CleanScreen()
        GraphicsMain.Clear(BGColor)
    End Sub
    Private Sub DrawSimpleText(ByVal ii As Integer)
        Dim x As Long = Image(ii).X + Thumbnail(ii).Width + 5
        Dim y As Long = Image(ii).Y + Canvas.Y + Wire.Y / 2 - 6

        GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y)
    End Sub
    Private Sub DrawUnderImageText(ByVal ii As Integer)
        Dim x As Long = Image(ii).X + (Wire.X - GraphicsMain.MeasureString(Trim(Image(ii).Name), font_filename).Width) / 2
        Dim y As Long = Image(ii).Y + Wire.Y / 2 + Thumbnail(ii).Height / 2 + Canvas.Y - 3

        GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y)
    End Sub
    Private Sub DrawSelections()
        Dim x, y As Long
        Dim sm As Long = Wire.dY / 2
        For i As Long = 1 To NImages
            If Image(i).Selected = True And Image(i).Y + Canvas.Y > -Wire.Y And Image(i).Y + Canvas.Y < Me.Height Then
                x = Image(i).X
                y = Image(i).Y + Canvas.Y

                SelRectFrameColor.Color = Color.FromArgb(255 * Image(i).Transparency, SelRectFrameColor.Color.R, SelRectFrameColor.Color.G, SelRectFrameColor.Color.B)
                GraphicsMain.DrawRectangle(SelRectFrameColor, x - sm - 1, y - sm - 1, Image(i).WidthWithText + Wire.dY + 1, Wire.Y + Wire.dY + 1)
            End If
        Next
        For i As Long = 1 To NImages
            If Image(i).Selected = True And Image(i).Y + Canvas.Y > -Wire.Y And Image(i).Y + Canvas.Y < Me.Height Then
                x = Image(i).X
                y = Image(i).Y + Canvas.Y

                Dim b1 As SolidBrush = Brushes.GreenYellow
                b1.Color = Color.FromArgb(255 * Image(i).Transparency, 150, 255, 0)
                GraphicsMain.FillRectangle(b1, x - sm, y - sm, Image(i).WidthWithText + Wire.dY, Wire.Y + Wire.dY)
            End If
        Next
    End Sub
    Private Sub DrawPicture(ByVal I As Integer)
        Dim p As Point
        Dim w As Long = Wire.Border

        With Image(I)
            p.Y = Math.Round(.Y + Canvas.Y) + (Wire.Y - Thumbnail(I).Height) / 2
            p.X = Math.Round(.X)

            If FileTags.Tags(.InTagsIngex).Type = "image" Then
                If DrawShadow Then
                    Dim width, height As Long
                    Dim smx As Short = 0
                    Dim smy As Short = 0
                    width = Thumbnail(I).Width + w * 2 + 1
                    height = Thumbnail(I).Height + w * 2 + 1
                    Dim a1 As New Pen(Color.FromArgb(50 * .Transparency, 0, 0, 0))
                    smx = 1 : smy = 0
                    GraphicsMain.DrawRectangle(a1, p.X - w - 1 + smx, p.Y - w - 1 + smy, width, height)
                    smx = 0 : smy = -1
                    GraphicsMain.DrawRectangle(a1, p.X - w - 1 + smx, p.Y - w - 1 + smy, width, height)
                    smx = 0 : smy = 1
                    GraphicsMain.DrawRectangle(a1, p.X - w - 1 + smx, p.Y - w - 1 + smy, width, height)
                    smx = -1 : smy = 0
                    GraphicsMain.DrawRectangle(a1, p.X - w - 1 + smx, p.Y - w - 1 + smy, width, height)
                End If
                While w > 0
                    Dim pen1 As New Pen(Color.FromArgb(255 * .Transparency, 255, 255, 255))
                    Dim r As New Rectangle(p.X - w, p.Y - w, Thumbnail(I).Width + w * 2 - 1, Thumbnail(I).Height + w * 2 - 1)
                    GraphicsMain.DrawRectangle(pen1, r)
                    w = w - 1
                End While
            End If

            If .Transparency < 1 Then
                Dim att As New Drawing.Imaging.ImageAttributes()
                Dim cm As Drawing.Imaging.ColorMatrix = New Drawing.Imaging.ColorMatrix(New Single()() _
                           {New Single() {1, 0, 0, 0, 0}, _
                            New Single() {0, 1, 0, 0, 0}, _
                            New Single() {0, 0, 1, 0, 0}, _
                            New Single() {0, 0, 0, .Transparency, 0}, _
                            New Single() {0, 0, 0, 0, 1}})
                att.SetColorMatrix(cm)
                Dim r1 As New Rectangle(p.X, p.Y, Thumbnail(I).Width, Thumbnail(I).Height)
                GraphicsMain.DrawImage(Thumbnail(I), r1, 0, 0, Thumbnail(I).Width, Thumbnail(I).Height, GraphicsUnit.Pixel, att)
            Else
                GraphicsMain.DrawImageUnscaled(Thumbnail(I), p)
            End If
        End With
    End Sub

    Sub CorrectSelectedImageIndex()
        If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
        If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
    End Sub
    Dim InputLine As String = " "
    Private Sub ucImagesBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        InputLine = InputLine + Chr(e.KeyCode)
        If InputLine.Length > 90 Then InputLine = Mid(InputLine, 30)
        If InStrRev(InputLine, " ") < InputLine.Length Then
            Dim str As String = LCase(Mid(InputLine, InStrRev(InputLine, " ") + 1))
            If Mid(str, str.Length, 1) = Chr(187) Then
                str = Mid(str, 1, str.Length - 1)
                Dim NF As New frmTempArea
                NF.Show()
                NF.ImagesBox.SetWire(Wire.X, Wire.Y, Wire.dX, Wire.dY)
                NF.ImagesBox.Wire.Border = Wire.Border
                NF.ImagesBox.BGColor = BGColor
                NF.ImagesBox.DrawShadow = False
                NF.ImagesBox.ShowImagesName = True
                NF.Left = 10 + Me.Parent.Left '+ Me.Left 'btnTempArea.Left - NF.Width / 2
                NF.Top = 10 + Me.Parent.Top '+ Me.Top 'btnTempArea.Top - 10
                NF.ImagesBox.StopLoading = True
                NF.ImagesBox.ClearImages()

                For i As Long = 1 To NImages
                    If InStr(LCase(Image(i).Name), str) > 0 Then NF.ImagesBox.AddImage(Image(i).FileName)
                Next

                'NF.ImagesBox.OrderImages()
                NF.ImagesBox.SetImagesLocation()
                NF.ImagesBox.MakeAllThumbnails()

                NF.Select()
                NF.ImagesBox.Select()
            Else
                For i As Long = 1 To NImages
                    If InStr(LCase(Image(i).Name), str) > 0 Then SelectedImageIndex = i : Exit For
                Next
            End If
        End If
        Select Case e.KeyCode
            Case Keys.Down
                SelectedImageIndex += Canvas.Columns : CorrectSelectedImageIndex()
            Case Keys.Up
                If SelectedImageIndex <= Canvas.Columns Then
                    Dim a As Boolean = True
                    RaiseEvent SendFocusToTheTop(a)
                    If a = False Then SelectedImageIndex -= Canvas.Columns : CorrectSelectedImageIndex()
                Else
                    SelectedImageIndex -= Canvas.Columns : CorrectSelectedImageIndex()
                End If
            Case Keys.Left
                SelectedImageIndex -= 1 : CorrectSelectedImageIndex()
            Case Keys.Right
                SelectedImageIndex += 1 : CorrectSelectedImageIndex()
            Case Keys.PageDown
                Canvas.V += 20
            Case Keys.PageUp
                Canvas.V -= 20
            Case Keys.Escape
                StopLoading = True
                'bwLoadImages.CancelAsync()
            Case Keys.Back
                RaiseEvent BackSpaceKey()
            Case Keys.Enter
                IsEnterDown = True
                If SelectedImageIndex >= 1 And SelectedImageIndex <= NImages Then
                    If InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(Image(SelectedImageIndex).FileName)) + " ") Then
                        Dim a As New frmShowPhoto2
                        Dim FN(NImages) As String
                        For j As Long = 1 To NImages
                            FN(j) = Image(j).FileName
                        Next
                        If Image(SelectedImageIndex).Loaded = True Then
                            a.InitFullScreened(Thumbnail(SelectedImageIndex), SelectedImageIndex, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
                        Else
                            a.InitFullScreened(bmp_error, SelectedImageIndex, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
                        End If
                        a.Show()
                    Else
                        Process.Start(Image(SelectedImageIndex).FileName, AppWinStyle.NormalFocus)
                    End If
                    FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).LaunchingTimes += 1
                End If
        End Select
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.D0
                    FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 0
                    Image(SelectedImageIndex).ReLoaded = False
                    IsNotEverithingLoaded = True
                Case Keys.D1
                    FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 1
                    Image(SelectedImageIndex).ReLoaded = False
                    IsNotEverithingLoaded = True
                Case Keys.D2
                    FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 2
                    Image(SelectedImageIndex).ReLoaded = False
                    IsNotEverithingLoaded = True
                Case Keys.D3
                    FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).Rating = 3
                    Image(SelectedImageIndex).ReLoaded = False
                    IsNotEverithingLoaded = True
                Case Keys.R
                    Dim str As String
                    Dim old_str As String = Image(SelectedImageIndex).FileName
                    str = InputBox("new name", "name changing", IO.Path.GetFileName(old_str))
                    Try
                        Rename(old_str, Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str)
                        Image(SelectedImageIndex).FileName = Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str
                        FileTags.Files(Image(SelectedImageIndex).InTagsIngex) = Image(SelectedImageIndex).FileName
                    Catch ex As Exception
                        MsgBox("Не удаётся переименовать файл")
                    End Try
                Case Keys.T
                    Dim str As String
                    For i As Long = 1 To NImages
                        Dim old_str As String = Image(i).FileName
                        str = IO.Path.GetFileName(old_str).Replace("_", " ")

                        Dim a As String
                        a = Mid(str, 1, 1)
                        While InStr("qwertyuioplkjhgfdsazxcvbnm", LCase(a)) = 0
                            str = Mid(str, 2)
                            a = Mid(str, 1, 1)
                        End While

                        If InStr(str, " - ") = 0 Then
                            str = str.Replace("-", " - ")
                        End If

                        Try
                            Rename(old_str, Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str)
                            Image(i).FileName = Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str
                            FileTags.Files(Image(i).InTagsIngex) = Image(i).FileName
                            Image(i).Name = str
                        Catch ex As Exception
                            MsgBox("Не удаётся переименовать файл")
                        End Try
                    Next
                Case Keys.S
                    If SelectedImageIndex >= 1 And SelectedImageIndex <= NImages Then
                        Image(SelectedImageIndex).Selected = Not (Image(SelectedImageIndex).Selected)
                    End If
                Case Keys.A
                    For i As Long = 1 To NImages
                        Image(i).Selected = True
                    Next
                Case Keys.D
                    For i As Long = 1 To NImages
                        Image(i).Selected = False
                    Next
            End Select
        End If
        NextFrameInAnyWay()
        Canvas.DestY = -Image(SelectedImageIndex).Y + (-Wire.Y + Me.Height) / 2 ' - Int((SelectedImageIndex - 1) / Canvas.Columns - 1) * (Wire.Y + Wire.dY)
        'MsgBox(Canvas.DestY)
        If Canvas.DestY > 0 Then Canvas.DestY = 0
        If Canvas.DestY < Canvas.MinY Then Canvas.DestY = Canvas.MinY
        Canvas.Animate = True
    End Sub
    Private Sub ucImagesBox_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        If e.KeyCode <> Keys.Tab Then e.IsInputKey() = True
    End Sub

    Private Sub ucImagesBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DoResizeEvent = True

        BmpMain = New Bitmap(Me.Width, Me.Height)
        GraphicsMain = Graphics.FromImage(BmpMain)
        InitButtons()
        NextFrameInAnyWay()

        picMain.Left = 0
        picMain.Top = 0
    End Sub

    Public DoResizeEvent As Boolean = False

    Private Sub ucImagesBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If DoResizeEvent Then
            BmpMain = New Bitmap(Me.Width, Me.Height)
            GraphicsMain = Graphics.FromImage(BmpMain)

            RecalculateCanvasParameters()

            Draw_Picturies()
            NextFrame()
            picMain.Image = BmpMain
            picMain.Refresh()
            'Me.BackgroundImage = BmpMain
            Me.Refresh()
        End If
    End Sub
    Public Sub NextFrameInAnyWay()
        'If Not IsMouseDown Then
        'If SurfaceFly Then
        'Canvas.V = 0
        'Canvas.Y = Canvas.Y + (SurfaceDest - Canvas.Y) / 6
        'If SurfaceDest = Int(Canvas.Y) Then Canvas.Y = Int(Canvas.Y) : SurfaceFly = False
        'Else
        Canvas.Y += Canvas.V
        If Canvas.Y > 0 Then
            Canvas.Y -= Canvas.Y / 8
            Canvas.V = Canvas.V * 0.7
        Else
            If Canvas.Y < Canvas.MinY Then
                Canvas.Y -= (Canvas.Y - Canvas.MinY) / 8
                Canvas.V = Canvas.V * 0.7
            Else
                Canvas.V = Canvas.V * 0.9
            End If
        End If
        If Math.Abs(Canvas.V) < 0.2 Then Canvas.V = 0
        If Math.Abs(Canvas.Y) < 0.2 Then Canvas.Y = 0
        'End If
        'End If

        If IsAnimatedImages Then
            IsAnimatedImages = False
            For I As Long = 1 To NImages
                If Image(I).Animate = True Then
                    If Math.Abs(Image(I).X - Image(I).DestX) < 0.4 And Math.Abs(Image(I).Y - Image(I).DestY) < 0.4 Then
                        Image(I).X = Math.Round(Image(I).DestX)
                        Image(I).Y = Math.Round(Image(I).DestY)
                        'Image(I).Animate = False
                    Else
                        Image(I).X += (Image(I).DestX - Image(I).X) * 0.22
                        Image(I).Y += (Image(I).DestY - Image(I).Y) * 0.22
                        IsAnimatedImages = True
                    End If
                    If Image(I).Transparency < 1 Then
                        Image(I).Transparency += 0.1 '05
                        If Image(I).Transparency > 1 Then Image(I).Transparency = 1
                        IsAnimatedImages = True
                    Else
                        Image(I).Transparency = 1
                        'Image(I).Animate = False
                    End If
                End If
            Next
        End If

        Dim fnt As New Font("arial", 10, FontStyle.Regular, GraphicsUnit.World)

        Draw_Picturies()
        If bwLoadImages.IsBusy Then
            'Try
            GraphicsMain.DrawString(Str, fnt, Brushes.White, 20, BmpMain.Height - 35)
            GraphicsMain.DrawImageUnscaled(GenerateStatusBar(300, LoadingProgress), 2, BmpMain.Height - 19)
            'Catch ex As Exception
            'End Try
        End If

        If Buttons.Length > 4 Then
            If Canvas.MinY <> 0 Then
                Buttons(2).Y = (Canvas.Y / Canvas.MinY) * (BmpMain.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) + 3 + Buttons(3).Image.Height
                Buttons(2).X = BmpMain.Width - Buttons(2).Image.Width - 2

                Buttons(3).X = Buttons(2).X : Buttons(4).X = Buttons(2).X

                Buttons(3).Y = 2 : Buttons(4).Y = BmpMain.Height - Buttons(4).Image.Height - 2

                Buttons(2).Visible = True
                Buttons(3).Visible = True
                Buttons(4).Visible = True
            Else
                Buttons(2).Visible = False
                Buttons(3).Visible = False
                Buttons(4).Visible = False
            End If
            Dim a As ButtonStruct
            For I As Long = 0 To Buttons.Length - 1
                a = Buttons(I)
                If a.Visible = True Then GraphicsMain.DrawImageUnscaled(a.Image, a.X, a.Y)
            Next
        End If
        If ShowFramesPerSecond Then
            counter += 1
            If counter >= 5 Then
                counter = 0
                time2 = System.DateTimeOffset.Now
                time_delta = time2 - time1
                time1 = time2
            End If
            GraphicsMain.DrawString((Math.Round(5 / time_delta.TotalSeconds)).ToString + " fps", fnt, Brushes.BlueViolet, BmpMain.Width - 100, BmpMain.Height - 15)
        End If

        If Me.Focused Then
            GraphicsMain.DrawString("focused", fnt, Brushes.White, BmpMain.Width - 150, BmpMain.Height - 15)
        End If
    End Sub

    Public Sub NextFrame()
        'If ChosenObj > 0 And moved Then
        '    If Image(ChosenObj).Transparency > 0.6 Then Image(ChosenObj).Transparency -= 0.05
        'End If
        'If Canvas.Y < 0 And IsMouseDown And ChosenObj > 0 Then
        '    If Image(ChosenObj).Y + Canvas.Y < 15 Then
        '        Dim d As Long = 15 - (Image(ChosenObj).Y + Canvas.Y)
        '        If d > 25 Then d = 25
        '        If d < 0 Then d = 0
        '        If Canvas.Y > -d Then d = -Canvas.Y
        '        Canvas.Y += d : Image(ChosenObj).Y -= d
        '    End If
        'End If
        'If Canvas.Y > Canvas.MinY And IsMouseDown And ChosenObj > 0 Then
        '    If Image(ChosenObj).Loaded Then
        '        If Image(ChosenObj).Y + Thumbnail(ChosenObj).Height + Canvas.Y > Me.Height - 15 Then
        '            Dim d As Long = (Image(ChosenObj).Y + Canvas.Y + Thumbnail(ChosenObj).Height) - (Me.Height - 15)
        '            If d > 25 Then d = 25
        '            If d < 0 Then d = 0
        '            If Canvas.Y - Canvas.MinY < d Then d = Canvas.Y - Canvas.MinY
        '            Canvas.Y -= d : Image(ChosenObj).Y += d
        '        End If
        '    End If
        'End If
        If Canvas.Animate Then
            Canvas.V = 0
            Canvas.Y = Canvas.Y + (Canvas.DestY - Canvas.Y) / 6
            If Canvas.DestY = Math.Round(Canvas.Y) Then Canvas.Y = Canvas.DestY : Canvas.Animate = False
        Else
            If Not (IsMouseDown And (ChosenObj = 0 Or ChosenObj = -3)) Then
                Canvas.Y += Canvas.V
                If Canvas.Y > 0 Then
                    Canvas.Y -= Canvas.Y / 8
                    Canvas.V = Canvas.V * 0.7
                Else
                    If Canvas.Y < Canvas.MinY Then
                        Canvas.Y -= (Canvas.Y - Canvas.MinY) / 8
                        If (-Canvas.Y + Canvas.MinY) < 0.4 Then Canvas.Y = Canvas.MinY
                        Canvas.V = Canvas.V * 0.7
                    Else
                        Canvas.V = Canvas.V * 0.9
                    End If
                End If
                If Math.Abs(Canvas.V) < 0.2 Then Canvas.V = 0
                If Math.Abs(Canvas.Y) < 0.2 Then Canvas.Y = 0
            End If
        End If

        Dim proc As Double = 0.22
        If IsAnimatedImages Then
            IsAnimatedImages = False
            For I As Long = 1 To NImages
                If Image(I).Animate = True Then
                    With Image(I)
                        If Math.Abs(.X - .DestX) < 0.4 And Math.Abs(.Y - .DestY) < 0.4 Then
                            .X = Math.Round(.DestX)
                            .Y = Math.Round(.DestY)
                            If Math.Abs(.Transparency - .DestTransparency) < 0.05 Then .Animate = False
                        Else
                            .X += (.DestX - .X) * proc
                            .Y += (.DestY - .Y) * proc
                            IsAnimatedImages = True
                        End If
                        If Math.Abs(.Transparency - .DestTransparency) < 0.05 Then
                            .Transparency = .DestTransparency
                        Else
                            .Transparency += (.DestTransparency - .Transparency) * proc
                            If .Transparency > 1 Then .Transparency = 1
                            IsAnimatedImages = True
                        End If
                    End With
                End If
            Next
        End If

        Dim fnt As New Font("arial", 10, FontStyle.Regular, GraphicsUnit.World)

        If (IsSmthDragging Or IsMouseDown Or Canvas.V <> 0 Or IsAnimatedImages = True Or Canvas.Y > 0 Or Canvas.Y < Canvas.MinY Or Canvas.Animate) Then
            Draw_Picturies()
            If CurrentLoadingImgIndex < NImages And StopLoading <> True Then
                GraphicsMain.DrawString(CurrentLoadingImgIndex.ToString + " | " + NImages.ToString, fnt, Brushes.White, 20, BmpMain.Height - 35)
                GraphicsMain.DrawImageUnscaled(GenerateStatusBar(300, CurrentLoadingImgIndex / NImages), 2, BmpMain.Height - 19)
            End If

            If IsSmthDragging Then
                Dim p As Point = MousePosition
                p.X = p.X - Me.Parent.Left - Me.Left - 6
                p.Y = p.Y - Me.Parent.Top - Me.Top - 12
                GraphicsMain.DrawString("file", fnt, Brushes.White, p)
            End If

            If ShowFramesPerSecond Then
                counter += 1
                If counter >= 5 Then
                    counter = 0

                    time2 = System.DateTimeOffset.Now
                    time_delta = time2 - time1
                    time1 = time2
                End If
                Try
                    GraphicsMain.DrawString((Math.Round(1 / time_delta.TotalSeconds * 5000) / 1000).ToString + " fps", fnt, Brushes.White, BmpMain.Width - 100, BmpMain.Height - 15)
                Catch ex As Exception
                End Try
            End If

            If Me.Focused Then
                GraphicsMain.DrawString("focused", fnt, Brushes.LightGray, BmpMain.Width - 150, BmpMain.Height - 15)
            End If

            If Canvas.MinY <> 0 Then
                Buttons(2).Y = (Canvas.Y / Canvas.MinY) * (BmpMain.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) + 3 + Buttons(3).Image.Height
                Buttons(2).X = BmpMain.Width - Buttons(2).Image.Width - 2

                Buttons(3).X = Buttons(2).X : Buttons(4).X = Buttons(2).X

                Buttons(3).Y = 2 : Buttons(4).Y = BmpMain.Height - Buttons(4).Image.Height - 2

                Buttons(2).Visible = True
                Buttons(3).Visible = True
                Buttons(4).Visible = True
            Else
                Buttons(2).Visible = False
                Buttons(3).Visible = False
                Buttons(4).Visible = False
            End If

            Dim a As ButtonStruct
            For I As Long = 0 To Buttons.Length - 1
                a = Buttons(I)
                If a.Visible = True Then
                    'Try
                    GraphicsMain.DrawImageUnscaled(a.Image, a.X, a.Y)
                    'Catch ex As Exception
                    'End Try
                End If
            Next

            'If MDTime > 0 Then
            '    MDTime += 1
            '    If MDTime >= 60 Then
            '        MDTime = 0
            '        Process.Start("C:\Program Files (x86)\Adobe\Adobe Photoshop CS4\Photoshop.exe", Image(ChosenObj).FileName)
            '    End If

            '    'GraphicsMain.DrawString(MDTime.ToString, fnt, Brushes.LightGray, Cl.X, Cl.Y)
            '    '                Dim p As Long = Math.Sqrt(Math.Abs((100 - MDTime))) * 10
            '    Dim p As Long = (Math.Abs((10 - MDTime / 10)) ^ 4) / 10

            '    Dim clr As Long = MDTime / 2

            '    Dim c As New Pen(Color.FromArgb(MDTime, 255, 255, 255))
            '    Dim r As New Rectangle(Cl.X - p / 2, Cl.Y - p / 2, p, p)
            '    GraphicsMain.DrawEllipse(c, r)

            '    c = New Pen(Color.FromArgb(clr, 255, 255, 255)) : p = p - 2
            '    r = New Rectangle(Cl.X - p / 2, Cl.Y - p / 2, p, p)
            '    GraphicsMain.DrawEllipse(c, r)

            '    c = New Pen(Color.FromArgb(clr, 255, 255, 255)) : p = p + 4
            '    r = New Rectangle(Cl.X - p / 2, Cl.Y - p / 2, p, p)
            '    GraphicsMain.DrawEllipse(c, r)
            'End If
        End If
    End Sub

    Private Sub tmrAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAnimation.Tick
        If StopLoading = True Then
            CurrentLoadingImgIndex = NImages + 1
        Else
            If IsNotEverithingLoaded Then
                If Not bwLoadOne.IsBusy Then
                    If CurrentLoadingImgIndex >= 1 And CurrentLoadingImgIndex <= NImages Then
                        If Image(CurrentLoadingImgIndex).Loading = True Then
                            Dim I As Long = CurrentLoadingImgIndex

                            Image(I).InTagsIngex = FileTags.FindByFileName(Image(I).FileName)
                            If Image(I).InTagsIngex = 0 Then Image(I).InTagsIngex = FileTags.Add(Image(I).FileName)

                            For d As Long = 1 To FileTags.Tags(Image(I).InTagsIngex).Rating
                                Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                                    graf.DrawImage(bmp_star, 44 - d * 13, 32 - 14 + 1)
                                End Using
                            Next

                            Thumbnail(I) = LoadedThumbnail

                            Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width
                            While Image(I).WidthWithText > Wire.X - Thumbnail(I).Width - 5
                                Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                                Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width
                            End While
                            Image(I).WidthWithText += Thumbnail(I).Width + 5

                            If Image(I).Loaded = False Then
                                If Wire.Y >= 40 Then Image(I).Y = Image(I).Y - Thumbnail(I).Height : Image(I).Transparency = 0 Else Image(I).X = Image(I).X - Thumbnail(I).Width - 5 : Image(I).Transparency = 0
                                Image(I).Transparency = 0
                            End If
                            Image(I).DestTransparency = 1
                            Image(I).Animate = True

                            Image(I).Loaded = True
                            Image(I).Loading = False
                            Image(I).ReLoaded = True

                            IsAnimatedImages = True
                        End If
                    End If

                    Dim f As Boolean = False
                    For i As Long = 1 To NImages
                        If Image(i).Loaded = False Or Image(i).ReLoaded = False Then
                            CurrentLoadingImgIndex = i
                            CurrentLoadingFileName = Image(i).FileName
                            Image(i).Loading = True
                            Image(i).DestTransparency = 0
                            Image(i).Animate = True
                            bwLoadOne.RunWorkerAsync()
                            f = True
                            Exit For
                        End If
                    Next
                    If f = False Then
                        IsNotEverithingLoaded = False
                        'SaveThumbsInfo()
                        'FileTags.Save()
                    End If
                End If
            End If
        End If

        NextFrame()

        picMain.Size = Me.Size
        picMain.Image = BmpMain
        picMain.Refresh()
    End Sub
    Dim time1, time2 As DateTimeOffset, time_delta As TimeSpan
    Dim ShowFramesPerSecond As Boolean = True, counter As Short




    Private Sub ucImagesBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        If IsMouseDown And ChosenObj > 0 And ChosenObj <= NImages Then
            StartDragDrop(ChosenObj)
        End If
        NextFrameInAnyWay()
    End Sub
    Private Sub ucImagesBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        NextFrameInAnyWay()
    End Sub

    Sub StartDragDrop(ByVal i As Integer)
        'picForDrag.Image = Thumbnail(i)
        IsMouseDown = False
        'ChosenObj = 0
        If Image(i).Selected = True Then
            Dim k As Long
            Dim str(DraggingFilesList.Length - 2) As String

            For j As Long = 1 To DraggingFilesList.Length - 1
                k = DraggingFilesList(j)
                'SetImageLocation(k)
                'Image(k).DestTransparency = 1
                str(j - 1) = Image(k).FileName
            Next
            IsAnimatedImages = True

            Dim m As New DataObject(DataFormats.FileDrop, str)
            picForDrag.DoDragDrop(m, DragDropEffects.Copy)
        Else
            Dim str(0) As String
            str(0) = Image(i).FileName
            'SetImageLocation(i)
            'Image(i).DestTransparency = 1

            Dim m As New DataObject(DataFormats.FileDrop, str)
            picForDrag.DoDragDrop(m, DragDropEffects.Copy)
        End If
    End Sub
    Private Sub bwLoadImages_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadImages.RunWorkerCompleted
        NextFrameInAnyWay()
    End Sub


    Private Sub ucImagesBox_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If Canvas.Animate = False And Canvas.DestY <> Canvas.Y Then Canvas.DestY = Canvas.Y
        Canvas.DestY = Canvas.DestY + e.Delta
        Canvas.Y = Canvas.DestY - 0.7 'или без неё
        Canvas.Animate = True
        If Canvas.DestY > 0 Then
            Dim f As Boolean : RaiseEvent SendFocusToTheTop(f) : If f = False Then Canvas.Y = 0 : Canvas.DestY = 0 : NextFrameInAnyWay()
        End If
    End Sub

    Private Sub ucImagesBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        IsEnterDown = False
        NextFrameInAnyWay()
    End Sub

    Private Sub picMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMain.LostFocus
        If IsMouseDown And ChosenObj > 0 And ChosenObj <= NImages Then
            StartDragDrop(ChosenObj)
        End If
    End Sub

    Private Sub picMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picMain.Click

    End Sub
End Class
