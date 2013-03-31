Public Class ucPathLine
    Event PathChaged(ByVal Up As Boolean)

    Structure PathType
        Dim x As Long
        Dim name As String
        Dim name_min As String
        Dim width As Long
        Dim width_min As Long
    End Structure

    Dim tile(9) As Bitmap
    Public CurrentPath, MaxPath As String
    Dim Path(50) As PathType, NDirs As Long, NDirsMax As Long
    Dim TextMode As Boolean
    Dim TilesLoaded As Boolean = False

    Dim bmp As New Bitmap(100, 25, Imaging.PixelFormat.Format32bppRgb)
    Dim graph As Graphics = Graphics.FromImage(bmp)


    'Private Function GetShortName(ByVal name As String) As String
    '    lblName.Text = name
    '    While lblName.Width > 50
    '        name = Mid(name, 1, Len(name) - 1)
    '        lblName.Text = name
    '    End While

    '    Return name
    'End Function


    Public Sub NewMaxPath(ByVal path As String)
        If path.Length <> 0 Then
            Dim dr As String = "F" 'Mid(Application.StartupPath, 1, 1)
            Dim s As String = LCase(path)
            If s = "music" Then path = dr + ":\[ music ]\"
            If s = "films" Then path = dr + ":\[ films ]\"
            If s = "progs" Then path = dr + ":\[ progs ]\_net\"
            If s = "univer" Then path = "D:\UNIVER\"
            If s = "foto" Or s = "photo" Then path = dr + ":\[ photo ]\"
            If s = "makro" Then path = dr + ":\[ photo ]\[ makro ]\"
            If s = "35" Or s = "35photo" Then path = Application.StartupPath + "\35photo\"
            If s = "ws" Or s = "screen" Then path = "C:\Users\Denis\Desktop\"
            If Mid(path, path.Length, 1) <> "\" Then path += "\"
        End If
        If Not IO.Directory.Exists(path) Then path = "home"
        MaxPath = path
        Parse()
        If NDirsMax < NDirs Then NDirsMax = NDirs
        Redraw()
    End Sub
    'Dim text_font As New Font("Calibri", 8, FontStyle.Regular)
    Dim text_font As New Font("Verdana", 7, FontStyle.Regular)

    'Dim text_font As New Font("Tahoma", 9, FontStyle.Regular)
    'Dim text_font As New Font("Georgia", 8, FontStyle.Regular)
    'Dim text_font As New Font("Tahoma", 8, GraphicsUnit.Point)
    'Dim a As New Font

    Private Sub Parse()
        Dim str As String = MaxPath
        Dim x As Long = tile(1).Width + 3, n As Short
        NDirs = 0
        n = 0
        While InStr(str, "\")
            NDirs += 1
            If Path(NDirs).name <> Mid(str, 1, InStr(str, "\") - 1) And n = 0 Then
                n = NDirs
            End If
            Path(NDirs).name = Mid(str, 1, InStr(str, "\") - 1)
            Path(NDirs).name_min = Path(NDirs).name
            'lblName.Text = Path(NDirs).name
            Path(NDirs).width = graph.MeasureString(Path(NDirs).name_min, text_font).Width + 1  'lblName.Width - 1
            If Path(NDirs).width < 16 Then Path(NDirs).width = 16
            While Path(NDirs).width > 80
                If Mid(Path(NDirs).name_min, Path(NDirs).name_min.Length - 4, 1) = " " Then
                    Path(NDirs).name_min = Mid(Path(NDirs).name_min, 1, Path(NDirs).name_min.Length - 5) + "..."
                Else
                    Path(NDirs).name_min = Mid(Path(NDirs).name_min, 1, Path(NDirs).name_min.Length - 4) + "..."
                End If
                Path(NDirs).width = graph.MeasureString(Path(NDirs).name_min, text_font).Width + 1
            End While
            Path(NDirs).x = x
            x = x + Path(NDirs).width + tile(2).Width + tile(4).Width
            str = Mid(str, InStr(str, "\") + 1)
        End While
        If NDirs > 1 Then
            x = x - Path(NDirs).width
            Path(NDirs).name_min = Path(NDirs).name
            Path(NDirs).width = graph.MeasureString(Path(NDirs).name_min, text_font).Width + 1
            While Path(NDirs).width > Me.Width - (Path(NDirs - 1).x + Path(NDirs - 1).width + tile(2).Width + tile(4).Width) - 30 And Path(NDirs).name_min.Length > 3
                Path(NDirs).name_min = Mid(Path(NDirs).name_min, 1, Path(NDirs).name_min.Length - 4) + "..."
                Path(NDirs).width = graph.MeasureString(Path(NDirs).name_min, text_font).Width + 1
            End While
            x = x + Path(NDirs).width
        End If
        If n <> 0 Then NDirsMax = NDirs
        For i As Short = NDirs + 1 To NDirsMax
            Path(i).name_min = Path(i).name
            Path(i).width = graph.MeasureString(Path(i).name_min, text_font).Width + 1
            While Path(i).width > 80
                Path(i).name_min = Mid(Path(i).name_min, 1, Path(i).name_min.Length - 4) + "..."
                Path(i).width = graph.MeasureString(Path(i).name_min, text_font).Width + 1
            End While
            Path(i).x = x
            x = x + Path(i).width + tile(2).Width + tile(4).Width
        Next

        'If x > Me.Width - 5 Then
        '    Dim d As Long = -Me.Width + x + 5
        '    For i As Long = 0 To NDirsMax
        '        Path(i).x = Path(i).x - d
        '    Next
        'End If
        'For i As Long = NDirs + 1 To NDirsMax
        '    Path(i).x = Path(i - 1).x + Path(i - 1).width
        'Next
    End Sub
    Private Sub Redraw()
        If TilesLoaded Then
            
            Dim x As Long = 0
            'Dim bmp As New Bitmap(Me.Width, 25, Imaging.PixelFormat.Format32bppRgb)
            'Dim graph As Graphics = Graphics.FromImage(bmp)
            graph = Graphics.FromImage(bmp)
            'graph.Clear(Color.White)

            graph.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graph.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            'graph.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            graph.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            graph.DrawImageUnscaled(tile(1), x, 0) : x = x + tile(1).Width
            graph.DrawImageUnscaled(tile(4), x, 0) : x = x + tile(4).Width
            Dim k As Short
            For i As Short = 1 To NDirsMax
                x = Path(i).x
                If i <= NDirs Then k = 0 Else k = 3
                graph.DrawImageUnscaled(tile(2 + k), x, 0) : x = x + tile(2 + k).Width
                For ii As Short = 1 To Path(i).width
                    graph.DrawImageUnscaled(tile(3 + k), x, 0) : x = x + tile(3 + k).Width
                Next
                graph.DrawImageUnscaled(tile(4 + k), x, 0) : x = x + tile(4 + k).Width
                Dim a As New SolidBrush(Color.FromArgb(255 - k * 12, 255 - k * 12, 255 - k * 12))
                graph.DrawString(Path(i).name_min, text_font, a, Path(i).x + tile(2 + k).Width, 7)
            Next
            While x < Me.Width - tile(9).Width
                graph.DrawImageUnscaled(tile(8), x, 0) : x = x + tile(8).Width
            End While
            graph.DrawImageUnscaled(tile(9), x, 0) : x = x + tile(9).Width

            picMain.Image = bmp
            picMain.Refresh()
            Me.Refresh()
        End If
    End Sub
    Private Sub RedrawCustom(ByVal index As Short)
        If TilesLoaded Then
            Dim text_font As New Font("Verdana", 7, FontStyle.Regular)
            Dim x As Long = 0, k As Short

            graph.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graph.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            'graph.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            graph.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            graph.DrawImageUnscaled(tile(1), x, 0) : x = x + tile(1).Width
            graph.DrawImageUnscaled(tile(4), x, 0) : x = x + tile(4).Width
            For i As Short = 1 To NDirsMax
                x = Path(i).x
                If i <= index Then k = 0 Else k = 3
                graph.DrawImageUnscaled(tile(2 + k), x, 0) : x = x + tile(2 + k).Width
                For ii As Short = 1 To Path(i).width
                    graph.DrawImageUnscaled(tile(3 + k), x, 0) : x = x + tile(3 + k).Width
                Next
                graph.DrawImageUnscaled(tile(4 + k), x, 0) : x = x + tile(4 + k).Width
                Dim a As New SolidBrush(Color.FromArgb(255 - k * 12, 255 - k * 12, 255 - k * 12))
                graph.DrawString(Path(i).name_min, text_font, a, Path(i).x + tile(2 + k).Width, 7)
            Next
            While x < Me.Width - tile(9).Width
                graph.DrawImageUnscaled(tile(8), x, 0) : x = x + tile(8).Width
            End While
            graph.DrawImageUnscaled(tile(9), x, 0) : x = x + tile(9).Width

            picMain.Image = bmp
            Me.Refresh()
        End If
    End Sub
    Private Sub RedrawInTextMode()
        If TilesLoaded Then
            Dim text_font As New Font("Verdana", 7, FontStyle.Regular)
            Dim x As Long = 0

            graph.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graph.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            'graph.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            graph.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            graph.DrawImageUnscaled(tile(1), x, 0) : x = x + tile(1).Width
            graph.DrawImageUnscaled(tile(4), x, 0) : x = x + tile(4).Width
            While x < Me.Width - tile(9).Width
                graph.DrawImageUnscaled(tile(8), x, 0) : x = x + tile(8).Width
            End While
            graph.DrawImageUnscaled(tile(9), x, 0) : x = x + tile(9).Width

            picMain.Image = bmp
            Me.Refresh()
        End If
    End Sub

    Private Sub PathLine_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim err As Boolean = False
        Me.Height = 25

        For i As Short = 1 To 9
            If IO.File.Exists(Application.StartupPath + "\path_line_1\" + i.ToString + ".bmp") Then tile(i) = New Bitmap(Application.StartupPath + "\path_line_1\" + i.ToString + ".bmp") Else err = True : Exit For ':MsgBox("NO FILE!! " + Application.StartupPath + "\path_line_1\" + i.ToString + ".bmp") 
        Next
        If Not err Then TilesLoaded = True : NewMaxPath("home")
    End Sub

    Private Sub picMain_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseClick
        Dim x As Long = e.X
        Dim str As String = ""
        Dim f As Boolean

        If TextMode Then
            NewMaxPath("home")
            RaiseEvent PathChaged(True)
        Else
            For i As Short = 1 To NDirsMax
                str = str + Path(i).name + "\"
                If x > Path(i).x And x <= Path(i).x + Path(i).width + tile(2).Width + tile(4).Width Then
                    NewMaxPath(str)
                    RaiseEvent PathChaged(True)
                    f = True
                End If
            Next
            If Not f Then
                If x > 0 And x <= Path(1).x Then
                    NewMaxPath("home")
                    RaiseEvent PathChaged(True)
                Else
                    MakeTextMode()
                End If
            End If
        End If
    End Sub
    Public Sub MakeTextMode()
        TextMode = True
        txtPath.Text = MaxPath
        txtPath.Visible = True
        txtPath.Select()
        RedrawInTextMode()
    End Sub
    Private Sub picMain_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMain.MouseLeave
        If TextMode Then
            RedrawInTextMode()
        Else
            Redraw()
        End If
    End Sub

    Private Sub picMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseMove
        If Not TextMode Then
            Dim x As Long = e.X
            Dim str As String = ""
            Dim f As Boolean

            For i As Short = 1 To NDirsMax
                If x > Path(i).x And x <= Path(i).x + Path(i).width + tile(2).Width + tile(4).Width Then
                    RedrawCustom(i)
                    f = True
                End If
            Next
            If Not f Then
                If x > 0 And x <= Path(1).x Then
                    RedrawCustom(0)
                Else
                    Redraw()
                End If
            End If
        Else
            'RedrawInTextMode()
        End If
    End Sub

    Private Sub txtPath_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPath.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextMode = False
            txtPath.Visible = False
            NewMaxPath(txtPath.Text)
            RaiseEvent PathChaged(False)
        End If
    End Sub

    Private Sub txtPath_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.LostFocus
        TextMode = False
        txtPath.Visible = False
        NewMaxPath(txtPath.Text)
        RaiseEvent PathChaged(False)
    End Sub

    Private Sub PathLine_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        picMain.Width = Me.Width
        bmp = New Bitmap(Me.Width, 25, Imaging.PixelFormat.Format32bppRgb)
        graph = Graphics.FromImage(bmp)
        'txtPath.Width = Me.Width - txtPath.Left - 5
        'RedrawInTextMode()
    End Sub
End Class
