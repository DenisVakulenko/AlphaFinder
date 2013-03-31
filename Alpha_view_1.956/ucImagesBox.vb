'Option Strict On
Imports System.Threading.Tasks

Public Class ucImagesBox
    Public Sub New()
        InitializeComponent()
        Wire.X = 200
        Wire.Y = 16
        Wire.dX = 10
        Wire.dY = 5
    End Sub

    Enum NameLocations As Short
        Right = 0
        Under = 1
    End Enum
    Enum PlayerStates As Short
        Stopped = 0
        Pause = 1
        Playing = 2
    End Enum
    Enum FlyingAlgorithms As Short
        Simple = 0
        NotSimple = 1
    End Enum
    Enum ArrangmentTypes As Short
        No = 0
        Õorizontal = 1
        Vertical = 2
    End Enum
    Enum Sortings As Short
        User = 0
        ByName = 1
        ByType = 2
        ByDate = 3
        By = 4
    End Enum
    Enum FileTypes As Short
        NotDefined = -1
        File = 0
        Image = 1
        Music = 2
        ExeFile = 3
        Folder = 4
        PhotoAlbum = 5
        MusicAlbum = 6
        Discography = 7
        Pile = 8
        Drive = 9
        Usb = 10
        FilmsAlboom = 11
        Command = 12
    End Enum

    Event BackSpaceKey(ByVal TurnOffTheButtonAutomatically As Boolean)
    Event GoForvard(ByVal TurnOffTheButtonAutomatically As Boolean)
    Event FillScreenMe()
    Event SendFocusToTheTop(ByRef Done As Boolean)
    Event ChangeDir(ByVal Path As String, ByVal a As Boolean)
    Event FileClick(ByVal Image As ImageStruct)
    Event NeedToRefine()

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
        Dim min_dY As Short
        Dim dX As Short
        Dim dY As Short
        Dim ExtendSpaces As Boolean
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
        Dim Animate As Boolean

        Dim Singer, Name, FileName As String
        Dim OriginalName, toshowArtist As String
        Dim InTagsIngex As Long
        Dim Type As FileTypes

        Dim Loaded, Loading, ReLoaded As Boolean
        Dim Transparency, DestTransparency As Double
        Dim Selected, PreSelected As Boolean

        Dim ImagePosX, ImagePosY As Double
        'Dim Visible As Boolean
    End Structure
#End Region

    Public InPlayerFileName As String = ""
    Public PlayerState As PlayerStates = PlayerStates.Stopped

    Dim BmpFolder, BmpLoading, BmpDrive, BmpUsb, BmpSongs, BmpFilms, BmpMkDir, BmpAlboom, BmpDesktop As Bitmap

    Public DrawContrastFrames As Boolean = False '!!!!!!!!!!

    Public SelectionMode As Boolean = False

    Dim time1, time2 As DateTimeOffset, time_delta As TimeSpan
    Dim ShowFPS As Boolean = True, counter As Short

    Dim SelectedAngleOffset As Double

    Public SelectedImageIndex As Long = 0
    Public ShowImagesName As Boolean = False

    Private VScrolling As Boolean ' true - vertical; false - gorizontal
    Public IsNotEverithingLoaded As Boolean = False
    Dim URLs35photo(40) As String
    Public Path As String = ""
    Public Sorting As String = "by name"
    Public Animation As Boolean = True

    Public FlyMode As Boolean = False

    Dim startX, startY As Long

    Public Arrangement As ArrangmentTypes = ArrangmentTypes.Vertical
    Public NameLocation As NameLocations = NameLocations.Right
    Public FlyingAlgorithm As FlyingAlgorithms = FlyingAlgorithms.Simple
#Region "-------|  BUTTONS"
    Dim Buttons(1) As ButtonStruct
    Public Sub InitButtons()
        Try
            ReDim Buttons(13)
            'Buttons(6).X = 2
            'Buttons(6).Y = 2
            'Buttons(6).Image = New Bitmap(Application.StartupPath + "\order.png")
            Buttons(6).Visible = False

            Dim k As Long = 28
            'Buttons(0).X = 2
            'Buttons(0).Y = 2
            'Buttons(0).Image = New Bitmap(Application.StartupPath + "\sorting\name.png")
            Buttons(0).Visible = False
            'Buttons(1).X = 2
            'Buttons(1).Y = k * 1 + 4
            'Buttons(1).Image = New Bitmap(Application.StartupPath + "\sorting\rating.png")
            Buttons(1).Visible = False
            'Buttons(7).X = 2
            'Buttons(7).Y = k * 2 + 4
            'Buttons(7).Image = New Bitmap(Application.StartupPath + "\sorting\singer.png")
            Buttons(7).Visible = False
            'Buttons(8).X = 2
            'Buttons(8).Y = k * 3 + 4
            'Buttons(8).Image = New Bitmap(Application.StartupPath + "\sorting\pop.png")
            Buttons(8).Visible = False
            'Buttons(12).X = 2
            'Buttons(12).Y = k * 4 + 4
            'Buttons(12).Image = New Bitmap(Application.StartupPath + "\sorting\type.png")
            Buttons(12).Visible = False
            'Buttons(13).X = 2
            'Buttons(13).Y = k * 5 + 4
            'Buttons(13).Image = New Bitmap(Application.StartupPath + "\sorting\rand.png")
            Buttons(13).Visible = False


            'Buttons(9).X = 2
            'Buttons(9).Y = 19 * 3 + 2 + 5 + 19
            'Buttons(9).Image = New Bitmap(Application.StartupPath + "\rate1.bmp")
            Buttons(9).Visible = False
            'Buttons(10).X = 2
            'Buttons(10).Y = 19 * 3 + 2 + 5 + 19 * 2
            'Buttons(10).Image = New Bitmap(Application.StartupPath + "\rate2.bmp")
            Buttons(10).Visible = False
            'Buttons(11).X = 2
            'Buttons(11).Y = 19 * 3 + 2 + 5 + 19 * 3
            'Buttons(11).Image = New Bitmap(Application.StartupPath + "\rate3.bmp")
            Buttons(11).Visible = False

            'Buttons(5).X = 2
            'Buttons(5).Y = 2
            'Buttons(5).Image = New Bitmap(Application.StartupPath + "\sorting\main.png")
            Buttons(5).Visible = False 'True

            Buttons(2).X = 55
            Buttons(2).Y = 17
            Buttons(2).Image = New Bitmap(Application.StartupPath + "\scroller\main_rounded.png")
            Buttons(2).Visible = False

            Buttons(3).X = 55
            Buttons(3).Y = 17
            Buttons(3).Image = New Bitmap(Application.StartupPath + "\scroller\up2.png")
            Buttons(3).Visible = False
            Buttons(4).X = 55
            Buttons(4).Y = 17
            Buttons(4).Image = New Bitmap(Application.StartupPath + "\scroller\down2.png")
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
        Buttons(13).Visible = True
    End Sub
    Private Sub HideSortingVariants()
        Buttons(5).Visible = False 'True()

        Buttons(0).Visible = False
        Buttons(1).Visible = False
        Buttons(7).Visible = False
        Buttons(8).Visible = False
        Buttons(12).Visible = False
        Buttons(13).Visible = False
    End Sub
#End Region
#Region "--------------------------|  UI IMAGES             |-------------------------- GOOD  "
    'Dim BmpScrollThing, BmpScrollThingMD, BmpScrollUp, BmpScrollDown As Bitmap
    Dim bmp_error, bmp_star, bmpPlus, icoImg16, icoImg32, icoMusic32, icoMusic16, icoFolder16, icoFolder32 As Bitmap
    Public Sub LoadUIImages()
        bmp_error = New Bitmap(Application.StartupPath + "\err.png")
        bmp_star = New Bitmap(Application.StartupPath + "\star_gray_2.png")
        bmpPlus = New Bitmap(Application.StartupPath + "\plus2.png")
        icoMusic32 = New Bitmap(Application.StartupPath + "\muz_new.png")
        icoMusic16 = New Bitmap(50, 16) 'Application.StartupPath + "\muz16.png")
        icoFolder16 = New Bitmap(Application.StartupPath + "\dir3_16.png")
        icoFolder32 = New Bitmap(Application.StartupPath + "\dir2_32.png")
        icoImg16 = New Bitmap(Application.StartupPath + "\ico_img_16.png")
        icoImg32 = New Bitmap(Application.StartupPath + "\ico_img_32.png")
        Using g As Graphics = Graphics.FromImage(icoMusic16)
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

    Public Sub CalculateCanvasHeight()
        If Canvas.Columns > 0 Then
            If Arrangement = ArrangmentTypes.Õorizontal Then
                Canvas.Height = (Math.Truncate((NImages) / Canvas.Columns) + 1) * (Wire.Y + Wire.dY) + Wire.dY
                Canvas.Height += (Wire.Y + Wire.dY)
            Else
                Canvas.Height = Me.Height
            End If
        Else
            Canvas.Height = 0 'MsgBox("really!")
        End If
    End Sub
    Public Sub CalculateCanvasWidth()
        If Canvas.Columns > 0 Then
            If Arrangement <> ArrangmentTypes.Õorizontal Then
                Canvas.Width = (Math.Truncate((NImages) / Canvas.Columns) + 1) * (Wire.X + Wire.dX) + Wire.dX
            Else
                Canvas.Width = Me.Width
            End If
        Else
            Canvas.Width = 0 'MsgBox("really!")
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
    Public Sub CalculateLinesAndColumns()
        Dim SBIndent As Short = 0
        If False Then SBIndent = Buttons(2).Image.Width
        If Arrangement = ArrangmentTypes.Õorizontal Then
            'Canvas.DestX = 0
            Canvas.LinesInBox = Math.Truncate(BmpMain.Height / (Wire.Y + Wire.dY))
            Canvas.Columns = Math.Truncate((BmpMain.Width - Wire.min_dX - SBIndent) / (Wire.X + Wire.min_dX))
            If Canvas.Columns < 1 Then Canvas.Columns = 1
            If Wire.ExtendSpaces Then
                Wire.dX = (BmpMain.Width - Canvas.Columns * Wire.X - SBIndent) / (Canvas.Columns + 1)
            Else
                Wire.dX = Wire.min_dX
            End If
            Canvas.Plus = Math.Truncate((BmpMain.Width - ((Wire.X + Wire.dX) * Canvas.Columns - Wire.dX) - SBIndent) / 2)
        Else
            'Canvas.DestY = 0
            Canvas.LinesInBox = Math.Truncate(BmpMain.Width / (Wire.X + Wire.dX))
            Canvas.Columns = Math.Truncate((BmpMain.Height - Wire.min_dY - SBIndent) / (Wire.Y + Wire.min_dY))
            If Canvas.Columns < 1 Then Canvas.Columns = 1
            If Wire.ExtendSpaces Then
                Wire.dY = (BmpMain.Height - Wire.min_dY - Canvas.Columns * Wire.Y) / (Canvas.Columns + 1)
            Else
                Wire.dY = Wire.min_dY
            End If
            Canvas.Plus = Math.Truncate((BmpMain.Height - ((Wire.Y + Wire.dY) * Canvas.Columns - Wire.dY) - SBIndent) / 2)
        End If
    End Sub
    Public Sub RecalculateCanvasParameters()
        If Wire.Y <> 0 Then
            FixBorder()
            CalculateLinesAndColumns()
            CalculateCanvasWidth()
            CalculateCanvasHeight()
            CalculateCanvasMinX()
            CalculateCanvasMinY()
        End If
    End Sub
    Public Sub FixBorder()
        If Wire.Y <= 16 Then
            Wire.Border = Math.Min(Wire.Border, 1)
        ElseIf Wire.Y <= 32 Then
            Wire.Border = Math.Min(Wire.Border, 2)
        ElseIf Wire.Y <= 64 Then
            Wire.Border = Math.Min(Wire.Border, 3)
        End If
    End Sub
    Public Sub SetWire(ByVal x As Short, ByVal y As Short, ByVal dx As Short, ByVal dy As Short)
        If x > 500 Then x = 500
        If y > 500 Then y = 500
        Wire.X = x
        Wire.Y = y
        Wire.min_dX = dx
        Wire.min_dY = dy
        Wire.dX = dx
        Wire.dY = dy

        Wire.ExtendSpaces = False

        If Wire.Y <= 32 And Wire.Border > 1 Then Wire.Border = 1
        CalculateLinesAndColumns()

        If Wire.Y >= 16 And Wire.Y < 32 Then
            font_filename = New Font("Lucida Sans Unicode", 11, FontStyle.Regular, GraphicsUnit.Pixel)
            font_singer = New Font("Lucida Sans Unicode", 10, FontStyle.Regular, GraphicsUnit.Pixel)
            font_bold = New Font(font_singer, FontStyle.Bold)
        End If
        If Wire.Y = 32 Then
            font_filename = New Font("Lucida Sans Unicode", Math.Min(font_filename.Size, 13), FontStyle.Regular, GraphicsUnit.Pixel)
            font_singer = New Font("Lucida Sans Unicode", Math.Min(font_singer.Size, 12), FontStyle.Regular, GraphicsUnit.Pixel)
            font_bold = New Font(font_singer, FontStyle.Bold)
        End If
        'RecalculateCanvasParameters()
        'ClearThumbs()
        'MakeAllThumbnails()
    End Sub
