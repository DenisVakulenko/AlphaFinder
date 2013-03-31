Module thumbs
    Public Thumbnails(10000) As String, ThumbDate(10000) As Long, NThumbs As Long


    Public Sub LoadThumbsInfo()
        FileOpen(1, Application.StartupPath + "\config\base.txt", OpenMode.Input)
        FileOpen(2, Application.StartupPath + "\config\dates.txt", OpenMode.Input)

        NThumbs = 0
        While Not EOF(1)
            NThumbs += 1
            Thumbnails(NThumbs) = LineInput(1).ToLower.Trim
        End While
        NThumbs = 0
        While Not EOF(2)
            NThumbs += 1
            ThumbDate(NThumbs) = LineInput(2)
        End While

        FileClose(2)
        FileClose(1)
    End Sub
    Public Sub SaveThumbsInfo()
        FileOpen(1, Application.StartupPath + "\config\base.txt", OpenMode.Output)
        FileOpen(2, Application.StartupPath + "\config\dates.txt", OpenMode.Output)
        For i As Long = 1 To NThumbs
            PrintLine(1, Thumbnails(i))
            PrintLine(2, ThumbDate(i).ToString)
        Next
        FileClose(2)
        FileClose(1)
    End Sub
    Public Function SearchInThumbs(ByVal path As String) As Long
        path = path.ToLower
        For i As Long = 1 To NThumbs
            If path = Thumbnails(i) Then Return (i) : Exit For
        Next
    End Function
    Public Function ThumbIsNotOld(ByVal i As Long) As Boolean
        If My.Computer.FileSystem.GetFileInfo(Thumbnails(i)).LastWriteTime.Ticks() = ThumbDate(i) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function ReMakeThumb(ByVal I As Long, ByVal Size As Point, ByRef Thumb As Bitmap) As Boolean 'bbbbbbbbbbbbbbbbbbbb
        Dim bmp1 As Bitmap, W, H As Long

        Try
            bmp1 = Image.FromFile(Thumbnails(I))
        Catch ex As Exception
            Return False
        Finally
        End Try
        W = bmp1.Width
        H = bmp1.Height

        If W > Size.X Or H > Size.Y Then
            If W / H > Size.X / Size.Y Then
                H = Int(Size.X / W * H)
                W = Size.X
            Else
                W = Int(Size.Y / H * W)
                H = Size.Y
            End If
        End If
        If H < 1 Then H = 1
        If W < 1 Then W = 1


        Thumb = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        Using graf As Graphics = Graphics.FromImage(Thumb)
            graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graf.DrawImage(bmp1, 0, 0, W, H)

            ThumbDate(I) = My.Computer.FileSystem.GetFileInfo(Thumbnails(I)).LastWriteTime.Ticks
            Thumb.Save(Application.StartupPath + "\config\th\" + I.ToString + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
        End Using
        bmp1 = Nothing
        bmp1.Dispose()
    End Function
    Public Function MakeThumb(ByVal FileName As String, ByVal Size As Point, ByRef Thumb As Bitmap) As Boolean
        Dim bmp1 As Bitmap, W, H As Long

        Try
            bmp1 = Image.FromFile(FileName)
        Catch ex As Exception
            Return False
        Finally
        End Try
        W = bmp1.Width
        H = bmp1.Height

        If W > Size.X Or H > Size.Y Then
            If W / H > Size.X / Size.Y Then
                H = Int(Size.X / W * H)
                W = Size.X
            Else
                W = Int(Size.Y / H * W)
                H = Size.Y
            End If
        End If
        If H < 1 Then H = 1
        If W < 1 Then W = 1


        Thumb = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        Using graf As Graphics = Graphics.FromImage(Thumb)
            graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graf.DrawImage(bmp1, 0, 0, W, H)

            NThumbs += 1
            Thumbnails(NThumbs) = FileName.ToLower
            ThumbDate(NThumbs) = My.Computer.FileSystem.GetFileInfo(FileName).LastWriteTime.Ticks

            Thumb.Save(Application.StartupPath + "\config\th\" + NThumbs.ToString + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
            SaveThumbsInfo()
        End Using
        bmp1 = Nothing

        Return True
    End Function

End Module
