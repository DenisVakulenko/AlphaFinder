Public Class ucImagesBox
    Public Sub New()
        InitializeComponent()
        Wire.X = 200
        Wire.Y = 16
        Wire.dX = 10
        Wire.dY = 5
    End Sub
    Event BackSpaceKey()
    Event FillScreenMe()
    Event SendFocusToTheTop(ByRef Done As Boolean)
    Event ChangeDir(ByRef Path As String)
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
        Dim Width As Long

        Dim MinY As Long
        Dim MinX As Long

        Dim Y As Double
        Dim X As Double
        Dim DestX As Long
        Dim DestY As Long

        Dim V As Double
        Dim VX As Double

        Dim Animate As Boolean
        Dim Plus As Short
    End Structure
    Public Structure ImageStruct
        Dim X, Y As Double, DestX, DestY As Integer
        Dim Width, Height As Double, DestWidth, DestHeight As Short

        Dim WidthWithText As Short
        'Dim Height As Short
        Dim Animate As Boolean

        Dim Singer As String
        Dim Name As String
        Dim FileName As String

        Dim toshowName As String
        Dim toshowArtist As String

        Dim Loaded As Boolean
        Dim Loading As Boolean
        Dim ReLoaded As Boolean

        Dim Transparency As Double
        Dim DestTransparency As Double

        Dim Selected As Boolean
        Dim WillBeSelected As Boolean
        Dim WillBeUnSelected As Boolean
        Dim InTagsIngex As Long

        Dim Visible As Boolean

        Dim Type As String '  "file" "image" "song" "folder" "photo album" "song album"
    End Structure
#End Region
    Public SelectedImageIndex As Long = 0
    Public ShowImagesName As Boolean = False
    'Public View As String ' "big" "ave" "min"
    Public Arrangement As String ' "gorizontal" "vertical" 
    Private VScrolling As Boolean ' true - vertical; false - gorizontal
    Dim IsNotEverithingLoaded As Boolean = False
    Dim IsEnterDown As Boolean
    Dim DraggingFilesList() As Long, NDraggingFiles As Long, IndexOfChosenInDraggingFiles As Long
    Dim URLs35photo(40) As String
    Public Path As String = "   "
    Public Sorting As String = "by name"
    Public Animation As Boolean = True
    Public FlyMode As Boolean = False
    Dim startX, startY As Long
#Region "BUTTONS"
    Dim Buttons(1) As ButtonStruct
    Public Sub InitButtons()
        Try
            ReDim Buttons(12)
            Buttons(6).X = 2
            Buttons(6).Y = 2
            Buttons(6).Image = New Bitmap(Application.StartupPath + "\order.png")
            Buttons(6).Visible = False

            Dim k As Long = 28
            Buttons(0).X = 2
            Buttons(0).Y = 2
            Buttons(0).Image = New Bitmap(Application.StartupPath + "\sorting\name.png")
            Buttons(0).Visible = False
            Buttons(1).X = 2
            Buttons(1).Y = k * 1 + 4
            Buttons(1).Image = New Bitmap(Application.StartupPath + "\sorting\rating.png")
            Buttons(1).Visible = False
            Buttons(7).X = 2
            Buttons(7).Y = k * 2 + 4
            Buttons(7).Image = New Bitmap(Application.StartupPath + "\sorting\singer.png")
            Buttons(7).Visible = False
            Buttons(8).X = 2
            Buttons(8).Y = k * 3 + 4
            Buttons(8).Image = New Bitmap(Application.StartupPath + "\sorting\pop.png")
            Buttons(8).Visible = False
            Buttons(12).X = 2
            Buttons(12).Y = k * 4 + 4
            Buttons(12).Image = New Bitmap(Application.StartupPath + "\sorting\rand.png")
            Buttons(12).Visible = False

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
            Buttons(5).Image = New Bitmap(Application.StartupPath + "\sorting\main.png")
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
    Private Sub ShowSortingVariants()
        Buttons(5).Visible = False

        Buttons(0).Visible = True
        Buttons(1).Visible = True
        Buttons(7).Visible = True
        Buttons(8).Visible = True
        Buttons(12).Visible = True
    End Sub
    Private Sub HideSortingVariants()
        Buttons(5).Visible = True

        Buttons(0).Visible = False
        Buttons(1).Visible = False
        Buttons(7).Visible = False
        Buttons(8).Visible = False
        Buttons(12).Visible = False
    End Sub
#End Region
#Region "--------------------------|  UI IMAGES             |-------------------------- GOOD"
    'Dim BmpScrollThing, BmpScrollThingMD, BmpScrollUp, BmpScrollDown As Bitmap
    Dim bmp_error, bmp_star, bmp_muz_ico, bmp_muz_ico_16, bmp_dir_min, bmp_dir_min_32 As Bitmap
    Public Sub LoadUIImages()
        bmp_error = New Bitmap(Application.StartupPath + "\err.png")
        bmp_star = New Bitmap(Application.StartupPath + "\star2.png")
        bmp_muz_ico = New Bitmap(Application.StartupPath + "\muz.png")
        bmp_dir_min = New Bitmap(Application.StartupPath + "\dir_min.bmp")
        bmp_dir_min_32 = New Bitmap(Application.StartupPath + "\folder5.png")
        bmp_muz_ico_16 = New Bitmap(50, 16) 'Application.StartupPath + "\muz16.png")
        Using g As Graphics = Graphics.FromImage(bmp_muz_ico_16)
            g.DrawLine(Pens.Gray, 49, 0, 49, 16)
            g.DrawLine(Pens.DarkGray, 0, 0, 0, 16)
        End Using
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
            If Arrangement = "gorizontal" Then
                Canvas.Height = (Math.Truncate((NImages - 1) / Canvas.Columns) + 1) * (Wire.Y + Wire.dY) + Wire.dY
            Else
                Canvas.Height = Me.Height
            End If
        Else
            Canvas.Height = 0
            'MsgBox("really!")
        End If
    End Sub
    Private Sub CalculateCanvasWidth()
        If Canvas.Columns > 0 Then
            Canvas.Width = (Math.Truncate((NImages - 1) / Canvas.Columns) + 1) * (Wire.X + Wire.dX) + Wire.dX
        Else
            Canvas.Width = 0
            'MsgBox("really!")
        End If
    End Sub
    Private Sub CalculateCanvasMinY()
        Canvas.MinY = Me.Height - Canvas.Height
        If Canvas.MinY > 0 Then Canvas.MinY = 0
    End Sub
    Private Sub CalculateCanvasMinX()
        Canvas.MinX = Me.Width - Canvas.Width
        If Canvas.MinX > 0 Then Canvas.MinX = 0
    End Sub
    Private Sub RecalculateCanvasParameters()
        If Wire.Y <> 0 Then
            If Arrangement = "gorizontal" Then
                Canvas.DestX = 0
                Canvas.LinesInBox = Math.Truncate(BmpMain.Height / (Wire.Y + Wire.dY))
                Canvas.Columns = Math.Truncate((BmpMain.Width - Wire.min_dX) / (Wire.X + Wire.min_dX))
                If Canvas.Columns < 1 Then Canvas.Columns = 1
                Wire.dX = (BmpMain.Width - Canvas.Columns * Wire.X) / (Canvas.Columns + 1)
                Canvas.Plus = Math.Truncate((BmpMain.Width - ((Wire.X + Wire.dX) * Canvas.Columns - Wire.dX)) / 2)
            Else
                Canvas.DestY = 0
                Canvas.LinesInBox = Math.Truncate(BmpMain.Width / (Wire.X + Wire.dX))
                Canvas.Columns = Math.Truncate((BmpMain.Height - Wire.dY) / (Wire.Y + Wire.dY))
                If Canvas.Columns < 1 Then Canvas.Columns = 1
                Wire.dX = Wire.min_dX ' (BmpMain.Width - Canvas.Columns * Wire.X) / (Canvas.Columns + 1)
                Canvas.Plus = Math.Truncate((BmpMain.Height - ((Wire.Y + Wire.dY) * Canvas.Columns - Wire.dY)) / 2)
            End If
        End If
    End Sub
    Public Sub SetWire(ByVal x As Short, ByVal y As Short, ByVal dx As Short, ByVal dy As Short)
        If x > 500 Then x = 500
        If y > 500 Then y = 500
        Wire.X = x
        Wire.Y = y
        Wire.min_dX = dx
        Wire.dY = dy

        If Wire.Y <= 32 And Wire.Border > 1 Then Wire.Border = 1

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
                With Image(i)
                    .Loaded = False
                    .Loading = False
                    .ReLoaded = False
                    .Transparency = 0
                    .DestTransparency = 0
                    .Selected = False
                    .Type = ""
                End With
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
        NextFrame(True)
        SelectedImageIndex = 0
        tmrAnimation.Enabled = True
    End Sub
    Public Sub AddImage(ByVal Filename As String)
        NImages += 1
        Image(NImages).FileName = Filename
        Image(NImages).Name = GetFileName(Filename)
        Image(NImages).Singer = "noname"
        Image(NImages).InTagsIngex = 0
        Image(NImages).Transparency = 0
        Image(NImages).DestTransparency = 0.5
        FindWidthWithText_NoReduce(NImages)
        'Image(NImages).DestX = 0
        'FindWidthWithText(NImages) 'BAAD
        If SelectedImageIndex = 0 Then SelectedImageIndex = 1
        If LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".vbproj" Or _
           LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".csproj" Or _
           LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".cpproj" Then _
           SelectedImageIndex = NImages
    End Sub
#Region "------|  Sortings  |-"
    Public Sub SortImagesByName()
        Dim i, j As Long
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If Image(i).Name > Image(i + 1).Name Then
                    SwapImages(i, i + 1)
                End If
            Next
        Next
        OrderImages()
        Sorting = "by name"
    End Sub
    Public Sub SortImagesBySinger()
        Dim i, j As Long
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If Image(i).Singer > Image(i + 1).Singer Then
                    SwapImages(i, i + 1)
                End If
            Next
        Next
        OrderImages()
        Sorting = "by singer"
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
        Sorting = "by rating"
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
        Sorting = "by pop"
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
                    d = dates(i) : dates(i) = dates(i + 1) : dates(i + 1) = d
                End If
            Next
        Next
        OrderImages()
        Sorting = "by date"
    End Sub
    Public Sub SortImagesByType()
        Dim i, j As Long, Temp(NImages), T As String
        For i = 1 To NImages
            Temp(i) = LCase(IO.Path.GetExtension(Image(i).FileName))
        Next
        For j = 1 To NImages
            For i = 1 To NImages - 1
                If Temp(i) > Temp(i + 1) Then
                    SwapImages(i, i + 1)
                    T = Temp(i) : Temp(i) = Temp(i + 1) : Temp(i + 1) = T
                End If
            Next
        Next
        OrderImages()
        'SetCanvas()
        Sorting = "by type"
    End Sub
    Public Sub RandomFiles()
        If Sorting = "user" Then
            Dim i As Long
            For i = 1 To NImages * 20
                SwapImages(Rnd() * (NImages - 1) + 1, Rnd() * (NImages - 1) + 1)
            Next
            OrderImages()
        Else
            Sorting = "user"
        End If
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

        If a = SelectedImageIndex Then SelectedImageIndex = b Else If b = SelectedImageIndex Then SelectedImageIndex = a
    End Sub