#End Region
#Region "--------------------------|  WITH FILES MANIP-ION  |-------------------------- GOOD  "
    Private Sub Rename(ByVal index As Integer)
        Dim str As String
        Dim old_str As String = Image(index).FileName
        str = InputBox("new name", "name changing", IO.Path.GetFileName(old_str))
        If str <> "" Then
            Try
                If My.Computer.FileSystem.FileExists(Image(index).FileName) Then
                    Image(index).FileName = Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str
                    My.Computer.FileSystem.RenameFile(old_str, str)
                    FileTags.Files(Image(index).InTagsIngex) = Image(index).FileName
                    Image(index).Name = str
                    Image(index).OriginalName = str
                    FindWidthWithText(index)
                End If
                If My.Computer.FileSystem.DirectoryExists(Image(index).FileName) Then
                    Image(index).FileName = Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str
                    My.Computer.FileSystem.RenameDirectory(old_str, str)
                    FileTags.Files(Image(index).InTagsIngex) = Image(index).FileName
                    Image(index).Name = str
                    Image(index).OriginalName = str
                    FindWidthWithText(index)
                End If
            Catch ex As Exception
                MsgBox("ÕÂ Û‰‡∏ÚÒˇ ÔÂÂËÏÂÌÓ‚‡Ú¸ Ù‡ÈÎ" + ex.ToString + vbNewLine + Image(index).FileName)
            End Try
        End If
    End Sub
    Public Sub DeleteOne(ByVal IndexToDelete As Long)
        For i As Long = IndexToDelete To NImages - 1
            SwapImages(i, i + 1)
        Next
        NImages -= 1
        SelectedImageIndex = Math.Min(IndexToDelete, NImages)
        For i As Long = IndexToDelete To NImages
            SetImageDestination(i)
        Next

        ClearThumb(NImages + 1)
        Try
            If My.Computer.FileSystem.FileExists(Image(NImages + 1).FileName) Then
                My.Computer.FileSystem.DeleteFile(Image(NImages + 1).FileName, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin)
            End If
            If My.Computer.FileSystem.DirectoryExists(Image(NImages + 1).FileName) Then
                My.Computer.FileSystem.DeleteDirectory(Image(NImages + 1).FileName, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
            End If
        Catch ex As Exception
            MsgBox("failed" + ex.ToString)
        End Try
    End Sub
    Public Sub Delete(ByVal index As Long)
        If index > 0 And index <= NImages Then
            If Path = "" Then
                Dim IndexToDelete As Long = index
                If Image(index).Selected Then
                    For I As Long = NImages To 1 Step -1
                        If Image(I).Selected Then
                            For j As Long = IndexToDelete To NImages - 1
                                SwapImages(j, j + 1)
                            Next
                            NImages -= 1
                            SelectedImageIndex = Math.Min(IndexToDelete, NImages)
                        End If
                    Next
                    OrderImages()
                    IsAnimatedImages = True
                Else
                    For i As Long = IndexToDelete To NImages - 1
                        SwapImages(i, i + 1)
                    Next
                    NImages -= 1
                    SelectedImageIndex = Math.Min(IndexToDelete, NImages)
                    For i As Long = IndexToDelete To NImages
                        SetImageDestination(i)
                    Next
                    IsAnimatedImages = True
                End If
            Else
                Dim box As New frmMMMenu
                box.LeftText = "no"
                box.RightText = "yes"
                box.ShowDialog()
                If box.Ansver = 1 Then
                    If Image(index).Selected Then
                        For I As Long = NImages To 1 Step -1
                            If Image(I).Selected Then
                                DeleteOne(I)
                            End If
                        Next
                    Else
                        DeleteOne(index)
                    End If

                    IsAnimatedImages = True
                End If
                box.Dispose()
            End If
        End If
    End Sub
    Private Sub Delete(ByVal index As Long, f As Boolean)
        If index > 0 And index <= NImages Then
            If Path = "" Then
                Dim IndexToDelete As Long = index
                If Image(index).Selected Then
                    For I As Long = NImages To 1 Step -1
                        If Image(I).Selected Then
                            For j As Long = IndexToDelete To NImages - 1
                                SwapImages(j, j + 1)
                            Next
                            NImages -= 1
                            SelectedImageIndex = Math.Min(IndexToDelete, NImages)
                        End If
                    Next
                    OrderImages()
                    IsAnimatedImages = True
                Else
                    For i As Long = IndexToDelete To NImages - 1
                        SwapImages(i, i + 1)
                    Next
                    NImages -= 1
                    SelectedImageIndex = Math.Min(IndexToDelete, NImages)
                    For i As Long = IndexToDelete To NImages
                        SetImageDestination(i)
                    Next
                    IsAnimatedImages = True
                End If
            Else
                Dim box As New frmMMMenu
                box.LeftText = "no"
                box.RightText = "yes"
                box.StartX = Me.Left + Me.Parent.Left + Image(index).X + Canvas.X + Image(index).Width / 2
                box.StartY = Me.Top + Me.Parent.Top + Image(index).Y + Canvas.Y + Wire.Y / 2
                box.ShowDialog()
                If box.Ansver = 1 Then
                    If Image(index).Selected Then
                        For I As Long = NImages To 1 Step -1
                            If Image(I).Selected Then
                                DeleteOne(I)
                            End If
                        Next
                    Else
                        DeleteOne(index)
                    End If

                    IsAnimatedImages = True
                End If
                box.Dispose()
            End If
        End If
    End Sub
#End Region
#Region "--------------------------|  IMAGE LIST            |-------------------------- GOOD  "
    Public Image(50000) As ImageStruct, NImages As Long = 0, IsAnimatedImages As Boolean

    Public Sub Refine(tmpNewList() As String)
        Dim NewN As Long = tmpNewList.Length - 1
        For i As Short = 1 To tmpNewList.Length - 1
            For j As Short = 1 To i - 1
                If LCase(tmpNewList(i)) = LCase(tmpNewList(j)) Then
                    tmpNewList(i) = ""
                    NewN -= 1
                    Exit For
                End If
            Next
        Next
        For i As Short = 1 To tmpNewList.Length - 1
            If (Not (IO.File.Exists(tmpNewList(i)) Or IO.Directory.Exists(tmpNewList(i)))) And tmpNewList(i) <> "" Then
                tmpNewList(i) = ""
                NewN -= 1
            End If
        Next
        Dim NewList(NewN) As String
        NewN = 0
        For j As Short = 1 To tmpNewList.Length - 1
            If tmpNewList(j) <> "" Then
                NewN += 1
                NewList(NewN) = tmpNewList(j)
            End If
        Next

        Dim SavedSI As Long = SelectedImageIndex
        Dim N As Long = 0
        For j As Long = NImages To 1 Step -1
            Dim F As Boolean = False
            For i As Short = 1 To NewList.Length - 1
                If LCase(Image(j).FileName) = LCase(NewList(i)) Then F = True : Exit For
            Next
            If Not F Then
                Image(j).FileName = ""
                Image(j).OriginalName = ""
                If SelectedImageIndex = j Then SelectedImageIndex = 0
            End If
            Image(j).ReLoaded = False
        Next

        For j As Long = 1 To NImages
            If Image(j).FileName <> "" Then
                N += 1
                If j <> N Then SwapImages(j, N)
            End If
        Next
        NImages = N

        For j As Short = 1 To NewList.Length - 1
            Dim NInList As Long = 0
            For i As Integer = 1 To NImages
                If LCase(Image(i).FileName) = LCase(NewList(j)) Then NInList = i
            Next
            If NInList = 0 Then
                AddImage(NewList(j))

                Image(NImages).Transparency = 0
                Image(NImages).DestTransparency = 1

                Image(NImages).Loaded = False
                IsNotEverithingLoaded = True

                SwapImages(NImages, j)
                If Arrangement = ArrangmentTypes.Õorizontal Then
                    Image(j).X = (Image(j - 1).X + Image(Math.Min(NImages, j + 1)).X) / 2
                    Image(j).Y = Image(j - 1).Y '(Image(j - 1).Y + Image(j + 1).Y) / 2
                Else
                    Image(j).X = Image(j - 1).X
                    Image(j).Y = (Image(j - 1).Y + Image(Math.Min(NImages, j + 1)).Y) / 2
                End If
            Else
                If j > NInList Then SwapImages(NInList, j)
            End If
        Next

        MakeAllThumbnails()
        OrderImages()
        If SelectedImageIndex = 0 Then SelectedImageIndex = 1 : SetCanvas()
        RedrawOnce = True
        'SetCanvas()
    End Sub
    Public Sub ClearThumbs()
        For i As Long = 1 To NImages
            'ClearThumb(i)
            If Image(i).Loaded Then
                With Image(i)
                    .Loaded = False
                    .Loading = False
                    .ReLoaded = False
                    .Transparency = 0
                    .DestTransparency = 0
                    .Selected = False
                    .Type = FileTypes.NotDefined
                    .Width = 16
                    .Height = 16
                    .DestWidth = 16
                    .DestHeight = 16
                End With
                Thumbnail(i).Dispose()
            End If
        Next
    End Sub
    Public Sub ClearThumb(ByVal i As Long)
        If Image(i).Loaded Then
            With Image(i)
                .Loaded = False
                .Loading = False
                .ReLoaded = False
                .Transparency = 0
                .DestTransparency = 0
                .Selected = False
                .Type = FileTypes.NotDefined
            End With
            Thumbnail(i).Dispose()
        End If
    End Sub
    Public Sub ClearImages()
        tmrAnimation.Enabled = False
        ClearThumbs()
        NImages = 0
        SelectedImageIndex = 0
        Canvas.DestX = 0
        Canvas.DestY = 0
        'If Arrangement = ArrangmentTypes.Õorizontal Then
        '    Canvas.X = 0
        'Else
        '    Canvas.Y = 0
        'End If
        'Canvas.Y = 0
        'Canvas.V = 0
        RedrawOnce = True
        SelectedImageIndex = 0
        tmrAnimation.Enabled = True
    End Sub
    Public Sub AddImage(ByVal Filename As String, Optional ByVal FileType As FileTypes = FileTypes.NotDefined)
        NImages += 1
        'If FileType <> FileTypes.Command Then
        '    If Not IO.File.Exists(Filename) And Not IO.Directory.Exists(Filename) Then
        '        Image(NImages).Type = FileTypes.Command
        '    End If
        'End If
        With Image(NImages)
            If (Filename.Length = 3) Then
                .FileName = Mid(Filename, 1, 2)
                .OriginalName = ""
                .Name = ""
            Else
                .FileName = Filename
                Try
                    .OriginalName = IO.Path.GetFileName(Filename)
                Catch
                    .OriginalName = Filename
                End Try
                .Name = .OriginalName
            End If
            .Singer = "noname"
            .InTagsIngex = 0
            .Transparency = 0
            .DestTransparency = 0.5
            .Selected = False
            .PreSelected = False
            .Type = FileType

            If FlyMode Then
                .X = startX
                .Y = startY
            Else
                SetImageLocation(NImages)
            End If

            FindWidthWithText_NoReduce(NImages)
            'Image(NImages).DestX = 0
            'FindWidthWithText(NImages) 'BAAD
            'If SelectedImageIndex = 0 Then SelectedImageIndex = 1
            Try
                If (LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".vbproj" Or _
                   LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".csproj" Or _
                   LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".cpproj" Or _
                   LCase(IO.Path.GetExtension(Image(NImages).FileName)) = ".sln") And _
                   SelectedImageIndex = 0 Then _
                    SelectedImageIndex = NImages
            Catch
            End Try
            If Not Thumbnail(NImages) Is Nothing Then
                Thumbnail(NImages).Dispose()
                .Loaded = False
                .Loading = False
            End If
        End With
        If FlyMode Then FlyingAlgorithm = FlyingAlgorithms.Simple
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
            'dates(i) = System.IO.File.GetCreationTime(Image(i).FileName).Ticks
            dates(i) = System.IO.File.GetLastWriteTime(Image(i).FileName).Ticks
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
        Dim R As New Random(DateTime.Now.Millisecond)
        If Sorting = "user" Then
            Dim i As Long
            For i = 1 To NImages * 20
                SwapImages(R.Next(1, NImages), R.Next(1, NImages))
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
#Region "--------------------------|  LOCATION & ANIMATING  |-------------------------- GOOD  "
    Public Sub SetImageLocation(ByVal i As Long)
        SetImageDestination(i)
        Image(i).X = Image(i).DestX
        Image(i).Y = Image(i).DestY
        Image(i).Transparency = 0
    End Sub
    Public Sub SetImageDestination(ByVal i As Long)
        If Arrangement = ArrangmentTypes.Õorizontal Then
            Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
            Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
        ElseIf Arrangement = ucImagesBox.ArrangmentTypes.Vertical Then
            Image(i).DestY = ((i - 1) Mod Canvas.Columns) * (Wire.Y + Wire.dY) + Canvas.Plus
            Image(i).DestX = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.X + Wire.dX) + Wire.dX
        Else
            Image(i).DestX = Image(i).X - Image(i).X Mod (Wire.X + Wire.dX)
            Image(i).DestY = Image(i).Y - Image(i).Y Mod (Wire.Y + Wire.dY)
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
    Public Function IsImageVisible(I As Long) As Boolean
        If I > 0 And I <= NImages Then
            If Image(I).Y + Canvas.Y > -Wire.Y And Image(I).Y + Canvas.Y < Me.Height And Image(I).X + Canvas.X > -Wire.X And Image(I).X + Canvas.X < Me.Width Then Return True Else Return False
        End If
    End Function
    Public Sub OrderImages()
        Dim P As PointF, WasVisible As Boolean = False
        If IsImageVisible(SelectedImageIndex) Then
            WasVisible = True
            P = New PointF(Image(SelectedImageIndex).DestX, Image(SelectedImageIndex).DestY)
        Else
            'P = New Point(Canvas.Width, Canvas.Height)
            If Canvas.MinX <> 0 Then P = New PointF(Canvas.X / Canvas.MinX, 0) Else P = New PointF(0, 0)
            If Canvas.MinY <> 0 Then P = New PointF(P.X, Canvas.Y / Canvas.MinY) Else P = New PointF(P.X, 0)
        End If

        If Arrangement = ArrangmentTypes.Õorizontal Then
            For i As Long = 1 To NImages
                Image(i).DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                Image(i).DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
            Next
        Else
            For i As Long = 1 To NImages
                Image(i).DestY = ((i - 1) Mod Canvas.Columns) * (Wire.Y + Wire.dY) + Canvas.Plus
                Image(i).DestX = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.X + Wire.dX) + Wire.dX
            Next
        End If

        For i As Long = 1 To NImages
            With Image(i)
                .DestTransparency = 1
                If .Loaded = True And .ReLoaded = False Then
                    If .Type = FileTypes.Image Then
                        Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)
                        If Wire.Y > 40 Then
                            If ShowImagesName Then s.Y -= 14
                        Else
                            s.X = s.Y
                        End If
                        .DestHeight = Thumbnail(i).Height * 20
                        .DestWidth = Thumbnail(i).Width * 20
                        CorrectSize(.DestWidth, .DestHeight, s)
                    ElseIf Wire.Y <= 32 Then
                        If .Type <> FileTypes.Music Then
                            Dim s As New Point(Wire.X, Wire.Y)
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        Else
                            Dim s As New Point(Wire.X, Wire.Y)
                            If Wire.Y = 32 Then .DestHeight = 32 Else .DestHeight = Math.Min(16, Wire.Y)
                            .DestWidth = Thumbnail(i).Width
                        End If
                    End If
                End If
                .Animate = True
            End With
        Next

        ' RecalculateCanvasParameters()
        CalculateCanvasHeight()
        CalculateCanvasWidth()
        CalculateCanvasMinY()
        CalculateCanvasMinX()

        If Arrangement <> ArrangmentTypes.Õorizontal Then
            Dim dx As Long
            Dim dy As Long = -Canvas.Y
            If Canvas.Animate Then dy = -Canvas.DestY
            If WasVisible Then
                dx = P.X - Image(SelectedImageIndex).DestX
            Else
                'If P.X <> 0 Then dx = (Canvas.X - BmpMain.Width / 2) * (Canvas.Width / P.X) - Canvas.X + BmpMain.Width / 2
                If P.X <> 0 Then dx = Canvas.MinX * P.X - Canvas.X '+ BmpMain.Width / 2
                'dx = -10 - Canvas.X
            End If
            If Canvas.X + dx > 0 Then dx = -Canvas.X
            If Canvas.X + dx < Canvas.MinX Then dx = Canvas.MinX - Canvas.X

            Canvas.X += dx
            If Canvas.Animate Then Canvas.DestX += dx Else Canvas.DestX = Canvas.X
            Canvas.Y += dy
            'Canvas.DestY += dy
            For i As Long = 1 To NImages
                Image(i).X -= dx
                Image(i).Y -= dy
            Next
        Else
            Dim dx As Long = -Canvas.X
            If Canvas.Animate Then dx = -Canvas.DestX
            Dim dy As Long
            If WasVisible Then
                dy = P.Y - Image(SelectedImageIndex).DestY
            Else
                If P.Y <> 0 Then dy = (Canvas.Y - BmpMain.Height / 2) * (Canvas.Height / P.Y) - Canvas.Y + BmpMain.Height / 2
            End If
            If Canvas.Y + dy > 0 Then dy = Canvas.Y
            If Canvas.Y + dy < Canvas.MinY Then dy = Canvas.MinY - Canvas.Y
            Canvas.X += dx
            Canvas.Y += dy
            For i As Long = 1 To NImages
                Image(i).X -= dx
                Image(i).Y -= dy
            Next
        End If

        IsAnimatedImages = True
    End Sub
    Public Sub OrderImagesDifferentWidth()
        Dim CurrentWidth As Long
        If Arrangement = ArrangmentTypes.Õorizontal Then
            For i As Long = 1 To NImages
                With Image(i)
                    .DestX = ((i - 1) Mod Canvas.Columns) * (Wire.X + Wire.dX) + Canvas.Plus
                    .DestY = Math.Truncate((i - 1) / Canvas.Columns) * (Wire.Y + Wire.dY) + Wire.dY
                    .DestTransparency = 1

                    If .Loaded = True And .ReLoaded = False Then
                        If .Type = FileTypes.Image Then
                            Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)
                            If Wire.Y > 40 Then
                                If ShowImagesName Then s.Y -= 14
                            Else
                                s.X = s.Y
                            End If
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        ElseIf Wire.Y <= 32 Then
                            If .Type <> FileTypes.Music Then
                                Dim s As New Point(Wire.X, Wire.Y)
                                .DestHeight = Thumbnail(i).Height * 20
                                .DestWidth = Thumbnail(i).Width * 20
                                CorrectSize(.DestWidth, .DestHeight, s)
                            Else
                                Dim s As New Point(Wire.X, Wire.Y)
                                If Wire.Y = 32 Then .DestHeight = 32 Else .DestHeight = Math.Min(16, Wire.Y)
                                .DestWidth = Thumbnail(i).Width
                            End If
                        End If
                    End If
                    .Animate = True
                End With
            Next
        Else
            Dim StartPoint As Long = Wire.dX
            For i As Long = 1 To NImages
                If i Mod Canvas.Columns = 1 Then
                    StartPoint += CurrentWidth + Wire.dX
                    CurrentWidth = 0
                    For j As Long = i To Math.Min(Canvas.Columns + i - 1, NImages)
                        FindWidthWithText_NoReduce(j)
                        CurrentWidth = Math.Max(Image(j).WidthWithText + 10, CurrentWidth)
                    Next
                    Wire.X = CurrentWidth
                End If
                With Image(i)
                    .DestY = Canvas.Plus + ((i - 1) Mod Canvas.Columns) * (Wire.Y + Wire.dY)
                    .DestX = StartPoint
                    .DestTransparency = 1
                    If .Loaded = True And .ReLoaded = False Then
                        If .Type = FileTypes.Image Then
                            Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)
                            If Wire.Y > 40 Then
                                If ShowImagesName Then s.Y -= 14
                            Else
                                s.X = s.Y
                            End If
                            .DestHeight = Thumbnail(i).Height * 20
                            .DestWidth = Thumbnail(i).Width * 20
                            CorrectSize(.DestWidth, .DestHeight, s)
                        ElseIf Wire.Y <= 32 Then
                            If .Type <> FileTypes.Music Then
                                Dim s As New Point(Wire.X, Wire.Y)
                                .DestHeight = Thumbnail(i).Height * 20
                                .DestWidth = Thumbnail(i).Width * 20
                                CorrectSize(.DestWidth, .DestHeight, s)
                            Else
                                Dim s As New Point(Wire.X, Wire.Y)
                                If Wire.Y = 32 Then .DestHeight = 32 Else .DestHeight = Math.Min(16, Wire.Y)
                                .DestWidth = Thumbnail(i).Width
                            End If
                        End If
                    End If
                    .Animate = True
                End With
            Next
            Canvas.Width = (StartPoint + CurrentWidth + Wire.dX)
            Canvas.MinX = -(StartPoint + CurrentWidth + Wire.dX) + Me.Width
        End If

        'CalculateCanvasHeight()
        'CalculateCanvasWidth()
        'CalculateCanvasMinY()
        'CalculateCanvasMinX()
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
            If Wire.Y <= 40 Then
                Dim a As Long = 0, max As Long = 0
                For i As Long = 1 To NImages
                    a += Image(i).WidthWithText
                    If max < Image(i).WidthWithText Then max = Image(i).WidthWithText
                Next
                a = a / NImages
                Wire.X = max + 50
                If Wire.X > Me.Width / 2 - Wire.dX * 2 Then Wire.X = Me.Width / 2 - Wire.dX * 2
            End If
            Wire.X = Math.Min(Wire.X, 330)
            'SetWire(Wire.X, Wire.Y, Wire.dX, Wire.dY)

            tmrAnimation.Enabled = False

            IsNotEverithingLoaded = True

            'Cursor = Cursors.Arrow
            StopLoading = False
            RecalculateCanvasParameters()
            CurrentLoadingImgIndex = 0

            CalculateCanvasHeight()
            CalculateCanvasMinY()

            tmrAnimation.Enabled = True

            'MsgBox("pl1")
            'SetCanvas() '!!!!!!!!!!!!!!!!
        End If
    End Sub

    Public MakeThumbnails As Boolean = True
    Dim LoadedThumbnail As Bitmap, LoadingStrInfo As String
    Public CurrentLoadingImgIndex As Long, CurrentLoadingFileName As String
    Private Sub bwLoadOne_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadOne.DoWork
        MakeThumbnail(CurrentLoadingFileName)
    End Sub
    Private Sub MakeThumbnail(ByVal FileName As String)
        Dim wait As Boolean = True
        If IsImageFile(FileName) Then
            Dim ThNumber As Long = SearchInThumbs(FileName)
            Dim bmp As Bitmap
            Dim s As New Point(Wire.X - Wire.Border * 2, Wire.Y - Wire.Border * 2)

            If Wire.Y <= 40 Then
                's.X = Wire.Y
                s = New Point(Wire.Y - Wire.Border * 2, Wire.Y - Wire.Border * 2)
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

                    LoadedThumbnail = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                    Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                        graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                        graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                        graf.DrawImage(bmp, -1, -1, W + 2, H + 2)
                        graf.DrawImage(bmp, 0, 0, W, H) 'graf.DrawImage(bmp1, -1, -1, W + 1, H + 1)   'graf.DrawImage(bmp1, 0, 0, W, H)
                    End Using
                    bmp.Dispose()
                    wait = False
                Else
                    If (W = s.X Or H = s.Y) And ThumbIsNotOld(ThNumber) Then  'IF THUMBNAIL SIZE IS RIGHT
                        LoadingStrInfo = "thumb. is ideal"
                        LoadedThumbnail = bmp.Clone
                        bmp.Dispose()
                        bmp = Nothing
                        wait = False
                    Else                                               'IF THUMBNAIL SIZE IS BAD
                        LoadingStrInfo = "thumb. is smaller"
                        bmp.Dispose()
                        If MakeThumbnails Then
                            If Not ReMakeThumb(ThNumber, s, LoadedThumbnail) Then LoadedThumbnail = bmp_error.Clone
                        Else
                            If Wire.Y <= 16 Then
                                LoadedThumbnail = icoImg16.Clone()
                            Else
                                LoadedThumbnail = icoImg32.Clone()
                            End If
                        End If
                    End If
                End If
            Else                                               'IF THERE IS NO THUMBNAIL
                If MakeThumbnails Then
                    If ThNumber > 0 Then
                        If Not ReMakeThumb(ThNumber, s, LoadedThumbnail) Then LoadedThumbnail = bmp_error.Clone
                    Else
                        Try
                            Dim myImageCodecInfo As Imaging.ImageCodecInfo, myEncoder As Imaging.Encoder
                            Dim myEncoderParameter As Imaging.EncoderParameter, myEncoderParameters As Imaging.EncoderParameters
                            myImageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Jpeg)
                            myEncoder = System.Drawing.Imaging.Encoder.Quality
                            myEncoderParameters = New Imaging.EncoderParameters(1)
                            myEncoderParameter = New Imaging.EncoderParameter(myEncoder, CType(100L, Int32))
                            myEncoderParameters.Param(0) = myEncoderParameter
                            'for image saving

                            Dim bmp11 As Bitmap, W, H As Long

                            bmp11 = Bitmap.FromFile(FileName)

                            W = bmp11.Width
                            H = bmp11.Height

                            CorrectSize(W, H, s)

                            LoadedThumbnail = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                            Using temp_g As Graphics = Graphics.FromImage(LoadedThumbnail)
                                temp_g.InterpolationMode = Drawing2D.InterpolationMode.Low ' Drawing2D.InterpolationMode.HighQualityBicubic
                                temp_g.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed ' Drawing2D.CompositingQuality.HighQuality
                                temp_g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed ' Drawing2D.SmoothingMode.HighQuality
                                temp_g.DrawImage(bmp11, -1, -1, W + 1, H + 1)
                                temp_g.DrawImage(bmp11, 0, 0, W, H)
                            End Using
                            NThumbs += 1
                            Thumbnails(NThumbs) = FileName.ToLower
                            ThumbDate(NThumbs) = My.Computer.FileSystem.GetFileInfo(FileName).LastWriteTime.Ticks
                            LoadedThumbnail.Save(Application.StartupPath + "\config\th\" + NThumbs.ToString + ".jpg", myImageCodecInfo, myEncoderParameters)

                            bmp11.Dispose()
                        Catch ex As Exception
                            LoadedThumbnail = bmp_error.Clone
                        End Try

                        'If Not MakeThumb(FileName, s, LoadedThumbnail) Then LoadedThumbnail = bmp_error.Clone
                    End If
                Else
                    'On Error Resume Next '                                                             !!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    If Wire.Y <= 16 Then
                        LoadedThumbnail = icoImg16.Clone()
                    Else
                        LoadedThumbnail = icoImg32.Clone()
                    End If
                End If
            End If
        Else
            If IO.Directory.Exists(FileName) Then
                If Wire.Y >= 32 Then
                    LoadedThumbnail = icoFolder32.Clone

                    If (FileName.Length > 9) Then
                        If LCase(FileName.Substring(FileName.Length - 7)) = "desktop" Then
                            If (BmpDesktop Is Nothing) Then
                                BmpDesktop = New Bitmap(Application.StartupPath + "\desktop2_32.png")
                            End If
                            LoadedThumbnail = BmpDesktop.Clone()
                        End If
                    End If
                    If (FileName.Length = 2) Then
                        If (BmpDrive Is Nothing) Then
                            BmpDrive = New Bitmap(Application.StartupPath + "\drive3_32.png")
                        End If
                        LoadedThumbnail = BmpDrive.Clone()
                    End If
                    If (FileName.IndexOf("music") >= 0) Then
                        If (BmpSongs Is Nothing) Then
                            BmpSongs = New Bitmap(Application.StartupPath + "\muz5_32.png")
                        End If
                        LoadedThumbnail = BmpSongs.Clone()
                    End If
                Else
                    LoadedThumbnail = icoFolder16.Clone
                End If
                'BmpFilms = New Bitmap(Application.StartupPath + "\films4_32.png")
                'BmpMkDir = New Bitmap(Application.StartupPath + "\mkdir2_32.png")
                'BmpAlboom = New Bitmap(Application.StartupPath + "\alboom2_32.png")
                'BmpUsb = New Bitmap(Application.StartupPath + "\usb_32.png")
            Else
                'wait = False
                '1. »˘ÂÏ ‚ ÂÂÒÚÂ ‡Ò¯ËÂÌËÂ: ´HKEY_CLASSES_ROOT\.docª. ¡Â∏Ï ÁÌ‡˜ÂÌËÂ ËÁ ´HKEY_CLASSES_ROOT\.doc\(Default)ª (Ì‡ÔËÏÂ, ´Word.Document.8ª).
                '2. »˘ÂÏ ´HKEY_CLASSES_ROOT\Word.Document.8ª, ·Â∏Ï ÁÌ‡˜ÂÌËÂ ËÁ ´HKEY_CLASSES_ROOT\Word.Document.8\DefaultIcon\(Default)ª (Ì‡ÔËÏÂ, ´C:\WINDOWS\Installer\{90110419-6000-11D3-8CFE-0150048383C9}\wordicon.exe,1ª).

                If Not IsMusicFile(FileName) Then
                    If IO.File.Exists(FileName) Then
                        Dim IH As New ViewHelpers.IconHelper
                        If Wire.Y >= 32 Then
                            Dim FIco As Icon = Icon.ExtractAssociatedIcon(FileName)
                            'My.Computer.FileSystem.
                            'Dim NIco As Icon = New Icon(FIco, Wire.Y, Wire.Y)
                            LoadedThumbnail = FIco.ToBitmap  'IH.GetIcon32(FileName)
                        ElseIf Wire.Y >= 16 Then
                            LoadedThumbnail = IH.GetIcon16(FileName)
                        Else
                            LoadedThumbnail = New Bitmap(Wire.Y, Wire.Y)
                            Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                                graf.DrawImage(IH.GetIcon16(FileName), 0, 0, Wire.Y, Wire.Y)
                            End Using
                        End If
                    Else
                        If Wire.Y >= 32 Then LoadedThumbnail = icoFolder32.Clone Else LoadedThumbnail = icoFolder16.Clone
                    End If
                Else
                    If Wire.Y >= 32 Then LoadedThumbnail = icoMusic32.Clone Else LoadedThumbnail = icoMusic16.Clone
                End If
                End If
            End If
    End Sub
#End Region
#Region "--------------------------|  Resizing              |--------------------------"
    Public DoResizeEvent As Boolean = False
    Private Sub ucImagesBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If DoResizeEvent Then
            If Me.Height > 0 Then
                BmpMain = New Bitmap(Me.Width, Me.Height)
                GraphicsMain = Graphics.FromImage(BmpMain)

                RecalculateCanvasParameters()

                'Draw_Picturies()
                RedrawOnce = True
                NextFrame(True)
                picMain.Image = BmpMain
                picMain.Refresh()
                'Me.BackgroundImage = BmpMain
                Me.Refresh()
            End If
        End If
    End Sub
    Public Sub ResizeStarted()
        DoResizeEvent = False
        tmrAnimation.Enabled = False
        picMain.Visible = False
        Me.BackgroundImage = BmpMain
        FlyingAlgorithm = FlyingAlgorithms.NotSimple
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
        FlyingAlgorithm = FlyingAlgorithms.NotSimple
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
        If h > 70 Then
            If BmpMain.Height <> h Or BmpMain.Width <> w Then
                ''If Not bwDraw.IsBusy Then
                'Me.BackgroundImage = BmpMain
                ''picMain.Visible = True
                ''picMain.Height = h
                ''picMain.Image = BmpMain
                ''picMain.Refresh()
                'Me.Refresh()

                BmpMain = New Bitmap(w, h)
                GraphicsMain = Graphics.FromImage(BmpMain)

                RecalculateCanvasParameters()
                OrderImages()

                NextFrame(True) 'bwDraw.RunWorkerAsync()

                'End If

                'NextFrame(False)
                'picMain.Visible = True
                Me.Height = h
                Me.Width = w
                'picMain.Image = BmpMain

                Me.BackgroundImage = BmpMain
            End If
        End If
        If h > 0 And h <= 70 Then
            BmpMain = New Bitmap(w, h)
            GraphicsMain = Graphics.FromImage(BmpMain)
            RecalculateCanvasParameters()
            OrderImages()
            NextFrame(True)

            If h < 70 Then
                Dim a2 As New SolidBrush(Color.FromArgb(255, 200, 200, 200))
                If h > 10 Then a2.Color = Color.FromArgb(255 - 255 * Math.Sin(Math.PI / 2 * (h - 10) / 60), 200, 200, 200)
                GraphicsMain.FillRectangle(a2, 0, 0, w, h)
            End If

            Me.Height = h
            Me.Width = w
            Me.BackgroundImage = BmpMain

        End If
        Me.Height = h
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
        'FlyingAlgorithm = "simple"
    End Sub
    Public Sub ResizeEnded()
        RecalculateCanvasParameters()
        CalculateCanvasWidth()
        CalculateCanvasHeight()
        OrderImages()

        tmrAnimation.Enabled = True

        picMain.Width = Me.Width
        picMain.Height = Me.Height
        picMain.Image = BmpMain
        picMain.Visible = True
        DoResizeEvent = True
        'FlyingAlgorithm = "simple"
    End Sub
#End Region
    Dim PrevCursorPos As New Point(10, 10)
    Dim PrevPrevCursorPos As Point
    Dim CursorMoved As Boolean = False, ChosenObj As Long
    Dim FirstX As Long, FirstY As Long
    Dim PrevCursorPosInTimer As Point

    Dim MDTime As Double = 0
    Dim MDMoment As DateTimeOffset
    Dim RightButtonCircleRadius As Double = 0
    Dim RightButtonCircleLastDelta As Double = 0
    Dim RightButtonCircleOpacity As Double = 0

    Dim IsMouseDown As Boolean = False

    Dim DraggingFilesList() As Long, NDraggingFiles As Long = 0, IndexOfChosenInDraggingFiles As Long = -1
    Dim IsSmthDragging As Boolean = False
    Dim DraggingCount As Long = 0

    Dim IStartedDragDrop As Boolean = False
    'Dim d2dManager As D2DWraper '= New D2DWraper()
    'd2dManager.Initialize(CoreApplication.MainView.CoreWindow); 
    'd2dManager.DrawTextToImage("Any text to be written on the picture", "SourceFile.png", "TargetFile.png");


#Region "--------------------------|  Draggings  "
    Dim DragDropCandidateIndex As Long = -1

    Private Sub AddImageToDragDrop(ByVal i As Integer)
        If Image(i).Selected Then
            NDraggingFiles += 1
            ReDim Preserve DraggingFilesList(NDraggingFiles)

            DraggingFilesList(NDraggingFiles) = i
            Image(i).DestTransparency = 1 ' 0.5
            Image(i).Animate = True
            FileTags.Tags(Image(i).InTagsIngex).LaunchingTimes += 1

            IsAnimatedImages = True

            IsMouseDown = False
            IsSmthDragging = True
            IStartedDragDrop = True

            Dim str() As String
            If Image(ChosenObj).Selected = True Then
                ReDim str(NDraggingFiles - 1)
                For j As Long = 1 To NDraggingFiles
                    str(j - 1) = Image(DraggingFilesList(j)).FileName
                Next
            Else
                ReDim str(0)
                str(0) = Image(i).FileName
            End If

            IsAnimatedImages = True
            Image(ChosenObj).Animate = True
            RedrawOnce = True

            'Dim a As New System.Threading.Thread(AddressOf DragDrop1)
            'a.Start(str)

            'Dim m As New DataObject(DataFormats.FileDrop, str)
            'Me.DoDragDrop(m, DragDropEffects.All)
        End If
    End Sub
    Private Delegate Sub DelDragDrop(m As DataObject)
    Private Sub DragDrop1(str As Object)
        Dim m As New DataObject(DataFormats.FileDrop, str)
        Me.DoDragDrop(m, DragDropEffects.All)
        'MsgBox(str(0))
    End Sub

    Sub StartDragDrop(ByVal i As Integer)
        If Image(i).Selected Then
            NDraggingFiles = 0
            For j As Long = 1 To NImages
                If Image(j).Selected Then NDraggingFiles = NDraggingFiles + 1
            Next
            ReDim DraggingFilesList(NDraggingFiles)
            NDraggingFiles = 0
            For j As Long = 1 To NImages
                If Image(j).Selected Then
                    NDraggingFiles += 1
                    DraggingFilesList(NDraggingFiles) = j
                    Image(j).DestTransparency = 1 ' 0.5
                    Image(j).Animate = True
                    FileTags.Tags(Image(j).InTagsIngex).LaunchingTimes += 1
                    If ChosenObj = j Then IndexOfChosenInDraggingFiles = NDraggingFiles
                End If
            Next
        Else
            NDraggingFiles = 1
            ReDim DraggingFilesList(NDraggingFiles)
            DraggingFilesList(NDraggingFiles) = i
            Image(i).DestTransparency = 1 ' 0.5
            IndexOfChosenInDraggingFiles = 1
            Image(i).Animate = True
            FileTags.Tags(Image(i).InTagsIngex).LaunchingTimes += 1
        End If
        IsAnimatedImages = True

        IsMouseDown = False
        IsSmthDragging = True
        IStartedDragDrop = True
        Dim str() As String
        If Image(i).Selected = True Then
            ReDim str(NDraggingFiles - 1)
            For j As Long = 1 To NDraggingFiles
                str(j - 1) = Image(DraggingFilesList(j)).FileName
            Next
        Else
            ReDim str(0)
            str(0) = Image(i).FileName
        End If

        Dim m As New DataObject(DataFormats.FileDrop, str)
        Me.DoDragDrop(m, DragDropEffects.All) 'DragDropEffects.Link) 
    End Sub
    Private Sub picMain_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picMain.DragEnter, Me.DragEnter
        DragDropCandidateIndex = -1
        e.Effect = DragDropEffects.Copy
        If IStartedDragDrop Then
            PrevCursorPos.X = e.X - Me.Left - Me.Parent.Left
            PrevCursorPos.Y = e.Y - Me.Top - Me.Parent.Top
            If Not (Image(ChosenObj).X <= PrevCursorPos.X - Canvas.X _
            And Image(ChosenObj).X + Image(ChosenObj).WidthWithText >= PrevCursorPos.X - Canvas.X _
            And Image(ChosenObj).Y <= PrevCursorPos.Y - Canvas.Y _
            And Image(ChosenObj).Y + Wire.Y >= PrevCursorPos.Y - Canvas.Y) Then
                Image(ChosenObj).X = PrevCursorPos.X - Image(ChosenObj).WidthWithText / 2 - Canvas.X
                Image(ChosenObj).DestX = PrevCursorPos.X - Image(ChosenObj).WidthWithText / 2
                Image(ChosenObj).Y = PrevCursorPos.Y - Wire.Y / 2 - Canvas.Y
                Image(ChosenObj).DestY = PrevCursorPos.Y - Wire.Y / 2
            End If

            IsAnimatedImages = True
            Image(ChosenObj).Animate = True
            RedrawOnce = True
        Else
            IsSmthDragging = True
            Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
            DraggingCount = files.Length

            If IsInList(files) Then
                IStartedDragDrop = True
                ReDim DraggingFilesList(DraggingCount)
                For i As Long = 1 To DraggingCount
                    DraggingFilesList(i) = IsInList(files(i - 1))
                Next
                NDraggingFiles = DraggingCount
                IndexOfChosenInDraggingFiles = 1
                PrevCursorPos.X = e.X - Me.Left - Me.Parent.Left
                PrevCursorPos.Y = e.Y - Me.Top - Me.Parent.Top

                ChosenObj = DraggingFilesList(1)
                If Not (Image(ChosenObj).X <= PrevCursorPos.X - Canvas.X _
                And Image(ChosenObj).X + Image(ChosenObj).WidthWithText >= PrevCursorPos.X - Canvas.X _
                And Image(ChosenObj).Y <= PrevCursorPos.Y - Canvas.Y _
                And Image(ChosenObj).Y + Wire.Y >= PrevCursorPos.Y - Canvas.Y) Then
                    Image(ChosenObj).X = PrevCursorPos.X - Image(ChosenObj).WidthWithText / 2 - Canvas.X
                    Image(ChosenObj).DestX = PrevCursorPos.X - Image(ChosenObj).WidthWithText / 2
                    Image(ChosenObj).Y = PrevCursorPos.Y - Wire.Y / 2 - Canvas.Y
                    Image(ChosenObj).DestY = PrevCursorPos.Y - Wire.Y / 2
                End If
            Else
                NDraggingFiles = 0
            End If
        End If
    End Sub
    Private Sub ucImagesBox_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragOver
        If (LastBigMousePosition.X - e.X) ^ 2 + (LastBigMousePosition.Y - e.Y) ^ 2 > 5 ^ 2 Then
            LastBigMouseMoveTime = DateTime.Now ' LastMouseMoveTime
            LastBigMousePosition = New Point(e.X, e.Y)
        End If
        'If IsSmthDragging = False Then
        '    Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
        '    If IsInList(files) Then
        '        'MsgBox("d")
        '        IsSmthDragging = True
        '    End If
        'End If
        If IsSmthDragging = True Then
            Dim x, y As Long

            x = e.X - Me.Left - Me.Parent.Left - Canvas.X
            y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y
            Dim ii As Short
            If Arrangement = ArrangmentTypes.Õorizontal Then
                x = x - Canvas.Plus : y = y - Wire.dY / 2
                ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
            Else
                y = y - Canvas.Plus + Wire.dY / 2 : x = x - Wire.dX / 2
                ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
            End If
            If ii < 0 Then ii = 0
            x = e.X - Me.Left - Me.Parent.Left - Canvas.X
            y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y
            If ii <> SelectedImageIndex And (Image(ii).Selected = False Or (Image(ChosenObj).Selected = False And Image(ii).Selected = True)) Then 'Image(ii).Type = FileTypes.Folder And 
                If (x > Image(ii).X And y > Image(ii).Y And x < Image(ii).X + Image(ii).WidthWithText And y < Image(ii).Y + Wire.Y) Then
                    If (DragDropCandidateIndex <> ii) Then
                        DragDropCandidateIndex = ii
                        SelInt = 1
                    End If
                Else
                    DragDropCandidateIndex = -1
                End If
            Else
                DragDropCandidateIndex = -1
            End If

            x = e.X - Me.Left - Me.Parent.Left
            y = e.Y - Me.Top - Me.Parent.Top

            If IStartedDragDrop And NDraggingFiles > 0 Then
                Dim dx As Long = x - PrevCursorPos.X
                Dim dy As Long = y - PrevCursorPos.Y
                Image(ChosenObj).X += dx : PrevCursorPos.X = x
                Image(ChosenObj).Y += dy : PrevCursorPos.Y = y
                Image(ChosenObj).Animate = True
                'Image(1).Y += 1
                If DragDropCandidateIndex = -1 Then Image(ChosenObj).DestTransparency = 1 Else Image(ChosenObj).DestTransparency = 0.5
                IsAnimatedImages = True
                Image(ChosenObj).DestX = Image(ChosenObj).X
                Image(ChosenObj).DestY = Image(ChosenObj).Y
            End If
        End If
    End Sub
    Public Function IsInListIgnoreCase(path As String) As Long
        path = path.ToLower()
        For i As Long = 1 To NImages
            If Image(i).FileName.ToLower() = path Then Return i : Exit For
        Next
        Return 0
    End Function
    Public Function IsInList(path As String) As Long
        For i As Long = 1 To NImages
            If Image(i).FileName = path Then Return i : Exit For
        Next
        Return 0
    End Function
    Public Function IsInList(path() As String) As Boolean
        For Each t As String In path
            If IsInList(t) = 0 Then Return False : Exit For
        Next
        Return True
    End Function
    Private Sub picMain_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picMain.DragDrop, Me.DragDrop
        'MsgBox(e.KeyState)
        FlyingAlgorithm = FlyingAlgorithms.Simple
        MDTime = 0
        IsSmthDragging = False

        If NDraggingFiles > 0 Then
            For n As Long = 1 To NDraggingFiles
                SetImageDestination(DraggingFilesList(n))
                Image(DraggingFilesList(n)).DestTransparency = 1
            Next
        End If

        Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
        Dim t As String
        Dim Copy As Long
        Dim DraggingFilesExists As Boolean = True
        For Each t In files
            If IsInList(t) = 0 Then DraggingFilesExists = False : Exit For
        Next

        Dim x = e.X - Me.Left - Me.Parent.Left - Canvas.X
        Dim y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y
        Dim ii As Short
        If Arrangement = ArrangmentTypes.Õorizontal Then
            x = x - Canvas.Plus : y = y - Wire.dY
            ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        Else
            y = y - Canvas.Plus : x = x - Wire.dX
            ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
        End If

        ii = DragDropCandidateIndex
        If ii < 0 Then ii = 0
        If ii > 0 And Image(ii).Type = FileTypes.Folder And ii <> SelectedImageIndex And (Image(ii).Selected = False Or (Image(SelectedImageIndex).Selected = False And Image(ii).Selected = True)) Then
            Dim box As New frmMMMenu
            box.LeftText = "copy to" + vbNewLine + Mid(Image(ii).Name, 1, 8)
            box.RightText = "move to" + vbNewLine + Mid(Image(ii).Name, 1, 8)
            box.ShowDialog()
            Copy = box.Ansver
            box.Dispose()
            If Copy >= 0 Then
                Dim DestPath As String = Image(ii).FileName + "\"
                For Each t In files
                    Try
                        If My.Computer.FileSystem.FileExists(t) Then
                            If LCase(t) <> LCase(DestPath + GetFileName(t)) Then
                                If Copy = 0 Then
                                    My.Computer.FileSystem.CopyFile(t, DestPath + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                Else
                                    My.Computer.FileSystem.MoveFile(t, DestPath + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                End If
                            End If
                        End If
                        If My.Computer.FileSystem.DirectoryExists(t) Then
                            If LCase(t) <> LCase(DestPath + GetFileName(t)) Then
                                If Copy = 0 Then
                                    My.Computer.FileSystem.CopyDirectory(t, DestPath + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                Else
                                    My.Computer.FileSystem.MoveDirectory(t, DestPath + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        MsgBox("Œ¯Ë·Í‡ ÍÓÔËÓ‚‡ÌËˇ" + vbNewLine + vbNewLine + t + vbNewLine + vbNewLine + ex.ToString)
                    End Try
                Next
            End If

            Image(ChosenObj).DestTransparency = 1
            Image(ChosenObj).Animate = True
            SetImageDestination(ChosenObj)
            If NDraggingFiles > 1 Then
                Dim i As Long
                For nn As Long = 1 To NDraggingFiles
                    i = DraggingFilesList(nn)
                    Image(i).DestTransparency = 1
                    Image(i).Animate = True
                    SetImageDestination(i)
                Next
            End If
            IsAnimatedImages = True
            If (Copy = 1) Then
                FlyingAlgorithm = FlyingAlgorithms.NotSimple
                For k As Long = 1 To NImages
                    While Not (IO.File.Exists(Image(k).FileName)) And (Not IO.Directory.Exists(Image(k).FileName)) And NImages >= k
                        For nn As Long = k To NImages - 1
                            SwapImages(nn, nn + 1)
                        Next
                        NImages -= 1
                        Image(NImages + 1).Loaded = False
                        If Not (Thumbnail(NImages + 1) Is Nothing) Then Thumbnail(NImages + 1).Dispose()
                    End While
                    SetImageDestination(k)
                Next
                IsAnimatedImages = True
            End If
        ElseIf ii > 0 And Image(ii).Type = FileTypes.ExeFile And ii <> SelectedImageIndex And (Image(ii).Selected = False Or (Image(SelectedImageIndex).Selected = False And Image(ii).Selected = True)) Then
            Shell(Chr(34) + Image(ii).FileName + Chr(34) + " " + Chr(34) + files(0) + Chr(34), AppWinStyle.NormalFocus)
        Else
            x = e.X - Me.Left - Me.Parent.Left - Canvas.X
            y = e.Y - Me.Top - Me.Parent.Top - Canvas.Y

            If Arrangement = ArrangmentTypes.Õorizontal Then
                x = x - Canvas.Plus : y = y - Wire.dY
                ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
            Else
                y = y - Canvas.Plus : x = x - Wire.dX
                ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
            End If

            If (Not DraggingFilesExists) And Path <> "" Then
                Dim box As New frmMMMenu
                box.LeftText = "copy"
                box.RightText = "move"
                box.ShowDialog()
                Copy = box.Ansver
                box.Dispose()
            End If

            If Not DraggingFilesExists Then 'And Copy >= 0 Then
                StopLoading = False
                Dim r As New Random(DateTime.Now.Millisecond)
                For Each t In files
                    If Not IsInList(t) Then
                        If Path <> "" Then
                            Try
                                If My.Computer.FileSystem.FileExists(t) Then
                                    If LCase(t) <> LCase(Path + GetFileName(t)) Then
                                        If Copy <= 0 Then
                                            My.Computer.FileSystem.CopyFile(t, Path + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                        Else
                                            My.Computer.FileSystem.MoveFile(t, Path + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                        End If
                                    End If
                                    t = Path + GetFileName(t)
                                End If
                                If My.Computer.FileSystem.DirectoryExists(t) Then
                                    If LCase(t) <> LCase(Path + GetFileName(t)) Then
                                        If Copy <= 0 Then
                                            My.Computer.FileSystem.CopyDirectory(t, Path + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                        Else
                                            My.Computer.FileSystem.MoveDirectory(t, Path + GetFileName(t), FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                                        End If
                                    End If
                                    t = Path + GetFileName(t)
                                End If
                            Catch ex As Exception
                                MsgBox("Œ¯Ë·Í‡ ÍÓÔËÓ‚‡ÌËˇ" + vbNewLine + vbNewLine + t + vbNewLine + vbNewLine + ex.ToString)
                            End Try
                        End If
                        AddImage(t)
                        Image(NImages).Transparency = 0
                        Image(NImages).DestTransparency = 1

                        Image(NImages).Loaded = False
                        IsNotEverithingLoaded = True

                        Image(NImages).X = x - Wire.X / 2
                        Image(NImages).Y = y - Wire.Y / 2
                        Image(NImages).DestX = Image(NImages).X '+ r.Next(-Wire.X, Wire.X)
                        Image(NImages).DestY = Image(NImages).Y '+ r.Next(-Wire.Y, Wire.Y)

                        'Dim ii As Short
                        'If Arrangement = ArrangmentTypes.Õorizontal Then
                        '    x = x - Canvas.Plus : y = y - Wire.dY
                        '    ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
                        'Else
                        '    y = y - Canvas.Plus : x = x - Wire.dX
                        '    ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
                        'End If
                        If ii > NImages Then ii = NImages
                        For i As Long = NImages To ii + 1 Step -1
                            SwapImages(i, i - 1)
                        Next
                        SelectedImageIndex = ii
                    End If
                Next
                RecalculateCanvasParameters()
                For i As Long = 1 To NImages
                    SetImageDestination(i)
                Next
            Else
                For Each t In files
                    SetImageDestination(IsInList(t))
                Next
            End If
        End If

        NDraggingFiles = 0
        IsAnimatedImages = True
        RedrawOnce = True
        DragDropCandidateIndex = -1
    End Sub
    Private Sub ucImagesBox_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DragLeave
        DragDropCandidateIndex = -1
        IsSmthDragging = False
        For n As Long = 1 To NDraggingFiles
            'Image(DraggingFilesList(n)).Transparency = 0
            SetImageDestination(DraggingFilesList(n))
            Image(DraggingFilesList(n)).DestTransparency = 1
        Next
        MDTime = 0
        'OrderImages()
        IsAnimatedImages = True
        NDraggingFiles = 0
        If IStartedDragDrop = False Then RedrawOnce = True Else IStartedDragDrop = False
    End Sub
#End Region
#Region "--------------------------|  Mouse Events  "
    Private Sub CorrectCanvasDestY()
        If Canvas.DestY > 0 Then Canvas.DestY = 0
        If Canvas.DestY < Canvas.MinY Then Canvas.DestY = Canvas.MinY
    End Sub
    Private Sub CorrectCanvasDestX()
        If Canvas.DestX > 0 Then Canvas.DestX = 0
        If Canvas.DestX < Canvas.MinX Then Canvas.DestX = Canvas.MinX
    End Sub
    Private Sub picPhoto_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDown ' , picMain.'Me.MouseDown
        'If Allow Then
        If Me.Focused = False Then Me.Select()

        'If e.Button = Windows.Forms.MouseButtons.Left Then LeftMouseButton = True
        'If e.Button = Windows.Forms.MouseButtons.Right Then RightMouseButton = True
        FirstX = e.X : FirstY = e.Y
        FreeCanvasMoving = False
        FreeCanvasMovingAlloud = True
        MDMoment = DateTime.Now
        LastBigMouseMoveTime = DateTime.Now
        LastMouseMoveTime = DateTime.Now
        MDTime = 1

        CursorMoved = False
        ChosenObj = 0
        RightButtonPressedByPonter = False
        IsMouseDown = True

        Dim a As ButtonStruct
        Dim i As Long
        For i = 0 To Buttons.Length - 1
            a = Buttons(i)
            If a.Visible = True Then
                If e.X >= a.X And e.X <= a.X + a.Image.Width And e.Y >= a.Y And e.Y <= a.Y + a.Image.Height Then
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
                            SortImagesByType()
                        Case 13
                            RandomFiles()

                        Case 2
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
            End If
        Next
        'If ChosenObj <> -6 Then HideSortingVariants()

        Dim p As New Point(e.X - Canvas.X, e.Y - Canvas.Y)
        PrevCursorPos.X = e.X : PrevCursorPos.Y = e.Y
        PrevPrevCursorPos.X = PrevCursorPos.X : PrevPrevCursorPos.Y = PrevCursorPos.Y

        If ChosenObj = 0 Then
            Dim founded As Boolean = False
            MDTime = 1
            Canvas.V = 0
            Canvas.VX = 0
            Canvas.Animate = False
            For i = 1 To NImages
                'i = ii 'foto_p(ii)
                If Image(i).Loaded Then
                    If p.X >= Image(i).X And p.X < Image(i).X + Image(i).WidthWithText And p.Y >= Image(i).Y And p.Y < Image(i).Y + Wire.Y Then
                        founded = True
                        Image(i).Animate = False : ChosenObj = i
                        SelectedImageIndex = i
                        SelInt = 1
                        Exit For
                    End If
                Else
                    If p.X >= Image(i).X And p.X < Image(i).X + Wire.X And p.Y >= Image(i).Y And p.Y < Image(i).Y + Wire.Y Then
                        founded = True
                        Image(i).Animate = False : ChosenObj = i
                        SelectedImageIndex = i
                        SelInt = 1
                        Exit For
                    End If
                End If
            Next
            If Not founded And (e.Button = Windows.Forms.MouseButtons.Right Or (Control.ModifierKeys = Keys.Alt Or Control.ModifierKeys = Keys.Shift)) Then
                ChosenObj = -1111
            End If
        End If
        RedrawOnce = True
    End Sub
    Dim RightButtonPressedByPonter As Boolean = False
    Dim FreeCanvasMoving As Boolean = False
    Dim FreeCanvasMovingAlloud As Boolean = True
    Dim LastMouseMoveTime As DateTimeOffset
    Dim LastBigMouseMoveTime As DateTimeOffset
    Dim LastBigMousePosition As Point
    Private Sub picPhoto_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseMove 'Me.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None And (PrevCursorPos.X <> e.X Or PrevCursorPos.Y <> e.Y) Then
            LastMouseMoveTime = DateTime.Now
            If CursorMoved = False Then
                If (FirstX - e.X) ^ 2 + (FirstY - e.Y) ^ 2 > 5 ^ 2 Then
                    CursorMoved = True
                    If (ChosenObj >= 0 And e.Button = Windows.Forms.MouseButtons.Left And (Not RightButtonPressedByPonter)) Then ChosenObj = 0
                    If ChosenObj = 0 And RightButtonPressedByPonter Then ChosenObj = -1111
                End If
            End If

            If (ChosenObj >= 0 And e.Button = Windows.Forms.MouseButtons.Left And (Not RightButtonPressedByPonter)) Then
                If ((FirstX - e.X) ^ 2 + (FirstY - e.Y) ^ 2) > 15 ^ 2 And FreeCanvasMovingAlloud Then
                    Dim angle As Double = Math.Atan2(FirstY - e.Y, FirstX - e.X) * 180 / Math.PI
                    If Arrangement = ArrangmentTypes.Õorizontal Then
                        If angle < 0 Then angle = -angle
                        If 90 + 40 < angle Or angle < 90 - 40 Then FreeCanvasMoving = True
                    Else
                        If angle < 0 Then angle += 180
                        If 40 < angle And angle < 180 - 40 Then FreeCanvasMoving = True
                    End If
                    angle = Math.Atan2(FirstY - e.Y, FirstX - e.X) * 180 / Math.PI + 360
                    While angle > 90
                        angle -= 90
                    End While
                    If angle > 25 And angle < 55 Then ChosenObj = -2222 : FreeCanvasMoving = False
                    FreeCanvasMovingAlloud = False
                End If

                If FreeCanvasMovingAlloud Then
                    'Canvas.Animate = False
                    If Arrangement = ArrangmentTypes.Õorizontal Then Canvas.Y += e.Y - PrevCursorPos.Y Else Canvas.X += e.X - PrevCursorPos.X
                    'Canvas.DestX = Canvas.X
                    'Canvas.DestY = Canvas.Y
                ElseIf FreeCanvasMoving Then
                    If Arrangement <> ArrangmentTypes.Õorizontal Then Canvas.Y += e.Y - PrevCursorPos.Y Else Canvas.X += e.X - PrevCursorPos.X
                    'If Arrangement <> ArrangmentTypes.Õorizontal Then Canvas.Y += e.Y - PrevCursorPos.Y : Canvas.X = 0 Else Canvas.X += e.X - PrevCursorPos.X : Canvas.Y = 0
                Else
                    If Arrangement = ArrangmentTypes.Õorizontal Then Canvas.Y += e.Y - PrevCursorPos.Y Else Canvas.X += e.X - PrevCursorPos.X
                    'If Arrangement = ArrangmentTypes.Õorizontal Then Canvas.Y += e.Y - PrevCursorPos.Y : Canvas.X = 0 Else Canvas.X += e.X - PrevCursorPos.X : Canvas.Y = 0
                End If
                PrevPrevCursorPos.X = PrevCursorPos.X : PrevPrevCursorPos.Y = PrevCursorPos.Y
                PrevCursorPos.X = e.X : PrevCursorPos.Y = e.Y
            ElseIf (ChosenObj > 0 And (e.Button = Windows.Forms.MouseButtons.Right Or RightButtonPressedByPonter)) Then
                Image(ChosenObj).X += e.X - PrevCursorPos.X : Image(ChosenObj).Y += e.Y - PrevCursorPos.Y
                PrevCursorPos.X = e.X : PrevCursorPos.Y = e.Y
                If CursorMoved Then StartDragDrop(ChosenObj) : Exit Sub
            ElseIf ChosenObj = -3 Then
                Canvas.Y += (e.Y - PrevCursorPos.Y) / ((Me.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) / Canvas.MinY)
            ElseIf ChosenObj = -1111 And CursorMoved Then
                Dim x, i1, i2, t, b, i As Long
                i1 = XYtoXIndex(e.X - Canvas.X, e.Y - Canvas.Y)
                i2 = XYtoXIndex(FirstX - Canvas.X, FirstY - Canvas.Y)
                t = XYtoYIndex(e.X - Canvas.X, e.Y - Canvas.Y)
                b = XYtoYIndex(FirstX - Canvas.X, FirstY - Canvas.Y)
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
                    If Control.ModifierKeys = Keys.Alt Then
                        If (x >= t And x <= b) And i >= i1 And i <= i2 Then Image(i).PreSelected = True Else Image(i).PreSelected = False
                    Else
                        'End If
                        'If IsShiftDown Then
                        If (x >= t And x <= b) And i >= i1 And i <= i2 Then Image(i).PreSelected = True Else Image(i).PreSelected = False
                    End If
                Next
            End If
            PrevCursorPos.X = e.X : PrevCursorPos.Y = e.Y '!!!!!!!!!!!!!!!!!!!!!!!!
        End If
    End Sub
    Private Function XYtoXIndex(ByVal x As Long, ByVal y As Long) As Long
        If Arrangement = ArrangmentTypes.Õorizontal Then
            x = x - Canvas.Plus : y = y - Wire.dY
            Return (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
        Else
            y = y - Canvas.Plus : x = x - Wire.dX
            Return (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        End If
    End Function
    Private Function XYtoYIndex(ByVal x As Long, ByVal y As Long) As Long
        If Arrangement = ArrangmentTypes.Õorizontal Then
            x = x - Canvas.Plus
            Return Math.Truncate(x / (Wire.X + Wire.dX)) + 1
        Else
            y = y - Canvas.Plus
            Return Math.Truncate(y / (Wire.Y + Wire.dY)) + 1
        End If
    End Function
    Private Function XYtoIndex(ByVal x As Long, ByVal y As Long) As Long
        If Arrangement = ArrangmentTypes.Õorizontal Then
            x = x - Canvas.Plus : y = y - Wire.dY
            Return (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        Else
            y = y - Canvas.Plus : x = x - Wire.dX
            Return (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
        End If
    End Function
    Private Function CorrectIndex(ByVal i As Long) As Long
        If i > NImages Then i = NImages
        If i < 0 Then i = 0
        Return i
    End Function
    Private Async Sub picPhoto_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseUp 'Me.MouseUp
        IsMouseDown = False
        If Windows.Forms.MouseButtons.Left Or Windows.Forms.MouseButtons.Right Then
            'If e.Button = Windows.Forms.MouseButtons.Left Then LeftMouseButton = False ': txt1.Text += vbNewLine + "left up"
            'If e.Button = Windows.Forms.MouseButtons.Right Then RightMouseButton = False ': txt1.Text += vbNewLine + "right up"
            'If moved = False And RightButtonPressedByPonter Then Allow = False : AllowDC = False : tmrWait.Enabled = True : tmrWaitForDC.Enabled = True
            If ChosenObj = -1111 Then
                If Control.ModifierKeys = Keys.Alt Then
                    For i As Integer = 1 To NImages
                        If Image(i).PreSelected = True Then Image(i).Selected = False : Image(i).PreSelected = False
                    Next
                Else
                    For i As Integer = 1 To NImages
                        If Image(i).PreSelected = True Then Image(i).Selected = True : Image(i).PreSelected = False
                    Next
                End If
            End If
            If ChosenObj = 0 Then
                If (DateTime.Now - LastMouseMoveTime).TotalSeconds < 0.08 Then
                    If Arrangement = ArrangmentTypes.Õorizontal Then
                        If FreeCanvasMoving Then
                            Canvas.VX = e.X - PrevPrevCursorPos.X
                        Else
                            Canvas.V = e.Y - PrevPrevCursorPos.Y
                        End If
                    Else
                        If FreeCanvasMoving Then
                            Canvas.V = e.Y - PrevPrevCursorPos.Y
                        Else
                            Canvas.VX = e.X - PrevPrevCursorPos.X
                        End If
                    End If
                End If
            End If
            If CursorMoved = False And e.Button = Windows.Forms.MouseButtons.Left And Not RightButtonPressedByPonter And Not CursorMoved Then 'And ChosenObj > 0 Then
                If ChosenObj > 0 Then
                    If SelectionMode Then
                        Image(SelectedImageIndex).Selected = Not Image(SelectedImageIndex).Selected
                    Else
                        'SelInt = -1
                        If ModifierKeys = Keys.None Then
                            If Image(SelectedImageIndex).Type = FileTypes.Image Then
                                If Path <> "" Then
                                    If Mid(Path, 1, Path.Length - 2) = Application.StartupPath + "\35photo" Then
                                        Process.Start(URLs35photo(SelectedImageIndex))
                                    Else
                                        ShowPhoto(SelectedImageIndex)
                                    End If
                                Else
                                    ShowPhoto(SelectedImageIndex)
                                End If
                            ElseIf Image(SelectedImageIndex).Type = FileTypes.Folder Or Image(SelectedImageIndex).Type = FileTypes.Drive Then
                                startX = Image(SelectedImageIndex).X
                                startY = Image(SelectedImageIndex).Y
                                RaiseEvent ChangeDir(Image(SelectedImageIndex).FileName, False)
                            Else
                                If Image(SelectedImageIndex).Selected Then
                                    Dim Fail As Boolean = False
                                    Dim Str As String = ""
                                    For i As Long = 1 To NImages
                                        If Image(i).Selected Then
                                            Str = Str + " " + Chr(34) + Image(i).FileName + Chr(34)
                                            If Image(i).Type <> FileTypes.Music Then Fail = True

                                            If i <> SelectedImageIndex Then FileTags.Tags(Image(i).InTagsIngex).LaunchingTimes += 1
                                        End If
                                    Next
                                    Str = Mid(Str, 2)
                                    If Not Fail Then
                                        Dim r As Microsoft.Win32.RegistryKey
                                        r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".mp3")
                                        Dim Alias1 As String = r.GetValue("")
                                        r.Close()
                                        r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Alias1)
                                        r = r.OpenSubKey("shell")

                                        If Control.ModifierKeys <> Keys.Shift Then
                                            r = r.OpenSubKey("open")
                                        Else
                                            r = r.OpenSubKey("Enqueue")
                                        End If
                                        r = r.OpenSubKey("command")

                                        Dim ShellStr As String = r.GetValue("")
                                        r.Close()
                                        ShellStr = ShellStr.Replace(Chr(34) + "%1" + Chr(34), Str)
                                        Shell(ShellStr, AppWinStyle.NormalNoFocus)
                                    End If
                                Else
                                    Try
                                        RaiseEvent FileClick(Image(SelectedImageIndex))
                                    Catch
                                    End Try
                                End If
                            End If
                            FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).LaunchingTimes += 1
                        ElseIf ModifierKeys = Keys.Control Then
                            Image(ChosenObj).Selected = Not Image(ChosenObj).Selected
                        ElseIf ModifierKeys = Keys.Shift Then
                            Dim st As Short = 1
                            If StartSelectionFrom > SelectedImageIndex Then st = -1
                            For i As Long = StartSelectionFrom To SelectedImageIndex Step st
                                Image(i).PreSelected = True
                            Next
                        End If
                    End If
                Else
                    For i As Long = 1 To NImages
                        Image(i).Selected = False
                    Next
                End If
            ElseIf CursorMoved = False And (e.Button = Windows.Forms.MouseButtons.Right Or RightButtonPressedByPonter) Then
                If ChosenObj > 0 Then
                    Dim box As New frmMenu()
                    Dim items(5) As String
                    items(0) = "start"
                    items(1) = "rename"
                    items(2) = "delete"
                    items(3) = "copy"
                    items(4) = "cut"
                    items(5) = "select"
                    If Image(SelectedImageIndex).Selected Then items(5) = "unselect"

                    box.Items = items
                    box.NItems = 5
                    box.ShowDialog()

                    Select Case box.Ansver
                        Case 0

                            Try
                                If Image(ChosenObj).Type = FileTypes.Music Then
                                    'Process.Start("http://www.google.ru/search?client=alpha_finder&rls=en&q=" + (FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Singer + " - " + FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Name + " lyrics").Replace(" ", "+") + "&ie=UTF-8&oe=UTF-8")
                                    Dim Q As String = FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Singer + " "
                                    If InStr(FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Name, "(") Then
                                        Q += Mid(FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Name, 1, -1 + InStr(FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Name, "("))
                                    Else
                                        Q += FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Name
                                    End If
                                    Q = Q.Replace(" ", "+")

                                    If IO.File.Exists(Application.StartupPath + "\lirics.txt") Then IO.File.Delete(Application.StartupPath + "\lirics.txt")
                                    My.Computer.Network.DownloadFile("http://www.lyrics007.com/search.php?q=" + Q + "&submit=go", Application.StartupPath + "\lirics.txt", False, 1000)
                                    'Process.Start("http://www.lyrics007.com/search.php?q=" + Q + "&submit=go")

                                    Dim htm As String = IO.File.ReadAllText(Application.StartupPath + "\lirics.txt")

                                    Dim pattern1 As String = "<div class=" + Chr(34) + "content" + Chr(34) + ">"
                                    Dim pattern2 As String = "<a href=" + Chr(34) + "click.php?url="

                                    Dim pos As Long = InStr(htm, pattern1)
                                    htm = Mid(htm, pos)
                                    pos = InStr(htm, pattern2)
                                    If pos <> 0 Then
                                        htm = Mid(htm, pos + pattern2.Length)

                                        If IO.File.Exists(Application.StartupPath + "\lirics.txt") Then IO.File.Delete(Application.StartupPath + "\lirics.txt")
                                        My.Computer.Network.DownloadFile(Mid(htm, 1, InStr(htm, Chr(34)) - 1), Application.StartupPath + "\lirics.txt", False, 1000)

                                        htm = IO.File.ReadAllText(Application.StartupPath + "\lirics.txt")

                                        Dim pattern12 As String = "<div class=" + Chr(34) + "content" + Chr(34) + ">"
                                        Dim pattern22 As String = "</fb:like>"
                                        Dim pattern32 As String = "<a href="

                                        pos = InStr(htm, pattern12)
                                        htm = Mid(htm, pos + pattern12.Length)
                                        pos = InStr(htm, pattern22)
                                        htm = Mid(htm, pos + pattern22.Length)
                                        If pos <> 0 Then
                                            Dim frmL As New frmLyrics
                                            frmL.txt.Text = (Mid(htm, 1, InStr(htm, pattern32))).Replace("<br>", vbNewLine).Replace("<BR>", vbNewLine).Replace(vbNewLine + vbNewLine, vbNewLine)
                                            Q = FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Singer + " - "
                                            Q += FileTags.Tags(Image(ChosenObj).InTagsIngex).Song.Name
                                            frmL.Text = Q
                                            frmL.Show()
                                        End If
                                    End If
                                Else
                                    'MsgBox(Environment.SystemDirectory + "\mspaint.exe")
                                    Shell(Environment.SystemDirectory + "\mspaint.exe" + " " + Chr(34) + Image(ChosenObj).FileName + Chr(34), AppWinStyle.NormalFocus)
                                    'ShowOpenList(ChosenObj)
                                End If
                            Catch
                            End Try
                        Case 1
                            Rename(ChosenObj)
                        Case 2
                            Delete(ChosenObj)
                        Case 3
                            CopyToClipboard()
                        Case 4
                            CutToClipboard()
                        Case 5
                            Image(SelectedImageIndex).Selected = Not Image(SelectedImageIndex).Selected
                    End Select
                    FlyingAlgorithm = FlyingAlgorithms.Simple
                    SetImageDestination(ChosenObj)
                    IsAnimatedImages = True
                    box.Dispose()
                Else
                    Dim box As New frmMenu()
                    Dim items(3) As String

                    Dim data As IDataObject = Clipboard.GetDataObject()
                    Dim lst() As String = data.GetData(DataFormats.FileDrop)

                    items(0) = "selection"
                    items(1) = "paste"
                    If Not (lst Is Nothing) Then items(1) += " (" + lst.Length.ToString + ")"
                    items(2) = "order"
                    items(3) = "add file"
                    box.Items = items : box.NItems = 3 : box.w = 140
                    box.ShowDialog()
                    Select Case box.Ansver
                        Case 0
                            Dim box2 As New frmMenu()
                            Dim items2(2) As String
                            items2(0) = "select all" : items2(1) = "select nothing" : items2(2) = "invert"
                            box2.Items = items2 : box2.NItems = 2 : box2.w = 160
                            box2.ShowDialog()
                            Select Case box2.Ansver
                                Case 0
                                    For i As Long = 1 To NImages
                                        Image(i).Selected = True
                                    Next
                                Case 1
                                    For i As Long = 1 To NImages
                                        Image(i).Selected = False
                                    Next
                                Case 2
                                    For i As Long = 1 To NImages
                                        Image(i).Selected = Not Image(i).Selected
                                    Next
                            End Select
                        Case 1
                            PasteFromClipboard(e.X, e.Y)
                        Case 2
                            FlyingAlgorithm = FlyingAlgorithms.Simple
                            Dim box2 As New frmMenu()
                            Dim items2(6) As String
                            items2(0) = "by name" : items2(1) = "by rating" : items2(2) = "by singer" : items2(3) = "by pop" : items2(4) = "by type" : items2(5) = "by date" : items2(6) = "random"
                            box2.Items = items2 : box2.NItems = 6 : box2.w = 130
                            box2.ShowDialog()
                            'Dim WasX As Long = Image(SelectedImageIndex).X
                            'Dim WasY As Long = Image(SelectedImageIndex).Y
                            Select Case box2.Ansver
                                Case 0
                                    SortImagesByName()
                                Case 1
                                    SortImagesByRating()
                                Case 2
                                    SortImagesBySinger()
                                Case 3
                                    SortImagesByLaunchingCount()
                                Case 4
                                    SortImagesByType()
                                Case 5
                                    SortImagesByDate()
                                Case 6
                                    RandomFiles()
                            End Select
                            OrderImages()
                            'Canvas.DestX = Canvas.X - Image(SelectedImageIndex).DestX + WasX
                            'Canvas.DestY = Canvas.Y - Image(SelectedImageIndex).DestY + WasY
                            'Canvas.Animate = True
                        Case 3 '!!!!!!!!!!!! REMAKE
                            IO.File.Create(Path + "new_file.txt")
                    End Select
                End If
            End If
            ChosenObj = 0
            MDTime = 0
            RedrawOnce = True
            If ChosenObj <> 0 Then OrderImages()
        End If
    End Sub
    Private Sub ucImagesBox_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If Arrangement = ArrangmentTypes.Õorizontal Then
            If Canvas.Animate = False And Canvas.DestY <> Canvas.Y Then Canvas.DestY = Canvas.Y
            If Canvas.Y < 0 And e.Delta > 0 Or e.Delta < 0 And Canvas.Y > Canvas.MinY Then
                Canvas.DestY = Canvas.DestY + e.Delta 'Canvas.Y = Canvas.DestY - 0.7 'ËÎË ·ÂÁ ÌÂ∏
                Canvas.Animate = True
            End If
            If Canvas.DestY > 0 Then
                Dim f As Boolean = True : RaiseEvent SendFocusToTheTop(f)
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
            If Canvas.X < 0 And e.Delta > 0 Or e.Delta < 0 And Canvas.X > Canvas.MinX Then
                Canvas.DestX = Canvas.DestX + e.Delta 'Canvas.X = Canvas.DestX - 0.7 'ËÎË ·ÂÁ ÌÂ∏
                Canvas.Animate = True
            End If
            If Canvas.X > 0 Then
                Dim f As Boolean = True : RaiseEvent SendFocusToTheTop(f)
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
    End Sub
    Private Sub picMain_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDoubleClick
        If ChosenObj = 0 Then 'And AllowDC Then
            RaiseEvent FillScreenMe()
        End If
    End Sub
    Private Sub ucImagesBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.ShiftKey Then
            For i As Long = 1 To NImages
                If Image(i).PreSelected Then Image(i).PreSelected = False : Image(i).Selected = True
            Next
        End If

        If e.KeyCode = Keys.ShiftKey Then IsShiftDown = False
        If e.KeyCode = Keys.ControlKey Then IsCtrlDown = False
        If e.KeyCode = 18 Then IsAltDown = False

        'IsEnterDown = False
        RedrawOnce = True
    End Sub
#End Region

    Public Sub PasteFromClipboard(Optional eX As Long = 0, Optional eY As Long = 0)
        Dim data As IDataObject = Clipboard.GetDataObject()
        Dim lst() As String = data.GetData(DataFormats.FileDrop)

        If Not (lst Is Nothing) Then
            Dim stream As IO.MemoryStream = data.GetData("Preferred DropEffect")
            If Not (stream Is Nothing) Then
                Dim bytes(stream.Length) As Byte
                stream.Read(bytes, 0, bytes.Length)
                Dim bit As Byte = bytes(0)

                StopLoading = True
                For Each t As String In lst
                    If Not IsInList(t) Then
                        If Path <> "" Then
                            Try
                                If My.Computer.FileSystem.FileExists(t) Then
                                    If LCase(t) <> LCase(Path + GetFileName(t)) Then
                                        If bit <> 2 Then
                                            My.Computer.FileSystem.CopyFile(t, Path + GetFileName(t))
                                        Else
                                            My.Computer.FileSystem.MoveFile(t, Path + GetFileName(t))
                                        End If
                                    End If
                                    t = Path + GetFileName(t)
                                End If
                                If My.Computer.FileSystem.DirectoryExists(t) Then
                                    If LCase(t) <> LCase(Path + GetFileName(t)) Then
                                        If bit <> 2 Then
                                            My.Computer.FileSystem.CopyDirectory(t, Path + GetFileName(t))
                                        Else
                                            My.Computer.FileSystem.MoveDirectory(t, Path + GetFileName(t))
                                        End If
                                    End If
                                    t = Path + GetFileName(t)
                                End If
                            Catch exc As Exception
                                MsgBox("Œ¯Ë·Í‡ ÓÔÂ‡ˆËË Ò Ù‡ÈÎÓÏ" + vbNewLine + vbNewLine + t + vbNewLine + vbNewLine + exc.ToString)
                            End Try
                        End If
                        AddImage(t)
                        Image(NImages).Transparency = 0
                        Image(NImages).DestTransparency = 1

                        Image(NImages).Loaded = False
                        IsNotEverithingLoaded = True

                        Image(NImages).X = eX - Canvas.X
                        Image(NImages).Y = eY - Canvas.Y

                        Dim x = Image(NImages).X
                        Dim y = Image(NImages).Y

                        Dim ii As Short
                        If Arrangement = ArrangmentTypes.Õorizontal Then
                            x = x - Canvas.Plus : y = y - Wire.dY
                            ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
                        Else
                            y = y - Canvas.Plus : x = x - Wire.dX
                            ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
                        End If
                        If ii > NImages Then ii = NImages
                        If ii < 1 Then ii = 1
                        For i As Long = NImages To ii + 1 Step -1
                            SwapImages(i, i - 1)
                            SetImageDestination(i)
                            IsAnimatedImages = True
                        Next
                        SelectedImageIndex = ii
                    End If
                    StopLoading = False
                Next
                StopLoading = False
                MDTime = 0
                OrderImages()
                IsAnimatedImages = True
                IsSmthDragging = False
                RedrawOnce = True
            End If
        End If
    End Sub
    Public Sub CutToClipboard()
        Dim DObject As DataObject
        If Image(SelectedImageIndex).Selected Then
            Dim count As Long = -1
            For I As Long = 1 To NImages
                If Image(I).Selected Then
                    count += 1
                End If
            Next
            Dim lst(count) As String
            count = 0
            For I As Long = 1 To NImages
                If Image(I).Selected Then
                    lst(count) = Image(I).FileName
                    count += 1
                End If
            Next
            DObject = New DataObject(DataFormats.FileDrop, lst)
        Else
            DObject = New DataObject(DataFormats.FileDrop, New String() {Image(SelectedImageIndex).FileName})
        End If

        Dim memo As New IO.MemoryStream()
        memo.Write(New Byte() {2, 0, 0, 0}, 0, 4)
        memo.SetLength(4)
        DObject.SetData("Preferred DropEffect", memo)
        Clipboard.SetDataObject(DObject)
    End Sub
    Public Sub CopyToClipboard()
        Dim DObject As DataObject
        If Image(SelectedImageIndex).Selected Then
            Dim count As Long = -1
            For I As Long = 1 To NImages
                If Image(I).Selected Then
                    count += 1
                End If
            Next
            Dim lst(count) As String
            count = 0
            For I As Long = 1 To NImages
                If Image(I).Selected Then
                    lst(count) = Image(I).FileName
                    count += 1
                End If
            Next
            DObject = New DataObject(DataFormats.FileDrop, lst)
        Else
            DObject = New DataObject(DataFormats.FileDrop, New String() {Image(SelectedImageIndex).FileName})
        End If

        Dim memo As New IO.MemoryStream()
        memo.Write(New Byte() {5, 0, 0, 0}, 0, 4)
        memo.SetLength(4)
        DObject.SetData("Preferred DropEffect", memo)
        Clipboard.SetDataObject(DObject)
    End Sub
    Private Sub ShowOpenList(i As Short)
        Dim Fail As Boolean = False
        Dim Str As String = ""
        'For i As Long = 1 To NImages
        '    If Image(i).Selected Then
        '        Str = Str + " " + Chr(34) + Image(i).FileName + Chr(34)
        '        If FileTags.Tags(Image(i).InTagsIngex).Type <> "music" Then Fail = True
        '    End If
        'Next
        'Str = Mid(Str, 2)
        'If Not Fail Then
        Dim r As Microsoft.Win32.RegistryKey
        r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(IO.Path.GetExtension(Image(i).FileName))
        Dim DefAlias As String = r.GetValue("")
        If DefAlias <> "" Then
            r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(DefAlias)
            If Not r Is Nothing Then
                r = r.OpenSubKey("shell")
                If Not r Is Nothing Then
                    r = r.OpenSubKey("open")
                    If Not r Is Nothing Then
                        r = r.OpenSubKey("command")

                        Dim ShellStr As String = r.GetValue("")
                        r.Close()

                        MsgBox(ShellStr)
                    End If
                End If
            End If
        End If

        r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(IO.Path.GetExtension(Image(i).FileName))
        r = r.OpenSubKey("OpenWithProgids")
        If Not (r Is Nothing) Then
            Dim Aliases() As String = r.GetValueNames()
            r.Close()
            For ii As Long = 1 To Aliases.Length - 1

                r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Aliases(ii))
                r = r.OpenSubKey("shell")
                r = r.OpenSubKey("open")
                If Not r Is Nothing Then
                    r = r.OpenSubKey("command")

                    Dim ShellStr As String = r.GetValue("")
                    If ShellStr = "" Then
                        ShellStr = r.GetValue("DelegateExecute")
                        If Not r Is Nothing Then
                            r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(ShellStr)

                        End If
                    End If
                    r.Close()

                    MsgBox(Aliases(ii))
                    MsgBox(ShellStr)
                End If
            Next
        End If
        'ShellStr = ShellStr.Replace(Chr(34) + "%1" + Chr(34), Str)
        'Shell(ShellStr, AppWinStyle.NormalNoFocus)
        'End If
    End Sub
    Private Sub ShowPhoto(i As Long)
        Dim a As New frmShowPhoto2
        Dim FN(NImages) As String
        Dim fnn As Long = 0
        For j As Long = 1 To NImages
            If Image(j).Type = FileTypes.Image Then
                fnn += 1
                FN(fnn) = Image(j).FileName
                If i = j Then i = fnn
            End If
        Next
        ReDim Preserve FN(fnn)
        If Image(i).Loaded = True Then
            a.InitFullScreened(Thumbnail(i).Clone(), i, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
        Else
            a.InitFullScreened(bmp_error.Clone(), i, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
        End If
        a.Show()
    End Sub
    Public Sub ShowPhoto(Path As String)
        Dim a As New frmShowPhoto2
        Dim FN(0) As String
        FN(0) = Path
        a.InitFullScreened(bmp_error.Clone(), 0, FN) ', Parent.Left + Me.Left + Image(SelectedImageIndex).X, Parent.Top + Me.Top + Image(SelectedImageIndex).Y + Canvas.Y)
        a.Show()
    End Sub
#Region "--------------------------|  KEYS & Choosing Images  "
    Sub CorrectSelectedImageIndex()
        If SelectedImageIndex <= 0 Then SelectedImageIndex = 1
        If SelectedImageIndex > NImages Then SelectedImageIndex = NImages
    End Sub
    Dim InputLine As String = " "
    Dim StartSelectionFrom As Long
    Private Sub ChooseObj(ByVal key As Keys)
        Select Case key
            Case Keys.End
                SelectedImageIndex = NImages
            Case Keys.Home
                SelectedImageIndex = 1
            Case Keys.PageDown
                SelectedImageIndex += Canvas.LinesInBox * Canvas.Columns
            Case Keys.PageUp
                SelectedImageIndex -= Canvas.LinesInBox * Canvas.Columns
            Case Keys.Down
                If Arrangement <> ArrangmentTypes.Õorizontal Then
                    SelectedImageIndex += 1
                Else
                    SelectedImageIndex += Canvas.Columns
                End If
            Case Keys.Up
                If SelectedImageIndex <= 1 Then
                    Dim a As Boolean = True
                    RaiseEvent SendFocusToTheTop(a)
                    If a = False Then
                        If Arrangement <> ArrangmentTypes.Õorizontal Then
                            SelectedImageIndex -= 1
                        Else
                            SelectedImageIndex -= Canvas.Columns
                        End If
                    End If
                Else
                    If Arrangement <> ArrangmentTypes.Õorizontal Then
                        SelectedImageIndex -= 1
                    Else
                        SelectedImageIndex -= Canvas.Columns
                    End If
                End If
            Case Keys.Left
                If Arrangement <> ArrangmentTypes.Õorizontal Then
                    SelectedImageIndex -= Canvas.Columns
                Else
                    SelectedImageIndex -= 1
                End If
            Case Keys.Right
                If Arrangement <> ArrangmentTypes.Õorizontal Then
                    SelectedImageIndex += Canvas.Columns
                Else
                    SelectedImageIndex += 1
                End If
        End Select
        CorrectSelectedImageIndex()
    End Sub
    Dim IsShiftDown As Boolean = False
    Dim IsAltDown As Boolean = False
    Dim IsCtrlDown As Boolean = False
    Dim TimeFromTyping As DateTimeOffset

    'Public Sub PositioningKey(keycode As Windows.Forms.KeyEventArgs)
    '    ChooseObj(e.KeyCode)
    '    SetCanvas()
    '    'If Not Animation Then NextFrame(True)'!!!! LOOK! Func 'is there reasons to redraw'
    '    RedrawOnce = True
    'End Sub
    Public Sub SelectAll()
        For i As Long = 1 To NImages
            Image(i).Selected = True
            Image(i).PreSelected = False
        Next
    End Sub
    Public Sub DeselectAll()
        For i As Long = 1 To NImages
            Image(i).Selected = False
            Image(i).PreSelected = False
        Next
    End Sub
    Public Sub InvertSelection()
        For i As Long = 1 To NImages
            Image(i).Selected = Not Image(i).Selected
            Image(i).PreSelected = False
        Next
    End Sub
    Public Sub SelectCurrentItem()
        For i As Long = 1 To NImages
            Image(SelectedImageIndex).Selected = True
            Image(SelectedImageIndex).PreSelected = False
        Next
    End Sub
    Public Sub DeselectCurrentItem()
        For i As Long = 1 To NImages
            Image(SelectedImageIndex).Selected = False
            Image(SelectedImageIndex).PreSelected = False
        Next
    End Sub


    Public Sub ucImagesBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.ShiftKey Then IsShiftDown = True
        If e.KeyCode = Keys.ControlKey Then IsCtrlDown = True
        If e.KeyCode = 18 Then IsAltDown = True

        If (Mid(InputLine, 1, 2) = "CD") Or (e.Control = False And e.Shift = False And e.Alt = False) And ((Chr(e.KeyValue) >= "A" And Chr(e.KeyValue) <= "Z") Or (Chr(e.KeyValue) >= "¿" And Chr(e.KeyValue) <= "ﬂ")) Then
            InputLine = InputLine + Chr(e.KeyCode)
            TimeFromTyping = System.DateTimeOffset.Now
        End If
        If InputLine.Length > 0 Then
            If InputLine = "¿RENAME" Then
                Rename(SelectedImageIndex)
            ElseIf Mid(InputLine, 1, 3) = "CD " And InputLine.Length >= 4 Then
                If My.Computer.FileSystem.DirectoryExists(Path + Mid(InputLine, 4) + "\") Then RaiseEvent ChangeDir(Path + Mid(InputLine, 4), True) ' MsgBox("cool!")
                If (Path = "" Or Path = "home") And My.Computer.FileSystem.DirectoryExists(Mid(InputLine, 4) + ":\") Then RaiseEvent ChangeDir(Mid(InputLine, 4) + ":", True) ' MsgBox("cool!")
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
                            If InStr(LCase(Image(i).OriginalName), str) > 0 Then SelectedImageIndex = i : Exit For
                        Next
                    End If
                    SetCanvas()
                End If
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SelInt = 1 '-1
            NextFrame(True) : Me.Refresh()
            'If Not IsEnterDown Then
            'IsEnterDown = True
            If SelectedImageIndex >= 1 And SelectedImageIndex <= NImages Then
                If Image(SelectedImageIndex).Type = FileTypes.Image Then
                    ShowPhoto(SelectedImageIndex)
                ElseIf Image(SelectedImageIndex).Type = FileTypes.Folder Or Image(SelectedImageIndex).Type = FileTypes.Drive Then
                    'FlyMode = True
                    startX = Image(SelectedImageIndex).X
                    startY = Image(SelectedImageIndex).Y
                    RaiseEvent ChangeDir(Image(SelectedImageIndex).FileName, False)
                Else
                    If Image(SelectedImageIndex).Selected Then
                        Dim Fail As Boolean = False
                        Dim Str As String = ""
                        For i As Long = 1 To NImages
                            If Image(i).Selected Then
                                Str = Str + " " + Chr(34) + Image(i).FileName + Chr(34)
                                If Image(i).Type <> FileTypes.Music Then Fail = True
                            End If
                        Next
                        Str = Mid(Str, 2)
                        If Not Fail Then
                            Dim r As Microsoft.Win32.RegistryKey
                            r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".mp3")
                            Dim Alias1 As String = r.GetValue("")
                            r.Close()
                            r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Alias1)
                            r = r.OpenSubKey("shell")

                            If Control.ModifierKeys <> Keys.Shift Then
                                r = r.OpenSubKey("open")
                            Else
                                r = r.OpenSubKey("Enqueue")
                            End If
                            r = r.OpenSubKey("command")

                            Dim ShellStr As String = r.GetValue("")
                            r.Close()
                            ShellStr = ShellStr.Replace(Chr(34) + "%1" + Chr(34), Str)
                            Shell(ShellStr, AppWinStyle.NormalNoFocus)
                        End If
                    Else
                        Try
                            RaiseEvent FileClick(Image(SelectedImageIndex))
                        Catch
                        End Try
                    End If
                End If
                FileTags.Tags(Image(SelectedImageIndex).InTagsIngex).LaunchingTimes += 1
            End If
        End If
        If e.Control = False And e.Alt = False And e.Shift = False Then
            If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Or e.KeyCode = Keys.PageDown Or e.KeyCode = Keys.PageUp Or e.KeyCode = Keys.Home Or e.KeyCode = Keys.End Then
                ChooseObj(e.KeyCode)
                SetCanvas()
                'If Not Animation Then NextFrame(True)'!!!! LOOK! Func 'is there reasons to redraw'
                RedrawOnce = True
            End If
            Select Case e.KeyCode
                Case Keys.Subtract
                    SetWire(Wire.X * 0.8, Wire.Y * 0.8, Wire.dX, Wire.dY)
                    ReloadAllThumbs()
                    OrderImages()
                Case Keys.Add
                    SetWire(Wire.X * 1.4, Wire.Y * 1.4, Wire.dX, Wire.dY)
                    ReloadAllThumbs()
                    OrderImages()

                Case Keys.Delete
                    SetCanvas()
                    Delete(SelectedImageIndex, True)
                Case Keys.Escape
                    StopLoading = True
                Case Keys.Back
                    RaiseEvent BackSpaceKey(False)
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
                            If i Mod 100 = 0 Then
                                CurrentLoadingImgIndex = i
                                DrawStatus()
                                picMain.Image = BmpMain
                                picMain.Refresh()
                            End If
                        End If
                    Next

                    IsNotEverithingLoaded = False
                    CurrentLoadingImgIndex = NImages + 1
                    If Path.Length > 3 Then
                        If Mid(Path, 1, Path.Length - 2) = Application.StartupPath + "\35photo" Then
                            Dim i As Long = 0
                            FileOpen(1, Path + "urls.txt", OpenMode.Input)
                            While Not EOF(1)
                                i += 1
                                URLs35photo(i) = LineInput(1)
                            End While
                            FileClose(1)
                        End If
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
                    Image(i).PreSelected = True
                Next
                RedrawOnce = True
                SetCanvas()
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
                RedrawOnce = True
                SetCanvas()
            End If
        End If
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.OemMinus
                    SetWire(Wire.X * 0.8, Wire.Y * 0.8, Wire.dX, Wire.dY)
                    ReloadAllThumbs()
                    OrderImages()
                Case Keys.Oemplus
                    SetWire(Wire.X * 1.4, Wire.Y * 1.4, Wire.dX, Wire.dY)
                    ReloadAllThumbs()
                    OrderImages()

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
                    Rename(SelectedImageIndex)

                Case Keys.C
                    CopyToClipboard()
                Case Keys.X
                    CutToClipboard()
                Case Keys.V
                    PasteFromClipboard()
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
                                My.Computer.FileSystem.RenameFile(old_str, Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str)
                                Image(i).FileName = Mid(old_str, 1, old_str.Length - IO.Path.GetFileName(old_str).Length) + str
                                FileTags.Files(Image(i).InTagsIngex) = Image(i).FileName
                                Image(i).OriginalName = str
                                Image(i).Name = str
                            Catch ex As Exception
                                MsgBox("ÕÂ Û‰‡∏ÚÒˇ ÔÂÂËÏÂÌÓ‚‡Ú¸ Ù‡ÈÎ")
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


        'NextFrame(True)
    End Sub
    Public Sub AddToPlaylist()
        If Image(SelectedImageIndex).Selected Then
            Dim Fail As Boolean = False
            Dim Str As String = ""
            For i As Long = 1 To NImages
                If Image(i).Selected Then
                    Str = Str + " " + Chr(34) + Image(i).FileName + Chr(34)
                    If Image(i).Type <> FileTypes.Music Then Fail = True
                End If
            Next
            Str = Mid(Str, 2)
            If Not Fail Then
                Dim r As Microsoft.Win32.RegistryKey
                r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".mp3")
                Dim Alias1 As String = r.GetValue("")
                r.Close()
                r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Alias1)
                r = r.OpenSubKey("shell")

                'If Control.ModifierKeys <> Keys.Shift Then
                '    r = r.OpenSubKey("open")
                'Else
                r = r.OpenSubKey("Enqueue")
                'End If
                r = r.OpenSubKey("command")

                Dim ShellStr As String = r.GetValue("")
                r.Close()
                ShellStr = ShellStr.Replace(Chr(34) + "%1" + Chr(34), Str)
                Shell(ShellStr, AppWinStyle.NormalNoFocus)
            End If
        Else
            Dim Fail As Boolean = False
            Dim Str As String = ""
            Str = Chr(34) + Image(SelectedImageIndex).FileName + Chr(34)
            If Image(SelectedImageIndex).Type <> FileTypes.Music Then Fail = True

            If Not Fail Then
                Dim r As Microsoft.Win32.RegistryKey
                r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".mp3")
                Dim Alias1 As String = r.GetValue("")
                r.Close()
                r = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Alias1)
                r = r.OpenSubKey("shell")

                'If Control.ModifierKeys <> Keys.Shift Then
                '    r = r.OpenSubKey("open")
                'Else
                r = r.OpenSubKey("Enqueue")
                'End If
                r = r.OpenSubKey("command")

                Dim ShellStr As String = r.GetValue("")
                r.Close()
                ShellStr = ShellStr.Replace(Chr(34) + "%1" + Chr(34), Str)
                Shell(ShellStr, AppWinStyle.NormalNoFocus)
            End If
        End If
    End Sub

    Public Sub SetCanvas(Optional Animation As Boolean = True)
        Dim DestPos As Point
        If Image(SelectedImageIndex).Animate Then
            DestPos = New Point(Image(SelectedImageIndex).DestX, Image(SelectedImageIndex).DestY)
        Else
            DestPos = New Point(Image(SelectedImageIndex).X, Image(SelectedImageIndex).Y)
        End If

        If Arrangement = ArrangmentTypes.Õorizontal Then
            If Canvas.Animate = False Then Canvas.DestY = Canvas.Y
            If Canvas.LinesInBox >= 3 Then
                If DestPos.Y < -Canvas.Y + (Wire.Y + Wire.dY) Then
                    Canvas.DestY = -DestPos.Y + Wire.dY + (Wire.Y + Wire.dY)
                End If
                If DestPos.Y > -Canvas.Y + Me.Height - (Wire.Y + Wire.dY) * 2 Then
                    Canvas.DestY = -DestPos.Y + Me.Height - (Wire.Y + Wire.dY) * 2
                End If
            Else
                If DestPos.Y < -Canvas.Y Then
                    Canvas.DestY = -DestPos.Y + Wire.dY
                End If
                If DestPos.Y > -Canvas.Y + Me.Height - (Wire.Y + Wire.dY) Then
                    Canvas.DestY = -DestPos.Y + Me.Height - (Wire.Y + Wire.dY)
                End If
            End If
            If Canvas.DestY > 0 Then Canvas.DestY = 0
            If Canvas.DestY < Canvas.MinY Then Canvas.DestY = Canvas.MinY
            Canvas.Animate = True
        Else
            If Canvas.Animate = False Then Canvas.DestX = Canvas.X
            If Canvas.LinesInBox >= 3 Then
                If DestPos.X < -Canvas.X + (Wire.X + Wire.dX) Then
                    Canvas.DestX = -DestPos.X + Wire.dX + (Wire.X + Wire.dX)
                End If
                If DestPos.X > -Canvas.X + Me.Width - (Wire.X + Wire.dX) * 2 Then
                    Canvas.DestX = -DestPos.X + Me.Width - (Wire.X + Wire.dX) * 2
                End If
            Else
                If DestPos.X < -Canvas.X Then
                    Canvas.DestX = -DestPos.X + Wire.dX
                End If
                If DestPos.X > -Canvas.X + Me.Width - (Wire.X + Wire.dX) Then
                    Canvas.DestX = -DestPos.X + Me.Width - (Wire.X + Wire.dX)
                End If
            End If
            If Canvas.DestX > 0 Then Canvas.DestX = 0
            If Canvas.DestX < Canvas.MinX Then Canvas.DestX = Canvas.MinX
            Canvas.Animate = True
        End If

        If Not Animation Then
            If Arrangement <> ArrangmentTypes.Õorizontal Then
                Canvas.X = Canvas.DestX
            Else
                Canvas.Y = Canvas.DestY
            End If
        End If
    End Sub
    Private Sub ucImagesBox_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        If e.KeyCode <> Keys.Tab Then e.IsInputKey() = True
    End Sub
#End Region
#Region "--------------------------|  Drawing  "
    Public BmpMain As Bitmap = New Bitmap(10, 10), GraphicsMain As Graphics = Graphics.FromImage(BmpMain)

    Dim SelInt As Double = 1
    Public DrawBorders As Boolean = True

    Public font_filename As New Font("Lucida Sans Unicode", 11, FontStyle.Regular, GraphicsUnit.Pixel)
    Public font_singer As New Font("Lucida Sans Unicode", 10, FontStyle.Regular, GraphicsUnit.Pixel)
    Public font_bold As New Font(font_singer, FontStyle.Bold)
    'Dim font_singer As New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Pixel)

    Dim TextColor As New SolidBrush(Color.Black)

    Dim SelRectColor As New SolidBrush(Color.FromArgb(255, 19, 130, 206)) 'SolidBrush(Color.FromArgb(110, 255, 255, 255))
    Dim SelRectFrameColor As New Pen(Color.FromArgb(0, 0, 0, 0)) 'Pen(Color.FromArgb(70, 0, 0, 0))

    Public BGColor As Color = Color.FromArgb(240, 240, 240)
    Public DrawShadow As Boolean = True
    Public DrawVolume As Boolean = False
    Public DrawFrame As Boolean = False
    Public RoundedElements As Boolean = True

    Private Sub Draw_FreeAreas()
        Dim a2 As New SolidBrush(Color.FromArgb(BGColor.R * 0.9, BGColor.G * 0.9, BGColor.B * 0.9))
        'Dim a2 As New SolidBrush(Color.FromArgb(BGColor.R, BGColor.G, BGColor.B))
        If Canvas.Y >= 1 Then
            Dim ii As Long = Canvas.Y
            GraphicsMain.FillRectangle(a2, 0, 0, BmpMain.Width, ii)

            If DrawBorders Then
                Dim c As New Pen(Color.FromArgb(100, 100, 100))
                GraphicsMain.DrawLine(c, 0, ii, BmpMain.Width, ii)
            End If
        End If
        If Canvas.Y < Canvas.MinY Then
            Dim ii As Long = Canvas.MinY - Canvas.Y
            GraphicsMain.FillRectangle(a2, 0, BmpMain.Height - ii, BmpMain.Width, ii)
            ii = -ii + BmpMain.Height

            If DrawBorders Then
                Dim c As New Pen(Color.FromArgb(100, 100, 100))
                GraphicsMain.DrawLine(c, 0, ii, BmpMain.Width, ii)
            End If
        End If

        If Canvas.X >= 1 Then
            Dim ii As Long = Canvas.X
            GraphicsMain.FillRectangle(a2, 0, 0, ii, BmpMain.Height)

            If DrawBorders Then
                Dim c As New Pen(Color.FromArgb(100, 100, 100))
                GraphicsMain.DrawLine(c, ii - 1, Math.Max(0, CInt(Canvas.Y)), ii - 1, Math.Min(CInt(Canvas.Y - Canvas.MinY), 0) + BmpMain.Height)
            End If
        End If
        If Canvas.X < Canvas.MinX Then
            Dim ii As Long = Canvas.MinX - Canvas.X
            GraphicsMain.FillRectangle(a2, BmpMain.Width - ii, 0, ii, BmpMain.Height)
            ii = -ii + BmpMain.Width

            If DrawBorders Then
                Dim c As New Pen(Color.FromArgb(100, 100, 100))
                GraphicsMain.DrawLine(c, ii, Math.Max(0, CInt(Canvas.Y)), ii, Math.Min(CInt(Canvas.Y - Canvas.MinY), 0) + BmpMain.Height)
            End If
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
                SelRectColor.Color = Color.FromArgb(255, 19, 130, 206) 'Color.FromArgb(110, 255, 255, 255)
                SelRectFrameColor.Color = Color.FromArgb(0, 0, 0, 0) 'Color.FromArgb(70, 0, 0, 0)
            End If

            GraphicsMain.Clear(BGColor)

            Draw_FreeAreas()
            'If Not IsSmthDragging Then DrawSelections()
            'DrawSelections()
            If IsSmthDragging And CanvasAutoScrolling = False And DragDropCandidateIndex = -1 And NDraggingFiles <= 1 And IStartedDragDrop Then
                Dim R As RectangleF
                If Arrangement = ArrangmentTypes.Vertical Then
                    If DividerIndex - 1 > DraggingFilesList(1) Then
                        R = New RectangleF(Math.Round(Image(DividerIndex - 1).X + Canvas.X - Wire.dX / 2), Math.Round(Image(DividerIndex - 1).Y + Wire.Y + Canvas.Y + 1), Image(DraggingFilesList(1)).WidthWithText + Wire.dX, Wire.dY - 2)
                    ElseIf DividerIndex < DraggingFilesList(1) Then
                        R = New RectangleF(Math.Round(Image(DividerIndex).X + Canvas.X - Wire.dX / 2), Math.Round(Image(DividerIndex).Y - Wire.dY + Canvas.Y + 1), Image(DraggingFilesList(1)).WidthWithText + Wire.dX, Wire.dY - 2)
                    End If
                    GraphicsMain.FillRectangle(New SolidBrush(Color.FromArgb(255, 19, 130, 206)), R)
                Else
                    If DividerIndex - 1 = DraggingFilesList(1) Or DividerIndex = DraggingFilesList(1) Then
                        'R = New RectangleF(Math.Round(Image(DividerIndex - 1).X + Canvas.X + 1 + Wire.X), _
                        '                   Math.Round(Image(DividerIndex - 1).Y + Canvas.Y - Wire.dY / 2), _
                        '                   Wire.dX - 2, Wire.Y + Wire.dY * 2)
                        Dim Sum As Long = Image(DraggingFilesList(1) - 1).X + Image(DraggingFilesList(1) + 1).X
                        Dim Dist As Long = Image(DraggingFilesList(1) + 1).X - Image(DraggingFilesList(1) - 1).X - (Wire.X + Wire.dX)
                        If Dist < 0 Or Dist > Wire.X + Wire.dX Then
                            If DividerIndex - 1 >= DraggingFilesList(1) Then
                                Sum = Image(DraggingFilesList(1) - 1).X + (Canvas.Plus + Canvas.Columns * (Wire.X + Wire.dX))
                                Dist = -Image(DraggingFilesList(1) - 1).X + (Canvas.Plus + Canvas.Columns * (Wire.X + Wire.dX)) - (Wire.X + Wire.dX)
                                R = New RectangleF(Math.Round(Sum / 2 + Canvas.X + 1 + (Wire.X - Wire.dX) / 2), _
                                           Math.Round(Image(DraggingFilesList(1) - 1).Y + Canvas.Y - Wire.dY / 2), _
                                           Wire.dX - 2, Wire.Y + Wire.dY)
                            ElseIf DividerIndex <= DraggingFilesList(1) Then
                                Sum = Image(DraggingFilesList(1) + 1).X + Canvas.Plus - (Wire.X + Wire.dX)
                                Dist = Image(DraggingFilesList(1) + 1).X - (Canvas.Plus - (Wire.X + Wire.dX)) - (Wire.X + Wire.dX)
                                R = New RectangleF(Math.Round(Sum / 2 + Canvas.X + 1 + (Wire.X - Wire.dX) / 2), _
                                           Math.Round(Image(DraggingFilesList(1) + 1).Y + Canvas.Y - Wire.dY / 2), _
                                           Wire.dX - 2, Wire.Y + Wire.dY)
                            End If
                            If Dist < 0 Then Dist = 0
                            If Dist > Wire.X + Wire.dX Then Dist = Wire.X + Wire.dX
                        Else
                            R = New RectangleF(Math.Round(Sum / 2 + Canvas.X + 1 + (Wire.X - Wire.dX) / 2), _
                                       Math.Round(Image(DraggingFilesList(1) - 1).Y + Canvas.Y - Wire.dY / 2), _
                                       Wire.dX - 2, Wire.Y + Wire.dY)
                        End If
                        GraphicsMain.FillRectangle(New SolidBrush(Color.FromArgb((1 - Dist / (Wire.X + Wire.dX)) * 255, 19, 130, 206)), R)
                    Else
                        'R = New RectangleF(Math.Round(Image(DividerIndex).X + Canvas.X + 1 - Wire.dX), Math.Round(Image(DividerIndex).Y + Canvas.Y - Wire.dY / 2), Wire.dX - 2, Wire.Y + Wire.dY * 2)
                        Dim Sum As Long = Image(DividerIndex).X + Image(DividerIndex - 1).X
                        Dim Dist As Long = Image(DividerIndex).X - Image(DividerIndex - 1).X - (Wire.X + Wire.dX)
                        If Dist < 0 Or Dist > Wire.X + Wire.dX Then
                            Dist = 0
                            If Dist > Wire.X + Wire.dX Then Dist = Wire.X + Wire.dX
                            If DividerIndex - 1 >= DraggingFilesList(1) Then
                                R = New RectangleF(Math.Round(Image(DividerIndex - 1).X + Canvas.X + 1 + Wire.X), _
                                           Math.Round(Image(DividerIndex - 1).Y + Canvas.Y - Wire.dY / 2), _
                                           Wire.dX - 2, Wire.Y + Wire.dY)
                            ElseIf DividerIndex <= DraggingFilesList(1) Then
                                R = New RectangleF(Math.Round(Image(DividerIndex).X + Canvas.X + 1 - Wire.dX), _
                                           Math.Round(Image(DividerIndex).Y + Canvas.Y - Wire.dY / 2), _
                                           Wire.dX - 2, Wire.Y + Wire.dY)
                            End If
                        Else
                            R = New RectangleF(Math.Round(Sum / 2 + Canvas.X + 1 + (Wire.X - Wire.dX) / 2), _
                                               Math.Round(Image(DividerIndex).Y + Canvas.Y - Wire.dY / 2), _
                                                Wire.dX - 2, Wire.Y + Wire.dY)
                        End If
                        GraphicsMain.FillRectangle(New SolidBrush(Color.FromArgb(255, 19, 130, 206)), R)
                    End If
                End If
            End If
                GraphicsMain.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                Dim I As Long = SelectedImageIndex
                Dim x, y As Long
                Dim II As Long
                For II = NImages To 1 Step -1
                    If II <> SelectedImageIndex And Image(II).Y + Canvas.Y > -Wire.Y And Image(II).Y + Canvas.Y < Me.Height And Image(II).X + Canvas.X > -Wire.X And Image(II).X + Canvas.X < Me.Width Then
                        If (NeedToDrawSelection(II) = False) Then
                            If (DragDropCandidateIndex = II) Then DrawSelection(II)
                            If Image(II).Loaded Then
                                DrawPicture(II)
                            Else
                                DrawFakePicture(II)
                            End If
                            If Wire.Y > 40 And Image(II).Type = FileTypes.Image Then
                                If ShowImagesName Or Not Image(II).Loaded Then
                                    DrawUnderImageText(II)
                                End If
                            Else
                                If Image(II).Type = FileTypes.Music Or Image(II).Type = FileTypes.Drive Then
                                    DrawSongText(II)
                                Else
                                    DrawSimpleText(II)
                                End If
                            End If
                        End If
                    End If
                Next
                'For II = NImages To 1 Step -1
                '    If II <> SelectedImageIndex And Image(II).Y + Canvas.Y > -Wire.Y And Image(II).Y + Canvas.Y < Me.Height And Image(II).X + Canvas.X > -Wire.X And Image(II).X + Canvas.X < Me.Width Then
                '        If (NeedToDrawSelection(II) = True) Then DrawSelection(II)
                '    End If
                'Next
                For II = NImages To 1 Step -1
                    If II <> SelectedImageIndex And Image(II).Y + Canvas.Y > -Wire.Y And Image(II).Y + Canvas.Y < Me.Height And Image(II).X + Canvas.X > -Wire.X And Image(II).X + Canvas.X < Me.Width Then
                        If (NeedToDrawSelection(II) = True) Then
                            DrawSelection(II)
                            If Image(II).Loaded Then
                                DrawPicture(II)
                            Else
                                DrawFakePicture(II)
                            End If
                            If Wire.Y > 40 And Image(II).Type = FileTypes.Image Then
                                If ShowImagesName Or Not Image(II).Loaded Then
                                    DrawUnderImageText(II)
                                End If
                            Else
                                If Image(II).Type = FileTypes.Music Or Image(II).Type = FileTypes.Drive Then
                                    DrawSongText(II)
                                Else
                                    DrawSimpleText(II)
                                End If
                            End If
                        End If
                    End If
                Next

                If I > 0 And I <= NImages Then
                    Dim PenBorder As Pen
                    Dim sm As Long = Math.Round(Wire.min_dY / 2 + 0.3) - 1
                    x = Image(I).X + Canvas.X
                    y = Image(I).Y + Canvas.Y
                    SpoilCoordinates(x, y)
                    If Me.Focused Then
                        'If (IsMouseDown And SelectedImageIndex = ChosenObj) Or IsEnterDown Then
                        'If SelInt < 0 Then
                        '    Dim K As Double = Math.Abs((1 - 0.2 * (Math.Sin(-SelInt * Math.PI))) * (1 - 0.3 * -SelInt))
                        '    SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 255, 19 * K, 130 * K, 206 * K)
                        'Else
                        SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 255, 19 * (1 - 0.3 * SelInt), 130 * (1 - 0.3 * SelInt), 206 * (1 - 0.25 * SelInt))
                        'End If
                        'End If
                        TextColor.Color = Color.FromArgb(Image(I).Transparency * 255, 255, 255, 255)
                        If Image(I).Selected Or Image(I).PreSelected = True Then
                            'SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 255, 19 * (1 - 0.3 * SelInt) * 1.2, 130 * (1 - 0.3 * SelInt) * 1.2, 206 * (1 - 0.25 * SelInt) * 1.2)
                            SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 255, 19 * (1 - 0.3 * SelInt) * 0.8, 130 * (1 - 0.3 * SelInt) * 0.8, 206 * (1 - 0.25 * SelInt) * 0.8)
                        End If
                    Else
                        If BGColor.R > 127 Then
                            SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 255, BGColor.R - 50, BGColor.G - 50, BGColor.B - 50)
                        Else
                            SelRectColor.Color = Color.FromArgb(Image(I).Transparency * 255, BGColor.R + 50, BGColor.G + 50, BGColor.B + 50)
                        End If
                    End If
                    If RoundedElements Then sm -= 1
                    Dim SelectionRect As New Rectangle(x - sm, y - sm, Image(I).WidthWithText + sm * 2, Wire.Y + sm * 2)
                    'If Image(I).Type = FileTypes.Image Then
                    '    SelectionRect = New Rectangle(x - sm, y - sm, Image(I).WidthWithText + sm * 2, Image(I).Height + sm * 2)
                    'End If

                    If DrawContrastFrames Then
                        Dim ContrastingColor As Color = Color.White
                        If BGColor.R < 127 Then ContrastingColor = Color.Black
                        GraphicsMain.DrawRectangle(New Pen(Color.FromArgb(Image(I).Transparency * 140, ContrastingColor), 1), x - sm - 1, y - sm - 1, Image(I).WidthWithText + sm * 2 - 1 + 2, Wire.Y + sm * 2 - 1 + 2)
                        DrawRoundedRect(GraphicsMain, New Pen(Color.FromArgb(Image(I).Transparency * 40, ContrastingColor), 1), x - sm - 2, y - sm - 2, Image(I).WidthWithText + sm * 2 - 1 + 4, Wire.Y + sm * 2 - 1 + 4)
                    End If

                    GraphicsMain.FillRectangle(SelRectColor, SelectionRect)
                    If RoundedElements Then
                        sm += 1
                        If DrawFrame Then PenBorder = New Pen(Color.FromArgb(Image(I).Transparency * 255, SelRectColor.Color.R * 0.7, SelRectColor.Color.G * 0.7, SelRectColor.Color.B * 0.7)) Else PenBorder = New Pen(SelRectColor)
                        DrawRoundedRect(GraphicsMain, PenBorder, x - sm, y - sm, Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)
                    End If
                    If DrawVolume Then
                        GraphicsMain.DrawLine(New Pen(Color.FromArgb(Image(I).Transparency * 100, Color.White)), x - sm + 1, y - sm + 1, x - sm + Image(I).WidthWithText + sm * 2 - 2, y - sm + 1)
                        GraphicsMain.DrawLine(New Pen(Color.FromArgb(Image(I).Transparency * 70, Color.White)), x - sm + 1, y - sm + 2, x - sm + 1, Wire.Y + sm * 2 + y - sm - 2)
                        GraphicsMain.DrawLine(New Pen(Color.FromArgb(Image(I).Transparency * 70, Color.White)), x - sm + Image(I).WidthWithText + sm * 2 - 2, y - sm + 2, x - sm + Image(I).WidthWithText + sm * 2 - 2, Wire.Y + sm * 2 + y - sm - 2)
                    End If
                    'sm -= 1

                    II = SelectedImageIndex
                    If Image(II).Loaded Then
                        DrawPicture(II)
                        TextColor.Color = Color.FromArgb(Image(II).Transparency * 255, TextColor.Color)
                        If Wire.Y > 40 And Image(II).Type = FileTypes.Image Then
                            If ShowImagesName Then DrawUnderImageText(II)
                        Else
                            If Image(II).Type = FileTypes.Music Or Image(II).Type = FileTypes.Drive Then
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
                            'Dim b4 As New SolidBrush(Color.FromArgb(140, 0, 0, 0))
                            'GraphicsMain.DrawString("(no thumb)", font_filename, b4, x + GraphicsMain.MeasureString(Image(II).Name, font_filename).Width, y)
                        End If
                    End If
                End If
                TextColor.Color = Color.FromArgb(0, 0, 0)
            End If
    End Sub

    Private Sub CleanScreen()
        GraphicsMain.Clear(BGColor)
    End Sub
    Private Sub DrawSongText(ByVal ii As Integer)
        Dim TextBrush As New SolidBrush(Color.FromArgb(Image(ii).Transparency * 255, TextColor.Color))
        If NeedToDrawSelection(ii) And Me.Focused And Image(SelectedImageIndex).Selected Then TextBrush.Color = Color.FromArgb(255, 255, 255)

        Dim x As Long = Image(ii).X + Canvas.X
        Dim y As Long = Image(ii).Y + Canvas.Y - 1
        SpoilCoordinates(x, y)
        If Wire.Y < 26 Then '32 Then
            x += Image(ii).Width + 5 ' 50 + 5 
            y += Wire.Y / 2 - 8
            If Not Image(ii).Loaded Then x -= 55
            If Image(ii).Singer <> "noname" And Image(ii).Singer <> "" Then
                GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextBrush, x, y + 2)
                GraphicsMain.DrawString("- " + Image(ii).Name, font_filename, TextBrush, x + GraphicsMain.MeasureString(Image(ii).Singer, font_singer).Width, y + 2)
            Else
                GraphicsMain.DrawString(Image(ii).Name, font_filename, TextBrush, x, y + 2)
            End If
        ElseIf Wire.Y = 32 Then
            x += Image(ii).Width + 3 'Thumbnail(ii).Width + 5
            'GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + 4)
            'GraphicsMain.DrawString(Image(ii).Name, font_filename, TextColor, x, y + 16)

            GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextBrush, x, y + 16)
            GraphicsMain.DrawString(Image(ii).Name, font_bold, TextBrush, x, y + 4)
        ElseIf Wire.Y < 80 Then
            x += Image(ii).Width + 3 'Thumbnail(ii).Width + 5
            'GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextColor, x, y + Wire.Y / 2 + 4 - 16)
            'GraphicsMain.DrawString(Image(ii).Name, font_bold, TextColor, x, y + Wire.Y / 2)
            GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextBrush, x, y + Wire.Y / 2)
            GraphicsMain.DrawString(Image(ii).Name, font_bold, TextBrush, x, y + Wire.Y / 2 - font_bold.Height * 0.8)
        Else
            Dim fnt As New Font(font_filename, FontStyle.Bold)
            GraphicsMain.DrawString(Image(ii).Singer, font_singer, TextBrush, _
            x + (Wire.X - GraphicsMain.MeasureString(Image(ii).Singer, font_singer).Width) / 2, y + Wire.Y / 2 + 2 + font_bold.Height * 0.8)
            GraphicsMain.DrawString(Image(ii).Name, fnt, TextBrush, _
            x + (Wire.X - GraphicsMain.MeasureString(Image(ii).Name, fnt).Width) / 2, y + Wire.Y / 2 + 2)
        End If
    End Sub
    Private Sub DrawSimpleText(ByVal ii As Integer)
        Dim x As Long
        'If Image(ii).Loaded Then
        x = Math.Max(Image(ii).X + Image(ii).Width + 4 + Canvas.X, Image(ii).X + 16 + 4 + Canvas.X)
        If Wire.Y <= 40 Then
            If Image(ii).Type = FileTypes.Image Or Image(ii).Type = FileTypes.Music Then
                x = Image(ii).X + Wire.Y + 2 + Canvas.X
            Else
                x = Image(ii).X + Image(ii).Width + 2 + Canvas.X
            End If
        End If
        'Else
        'x = Image(ii).X + Canvas.X
        'End If
        Dim y As Long = Image(ii).Y + Canvas.Y + Math.Floor((Wire.Y - font_filename.Size) / 2) - 1 '- font_filename.Size * 0.2

        SpoilCoordinates(x, y)
        'GraphicsMain.FillRectangle(Brushes.White, x, y, 100, font_filename.Size)

        Dim TextBrush As New SolidBrush(Color.FromArgb(Image(ii).Transparency * 255, TextColor.Color))
        If (Me.Focused And NeedToDrawSelection(ii) And Image(SelectedImageIndex).Selected) Or DragDropCandidateIndex = ii Then TextBrush.Color = Color.FromArgb(Image(ii).Transparency * 255, 255, 255, 255)
        GraphicsMain.DrawString(Image(ii).Name, font_filename, TextBrush, x, y)
    End Sub
    Private Sub DrawUnderImageText(ByVal ii As Integer)
        Dim TextBrush As New SolidBrush(Color.FromArgb(Image(ii).Transparency * 255, TextColor.Color))
        If NeedToDrawSelection(ii) And Me.Focused And Image(SelectedImageIndex).Selected Then TextBrush.Color = Color.FromArgb(Image(ii).Transparency * 255, 255, 255, 255)

        Dim x As Long = Image(ii).X + (Wire.X - GraphicsMain.MeasureString(Trim(Image(ii).Name), font_filename).Width) / 2 + Canvas.X
        Dim y As Long = Image(ii).Y + Wire.Y / 2 + Image(ii).Height / 2 + Canvas.Y - 3
        SpoilCoordinates(x, y)
        GraphicsMain.DrawString(Image(ii).Name, font_filename, TextBrush, x, y)
        If Not Image(ii).Loaded Then
            GraphicsMain.DrawString("loading...", font_filename, Brushes.Gray, x, y - font_filename.Size - 5)
        End If
    End Sub
    Private Function NeedToDrawSelection(I As Integer) As Boolean
        If Image(I).PreSelected = True Then
            If Control.ModifierKeys = Keys.Alt Then Return False Else Return True
        Else
            Return Image(I).Selected
        End If
    End Function
    Private Sub DrawRoundedRect(G As Graphics, P As Pen, x As Long, y As Long, w As Long, h As Long)
        G.DrawLine(P, x + 1, y, x + w - 1, y)
        G.DrawLine(P, x, y + 1, x, y + h - 1)
        G.DrawLine(P, x + 1, y + h, x + w - 1, y + h)
        G.DrawLine(P, x + w, y + 1, x + w, y + h - 1)
    End Sub
    Private Sub DrawPixeltRect(B As Bitmap, C As Color, x As Long, y As Long, w As Long, h As Long)
        Try
            B.SetPixel(x, y, C)
            B.SetPixel(x, y + h, C)
            B.SetPixel(x + w, y, C)
            B.SetPixel(x + w, y + h, C)
        Catch
        End Try
    End Sub

    Private Sub DrawSelections()
        Dim x, y As Long
        Dim sm As Long = Math.Round(Wire.min_dY / 2 + 0.3) - 1
        'Dim p1 As New Pen(Color.FromArgb(20, 100, 120, 205))
        Dim b1 As SolidBrush = Brushes.GreenYellow
        Dim penBorder As Pen

        For i As Long = 1 To NImages
            If NeedToDrawSelection(i) And IsImageVisible(i) Then
                sm -= 1
                x = Image(i).X + Canvas.X
                y = Image(i).Y + Canvas.Y
                SpoilCoordinates(x, y)
                If Image(SelectedImageIndex).Selected Then
                    If Me.Focused Then
                        'If SelInt < 0 Then SelInt = -SelInt
                        b1.Color = Color.FromArgb(255 * Image(i).Transparency, 67 * (1 - 0.3 * SelInt), 168 * (1 - 0.3 * SelInt), 228 * (1 - 0.3 * SelInt))
                        'b1.Color = Color.FromArgb(255 * Image(i).Transparency, 19, 130, 206)
                    Else
                        b1.Color = Color.FromArgb(Image(i).Transparency * 255, 225, 225, 225)
                    End If
                Else
                    If Me.Focused Then
                        b1.Color = Color.FromArgb(255 * Image(i).Transparency, 170, 170, 170) '112
                    Else
                        b1.Color = Color.FromArgb(255 * Image(i).Transparency, 225, 225, 225)
                    End If

                    'b1.Color = Color.FromArgb(255 * Image(i).Transparency, 255, 255, 255) '150, 170, 255)
                    'b1.Color = Color.FromArgb(255 * Image(i).Transparency / 6, 19, 130, 206)
                End If
                GraphicsMain.FillRectangle(b1, x - sm, y - sm, Image(i).WidthWithText + sm * 2, Wire.Y + sm * 2)
                If DrawFrame Then
                    penBorder = New Pen(Color.FromArgb(100, 19, 130, 206))
                Else
                    penBorder = New Pen(b1.Color)
                End If

                sm += 1
                GraphicsMain.DrawLine(penBorder, x - sm, y - sm + 1, x - sm, Wire.Y + sm * 2 + y - sm - 2)
                GraphicsMain.DrawLine(penBorder, x - sm + Image(i).WidthWithText + sm * 2 - 1, y - sm + 1, x - sm + Image(i).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 + y - sm - 2)
                GraphicsMain.DrawLine(penBorder, x - sm + 1, y - sm, x - sm + Image(i).WidthWithText + sm * 2 - 2, y - sm)
                GraphicsMain.DrawLine(penBorder, x - sm + 1, Wire.Y + sm * 2 + y - sm - 1, x - sm + Image(i).WidthWithText + sm * 2 - 2, Wire.Y + sm * 2 + y - sm - 1)

                If DrawContrastFrames Then
                    GraphicsMain.DrawRectangle(New Pen(Color.FromArgb(16, Color.Black), 1), x - sm - 1, y - sm - 1, Image(i).WidthWithText + sm * 2 - 1 + 2, Wire.Y + sm * 2 - 1 + 2)
                    GraphicsMain.DrawRectangle(New Pen(Color.FromArgb(4, Color.Black), 1), x - sm - 2, y - sm - 2, Image(i).WidthWithText + sm * 2 - 1 + 4, Wire.Y + sm * 2 - 1 + 4)
                End If
                'sm -= 1
            End If
        Next
        'TextColor.Color = Color.FromArgb(0, 0, 0)
    End Sub
    Private Sub DrawSelection(I As Long)
        Dim brushBG As SolidBrush
        Dim penBorder As Pen

        If Image(SelectedImageIndex).Selected Then
            If Me.Focused Then
                brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency, 67 * (1 - 0.3 * SelInt), 168 * (1 - 0.3 * SelInt), 228 * (1 - 0.3 * SelInt)))
            Else
                If BGColor.R < 127 Then
                    brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency / 8, Color.White)) '112
                Else
                    brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency / 8, Color.Black)) '112
                End If
                'brushBG = New SolidBrush(Color.FromArgb(Image(I).Transparency * 255, 225, 225, 225))
            End If
        Else
            'If Me.Focused Then
            If BGColor.R < 127 Then
                brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency / 10, Color.White)) '112
            Else
                brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency / 10, Color.Black)) '112
            End If
            'Else
            'brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency, 225, 225, 225))
            'End If
        End If
        If (I = DragDropCandidateIndex And ChosenObj > 0) Then
            Dim k As Double = (1 - 2 * (Image(ChosenObj).Transparency - 0.5))
            k = Math.Max(0, Math.Min(1, k))
            brushBG = New SolidBrush(Color.FromArgb(255 * Image(I).Transparency * k, 45 * (1 - 0.3 * SelInt), 206 * (1 - 0.3 * SelInt), 170 * (1 - 0.3 * SelInt)))
        End If


        Dim x As Long = Image(I).X + Canvas.X
        Dim y As Long = Image(I).Y + Canvas.Y

        Dim sm As Long = Math.Round(Wire.min_dY / 2 + 0.3) - 1 : If RoundedElements Then sm -= 1

        GraphicsMain.FillRectangle(brushBG, x - sm, y - sm, Image(I).WidthWithText + sm * 2, Wire.Y + sm * 2)

        DrawContrastFrames = True
        If DrawContrastFrames And Me.Focused And Image(SelectedImageIndex).Selected Then
            Dim ContrastingColor As Color = Color.White
            If BGColor.R < 127 Then ContrastingColor = Color.Black
            If RoundedElements Then
                sm += 1
                GraphicsMain.DrawRectangle(New Pen(Color.FromArgb(100 * Image(I).Transparency, ContrastingColor), 1), x - sm, y - sm, Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)
                penBorder = New Pen(Color.FromArgb(50 * Image(I).Transparency, ContrastingColor))
                sm += 1
                DrawRoundedRect(GraphicsMain, penBorder, x - sm, y - sm, Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)
                'DrawPixeltRect(BmpMain, Color.Red, x - sm, y - sm, Image(I).WidthWithText + sm * 2, Wire.Y + sm * 2)
                sm -= 2
            Else
                sm += 1
                GraphicsMain.DrawRectangle(New Pen(Color.FromArgb(60 * Image(I).Transparency, ContrastingColor), 1), x - sm, y - sm, Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)
                penBorder = New Pen(Color.FromArgb(25 * Image(I).Transparency, ContrastingColor))
                sm += 1
                DrawRoundedRect(GraphicsMain, penBorder, x - sm, y - sm, Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 - 1)
                sm -= 2
            End If
        End If

        If RoundedElements Or DrawFrame Then
            sm += 1
            If DrawFrame Then
                penBorder = New Pen(Color.FromArgb(100 * Image(I).Transparency, 19, 130, 206))
            Else
                penBorder = New Pen(brushBG.Color)
            End If

            GraphicsMain.DrawLine(penBorder, x - sm, y - sm + 1, x - sm, Wire.Y + sm * 2 + y - sm - 2)
            GraphicsMain.DrawLine(penBorder, x - sm + Image(I).WidthWithText + sm * 2 - 1, y - sm + 1, x - sm + Image(I).WidthWithText + sm * 2 - 1, Wire.Y + sm * 2 + y - sm - 2)
            GraphicsMain.DrawLine(penBorder, x - sm + 1, y - sm, x - sm + Image(I).WidthWithText + sm * 2 - 2, y - sm)
            GraphicsMain.DrawLine(penBorder, x - sm + 1, Wire.Y + sm * 2 + y - sm - 1, x - sm + Image(I).WidthWithText + sm * 2 - 2, Wire.Y + sm * 2 + y - sm - 1)
        End If
    End Sub
    Private Sub DrawPicture(ByVal I As Integer)
        Dim p As Point

        p.Y = Math.Round(Image(I).Y + Canvas.Y)
        p.X = Math.Round(Image(I).X + Canvas.X)
        SpoilCoordinates(p.X, p.Y)
        'GraphicsMain.FillRectangle(Brushes.White, p.X, p.Y, Wire.X, Wire.Y)

        If Image(I).Type = FileTypes.Music Then
            p.Y += Math.Round((Wire.Y - Image(I).Height) / 2)
            If Wire.Y >= 80 Then
                p.Y -= Image(I).DestHeight - 18
                p.X += Wire.X / 2 - Image(I).DestWidth / 2
            End If
        ElseIf Image(I).Type = FileTypes.Image Then
            Dim w As Long = Wire.Border

            If Wire.Y <= 40 Then
                p.X += Math.Round((Wire.Y - Image(I).Width - w))
                p.Y += Math.Round((Wire.Y - Image(I).Height) / 2)
            Else
                p.X += Math.Round((Wire.X - Image(I).Width) / 2)
                p.Y += Math.Round((Wire.Y - Image(I).Height) / 2)
                If ShowImagesName Then
                    p.Y = Math.Round((Wire.Y - Image(I).Height - font_filename.SizeInPoints + 2 - w) / 2) + Math.Round(Image(I).Y + Canvas.Y)
                End If
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
        ElseIf Wire.Y <= 40 Then
            p.Y += Math.Round((Wire.Y - Image(I).Height) / 2)
        Else
            p.Y += Math.Round((Wire.Y - Image(I).Height) / 2)
            'p.X += Math.Round((Wire.X - Image(I).Width) / 2)
        End If

        'Try
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
                If Image(I).Type = FileTypes.Music Then
                    If Image(I).FileName = InPlayerFileName Then
                        Dim r As New Rectangle(p.X, p.Y, .Width, .Height)
                        If PlayerState = PlayerStates.Pause Then GraphicsMain.FillRectangle(Brushes.White, r)
                        If PlayerState = PlayerStates.Playing Then GraphicsMain.FillRectangle(Brushes.Green, r)
                        If PlayerState = PlayerStates.Stopped Then GraphicsMain.FillRectangle(Brushes.Red, r)
                    End If
                End If

                If Not Thumbnail(I) Is Nothing Then
                    If .Width = Thumbnail(I).Width And .Height = Thumbnail(I).Height Then
                        GraphicsMain.DrawImageUnscaled(Thumbnail(I), p)
                    Else
                        Dim r As New Rectangle(p.X, p.Y, .Width, .Height)
                        GraphicsMain.DrawImage(Thumbnail(I), r)
                    End If
                End If
            End With
        End If
        'Catch
        'End Try
    End Sub
    Private Sub DrawFakePicture(ByVal I As Integer)
        Dim p As Point
        Dim w As Long = Wire.Border

        If Wire.Y <= 40 Or FileTags.Tags(Image(I).InTagsIngex).Type <> "image" Then
            p.Y = Math.Round(Image(I).Y + Canvas.Y) + Math.Round((Wire.Y - Image(I).Height) / 2)
            p.X = Math.Round(Image(I).X + Canvas.X)
        Else
            p.Y = Math.Round((Wire.Y - Image(I).Height) / 2) + Math.Round(Image(I).Y + Canvas.Y)
            p.X = Math.Round((Wire.X - Image(I).Width) / 2) + Math.Round(Image(I).X + Canvas.X)
            If ShowImagesName Then
                p.Y = Math.Round((Wire.Y - Image(I).Height - font_filename.SizeInPoints + 2 - w) / 2) + Math.Round(Image(I).Y + Canvas.Y)
            End If
        End If

        If Image(I).Type = FileTypes.Music And Wire.Y >= 80 Then
            p.Y -= Image(I).DestHeight - 18
            p.X += Wire.X / 2 - Image(I).DestWidth / 2
        End If

        If Image(I).Type = FileTypes.Image Then
            If Wire.Y <= 40 Then
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
            GraphicsMain.DrawImage(icoImg16, r1, 0, 0, icoImg16.Width, icoImg16.Height, GraphicsUnit.Pixel, att)
        Else
            With Image(I)
                If .Width = icoImg16.Width And .Height = icoImg16.Height Then
                    GraphicsMain.DrawImageUnscaled(icoImg16, p)
                Else
                    Dim r As New Rectangle(p.X, p.Y, .Width, .Height)
                    GraphicsMain.DrawImage(icoImg16, r)
                End If
            End With
        End If
    End Sub
    Private Sub SpoilCoordinates(ByRef x As Double, ByRef y As Double)
        'If Canvas.Y > 1 And Arrangement <> ArrangmentTypes.Õorizontal Then
        '    x = (x - Me.Width / 2 + Wire.X / 2) * (Math.Pow(Canvas.Y / Me.Height, 2) * Math.Pow((y - Canvas.Y) / Me.Height, 2) + 1) + Me.Width / 2 - Wire.X / 2
        '    y = (y - Canvas.Y) * (Canvas.Y / Me.Height + 1) + Canvas.Y
        'End If
        'If Canvas.Y < -1 And Arrangement <> ArrangmentTypes.Õorizontal Then
        '    x = (x - Me.Width / 2 + Wire.X / 2) / (Math.Pow(-Canvas.Y / Me.Height, 2) * Math.Pow(1 - (y - Canvas.Y) / Me.Height, 0.5) + 1) + Me.Width / 2 - Wire.X / 2
        '    y = (y - Canvas.Y - Canvas.Height) * (-Canvas.Y / Me.Height + 1) + Canvas.Y + Canvas.Height
        'End If
        'If Canvas.X > 1 And Arrangement = ArrangmentTypes.Õorizontal Then
        '    y = (y - Me.Height / 2 + Wire.Y / 2) * (Math.Pow(Canvas.X / Me.Width, 2) * Math.Pow((x - Canvas.X) / Me.Width, 2) + 1) + Me.Height / 2 - Wire.Y / 2
        '    x = (x - Canvas.X) * (Canvas.X / Me.Width + 1) + Canvas.X
        'End If
        'If Canvas.X < -1 And Arrangement = ArrangmentTypes.Õorizontal Then
        '    y = (y - Me.Height / 2 + Wire.Y / 2) / (Math.Pow(-Canvas.X / Me.Width, 2) * Math.Pow(1 - (x - Canvas.X) / Me.Width, 0.5) + 1) + Me.Height / 2 - Wire.Y / 2
        '    x = (x - Canvas.X - Canvas.Width) * (-Canvas.X / Me.Width + 1) + Canvas.X + Canvas.Width
        'End If
    End Sub
    Private Sub DrawFPS()
        counter += 1
        If counter >= 5 Then
            counter = 0
            time2 = System.DateTimeOffset.Now
            time_delta = time2 - time1
            time1 = time2
        End If
        GraphicsMain.DrawString(counter.ToString + " " + (Math.Round(1 / time_delta.TotalSeconds * 5000) / 1000).ToString + " fps", font_filename, Brushes.White, BmpMain.Width - 100, BmpMain.Height - 15)
    End Sub
    Private Sub DrawSelectionArea()
        Dim b As New SolidBrush(Color.FromArgb(125, Color.White))
        GraphicsMain.DrawRectangle(Pens.Gray, Math.Min(FirstX, PrevCursorPos.X), Math.Min(FirstY, PrevCursorPos.Y), Math.Abs(PrevCursorPos.X - FirstX), Math.Abs(PrevCursorPos.Y - FirstY))
        GraphicsMain.FillRectangle(b, Math.Min(FirstX, PrevCursorPos.X) + 1, Math.Min(FirstY, PrevCursorPos.Y) + 1, Math.Abs(PrevCursorPos.X - FirstX) - 1, Math.Abs(PrevCursorPos.Y - FirstY) - 1)
    End Sub
    Private Sub UpdateAndDrawButtons()
        If Canvas.MinY <> 0 Then
            Try
                Buttons(2).Y = (Canvas.Y / Canvas.MinY) * (BmpMain.Height - Buttons(2).Image.Height - 4 - Buttons(3).Image.Height * 2 - 2) + 3 + Buttons(3).Image.Height
                Buttons(2).X = BmpMain.Width - Buttons(2).Image.Width - 0 '1 '2
                Buttons(3).X = Buttons(2).X : Buttons(4).X = Buttons(2).X
                Buttons(3).Y = 0 '1 '2 
                Buttons(4).Y = BmpMain.Height - Buttons(4).Image.Height - 0 '1 '2

                Buttons(2).Visible = True : Buttons(3).Visible = True : Buttons(4).Visible = True
                For I As Long = 0 To Buttons.Length - 1
                    With Buttons(I)
                        If .Visible = True Then
                            GraphicsMain.DrawImageUnscaled(.Image, .X, .Y)
                        End If
                    End With
                Next
            Catch
            End Try
        Else
            Buttons(2).Visible = False : Buttons(3).Visible = False : Buttons(4).Visible = False
        End If
        If Canvas.MinX <> 0 Then
            Dim ScrollerSize As Size = New Size(100, 3)
            Dim ScrollerMargin As Short = 2 '1'0
            Dim ScrollerSidesMargin As Short = 2 '1'0
            Dim x As Long = (Canvas.X / Canvas.MinX) * (BmpMain.Width - ScrollerSize.Width - ScrollerSidesMargin * 2) + ScrollerSidesMargin
            Dim y As Long = BmpMain.Height - ScrollerSize.Height - ScrollerMargin

            GraphicsMain.FillRectangle(Brushes.Gray, x, y, ScrollerSize.Width, ScrollerSize.Height)
        End If
    End Sub
    Private Sub DrawDraggingInfo()
        If IsSmthDragging And NDraggingFiles > 1 Then
            Dim DraggingCountFont As Font = New Font("Verdana", 20, FontStyle.Regular)
            GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            Dim b As New SolidBrush(Color.FromArgb(19, 130, 206))
            Dim w As Long = GraphicsMain.MeasureString(NDraggingFiles.ToString, DraggingCountFont).Width - 1
            If w < 10 Then w = 10
            Dim x = MousePosition.X - Me.Left - Me.Parent.Left
            Dim y = MousePosition.Y - Me.Top - Me.Parent.Top - DraggingCountFont.Height - 50
            'GraphicsMain.FillRectangle(b, x - 17, y, 10 + w, DraggingCountFont.Height)
            'GraphicsMain.DrawRectangle(New Pen(Brushes.White, 1), x - 17, y, 10 + w, DraggingCountFont.Height)
            'GraphicsMain.DrawString(NDraggingFiles.ToString, DraggingCountFont, Brushes.White, x - 12, y)
            GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.None

            If NDraggingFiles > 1 Then 'Image(ChosenObj).Selected = True Then
                If Wire.Y > 48 And NDraggingFiles > 3 Then
                    Dim i As Long
                    SelectedAngleOffset -= 0.002
                    Dim dyr As Double
                    Dim dxr As Double
                    Dim dx As Long
                    Dim dy As Long
                    Dim r As Double
                    For nn As Long = 1 To NDraggingFiles
                        i = DraggingFilesList(nn)
                        If i <> ChosenObj Then
                            Dim n As Long = nn
                            If i > ChosenObj Then n -= 1
                            Dim k As Double = 1 '(n Mod 6 + 1) / 6 '(Math.Sin(-SelectedAngleOffset + 247 * Math.PI * n / NDraggingFiles)) 'Math.Abs(Math.Sin(-SelectedAngleOffset + 3 * Math.PI * n / NDraggingFiles))
                            Dim maxR As Short = 5
                            If Me.Height < 300 Then maxR = 3
                            If PrevCursorPosInTimer.X <> MousePosition.X Or PrevCursorPosInTimer.Y <> MousePosition.Y Then
                                dyr = 20 * Math.Sin((2 * Math.PI * n) / (NDraggingFiles - 1) + SelectedAngleOffset)
                                dxr = 20 * Math.Cos((2 * Math.PI * n) / (NDraggingFiles - 1) + SelectedAngleOffset)
                                dx = (dxr - (PrevCursorPosInTimer.X - MousePosition.X))
                                dy = (dyr - (PrevCursorPosInTimer.Y - MousePosition.Y))
                                r = Math.Max(0, maxR * k * (50 - Math.Sqrt(dx * dx + dy * dy))) '^ 2) / 200
                                'r = r - r / ((PrevCursorPosInTimer.X - MousePosition.X) ^ 2 + (PrevCursorPosInTimer.Y - MousePosition.Y) ^ 2)
                            Else
                                r = Math.Max(0, k * maxR * 30) '^ 2) / 200
                            End If
                            If DragDropCandidateIndex <> -1 Then r = 0 ' r / 3


                            dyr = r * Math.Sin((2 * Math.PI * n) / (NDraggingFiles - 1) + SelectedAngleOffset)
                            dxr = r * Math.Cos((2 * Math.PI * n) / (NDraggingFiles - 1) + SelectedAngleOffset)

                            'If DragDropCandidateIndex <> -1 Then
                            '    Image(i).DestX -= (Image(i).DestX - (Image(DragDropCandidateIndex).X)) '* Rnd() '* 10 - 5
                            '    Image(i).DestY -= (Image(i).DestY - (Image(DragDropCandidateIndex).Y)) '* Rnd() '* 4 - 2
                            'Else
                            Image(i).DestX -= (Image(i).DestX - (Image(ChosenObj).X)) '* Rnd() '* 10 - 5
                            Image(i).DestY -= (Image(i).DestY - (Image(ChosenObj).Y)) '* Rnd() '* 4 - 2
                            'End If
                            Image(i).DestX += dxr
                            Image(i).DestY += dyr

                            If DragDropCandidateIndex <> -1 Then
                                'dx = Image(i).DestX - Image(DragDropCandidateIndex).DestX
                                'dy = Image(i).DestY - Image(DragDropCandidateIndex).DestY
                                Image(i).DestTransparency = 1 '0.1 'Math.Max(Math.Min((Math.Sqrt(dx * dx + dy * dy) - r) / r, 0.9), 0)
                            Else
                                Image(i).DestTransparency = 1 '0.9
                            End If

                            If Image(i).Animate = False Then
                                Image(i).Animate = True
                                IsAnimatedImages = True
                                FlyingAlgorithm = FlyingAlgorithms.Simple
                            End If
                        Else
                            If DragDropCandidateIndex <> -1 Then Image(ChosenObj).DestTransparency = 0.5 Else Image(ChosenObj).DestTransparency = 1
                        End If
                        'DrawPicture(i)
                    Next
                Else
                    Dim i As Long
                    'SelectedAngleOffset -= 0.01
                    For nn As Long = 1 To NDraggingFiles
                        i = DraggingFilesList(nn)
                        If i <> ChosenObj Then
                            'Image(i).X = Image(ChosenObj).X
                            Image(i).DestX = Image(ChosenObj).X
                            Image(i).DestY = Image(ChosenObj).Y + ((nn - IndexOfChosenInDraggingFiles) Mod 10) * Wire.Y ' * NDraggingFiles / (Math.Abs(nn - IndexOfChosenInDraggingFiles))
                            If DragDropCandidateIndex <> -1 Then
                                'Image(i).DestX = Image(DragDropCandidateIndex).X
                                'Image(i).DestY = Image(DragDropCandidateIndex).Y
                                Image(i).DestX = Image(ChosenObj).X
                                Image(i).DestY = Image(ChosenObj).Y
                            End If

                            Image(i).DestTransparency = 1

                            If Math.Abs(nn - IndexOfChosenInDraggingFiles) > 10 Then
                                If Math.Abs(nn - IndexOfChosenInDraggingFiles) < 20 Then
                                    Image(i).DestTransparency = 1
                                Else
                                    Image(i).DestTransparency = 1
                                End If
                            End If
                            If DragDropCandidateIndex <> -1 Then Image(i).DestTransparency = 0

                            If Image(i).Animate = False Then
                                Image(i).Animate = True
                                IsAnimatedImages = True
                                FlyingAlgorithm = FlyingAlgorithms.Simple
                            End If
                        Else
                            If DragDropCandidateIndex <> -1 Then Image(ChosenObj).DestTransparency = 0.5 Else Image(ChosenObj).DestTransparency = 1
                        End If
                    Next
                End If
            End If
        End If
    End Sub
    Private Sub DrawRightButtonCircle()
        GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim D As Long = Math.Sqrt((FirstX - PrevCursorPos.X) ^ 2 + (FirstY - PrevCursorPos.Y) ^ 2)
        If D <= 5 Then
            D *= 50
            GraphicsMain.DrawEllipse(New Pen(Color.FromArgb(D, Color.Green), 1), New RectangleF(FirstX - 5, FirstY - 5, 10, 10))
        End If

        Dim r As New RectangleF(PrevCursorPos.X - RightButtonCircleRadius / 2 + 3, PrevCursorPos.Y - RightButtonCircleRadius / 2 + 3, RightButtonCircleRadius - 6, RightButtonCircleRadius - 6)
        Dim b As SolidBrush = New SolidBrush(Color.FromArgb(Math.Min(RightButtonCircleOpacity * 0.7, 255), 255, 255, 255))
        GraphicsMain.FillEllipse(b, r)

        r = New RectangleF(PrevCursorPos.X - RightButtonCircleRadius / 2, PrevCursorPos.Y - RightButtonCircleRadius / 2, RightButtonCircleRadius, RightButtonCircleRadius)
        Dim c As New Pen(Color.FromArgb(Math.Min(RightButtonCircleOpacity * 1.2, 255), 255, 255, 255), 4)
        'If CursorMoved Then c.Color = Color.FromArgb(RightButtonCircleOpacity, Color.Red)
        'If LeftMouseButton Then    c = New Pen(Color.FromArgb(Math.Min(RightButtonCircleOpacity * 1.6, 255), 0, 255, 255), 4)
        'If RightMouseButton Then   c = New Pen(Color.FromArgb(Math.Min(RightButtonCircleOpacity * 1.6, 255), 255, 255, 0), 4)
        GraphicsMain.DrawEllipse(c, r)

        c = New Pen(Color.FromArgb(RightButtonCircleOpacity, 0, 0, 0), 1)
        r = New RectangleF(PrevCursorPos.X - RightButtonCircleRadius / 2 + 2, PrevCursorPos.Y - RightButtonCircleRadius / 2 + 2, RightButtonCircleRadius - 4, RightButtonCircleRadius - 4)
        GraphicsMain.DrawEllipse(c, r)
        r = New RectangleF(PrevCursorPos.X - RightButtonCircleRadius / 2 - 2, PrevCursorPos.Y - RightButtonCircleRadius / 2 - 2, RightButtonCircleRadius + 4, RightButtonCircleRadius + 4)
        GraphicsMain.DrawEllipse(c, r)

        GraphicsMain.SmoothingMode = Drawing2D.SmoothingMode.None
    End Sub
    Private Sub DrawStatus()
        'Dim str As String = CurrentLoadingImgIndex.ToString + " | " + NImages.ToString
        'Dim b As New SolidBrush(BGColor)
        'Dim b2 As New SolidBrush(Color.FromArgb(240, 240, 240))
        'GraphicsMain.FillRectangle(b, 20 - 1, BmpMain.Height - 34, GraphicsMain.MeasureString(str, font_filename).Width + 2, 12)
        'Dim p As New Pen(Color.FromArgb(120, BGColor))
        'GraphicsMain.DrawRectangle(p, 20 - 2, BmpMain.Height - 35, GraphicsMain.MeasureString(str, font_filename).Width + 3, 13)

        'GraphicsMain.DrawString(str, font_filename, b2, 20, BmpMain.Height - 35 + 1)
        'GraphicsMain.DrawString(str, font_filename, Brushes.Black, 20, BmpMain.Height - 35)

        'Dim RectF As New RectangleF(0, BmpMain.Height - 2, (BmpMain.Width) * (CurrentLoadingImgIndex / NImages), 2)
        Dim RectF As New RectangleF(0, 1, (BmpMain.Width) * (CurrentLoadingImgIndex / NImages), 2)
        GraphicsMain.FillRectangle(New SolidBrush(Color.FromArgb(19, 130, 206)), RectF)
        'GraphicsMain.DrawImageUnscaled(GenerateStatusBar(300, CurrentLoadingImgIndex / NImages), 2, BmpMain.Height - 19)
    End Sub
    Public Sub DrawFrame3(ByRef Grafic As System.Drawing.Graphics, ByRef Bitm As Bitmap)
        Dim p1 As New Pen(Color.FromArgb(BGColor.R * 0.3, BGColor.R * 0.3, BGColor.R * 0.3))
        Grafic.DrawLine(p1, 0, 0, Bitm.Width, 0)

        Dim I As Integer = (200 * 0.6 + (BGColor.R * 0.3) * 0.4)
        Dim Color1 As Color = Color.FromArgb(I, I, I)
        I = ((BGColor.R * 0.3) * 0.8 + BGColor.R * 0.2)
        Dim Color2 As Color = Color.FromArgb(I, I, I)

        'Bitm.SetPixel(1, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, 1, Color2)
        Bitm.SetPixel(1, 0, Color2)
        Bitm.SetPixel(0, 0, Color1)
        'Bitm.SetPixel(Bitm.Width - 2, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, 1, Color2)
        Bitm.SetPixel(Bitm.Width - 2, 0, Color2)
        Bitm.SetPixel(Bitm.Width - 1, 0, Color1)

        Dim Color3 As Color = Color.FromArgb(BGColor.R * 0.4, BGColor.R * 0.4, BGColor.R * 0.4)
        Dim Color4 As Color = Color.FromArgb(BGColor.R * 0.77, BGColor.R * 0.77, BGColor.R * 0.77)
        'Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 2, Color4)
        Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 1, Color4)
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 1, Color2) '.FromArgb(150, 150, 150))
        'Bitm.SetPixel(1, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, Bitm.Height - 2, Color4)
        Bitm.SetPixel(1, Bitm.Height - 1, Color4)
        Bitm.SetPixel(0, Bitm.Height - 1, Color2)
    End Sub
    Private Sub DrawCanvasAutoScrollingArea()
        If CanvasAutoScrolling = True Then
            Dim x = MousePosition.X - Me.Left - Me.Parent.Left
            Dim y = MousePosition.Y - Me.Top - Me.Parent.Top

            Dim AreaBGBrush As New SolidBrush(Color.FromArgb(Math.Min(120 + 80 * Math.Abs(Canvas.V + Canvas.VX) / CanvasAutoScrollingMaxV, 220), 19, 130, 206))
            Dim AreaBorderPen As New Pen(Color.FromArgb(120 + 80 * Math.Min(Math.Abs(Canvas.V + Canvas.VX) / CanvasAutoScrollingMaxV, 1), 50, 50, 50))

            Dim TextOffset As Long
            If Arrangement <> ArrangmentTypes.Õorizontal Then
                If Canvas.X < 0 And x < CanvasAutoScrollingActiveAreaWidth Then
                    GraphicsMain.FillRectangle(AreaBGBrush, 0, 0, CanvasAutoScrollingActiveAreaWidth, BmpMain.Height)
                    GraphicsMain.DrawLine(AreaBorderPen, CanvasAutoScrollingActiveAreaWidth, 0, CanvasAutoScrollingActiveAreaWidth, BmpMain.Height)

                    TextOffset = 16
                    If y > CanvasAutoScrollingActiveAreaWidth Then
                        GraphicsMain.DrawString("page" + vbNewLine + "home", New Font("Verdana", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(150, 255, 255, 255)), TextOffset, TextOffset)
                    Else
                        GraphicsMain.DrawString("page" + vbNewLine + "home", New Font("Verdana", 10, FontStyle.Bold), Brushes.White, TextOffset, TextOffset)
                    End If
                ElseIf Canvas.X > Canvas.MinX And x > Me.Width - CanvasAutoScrollingActiveAreaWidth Then
                    GraphicsMain.FillRectangle(AreaBGBrush, BmpMain.Width - CanvasAutoScrollingActiveAreaWidth, 0, CanvasAutoScrollingActiveAreaWidth, BmpMain.Height)
                    GraphicsMain.DrawLine(AreaBorderPen, BmpMain.Width - CanvasAutoScrollingActiveAreaWidth, 0, BmpMain.Width - CanvasAutoScrollingActiveAreaWidth, BmpMain.Height)

                    TextOffset = 22
                    If y < BmpMain.Height - CanvasAutoScrollingActiveAreaWidth Then
                        GraphicsMain.DrawString("page" + vbNewLine + "end", New Font("Verdana", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(150, 255, 255, 255)), BmpMain.Width - CanvasAutoScrollingActiveAreaWidth + TextOffset, BmpMain.Height - CanvasAutoScrollingActiveAreaWidth + TextOffset)
                    Else
                        GraphicsMain.DrawString("page" + vbNewLine + "end", New Font("Verdana", 10, FontStyle.Bold), Brushes.White, BmpMain.Width - CanvasAutoScrollingActiveAreaWidth + TextOffset, BmpMain.Height - CanvasAutoScrollingActiveAreaWidth + TextOffset)
                    End If
                End If
            Else
                If Canvas.Y < 0 And y < CanvasAutoScrollingActiveAreaWidth Then
                    GraphicsMain.FillRectangle(AreaBGBrush, 0, 0, BmpMain.Width, CanvasAutoScrollingActiveAreaWidth)
                    GraphicsMain.DrawLine(AreaBorderPen, 0, CanvasAutoScrollingActiveAreaWidth, BmpMain.Width, CanvasAutoScrollingActiveAreaWidth)
                ElseIf Canvas.Y > Canvas.MinY And y > Me.Height - CanvasAutoScrollingActiveAreaWidth Then
                    GraphicsMain.FillRectangle(AreaBGBrush, 0, BmpMain.Height - CanvasAutoScrollingActiveAreaWidth, BmpMain.Width, CanvasAutoScrollingActiveAreaWidth)
                    GraphicsMain.DrawLine(AreaBorderPen, 0, BmpMain.Height - CanvasAutoScrollingActiveAreaWidth, BmpMain.Width, BmpMain.Height - CanvasAutoScrollingActiveAreaWidth)
                End If
            End If
        End If
    End Sub
