Module thumbs
    Public Thumbnails(50000) As String, ThumbDate(50000) As Long, NThumbs As Long = 0

    Public Sub LoadThumbsInfo()
        If IO.File.Exists(Application.StartupPath + "\config\names.txt") And IO.File.Exists(Application.StartupPath + "\config\dates.txt") Then
            Try
                FileOpen(1, Application.StartupPath + "\config\names.txt", OpenMode.Input)
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

                FileClose(1) : FileClose(2)
            Catch
                FileClose(1) : FileClose(2)
            End Try
        Else
            NThumbs = 0
            IO.File.Create(Application.StartupPath + "\config\names.txt")
            IO.File.Create(Application.StartupPath + "\config\dates.txt")
        End If
    End Sub
    Public Sub SaveThumbsInfo()
        If NThumbs > 0 Then
            Try
                FileOpen(1, Application.StartupPath + "\config\names.txt", OpenMode.Output)
                FileOpen(2, Application.StartupPath + "\config\dates.txt", OpenMode.Output)
                For i As Long = 1 To NThumbs
                    PrintLine(1, Thumbnails(i))
                    PrintLine(2, ThumbDate(i).ToString)
                Next
                FileClose(1) : FileClose(2)
            Catch
                FileClose(1) : FileClose(2)
            End Try
        End If
    End Sub
    Public Function SearchInThumbs(ByVal path As String) As Long
        path = path.ToLower
        For i As Long = 1 To NThumbs
            If path = Thumbnails(i) Then Return i : Exit For
        Next
    End Function

    Public Function ThumbIsNotOld(ByVal i As Long) As Boolean
        'MsgBox(My.Computer.FileSystem.GetFileInfo(Thumbnails(i)).LastWriteTime.Ticks())
        'MsgBox(ThumbDate(i))
        If My.Computer.FileSystem.GetFileInfo(Thumbnails(i)).LastWriteTime.Ticks() = ThumbDate(i) Then
            Return True
        Else
            Return False
        End If
    End Function



    Public Sub CorrectSize(ByRef W As Long, ByRef H As Long, ByVal DestSize As Point)
        If w > DestSize.X Or h > DestSize.Y Then
            If w / h > DestSize.X / DestSize.Y Then
                h = Int(DestSize.X / w * h)
                w = DestSize.X
            Else
                w = Int(DestSize.Y / h * w)
                h = DestSize.Y
            End If
        End If
        If h < 1 Then h = 1
        If w < 1 Then w = 1
    End Sub
    'Private Sub CorrectSize(ByRef w As Long, ByRef h As Long)
    '    If w > img_wire.Width Then
    '        h = Int(img_wire.Width / w * h)
    '        w = img_wire.Width
    '    End If

    '    If H < 1 Then H = 1
    '    If W < 1 Then W = 1
    'End Sub
    Public Function ReMakeThumb(ByVal I As Long, ByVal Size As Point, ByRef Thumb As Bitmap) As Boolean 'bbbbbbbbbbbbbbbbbbbb
        'for image saving
        Dim myImageCodecInfo As Imaging.ImageCodecInfo, myEncoder As Imaging.Encoder
        Dim myEncoderParameter As Imaging.EncoderParameter, myEncoderParameters As Imaging.EncoderParameters
        myImageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Jpeg)
        myEncoder = System.Drawing.Imaging.Encoder.Quality
        myEncoderParameters = New Imaging.EncoderParameters(1)
        myEncoderParameter = New Imaging.EncoderParameter(myEncoder, CType(100L, Int32))
        myEncoderParameters.Param(0) = myEncoderParameter
        'for image saving
        Dim bmp1 As Bitmap, W, H As Long
        Try
            bmp1 = Image.FromFile(Thumbnails(I))
        Catch ex As Exception
            Return False
        End Try
        W = bmp1.Width
        H = bmp1.Height

        CorrectSize(W, H, Size)
        Thumb = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graf As Graphics = Graphics.FromImage(Thumb)
            graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graf.DrawImage(bmp1, -1, -1, W + 1, H + 1)
            graf.DrawImage(bmp1, 0, 0, W, H)

            ThumbDate(I) = My.Computer.FileSystem.GetFileInfo(Thumbnails(I)).LastWriteTime.Ticks
            Try
                Thumb.Save(Application.StartupPath + "\config\th\" + I.ToString + ".jpg", myImageCodecInfo, myEncoderParameters)
            Catch ex As Exception
            End Try
        End Using
        'bmp1 = Nothing

        bmp1.Dispose()
        Return True
    End Function
    Public Function MakeThumb(ByVal FileName As String, ByVal Size As Point, ByRef Thumb As Bitmap) As Boolean
        'for image saving
        Dim myImageCodecInfo As Imaging.ImageCodecInfo, myEncoder As Imaging.Encoder
        Dim myEncoderParameter As Imaging.EncoderParameter, myEncoderParameters As Imaging.EncoderParameters
        myImageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Jpeg)
        myEncoder = System.Drawing.Imaging.Encoder.Quality
        myEncoderParameters = New Imaging.EncoderParameters(1)
        myEncoderParameter = New Imaging.EncoderParameter(myEncoder, CType(100L, Int32))
        myEncoderParameters.Param(0) = myEncoderParameter
        'for image saving


        Dim bmp1 As Bitmap, W, H As Long

        Try
            bmp1 = Image.FromFile(FileName)
        Catch ex As Exception
            Return False
        Finally
        End Try
        W = bmp1.Width
        H = bmp1.Height

        CorrectSize(W, H, Size)

        Thumb = New Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
        Using graf As Graphics = Graphics.FromImage(Thumb)
            graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graf.DrawImage(bmp1, -1, -1, W + 1, H + 1)
            graf.DrawImage(bmp1, 0, 0, W, H)

            '    graf.Clear(Color.FromArgb(0, 0, 0))
            '    graf.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            '    graf.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            '    graf.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            '    'graf.CompositingMode = Drawing2D.CompositingMode.SourceCopy
            '    'graf.DrawImage(bmp1, ToRect(0, 0, W, H), ToRect(0, 0, bmp1.Width, bmp1.Height), GraphicsUnit.Pixel)
            '    'graf.DrawImage(bmp1, -1, -1, W + 1, H + 1)
            '    graf.DrawImage(bmp1, -1, -1, W + 2, H + 2)
            '    graf.DrawImage(bmp1, 0, 0, W, H)

            NThumbs += 1
            Thumbnails(NThumbs) = FileName.ToLower
            ThumbDate(NThumbs) = My.Computer.FileSystem.GetFileInfo(FileName).LastWriteTime.Ticks

            Thumb.Save(Application.StartupPath + "\config\th\" + NThumbs.ToString + ".jpg", myImageCodecInfo, myEncoderParameters)
        End Using
        bmp1 = Nothing

        Return True
    End Function

End Module
