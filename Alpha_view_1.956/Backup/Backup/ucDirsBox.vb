Public Class ucDirsBox
    Event DirChanged(ByVal Up As Boolean)
    Event SendFocusToTheBottom(ByRef Done As Boolean)
    Structure WireStruct
        Dim X As Short
        Dim Y As Short
        Dim dX As Short
        Dim dY As Short
        Dim LinesInBox As Short
        Dim ColumnsInBox As Short
    End Structure
    Public Structure dirs_class
        Public X As Long
        Public Y As Long

        Public Name1 As String
        Public Width1 As Long

        Public Name2 As String
        Public Width2 As String

        Public Path As String
        Public Type As String

        Public ShowSize As Boolean
        Public Size As Long
        Public FreeSize As Long

        Dim Selected As Boolean

        Public InTagsIndex As Long
    End Structure

    Public Path As String
    Dim Cols As Short, Plus As Short

    Dim Dir(1000) As dirs_class
    Public NDirs As Long
    Public Wire As WireStruct
    Dim BGPos As Long
    Dim str As String

    Dim ChosenDir As Short, MMDir As Short

    Dim bmpFolders, bmpComposed As Bitmap
    Dim GraphicsComposed As Graphics
    Dim GraphicsFolders As Graphics

    Dim CanvasY As Double = 0, DestCanvasY As Double = 0, MaxCanvasY As Long, AnimateCanvas As Boolean

    Public Sub SetWire(ByVal x As Short, ByVal y As Short, ByVal dx As Short, ByVal dy As Short)
        Wire.X = x
        Wire.Y = y
        Wire.dX = dx
        Wire.dY = dy
        Wire.LinesInBox = Math.Truncate(Me.Height / (y + dy))
        Wire.ColumnsInBox = Math.Truncate((picMain.Width - Wire.dX) / (Wire.X + Wire.dX))

        Cols = Math.Truncate((picMain.Width - Wire.dX) / (Wire.X + Wire.dX))
        Plus = Math.Truncate((Me.Width - ((Wire.X + Wire.dX) * Cols + Wire.dX)) / 2)
    End Sub

    Dim BmpFolder, BmpDrive, BmpAlboom, BmpSelection, BmpScrollThing, BmpScrollThingMD, BmpScrollUp, BmpScrollDown, bmpBG, BmpDesktop As Bitmap
    Public Sub Init()
        Try
            BmpFolder = New Bitmap(Application.StartupPath + "\folder5.png")
            BmpDesktop = New Bitmap(Application.StartupPath + "\desktop.bmp")
            BmpDrive = New Bitmap(Application.StartupPath + "\drive.png")
            BmpAlboom = New Bitmap(Application.StartupPath + "\alboom.png")
            BmpSelection = New Bitmap(Application.StartupPath + "\sel.bmp")
            BmpScrollThing = New Bitmap(Application.StartupPath + "\scroller\scroll_thing.bmp")
            BmpScrollUp = New Bitmap(Application.StartupPath + "\scroller\scroll_up.bmp")
            BmpScrollDown = New Bitmap(Application.StartupPath + "\scroller\scroll_down.bmp")
            BmpScrollThingMD = New Bitmap(Application.StartupPath + "\scroller\scroll_thing_md.bmp")
            bmpBG = New Bitmap(Application.StartupPath + "\bgDirsBox.bmp")
        Catch ex As Exception
            BmpFolder = New Bitmap(1, 1)
            BmpSelection = New Bitmap(1, 1)
            BmpScrollThing = New Bitmap(1, 1)
            BmpScrollUp = New Bitmap(1, 1)
            BmpScrollDown = New Bitmap(1, 1)
            BmpScrollThingMD = New Bitmap(1, 1)
            bmpBG = New Bitmap(1, 1)
        End Try
    End Sub

    Dim text_font As New Font("verdana", 7, FontStyle.Regular)
    Public Sub NewPath(ByVal NewPath As String, ByVal s As String)
        If NewPath <> "home" Then
            Dim Dirs() As String
            Try
                DirTags.Tags(DirTags.FindByName(NewPath)).LaunchingTimes += 1
                DirTags.Save()
                'MsgBox(DirTags.FindByName(NewPath))

                Dirs = IO.Directory.GetDirectories(NewPath)
                Path = NewPath
                NDirs = Dirs.Length

                ChosenDir = 0
                Dim i As Long, name As String
                For i = 1 To NDirs
                    Dir(i).Path = Dirs(i - 1)
                Next
                Dim j As Long
                For j = 1 To NDirs
                    For i = 1 To NDirs - 1
                        If LCase(Dir(i).Path) > LCase(Dir(i + 1).Path) Then
                            Dim st As String
                            st = Dir(i).Path
                            Dir(i).Path = Dir(i + 1).Path
                            Dir(i + 1).Path = st
                        End If
                    Next
                Next
                For i = 1 To NDirs
                    Dir(i).InTagsIndex = DirTags.FindByName(Dir(i).Path)
                    If Dir(i).InTagsIndex = 0 Then
                        Dir(i).InTagsIndex = DirTags.Add(Dir(i).Path)
                    End If

                    name = GetFileName(Dir(i).Path)
                    Dir(i).Name1 = Trim(GetShortName(name))
                    Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                    Dir(i).Name2 = GetShortName(Trim(Mid(name, Dir(i).Name1.Length + 1)))
                    Dir(i).Width2 = GraphicsFolders.MeasureString(Dir(i).Name2, text_font).Width

                    'MsgBox(DirTags.Tags(Dir(i).InTagsIndex).Type)
                    Dir(i).Type = DirTags.Tags(Dir(i).InTagsIndex).Type

                    Dir(i).ShowSize = False
                    Dir(i).Selected = False
                    If LCase(Dir(i).Path + "\") = LCase(s) Then ChosenDir = i
                Next
                If ChosenDir = 0 Then
                    Dim maxI As Long = 1, max As Long = DirTags.Tags(Dir(1).InTagsIndex).LaunchingTimes
                    For i = 2 To NDirs
                        If DirTags.Tags(Dir(i).InTagsIndex).LaunchingTimes > max Then
                            max = DirTags.Tags(Dir(i).InTagsIndex).LaunchingTimes
                            maxI = i
                        End If
                    Next
                    ChosenDir = maxI
                End If
            Catch ex As Exception
            End Try
        Else
            Path = NewPath
            Dim Drives() As IO.DriveInfo = IO.DriveInfo.GetDrives()
            Dim Info As IO.DriveInfo, ent As String = Chr(13) + Chr(10)
            Dim dr(Drives.Length) As String, i As Long = 0

            ChosenDir = 1
            If IO.Directory.Exists("D:\_UNIVER\") Then
                i += 1
                Dir(i).Path = "D:\_UNIVER\"
                Dir(i).Name1 = "univer"
                Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                Dir(i).Name2 = ""
                Dir(i).Width2 = 0
                Dir(i).ShowSize = False
                Dir(i).Selected = False
            End If
            If IO.Directory.Exists("I:\[ music ]\") Then
                i += 1
                Dir(i).Path = "I:\[ music ]\"
            End If
            If IO.Directory.Exists("J:\[ music ]\") Then
                i += 1
                Dir(i).Path = "J:\[ music ]\"
            End If
            If IO.Directory.Exists("K:\[ music ]\") Then
                i += 1
                Dir(i).Path = "K:\[ music ]\"
            End If
            If IO.Directory.Exists("G:\[ music ]\") Then
                i += 1
                Dir(i).Path = "G:\[ music ]\"
            End If
            If Mid(Dir(i).Path, 4, 9) = "[ music ]" Then
                Dir(i).Name1 = "music"
                Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                Dir(i).Name2 = ""
                Dir(i).Width2 = 0
                Dir(i).ShowSize = False
                Dir(i).Selected = False
            End If
            If IO.Directory.Exists("C:\Users\" + System.Environment.UserName + "\Desktop\") Then
                i += 1
                Dir(i).Path = "C:\Users\" + System.Environment.UserName + "\Desktop\"
                Dir(i).Name1 = "desktop"
                Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                Dir(i).Name2 = ""
                Dir(i).Width2 = 0
                Dir(i).ShowSize = False
                Dir(i).Selected = False
                Dir(i).Type = "desktop"
            End If

            For Each Info In Drives
                If Info.RootDirectory.ToString <> "A:\" Then
                    If Info.IsReady Then
                        i += 1
                        Dir(i).Path = Info.RootDirectory.ToString
                        Dir(i).Type = Info.DriveType

                        Dir(i).Name1 = Info.Name
                        Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                        Dir(i).Name2 = Trim(GetShortName(Info.VolumeLabel))
                        Dir(i).Width2 = GraphicsFolders.MeasureString(Dir(i).Name2, text_font).Width

                        Dir(i).ShowSize = True
                        Dir(i).Size = Info.TotalSize
                        Dir(i).FreeSize = Info.TotalFreeSpace

                        Dir(i).Selected = False
                    End If
                End If
                If LCase(Dir(i).Path) = LCase(s) Then ChosenDir = i
            Next
            NDirs = i
            Array.Clear(Drives, 0, Drives.Length)
        End If
        RefreshDirsPlace()
        If ChosenDir >= 1 And ChosenDir <= Wire.ColumnsInBox Then
            CanvasY = 0
            DestCanvasY = 0
        Else
            If MaxCanvasY <> 0 Then
                CanvasY = -Dir(ChosenDir).Y + (Wire.Y + Wire.dY) + Wire.dY
                DestCanvasY = CanvasY
                CorrectCanvas()
            End If
        End If
        RedrawFolders()
        RedrawComposed()
    End Sub
    Public Sub FolderUp()
        Dim str As String = Path
        If Path <> "home" Then
            If Len(Path) > 3 Then
                Path = Mid(Path, 1, Path.Length - 1)
                Path = Mid(Path, 1, Path.Length - GetFileName(Path).Length)
                NewPath(Path, str)
            Else
                NewPath("home", str)
            End If
            RaiseEvent DirChanged(True)
        End If
    End Sub

    Public Sub RedrawFolder(ByVal i As Long, ByVal g As Graphics)
        Dim x, y As Long
        If i > 0 Then
            g.FillRectangle(Brushes.White, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
            If Dir(i).Selected Then
                Dim SelectedFColor As New Pen(Color.FromArgb(90, 0, 0, 0))
                Dim SelectedBGColor As New SolidBrush(Color.FromArgb(220, 255, 255, 255))

                g.DrawImage(BmpSelection, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                g.FillRectangle(SelectedBGColor, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                g.DrawRectangle(SelectedFColor, Dir(i).X, Dir(i).Y, Wire.X - 1, Wire.Y - 1)
            End If
            If i = ChosenDir And Me.Focused Then
                Dim ChosenFColor As New Pen(Color.FromArgb(110, 110, 220))
                Dim ChosenBGColor As New SolidBrush(Color.FromArgb(100, 180, 180, 255))

                g.FillRectangle(ChosenBGColor, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                g.DrawRectangle(ChosenFColor, Dir(i).X, Dir(i).Y, Wire.X - 1, Wire.Y - 1)
            End If
            If Dir(i).ShowSize Then
                Dim b As Bitmap
                Dim attrs As New System.Drawing.Imaging.ImageAttributes()
                attrs.SetColorKey(Color.Magenta, Color.Magenta)

                x = Dir(i).X + Math.Truncate((Wire.X - BmpFolder.Width) / 2)
                y = Dir(i).Y + Math.Truncate(((Wire.Y - 36) - BmpFolder.Height) / 2)
                g.DrawImage(BmpDrive, x, y + 1)

                x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                g.DrawString(Dir(i).Name1, text_font, Brushes.Black, x, Dir(i).Y + Wire.Y - 36)
                x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width2) / 2)
                g.DrawString(Dir(i).Name2, text_font, Brushes.Black, x, Dir(i).Y + Wire.Y - 25)

                b = GenerateMiniStatusBar(Wire.X - 6, 1 - Dir(i).FreeSize / Dir(i).Size)
                g.DrawImage(b, ToRect(Dir(i).X + 3, Dir(i).Y + Wire.Y - 11, b.Width, b.Height), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, attrs)
            Else
                If Dir(i).Name2 = "" Then
                    x = Dir(i).X + Math.Truncate((Wire.X - BmpFolder.Width) / 2)
                    y = Dir(i).Y + Math.Truncate(((Wire.Y - 25) - BmpFolder.Height) / 2) + 2
                    Select Case Dir(i).Type
                        Case "desktop"
                            g.DrawImage(BmpDesktop, x, y)
                        Case "alboom"
                            g.DrawImage(BmpAlboom, x, y)
                        Case Else
                            g.DrawImage(BmpFolder, x, y)
                    End Select
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                    g.DrawString(Dir(i).Name1, text_font, Brushes.Black, x, Dir(i).Y + Wire.Y - 20)
                Else
                    x = Dir(i).X + Math.Truncate((Wire.X - BmpFolder.Width) / 2)
                    y = Dir(i).Y + Math.Truncate(((Wire.Y - 25) - BmpFolder.Height) / 2) + 2
                    Select Case Dir(i).Type
                        Case "desktop"
                            g.DrawImage(BmpDesktop, x, y)
                        Case "alboom"
                            g.DrawImage(BmpAlboom, x, y)
                        Case Else
                            g.DrawImage(BmpFolder, x, y)
                    End Select
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                    g.DrawString(Dir(i).Name1, text_font, Brushes.Black, x, Dir(i).Y + Wire.Y - 25)
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width2) / 2)
                    g.DrawString(Dir(i).Name2, text_font, Brushes.Black, x, Dir(i).Y + Wire.Y - 14)
                End If
            End If
        End If
    End Sub

    Public Sub RedrawFolders()
        If picMain.Height > 0 Then
            Dim bmpFoldersHeight As Short = Dir(NDirs).Y + Wire.Y + Wire.dY
            If bmpFoldersHeight < Me.Height Then bmpFoldersHeight = Me.Height
            bmpFolders = New Bitmap(Me.Width, bmpFoldersHeight, Imaging.PixelFormat.Format32bppArgb)
            GraphicsFolders = Graphics.FromImage(bmpFolders)
            GraphicsFolders.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            GraphicsFolders.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            GraphicsFolders.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            For i As Long = 1 To NDirs
                RedrawFolder(i, GraphicsFolders)
            Next
        End If
    End Sub
    Public Sub RedrawComposed()
        If picMain.Height > 0 Then
            Dim i As Long
            Dim W As Long = picMain.Width, H As Long = picMain.Height

            Dim SelectedFColor As New Pen(Color.FromArgb(90, 0, 0, 0))
            Dim SelectedBGColor As New SolidBrush(Color.FromArgb(220, 255, 255, 255)) 'SolidBrush(Color.FromArgb(120, 245, 245, 245))
            Dim ChosenFColor As New Pen(Color.FromArgb(110, 110, 220))
            Dim ChosenBGColor As New SolidBrush(Color.FromArgb(100, 180, 180, 255))

            Dim d As Short = CanvasY
            GraphicsComposed.Clear(Color.FromArgb(255, 255, 255))
            'For i = 1 To NDirs
            '    If Dir(i).Selected Then
            '        GraphicsComposed.DrawImage(BmpSelection, Dir(i).X, Dir(i).Y + d, Wire.X, Wire.Y)
            '        GraphicsComposed.FillRectangle(SelectedBGColor, Dir(i).X, Dir(i).Y + d, Wire.X, Wire.Y)
            '        GraphicsComposed.DrawRectangle(SelectedFColor, Dir(i).X, Dir(i).Y + d, Wire.X, Wire.Y)
            '    End If
            '    If i = ChosenDir And (Me.Focused) Then
            '        GraphicsComposed.FillRectangle(ChosenBGColor, Dir(i).X, Dir(i).Y + d, Wire.X, Wire.Y)
            '        GraphicsComposed.DrawRectangle(ChosenFColor, Dir(i).X, Dir(i).Y + d, Wire.X, Wire.Y)
            '    End If
            'Next

            GraphicsComposed.DrawImageUnscaled(bmpFolders, 0, CanvasY)

            If MaxCanvasY <> 0 And Me.Height > Wire.Y + Wire.dY * 2 Then
                Dim ScrollThingTop As Long = (CanvasY / MaxCanvasY) * (Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2)
                ScrollThingTop += 3 + BmpScrollUp.Height
                Dim ScrollLeft As Long = Me.Width - BmpScrollThing.Width - 2
                If ScrollingBar Then
                    GraphicsComposed.DrawImageUnscaled(BmpScrollThingMD, ScrollLeft, ScrollThingTop)
                Else
                    GraphicsComposed.DrawImageUnscaled(BmpScrollThing, ScrollLeft, ScrollThingTop)
                End If
                GraphicsComposed.DrawImageUnscaled(BmpScrollUp, ScrollLeft, 2)
                GraphicsComposed.DrawImageUnscaled(BmpScrollDown, ScrollLeft, Me.Height - 2 - BmpScrollDown.Height)
            End If

            DrawFrame(GraphicsComposed, bmpComposed)

            If H < 35 Then
                Dim a2 As New SolidBrush(Color.AliceBlue)
                If H < 10 Then a2.Color = Color.FromArgb(255, 200, 200, 200) Else a2.Color = Color.FromArgb(350 - H * 10, 200, 200, 200)
                GraphicsComposed.FillRectangle(a2, 0, 0, W, H)
            End If

            picMain.Image = bmpComposed
        End If
    End Sub
    Public Sub Redraw()
        RedrawFolders()
        'On Error Resume Next
        RedrawComposed()
    End Sub

    Private Sub ChDir(ByVal i As Short)
        If i > 0 And i <= NDirs Then
            Path = Dir(i).Path
            If Mid(Path, Len(Path), 1) <> "\" Then
                Path += "\"
            End If
            NewPath(Path, "")
            'Redraw()
            RaiseEvent DirChanged(False)
        End If
    End Sub

#Region "             MouseEvents"
    Private Function XYToIndex(ByVal x As Short, ByVal y As Short) As Short
        Dim i As Short
        x = x - Plus - Wire.dX : y = y - Wire.dY - CanvasY
        i = (Cols * Math.Truncate(y / (Wire.Y + Wire.dY)) + Math.Truncate(x / (Wire.X + Wire.dX)) + 1)
        If i > NDirs Or (Math.Truncate(x / (Wire.X + Wire.dX)) + 1) > Cols Then i = 0
        If x Mod (Wire.X + Wire.dX) - Wire.X > 0 Or y Mod (Wire.Y + Wire.dY) - Wire.Y > 0 Then i = 0
        Return i
    End Function

    Dim ScrollingCanvas, ScrollingBar As Boolean, ScrollingStart, ScrollingPrev As Short
    Dim IsClick As Boolean

    Private Sub picMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDown
        Me.Focus()
        IsClick = True
        Dim PrevCd As Short = ChosenDir : ChosenDir = XYToIndex(e.X, e.Y)
        If MaxCanvasY < 0 Then
            If e.X > Me.Width - BmpScrollThing.Width - 3 Then
                If e.Y > Me.Height - BmpScrollDown.Height - 3 Then
                    DestCanvasY -= (Wire.Y + Wire.dY) : CorrectCanvas() : tmrScrolling.Enabled = True
                Else
                    If e.Y < BmpScrollUp.Height + 3 Then
                        DestCanvasY += (Wire.Y + Wire.dY) : CorrectCanvas() : tmrScrolling.Enabled = True
                    Else
                        Dim p As Long = BmpScrollUp.Height + 3 + (CanvasY / MaxCanvasY) * (Me.Height - BmpScrollThing.Height - BmpScrollUp.Height * 2 - 6)
                        If e.Y > p And e.Y < p + BmpScrollThing.Height Then
                            ScrollingCanvas = False : ScrollingBar = True
                        Else
                            ScrollingCanvas = True : ScrollingBar = False
                        End If
                        ScrollingPrev = e.Y : ScrollingStart = e.Y : tmrScrolling.Enabled = False
                    End If
                End If
            Else
                ScrollingCanvas = True : ScrollingBar = False
                ScrollingPrev = e.Y : ScrollingStart = e.Y : tmrScrolling.Enabled = False
            End If
        End If

        If PrevCd <> ChosenDir Then
            RedrawFolder(PrevCd, GraphicsFolders)
            RedrawFolder(ChosenDir, GraphicsFolders)
            'RedrawComposed()
        End If
        RedrawComposed()
    End Sub

    Public Sub CorrectCanvas()
        If DestCanvasY > 0 Then DestCanvasY = 0
        If DestCanvasY < MaxCanvasY Then DestCanvasY = MaxCanvasY
        If CanvasY > 0 Then CanvasY = 0
        If CanvasY < MaxCanvasY Then CanvasY = MaxCanvasY
    End Sub
    Private Sub picMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseMove
        Dim i As Short = XYToIndex(e.X, e.Y)
        If IsClick = True Then If Math.Abs(e.Y - ScrollingStart) > 3 Then IsClick = False
        If ScrollingCanvas Then
            DestCanvasY += e.Y - ScrollingPrev
            CanvasY = DestCanvasY
            ScrollingPrev = e.Y
            CorrectCanvas()
            RedrawComposed()
        Else
            If ScrollingBar Then
                DestCanvasY += ((e.Y - ScrollingPrev)) / (Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2) * MaxCanvasY
                CanvasY = DestCanvasY
                ScrollingPrev = e.Y
                CorrectCanvas()
                RedrawComposed()
            Else
                'If MMDir <> i Then
                '    If i > 0 Then
                '        Dim SelectedFColor As New Pen(Color.FromArgb(50, 0, 0, 0))
                '        If MMDir <> 0 Then GraphicsComposed.DrawRectangle(Pens.White, Dir(MMDir).X, Dir(MMDir).Y, Wire.X, Wire.Y)
                '        GraphicsComposed.DrawRectangle(SelectedFColor, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                '        picMain.Image = bmpComposed
                '    Else
                '        RedrawComposed()
                '    End If
                '    MMDir = i
                'End If
            End If
        End If
    End Sub
    Private Sub picMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseUp
        ScrollingCanvas = False : ScrollingBar = False : RedrawComposed()

        'Dim i As Short = XYToIndex(e.X, e.Y)
        If ChosenDir > 0 And ChosenDir <= NDirs And IsClick Then
            If e.Button = Windows.Forms.MouseButtons.Middle Then
                If IsClick Then
                    Dim f As New frmMain
                    Dim p As String = Dir(ChosenDir).Path
                    If Mid(p, p.Length, 1) <> "\" Then
                        p += "\"
                    End If
                    f.StartupPath = p
                    f.Show()
                    f.Left = Me.Parent.Left + 10
                    f.Top = Me.Parent.Top + 10
                End If
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                ChDir(ChosenDir)
            End If
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Dir(ChosenDir).Selected = Not (Dir(ChosenDir).Selected)
                RedrawFolder(ChosenDir, GraphicsFolders)
                RedrawComposed()
            End If
        Else
            'bla bla
        End If
    End Sub

    Private Sub picMain_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMain.MouseLeave
        RedrawComposed()
    End Sub
#End Region

    Private Function GetShortName(ByVal name As String) As String
        While GraphicsFolders.MeasureString(name, text_font).Width > Wire.X
            name = Mid(name, 1, Len(name) - 1)
        End While
        Return name
    End Function

    Private Sub RefreshDirsPlace()
        For i As Long = 1 To NDirs
            Dir(i).X = (((i - 1) Mod Cols)) * (Wire.X + Wire.dX) + Wire.dX + Plus
            Dir(i).Y = Math.Truncate((i - 1) / Cols) * (Wire.Y + Wire.dY) + Wire.dY
        Next
        MaxCanvasY = -Dir(NDirs).Y + Me.Height - (Wire.Y + Wire.dY)
        If MaxCanvasY > 0 Then MaxCanvasY = 0
    End Sub

    Private Sub DirsBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        Redraw()
    End Sub
    Private Sub DirsBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Redraw()
    End Sub

    Private Sub DirsBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim PrevCD As Short = ChosenDir
        Select Case e.KeyCode
            Case Keys.NumPad6
                ChosenDir = ChosenDir + 1 ' : Redraw()
            Case Keys.NumPad4
                ChosenDir = ChosenDir - 1 ': Redraw()
            Case Keys.NumPad8
                ChosenDir = ChosenDir - Cols ': Redraw()
            Case Keys.NumPad2
                ChosenDir = ChosenDir + Cols ': Redraw()
            Case Keys.NumPad5
                ChDir(ChosenDir) ': Redraw()
            Case Keys.Enter
                ChDir(ChosenDir) ': Redraw()
            Case Keys.S
                Dir(ChosenDir).Selected = Not (Dir(ChosenDir).Selected)
                'Redraw()
            Case Keys.Back
                FolderUp()
            Case Keys.A
                DirTags.Tags(Dir(ChosenDir).InTagsIndex).Type = "alboom"
                'Case e.Shift
                '    ChDir(ChosenDir) : Redraw()
        End Select
        'If PrevCD <> ChosenDir Then
        '    RedrawFolder(PrevCD, GraphicsFolders)
        '    RedrawFolder(ChosenDir, GraphicsFolders)
        '    RedrawComposed()
        'End If
    End Sub

    Private Sub DirsBox_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        Dim PrevCD As Short = ChosenDir
        Select Case e.KeyCode
            Case Keys.Right
                ChosenDir = ChosenDir + 1
                e.IsInputKey = True
            Case Keys.Left
                ChosenDir = ChosenDir - 1
                e.IsInputKey = True
            Case Keys.Up
                ChosenDir = ChosenDir - Cols
                e.IsInputKey = True
            Case Keys.Down
                If ChosenDir > NDirs - Cols Then
                    Dim a As Boolean = True
                    RaiseEvent SendFocusToTheBottom(a)
                    If a = False Then ChosenDir = ChosenDir + Cols
                Else
                    ChosenDir = ChosenDir + Cols
                End If
                e.IsInputKey = True
        End Select
        If ChosenDir < 1 Then ChosenDir = 1
        If ChosenDir > NDirs Then ChosenDir = NDirs
        'If ChosenDir <> prev_dir Then RedrawComposed()
        If PrevCD <> ChosenDir Then
            RedrawFolder(PrevCD, GraphicsFolders)
            RedrawFolder(ChosenDir, GraphicsFolders)
            RedrawComposed()
        End If

        Dim h1 As Long = (Wire.Y + Wire.dY)
        If Dir(ChosenDir).Y + h1 * 2 + DestCanvasY > Me.Height Then DestCanvasY -= h1
        If Dir(ChosenDir).Y - h1 + DestCanvasY < 0 Then DestCanvasY += h1

        'If DestCanvasY - h1 * (Lines - 1) > -Dir(ChosenDir).Y Then DestCanvasY -= h1 * Lines ': tmrScrolling.Enabled = True
        'If DestCanvasY < -Dir(ChosenDir).Y + Wire.dY Then DestCanvasY += h1 * Lines ': tmrScrolling.Enabled = True
        If DestCanvasY > 0 Then DestCanvasY = 0
        If DestCanvasY < MaxCanvasY Then DestCanvasY = MaxCanvasY

        If tmrScrolling.Enabled = False Then tmrScrolling.Enabled = True
        'MsgBox(Dir(ChosenDir).Y)
    End Sub


    Private Sub DirsBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        picMain.Width = Me.Width
        picMain.Height = Me.Height
        If (Wire.X + Wire.dX) <> 0 Then
            Cols = Math.Truncate((picMain.Width - Wire.dX) / (Wire.X + Wire.dX))
            Plus = Math.Truncate((Me.Width - ((Wire.X + Wire.dX) * Cols + Wire.dX)) / 2)
        End If

        If picMain.Height > 0 Then
            bmpComposed = New Bitmap(Me.Width, Me.Height, Imaging.PixelFormat.Format32bppRgb)
            GraphicsComposed = Graphics.FromImage(bmpComposed)
            GraphicsComposed.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            GraphicsComposed.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            bmpFolders = New Bitmap(Me.Width, Dir(NDirs).Y + 10, Imaging.PixelFormat.Format32bppArgb)
            GraphicsFolders = Graphics.FromImage(bmpFolders)
            GraphicsFolders.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            GraphicsFolders.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            GraphicsFolders.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

            RefreshDirsPlace()
            Redraw()
        End If
    End Sub

    Public Sub SetCanvasY()
        If ChosenDir >= 1 And ChosenDir <= Wire.ColumnsInBox Then
            CanvasY = 0
            DestCanvasY = 0
        Else
            If MaxCanvasY <> 0 Then
                CanvasY = -Dir(ChosenDir).Y - (Wire.Y + Wire.dY)
                DestCanvasY = CanvasY
                CorrectCanvas()
            End If
        End If
        Redraw()
    End Sub

    Private Sub DirsBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        picMain.Left = 0
        picMain.Top = 0
        picMain.Width = Me.Width
        picMain.Height = Me.Height
    End Sub

    Private Sub tmrScrolling_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrScrolling.Tick
        CanvasY += (DestCanvasY - CanvasY) / 5
        Dim d As Short = Math.Round(CanvasY)
        If d <> DestCanvasY Then RedrawComposed() Else CanvasY = DestCanvasY : RedrawComposed() : tmrScrolling.Enabled = False
    End Sub


    'DRAG & GROP SAMPLE
    'Private Sub Label1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    Dim str(0) As String
    '    str(0) = "hello world"
    '    Dim a As New DataObject(DataFormats.FileDrop, str)
    '    Label1.DoDragDrop(a, DragDropEffects.Copy)
    'End Sub
    'Private Sub Label2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
    '    Dim files() As String = e.Data.GetData(DataFormats.FileDrop, True)
    '    Dim t As String
    '    For Each t In files
    '        MsgBox(t)
    '    Next
    'End Sub
    'Private Sub Label2_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
    '    e.Effect = DragDropEffects.All
    'End Sub

    Private Sub ucDirsBox_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        DestCanvasY += e.Delta
        If DestCanvasY > 0 Then DestCanvasY = 0
        If DestCanvasY < MaxCanvasY Then DestCanvasY = MaxCanvasY : Dim f As Boolean : RaiseEvent SendFocusToTheBottom(f)
        CanvasY = DestCanvasY
        If tmrScrolling.Enabled = False Then tmrScrolling.Enabled = True

        If ChosenDir > 0 Then
            If Dir(ChosenDir).Y + CanvasY < 0 Then
                ChosenDir = Wire.ColumnsInBox * (Math.Truncate(-CanvasY / (Wire.Y + Wire.dY)) + 1) + 1
            End If
            If Dir(ChosenDir).Y + CanvasY > Me.Height - Wire.Y - Wire.dY Then
                ChosenDir = Wire.ColumnsInBox * (Math.Truncate(-CanvasY / (Wire.Y + Wire.dY)) - 1 + Wire.LinesInBox) + 1
            End If
        End If
    End Sub

    Private Sub picMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picMain.Click

    End Sub
End Class
