Public Class ucDirsBox
    Event DirChanged(ByVal Up As Boolean)
    Event SendFocusToTheBottom(ByRef Done As Boolean)
    Event BackPressed()

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
    Dim Cols, Plus As Short

    Public Dir(1000) As dirs_class
    Public NDirs As Long

    Public Wire As WireStruct

    Dim ChosenDir, MMDir As Short

    Dim bmpFolders, bmpComposed As Bitmap
    Dim GraphicsComposed As Graphics
    Dim GraphicsFolders As Graphics

    Dim CanvasY As Double = 0, DestCanvasY As Double = 0, MaxCanvasY As Long, AnimateCanvas As Boolean

    Dim BGBrush As New SolidBrush(Color.FromArgb(245, 245, 245)) 'SolidBrush(Color.White)
    Dim BGUnderElementBrush As New SolidBrush(Color.FromArgb(245, 245, 245)) 'SolidBrush(Color.FromArgb(252, 252, 252))

    Public Sub SetWire(ByVal x As Short, ByVal y As Short, ByVal dx As Short, ByVal dy As Short)
        Wire.X = x
        Wire.Y = y
        Wire.dX = dx
        Wire.dY = dy
        RecalculateWireParameters()
    End Sub
    Private Sub RecalculateWireParameters()
        Wire.LinesInBox = Math.Truncate(Me.Height / (Wire.Y + Wire.dY))
        Wire.ColumnsInBox = Math.Truncate((picMain.Width - Wire.dX) / (Wire.X + Wire.dX))

        Cols = Math.Truncate((picMain.Width - Wire.dX) / (Wire.X + Wire.dX))
        Plus = Math.Truncate((Me.Width - ((Wire.X + Wire.dX) * Cols + Wire.dX)) / 2)
    End Sub

    Dim btnDeletePos As Point = New Point(-100, -100)

    Dim BmpFolder, BmpLoading, BmpDrive, BmpUsb, BmpSongs, BmpFilms, BmpMkDir, BmpAlboom, BmpDesktop As Bitmap
    Dim BmpFolder16, BmpDrive16, BmpSongs16, BmpFilms16, BmpMkDir16, BmpAlboom16, BmpDesktop16 As Bitmap

    Dim BmpDelete, BmpSelection, BmpFocus, BmpFocusGray, BmpFocusWhite, BmpScrollThing, BmpScrollThingMD, BmpScrollUp, BmpScrollDown, bmpBG As Bitmap
    Dim TilesLoaded As Boolean = False
    Public Sub Init()
        Try
            BmpFolder = New Bitmap(Application.StartupPath + "\dir2_32.png")
            BmpSongs = New Bitmap(Application.StartupPath + "\muz5_32.png")
            BmpFilms = New Bitmap(Application.StartupPath + "\films4_32.png")
            BmpMkDir = New Bitmap(Application.StartupPath + "\mkdir2_32.png")
            BmpDesktop = New Bitmap(Application.StartupPath + "\desktop2_32.png")
            BmpDrive = New Bitmap(Application.StartupPath + "\drive3_32.png")
            BmpAlboom = New Bitmap(Application.StartupPath + "\alboom2_32.png")
            BmpUsb = New Bitmap(Application.StartupPath + "\usb_32.png")

            BmpFocus = New Bitmap(Application.StartupPath + "\selection15_32.png")
            BmpFocusGray = New Bitmap(Application.StartupPath + "\selection9_32.png")
            BmpFocusWhite = New Bitmap(Application.StartupPath + "\selection10_32.png")
            BmpSelection = New Bitmap(Application.StartupPath + "\selection14_32.png")
            BmpLoading = New Bitmap(Application.StartupPath + "\loading.png")

            BmpDelete = New Bitmap(Application.StartupPath + "\buttons\delete.png")

            BmpScrollThing = New Bitmap(Application.StartupPath + "\scroller\main_rounded.png")
            BmpScrollUp = New Bitmap(Application.StartupPath + "\scroller\up2.png")
            BmpScrollDown = New Bitmap(Application.StartupPath + "\scroller\down2.png")
            BmpScrollThingMD = New Bitmap(Application.StartupPath + "\scroller\main_md_rounded.png")

            TilesLoaded = True
            'bmpBG = New Bitmap(Application.StartupPath + "\bgDirsBox.bmp")
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

    'Правильный ответ 9 pt. black (#000000) Segoe UI. Стиль написания обычный.
    'Взято отсуда: http://msdn.microsoft.com/en-us/library/aa511282.aspx
    'Dim text_font As New Font("Verdana", 7, FontStyle.Regular)
    Dim text_font As New Font("Calibri", 11, FontStyle.Regular, GraphicsUnit.Pixel)
    'Dim text_font As New Font("Tahoma", 7 or 8, FontStyle.Regular)
    'Dim text_font As New Font("Lucida Sans Unicode", 10, FontStyle.Regular, GraphicsUnit.Pixel)
    'Dim text_font As New Font("Segoe WP", 9, FontStyle.Regular)'UI

    Public Sub NewPath(ByVal NewPath As String, ByVal s As String)
        If NewPath <> "home" And IO.Directory.Exists(NewPath) Then
            Dim Dirs() As String
            'Try
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

            NDirs += 1
            i = NDirs
            Dir(i).Name1 = "mkdir"
            Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
            Dir(i).Name2 = ""
            Dir(i).Width2 = GraphicsFolders.MeasureString(Dir(i).Name2, text_font).Width
            Dir(i).Type = "mkdir"
            Dir(i).ShowSize = False
            Dir(i).Selected = False
            'Catch ex As Exception
            'MsgBox(ex.ToString)
            'End Try
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
                Dir(i).Type = ""
            End If
            If IO.Directory.Exists("I:\[ music ]\") Then i += 1 : Dir(i).Path = "I:\[ music ]\"
            If IO.Directory.Exists("J:\[ music ]\") Then i += 1 : Dir(i).Path = "J:\[ music ]\"
            If IO.Directory.Exists("K:\[ music ]\") Then i += 1 : Dir(i).Path = "K:\[ music ]\"
            If IO.Directory.Exists("G:\[ music ]\") Then i += 1 : Dir(i).Path = "G:\[ music ]\"
            If IO.Directory.Exists("F:\[ music ]\") Then i += 1 : Dir(i).Path = "F:\[ music ]\"
            If Mid(Dir(i).Path, 4, 9) = "[ music ]" Then
                Dir(i).Name1 = "music"
                Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                Dir(i).Name2 = ""
                Dir(i).Width2 = 0
                Dir(i).ShowSize = False
                Dir(i).Selected = False
                Dir(i).Type = "songs"
            End If
            If IO.Directory.Exists("D:\Documents\") Then
                i += 1
                Dir(i).Path = "D:\Documents\"
                Dir(i).Name1 = "docs"
                Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                Dir(i).Name2 = "" : Dir(i).Width2 = 0 : Dir(i).ShowSize = False : Dir(i).Selected = False : Dir(i).Type = ""
            End If
            If IO.Directory.Exists("D:\Documents\_Univer") Then
                i += 1
                Dir(i).Path = "D:\Documents\_Univer"
                Dir(i).Name1 = "univer"
                Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
                Dir(i).Name2 = "" : Dir(i).Width2 = 0 : Dir(i).ShowSize = False : Dir(i).Selected = False : Dir(i).Type = ""
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
                        Dir(i).Type = Info.DriveType.ToString

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

            'For j As Long = 0 To 0
            '    Dim Max As Long = 0
            '    For k As Long = 1 To DirTags.N
            '        If DirTags.Tags(k).LaunchingTimes > DirTags.Tags(Max).LaunchingTimes Then Max = k
            '    Next
            '    If IO.Directory.Exists(DirTags.Dirs(Max)) Then
            '        i += 1
            '        Dir(i).Path = DirTags.Dirs(Max)
            '        Dir(i).Name1 = DirTags.Dirs(Max)
            '        Dir(i).Width1 = GraphicsFolders.MeasureString(Dir(i).Name1, text_font).Width
            '        Dir(i).Name2 = "" : Dir(i).Width2 = 0 : Dir(i).ShowSize = False : Dir(i).Selected = False : Dir(i).Type = ""
            '    End If
            'Next
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
    Private Sub DrawIco(ByVal i As Long, ByVal g As Graphics, ByVal x As Long, ByVal y As Long)
        Select Case Dir(i).Type
            Case "desktop"
                g.DrawImage(BmpDesktop, x, y)
            Case "alboom"
                g.DrawImage(BmpAlboom, x, y)
            Case "songs"
                g.DrawImage(BmpSongs, x, y)
            Case "films"
                g.DrawImage(BmpFilms, x, y)
            Case "mkdir"
                g.DrawImage(BmpMkDir, x, y)
            Case Else
                g.DrawImage(BmpFolder, x, y)
        End Select
    End Sub
    Public RoundedElements As Boolean = True
    Public DrawBorders As Boolean = True
    Public Sub RedrawFolder(ByVal i As Long, ByVal g As Graphics, Optional ByVal MouseDown As Boolean = False)
        Dim x, y As Long
        Dim TextColor As New SolidBrush(Color.FromArgb(0, 0, 0))
        If Me.BackColor.R < 127 Then TextColor.Color = Color.White Else TextColor.Color = Color.Black
        If i > 0 Then
            'g.FillRectangle(BGUnderElementBrush, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
            g.FillRectangle(New SolidBrush(Me.BackColor), Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
            If Dir(i).ShowSize Then
                g.FillRectangle(New SolidBrush(Me.BackColor), Dir(i).X, Dir(i).Y + Wire.Y, Wire.X, 20)
            End If
            If MMIndex = i And i <> ChosenDir Then
                If RoundedElements Then
                    g.DrawImageUnscaled(BmpFocusWhite, Dir(i).X, Dir(i).Y)
                    If Dir(i).ShowSize Then
                        g.DrawImageUnscaled(BmpFocusWhite, Dir(i).X, Dir(i).Y + 20)
                    End If
                Else
                    Dim bb As New SolidBrush(Color.FromArgb(255, 255, 255))
                    g.FillRectangle(bb, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                End If
                TextColor.Color = Color.FromArgb(0, 0, 0)
            End If
            If i = ChosenDir Then
                If Me.Focused Then
                    If RoundedElements Then
                        g.DrawImageUnscaled(BmpFocus, Dir(i).X, Dir(i).Y)
                        If Dir(i).ShowSize Then
                            g.DrawImageUnscaled(BmpFocus, Dir(i).X, Dir(i).Y + 20)
                        End If
                    Else
                        If MouseDown Then
                            Dim bb As New SolidBrush(Color.FromArgb(15, 100, 160))
                            g.FillRectangle(bb, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                        Else
                            Dim bb As New SolidBrush(Color.FromArgb(19, 130, 206))
                            g.FillRectangle(bb, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                        End If
                    End If
                    TextColor.Color = Color.FromArgb(255, 255, 255)
                Else
                    If RoundedElements Then
                        g.DrawImageUnscaled(BmpFocusGray, Dir(i).X, Dir(i).Y)
                        If Dir(i).ShowSize Then
                            g.DrawImageUnscaled(BmpFocusGray, Dir(i).X, Dir(i).Y + 20)
                        End If
                    Else
                        Dim bb As New SolidBrush(Color.FromArgb(208, 210, 211))
                        g.FillRectangle(bb, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
                    End If
                End If
            End If
            If Dir(i).Selected Then
                g.DrawImage(BmpSelection, Dir(i).X, Dir(i).Y, Wire.X, Wire.Y)
            End If
            If Dir(i).ShowSize Then
                Dim b As Bitmap
                Dim attrs As New System.Drawing.Imaging.ImageAttributes()
                attrs.SetColorKey(Color.Magenta, Color.Magenta)

                x = Dir(i).X + Math.Truncate((Wire.X - BmpFolder.Width) / 2)
                y = Dir(i).Y + Math.Truncate(((Wire.Y - 36 + 10) - BmpFolder.Height) / 2) + 1
                If Dir(i).Type = "Removable" Then
                    g.DrawImage(BmpUsb, x, y + 4)
                Else
                    g.DrawImage(BmpDrive, x, y + 1)
                End If

                If Dir(i).Name2 = "" Then
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                    g.DrawString(Dir(i).Name1, text_font, TextColor, x, Dir(i).Y + Wire.Y - 20)
                Else
                    y = Dir(i).Y + Wire.Y - 36 + 10
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                    g.DrawString(Dir(i).Name1, text_font, TextColor, x, y)
                    y = Dir(i).Y + Wire.Y - 25 + 10
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width2) / 2)
                    g.DrawString(Dir(i).Name2, text_font, TextColor, x, y)
                End If
                y = Dir(i).Y + Wire.Y - 11 + 16
                b = GenerateMiniStatusBar(Wire.X - 5, 1 - Dir(i).FreeSize / Dir(i).Size)
                g.DrawImage(b, ToRect(Dir(i).X + 3, y, b.Width, b.Height), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, attrs)
            Else
                If Dir(i).Name2 = "" Then
                    x = Dir(i).X + Math.Truncate((Wire.X - BmpFolder.Width) / 2)
                    y = Dir(i).Y + Math.Truncate(((Wire.Y - 25) - BmpFolder.Height) / 2) + 2
                    DrawIco(i, g, x, y)
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                    g.DrawString(Dir(i).Name1, text_font, TextColor, x, Dir(i).Y + Wire.Y - 20)
                Else
                    x = Dir(i).X + Math.Truncate((Wire.X - BmpFolder.Width) / 2)
                    y = Dir(i).Y + Math.Truncate(((Wire.Y - 25) - BmpFolder.Height) / 2) + 2
                    DrawIco(i, g, x, y)
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width1) / 2)
                    g.DrawString(Dir(i).Name1, text_font, TextColor, x, Dir(i).Y + Wire.Y - 25)
                    x = Dir(i).X + Math.Truncate((Wire.X - Dir(i).Width2) / 2)
                    g.DrawString(Dir(i).Name2, text_font, TextColor, x, Dir(i).Y + Wire.Y - 14)
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
            Dim W As Long = picMain.Width, H As Long = picMain.Height

            Dim SelectedFColor As New Pen(Color.FromArgb(90, 0, 0, 0))
            Dim SelectedBGColor As New SolidBrush(Color.FromArgb(220, 255, 255, 255)) 'SolidBrush(Color.FromArgb(120, 245, 245, 245))
            Dim ChosenFColor As New Pen(Color.FromArgb(110, 110, 220))
            Dim ChosenBGColor As New SolidBrush(Color.FromArgb(100, 180, 180, 255))

            Dim d As Short = CanvasY
            GraphicsComposed.Clear(Me.BackColor) 'BGBrush.Color)
            GraphicsComposed.DrawImageUnscaled(bmpFolders, 0, CanvasY)

            If TilesLoaded Then
                If MaxCanvasY <> 0 Then
                    Dim ScrollLeft As Long = Me.Width - BmpScrollThing.Width - 0 '1 '2
                    If Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2 > 0 Then
                        Dim ScrollThingTop As Long = (CanvasY / MaxCanvasY) * (Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2)
                        ScrollThingTop += BmpScrollUp.Height + 3

                        If ScrollingBar Then
                            GraphicsComposed.DrawImageUnscaled(BmpScrollThingMD, ScrollLeft, ScrollThingTop)
                        Else
                            GraphicsComposed.DrawImageUnscaled(BmpScrollThing, ScrollLeft, ScrollThingTop)
                        End If
                    End If
                    If Me.Height > 40 Then
                        GraphicsComposed.DrawImageUnscaled(BmpScrollUp, ScrollLeft, 0) '1) '2)
                        GraphicsComposed.DrawImageUnscaled(BmpScrollDown, ScrollLeft, Me.Height - BmpScrollDown.Height - 0) '1) '2)
                    End If
                   
                    'GraphicsComposed.FillRectangle()
                End If

                GraphicsComposed.DrawImageUnscaled(BmpDelete, btnDeletePos.X, btnDeletePos.Y)
            End If

            If DrawBorders Then DrawFrameMain(GraphicsComposed, bmpComposed)

            If H < 35 Then
                Dim a2 As New SolidBrush(Color.AliceBlue)
                If H < 10 Then a2.Color = Color.FromArgb(255, 200, 200, 200) Else a2.Color = Color.FromArgb(350 - H * 10, 200, 200, 200)
                GraphicsComposed.FillRectangle(a2, 0, 0, W, H)
            End If

            picMain.Image = bmpComposed
        End If
    End Sub

    Public Sub DrawFrameMain(ByRef Grafic As System.Drawing.Graphics, ByRef Bitm As Bitmap)
        If Bitm.Height > 3 Then
            Dim p1 As New Pen(Color.FromArgb(Me.BackColor.R * 0.3, Me.BackColor.R * 0.3, Me.BackColor.R * 0.3))
            Grafic.DrawLine(p1, 0, 0, Bitm.Width, 0)
            Grafic.DrawLine(p1, 0, Bitm.Height - 1, Bitm.Width, Bitm.Height - 1)

            Dim I As Integer = (200 * 0.6 + (Me.BackColor.R * 0.3) * 0.4)
            Dim Color1 As Color = Color.FromArgb(I, I, I)
            I = ((Me.BackColor.R * 0.3) * 0.8 + Me.BackColor.R * 0.2)
            Dim Color2 As Color = Color.FromArgb(I, I, I)

            'Bitm.SetPixel(1, 1, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(0, 1, Color2)
            Bitm.SetPixel(1, 0, Color2)
            Bitm.SetPixel(0, 0, Color1)
            'Bitm.SetPixel(Bitm.Width - 2, 1, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(Bitm.Width - 1, 1, Color2)
            Bitm.SetPixel(Bitm.Width - 2, 0, Color2)
            Bitm.SetPixel(Bitm.Width - 1, 0, Color1)

            '.SetPixel(Bitm.Width - 2, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 2, Color2)
            Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 1, Color2)
            Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 1, Color1) '.FromArgb(150, 150, 150))
            'Bitm.SetPixel(1, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(0, Bitm.Height - 2, Color2)
            Bitm.SetPixel(1, Bitm.Height - 1, Color2)
            Bitm.SetPixel(0, Bitm.Height - 1, Color1)
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

    Dim ScrollingCanvas, ScrollingBar As Boolean, ScrollingStart, ScrollingStartX, ScrollingPrev As Short
    Dim IsClick As Boolean

    Private Sub picMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseDown
        Me.Focus()
        IsClick = True : ScrollingStart = e.Y : ScrollingStartX = e.X
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
                        If e.Y > p And e.Y < p + BmpScrollThing.Height And Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2 > 0 Then
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
            'RedrawComposed()
        End If
        RedrawFolder(ChosenDir, GraphicsFolders, True)
        RedrawComposed()
    End Sub

    Public Sub CorrectCanvas()
        If DestCanvasY > 0 Then DestCanvasY = 0
        If DestCanvasY < MaxCanvasY Then DestCanvasY = MaxCanvasY
        If CanvasY > 0 Then CanvasY = 0
        If CanvasY < MaxCanvasY Then CanvasY = MaxCanvasY
    End Sub
    Dim MMIndex As Long
    Private Sub picMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            Dim NeedToRedrawComposed As Boolean = False
            Dim i As Short = XYToIndex(e.X, e.Y)
            If IsClick = True Then
                If Math.Abs(e.Y - ScrollingStart) > 3 Or Math.Abs(e.X - ScrollingStartX) > 3 Then
                    IsClick = False : RedrawFolder(ChosenDir, GraphicsFolders, False) : NeedToredrawComposed = True
                End If
            End If
            If ScrollingCanvas Then
                DestCanvasY += e.Y - ScrollingPrev
                CanvasY = DestCanvasY
                ScrollingPrev = e.Y
                CorrectCanvas()
                NeedToRedrawComposed = True
            Else
                If ScrollingBar Then
                    'If (Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2 = 0) Then
                    '    MsgBox("BAAANG")
                    'End If
                    DestCanvasY += ((e.Y - ScrollingPrev)) / (Me.Height - BmpScrollThing.Height - 4 - BmpScrollUp.Height * 2 - 2) * MaxCanvasY
                    CanvasY = DestCanvasY
                    ScrollingPrev = e.Y
                    CorrectCanvas()
                    NeedToRedrawComposed = True
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
            If NeedToredrawComposed Then RedrawComposed()
        Else
            Dim PrevMMIndex As Long = MMIndex
            MMIndex = XYToIndex(e.X, e.Y)

            RedrawFolder(PrevMMIndex, GraphicsFolders)
            RedrawFolder(MMIndex, GraphicsFolders)
            RedrawComposed()
        End If
    End Sub
    Private Sub picMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseUp
        RedrawFolder(ChosenDir, GraphicsFolders, False)
        ScrollingCanvas = False : ScrollingBar = False : RedrawComposed()

        'Dim i As Short = XYToIndex(e.X, e.Y)
        If (btnDeletePos.X <= e.X And btnDeletePos.X + 20 >= e.X And btnDeletePos.Y <= e.Y And btnDeletePos.Y + 20 >= e.Y) Then
            btnDeletePos.X = -100
            btnDeletePos.Y = -100
            Try
                Dim ans As Long = MsgBox("Are you sure, you want to delete this directory?" + vbNewLine + Dir(ChosenDir).Path, MsgBoxStyle.YesNo, "Delete directory")
                If ans = 6 Then My.Computer.FileSystem.DeleteDirectory(Dir(ChosenDir).Path, FileIO.DeleteDirectoryOption.DeleteAllContents)
                NewPath(Path, Path)
            Catch
                MsgBox("Delete failed")
            End Try
        Else
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
                    If Dir(ChosenDir).Type <> "mkdir" Then
                        GraphicsComposed.DrawImageUnscaled(BmpLoading, Dir(ChosenDir).X, Dir(ChosenDir).Y + CanvasY)
                        picMain.Image = bmpComposed
                        picMain.Refresh()
                        ChDir(ChosenDir)
                    Else
                        TextEntryMode = "mkdir"
                        SetAndClearTextBox()
                    End If
                End If
                If e.Button = Windows.Forms.MouseButtons.Right Then
                    Dir(ChosenDir).Selected = Not (Dir(ChosenDir).Selected)

                    btnDeletePos.X = Dir(ChosenDir).X + Wire.X - 20
                    btnDeletePos.Y = Dir(ChosenDir).Y

                    RedrawFolder(ChosenDir, GraphicsFolders)
                    RedrawComposed()
                End If
            End If
        End If
    End Sub

    Private Sub picMain_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMain.MouseLeave
        Dim PrevMMIndex As Long = MMIndex
        MMIndex = 0
        RedrawFolder(PrevMMIndex, GraphicsFolders)
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

    Private Sub SetAndClearTextBox()
        txtName.Left = Dir(ChosenDir).X
        txtName.Top = Dir(ChosenDir).Y + CanvasY + Wire.Y - txtName.Height - 5
        txtName.Text = ""
        txtName.Visible = True
        txtName.Select()
    End Sub

    Private Sub DirsBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        'Redraw()
    End Sub
    Private Sub DirsBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        'Redraw()
    End Sub
    Dim TextEntryMode As String
    Private Sub DirsBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim PrevCD As Short = ChosenDir
        Select Case e.KeyCode
            Case Keys.Right
                ChosenDir = ChosenDir + 1
            Case Keys.Left
                ChosenDir = ChosenDir - 1
            Case Keys.Up
                ChosenDir = ChosenDir - Cols
            Case Keys.Down
                If ChosenDir > NDirs - Cols Then
                    Dim a As Boolean = True
                    RaiseEvent SendFocusToTheBottom(a)
                    If a = False Then ChosenDir = ChosenDir + Cols
                Else
                    ChosenDir = ChosenDir + Cols
                End If
            Case Keys.Home
                ChosenDir = 1
            Case Keys.End
                ChosenDir = NDirs
            Case Keys.PageDown
                ChosenDir += Wire.LinesInBox * Wire.ColumnsInBox
            Case Keys.PageUp
                ChosenDir -= Wire.LinesInBox * Wire.ColumnsInBox
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
                If Dir(ChosenDir).Type <> "mkdir" Then
                    If e.Control Then
                        Dim f As New frmMain
                        Dim p As String = Dir(ChosenDir).Path
                        If Mid(p, p.Length, 1) <> "\" Then
                            p += "\"
                        End If
                        f.StartupPath = p
                        f.Show()
                        f.Left = Me.Parent.Left + 10
                        f.Top = Me.Parent.Top + 10
                    Else
                        GraphicsComposed.DrawImageUnscaled(BmpLoading, Dir(ChosenDir).X, Dir(ChosenDir).Y + CanvasY)
                        picMain.Image = bmpComposed
                        picMain.Refresh()
                        ChDir(ChosenDir)
                    End If
                Else
                    TextEntryMode = "mkdir"
                    SetAndClearTextBox()
                End If
            Case Keys.S
                Dir(ChosenDir).Selected = Not (Dir(ChosenDir).Selected)
                RedrawFolder(ChosenDir, GraphicsFolders)
                'Redraw()
            Case Keys.Back
                RaiseEvent BackPressed()
                FolderUp()
            Case Keys.A
                DirTags.Tags(Dir(ChosenDir).InTagsIndex).Type = "alboom"
                Dir(ChosenDir).Type = "alboom"
                RedrawFolder(ChosenDir, GraphicsFolders)
            Case Keys.M
                DirTags.Tags(Dir(ChosenDir).InTagsIndex).Type = "songs"
                Dir(ChosenDir).Type = "songs"
                RedrawFolder(ChosenDir, GraphicsFolders)
            Case Keys.F
                DirTags.Tags(Dir(ChosenDir).InTagsIndex).Type = "films"
                Dir(ChosenDir).Type = "films"
                RedrawFolder(ChosenDir, GraphicsFolders)
            Case Keys.D
                DirTags.Tags(Dir(ChosenDir).InTagsIndex).Type = ""
                Dir(ChosenDir).Type = "folder"
                RedrawFolder(ChosenDir, GraphicsFolders)
                'Case e.Shift
                '    ChDir(ChosenDir) : Redraw()
            Case Keys.Delete
                Try
                    Dim ans As Long = MsgBox("Are you sure, you want to delete this directory?" + vbNewLine + Dir(ChosenDir).Path, MsgBoxStyle.YesNo, "Delete directory")
                    If ans = 6 Then My.Computer.FileSystem.DeleteDirectory(Dir(ChosenDir).Path, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    NewPath(Path, Path)
                Catch
                    MsgBox("Delete failed")
                End Try
        End Select
        If ChosenDir < 1 Then ChosenDir = 1
        If ChosenDir > NDirs Then ChosenDir = NDirs
        If PrevCD <> ChosenDir Then
            RedrawFolder(PrevCD, GraphicsFolders)
            RedrawFolder(ChosenDir, GraphicsFolders)

            If MMIndex > 0 Then
                Dim PrevMM As Long = MMIndex
                MMIndex = 0
                RedrawFolder(PrevMM, GraphicsFolders)
            End If

            RedrawComposed()
        End If

        'MsgBox(Wire.LinesInBox)
        If Wire.LinesInBox > 2 Then
            Dim h1 As Long = (Wire.Y + Wire.dY)
            If Dir(ChosenDir).Y + h1 * 2 + DestCanvasY > Me.Height Then
                DestCanvasY = -(Dir(ChosenDir).Y - Me.Height + h1 * 2 + Wire.dX)
            End If
            If Dir(ChosenDir).Y - h1 + DestCanvasY < 0 Then
                DestCanvasY = -(Dir(ChosenDir).Y - h1 - Wire.dX)
            End If
            'While (Dir(ChosenDir).Y + h1 * 2 + DestCanvasY > Me.Height Or Dir(ChosenDir).Y - h1 + DestCanvasY < 0)
            '    If Dir(ChosenDir).Y + h1 * 2 + DestCanvasY > Me.Height Then
            '        DestCanvasY -= h1
            '    Else
            '        If Dir(ChosenDir).Y - h1 + DestCanvasY < 0 Then DestCanvasY += h1
            '    End If
            '    MsgBox(Me.Height.ToString)
            'End While

            If DestCanvasY > 0 Then DestCanvasY = 0
            If DestCanvasY < MaxCanvasY Then DestCanvasY = MaxCanvasY
        End If
        If Wire.LinesInBox <= 2 Then
            Dim h1 As Long = (Wire.Y + Wire.dY)
            If Dir(ChosenDir).Y + h1 + DestCanvasY > Me.Height Then
                DestCanvasY = -(Dir(ChosenDir).Y - Me.Height + h1 + Wire.dX)
            End If
            If Dir(ChosenDir).Y + DestCanvasY < 0 Then
                DestCanvasY = -(Dir(ChosenDir).Y - Wire.dX)
            End If
            If DestCanvasY > 0 Then DestCanvasY = 0
            If DestCanvasY < MaxCanvasY Then DestCanvasY = MaxCanvasY
        End If

        If tmrScrolling.Enabled = False Then tmrScrolling.Enabled = True

    End Sub

    Private Sub DirsBox_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
            e.IsInputKey = True
        End If
    End Sub


    Private Sub DirsBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        picMain.Width = Me.Width
        picMain.Height = Me.Height
        If (Wire.X + Wire.dX) <> 0 Then
            RecalculateWireParameters()
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

    Private Sub EntryDone()
        txtName.Visible = False
        If TextEntryMode = "mkdir" Then
            Try
                Dim nname As String = txtName.Text
                My.Computer.FileSystem.CreateDirectory(Path + nname)
                NewPath(Path, Path + nname + "\")
            Catch ' ex As Exception
                MsgBox("Creating failed")
            End Try
        End If
        TextEntryMode = ""
        Me.Select()
    End Sub
    Private Sub EntryCancelled()
        txtName.Visible = False
        TextEntryMode = ""
        Me.Select()
    End Sub

    Private Sub txtName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown
        If e.KeyCode = Keys.Enter Then EntryDone()
        If e.KeyCode = Keys.Escape Then EntryCancelled()
    End Sub

    Private Sub txtName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.LostFocus
        EntryDone()
    End Sub

    Private Sub picMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picMain.Click

    End Sub

    Private Sub picMain_MouseWheel(sender As Object, e As MouseEventArgs) Handles picMain.MouseWheel

    End Sub
End Class