#End Region
#End Region
#Region "--------------------------|  LOCATION & ANIMATING  |-------------------------- GOOD"
    Private Sub SetImageLocation(ByVal i As Long)
        If Arrangement = "gorizontal" Then
            Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
            Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
        Else
            Image(i).DestY = ((i - 1) Mod Canvas.Columns) * (Wire.Y + Wire.dY) + Canvas.Plus
            Image(i).DestX = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.X + Wire.dX) + Wire.dX
        End If
        Image(i).X = Image(i).DestX
        Image(i).Y = Image(i).DestY
        Image(i).Transparency = 0
        Image(i).Animate = True
    End Sub
    Private Sub SetImageDestination(ByVal i As Long)
        If Arrangement = "gorizontal" Then
            Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
            Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
        Else
            Image(i).DestY = ((i - 1) Mod Canvas.Columns) * (Wire.Y + Wire.dY) + Canvas.Plus
            Image(i).DestX = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.X + Wire.dX) + Wire.dX
        End If
        Image(i).Animate = True
    End Sub
    Public Sub SetImagesLocation()
        For i As Long = 1 To NImages
            SetImageLocation(i)
        Next
        CalculateCanvasHeight()
        CalculateCanvasWidth()
        CalculateCanvasMinY()
        CalculateCanvasMinX()
        IsAnimatedImages = True
    End Sub
    Public Sub OrderImages()
        If Arrangement = "gorizontal" Then
            For i As Long = 1 To NImages
                With Image(i)
                    .DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                    .DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
                    .DestTransparency = 1

                    If .Loaded = True And .ReLoaded = False Then
                        If FileTags.Tags(.InTagsIngex).Type = "image" Then
                            Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)
                            If Wire.Y >= 40 Then
                                If ShowImagesName Then s.Y -= 14
                            Else
                                s.X = s.Y
                            End If
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        ElseIf Wire.Y <= 32 Then
                            Dim s As New Point(Wire.X, Wire.Y)
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        End If
                    End If
                    .Animate = True
                End With
            Next
        Else
            For i As Long = 1 To NImages
                With Image(i)
                    .DestY = ((i - 1) Mod Canvas.Columns) * (Wire.Y + Wire.dY) + Canvas.Plus
                    .DestX = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.X + Wire.dX) + Wire.dX
                    .DestTransparency = 1
                    If .Loaded = True And .ReLoaded = False Then
                        If FileTags.Tags(.InTagsIngex).Type = "image" Then
                            Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)
                            If Wire.Y >= 40 Then
                                If ShowImagesName Then s.Y -= 14
                            Else
                                s.X = s.Y
                            End If
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        ElseIf Wire.Y <= 32 Then
                            Dim s As New Point(Wire.X, Wire.Y)
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        End If
                    End If
                    .Animate = True
                End With
            Next
        End If

        CalculateCanvasHeight()
        CalculateCanvasWidth()
        CalculateCanvasMinY()
        CalculateCanvasMinX()
        IsAnimatedImages = True
    End Sub
    Public Sub RandomImages()
        If Sorting = "user" Then
            For i As Long = 1 To NImages
                If Image(i).Loaded Then
                    Image(i).Animate = True
                    Image(i).DestX = Rnd() * (BmpMain.Width - Thumbnail(i).Width) '(Wire.X - Thumbnail(i).Width) / 2 + ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                    Image(i).DestY = Rnd() * (BmpMain.Height - Thumbnail(i).Height) '(Wire.Y - Thumbnail(i).Height) / 2 + Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
                End If
            Next
            CalculateCanvasHeight()
            CalculateCanvasWidth()
            CalculateCanvasMinY()
            CalculateCanvasMinX()
            IsAnimatedImages = True
        Else
            Sorting = "user"
        End If
    End Sub
    Private Sub StopAnimatingImages()
        For i As Long = 1 To NImages
            Image(i).Animate = False
        Next
        IsAnimatedImages = False
    End Sub
    Private Sub StopCanvas()
        Canvas.Y = 0 : Canvas.V = 0
        Canvas.X = 0 : Canvas.VX = 0
        Canvas.Animate = False
    End Sub
#End Region
#Region "--------------------------|  THUMBNAILS            |--------------------------"
    Dim Thumbnail(50000) As Bitmap
    Dim LoadingProgress As Double, Str As String
    Public StopLoading As Boolean = False
    Public ReloadWidthWithText As Boolean = False

    Public Sub ReloadAllThumbs()
        StopLoading = False
        For i As Long = 1 To NImages
            Image(i).ReLoaded = False
            Image(i).Loading = False
            ReloadWidthWithText = True
            'FindWidthWithText(i)
        Next
        IsNotEverithingLoaded = True
    End Sub

    Public Sub MakeAllThumbnails()
        If NImages > 0 Then
            If Wire.Y < 40 Then
                Dim a As Long = 0, max As Long = 0
                For i As Long = 1 To NImages
                    a += Image(i).WidthWithText
                    If max < Image(i).WidthWithText Then max = Image(i).WidthWithText
                Next
                a = a / NImages
                Wire.X = max + 50
                If Wire.X > Me.Width / 2 - Wire.dX * 2 Then Wire.X = Me.Width / 2 - Wire.dX * 2
            End If
            'SetWire(Wire.X, Wire.Y, Wire.dX, Wire.dY)

            tmrAnimation.Enabled = False

            IsNotEverithingLoaded = True

            Cursor = Cursors.Arrow
            StopLoading = False
            RecalculateCanvasParameters()
            CurrentLoadingImgIndex = 0

            CalculateCanvasHeight()
            CalculateCanvasMinY()

            tmrAnimation.Enabled = True

            'SetCanvas() '!!!!!!!!!!!!!!!!
        End If
    End Sub

    Dim LoadedThumbnail As Bitmap, LoadingStrInfo As String
    Public CurrentLoadingImgIndex As Long, CurrentLoadingFileName As String
    Private Sub bwLoadOne_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadOne.DoWork
        MakeThumbnail(CurrentLoadingFileName)
    End Sub
    Private Sub MakeThumbnail(ByVal FileName As String)
        Dim wait As Boolean = True
        If InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(FileName)) + " ") Then
            Dim ThNumber As Long = SearchInThumbs(FileName)
            Dim bmp, bmp1 As Bitmap
            'Dim s As New Point(Wire.X, Wire.Y - 14)
            Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)

            If Wire.Y < 40 Then
                s.X = Wire.Y
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
            Else                                               'IF THERE IS NO THUMBNAIL
                If Not MakeThumb(FileName, s, LoadedThumbnail) Then LoadedThumbnail = bmp_error.Clone
            End If
        Else
            If IO.Directory.Exists(FileName) Then

                If Wire.Y >= 32 Then
                    LoadedThumbnail = bmp_dir_min_32.Clone
                Else
                    LoadedThumbnail = bmp_dir_min.Clone
                End If
            Else
                wait = False
                '1. Ищем в реестре расширение: «HKEY_CLASSES_ROOT\.doc». Берём значение из «HKEY_CLASSES_ROOT\.doc\(Default)» (например, «Word.Document.8»).
                '2. Ищем «HKEY_CLASSES_ROOT\Word.Document.8», берём значение из «HKEY_CLASSES_ROOT\Word.Document.8\DefaultIcon\(Default)» (например, «C:\WINDOWS\Installer\{90110419-6000-11D3-8CFE-0150048383C9}\wordicon.exe,1»).
                'MsgBox(Mid(Image(i).FileName, Image(i).FileName.Length - 2, 3))
                If Mid(FileName, FileName.Length - 2, 3) <> "mp3" And Mid(FileName, FileName.Length - 3, 4) <> "flac" Then
                    If Wire.Y >= 32 Then
                        LoadedThumbnail = Icon.ExtractAssociatedIcon(FileName).ToBitmap
                    Else
                        LoadedThumbnail = New Bitmap(Wire.Y, Wire.Y)
                        Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                            graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                            Dim r As New Rectangle(0, 0, Wire.Y, Wire.Y)
                            graf.DrawIcon(Icon.ExtractAssociatedIcon(FileName), r)
                        End Using
                    End If
                Else
                    If Wire.Y >= 32 Then
                        LoadedThumbnail = bmp_muz_ico.Clone
                    Else
                        LoadedThumbnail = bmp_muz_ico_16.Clone
                    End If
                End If
            End If
        End If
    End Sub
#End Region
#Region "-----|  Resizing "
    Public Sub ResizeStarted()
        DoResizeEvent = False
        tmrAnimation.Enabled = False
        picMain.Visible = False
        Me.BackgroundImage = BmpMain
    End Sub
    Public Sub ResizeStarted(ByRef DestSizeW As Integer, ByRef DestSizeH As Integer)
        DoResizeEvent = False
        tmrAnimation.Enabled = False
        picMain.Visible = False
        If DestSizeW > BmpMain.Width Or DestSizeH > BmpMain.Height Then
            BmpMain = New Bitmap(Math.Max(DestSizeW, BmpMain.Width), Math.Max(DestSizeH, BmpMain.Height))
            GraphicsMain = Graphics.FromImage(BmpMain)
        End If
        Me.BackgroundImage = BmpMain
    End Sub
    Public Sub Resizing2(ByRef W As Integer, ByRef H As Integer)
        If H > 35 Then
            NextFrame(True)
            Dim r As New Rectangle(0, 0, W, H)
            Me.BackgroundImage = BmpMain.Clone(r, Imaging.PixelFormat.DontCare)
            Me.Width = W
            Me.Height = H
        End If
        If H > 0 And H <= 35 Then
            NextFrame(True)

            If H < 35 Then
                Dim a2 As New SolidBrush(Color.AliceBlue)
                If H < 10 Then a2.Color = Color.FromArgb(255, 200, 200, 200) Else a2.Color = Color.FromArgb(350 - H * 10, 200, 200, 200)
                GraphicsMain.FillRectangle(a2, 0, 0, W, H)
            End If

            Dim r As New Rectangle(0, 0, W, H)
            Me.BackgroundImage = BmpMain.Clone(r, Imaging.PixelFormat.DontCare)
            Me.Height = H
        End If
        Me.Refresh()
    End Sub
    Public Sub Resizing(ByVal w As Integer, ByVal h As Integer)
        If h > 35 Then
            BmpMain = New Bitmap(w, h)
            GraphicsMain = Graphics.FromImage(BmpMain)
            'NextFrame(True) '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            RecalculateCanvasParameters()
            OrderImages()
            NextFrame(True)
            Me.BackgroundImage = BmpMain
            Me.Height = h
        End If
        If h > 0 And h <= 35 Then
            BmpMain = New Bitmap(w, h)
            GraphicsMain = Graphics.FromImage(BmpMain)
            RecalculateCanvasParameters()
            OrderImages()
            NextFrame(True)

            If h < 35 Then
                Dim a2 As New SolidBrush(Color.AliceBlue)
                If h < 10 Then a2.Color = Color.FromArgb(255, 200, 200, 200) Else a2.Color = Color.FromArgb(350 - h * 10, 200, 200, 200)
                GraphicsMain.FillRectangle(a2, 0, 0, w, h)
            End If

            Me.BackgroundImage = BmpMain
            Me.Height = h
        End If
    End Sub
    Public Sub ResizeEnded(ByRef DestSizeW As Integer, ByRef DestSizeH As Integer)
        If DestSizeW <> BmpMain.Width Or DestSizeH <> BmpMain.Height Then
            BmpMain = New Bitmap(Math.Max(DestSizeW, BmpMain.Width), Math.Max(DestSizeH, BmpMain.Height))
            GraphicsMain = Graphics.FromImage(BmpMain)
        End If
        'NextFrame(True)

        tmrAnimation.Enabled = True

        picMain.Width = Me.Width
        picMain.Height = Me.Height
        picMain.Image = BmpMain
        picMain.Visible = True
        DoResizeEvent = True
    End Sub
    Public Sub ResizeEnded()
        RecalculateCanvasParameters()
        OrderImages()

        tmrAnimation.Enabled = True

        picMain.Width = Me.Width
        picMain.Height = Me.Height
        picMain.Image = BmpMain
        picMain.Visible = True
        DoResizeEvent = True
    End Sub
