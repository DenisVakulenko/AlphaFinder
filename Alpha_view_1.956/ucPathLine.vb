Public Class ucPathLine
    Event PathChaged(ByVal Up As Boolean)

    Structure PathType
        Dim x As Long
        Dim name As String
        Dim name_min As String
        Dim width As Long
        Dim width_min As Long
    End Structure

    Dim tile(12) As Bitmap
    Public CurrentPath, MaxPath As String
    Public Path(50) As PathType
    Public NDirs As Long, NDirsMax As Long
    Dim TextMode As Boolean
    Dim TilesLoaded As Boolean = False

    Dim bmp As New Bitmap(100, 25, Imaging.PixelFormat.Format32bppRgb)
    Dim graph As Graphics = Graphics.FromImage(bmp)

    Public Function NewPathByIndex(Index As Short) As Boolean
        If Index <= NDirsMax And Index > 0 Then

            NDirs = Index

            UnderSelectedColor = New SolidBrush(Color.FromArgb(19, 130, 206))
            RedrawCustom(NDirs)
            tmrMain.Enabled = True

            Dim newPath As String = ""
            For i As Short = 1 To Index
                newPath = newPath + Path(i).name + "\"
            Next
            NewMaxPath(newPath)
            RaiseEvent PathChaged(True)

            Return True
        Else
            Return False
        End If
    End Function

    Public Sub GoUp()
        Dim str As String = MaxPath
        If MaxPath <> "home" Then
            If Len(MaxPath) > 3 Then
                MaxPath = Mid(MaxPath, 1, MaxPath.Length - 1)
                MaxPath = Mid(MaxPath, 1, MaxPath.LastIndexOf("\") + 1)
                NewMaxPath(MaxPath)
            Else
                NewMaxPath("home")
            End If
        End If
    End Sub
    Public Sub NewMaxPath(ByVal path As String)
        'tmrMain.Enabled = True
        'UnderSelectedColor = New SolidBrush(Color.FromArgb(19, 130, 206))
        'MsgBox("1" + path)
        If path = "" Then path = "home"
        If path <> "home" And path.IndexOf("search", StringComparison.CurrentCultureIgnoreCase) <> 0 And path.IndexOf("music", StringComparison.CurrentCultureIgnoreCase) <> 0 Then
            Dim dr As String = "F" 'Mid(Application.StartupPath, 1, 1)
            Dim s As String = LCase(path)
            'If s = "music" Then
            '    path = dr + ":\[ music ]\" : If Not My.Computer.FileSystem.DirectoryExists(path) Then path = "D:\music\"
            'End If
            If s = "films" Then path = dr + ":\[ films ]\"
            If s = "progs" Then path = dr + ":\[ progs ]\_net\"
            If s = "univer" Then path = "D:\Documents\_UNIVER\"
            If s = "docs" Then path = "D:\Documents\"
            If s = "my prog" Then path = "D:\Documents\Alpha_view_1.955_test_11\"
            If s = "foto" Or s = "photo" Then path = dr + ":\[ photo ]\"
            If s = "makro" Then path = dr + ":\[ photo ]\[ makro ]\"
            If s = "35" Or s = "35photo" Then path = Application.StartupPath + "\35photo\"
            If s = "ws" Or s = "screen" Then path = "C:\Users\" + System.Environment.UserName + "\Desktop\"
            If Mid(path, path.Length, 1) <> "\" Then path += "\"

            If Not My.Computer.FileSystem.DirectoryExists(path) Or path.Length = 0 Then
                path = "home" ': MsgBox("HERE")
            End If
        End If
        If path <> "home" And Mid(path, path.Length, 1) <> "\" Then path += "\"

        MaxPath = path
        Parse()
        If NDirsMax < NDirs Then NDirsMax = NDirs
        Redraw()
        'MsgBox("2" + path)
    End Sub

    Dim text_font As New Font("Verdana", 7, FontStyle.Regular)
    'Dim text_font As New Font("Calibri", 8, FontStyle.Regular)
    'Dim text_font As New Font("Tahoma", 9, FontStyle.Regular)
    'Dim text_font As New Font("Tahoma", 8, GraphicsUnit.Point)
    'Dim text_font As New Font("Georgia", 8, FontStyle.Regular)

    Private Sub Parse()
        Dim str As String = MaxPath
        Dim x As Long = tile(1).Width + 1, n As Short
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
            If Path(NDirs).width < Me.Height - 6 Then Path(NDirs).width = Me.Height - 6
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
            While Path(NDirs).width > Me.Width - (Path(NDirs - 1).x + Path(NDirs - 1).width + tile(2).Width + tile(4).Width) - tile(9).Width - 9 And Path(NDirs).name_min.Length > 3
                Path(NDirs).name_min = Mid(Path(NDirs).name_min, 1, Path(NDirs).name_min.Length - 4) + "..."
                Path(NDirs).width = graph.MeasureString(Path(NDirs).name_min, text_font).Width + 1
            End While
            If Path(NDirs).width < Me.Height - 6 Then Path(NDirs).width = Me.Height - 6
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
            If Path(i).width < Me.Height - 6 Then Path(i).width = Me.Height - 6
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

    Dim BGColorDouble As Double = 245
    Dim BGColor As Color = Color.FromArgb(245, 245, 245)
    Dim FrameColor As New Pen(Color.FromArgb(70, 70, 70))
    Dim UnderSelectedColor As New SolidBrush(Color.FromArgb(110, 110, 110)) '149, 149, 149))
    Private Sub Redraw()
        If TilesLoaded And Me.Width > 1 Then
            If TextMode Then
                RedrawInTextMode()
            Else
                Dim x As Long = 0, y As Short = 2
                graph = Graphics.FromImage(bmp)

                graph.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                graph.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                graph.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                graph.Clear(BGColor)

                'If UnderSelectedColor.Color.R = 19 Then
                'graph.DrawImageUnscaled(tile(11), x, 0)
                'Else
                graph.FillRectangle(UnderSelectedColor, 0, 1, tile(1).Width, 29 - 2)
                graph.DrawImageUnscaled(tile(1), x, 0)
                'End If
                Dim k As Short
                Dim a As SolidBrush
                For i As Short = 1 To NDirsMax
                    x = Path(i).x
                    If i <= NDirs Then
                        graph.FillRectangle(UnderSelectedColor, x - 1, 0, Path(i).width + 9, 29)
                        graph.DrawLine(New Pen(BGColor), x, 6, x, Me.Height - 1 - 6)
                        a = New SolidBrush(Color.FromArgb(255 - k * 12, 255 - k * 12, 255 - k * 12))
                    Else
                        graph.DrawLine(New Pen(Color.FromArgb(109, 109, 109)), x + 8 + Path(i).width, 6, x + 8 + Path(i).width, Me.Height - 1 - 6)
                        a = New SolidBrush(Color.FromArgb(0 + k * 12, 0 + k * 12, 0 + k * 12))
                    End If

                    If Path(i).width = Me.Height - 6 Then 'And i = 1 Then
                        graph.DrawString(Path(i).name_min, text_font, a, Path(i).x + tile(2 + k).Width + (Me.Height - 6 - graph.MeasureString(Path(i).name_min, text_font).Width) / 2, 7 + y)
                    Else
                        graph.DrawString(Path(i).name_min, text_font, a, Path(i).x + tile(2 + k).Width, 7 + y)
                    End If
                Next

                Dim lastX As Short = Path(NDirs).x + Path(NDirs).width + 8
                If NDirs = 0 Then lastX = tile(1).Width
                If lastX >= Me.Width Then lastX = Me.Width - 1
                bmp.SetPixel(lastX, 1, Color.FromArgb(170, 170, 170))
                graph.DrawLine(New Pen(UnderSelectedColor), lastX, 2, lastX, 27)
                bmp.SetPixel(lastX, 27, Color.FromArgb(170, 170, 170))

                graph.FillRectangle(New SolidBrush(Color.FromArgb(255, BGColor)), Me.Width - tile(9).Width, 0, 39, 29)
                'graph.FillRectangle(New SolidBrush(Color.FromArgb(190, BGColor)), Me.Width - tile(9).Width, 0, 39, 29)
                graph.DrawImageUnscaled(tile(9), Me.Width - tile(9).Width, 0)

                DrawFrame()

                picMain.Image = bmp
                picMain.Refresh()
                Me.Refresh()
            End If
        End If
    End Sub
    Dim fontMain As New Font("Verdana", 8, FontStyle.Regular)
    Private Sub RedrawCustom(ByVal index As Short)
        If TilesLoaded And Me.Width > 1 Then
            Dim x As Long = 0, k As Short, y As Short = 2

            graph.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            graph.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graph.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            graph.Clear(BGColor)

            graph.FillRectangle(UnderSelectedColor, 0, 1, tile(1).Width, 29 - 2)
            graph.DrawImageUnscaled(tile(1), x, 0)
            Dim a As SolidBrush
            For i As Short = 1 To NDirsMax
                x = Path(i).x
                If i <= index Then
                    graph.FillRectangle(UnderSelectedColor, x - 1, 0, Path(i).width + 9, 29)
                    graph.DrawLine(New Pen(BGColor), x, 6, x, Me.Height - 1 - 6)
                    a = New SolidBrush(Color.FromArgb(255 - k * 12, 255 - k * 12, 255 - k * 12))
                Else
                    graph.DrawLine(New Pen(Color.FromArgb(109, 109, 109)), x + 8 + Path(i).width, 6, x + 8 + Path(i).width, Me.Height - 1 - 6)
                    a = New SolidBrush(Color.FromArgb(0 + k * 12, 0 + k * 12, 0 + k * 12))
                End If

                If Path(i).width = Me.Height - 6 Then 'And i = 1 Then
                    graph.DrawString(Path(i).name_min, text_font, a, Path(i).x + tile(2 + k).Width + (Me.Height - 6 - graph.MeasureString(Path(i).name_min, text_font).Width) / 2, 7 + y)
                Else
                    graph.DrawString(Path(i).name_min, text_font, a, Path(i).x + tile(2 + k).Width, 7 + y)
                End If
            Next

            Dim lastX As Short = Path(index).x + Path(index).width + 8
            If index = 0 Then lastX = tile(1).Width
            If lastX >= Me.Width Then lastX = Me.Width - 1
            bmp.SetPixel(lastX, 1, Color.FromArgb(150, 150, 150))
            graph.DrawLine(New Pen(UnderSelectedColor), lastX, 2, lastX, 27)
            bmp.SetPixel(lastX, 27, Color.FromArgb(150, 150, 150))

            'If x > Me.Width - 39 Then
            '    graph.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255)), Me.Width - tile(9).Width, 0, 39, 29)
            'Else
            '    graph.FillRectangle(New SolidBrush(Color.FromArgb(255, BGColor)), Me.Width - tile(9).Width, 0, 39, 29)
            'End If
            graph.DrawImageUnscaled(tile(9), Me.Width - tile(9).Width, 0)

            DrawFrame()

            picMain.Image = bmp
            Me.Refresh()
        End If
    End Sub
    Private Sub DrawFrame()
        graph.DrawRectangle(New Pen(Color.FromArgb(200, 200, 200)), 0, 0, Me.Width - 1, Me.Height - 1)
        graph.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 1, Me.Height - 1, Me.Width - 2, Me.Height - 1)
        graph.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 1, 0, Me.Width - 2, 0)

        graph.DrawImageUnscaled(tile(0), 0, 0)
        graph.DrawImageUnscaled(tile(10), Me.Width - 2, 0)
    End Sub
    Private Sub RedrawInTextMode()
        If TilesLoaded And Me.Width > 1 Then
            Dim pos As Short = 0 ' (245 - BGColorDouble) / 2 '(245 - BGColorDouble) * 4

            graph.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            'graph.Clear(Color.FromArgb((-245 + BGColorDouble) * 4, BGColor))
            Dim Intens As Double = 1 - (BGColorDouble - 245) / 10
            graph.FillRectangle(New SolidBrush(Color.FromArgb(Intens * 255, BGColor)), tile(1).Width + pos, 1, Me.Width - tile(1).Width, Me.Height - 2)
            'graph.FillRectangle(New SolidBrush(Color.FromArgb(Intens, BGColor)), 0, 1, Me.Width, Me.Height - 2)


            Dim att As New Drawing.Imaging.ImageAttributes()
            Dim cm As Drawing.Imaging.ColorMatrix = New Drawing.Imaging.ColorMatrix(New Single()() _
                       {New Single() {1, 0, 0, 0, 0}, _
                        New Single() {0, 1, 0, 0, 0}, _
                        New Single() {0, 0, 1, 0, 0}, _
                        New Single() {0, 0, 0, Intens, 0}, _
                        New Single() {0, 0, 0, 0, 1}})
            att.SetColorMatrix(cm)

            graph.FillRectangle(New SolidBrush(Color.FromArgb(Intens * 255, 19, 130, 206)), txtPath.Left, txtPath.Top, graph.MeasureString(txtPath.Text, fontMain).Width, txtPath.Height)
            graph.DrawString(txtPath.Text, fontMain, New SolidBrush(Color.FromArgb(Intens * 255, Color.White)), txtPath.Left, txtPath.Top)

            Dim r1 As New Rectangle(pos, 0, tile(11).Width, tile(11).Height)
            'Dim r1 As New Rectangle((-245 + BGColorDouble) * 2, 0, tile(11).Width, tile(11).Height)
            graph.DrawImage(tile(11), r1, 0, 0, tile(11).Width, tile(11).Height, GraphicsUnit.Pixel, att)

            r1 = New Rectangle(Me.Width - tile(12).Width + (-245 + BGColorDouble), 0, tile(12).Width, tile(12).Height)
            'r1 = New Rectangle(Me.Width - tile(12).Width + (-245 + BGColorDouble) * 6, 0, tile(12).Width, tile(12).Height)
            'r1 = New Rectangle(Me.Width - tile(12).Width, 0, tile(12).Width, tile(12).Height)
            graph.DrawImage(tile(12), r1, 0, 0, tile(12).Width, tile(12).Height, GraphicsUnit.Pixel, att)

            DrawFrame()

            picMain.Image = bmp
            Me.Refresh()
        End If
    End Sub

    Private Sub PathLine_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim err As Boolean = False
        Me.Height = 29
        txtPath.BackColor = BGColor

        For i As Short = 1 To 9
            If IO.File.Exists(Application.StartupPath + "\path_line_1\" + i.ToString + ".bmp") Then tile(i) = New Bitmap(Application.StartupPath + "\path_line_1\" + i.ToString + ".bmp") Else err = True : Exit For ':MsgBox("NO FILE!! " + Application.StartupPath + "\path_line_1\" + i.ToString + ".bmp") 
        Next
        If IO.File.Exists(Application.StartupPath + "\path_line_1\home_alpha.png") Then tile(1) = New Bitmap(Application.StartupPath + "\path_line_1\home_alpha.png") Else err = True
        If IO.File.Exists(Application.StartupPath + "\path_line_1\edit_alpha.png") Then tile(9) = New Bitmap(Application.StartupPath + "\path_line_1\edit_alpha.png") Else err = True
        If IO.File.Exists(Application.StartupPath + "\path_line_1\back.png") Then tile(11) = New Bitmap(Application.StartupPath + "\path_line_1\back.png") Else err = True
        If IO.File.Exists(Application.StartupPath + "\path_line_1\right_side_2.png") Then tile(12) = New Bitmap(Application.StartupPath + "\path_line_1\right_side_2.png") Else err = True
        If IO.File.Exists(Application.StartupPath + "\frame_left.png") Then tile(0) = New Bitmap(Application.StartupPath + "\frame_left.png") Else err = True
        If IO.File.Exists(Application.StartupPath + "\frame_right.png") Then tile(10) = New Bitmap(Application.StartupPath + "\frame_right.png") Else err = True
        If Not err Then TilesLoaded = True : NewMaxPath("home")
    End Sub

    Private Sub picMain_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picMain.MouseClick
        Dim x As Long = e.X
        Dim str As String = ""
        Dim f As Boolean

        If TextMode Then
            TextMode = False
            NewMaxPath("home")
            RaiseEvent PathChaged(True)
        Else
            If x > Me.Width - 39 Then
                MakeTextMode()
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
                    If x > 0 And x <= tile(1).Width Then
                        NewMaxPath("home")
                        RaiseEvent PathChaged(True)
                    Else
                        MakeTextMode()
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub MakeTextMode()
        TextMode = True
        txtPath.Text = MaxPath
        txtPath.Font = fontMain
        'txtPath.Visible = True
        'txtPath.Select()
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
        If e.Button Then
            If e.Y >= 0 And e.Y <= Me.Height Then
                If tmrMain.Enabled = True Then tmrMain.Enabled = False
                If Not TextMode Then
                    Dim str As String = ""
                    Dim f As Boolean

                    For i As Short = 1 To NDirsMax
                        If e.X > Path(i).x And e.X <= Path(i).x + Path(i).width + tile(2).Width + tile(4).Width Then
                            UnderSelectedColor = New SolidBrush(Color.FromArgb(19, 130, 206))
                            RedrawCustom(i)
                            f = True
                        End If
                    Next
                    If Not f Then
                        If e.X > 0 And e.X <= Path(1).x Then
                            UnderSelectedColor = New SolidBrush(Color.FromArgb(19, 130, 206))
                            RedrawCustom(0)
                        Else
                            BGColor = Color.White : BGColorDouble = 255
                            Redraw()
                        End If
                    End If
                Else
                    'RedrawInTextMode()
                End If
            Else
                'UnderSelectedColor = New SolidBrush(Color.FromArgb(110, 110, 110))
                'BGColor = Color.FromArgb(245, 245, 245) : BGColorDouble = 245
                'Redraw()
                tmrMain.Enabled = True
            End If
        End If
    End Sub

    Private Sub txtPath_HideSelectionChanged(sender As Object, e As EventArgs) Handles txtPath.HideSelectionChanged

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
        If Me.Width > 0 Then
            picMain.Width = Me.Width
            bmp = New Bitmap(Me.Width, 29, Imaging.PixelFormat.Format32bppRgb)
            graph = Graphics.FromImage(bmp)
            graph.Clear(Color.FromArgb(200, 200, 200))
            If TilesLoaded Then Redraw() : txtPath.Width = Me.Width - tile(1).Width - tile(12).Width - 10
            'RedrawInTextMode()
        End If
    End Sub

    Private Sub picMain_MouseDown(sender As Object, e As MouseEventArgs) Handles picMain.MouseDown
        tmrMain.Enabled = False
        If Not TextMode Then
            Dim str As String = ""
            Dim f As Boolean

            For i As Short = 1 To NDirsMax
                If e.X > Path(i).x And e.X <= Path(i).x + Path(i).width + tile(2).Width + tile(4).Width Then
                    UnderSelectedColor = New SolidBrush(Color.FromArgb(19, 130, 206))
                    RedrawCustom(i)
                    f = True
                End If
            Next
            If Not f Then
                If e.X > 0 And e.X <= tile(1).Width Then
                    UnderSelectedColor = New SolidBrush(Color.FromArgb(19, 130, 206))
                    RedrawCustom(0)
                Else
                    BGColor = Color.White : BGColorDouble = 255
                    txtPath.BackColor = BGColor
                    Redraw()
                End If
            End If
        Else
            'BGColor = Color.White
            'txtPath.BackColor = BGColor
            Redraw()
            'RedrawInTextMode()
        End If

        Me.Select()
    End Sub

    Private Sub picMain_MouseUp(sender As Object, e As MouseEventArgs) Handles picMain.MouseUp
        tmrMain.Enabled = True
    End Sub

    Private Sub tmrMain_Tick(sender As Object, e As EventArgs) Handles tmrMain.Tick
        With UnderSelectedColor
            UnderSelectedColor = New SolidBrush(Color.FromArgb(.Color.R + (110 - .Color.R) * 0.08, .Color.G + (110 - .Color.G) * 0.08, .Color.B + (110 - .Color.B) * 0.08))
        End With
        With BGColor
            BGColorDouble += (245 - BGColorDouble) / 6 'BGColorDouble -= 0.5
            BGColor = Color.FromArgb(BGColorDouble, BGColorDouble, BGColorDouble)
            txtPath.BackColor = BGColor
        End With
        If (UnderSelectedColor.Color.R > 102) Then
            UnderSelectedColor = New SolidBrush(Color.FromArgb(110, 110, 110))
        End If
        If (BGColorDouble < 245.5) Then
            BGColor = Color.FromArgb(245, 245, 245) : BGColorDouble = 245 : txtPath.BackColor = BGColor
            If UnderSelectedColor.Color.R = 110 Then tmrMain.Enabled = False
        End If
        If BGColorDouble < 248 And TextMode And txtPath.Visible = False Then txtPath.Visible = True : txtPath.Select()
        Redraw()
    End Sub

    Private Sub picMain_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles picMain.MouseDoubleClick
        picMain_MouseClick(sender, e)
    End Sub

    Private Sub picMain_Click(sender As Object, e As EventArgs) Handles picMain.Click

    End Sub

    Private Sub txtPath_TextChanged(sender As Object, e As EventArgs) Handles txtPath.TextChanged

    End Sub
End Class
