Module rotating
    ' + -------- + '
    ' | ROTATING | '
    ' + -------- + '

    Public Sub rotate_picture_plus(ByVal Path As String)
        'show_wait_panel()
        'set_wait_panel(0, "wait please..." + Chr(13) + Chr(10) + "[ loading ]")

        'Dim bmp1 As Bitmap = frmMain.picBigPhoto.Image, bmp2 As New Bitmap(bmp1.Height, bmp1.Width)
        'Dim graf As Graphics = Graphics.FromImage(bmp2)


        'set_wait_panel(0.33, "wait please..." + Chr(13) + Chr(10) + "[ resizing ]")

        'graf.TranslateTransform(bmp1.Height - 1, 0)
        'graf.RotateTransform(90)
        'graf.DrawImage(bmp1, 0, 0, bmp1.Width, bmp1.Height)


        'set_wait_panel(0.66, "wait please..." + Chr(13) + Chr(10) + "[ saving ]")

        'Try : frmMain.picBigPhoto.Image.Dispose() : bmp1.Dispose() : Catch ex As Exception : MsgBox("err") : End Try 'unloading images
        'Try : bmpPicts(0).Dispose() : Catch ex As Exception : End Try
        'Try : bmpPicts(1).Dispose() : Catch ex As Exception : End Try

        'Dim myImageCodecInfo As Imaging.ImageCodecInfo, myEncoder As Imaging.Encoder 'saving process
        'Dim myEncoderParameter As Imaging.EncoderParameter, myEncoderParameters As Imaging.EncoderParameters

        'myImageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Jpeg)
        'myEncoder = System.Drawing.Imaging.Encoder.Quality
        'myEncoderParameters = New Imaging.EncoderParameters(1)

        'myEncoderParameter = New Imaging.EncoderParameter(myEncoder, CType(100L, Int32))
        'myEncoderParameters.Param(0) = myEncoderParameter
        'bmp2.Save(Path, myImageCodecInfo, myEncoderParameters)


        'set_wait_panel(1, "wait please..." + Chr(13) + Chr(10) + "[ done ]")

        'frmMain.picBigPhoto.Image = bmp2

        'hide_wait_panel()
    End Sub
    Public Sub rotate_picture_minus(ByVal Path As String)
        'show_wait_panel()
        'set_wait_panel(0, "wait please..." + Chr(13) + Chr(10) + "[ loading ]")

        'Dim bmp1 As Bitmap = frmMain.picBigPhoto.Image, bmp2 As New Bitmap(bmp1.Height, bmp1.Width)
        'Dim graf As Graphics = Graphics.FromImage(bmp2)


        'set_wait_panel(0.33, "wait please..." + Chr(13) + Chr(10) + "[ resizing ]")

        'graf.TranslateTransform(0, bmp1.Width - 1)
        'graf.RotateTransform(-90)
        'graf.DrawImage(bmp1, 0, 0, bmp1.Width, bmp1.Height)


        'set_wait_panel(0.66, "wait please..." + Chr(13) + Chr(10) + "[ saving ]")

        'Try : frmMain.picBigPhoto.Image.Dispose() : bmp1.Dispose() : Catch ex As Exception : MsgBox("err") : End Try 'unloading images
        'Try : bmpPicts(0).Dispose() : Catch ex As Exception : End Try
        'Try : bmpPicts(1).Dispose() : Catch ex As Exception : End Try

        'Dim myImageCodecInfo As Imaging.ImageCodecInfo, myEncoder As Imaging.Encoder 'saving process
        'Dim myEncoderParameter As Imaging.EncoderParameter, myEncoderParameters As Imaging.EncoderParameters

        'myImageCodecInfo = GetEncoderInfo(System.Drawing.Imaging.ImageFormat.Jpeg)
        'myEncoder = System.Drawing.Imaging.Encoder.Quality
        'myEncoderParameters = New Imaging.EncoderParameters(1)

        'myEncoderParameter = New Imaging.EncoderParameter(myEncoder, CType(100L, Int32))
        'myEncoderParameters.Param(0) = myEncoderParameter
        'bmp2.Save(Path, myImageCodecInfo, myEncoderParameters)


        'set_wait_panel(1, "wait please..." + Chr(13) + Chr(10) + "[ done ]")

        'frmMain.picBigPhoto.Image = bmp2

        'hide_wait_panel()
    End Sub
    Public Function GetEncoderInfo(ByVal format As Imaging.ImageFormat) As Imaging.ImageCodecInfo
        Dim j As Integer
        Dim encoders() As Imaging.ImageCodecInfo
        encoders = Imaging.ImageCodecInfo.GetImageEncoders()
        j = 0
        While j < encoders.Length
            If encoders(j).FormatID = format.Guid Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing
    End Function 'GetEncoderInfo



End Module