#End Region
#Region "-----|  Draggings"
    Dim IsMouseDown As Boolean
    Dim Cl As New Point(10, 10), ClF As Point, moved As Boolean = False, ChosenObj As Long
    Dim MDTime As Long = 0

    Dim IsSmthDragging As Boolean = False
    Dim DraggingCount As Long = 0
    Sub StartDragDrop(ByVal i As Integer)
        'picForDrag.Image = Thumbnail(i)
        IsMouseDown = False
        IsSmthDragging = True
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
    Private Sub picMain_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picMain.DragEnter, Me.DragEnter
        e.Effect = DragDropEffects.Copy
        If IsSmthDragging = True Then
            Cl.X = e.X - Me.Left - Me.Parent.Left - Canvas.X
            Cl.Y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y
            If Not (Image(ChosenObj).X <= Cl.X And Image(ChosenObj).X + Image(ChosenObj).WidthWithText >= Cl.X _
            And Image(ChosenObj).Y <= Cl.Y And Image(ChosenObj).Y + Wire.Y >= Cl.Y) Then
                Image(ChosenObj).X = Cl.X - Image(ChosenObj).WidthWithText / 2
                Image(ChosenObj).DestX = Cl.X - Image(ChosenObj).WidthWithText / 2
                Image(ChosenObj).Y = Cl.Y - Wire.Y / 2
                Image(ChosenObj).DestY = Cl.Y - Wire.Y / 2
            End If
        Else
            IsSmthDragging = True
            Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
            DraggingCount = files.Length
            NDraggingFiles = 0
        End If
    End Sub
    Private Sub ucImagesBox_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragOver
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
        DraggingCount = files.Length
        'NextFrame(True)
        If IsSmthDragging = True Then
            Dim x, y As Long
            x = e.X - Me.Left - Me.Parent.Left - Canvas.X
            y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y

            If NDraggingFiles > 0 Then
                Image(ChosenObj).X += x - Cl.X : Cl.X = x
                Image(ChosenObj).Y += y - Cl.Y : Cl.Y = y
                Image(ChosenObj).DestX = Image(ChosenObj).X
                Image(ChosenObj).DestY = Image(ChosenObj).Y
                If Image(ChosenObj).Selected = True Then
                    Dim i As Long
                    For n As Long = 1 To NDraggingFiles
                        i = DraggingFilesList(n)
                        If i <> ChosenObj Then
                            Image(i).DestX = Image(ChosenObj).X + Rnd() * 10 - 5
                            Image(i).DestY = Image(ChosenObj).Y + Rnd() * 4 - 2
                            If Wire.Y < 60 Then
                                Image(i).DestY += (n - IndexOfChosenInDraggingFiles) * Wire.Y
                            Else
                                Image(i).DestY += (n - IndexOfChosenInDraggingFiles) * Wire.Y / 3
                            End If

                            If Image(i).Animate = False Then
                                Image(i).Animate = True
                                IsAnimatedImages = True
                            End If
                        End If
                    Next
                End If
            End If

            If Sorting = "user" And NDraggingFiles = 1 And Image(ChosenObj).Selected = False Then
                Dim ii As Short
                If Arrangement = "gorizontal" Then
                    x = x - Canvas.Plus : y = y - Wire.dY
                    ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
                Else
                    y = y - Canvas.Plus : x = x - Wire.dX
                    ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
                End If
                If ii > NImages Then ii = NImages
                'MsgBox(ii)
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
            End If
        End If
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
                AddImage(t)
                Image(NImages).Transparency = 0
                Image(NImages).DestTransparency = 1

                Image(NImages).Loaded = False
                IsNotEverithingLoaded = True

                Image(NImages).X = e.X - Me.Left - Me.Parent.Left - Canvas.X
                Image(NImages).Y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y
                Image(NImages).DestX = Image(NImages).X
                Image(NImages).DestY = Image(NImages).Y

                Dim x = Image(NImages).X
                Dim y = Image(NImages).Y
                Dim ii As Short
                ChosenObj = NImages

                If Arrangement = "gorizontal" Then
                    x = x - Canvas.Plus : y = y - Wire.dY
                    ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
                Else
                    y = y - Canvas.Plus : x = x - Wire.dX
                    ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
                End If
                If ii > NImages Then ii = NImages
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
                SelectedImageIndex = ii

                Image(NImages).Animate = True
                IsAnimatedImages = True
                OrderImages()
            End If
        Next
        OrderImages()
        IsAnimatedImages = True
        IsSmthDragging = False
    End Sub
    Private Sub ucImagesBox_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DragLeave
        IsSmthDragging = False
        'OrderImages()
        'If Image(ChosenObj).Selected = True Then
        For n As Long = 1 To NDraggingFiles
            'Image(DraggingFilesList(n)).Transparency = 0
            'SetImageLocation(DraggingFilesList(n))
            SetImageDestination(DraggingFilesList(n))
            Image(DraggingFilesList(n)).DestTransparency = 1
        Next
        OrderImages()
        IsAnimatedImages = True
        'Else
        'SetImageDestination(DraggingFilesList(ChosenObj))
        'Image(DraggingFilesList(ChosenObj)).DestTransparency = 1
        'End If
        IsAnimatedImages = True
    End Sub
