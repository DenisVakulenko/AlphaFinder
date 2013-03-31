Module mod_graphics
    Dim Temp(4) As Bitmap, TempBitmapsLoaded As Boolean = False

    Private Sub LoadTempBitmaps()
        'For i As Short = 0 To 4
        '    Temp(i) = Bitmap.FromFile(Application.StartupPath + "\status_bar\" + i.ToString + ".gif")
        'Next
        For i As Short = 1 To 3
            Temp(i) = Bitmap.FromFile(Application.StartupPath + "\status_bar\" + i.ToString + ".gif")
        Next
        Temp(0) = Bitmap.FromFile(Application.StartupPath + "\status_bar\0.png")
        Temp(4) = Bitmap.FromFile(Application.StartupPath + "\status_bar\4.png")
        TempBitmapsLoaded = True
    End Sub
    Public Function ToRect(ByVal a As Long, ByVal b As Long, ByVal c As Long, ByVal d As Long) As Rectangle
        Dim g As Rectangle
        g.X = a
        g.Y = b
        g.Width = c
        g.Height = d
        Return g
    End Function


    Public Function GenerateStatusBar(ByVal width As Short, ByVal percent As Double) As Bitmap
        If TempBitmapsLoaded = False Then LoadTempBitmaps()

        If percent > 1 Then percent = 1

        Dim Length As Long = (width - 7) * percent
        Dim Bmp As New Bitmap(width, 17)
        Dim Graf As Graphics = Graphics.FromImage(Bmp)

        Graf.CompositingMode = Drawing2D.CompositingMode.SourceCopy

        Graf.DrawImageUnscaled(Temp(0), 0, 0)
        Graf.DrawImage(Temp(2), ToRect(3, 0, Length, 17), ToRect(0, 0, 1, 17), GraphicsUnit.Pixel)
        Graf.DrawImageUnscaled(Temp(1), 3 + Length, 0)
        'a = ToRect(3 + Length + 1, 0, width - Length - 6 - 1, 17)
        Graf.DrawImage(Temp(3), ToRect(3 + Length + 1, 0, width - Length - 6 - 1, 17), ToRect(0, 0, 1, 17), GraphicsUnit.Pixel)
        Graf.DrawImageUnscaled(Temp(4), width - 3, 0)

        Return Bmp
    End Function
    Public Function GenerateMiniStatusBar(ByVal width As Short, ByVal percent As Double) As Bitmap
        If TempBitmapsLoaded = False Then LoadTempBitmaps()

        Dim Length As Long = (width - 7) * percent
        Dim Bmp As New Bitmap(width, 17)
        Dim bmp2 As New Bitmap(width, 10)
        Dim Graf As Graphics = Graphics.FromImage(Bmp)
        Dim Graf2 As Graphics = Graphics.FromImage(bmp2)

        Graf.CompositingMode = Drawing2D.CompositingMode.SourceCopy

        Graf.DrawImageUnscaled(Temp(0), 0, 0)
        Graf.DrawImage(Temp(2), ToRect(3, 0, Length, 17), ToRect(0, 0, 1, 17), GraphicsUnit.Pixel)
        Graf.DrawImageUnscaled(Temp(1), 3 + Length, 0)
        Graf.DrawImage(Temp(3), ToRect(3 + Length + 1, 0, width - Length - 6 - 1, 17), ToRect(0, 0, 1, 17), GraphicsUnit.Pixel)
        Graf.DrawImageUnscaled(Temp(4), width - 3, 0)

        Graf2.DrawImage(Bmp, ToRect(0, 0, width, 5), ToRect(0, 0, width, 5), GraphicsUnit.Pixel)
        Graf2.DrawImage(Bmp, ToRect(0, 4, width, 4), ToRect(0, 17 - 4, width, 4), GraphicsUnit.Pixel)
        'Graf2.DrawImage(Bmp, ToRect(0, 5, width, 5), ToRect(0, 17 - 5, width, 5), GraphicsUnit.Pixel)
        Return bmp2
    End Function
    Public Function GenerateStatusBar(ByVal width As Short, ByVal height As Short, ByVal percent As Double) As Bitmap
        If TempBitmapsLoaded = False Then LoadTempBitmaps()

        Dim Length As Long = (width - 7) * percent
        Dim Bmp As New Bitmap(width, 17)
        Dim bmp2 As New Bitmap(width, height)
        Dim Graf As Graphics = Graphics.FromImage(Bmp)
        Dim Graf2 As Graphics = Graphics.FromImage(bmp2)

        Graf.CompositingMode = Drawing2D.CompositingMode.SourceCopy

        Graf.DrawImageUnscaled(Temp(0), 0, 0)
        Graf.DrawImage(Temp(2), ToRect(3, 0, Length, 17), ToRect(0, 0, 1, 17), GraphicsUnit.Pixel)
        Graf.DrawImageUnscaled(Temp(1), 3 + Length, 0)
        Graf.DrawImage(Temp(3), ToRect(3 + Length + 1, 0, width - Length - 6 - 1, 17), ToRect(0, 0, 1, 17), GraphicsUnit.Pixel)
        Graf.DrawImageUnscaled(Temp(4), width - 3, 0)

        Graf2.DrawImage(Bmp, ToRect(0, 0, width, 4), ToRect(0, 0, width, 4), GraphicsUnit.Pixel)
        Graf2.DrawImage(Bmp, ToRect(0, 4, width, height - 8), ToRect(0, 4, width, 8), GraphicsUnit.Pixel)
        Graf2.DrawImage(Bmp, ToRect(0, height - 4, width, 4), ToRect(0, 17 - 4, width, 4), GraphicsUnit.Pixel)
        'Graf2.DrawImage(Bmp, ToRect(0, 5, width, 5), ToRect(0, 17 - 5, width, 5), GraphicsUnit.Pixel)
        Return bmp2
    End Function
    Public Sub DrawFrame3(ByRef Grafic As System.Drawing.Graphics, ByRef Bitm As Bitmap)
        Dim p1 As New Pen(Color.FromArgb(60, 60, 60))
        p1 = New Pen(Color.FromArgb(70, 70, 70))
        Grafic.DrawLine(p1, 0, 0, Bitm.Width, 0)

        Dim Color1 As Color = Color.FromArgb(150, 150, 150)
        Dim Color2 As Color = Color.FromArgb(100, 100, 100)

        'Bitm.SetPixel(1, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, 1, Color.FromArgb(100, 100, 100))
        Bitm.SetPixel(1, 0, Color.FromArgb(100, 100, 100))
        Bitm.SetPixel(0, 0, Color1)
        'Bitm.SetPixel(Bitm.Width - 2, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, 1, Color2)
        Bitm.SetPixel(Bitm.Width - 2, 0, Color2)
        Bitm.SetPixel(Bitm.Width - 1, 0, Color1)

        'Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 2, Color.FromArgb(190, 190, 190))
        Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 1, Color.FromArgb(190, 190, 190))
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 1, Color.FromArgb(100, 100, 100)) '.FromArgb(150, 150, 150))
        'Bitm.SetPixel(1, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, Bitm.Height - 2, Color.FromArgb(190, 190, 190))
        Bitm.SetPixel(1, Bitm.Height - 1, Color.FromArgb(190, 190, 190))
        Bitm.SetPixel(0, Bitm.Height - 1, Color.FromArgb(100, 100, 100))
    End Sub

    Public Sub DrawFrame2(ByRef Grafic As System.Drawing.Graphics, ByRef Bitm As Bitmap)
        Dim p1 As New Pen(Color.FromArgb(60, 60, 60))
        'Grafic.DrawLine(p1, 0, 0, 0, Bitm.Height)
        p1 = New Pen(Color.FromArgb(230, 0, 0, 0))
        Grafic.DrawLine(p1, 0, Bitm.Height - 1, Bitm.Width, Bitm.Height - 1)
        'Grafic.DrawLine(p1, Bitm.Width - 1, 0, Bitm.Width - 1, Bitm.Height)
        Grafic.DrawLine(p1, 0, 0, Bitm.Width, 0)

        'Dim a1 As New Pen(Color.FromArgb(60, 0, 0, 0))
        'Dim a2 As New Pen(Color.FromArgb(20, 0, 0, 0))
        'Grafic.DrawLine(a1, 0, 1, Bitm.Width, 1)
        'Grafic.DrawLine(a2, 0, 2, Bitm.Width, 2)
        'Grafic.DrawLine(a1, 1, 2, 1, Bitm.Height - 3)
        'Grafic.DrawLine(a1, Bitm.Width - 2, 2, Bitm.Width - 2, Bitm.Height - 3)
        'Grafic.DrawLine(a2, 2, 2, 2, Bitm.Height - 3)
        'Grafic.DrawLine(a2, Bitm.Width - 3, 2, Bitm.Width - 3, Bitm.Height - 3)

        Dim Color1 As Color = Color.FromArgb(150, 150, 150)
        Dim Color2 As Color = Color.FromArgb(100, 100, 100)

        'Bitm.SetPixel(1, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, 1, Color.FromArgb(100, 100, 100))
        Bitm.SetPixel(1, 0, Color.FromArgb(100, 100, 100))
        Bitm.SetPixel(0, 0, Color1)

        'Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 2, Color2)
        Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 1, Color2)
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 1, Color1)

        'Bitm.SetPixel(Bitm.Width - 2, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, 1, Color2)
        Bitm.SetPixel(Bitm.Width - 2, 0, Color2)
        Bitm.SetPixel(Bitm.Width - 1, 0, Color1)

        'Bitm.SetPixel(1, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, Bitm.Height - 2, Color2)
        Bitm.SetPixel(1, Bitm.Height - 1, Color2)
        Bitm.SetPixel(0, Bitm.Height - 1, Color1)
    End Sub

    Public Sub DrawFrame(ByRef Grafic As System.Drawing.Graphics, ByRef Bitm As Bitmap)
        Grafic.DrawLine(Pens.Black, 2, 0, Bitm.Width - 3, 0)
        Grafic.DrawLine(Pens.Black, 0, 2, 0, Bitm.Height - 3)
        Grafic.DrawLine(Pens.Black, 2, Bitm.Height - 1, Bitm.Width - 3, Bitm.Height - 1)
        Grafic.DrawLine(Pens.Black, Bitm.Width - 1, 2, Bitm.Width - 1, Bitm.Height - 3)

        Dim a1 As New Pen(Color.FromArgb(60, 0, 0, 0))
        Dim a2 As New Pen(Color.FromArgb(20, 0, 0, 0))
        Grafic.DrawLine(a1, 2, 1, Bitm.Width - 3, 1)
        Grafic.DrawLine(a2, 1, 2, Bitm.Width - 2, 2)
        Grafic.DrawLine(a1, 1, 2, 1, Bitm.Height - 3)
        Grafic.DrawLine(a1, Bitm.Width - 2, 2, Bitm.Width - 2, Bitm.Height - 3)
        Grafic.DrawLine(a2, 2, 2, 2, Bitm.Height - 3)
        Grafic.DrawLine(a2, Bitm.Width - 3, 2, Bitm.Width - 3, Bitm.Height - 3)

        Bitm.SetPixel(1, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, 1, Color.FromArgb(100, 100, 100))
        Bitm.SetPixel(1, 0, Color.FromArgb(100, 100, 100))
        Bitm.SetPixel(0, 0, Color.FromArgb(200, 200, 200))

        Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 2, Color.FromArgb(150, 150, 150))
        Bitm.SetPixel(Bitm.Width - 2, Bitm.Height - 1, Color.FromArgb(150, 150, 150))
        Bitm.SetPixel(Bitm.Width - 1, Bitm.Height - 1, Color.FromArgb(200, 200, 200))

        Bitm.SetPixel(Bitm.Width - 2, 1, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(Bitm.Width - 1, 1, Color.FromArgb(150, 150, 150))
        Bitm.SetPixel(Bitm.Width - 2, 0, Color.FromArgb(150, 150, 150))
        Bitm.SetPixel(Bitm.Width - 1, 0, Color.FromArgb(200, 200, 200))

        Bitm.SetPixel(1, Bitm.Height - 2, Color.FromArgb(50, 50, 50))
        Bitm.SetPixel(0, Bitm.Height - 2, Color.FromArgb(150, 150, 150))
        Bitm.SetPixel(1, Bitm.Height - 1, Color.FromArgb(150, 150, 150))
        Bitm.SetPixel(0, Bitm.Height - 1, Color.FromArgb(200, 200, 200))
    End Sub
    Public Sub DrawFrame(ByRef Grafic As System.Drawing.Graphics, ByRef Bitm As Bitmap, ByVal w As Short, ByVal h As Short)
        If w <= Bitm.Width And h <= Bitm.Height Then
            'MsgBox(w.ToString + " " + h.ToString + " " + Bitm.Width.ToString + " " + Bitm.Height.ToString)

            Grafic.DrawLine(Pens.Black, 2, 0, w - 3, 0)
            Grafic.DrawLine(Pens.Black, 0, 2, 0, h - 3)
            Grafic.DrawLine(Pens.Black, 2, h - 1, w - 3, h - 1)
            Grafic.DrawLine(Pens.Black, w - 1, 2, w - 1, h - 3)

            Dim a1 As New Pen(Color.FromArgb(60, 0, 0, 0))
            Dim a2 As New Pen(Color.FromArgb(20, 0, 0, 0))
            Grafic.DrawLine(a1, 2, 1, w - 3, 1)
            Grafic.DrawLine(a2, 1, 2, w - 2, 2)
            Grafic.DrawLine(a1, 1, 2, 1, h - 3)
            Grafic.DrawLine(a1, w - 2, 2, w - 2, h - 3)
            Grafic.DrawLine(a2, 2, 2, 2, h - 3)
            Grafic.DrawLine(a2, w - 3, 2, w - 3, h - 3)

            Bitm.SetPixel(1, 1, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(0, 1, Color.FromArgb(100, 100, 100))
            Bitm.SetPixel(1, 0, Color.FromArgb(100, 100, 100))
            Bitm.SetPixel(0, 0, Color.FromArgb(200, 200, 200))

            Bitm.SetPixel(w - 2, h - 2, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(w - 1, h - 2, Color.FromArgb(150, 150, 150))
            Bitm.SetPixel(w - 2, h - 1, Color.FromArgb(150, 150, 150))
            Bitm.SetPixel(w - 1, h - 1, Color.FromArgb(200, 200, 200))

            Bitm.SetPixel(w - 2, 1, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(w - 1, 1, Color.FromArgb(150, 150, 150))
            Bitm.SetPixel(w - 2, 0, Color.FromArgb(150, 150, 150))
            Bitm.SetPixel(w - 1, 0, Color.FromArgb(200, 200, 200))

            Bitm.SetPixel(1, h - 2, Color.FromArgb(50, 50, 50))
            Bitm.SetPixel(0, h - 2, Color.FromArgb(150, 150, 150))
            Bitm.SetPixel(1, h - 1, Color.FromArgb(150, 150, 150))
            Bitm.SetPixel(0, h - 1, Color.FromArgb(200, 200, 200))
        End If
    End Sub

End Module