#End Region

    Public RedrawOnce As Boolean = False
    Public Sub RedrawAll()
        RedrawOnce = False

        Draw_Picturies()
        DrawCanvasAutoScrollingArea()

        If ChosenObj = -1111 Then DrawSelectionArea()
        UpdateAndDrawButtons()
        If RightButtonCircleRadius > 0 Then DrawRightButtonCircle()
        DrawDraggingInfo()
        If CurrentLoadingImgIndex < NImages And Not StopLoading And IsNotEverithingLoaded Then DrawStatus()
        If BmpMain.Height > 2 And DrawBorders Then DrawFrame3(GraphicsMain, BmpMain)

        'APP INFO
        'GraphicsMain.DrawString(ChosenObj.ToString, font_filename, Brushes.White, BmpMain.Width - 150, BmpMain.Height - 15 - 10)
        'GraphicsMain.DrawString(Path, font_filename, Brushes.White, 20, BmpMain.Height - 15)
        If ShowFPS Then DrawFPS()
        'If Me.Focused Then GraphicsMain.DrawString("focused", font_filename, Brushes.LightGray, BmpMain.Width - 150, BmpMain.Height - 15)
    End Sub


    Private Sub ucImagesBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DoResizeEvent = True

        BmpMain = New Bitmap(Me.Width, Me.Height)
        GraphicsMain = Graphics.FromImage(BmpMain)
        InitButtons()
        RedrawOnce = True

        picMain.Left = 0
        picMain.Top = 0

        'MsgBox(SystemInformation.DoubleClickTime.ToString)
        tmrWaitForDC.Interval = SystemInformation.DoubleClickTime
    End Sub

    Dim CanvasAutoScrolling As Boolean = False
    Dim CanvasAutoScrollingMaxV As Double = 40
    Dim CanvasAutoScrollingActiveAreaWidth As Long = 80
    Private Sub MaybeMoveCanvasIfDragDrop()
        'If ChosenObj > 0 And CursorMoved Then If Image(ChosenObj).Transparency > 0.6 Then Image(ChosenObj).Transparency -= 0.05
        Dim WasScrolling As Boolean = CanvasAutoScrolling
        If CanvasAutoScrolling Or (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 0.2 Then
            Dim d As Double = 0

            Dim x = MousePosition.X - Me.Left - Me.Parent.Left
            Dim y = MousePosition.Y - Me.Top - Me.Parent.Top

            If IsSmthDragging And ChosenObj > 0 And NDraggingFiles > 0 Then
                If Arrangement <> ArrangmentTypes.Õorizontal Then
                    If Canvas.X < 0 And x < CanvasAutoScrollingActiveAreaWidth Then
                        d = (CanvasAutoScrollingActiveAreaWidth - x)
                        d = CanvasAutoScrollingMaxV * d / CanvasAutoScrollingActiveAreaWidth
                        d = Math.Max(0, Math.Min(CanvasAutoScrollingMaxV, d))

                        If (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 0 And y < CanvasAutoScrollingActiveAreaWidth Then
                            Canvas.Animate = True : Canvas.DestX = 0
                        Else
                            Canvas.Animate = False
                            Canvas.VX += 3
                            Canvas.V = 0
                            If Canvas.VX > d Then Canvas.VX = d
                        End If
                        CanvasAutoScrolling = True
                    ElseIf Canvas.X > Canvas.MinX And x > Me.Width - CanvasAutoScrollingActiveAreaWidth Then
                        d = x - (Me.Width - CanvasAutoScrollingActiveAreaWidth)
                        d = CanvasAutoScrollingMaxV * d / CanvasAutoScrollingActiveAreaWidth
                        d = Math.Max(0, Math.Min(CanvasAutoScrollingMaxV, d))

                        If (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 0 And y > Me.Height - CanvasAutoScrollingActiveAreaWidth Then
                            Canvas.Animate = True : Canvas.DestX = Canvas.MinX
                        Else
                            Canvas.Animate = False
                            Canvas.VX -= 3
                            Canvas.V = 0
                            If Canvas.VX < -d Then Canvas.VX = -d
                        End If
                        CanvasAutoScrolling = True
                    Else
                        If WasScrolling Then LastBigMouseMoveTime = DateTime.Now
                        CanvasAutoScrolling = False
                    End If
                Else
                    If Canvas.Y < 0 And y < CanvasAutoScrollingActiveAreaWidth Then
                        d = CanvasAutoScrollingActiveAreaWidth - y
                        d = CanvasAutoScrollingMaxV * d / CanvasAutoScrollingActiveAreaWidth
                        d = Math.Max(0, Math.Min(CanvasAutoScrollingMaxV, d))

                        If (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 1 And x < CanvasAutoScrollingActiveAreaWidth Then
                            Canvas.Animate = True : Canvas.DestY = 0
                        Else
                            Canvas.Animate = False
                            Canvas.V += 3
                            Canvas.VX = 0
                            If Canvas.V > d Then Canvas.V = d
                        End If
                        CanvasAutoScrolling = True
                    ElseIf Canvas.Y > Canvas.MinY And y > Me.Height - CanvasAutoScrollingActiveAreaWidth Then
                        d = y - (Me.Height - CanvasAutoScrollingActiveAreaWidth)
                        d = CanvasAutoScrollingMaxV * d / CanvasAutoScrollingActiveAreaWidth
                        d = Math.Max(0, Math.Min(CanvasAutoScrollingMaxV, d))
                        If (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 1 And x > Me.Width - CanvasAutoScrollingActiveAreaWidth Then
                            Canvas.Animate = True : Canvas.DestY = Canvas.MinY
                        Else
                            Canvas.Animate = False
                            Canvas.V -= 3
                            Canvas.VX = 0
                            If Canvas.V < -d Then Canvas.V = -d
                        End If
                        CanvasAutoScrolling = True
                    Else
                        If WasScrolling Then LastBigMouseMoveTime = DateTime.Now
                        CanvasAutoScrolling = False
                    End If
                End If
            Else
                CanvasAutoScrolling = False
            End If
        End If
    End Sub
    Private Sub RecalculateCanvasPosition()
        Dim PrevX As Double = Canvas.X
        Dim PrevY As Double = Canvas.Y
        If Canvas.Animate Then
            If Arrangement = ArrangmentTypes.Õorizontal Then Canvas.DestX = 0
            If Arrangement = ArrangmentTypes.Vertical Then Canvas.DestY = 0
            If (Canvas.DestY = Math.Round(Canvas.Y) And Canvas.DestX = Math.Round(Canvas.X)) Or Animation = False Then
                Canvas.X = Canvas.DestX
                Canvas.Y = Canvas.DestY
                Canvas.Animate = False
            Else
                Canvas.V = (Canvas.DestY - Canvas.Y) / 6
                Canvas.VX = (Canvas.DestX - Canvas.X) / 6
                Canvas.Y += Canvas.V
                Canvas.X += Canvas.VX

                If Canvas.X > 0 And Canvas.DestX > 0 Then Canvas.Animate = False
                If Canvas.Y > 0 And Canvas.DestY > 0 Then Canvas.Animate = False
                If Canvas.X < Canvas.MinX And Canvas.DestX < Canvas.MinX Then Canvas.Animate = False
                If Canvas.Y < Canvas.MinY And Canvas.DestY < Canvas.MinY Then Canvas.Animate = False
            End If
        Else
            If Not (IsMouseDown) Then ' And (ChosenObj = 0 Or ChosenObj = -3 Or (ChosenObj > 0 And CursorMoved = False))) Then
                If Animation = False Then
                    Canvas.VX = 0 : Canvas.V = 0
                Else
                    Canvas.Y += Canvas.V : Canvas.X += Canvas.VX
                End If

                If Arrangement = ArrangmentTypes.Õorizontal Then
                    If Canvas.Y > 0 Then
                        Canvas.Y -= Canvas.Y / 8
                        Canvas.V = Canvas.V * 0.7
                    ElseIf Canvas.Y < Canvas.MinY Then
                        Canvas.Y -= (Canvas.Y - Canvas.MinY) / 8
                        If (-Canvas.Y + Canvas.MinY) < 0.4 Then Canvas.Y = Canvas.MinY
                        Canvas.V = Canvas.V * 0.7
                    End If

                    'If Math.Abs(Canvas.VX) < 2 Then
                    Dim D As Double = 1 - Math.Sqrt((Math.Abs(Canvas.VX)) / 2)
                    If Canvas.X > 0 Then
                        'Canvas.X -= Canvas.X / 8 * D
                        Canvas.X -= Canvas.X / (8 * (1 + Math.Abs(Canvas.VX)))
                    ElseIf Canvas.X < Canvas.MinX Then
                        'Canvas.X -= (Canvas.X - Canvas.MinX) / 8 * D
                        Canvas.X -= (Canvas.X - Canvas.MinX) / (8 * (1 + Math.Abs(Canvas.VX)))
                        If (-Canvas.X + Canvas.MinX) < 0.4 Then Canvas.X = Canvas.MinX
                    End If
                    'End If
                    If Canvas.X > Canvas.Width Then
                        Canvas.Animate = True : Canvas.X = -Canvas.Width : Canvas.VX = 0 : Canvas.DestX = 0 : Canvas.DestY = 0
                        RaiseEvent BackSpaceKey(True)
                    ElseIf Canvas.X < -Canvas.Width Then
                        Canvas.Animate = True : Canvas.X = Canvas.Width : Canvas.VX = 0 : Canvas.DestX = 0 : Canvas.DestY = 0
                        RaiseEvent GoForvard(True)
                    End If
                Else
                    If Canvas.MinX > 0 Then
                        Canvas.X -= (Canvas.X + (Canvas.Width - Me.Width) / 2) / 8
                        Canvas.VX = Canvas.VX * 0.7
                    ElseIf Canvas.X > 0 Then
                        Canvas.X -= Canvas.X / 8
                        Canvas.VX = Canvas.VX * 0.7 * (1 - Canvas.X / BmpMain.Width)
                    ElseIf Canvas.X < Canvas.MinX Then
                        Canvas.X -= (Canvas.X - Canvas.MinX) / 8
                        If (-Canvas.X + Canvas.MinX) < 0.4 Then Canvas.X = Canvas.MinX
                        Canvas.VX = Canvas.VX * 0.7 * (1 - (-Canvas.X + Canvas.MinX) / BmpMain.Width)
                    End If

                    'If Math.Abs(Canvas.V) < 2 Then
                    Dim D As Double = 1 - Math.Sqrt((Math.Abs(Canvas.V)) / 2)
                    If Canvas.Y > 0 Then
                        'Canvas.Y -= Canvas.Y / 8 * D
                        Canvas.Y -= Canvas.Y / (8 * (1 + Math.Abs(Canvas.V)))
                    ElseIf Canvas.Y < Canvas.MinY Then
                        'Canvas.Y -= (Canvas.Y - Canvas.MinY) / 8 * D
                        Canvas.Y -= (Canvas.Y - Canvas.MinY) / (8 * (1 + Math.Abs(Canvas.V)))
                        If (-Canvas.Y + Canvas.MinY) < 0.4 Then Canvas.Y = Canvas.MinY
                    End If
                    'End If
                    If Canvas.Y > Canvas.Height Then
                        Canvas.Animate = True : Canvas.Y = -Canvas.Height : Canvas.V = 0 : Canvas.DestY = 0 : Canvas.DestX = 0
                        RaiseEvent BackSpaceKey(True)
                    ElseIf Canvas.Y < -Canvas.Height Then
                        Canvas.Animate = True : Canvas.Y = Canvas.Height : Canvas.V = 0 : Canvas.DestY = 0 : Canvas.DestX = 0
                        RaiseEvent GoForvard(True)
                    End If
                End If

                If Not CanvasAutoScrolling Then
                    Canvas.V = Canvas.V * 0.9
                    Canvas.VX = Canvas.VX * 0.9
                End If

                If Math.Abs(Canvas.V) < 0.2 Then Canvas.V = 0
                If Math.Abs(Canvas.Y) < 0.4 Then Canvas.Y = 0
                If Math.Abs(Canvas.VX) < 0.2 Then Canvas.VX = 0
                If Math.Abs(Canvas.X) < 0.2 Then Canvas.X = 0
            End If
        End If

            If (IsSmthDragging And ChosenObj > 0) Then
                With Image(ChosenObj)
                    .X += PrevX - Canvas.X : .DestX = .X
                    .Y += PrevY - Canvas.Y : .DestY = .Y
                End With
            End If
    End Sub
    Private Sub RecalculateImagesPosition()
        Dim proc As Double = 0.22
        If IsAnimatedImages Then
            IsAnimatedImages = False
            For I As Long = 1 To NImages
                If Image(I).Animate = True Then
                    With Image(I)
                        If (Math.Abs(.X - .DestX) < 0.4 And Math.Abs(.Y - .DestY) < 0.4) Or _
                            (.X > -Canvas.X + Canvas.Width And .DestX > -Canvas.X + Canvas.Width) Or _
                            (.X < -Canvas.X - Wire.X And .DestX < -Canvas.X - Wire.X) Or _
                            (.Y > -Canvas.Y + Canvas.Height And .DestY > -Canvas.Y + Canvas.Height) Or _
                            (.Y < -Canvas.Y - Wire.Y And .DestY < -Canvas.Y - Wire.Y) Then
                            .X = Math.Round(.DestX) : .Y = Math.Round(.DestY)
                            If Math.Abs(.Transparency - .DestTransparency) < 0.05 And Math.Abs(.Width - .DestWidth) < 0.4 And Math.Abs(.Height - .DestHeight) < 0.4 Then .Animate = False
                        Else
                            If FlyingAlgorithm = FlyingAlgorithms.Simple Then 'Wire.X + Wire.dX Then
                                .X += (.DestX - .X) * proc : .Y += (.DestY - .Y) * proc
                            Else
                                If Arrangement = ArrangmentTypes.Õorizontal Then
                                    If .DestY <= .Y - Wire.Y Then
                                        Dim d As Long
                                        d = (Me.Width + Wire.X) * (.DestY - .Y) / (Wire.dY + Wire.Y)
                                        d += .DestX - .X
                                        .X += d * proc
                                        If .X < -Wire.X Then
                                            .Y -= Wire.Y + Wire.dY
                                            '.X = .DestX
                                            .X += Me.Width + Wire.X '=Me.Height + Canvas.Plus 
                                        End If
                                    ElseIf .DestY >= .Y + Wire.Y Then
                                        Dim d As Long
                                        d = (Me.Width + Wire.X) * (.DestY - .Y) / (Wire.dY + Wire.Y)
                                        d += .DestX - .X
                                        .X += d * proc
                                        If .X > Me.Width Then
                                            .Y += Wire.Y + Wire.dY
                                            '.X = .DestX
                                            .X -= Me.Width + Wire.X  '=-Wire.Y - Canvas.Plus 
                                        End If
                                    Else
                                        .X += (.DestX - .X) * proc
                                        .Y += (.DestY - .Y) * proc
                                    End If
                                Else
                                    If .DestX <= .X - Wire.X Then
                                        Dim d As Long
                                        d = (Me.Height + Wire.Y) * (.DestX - .X) / (Wire.dX + Wire.X)
                                        d += .DestY - .Y
                                        .Y += d * proc
                                        If .Y < -Wire.Y Then
                                            .X -= Wire.X + Wire.dX
                                            '.X = .DestX
                                            .Y += Me.Height + Wire.Y '=Me.Height + Canvas.Plus 
                                        End If
                                    ElseIf .DestX >= .X + Wire.X Then
                                        Dim d As Long
                                        d = (Me.Height + Wire.Y) * (.DestX - .X) / (Wire.dX + Wire.X)
                                        d += .DestY - .Y
                                        .Y += d * proc
                                        If .Y > Me.Height Then
                                            .X += Wire.X + Wire.dX
                                            '.X = .DestX
                                            .Y -= Me.Height + Wire.Y  '=-Wire.Y - Canvas.Plus 
                                        End If
                                    Else
                                        .X += (.DestX - .X) * proc
                                        .Y += (.DestY - .Y) * proc
                                    End If
                                End If
                            End If
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
    End Sub
    Private Sub RecalculateRightButtonCircle()
        If RightButtonCircleRadius <> 0 And MDTime = 0 Then
            RightButtonCircleRadius -= RightButtonCircleLastDelta
            RightButtonCircleOpacity *= 0.88
            If RightButtonCircleOpacity < 3 Or RightButtonCircleRadius < 1 Then RightButtonCircleRadius = 0 : RightButtonCircleOpacity = 0 : RedrawOnce = True
        End If
    End Sub
    Public Sub NextFrame(ByRef DrawInAnyWay As Boolean)
        MaybeMoveCanvasIfDragDrop()
        RecalculateCanvasPosition()
        RecalculateImagesPosition()
        RecalculateRightButtonCircle()

        'isenterdown
        If Not (Control.ModifierKeys = Keys.Enter Or (IsMouseDown And CursorMoved = False)) Then
            'If SelInt > 0 Then
            SelInt *= 0.96 : If SelInt < 0.05 Then SelInt = 0 '-= 0.025
            'If SelInt < 0 Then SelInt *= 0.96 : If SelInt > -0.04 Then SelInt = 0 '-= 0.025
        End If

        If (RedrawOnce Or SelInt <> 0 Or RightButtonCircleRadius > 0 Or DrawInAnyWay Or IsSmthDragging Or NDraggingFiles > 0 Or _
        IsMouseDown Or IsAnimatedImages = True Or _
        (Canvas.V <> 0 Or Canvas.VX <> 0) Or _
        (Canvas.Y > 0 Or Canvas.Y < Canvas.MinY) Or _
        (Canvas.X > 0 Or Canvas.X < Canvas.MinX) Or _
        Canvas.Animate) Then
            If Me.Height > 1 Then RedrawAll()
        Else
            If CurrentLoadingImgIndex < NImages And Not StopLoading And IsNotEverithingLoaded Then
                DrawStatus()
                If BmpMain.Height > 2 And DrawBorders Then DrawFrame3(GraphicsMain, BmpMain)
            End If
        End If
        'GraphicsMain.DrawLine(Pens.Black, 0, 0, (New Random(DateTime.Now.Millisecond)).Next(100), (New Random(DateTime.Now.Second)).Next(100))
    End Sub


    Private Sub FindWidthWithText_NoReduce(ByVal i As Long)
        Image(i).Name = Image(i).OriginalName
        Dim w As Long = 0
        If Image(i).Loaded Then w = Thumbnail(i).Width

        If Image(i).Type = FileTypes.Music Or Image(i).Type = FileTypes.Drive Then
            If Image(i).Type = FileTypes.Music Then
                Image(i).Name = FileTags.Tags(Image(i).InTagsIngex).Song.Name
                Image(i).Singer = FileTags.Tags(Image(i).InTagsIngex).Song.Singer
            End If
            If Wire.Y >= 26 And Wire.Y < 80 Then
                Image(i).WidthWithText = GraphicsMain.MeasureString(Image(i).Singer, font_singer).Width
                Image(i).WidthWithText = Math.Max(Image(i).WidthWithText, GraphicsMain.MeasureString(Image(i).Name, font_bold).Width)
                Image(i).WidthWithText += w + 5
            ElseIf Wire.Y < 26 Then
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

        If Wire.Y > 40 And Image(i).Type = FileTypes.Image Then
            Image(i).WidthWithText = Wire.X 'Thumbnail(I).Width 
        End If
    End Sub
    Private Sub OptimizeStringForWidth(ByRef Str As String, ByRef CurWidth As Long, ByVal DestWidth As Long, ByVal CurFont As Font)
        CurWidth = GraphicsMain.MeasureString(Str, CurFont).Width
        While (CurWidth > DestWidth And Str.Length > 3)
            Str = Mid(Str, 1, Str.Length - 4) + "..."
            CurWidth = GraphicsMain.MeasureString(Str, CurFont).Width
        End While
    End Sub
    Private Sub FindWidthWithText(ByVal I As Long)
        Image(I).Name = Image(I).OriginalName
        Dim w As Long = 0
        If Image(I).Loaded Then w = Thumbnail(I).Width

        If Image(I).Type = FileTypes.Music Or Image(I).Type = FileTypes.Drive Then
            If Image(I).Type = FileTypes.Music Then
                Image(I).Name = FileTags.Tags(Image(I).InTagsIngex).Song.Name
                Image(I).Singer = FileTags.Tags(Image(I).InTagsIngex).Song.Singer
            End If
            If Wire.Y >= 26 And Wire.Y < 80 Then
                OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X - w - Wire.dX, font_bold)
                OptimizeStringForWidth(Image(I).Singer, Image(I).WidthWithText, Wire.X - w - Wire.dX, font_singer)
                Image(I).WidthWithText = Math.Max(Image(I).WidthWithText, GraphicsMain.MeasureString(Image(I).Name, font_bold).Width)
                Image(I).WidthWithText += w + Wire.dX
            ElseIf Wire.Y < 26 Then
                If Image(I).Singer <> "noname" Then
                    Image(I).WidthWithText = GraphicsMain.MeasureString("- " + Image(I).Name, font_filename).Width
                    Image(I).WidthWithText += GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width + 3
                    While Image(I).WidthWithText > Wire.X - w - 5 And (Image(I).Name.Length > 7 Or Image(I).Singer.Length > 7)
                        If Image(I).Name.Length > 7 Then Image(I).Name = Mid(Image(I).Name, 1, Image(I).Name.Length - 4) + "..."
                        If Image(I).Singer.Length > 7 Then Image(I).Singer = Mid(Image(I).Singer, 1, Image(I).Singer.Length - 4) + "..."

                        Image(I).WidthWithText = GraphicsMain.MeasureString("- " + Image(I).Name, font_filename).Width
                        Image(I).WidthWithText += GraphicsMain.MeasureString(Image(I).Singer, font_singer).Width + 3
                    End While
                    Image(I).WidthWithText += w + 5
                Else
                    OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X - w - Wire.dX, font_filename)
                    Image(I).WidthWithText += w + Wire.dX
                End If
            Else
                OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X - Wire.dX * 2, font_bold)
                OptimizeStringForWidth(Image(I).Singer, Image(I).WidthWithText, Wire.X - Wire.dX * 2, font_singer)
                Image(I).WidthWithText = Wire.X
            End If
        ElseIf Image(I).Type = FileTypes.Image Then
            If Wire.Y <= 40 Then
                OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X - Wire.Y, font_filename)
                Image(I).WidthWithText += Wire.Y
            Else
                OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X, font_filename)
            End If
            If w < Wire.Y Then w = Wire.Y
        Else
            If Wire.Y < 32 Then
                OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X - 16, font_filename)
                Image(I).WidthWithText += 16 + 1
            Else
                OptimizeStringForWidth(Image(I).Name, Image(I).WidthWithText, Wire.X - 32, font_filename)
                Image(I).WidthWithText += 32 + 1
            End If
        End If

        If Wire.Y > 40 And Image(I).Type = FileTypes.Image Then
            Image(I).WidthWithText = Wire.X 'Thumbnail(I).Width 
        End If
    End Sub

    Private Sub LoadImediately(I As Long)
        CurrentLoadingFileName = Image(I).FileName
        CurrentLoadingImgIndex = I
        Image(I).Loading = True

        MakeThumbnail(Image(I).FileName)

        AfterLoading(I)
        'IsAnimatedImages = True
        'DrawStatus()
    End Sub
    Private Sub AfterLoading(I As Long)
        If Not Image(I).Loaded Then
            Image(I).InTagsIngex = FileTags.FindByFileName(Image(I).FileName)
            If Image(I).InTagsIngex = 0 Then Image(I).InTagsIngex = FileTags.Add(Image(I).FileName)
        End If

        If Wire.Y >= 32 Then
            For d As Long = 1 To FileTags.Tags(Image(I).InTagsIngex).Rating
                Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                    graf.DrawImage(bmpPlus, icoMusic32.Width - 9 * d - 1, icoMusic32.Height - bmpPlus.Height - 1)
                End Using
            Next
        Else
            For d As Long = 1 To FileTags.Tags(Image(I).InTagsIngex).Rating
                Using graf As Graphics = Graphics.FromImage(LoadedThumbnail)
                    graf.DrawImage(bmp_star, LoadedThumbnail.Width - d * 13 - 2 - 4, CInt((LoadedThumbnail.Height - 14) / 2))
                End Using
            Next
        End If

        Thumbnail(I) = LoadedThumbnail

        If Image(I).Loaded = False Then
            'If Wire.Y >= 40 Then Image(I).Y = Image(I).Y - Thumbnail(I).Height Else Image(I).X = Image(I).X + Wire.Y
            Image(I).Transparency = 0
            Image(I).Height = Thumbnail(I).Height
            Image(I).Width = Thumbnail(I).Width
        End If

        If Image(I).Type = FileTypes.NotDefined Then
            If IO.File.Exists(Image(I).FileName) Then Image(I).Type = FileTypes.File
            If IO.Directory.Exists(Image(I).FileName) Then Image(I).Type = FileTypes.Folder ' FileTags.Tags(Image(I).InTagsIngex).Type
            If FileTags.Tags(Image(I).InTagsIngex).Type = "music" Then Image(I).Type = FileTypes.Music
            If FileTags.Tags(Image(I).InTagsIngex).Type = "image" Then Image(I).Type = FileTypes.Image
            If (Image(I).FileName.Length > 4) Then
                If Image(I).FileName.Substring(Image(I).FileName.Length - 4).ToLower() = ".exe" Then Image(I).Type = FileTypes.ExeFile
            End If
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
    End Sub


    Private Sub tfTest()
        'If Not LoadedThumbnail Is Nothing Then LoadedThumbnail.Dispose
        MakeThumbnail(CurrentLoadingFileName)
    End Sub
    Dim ptsTest As New Threading.ParameterizedThreadStart(AddressOf tfTest)
    Dim test As Threading.Thread = New Threading.Thread(ptsTest)

    Dim StupidFlag As Boolean = False
    Dim DividerIndex As Long
    Private Sub tmrAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAnimation.Tick
        RedrawImage() 'Render()'tmrAnimation.Enabled = False
    End Sub
    Dim Rendering As Boolean = False
    Dim DrawingStarted As Date
    Private Sub Render()
        If Not Rendering Then
            Rendering = True
            'Task.Run(
            'Sub()
            While (1)
                DrawingStarted = DateAndTime.Now
                RedrawImage()
                Application.DoEvents()
                Threading.Thread.Sleep(Math.Max(20 - (DateAndTime.Now - DrawingStarted).Milliseconds, 0))
            End While
            'End Sub)
        End If
    End Sub
    Private Sub RedrawImage()
        If IsSmthDragging And (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 1 And ChosenObj > 0 And NDraggingFiles > 1 Then
            'Dim x, y As Long
            'x = MousePosition.X - Me.Left - Me.Parent.Left
            'y = MousePosition.Y - Me.Top - Me.Parent.Top

            'If Image(ChosenObj).Selected = True Then 'Sorting = "user" And
            '    For I As Long = 1 To NDraggingFiles
            '        Image(DraggingFilesList(I)).DestX = x - Wire.X / 2
            '        Image(DraggingFilesList(I)).DestY = y - Wire.Y / 2
            '        Image(DraggingFilesList(I)).Animate = True
            '    Next
            '    IsAnimatedImages = True
            'End If
        End If
        If IsSmthDragging And CanvasAutoScrolling = False Then
            Dim x, y As Long
            x = MousePosition.X - Me.Left - Me.Parent.Left - Canvas.X
            y = MousePosition.Y - Me.Top - Me.Parent.Top - Canvas.Y

            Dim ii As Short
            If Arrangement = ArrangmentTypes.Õorizontal Then
                x = x - Canvas.Plus + Wire.X / 2 : y = y - Wire.dY / 2
                ii = (Canvas.Columns * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
            Else
                y = y - Canvas.Plus + Wire.dY / 2 + Wire.Y / 2 : x = x - Wire.dX / 2
                ii = (Canvas.Columns * Math.Truncate(x / (Wire.X + Wire.dX)) + Math.Truncate(y / (Wire.Y + Wire.dY)) + 1)
            End If

            If ii < -1 Then ii = -1
            If ii > NImages + 1 Then ii = NImages + 1
            DividerIndex = ii

            If DragDropCandidateIndex = -1 And (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 0.1 Then
                If NDraggingFiles = 1 And Image(ChosenObj).Selected = False Then
                    If ii > DraggingFilesList(1) Then
                        ii = ii - 1
                        For i As Long = DraggingFilesList(1) To ii - 1
                            SwapImages(i, i + 1)
                            SetImageDestination(i)
                            IsAnimatedImages = True
                        Next
                    ElseIf ii < DraggingFilesList(1) Then
                        For i As Long = DraggingFilesList(1) To ii + 1 Step -1
                            SwapImages(i, i - 1)
                            SetImageDestination(i)
                            IsAnimatedImages = True
                        Next
                    End If
                    ChosenObj = ii
                    DraggingFilesList(1) = ii
                    SelectedImageIndex = ChosenObj
                End If
            ElseIf (DateTime.Now - LastBigMouseMoveTime).TotalSeconds > 0.8 Then
                If NDraggingFiles > 0 And Image(DragDropCandidateIndex).Selected = False And DragDropCandidateIndex > 0 Then
                    If Image(ChosenObj).Selected = True Then
                        'GraphicsMain.DrawRectangle(Pens.Chocolate, 50, 0, DragDropCandidateIndex * 10 + 100, 200)
                        'picMain.Image = BmpMain
                        'Me.Refresh()

                        Image(DragDropCandidateIndex).Selected = True
                        AddImageToDragDrop(DragDropCandidateIndex)  'StartDragDrop(ChosenObj)
                        DragDropCandidateIndex = -1
                        'StupidFlag = True
                    Else
                        Image(ChosenObj).Selected = True
                        Image(DragDropCandidateIndex).Selected = True
                        AddImageToDragDrop(DragDropCandidateIndex)  'StartDragDrop(ChosenObj)
                        DragDropCandidateIndex = -1
                    End If
                End If
            End If
        End If

        Dim td As TimeSpan, td2 As DateTimeOffset
        If InputLine <> "" Then
            td2 = System.DateTimeOffset.Now
            td = td2 - TimeFromTyping
            If (td.TotalMilliseconds) > 1400 Then
                InputLine = ""
            End If
        End If

        If ReloadWidthWithText Then
            For i As Long = 1 To NImages
                FindWidthWithText(i)
            Next
            ReloadWidthWithText = False
            'OrderImages() '                    NEW NEW NEW
        End If

        If Not StopLoading Then
            If IsNotEverithingLoaded Then
                If test.ThreadState <> Threading.ThreadState.Running Then
                    If CurrentLoadingImgIndex >= 1 And CurrentLoadingImgIndex <= NImages Then
                        If Image(CurrentLoadingImgIndex).Loading = True Then
                            AfterLoading(CurrentLoadingImgIndex)
                        End If
                    End If

                    Dim f As Boolean = False
                    Dim n As Long = 100
                    Dim WisibleImage As Long = 0
                    Dim i As Long
                    Dim LoadingStart As DateTime = DateTime.Now
                    Dim MaxSeconds As Double = 0.005
                    If SelectedImageIndex >= 1 And SelectedImageIndex <= NImages Then
                        i = SelectedImageIndex
                        If (Image(i).Loaded = False Or Image(i).ReLoaded = False) And IsImageVisible(i) Then
                            f = True
                            If Image(i).Type = FileTypes.Image Then
                                CurrentLoadingFileName = Image(i).FileName
                                CurrentLoadingImgIndex = i
                                Image(i).Loading = True
                                'bwLoadOne.RunWorkerAsync()
                                test = New Threading.Thread(ptsTest)
                                test.Start()
                                n = 0
                            Else
                                LoadImediately(i)
                                n -= 1
                            End If
                        End If
                    End If
                    If (n <> 0 And (DateTime.Now - LoadingStart).TotalSeconds < MaxSeconds) Or f = False Then
                        If Canvas.V >= 0 Or Canvas.VX >= 0 Then
                            For i = 1 To NImages
                                If (Image(i).Loaded = False Or Image(i).ReLoaded = False) And IsImageVisible(i) Then
                                    f = True
                                    If Image(i).Type = FileTypes.Image Then
                                        CurrentLoadingFileName = Image(i).FileName
                                        CurrentLoadingImgIndex = i
                                        Image(i).Loading = True
                                        'bwLoadOne.RunWorkerAsync()
                                        test = New Threading.Thread(ptsTest)
                                        test.Start()
                                        n = 0 : Exit For
                                    Else
                                        LoadImediately(i)
                                        n -= 1
                                        If n = 0 Or (DateTime.Now - LoadingStart).TotalSeconds > MaxSeconds Then Exit For
                                    End If
                                End If
                            Next
                        Else
                            For i = NImages To 1 Step -1
                                If (Image(i).Loaded = False Or Image(i).ReLoaded = False) And IsImageVisible(i) Then
                                    f = True
                                    If InStr(" jpg jpeg jpe gif png ico cur bmp exe inc ", " " + LCase(GetFileExtention(Image(i).FileName)) + " ") Then
                                        CurrentLoadingFileName = Image(i).FileName
                                        CurrentLoadingImgIndex = i
                                        Image(i).Loading = True
                                        'bwLoadOne.RunWorkerAsync()
                                        test = New Threading.Thread(ptsTest)
                                        test.Start()
                                        n = 0 : Exit For
                                    Else
                                        LoadImediately(i)
                                        n -= 1
                                        If n = 0 Or (DateTime.Now - LoadingStart).TotalSeconds > MaxSeconds Then Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If
                    If (n <> 0 And (DateTime.Now - LoadingStart).TotalSeconds < MaxSeconds) Or f = False Then
                        For i = 1 To NImages
                            If Image(i).Loaded = False Or Image(i).ReLoaded = False Then
                                'Image(i).DestTransparency = 0
                                'Image(i).Animate = True
                                f = True
                                If Image(i).Type = FileTypes.Image Then
                                    CurrentLoadingFileName = Image(i).FileName
                                    CurrentLoadingImgIndex = i
                                    Image(i).Loading = True
                                    'bwLoadOne.RunWorkerAsync()
                                    test = New Threading.Thread(ptsTest)
                                    test.Start()
                                    Exit For
                                Else
                                    LoadImediately(i)
                                    n -= 1
                                    If n = 0 Or (DateTime.Now - LoadingStart).TotalSeconds > MaxSeconds Then Exit For
                                End If

                                'MsgBox("BA2")
                            End If
                        Next
                    End If
                    If f = False Then
                        'MsgBox("BAA")
                        IsNotEverithingLoaded = False

                        If Path <> "" Then
                            If Mid(Path, 1, Path.Length - 2) = Application.StartupPath + "\35photo" Then
                                i = 0
                                FileOpen(1, Path + "urls.txt", OpenMode.Input)
                                While Not EOF(1)
                                    i += 1
                                    URLs35photo(i) = LineInput(1)
                                End While
                                FileClose(1)
                            End If
                            OrderImages()
                            RedrawOnce = True
                        End If
                    End If
                End If
            End If
        End If

        Dim l As Long = 0
        If td.TotalMilliseconds < 1400 Then
            If td.TotalMilliseconds > 600 Then
                l = 255 - (td.TotalMilliseconds - 600) / 800 * 255
            Else
                l = 255
            End If
        Else
            If td.TotalMilliseconds < 5000 Then
                RedrawOnce = True
                TimeFromTyping -= td + td
            End If
        End If

        If IsMouseDown And Not CursorMoved Then
            If MDTime < 100 Then
                MDTime = (DateTime.Now - MDMoment).TotalSeconds * 100 * 4
                If MDTime > 100 Then MDTime = 100
                RightButtonCircleOpacity = MDTime * 2 '- 6
                RightButtonCircleLastDelta = RightButtonCircleRadius
                RightButtonCircleRadius = Math.Abs(Math.Sin(MDTime * Math.PI / 120) * 100)
                RightButtonCircleLastDelta -= RightButtonCircleRadius
            End If
            If MDTime >= 100 Then
                MDTime = 100
                RightButtonPressedByPonter = True
                RightButtonCircleLastDelta = 1
            End If
        Else
            If MDTime < 100 Then MDTime = 0
        End If

        NextFrame(False) 'If Not bwDraw.IsBusy Then bwDraw.RunWorkerAsync() 

        If InputLine <> "" Then
            Dim br As New SolidBrush(Color.FromArgb(l, 220, 220, 220))
            Dim br2 As New SolidBrush(Color.FromArgb(90, 92, 94))
            If Mid(InputLine, 1, 1) = "¿" Or InputLine = "¿RENAME" Then
                Dim s As String = "command: " + Mid(InputLine, 2)
                GraphicsMain.FillRectangle(br2, 80, 3, GraphicsMain.MeasureString(s, font_filename).Width + 1, 15)
                GraphicsMain.DrawRectangle(Pens.Black, 80, 3, GraphicsMain.MeasureString(s, font_filename).Width + 1, 15)
                GraphicsMain.DrawString(s, font_filename, br, 81, 3)
            Else
                GraphicsMain.FillRectangle(br2, 80, 3, GraphicsMain.MeasureString(InputLine, font_filename).Width + 1, 15)
                GraphicsMain.DrawRectangle(Pens.Black, 80, 3, GraphicsMain.MeasureString(InputLine, font_filename).Width + 1, 15)
                GraphicsMain.DrawString(InputLine, font_filename, br, 81, 3)
            End If
        End If

        PrevCursorPosInTimer = MousePosition

        Try
            picMain.Size = Me.Size
            picMain.Image = BmpMain
            picMain.Refresh()
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub


    Private Sub ucImagesBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        RedrawOnce = True
    End Sub
    Private Sub picMain_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMain.LostFocus
        If IsMouseDown And ChosenObj > 0 And ChosenObj <= NImages Then
            StartDragDrop(ChosenObj)
        End If
    End Sub
    Private Sub ucImagesBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        RedrawOnce = True
    End Sub

    Private Sub ucImagesBox_QueryContinueDrag(sender As Object, e As QueryContinueDragEventArgs) Handles Me.QueryContinueDrag
        GraphicsMain.FillRectangle(Brushes.White, 0, 0, 100, 20)
        GraphicsMain.DrawString(e.Action.ToString, font_filename, Brushes.Black, 0, 0)
        If e.Action = DragAction.Drop Then
            NDraggingFiles = 0
            ForceRefreshingTimer()
            RemoveUnexistingItems()
        End If
        If ChosenObj > 0 And ChosenObj <= NImages And IsSmthDragging And DraggingCount > 0 And StupidFlag = True Then
            StartDragDrop(ChosenObj)
            StupidFlag = False
        End If
    End Sub
    Private Sub bwDraw_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwDraw.DoWork
        NextFrame(False)
    End Sub

    Private Sub RemoveUnexistingItems()
        If Path <> "" And Path.IndexOf("music") <> 0 Then
            If IO.Directory.Exists(Path) Then
                Dim NewNImages = 0
                For K As Long = 1 To NImages
                    If (Image(K).Type <> FileTypes.Folder And Not IO.File.Exists(Image(K).FileName)) Or (Image(K).Type = FileTypes.Folder And Not IO.Directory.Exists(Image(K).FileName)) Then
                        Image(K).Loaded = False
                        If Not (Thumbnail(K) Is Nothing) Then Thumbnail(K).Dispose()
                    Else
                        NewNImages = NewNImages + 1
                        SwapImages(K, NewNImages)
                    End If
                Next
                If (NewNImages <> NImages) Then
                    If NImages - NewNImages > 2 Then FlyingAlgorithm = FlyingAlgorithms.Simple Else FlyingAlgorithm = FlyingAlgorithms.NotSimple
                    NImages = NewNImages
                    For k As Long = 1 To NImages
                        SetImageDestination(k)
                    Next
                End If
                IsAnimatedImages = True
            End If
        End If
    End Sub

#Region "   Awful   | never open this"
    ''ÃÕ≈ ŒŒŒ◊≈Õ‹ —“€€ƒÕŒ!
    'Dim Allow As Boolean = True
    'Dim AllowDC As Boolean = True
    'Private Sub tmrWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
    '    Allow = True
    '    tmrWait.Stop()
    'End Sub
    'Private Sub tmrWaitForDC_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWaitForDC.Tick
    '    AllowDC = True
    '    tmrWaitForDC.Stop()
    'End Sub
#End Region

    Public Sub ForceRefreshingTimer()
        tmrWait.Interval = 250
        RefresherTimerCounter = 20
        'tmrWait.Start()
    End Sub
    Dim RefresherTimerCounter = 0
    Private Sub tmrWait_Tick(sender As Object, e As EventArgs) Handles tmrWait.Tick
        RemoveUnexistingItems()
        If RefresherTimerCounter < 0 Then
            tmrWait.Interval = 1000
            RefresherTimerCounter = 0
        Else
            RefresherTimerCounter -= 1
        End If
    End Sub

    Private Sub tmrWaitForDC_Tick(sender As Object, e As EventArgs) Handles tmrWaitForDC.Tick

    End Sub
End Class