#End Region
#Region "-----|  Mouse Events"
    Private Sub CorrectCanvasDestY()
        If Canvas.DestY > 0 Then Canvas.DestY = 0
        If Canvas.DestY < Canvas.MinY Then Canvas.DestY = Canvas.MinY
    End Sub
    Private Sub CorrectCanvasDestX()
        If Canvas.DestX > 0 Then Canvas.DestX = 0
        If Canvas.DestX < Canvas.MinX Then Canvas.DestX = Canvas.MinX
    End Sub
    Dim FirstX As Long, FirstY As Long
    Private Sub picPhoto_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDown 'Me.MouseDown
        'If e.Button = Windows.Forms.MouseButtons.Right Then MDTime = 1
        FirstX = e.X
        FirstY = e.Y
        If Me.Focused = False Then Me.Select()

        moved = False
        ChosenObj = 0
        Dim a As ButtonStruct
        Dim i, ii As Long
        For i = 0 To Buttons.Length - 1
            a = Buttons(i)
            If e.X >= a.X And e.X <= a.X + a.Image.Width And e.Y >= a.Y And e.Y <= a.Y + a.Image.Height And a.Visible = True Then
                Select Case i
                    Case 5
                        ShowSortingVariants()
                    Case 0
                        SortImagesByName()
                    Case 1
                        SortImagesByRating()
                    Case 7
                        SortImagesBySinger()
                    Case 8
                        SortImagesByLaunchingCount()
                    Case 12
                        RandomFiles()


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
        If ChosenObj <> -6 Then HideSortingVariants()

        MDTime = 1

        Dim p As New Point(e.X - Canvas.X, e.Y - Canvas.Y)
        Cl.X = e.X : Cl.Y = e.Y
        ClF.X = Cl.X : ClF.Y = Cl.Y

        IsMouseDown = True
        If ChosenObj = 0 Then
            Dim founded As Boolean = False
            Canvas.V = 0
            Canvas.VX = 0
            Canvas.Animate = False
            'IsMouseDown = True
            For ii = 1 To NImages
                i = ii 'foto_p(ii)
                If Image(i).Loaded Then
                    If p.X >= Image(i).X And p.X < Image(i).X + Image(i).WidthWithText And p.Y >= Image(i).Y And p.Y < Image(i).Y + Wire.Y Then
                        founded = True
                        'If e.Button = Windows.Forms.MouseButtons.Right Then
                        Image(i).Animate = False : ChosenObj = i
                        SelectedImageIndex = i
                        Exit For
                    End If
                Else
                    If p.X >= Image(i).X And p.X < Image(i).X + Wire.X And p.Y >= Image(i).Y And p.Y < Image(i).Y + Wire.Y Then
                        founded = True
                        'If e.Button = Windows.Forms.MouseButtons.Right Then 
                        Image(i).Animate = False : ChosenObj = i
                        SelectedImageIndex = i
                        Exit For
                    End If
                End If
            Next
            If Not founded And (e.Button = Windows.Forms.MouseButtons.Right Or (IsAltDown Or IsShiftDown)) Then
                ChosenObj = -1111
            End If
        End If
        'If ChosenObj = 0 And ShiftPushed Then
        '    ChosenObj = -1111
        'End If
        'If e.Button = Windows.Forms.MouseButtons.Right Then MsgBox(ChosenObj.ToString)
        NextFrame(True)
    End Sub
    Private Sub picPhoto_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseMove 'Me.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None And (FirstX <> e.X Or FirstY <> e.Y) Then
            If moved = False And (FirstX - e.X) ^ 2 + (FirstY - e.Y) ^ 2 >= 9 Then
                moved = True
                If (ChosenObj >= 0 And e.Button = Windows.Forms.MouseButtons.Left) Then ChosenObj = 0
            End If

            If (ChosenObj >= 0 And e.Button = Windows.Forms.MouseButtons.Left) Then
                Canvas.Y += e.Y - Cl.Y : Canvas.X += e.X - Cl.X
                ClF.X = Cl.X : ClF.Y = Cl.Y
                Cl.X = e.X : Cl.Y = e.Y
                If Arrangement = "gorizontal" Then Canvas.X = 0 Else Canvas.Y = 0
            ElseIf (ChosenObj > 0 And e.Button = Windows.Forms.MouseButtons.Right) Then
                Image(ChosenObj).X += e.X - Cl.X : Image(ChosenObj).Y += e.Y - Cl.Y
                If moved Then
                    If Image(ChosenObj).Selected Then
                        NDraggingFiles = 0
                        For j As Long = 1 To NImages
                            If Image(j).Selected Then NDraggingFiles = NDraggingFiles + 1
                        Next
                        ReDim DraggingFilesList(NDraggingFiles)
                        NDraggingFiles = 0
                        For j As Long = 1 To NImages
                            If Image(j).Selected Then
                                NDraggingFiles = NDraggingFiles + 1
                                DraggingFilesList(NDraggingFiles) = j
                                Image(j).DestTransparency = 0.5
                                If ChosenObj = j Then IndexOfChosenInDraggingFiles = NDraggingFiles
                                Image(j).Animate = True
                            End If
                        Next
                    Else
                        NDraggingFiles = 1
                        ReDim DraggingFilesList(NDraggingFiles)
                        DraggingFilesList(NDraggingFiles) = ChosenObj
                        Image(ChosenObj).DestTransparency = 0.5
                        IndexOfChosenInDraggingFiles = 1
                        Image(ChosenObj).Animate = True
                    End If
                    IsAnimatedImages = True
                    StartDragDrop(ChosenObj)
                    Cl.X = e.X : Cl.Y = e.Y
                End If
            ElseIf ChosenObj = -3 Then
                Canvas.Y += (e.Y - Cl.Y) / ((Me.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) / Canvas.MinY)
                Cl.Y = e.Y
            ElseIf ChosenObj = -1111 Then
                Dim x, i1, i2, t, b, i As Long
                i1 = XYtoXIndex(e.X - Canvas.X, e.Y - Canvas.Y)
                i2 = XYtoXIndex(ClF.X - Canvas.X, ClF.Y - Canvas.Y)
                t = XYtoYIndex(e.X - Canvas.X, e.Y - Canvas.Y)
                b = XYtoYIndex(ClF.X - Canvas.X, ClF.Y - Canvas.Y)
                If t = 0 Then t = Canvas.Columns
                If b = 0 Then b = Canvas.Columns
                If i1 > i2 Then i = i1 : i1 = i2 : i2 = i
                If t > b Then i = t : t = b : b = i
                i2 += Canvas.Columns - 1

                i1 = CorrectIndex(i1)
                i2 = CorrectIndex(i2)
                For i = 1 To NImages
                    x = i Mod Canvas.Columns
                    If x = 0 And i > 0 Then x = Canvas.Columns
                    If IsAltDown Then
                        If (x >= t And x <= b) And i >= i1 And i <= i2 Then Image(i).WillBeUnSelected = True Else Image(i).WillBeUnSelected = False
                    Else
                        'End If
                        'If IsShiftDown Then
                        If (x >= t And x <= b) And i >= i1 And i <= i2 Then Image(i).WillBeSelected = True Else Image(i).WillBeSelected = False
                    End If
                Next
                Cl.X = e.X : Cl.Y = e.Y
            End If
        End If
    End Sub
    Private Function XYtoXIndex(ByVal x As Long, ByVal y As Long)
        If Arrangement = "gorizontal" Then
            x = x - Canvas.Plus : y = y - Wire.dY
            Return (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
        Else
            y = y - Canvas.Plus : x = x - Wire.dX
            Return (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        End If
    End Function
    Private Function XYtoYIndex(ByVal x As Long, ByVal y As Long)
        If Arrangement = "gorizontal" Then
            x = x - Canvas.Plus
            Return Math.Truncate(x / (Wire.X + Wire.dX)) + 1
        Else
            y = y - Canvas.Plus
            Return Math.Truncate(y / (Wire.Y + Wire.dY)) + 1
        End If
    End Function
    Private Function XYtoIndex(ByVal x As Long, ByVal y As Long)
        If Arrangement = "gorizontal" Then
            x = x - Canvas.Plus : y = y - Wire.dY
            Return (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        Else
            y = y - Canvas.Plus : x = x - Wire.dX
            Return (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
        End If
    End Function
    Private Function CorrectIndex(ByVal i As Long)
        If i > NImages Then i = NImages
        If i < 0 Then i = 0
        Return i
    End Function

    Private Sub picPhoto_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseUp 'Me.MouseUp
        If ChosenObj = -1111 Then
            For i As Integer = 1 To NImages
                If Image(i).WillBeSelected = True Then Image(i).Selected = True : Image(i).WillBeSelected = False
                If Image(i).WillBeUnSelected = True Then Image(i).Selected = False : Image(i).WillBeUnSelected = False
            Next
        End If
        If ChosenObj = 0 Then
            If Arrangement = "gorizontal" Then Canvas.V = e.Y - ClF.Y Else Canvas.VX = e.X - ClF.X
        End If
        If moved = False And e.Button = Windows.Forms.MouseButtons.Left Then 'And ChosenObj > 0 Then
            If ChosenObj > 0 And Not moved Then
                If e.Button = Windows.Forms.MouseButtons.Right Then
                    Image(ChosenObj).Selected = Not (Image(ChosenObj).Selected)
                End If

                If InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(Image(SelectedImageIndex).FileName)) + " ") Then
                    If Mid(Path, 1, Path.Length - 2) = Application.StartupPath + "\35photo" Then
                        Process.Start(URLs35photo(ChosenObj))
                    Else
                        Dim a As New frmShowPhoto2
                        Dim FN(NImages) As String
                        For j As Long = 1 To NImages
                            FN(j) = Image(j).FileName
                        Next
                        If Image(SelectedImageIndex).Loaded = True Then
                            a.Init3(Thumbnail(SelectedImageIndex), SelectedImageIndex, FN, Parent.Left + Me.Left + Image(SelectedImageIndex).X + Canvas.X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
                        Else
                            a.Init3(bmp_error, SelectedImageIndex, FN, Parent.Left + Me.Left + Image(SelectedImageIndex).X + Canvas.X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
                        End If
                        a.Show()
                    End If
                ElseIf Image(SelectedImageIndex).Type = "folder" Then
                    RaiseEvent ChangeDir(Image(SelectedImageIndex).FileName)
                Else
                    Process.Start(Image(SelectedImageIndex).FileName, AppWinStyle.NormalFocus)
                End If
                FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).LaunchingTimes += 1
            End If
        ElseIf moved = False And e.Button = Windows.Forms.MouseButtons.Right Then
            Image(SelectedImageIndex).Selected = Not Image(SelectedImageIndex).Selected
        End If
        ChosenObj = 0
        IsMouseDown = False
        MDTime = 0
        NextFrame(True)
        OrderImages()
    End Sub
    Private Sub ucImagesBox_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If Arrangement = "gorizontal" Then
            If Canvas.Animate = False And Canvas.DestY <> Canvas.Y Then Canvas.DestY = Canvas.Y
            Canvas.DestY = Canvas.DestY + e.Delta
            Canvas.Y = Canvas.DestY - 0.7 'или без неё
            Canvas.Animate = True
            If Canvas.DestY > 0 Then
                Dim f As Boolean = True : RaiseEvent SendFocusToTheTop(f)
                If f = True Then Canvas.Y = 0 : Canvas.DestY = 0 : NextFrame(True)
            End If
            If Image(SelectedImageIndex).Y + Canvas.Y < 0 Then
                SelectedImageIndex = Math.Truncate((-Canvas.Y) / (Wire.Y + Wire.dY)) * Canvas.Columns + Canvas.Columns + 1
                If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
                If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
            End If
            If Image(SelectedImageIndex).Y + Canvas.Y > Me.Height - Wire.Y Then
                SelectedImageIndex = Math.Truncate((-Canvas.Y) / (Wire.Y + Wire.dY) + Canvas.LinesInBox) * Canvas.Columns
                If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
                If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
            End If
        Else
            If Canvas.Animate = False And Canvas.DestX <> Canvas.X Then Canvas.DestX = Canvas.X
            Canvas.DestX = Canvas.DestX + e.Delta
            Canvas.X = Canvas.DestX - 0.7 'или без неё
            Canvas.Animate = True
            If Canvas.DestX > 0 Then
                Dim f As Boolean = True : RaiseEvent SendFocusToTheTop(f)
                If f = True Then Canvas.X = 0 : Canvas.DestX = 0 : NextFrame(True)
            End If
            If Image(SelectedImageIndex).X + Canvas.X < 0 Then
                SelectedImageIndex = Math.Truncate((-Canvas.X) / (Wire.X + Wire.dX)) * Canvas.Columns + Canvas.Columns + 1
                If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
                If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
            End If
            If Image(SelectedImageIndex).X + Canvas.X > Me.Width - Wire.X Then
                SelectedImageIndex = Math.Truncate((-Canvas.X) / (Wire.X + Wire.dX) + Canvas.LinesInBox) * Canvas.Columns
                If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
                If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
            End If
        End If
        'NextFrame(True)
    End Sub
    Private Sub picMain_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDoubleClick
        If ChosenObj = 0 Then
            RaiseEvent FillScreenMe()
        End If
    End Sub
    Private Sub ucImagesBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.ShiftKey Then IsShiftDown = False
        If e.KeyCode = Keys.ControlKey Then IsCtrlDown = False
        If e.KeyCode = 18 Then IsAltDown = False

        IsEnterDown = False
        NextFrame(True)
    End Sub
#End Region
#Region "-----|  Choosing Images And Keys"
    Sub CorrectSelectedImageIndex()
        If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
        If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
    End Sub
    Dim InputLine As String = " "
    Dim StartSelectionFrom As Long
    Private Sub ChooseObj(ByVal key As Keys)
        Select Case key
            Case Keys.Down
                If Arrangement <> "gorizontal" Then
                    SelectedImageIndex += 1 : CorrectSelectedImageIndex()
                Else
                    SelectedImageIndex += Canvas.Columns : CorrectSelectedImageIndex()
                End If
            Case Keys.Up
                If SelectedImageIndex <= 1 Then
                    Dim a As Boolean = True
                    RaiseEvent SendFocusToTheTop(a)
                    If a = False Then
                        If Arrangement <> "gorizontal" Then
                            SelectedImageIndex -= 1 : CorrectSelectedImageIndex()
                        Else
                            SelectedImageIndex -= Canvas.Columns : CorrectSelectedImageIndex()
                        End If
                    End If
                Else
                    If Arrangement <> "gorizontal" Then
                        SelectedImageIndex -= 1 : CorrectSelectedImageIndex()
                    Else
                        SelectedImageIndex -= Canvas.Columns : CorrectSelectedImageIndex()
                    End If
                End If
            Case Keys.Left
                If Arrangement <> "gorizontal" Then
                    SelectedImageIndex -= Canvas.Columns : CorrectSelectedImageIndex()
                Else
                    SelectedImageIndex -= 1 : CorrectSelectedImageIndex()
                End If
            Case Keys.Right
                If Arrangement <> "gorizontal" Then
                    SelectedImageIndex += Canvas.Columns : CorrectSelectedImageIndex()
                Else
                    SelectedImageIndex += 1 : CorrectSelectedImageIndex()
                End If
        End Select
    End Sub
    Dim IsShiftDown As Boolean = False
    Dim IsAltDown As Boolean = False
    Dim IsCtrlDown As Boolean = False
    Private Sub ucImagesBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.ShiftKey Then IsShiftDown = True
        If e.KeyCode = Keys.ControlKey Then IsCtrlDown = True
        If e.KeyCode = 18 Then IsAltDown = True

        If (e.Control = False And e.Shift = False And e.Alt = False) And ((Chr(e.KeyValue) >= "A" And Chr(e.KeyValue) <= "Z") Or (Chr(e.KeyValue) >= "А" And Chr(e.KeyValue) <= "Я")) Then
            InputLine = InputLine + Chr(e.KeyCode)
            TimeFromTyping = System.DateTimeOffset.Now
        End If
        If InputLine.Length > 0 Then
            If InputLine = "АRENAME" Then
                Dim str As String
                Dim old_str As String = Image(SelectedImageIndex).FileName
                str = InputBox("new name", "name changing", IO.Path.GetFileName(old_str))
                If str <> "" Then
                    Try
                        Rename(old_str, Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str)
                        Image(SelectedImageIndex).FileName = Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str
                        FileTags.Files(Image(SelectedImageIndex).InTagsIngex) = Image(SelectedImageIndex).FileName
                    Catch ex As Exception
                        MsgBox("Не удаётся переименовать файл")
                    End Try
                End If
            Else
                Dim str As String = LCase(InputLine)
                If e.KeyCode = 187 Then
                    Dim NF As New frmTempArea
                    NF.Show()
                    NF.ImagesBox.SetWire(Wire.X, Wire.Y, Wire.dX, Wire.dY)
                    NF.ImagesBox.Wire.Border = Wire.Border
                    NF.ImagesBox.BGColor = BGColor
                    NF.ImagesBox.DrawShadow = False
                    NF.ImagesBox.ShowImagesName = True
                    NF.Left = 10 + Me.Parent.Left
                    NF.Top = 10 + Me.Parent.Top
                    NF.ImagesBox.StopLoading = True
                    NF.ImagesBox.ClearImages()

                    For i As Long = 1 To NImages
                        If InStr(LCase(IO.Path.GetFileName(Image(i).FileName)), str) > 0 Then NF.ImagesBox.AddImage(Image(i).FileName)
                    Next

                    NF.ImagesBox.SetImagesLocation()
                    NF.ImagesBox.MakeAllThumbnails()
                    NF.Text = Chr(34) + LCase(InputLine) + Chr(34) + " - search results"
                    NF.Select()
                    NF.ImagesBox.Select()
                Else
                    If e.KeyValue = 189 Then
                        For i As Long = 1 To NImages
                            If InStr(LCase(IO.Path.GetFileName(Image(i).FileName)), str) > 0 Then Image(i).Selected = True Else Image(i).Selected = False
                        Next
                    Else
                        For i As Long = 1 To NImages
                            If InStr(LCase(IO.Path.GetFileName(Image(i).FileName)), str) > 0 Then SelectedImageIndex = i : Exit For
                        Next
                    End If
                End If
            End If
        End If
        If e.Control = False And e.Alt = False And e.Shift = False Then
            If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
                ChooseObj(e.KeyCode)
            End If
            Select Case e.KeyCode
                Case Keys.PageDown
                    SelectedImageIndex += Canvas.LinesInBox * Canvas.Columns
                Case Keys.PageUp
                    SelectedImageIndex -= Canvas.LinesInBox * Canvas.Columns
                Case Keys.Escape
                    StopLoading = True
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
                        ElseIf Image(SelectedImageIndex).Type = "folder" Then
                            FlyMode = True
                            startX = Image(SelectedImageIndex).X
                            startY = Image(SelectedImageIndex).Y
                            RaiseEvent ChangeDir(Image(SelectedImageIndex).FileName)
                        Else
                            Process.Start(Image(SelectedImageIndex).FileName, AppWinStyle.NormalFocus)
                        End If
                        FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).LaunchingTimes += 1
                    End If
                Case Keys.Space
                    If SelectedImageIndex >= 1 And SelectedImageIndex <= NImages Then
                        Image(SelectedImageIndex).Selected = Not (Image(SelectedImageIndex).Selected)
                    End If
                Case Keys.F8
                    tmrAnimation.Enabled = False

                    For i As Long = 1 To NImages
                        If Image(i).Loaded = False Or Image(i).ReLoaded = False Then
                            MakeThumbnail(Image(i).FileName)

                            Image(i).InTagsIngex = FileTags.FindByFileName(Image(i).FileName)
                            If Image(i).InTagsIngex = 0 Then Image(i).InTagsIngex = FileTags.Add(Image(i).FileName)

                            If Wire.Y >= 32 Then
                                For d As Long = 1 To FileTags.Tags(Image(i).InTagsIngex).Rating
                                    Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                                        graf.DrawImage(bmp_star, 44 - d * 13, Wire.Y - 13)
                                    End Using
                                Next
                            Else
                                For d As Long = 1 To FileTags.Tags(Image(i).InTagsIngex).Rating
                                    Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                                        graf.DrawImage(bmp_star, LoadedThumbnail.Width - d * 13 - 2 - 4, Wire.Y - 15)
                                    End Using
                                Next
                            End If

                            Thumbnail(i) = LoadedThumbnail

                            FindWidthWithText(i)
                            
                            If Image(i).Loaded = False Then
                                If Wire.Y >= 40 Then Image(i).Y = Image(i).Y - Thumbnail(i).Height : Image(i).Transparency = 0 Else Image(i).X = Image(i).X - Thumbnail(i).Width - 5 : Image(i).Transparency = 0
                                Image(i).Transparency = 0
                                Image(i).Height = Thumbnail(i).Height
                                Image(i).Width = Thumbnail(i).Width
                            End If
                            Image(i).DestTransparency = 1
                            Image(i).DestHeight = Thumbnail(i).Height
                            Image(i).DestWidth = Thumbnail(i).Width

                            Image(i).Loaded = True
                            Image(i).Loading = False
                            Image(i).ReLoaded = True
                            If i * 1000 / NImages Mod 10 = 0 Then
                                CurrentLoadingImgIndex = i
                                DrawStatus()
                                picMain.Image = BmpMain
                                picMain.Refresh()
                            End If
                        End If
                    Next

                    IsNotEverithingLoaded = False
                    CurrentLoadingImgIndex = NImages + 1

                    If Mid(Path, 1, Path.Length - 2) = Application.StartupPath + "\35photo" Then
                        Dim i As Long = 0
                        FileOpen(1, Path + "urls.txt", OpenMode.Input)
                        While Not EOF(1)
                            i += 1
                            URLs35photo(i) = LineInput(1)
                        End While
                        FileClose(1)
                    End If
                    OrderImages()
                    IsAnimatedImages = True
                    'SaveThumbsInfo()
                    'FileTags.Save()
                    tmrAnimation.Enabled = True
            End Select
        End If
        If e.KeyCode = Keys.ShiftKey Or e.KeyCode = 18 Then StartSelectionFrom = SelectedImageIndex
        'MsgBox(e.KeyCode)
        If e.Shift Then
            If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
                ChooseObj(e.KeyCode)
                Dim st As Short = 1
                If StartSelectionFrom > SelectedImageIndex Then st = -1
                For i As Long = StartSelectionFrom To SelectedImageIndex Step st
                    Image(i).Selected = True
                Next
            End If
        End If
        If e.Alt Then
            If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
                ChooseObj(e.KeyCode)
                Dim st As Short = 1
                If StartSelectionFrom > SelectedImageIndex Then st = -1
                For i As Long = StartSelectionFrom To SelectedImageIndex Step st
                    Image(i).Selected = False
                Next
            End If
        End If
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
                    If MessageBox.Show("Sure?", "Correcting names", MessageBoxButtons.YesNo) = DialogResult.OK Then
                        Dim str As String
                        For i As Long = 1 To NImages
                            Dim old_str As String = Image(i).FileName
                            str = IO.Path.GetFileName(old_str).Replace("_", " ")
                            str = str.Replace("  ", " ")

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
        'ChosenObj = SelectedImageIndex

        SetCanvas()
        NextFrame(True)
    End Sub
    Public Sub SetCanvas()
        If Arrangement = "gorizontal" Then
            If Canvas.LinesInBox >= 3 Then
                If Image(SelectedImageIndex).Y < -Canvas.Y + (Wire.Y + Wire.dY) Then
                    Canvas.DestY = -Image(SelectedImageIndex).Y + Wire.dY + (Wire.Y + Wire.dY)
                End If
                If Image(SelectedImageIndex).Y > -Canvas.Y + Me.Height - (Wire.Y + Wire.dY) * 2 Then
                    Canvas.DestY = -Image(SelectedImageIndex).Y + Me.Height - (Wire.Y + Wire.dY) * 2
                End If
            Else
                If Image(SelectedImageIndex).Y < -Canvas.Y Then
                    Canvas.DestY = -Image(SelectedImageIndex).Y + Wire.dY
                End If
                If Image(SelectedImageIndex).Y > -Canvas.Y + Me.Height - (Wire.Y + Wire.dY) Then
                    Canvas.DestY = -Image(SelectedImageIndex).Y + Me.Height - (Wire.Y + Wire.dY)
                End If
            End If
            If Canvas.DestY > 0 Then Canvas.DestY = 0
            If Canvas.DestY < Canvas.MinY Then Canvas.DestY = Canvas.MinY
            Canvas.Animate = True
        Else
            If Canvas.LinesInBox >= 3 Then
                If Image(SelectedImageIndex).X < -Canvas.X + (Wire.X + Wire.dX) Then
                    Canvas.DestX = -Image(SelectedImageIndex).X + Wire.dX + (Wire.X + Wire.dX)
                End If
                If Image(SelectedImageIndex).X > -Canvas.X + Me.Width - (Wire.X + Wire.dX) * 2 Then
                    Canvas.DestX = -Image(SelectedImageIndex).X + Me.Width - (Wire.X + Wire.dX) * 2
                End If
            Else
                If Image(SelectedImageIndex).X < -Canvas.X Then
                    Canvas.DestX = -Image(SelectedImageIndex).X + Wire.dX
                End If
                If Image(SelectedImageIndex).X > -Canvas.X + Me.Width - (Wire.X + Wire.dX) Then
                    Canvas.DestX = -Image(SelectedImageIndex).X + Me.Width - (Wire.X + Wire.dX)
                End If
            End If
            If Canvas.DestX > 0 Then Canvas.DestX = 0
            If Canvas.DestX < Canvas.MinX Then Canvas.DestX = Canvas.MinX
            Canvas.Animate = True
        End If
    End Sub
    Private Sub ucImagesBox_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        If e.KeyCode <> Keys.Tab Then e.IsInputKey() = True
    End Sub
#End Region
#Region "-----|  Drawing"
    Public BmpMain As Bitmap = New Bitmap(10, 10), GraphicsMain As Graphics = Graphics.FromImage(BmpMain)

    Dim font_filename As New Font("Lucida Sans Unicode", 11, FontStyle.Regular, GraphicsUnit.Pixel)
    Dim font_singer As New Font("Lucida Sans Unicode", 10, FontStyle.Regular, GraphicsUnit.Pixel)
    Dim font_bold As New Font(font_filename, FontStyle.Bold)
    'Dim font_singer As New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Pixel)

    Dim TextColor As New SolidBrush(Color.Black)
    Dim b4 As New SolidBrush(Color.FromArgb(140, 0, 0, 0))

    Dim SelRectColor As New SolidBrush(Color.FromArgb(110, 255, 255, 255))
    Dim SelRectFrameColor As New Pen(Color.FromArgb(70, 0, 0, 0))

    Public BGColor As Color = Color.FromArgb(200, 200, 200)
    Public FrameSize As Short = 1
    Public DrawShadow As Boolean = True

    Private Sub Draw_FreeAreas()
        If Canvas.Y > 0 Then
            Dim a2 As New SolidBrush(Color.FromArgb(100, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B))
            Dim ii As Long = Canvas.Y
            GraphicsMain.FillRectangle(a2, 0, 0, BmpMain.Width, ii)

            Dim c As New Pen(Color.FromArgb(50, 255, 255, 255))
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

        If Canvas.X > 0 Then
            Dim a2 As New SolidBrush(Color.FromArgb(100, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B))
            Dim ii As Long = Canvas.X
            GraphicsMain.FillRectangle(a2, 0, 0, ii, BmpMain.Height)

            Dim c As New Pen(Color.FromArgb(40, 255, 255, 255))
            GraphicsMain.DrawLine(c, ii, 0, ii, BmpMain.Height)
            c = New Pen(Color.FromArgb(160, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii - 1, 0, ii - 1, BmpMain.Height)
            c = New Pen(Color.FromArgb(80, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii - 2, 0, ii - 2, BmpMain.Height)
            c = New Pen(Color.FromArgb(30, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii - 3, 0, ii - 3, BmpMain.Height)
            c = New Pen(Color.FromArgb(10, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii - 4, 0, ii - 4, BmpMain.Height)
        End If
        If Canvas.X < Canvas.MinX Then
            Dim a2 As New SolidBrush(Color.FromArgb(100, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B))
            Dim ii As Long = Canvas.MinX - Canvas.X
            GraphicsMain.FillRectangle(a2, BmpMain.Width - ii, 0, ii, BmpMain.Height)
            ii = -ii + BmpMain.Width + 3

            Dim c As New Pen(Color.FromArgb(40, 255, 255, 255))
            GraphicsMain.DrawLine(c, ii - 4, 0, ii - 4, BmpMain.Height)
            c.Color = Color.FromArgb(160, 0, 0, 0)
            GraphicsMain.DrawLine(c, ii - 3, 0, ii - 3, BmpMain.Height)
            c = New Pen(Color.FromArgb(80, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii - 2, 0, ii - 2, BmpMain.Height)
            c = New Pen(Color.FromArgb(30, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii - 1, 0, ii - 1, BmpMain.Height)
            c = New Pen(Color.FromArgb(10, 0, 0, 0))
            GraphicsMain.DrawLine(c, ii, 0, ii, BmpMain.Height)
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
                If II <> SelectedImageIndex And Image(II).Y + Canvas.Y > -Wire.Y And Image(II).Y + Canvas.Y < Me.Height And Image(II).X + Canvas.X > -Wire.X And Image(II).X + Canvas.X < Me.Width Then
                    If Image(II).Loaded Then DrawPicture(II)
                    TextColor.Color = Color.FromArgb(Image(II).Transparency * 255, TextColor.Color)
                    If Wire.Y > 40 And FileTags.Tags(Image(II).InTagsIngex).Type = "image" Then
                        If ShowImagesName Or Not Image(II).Loaded Then
                            DrawUnderImageText(II)
                        End If
                    Else
                        If FileTags.Tags(Image(II).InTagsIngex).Type = "music" Then
                            DrawSongText(II)
                        Else
                            DrawSimpleText(II)
                        End If
                    End If
                    GraphicsMain.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                End If
            Next

            If Me.Focused And I > 0 And I <= NImages Then
                Dim sm As Long = Math.Round(Wire.dY / 2 + 0.3) - 1

                x = Image(I).X + Canvas.X
                y = Image(I).Y + Canvas.Y

                SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 120, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B)
                SelRectFrameColor.Color = Color.FromArgb(Image(I).Transparency * 120, SelRectFrameColor.Color.R, SelRectFrameColor.Color.G, SelRectFrameColor.Color.B)
                If (IsMouseDown And SelectedImageIndex = ChosenObj) Or IsEnterDown Then
                    SelRectColor.Color = Color.FromArgb(SelRectColor.Color.A * 2.0, SelRectColor.Color.R, SelRectColor.Color.G, SelRectColor.Color.B)
                    SelRectFrameColor.Color = Color.FromArgb(SelRectFrameColor.Color.A * 2.0, SelRectFrameColor.Color.R, SelRectFrameColor.Color.G, SelRectFrameColor.Color.B)
                End If
                GraphicsMain.FillRectangle(SelRectColor, x - sm + 1, y - sm + 1, Image(I).WidthWithText + sm * 2 - 2, Wire.Y + sm * 2 - 2)
                GraphicsMain.DrawRectangle(SelRectFrameColor, x - sm, y - sm, Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)
            End If

            If SelectedImageIndex <> 0 Then
                II = SelectedImageIndex
                If Image(II).Loaded Then
                    DrawPicture(II)
                    TextColor.Color = Color.FromArgb(Image(II).Transparency * 255, TextColor.Color)
                    If Wire.Y > 40 And InStr(" jpg jpeg jpe gif png ico cur bmp ", " " + LCase(GetFileExtention(Image(II).FileName)) + " ") Then
                        If ShowImagesName Then DrawUnderImageText(II)
                    Else
                        If FileTags.Tags(Image(II).InTagsIngex).Type = "music" Then
                            DrawSongText(II)
                        Else
                            DrawSimpleText(II)
                        End If
                    End If
                Else
                    If Canvas.Columns <> 0 Then
                        x = Image(II).X + Canvas.X
                        y = Image(II).Y + Canvas.Y + (Wire.Y - font_filename.Size) / 2
                        GraphicsMain.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                        GraphicsMain.DrawString(Image(II).Name, font_filename, TextColor, x, y)
                        GraphicsMain.DrawString("(no thumb)", font_filename, b4, x + GraphicsMain.MeasureString(Image(II).Name, font_filename).Width, y)
                    End If
                End If
            End If
            DrawFrame(GraphicsMain, BmpMain)
            'DrawFrame(GraphicsMain, BmpMain, Me.Width, Me.Height)
        End If
    End Sub

    Private Sub CleanScreen()
        GraphicsMain.Clear(BGColor)
    End Sub
    Private Sub DrawSongText(ByVal ii As Integer)
        Dim x As Long = Image(ii).X + Canvas.X
        Dim y As Long = Image(ii).Y + Canvas.Y - 1

        If Wire.Y < 32 Then
            x += 50 + 5 'Thumbnail(ii).Width + 5
            If Not Image(ii).Loaded Then x -= 55
            If Image(ii).Singer <> "noname" Then
                GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + 2)
                GraphicsMain.DrawString("- " + Image(ii).Name, font_filename, TextColor, x + GraphicsMain.MeasureString(Image(ii).Singer, font_singer).Width, y + 2)
            Else
                GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y + 2)
            End If
        ElseIf Wire.Y = 32 Then
            x += 43 + 5 'Thumbnail(ii).Width + 5
            'GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + 4)
            'GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y + 16)

            GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + 17)
            GraphicsMain.DrawString(Image(ii).Name, font_bold, TextColor, x, y + 5)
        ElseIf Wire.Y < 60 Then
            x += 43 + 5 'Thumbnail(ii).Width + 5
            'GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + Wire.Y / 2 + 4 - 16)
            'GraphicsMain.DrawString(Image(ii).Name, font_bold, TextColor, x, y + Wire.Y / 2)
            GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + Wire.Y / 2)
            GraphicsMain.DrawString(Image(ii).Name, font_bold, TextColor, x, y + Wire.Y / 2 + 4 - 16)
        Else
            Dim fnt As New Font(font_filename, FontStyle.Bold)
            GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, _
            x + (Wire.X - GraphicsMain.MeasureString(Image(ii).Singer, font_singer).Width) / 2, y + Wire.Y / 2 + 4 + 12)
            GraphicsMain.DrawString(Image(ii).Name, fnt, TextColor, _
            x + (Wire.X - GraphicsMain.MeasureString(Image(ii).Name, fnt).Width) / 2, y + Wire.Y / 2 + 4)
        End If
    End Sub
    Private Sub DrawSimpleText(ByVal ii As Integer)
        Dim x As Long
        If Image(ii).Loaded Then
            x = Math.Max(Image(ii).X + Image(ii).Width + 4 + Canvas.X, Image(ii).X + 16 + 4 + Canvas.X)
            If Wire.Y < 40 Then
                x = Image(ii).X + Wire.Y + 4 + Canvas.X
            End If
        Else
            x = Image(ii).X + Canvas.X
        End If
        Dim y As Long = Image(ii).Y + Canvas.Y + Math.Floor((Wire.Y - font_filename.Size) / 2) - 1 '- font_filename.Size * 0.2

        'GraphicsMain.FillRectangle(Brushes.White, x, y, 100, font_filename.Size)
        GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y)
    End Sub
    Private Sub DrawUnderImageText(ByVal ii As Integer)
        Dim x As Long = Image(ii).X + (Wire.X - GraphicsMain.MeasureString(Trim(Image(ii).Name), font_filename).Width) / 2 + Canvas.X
        Dim y As Long = Image(ii).Y + Wire.Y / 2 + Image(ii).Height / 2 + Canvas.Y - 3

        GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y)
        If Not Image(ii).Loaded Then
            GraphicsMain.DrawString("loading...", font_filename, Brushes.Gray, x, y - font_filename.Size - 5)
        End If
    End Sub
    Private Sub DrawSelections()
        Dim x, y As Long
        Dim sm As Long = Math.Round(Wire.dY / 2 + 0.3) - 1
        Dim p1 As New Pen(Color.FromArgb(20, 100, 120, 205))
        Dim b1 As SolidBrush = Brushes.GreenYellow

        For i As Long = 1 To NImages
            If (Image(i).Selected = True Or Image(i).WillBeSelected = True) And Image(i).WillBeUnSelected = False And Image(i).Y + Canvas.Y > -Wire.Y And Image(i).Y + Canvas.Y < Me.Height Then
                x = Image(i).X + Canvas.X
                y = Image(i).Y + Canvas.Y

                p1.Color = Color.FromArgb(255 * Image(i).Transparency, 50, 70, 155)
                GraphicsMain.DrawRectangle(p1, x - sm, y - sm, Image(i).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)

                b1.Color = Color.FromArgb(255 * Image(i).Transparency, 150, 170, 255)
                GraphicsMain.FillRectangle(b1, x - sm + 1, y - sm + 1, Image(i).WidthWithText + sm * 2 - 2, Wire.Y + sm * 2 - 2)
            End If
        Next
        'Dim b1 As SolidBrush = Brushes.GreenYellow
        'For i As Long = 1 To NImages
        '    If (Image(i).Selected = True Or Image(i).WillBeSelected = True) And Image(i).WillBeUnSelected = False And Image(i).Y + Canvas.Y > -Wire.Y And Image(i).Y + Canvas.Y < Me.Height Then
        '        x = Image(i).X + Canvas.X
        '        y = Image(i).Y + Canvas.Y

        '        b1.Color = Color.FromArgb(255 * Image(i).Transparency, 150, 170, 255)
        '        GraphicsMain.FillRectangle(b1, x - sm + 1, y - sm + 1, Image(i).WidthWithText + sm * 2 - 2, Wire.Y + sm * 2 - 2)
        '        'GraphicsMain.FillPie(b1, x - 6, y - sm, 15, 15, 180, 90)
        '    End If
        'Next
    End Sub
    Private Sub DrawPicture(ByVal I As Integer)
        Dim p As Point
        Dim w As Long = Wire.Border

        If Wire.Y < 40 Or FileTags.Tags(Image(I).InTagsIngex).Type <> "image" Then
            p.Y = Math.Round(Image(I).Y + Canvas.Y) + Math.Round((Wire.Y - Image(I).Height) / 2)
            p.X = Math.Round(Image(I).X + Canvas.X)
        Else
            p.Y = Math.Round((Wire.Y - Image(I).Height) / 2) + Math.Round(Image(I).Y + Canvas.Y)
            p.X = Math.Round((Wire.X - Image(I).Width) / 2) + Math.Round(Image(I).X + Canvas.X)
            If ShowImagesName Then
                p.Y = Math.Round((Wire.Y - Image(I).Height - font_filename.Size + 2 - w) / 2) + Math.Round(Image(I).Y + Canvas.Y)
            End If
        End If

        If FileTags.Tags(Image(I).InTagsIngex).Type = "music" And Wire.Y >= 60 Then
            p.Y -= Image(I).DestHeight - 18
            p.X += Wire.X / 2 - Image(I).DestWidth / 2
        End If

        If FileTags.Tags(Image(I).InTagsIngex).Type = "image" Then
            If Wire.Y < 40 Then
                p.X = Math.Round((Wire.Y - Image(I).Width)) + Math.Round(Image(I).X + Canvas.X)
            End If
            If DrawShadow Then
                Dim width, height As Long
                Dim smx As Short = 0
                Dim smy As Short = 0
                width = Image(I).Width + w * 2 + 1
                height = Image(I).Height + w * 2 + 1
                Dim a1 As New Pen(Color.FromArgb(100 * Image(I).Transparency, 0, 0, 0))
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
                Dim pen1 As New Pen(Color.FromArgb(255 * Image(I).Transparency, 255, 255, 255))
                Dim r As New Rectangle(p.X - w, p.Y - w, Image(I).Width + w * 2 - 1, Image(I).Height + w * 2 - 1)
                GraphicsMain.DrawRectangle(pen1, r)
                w = w - 1
            End While
        End If

        If Image(I).Transparency < 1 Then
            Dim att As New Drawing.Imaging.ImageAttributes()
            Dim cm As Drawing.Imaging.ColorMatrix = New Drawing.Imaging.ColorMatrix(New Single()() _
                       {New Single() {1, 0, 0, 0, 0}, _
                        New Single() {0, 1, 0, 0, 0}, _
                        New Single() {0, 0, 1, 0, 0}, _
                        New Single() {0, 0, 0, Image(I).Transparency, 0}, _
                        New Single() {0, 0, 0, 0, 1}})
            att.SetColorMatrix(cm)
            Dim r1 As New Rectangle(p.X, p.Y, Image(I).Width, Image(I).Height)
            GraphicsMain.DrawImage(Thumbnail(I), r1, 0, 0, Thumbnail(I).Width, Thumbnail(I).Height, GraphicsUnit.Pixel, att)
        Else
            With Image(I)
                If .Width = Thumbnail(I).Width And .Height = Thumbnail(I).Height Then
                    GraphicsMain.DrawImageUnscaled(Thumbnail(I), p)
                Else
                    Dim r As New Rectangle(p.X, p.Y, .Width, .Height)
                    GraphicsMain.DrawImage(Thumbnail(I), r)
                End If
            End With
        End If
    End Sub
#End Region

    Private Sub ucImagesBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DoResizeEvent = True

        BmpMain = New Bitmap(Me.Width, Me.Height)
        GraphicsMain = Graphics.FromImage(BmpMain)
        InitButtons()
        NextFrame(True)

        picMain.Left = 0
        picMain.Top = 0
    End Sub

    Public DoResizeEvent As Boolean = False

    Private Sub ucImagesBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If DoResizeEvent Then
            BmpMain = New Bitmap(Me.Width, Me.Height)
            GraphicsMain = Graphics.FromImage(BmpMain)

            RecalculateCanvasParameters()

            'Draw_Picturies()
            NextFrame(True)
            picMain.Image = BmpMain
            picMain.Refresh()
            'Me.BackgroundImage = BmpMain
            Me.Refresh()
        End If
    End Sub

    Public Sub NextFrame(ByRef DrawInAnyWay)
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
            Canvas.VX = 0
            Canvas.X = Canvas.X + (Canvas.DestX - Canvas.X) / 6
            If (Canvas.DestY = Math.Round(Canvas.Y) And Canvas.DestX = Math.Round(Canvas.X)) Or Animation = False Then
                Canvas.X = Canvas.DestX
                Canvas.Y = Canvas.DestY
                Canvas.Animate = False
            End If
        Else
            If Not (IsMouseDown And (ChosenObj = 0 Or ChosenObj = -3)) Then
                If Animation = False Then
                    Canvas.VX = 0 : Canvas.V = 0
                Else
                    Canvas.Y += Canvas.V : Canvas.X += Canvas.VX
                End If

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

                If Canvas.X > 0 Then
                    Canvas.X -= Canvas.X / 8
                    Canvas.VX = Canvas.VX * 0.7
                Else
                    If Canvas.X < Canvas.MinX Then
                        Canvas.X -= (Canvas.X - Canvas.MinX) / 8
                        If (-Canvas.X + Canvas.MinX) < 0.4 Then Canvas.X = Canvas.MinX
                        Canvas.VX = Canvas.VX * 0.7
                    Else
                        Canvas.VX = Canvas.VX * 0.9
                    End If
                End If
                If Math.Abs(Canvas.VX) < 0.2 Then Canvas.VX = 0
                If Math.Abs(Canvas.X) < 0.2 Then Canvas.X = 0
            End If
        End If

        Dim proc As Double = 0.22
        If IsAnimatedImages Then
            IsAnimatedImages = False
            For I As Long = 1 To NImages
                If Image(I).Animate = True Then
                    With Image(I)
                        If Math.Abs(.X - .DestX) < 0.4 And Math.Abs(.Y - .DestY) < 0.4 Then
                            .X = Math.Round(.DestX) : .Y = Math.Round(.DestY)
                            If Math.Abs(.Transparency - .DestTransparency) < 0.05 And Math.Abs(.Width - .DestWidth) < 0.4 And Math.Abs(.Height - .DestHeight) < 0.4 Then .Animate = False
                        Else
                            .X += (.DestX - .X) * proc : .Y += (.DestY - .Y) * proc
                            IsAnimatedImages = True
                        End If

                        If Math.Abs(.Transparency - .DestTransparency) < 0.05 Then
                            .Transparency = .DestTransparency
                        Else
                            .Transparency += (.DestTransparency - .Transparency) * proc
                            If .Transparency > 1 Then .Transparency = 1
                            IsAnimatedImages = True
                        End If

                        If Math.Abs(.Width - .DestWidth) < 0.4 And Math.Abs(.Height - .DestHeight) < 0.4 Then
                            .Width = Math.Round(.DestWidth) : .Height = Math.Round(.DestHeight)
                        Else
                            .Width += (.DestWidth - .Width) * proc : .Height += (.DestHeight - .Height) * proc
                            IsAnimatedImages = True
                        End If
                    End With
                End If
            Next
        End If

        If (DrawInAnyWay Or IsSmthDragging Or IsMouseDown Or Canvas.V <> 0 Or Canvas.VX <> 0 Or IsAnimatedImages = True Or Canvas.Y > 0 Or Canvas.Y < Canvas.MinY Or Canvas.X > 0 Or Canvas.X < Canvas.MinX Or Canvas.Animate) Then
            Draw_Picturies()

            If ChosenObj = -1111 Then
                GraphicsMain.DrawRectangle(Pens.DarkBlue, Math.Min(ClF.X, Cl.X), Math.Min(ClF.Y, Cl.Y), Math.Abs(Cl.X - ClF.X), Math.Abs(Cl.Y - ClF.Y))
                Dim b As New SolidBrush(Color.FromArgb(50, Color.Blue))
                GraphicsMain.FillRectangle(b, Math.Min(ClF.X, Cl.X), Math.Min(ClF.Y, Cl.Y), Math.Abs(Cl.X - ClF.X), Math.Abs(Cl.Y - ClF.Y))
            End If

            If ShowFramesPerSecond Then
                counter += 1
                If counter >= 5 Then
                    counter = 0
                    time2 = System.DateTimeOffset.Now
                    time_delta = time2 - time1
                    time1 = time2
                End If
                GraphicsMain.DrawString((Math.Round(1 / time_delta.TotalSeconds * 5000) / 1000).ToString + " fps", font_filename, Brushes.White, BmpMain.Width - 100, BmpMain.Height - 15)
            End If

            If Me.Focused Then
                GraphicsMain.DrawString("focused", font_filename, Brushes.LightGray, BmpMain.Width - 150, BmpMain.Height - 15)
            End If
            GraphicsMain.DrawString(ChosenObj.ToString, font_filename, Brushes.White, BmpMain.Width - 150, BmpMain.Height - 15 - 10)

            If Canvas.MinY <> 0 Then
                Buttons(2).Y = (Canvas.Y / Canvas.MinY) * (BmpMain.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) + 3 + Buttons(3).Image.Height
                Buttons(2).X = BmpMain.Width - Buttons(2).Image.Width - 2
                Buttons(3).X = Buttons(2).X : Buttons(4).X = Buttons(2).X
                Buttons(3).Y = 2 : Buttons(4).Y = BmpMain.Height - Buttons(4).Image.Height - 2

                Buttons(2).Visible = True : Buttons(3).Visible = True : Buttons(4).Visible = True
            Else
                Buttons(2).Visible = False : Buttons(3).Visible = False : Buttons(4).Visible = False
            End If

            For I As Long = 0 To Buttons.Length - 1
                With Buttons(I)
                    If .Visible = True Then
                        GraphicsMain.DrawImageUnscaled(.Image, .X, .Y)
                    End If
                End With
            Next

            If IsSmthDragging Then
                GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                Dim b As New SolidBrush(Color.FromArgb(150, 120, 120, 120))
                Dim w As Long = GraphicsMain.MeasureString(DraggingCount, font_filename).Width - 1
                If w < 10 Then w = 10
                Dim x = MousePosition.X - Me.Left - Me.Parent.Left
                Dim y = MousePosition.Y - Me.Top - Me.Parent.Top
                GraphicsMain.FillEllipse(b, x - 17, y - 17, 10 + w, 20)
                GraphicsMain.DrawEllipse(Pens.Black, x - 17, y - 17, 10 + w, 20)
                GraphicsMain.DrawString(DraggingCount.ToString, font_filename, Brushes.Black, x - 12, y - 13)
                GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.None
            End If
            'If IsSmthDragging Then
            '    Dim p As Point = MousePosition
            '    p.X = p.X - Me.Parent.Left - Me.Left - 6
            '    p.Y = p.Y - Me.Parent.Top - Me.Top - 12
            '    GraphicsMain.DrawString("file", fnt, Brushes.White, p)
            'End If
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
        If CurrentLoadingImgIndex < NImages And StopLoading <> True Then
            DrawStatus()
        End If
    End Sub
    Private Sub DrawStatus()
        Dim str As String = CurrentLoadingImgIndex.ToString + " | " + NImages.ToString
        Dim b As New SolidBrush(BGColor)
        Dim b2 As New SolidBrush(Color.FromArgb(240, 240, 240))
        GraphicsMain.FillRectangle(b, 20 - 1, BmpMain.Height - 34, GraphicsMain.MeasureString(str, font_filename).Width + 2, 12)
        Dim p As New Pen(Color.FromArgb(120, BGColor))
        GraphicsMain.DrawRectangle(p, 20 - 2, BmpMain.Height - 35, GraphicsMain.MeasureString(str, font_filename).Width + 3, 13)

        GraphicsMain.DrawString(str, font_filename, b2, 20, BmpMain.Height - 35 + 1)
        GraphicsMain.DrawString(str, font_filename, Brushes.Black, 20, BmpMain.Height - 35)
        GraphicsMain.DrawImageUnscaled(GenerateStatusBar(300, CurrentLoadingImgIndex / NImages), 2, BmpMain.Height - 19)
    End Sub
    Private Sub FindWidthWithText_NoReduce(ByVal i As Long)
        Image(i).Name = IO.Path.GetFileName(Image(i).FileName)
        Dim w As Long = 0
        If Image(i).Loaded Then w = Thumbnail(i).Width

        If FileTags.Tags(Image(i).InTagsIngex).Type = "music" Then
            Image(i).Name = FileTags.Tags(Image(i).InTagsIngex).Song.Name
            Image(i).Singer = FileTags.Tags(Image(i).InTagsIngex).Song.Singer
            If Wire.Y >= 32 And Wire.Y < 60 Then
                Image(i).WidthWithText = GraphicsMain.MeasureString(Image(i).Singer, font_singer).Width
                Image(i).WidthWithText = Math.Max(Image(i).WidthWithText, GraphicsMain.MeasureString(Image(i).Name, font_bold).Width)
                Image(i).WidthWithText += w + 5
            ElseIf Wire.Y < 32 Then
                If Image(i).Singer <> "noname" Then
                    Image(i).WidthWithText = GraphicsMain.MeasureString("- " + Image(i).Name, font_filename).Width
                    Image(i).WidthWithText += GraphicsMain.MeasureString(Image(i).Singer, font_singer).Width + 3
                    Image(i).WidthWithText += w + 5
                Else
                    Image(i).WidthWithText = GraphicsMain.MeasureString(Image(i).Name, font_filename).Width + 3
                    Image(i).WidthWithText += w + 5
                End If
            Else
                Image(i).WidthWithText = Wire.X
            End If
        Else
            Image(i).WidthWithText = GraphicsMain.MeasureString(Image(i).Name, font_filename).Width
            Image(i).WidthWithText += w + 5
        End If

        If Wire.Y > 40 And FileTags.Tags(Image(i).InTagsIngex).Type = "image" Then
            Image(i).WidthWithText = Wire.X 'Thumbnail(I).Width 
        End If

    End Sub
    Private Sub FindWidthWithText(ByVal I As Long)
        Image(I).Name = IO.Path.GetFileName(Image(I).FileName)
        Dim w As Long = 0
        If Image(I).Loaded Then w = Thumbnail(I).Width

        If FileTags.Tags(Image(I).InTagsIngex).Type = "music" Then
            Image(I).Name = FileTags.Tags(Image(I).InTagsIngex).Song.Name
            Image(I).Singer = FileTags.Tags(Image(I).InTagsIngex).Song.Singer
            'MsgBox(I.ToString)
            If Wire.Y >= 32 And Wire.Y < 60 Then
                Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_bold).Width
                While Image(I).WidthWithText > Wire.X - w - 5 And Image(I).Name.Length > 3
                    Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_bold).Width
                End While
                Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width
                While Image(I).WidthWithText > Wire.X - w - 5 And Image(I).Singer.Length > 3
                    Image(I).Singer = Mid(Image(I).Singer, 1, Image(I).Singer.Length - 4) + "..."
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width
                End While
                Image(I).WidthWithText = Math.Max(Image(I).WidthWithText, GraphicsMain.MeasureString(Image(I).Name, font_bold).Width)
                Image(I).WidthWithText += w + 5
            ElseIf Wire.Y < 32 Then
                If Image(I).Singer <> "noname" Then
                    Image(I).WidthWithText = GraphicsMain.MeasureString("- " + Image(I).Name, font_filename).Width
                    Image(I).WidthWithText += GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width + 3
                    While Image(I).WidthWithText > Wire.X - w - 5 And Image(I).Name.Length > 11
                        Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."

                        Image(I).WidthWithText = GraphicsMain.MeasureString("- " + Image(I).Name, font_filename).Width
                        Image(I).WidthWithText += GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width + 3
                    End While
                    While Image(I).WidthWithText > Wire.X - w - 5 And Image(I).Singer.Length > 7
                        Image(I).Singer = Mid(Image(I).Singer, 1, Image(I).Singer.Length - 4) + "..."

                        Image(I).WidthWithText = GraphicsMain.MeasureString("- " + Image(I).Name, font_filename).Width
                        Image(I).WidthWithText += GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width + 3
                    End While
                    Image(I).WidthWithText += w + 5
                Else
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width + 3
                    While Image(I).WidthWithText > Wire.X - w - 5 And Image(I).Name.Length > 11
                        Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                        Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width + 3
                    End While
                    Image(I).WidthWithText += w + 5
                End If
            Else
                Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_bold).Width
                While Image(I).WidthWithText > Wire.X And Image(I).Name.Length > 3
                    Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_bold).Width
                End While
                Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width
                While Image(I).WidthWithText > Wire.X And Image(I).Singer.Length > 3
                    Image(I).Singer = Mid(Image(I).Singer, 1, Image(I).Singer.Length - 4) + "..."
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width
                End While
                Image(I).WidthWithText = Wire.X
            End If
        Else
            Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width
            If Wire.Y < 40 Or FileTags.Tags(Image(I).InTagsIngex).Type <> "image" Then
                While Image(I).WidthWithText > Wire.X - w - 5 And Image(I).Name.Length > 3
                    Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width
                End While
            Else
                While Image(I).WidthWithText > Wire.X And Image(I).Name.Length > 3
                    Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                    Image(I).WidthWithText = GraphicsMain.MeasureString(Image(I).Name, font_filename).Width
                End While
            End If
            If w < Wire.Y And FileTags.Tags(Image(I).InTagsIngex).Type = "image" Then w = Wire.Y
            Image(I).WidthWithText += w + 5
        End If

        If Wire.Y > 40 And FileTags.Tags(Image(I).InTagsIngex).Type = "image" Then
            Image(I).WidthWithText = Wire.X 'Thumbnail(I).Width 
        End If
    End Sub

    Dim TimeFromTyping As DateTimeOffset
    Private Sub tmrAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAnimation.Tick
        Dim td As TimeSpan, td2 As DateTimeOffset
        td2 = System.DateTimeOffset.Now
        td = td2 - TimeFromTyping
        If (td.TotalMilliseconds) > 1400 Then
            If InputLine <> "" Then InputLine = ""
        End If

        If ReloadWidthWithText Then
            For i As Long = 1 To NImages
                FindWidthWithText(i)
            Next
            ReloadWidthWithText = False
        End If
        If StopLoading = True Then
            CurrentLoadingImgIndex = NImages + 1
        Else
            If IsNotEverithingLoaded Then
                If Not bwLoadOne.IsBusy Then
                    If CurrentLoadingImgIndex >= 1 And CurrentLoadingImgIndex <= NImages Then
                        If Image(CurrentLoadingImgIndex).Loading = True Then
                            Dim I As Long = CurrentLoadingImgIndex

                            If Not Image(I).Loaded Then
                                Image(I).InTagsIngex = FileTags.FindByFileName(Image(I).FileName)
                                If Image(I).InTagsIngex = 0 Then Image(I).InTagsIngex = FileTags.Add(Image(I).FileName)
                            End If

                            If Wire.Y >= 32 Then
                                For d As Long = 1 To FileTags.Tags(Image(I).InTagsIngex).Rating
                                    Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                                        graf.DrawImage(bmp_star, 44 - d * 13, 32 - 13)
                                    End Using
                                Next
                            Else
                                For d As Long = 1 To FileTags.Tags(Image(I).InTagsIngex).Rating
                                    Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                                        graf.DrawImage(bmp_star, LoadedThumbnail.Width - d * 13 - 2 - 4, Wire.Y - 15)
                                    End Using
                                Next
                            End If

                            Thumbnail(I) = LoadedThumbnail

                            
                            If Image(I).Loaded = False Then
                                If Wire.Y >= 40 Then Image(I).Y = Image(I).Y - Thumbnail(I).Height : Image(I).Transparency = 0 Else Image(I).X = Image(I).X - Thumbnail(I).Width - 5 : Image(I).Transparency = 0
                                Image(I).Transparency = 0
                                Image(I).Height = Thumbnail(I).Height
                                Image(I).Width = Thumbnail(I).Width
                            End If
                            If FlyMode Then
                                'Image(I).X = startX
                                'Image(I).Y = startY
                            End If
                            Image(I).DestTransparency = 1
                            Image(I).DestHeight = Thumbnail(I).Height
                            Image(I).DestWidth = Thumbnail(I).Width
                            Image(I).Animate = True
                            IsAnimatedImages = True

                            Image(I).Loaded = True
                            Image(I).Loading = False
                            Image(I).ReLoaded = True

                            FindWidthWithText(I)

                            'IsAnimatedImages = True
                        End If
                    End If

                    Dim f As Boolean = False
                    For i As Long = 1 To NImages
                        If Image(i).Loaded = False Or Image(i).ReLoaded = False Then
                            CurrentLoadingImgIndex = i
                            CurrentLoadingFileName = Image(i).FileName
                            Image(i).Loading = True
                            'Image(i).DestTransparency = 0
                            'Image(i).Animate = True
                            bwLoadOne.RunWorkerAsync()
                            f = True
                            Exit For
                        End If
                    Next
                    If f = False Then
                        IsNotEverithingLoaded = False

                        If Mid(Path, 1, Path.Length - 2) = Application.StartupPath + "\35photo" Then
                            Dim i As Long = 0
                            FileOpen(1, Path + "urls.txt", OpenMode.Input)
                            While Not EOF(1)
                                i += 1
                                URLs35photo(i) = LineInput(1)
                            End While
                            FileClose(1)
                        End If
                        OrderImages()
                        NextFrame(True)
                    End If
                End If
            End If
        End If

        NextFrame(False)

        Dim l As Long = 0
        If td.TotalMilliseconds < 1400 Then
            If td.TotalMilliseconds > 600 Then
                l = 255 - (td.TotalMilliseconds - 600) / 800 * 255
            Else
                l = 255
            End If
        Else
            If td.TotalMilliseconds < 5000 Then
                NextFrame(True)
                TimeFromTyping -= td + td
            End If
        End If
        Dim br As New SolidBrush(Color.FromArgb(l, 220, 220, 220))
        Dim br2 As New SolidBrush(Color.FromArgb(70, 70, 70))
        If Mid(InputLine, 1, 1) = "А" Or InputLine = "АRENAME" Then
            Dim s As String = "command: " + Mid(InputLine, 2)
            GraphicsMain.FillRectangle(br2, 80, 3, GraphicsMain.MeasureString(s, font_filename).Width, 13)
            GraphicsMain.DrawRectangle(Pens.White, 80, 3, GraphicsMain.MeasureString(s, font_filename).Width, 13)
            GraphicsMain.DrawString(s, font_filename, br, 80, 3)
        Else
            GraphicsMain.FillRectangle(br2, 80, 3, GraphicsMain.MeasureString(InputLine, font_filename).Width, 13)
            GraphicsMain.DrawRectangle(Pens.White, 80, 3, GraphicsMain.MeasureString(InputLine, font_filename).Width, 13)
            GraphicsMain.DrawString(InputLine, font_filename, br, 80, 3)
        End If
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
        NextFrame(True)
    End Sub
    Private Sub picMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMain.LostFocus
        If IsMouseDown And ChosenObj > 0 And ChosenObj <= NImages Then
            StartDragDrop(ChosenObj)
        End If
    End Sub
    Private Sub ucImagesBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        NextFrame(True)
    End Sub

    Private Sub picMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picMain.Click

    End Sub

    Private Sub picForDrag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picForDrag.Click

    End Sub

    Private Sub picForDrag_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picForDrag.MouseUp

    End Sub
End Class
