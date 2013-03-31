Public Class ucSkinButton
    Public Shadows Event Clicked()
    Private Picture As Button_pic_class
    Dim Borders As Boolean = True

    Public Function GetMDPic() As Bitmap
        Return Picture.mouse_down.Clone
    End Function

    Public Sub LoadImages(ByVal path_ico As String)
        Dim bmp As Bitmap
        Try
            bmp = Image.FromFile(Application.StartupPath + "\" + path_ico)
            Picture.mouse_move = New Bitmap(bmp.Width, bmp.Height)
            Picture.mouse_down = New Bitmap(bmp.Width, bmp.Height)
            Picture.normal = New Bitmap(bmp.Width, bmp.Height)
            Me.Size = bmp.Size

            Dim ColorExternal As Color
            Dim ColorInternal As Color
            Using g As Graphics = Graphics.FromImage(Picture.mouse_down)
                g.Clear(Color.FromArgb(19, 130, 206))
                g.DrawImageUnscaled(bmp, 0, 0) '1
                'g.FillRectangle(New SolidBrush(Color.FromArgb(100, 93, 93, 93)), 3, 3, bmp.Width - 6, bmp.Height - 6)
                'g.DrawRectangle(New Pen(Color.FromArgb(160, 0, 0, 0), 5), 0, 0, Me.Width - 1, Me.Height - 1)
                If Borders Then
                    Dim p As New Pen(Color.FromArgb(130, 0, 0, 0))
                    g.DrawLine(p, 0, 1, 0, Me.Height - 2)
                    g.DrawLine(p, Me.Width - 1, 1, Me.Width - 1, Me.Height - 2)
                    g.DrawLine(p, 1, Me.Height - 1, Me.Width - 2, Me.Height - 1)
                    g.DrawLine(p, 1, 0, Me.Width - 2, 0)
                    ColorExternal = Color.FromArgb(80, 0, 0, 0)
                    ColorInternal = Color.FromArgb(19 * 0.8, 130 * 0.8, 206 * 0.8)
                Else
                    ColorExternal = Color.FromArgb(80, Color.FromArgb(19, 130, 206))
                    ColorInternal = Color.FromArgb(255, Color.FromArgb(19, 130, 206))
                End If
                With Picture.mouse_down
                    .SetPixel(0, 0, ColorExternal)
                    .SetPixel(1, 1, ColorInternal)
                    .SetPixel(.Width - 1, Me.Height - 1, ColorExternal)
                    .SetPixel(.Width - 2, Me.Height - 2, ColorInternal)
                    .SetPixel(.Width - 1, 0, ColorExternal)
                    .SetPixel(.Width - 2, 1, ColorInternal)
                    .SetPixel(0, Me.Height - 1, ColorExternal)
                    .SetPixel(1, Me.Height - 2, ColorInternal)
                End With
            End Using

            If IO.File.Exists(Application.StartupPath + "\" + Mid(path_ico, 1, path_ico.Length - 4) + "_negative.png") Then
                bmp = New Bitmap(Application.StartupPath + "\" + Mid(path_ico, 1, path_ico.Length - 4) + "_negative.png")
            End If
            Using g As Graphics = Graphics.FromImage(Picture.normal)
                g.DrawImageUnscaled(bmp, 0, 0)
            End Using
            Using g As Graphics = Graphics.FromImage(Picture.mouse_move)
                g.FillRectangle(New SolidBrush(Color.FromArgb(240, 240, 240)), 0, 0, Me.Width, Me.Height)
                g.DrawImageUnscaled(bmp, 0, 0)
                If Borders = True Then
                    g.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 0, 1, 0, Me.Height - 2)
                    g.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), Me.Width - 1, 1, Me.Width - 1, Me.Height - 2)
                    g.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 1, Me.Height - 1, Me.Width - 2, Me.Height - 1)
                    g.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 1, 0, Me.Width - 2, 0)
                    ColorExternal = Color.FromArgb(80, 70, 70, 70)
                    ColorInternal = Color.FromArgb(80, 70, 70, 70)
                Else
                    ColorExternal = Color.FromArgb(80, 240, 240, 240)
                    ColorInternal = Color.FromArgb(255, 240, 240, 240)
                    'ColorExternal = Color.FromArgb(80, 19, 130, 206) 
                    'ColorInternal = Color.FromArgb(80, 19, 130, 206) 
                    'g.DrawRectangle(New Pen(Color.FromArgb(19, 130, 206), 1), 0, 0, Me.Width - 1, Me.Height - 1)
                End If

                With Picture.mouse_move
                    .SetPixel(0, 0, ColorExternal)
                    .SetPixel(1, 1, ColorInternal)
                    .SetPixel(.Width - 1, Me.Height - 1, ColorExternal)
                    .SetPixel(.Width - 2, Me.Height - 2, ColorInternal)
                    .SetPixel(.Width - 1, 0, ColorExternal)
                    .SetPixel(.Width - 2, 1, ColorInternal)
                    .SetPixel(0, Me.Height - 1, ColorExternal)
                    .SetPixel(1, Me.Height - 2, ColorInternal)
                End With
            End Using

            Me.BackgroundImage = Picture.normal
            'Me.TabStop = False
            Me.Visible = True
        Catch ex As Exception
            MsgBox("Loading failed!  " + path_ico + ex.ToString)
        End Try
    End Sub
    Public Sub LoadImages(ByVal path_normal As String, ByVal path_move As String, ByVal path_down As String, ByVal path_ico As String)
        Try
            Picture.normal = Image.FromFile(Application.StartupPath + "\" + path_normal)
            Picture.mouse_move = Image.FromFile(Application.StartupPath + "\" + path_move)
            Picture.mouse_down = Image.FromFile(Application.StartupPath + "\" + path_down)
            Dim bmp As Bitmap = Image.FromFile(Application.StartupPath + "\" + path_ico)

            Using g As Graphics = Graphics.FromImage(Picture.normal)
                g.DrawImageUnscaled(bmp, 0, 0)
            End Using
            Using g As Graphics = Graphics.FromImage(Picture.mouse_down)
                g.DrawImageUnscaled(bmp, 0, 1)
                g.FillRectangle(New SolidBrush(Color.FromArgb(100, 93, 93, 93)), 3, 3, bmp.Width - 6, bmp.Height - 6)
            End Using
            Using g As Graphics = Graphics.FromImage(Picture.mouse_move)
                g.DrawImageUnscaled(bmp, 0, 0)
            End Using
        Catch ex As Exception
            MsgBox("Fail!  " + path_normal)
        End Try

        Me.BackgroundImage = Picture.normal
        Me.Size = Picture.normal.Size
        'Me.TabStop = False
        Me.Visible = True
    End Sub

    Public Sub btnDown()
        Me.BackgroundImage = Picture.mouse_down
    End Sub
    Public Sub btnNormal()
        Me.BackgroundImage = Picture.normal
    End Sub

    Private Sub ButtonP_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        btnDown()
    End Sub
    Private Sub ButtonP_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Me.BackgroundImage = Picture.normal
    End Sub
    Private Sub ButtonP_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button = Windows.Forms.MouseButtons.None Then
            Me.BackgroundImage = Picture.mouse_move
        Else
            btnDown()
        End If
    End Sub

    Private Sub pic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Click
        RaiseEvent Clicked()
    End Sub
    Private Sub pic_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        RaiseEvent Clicked()
    End Sub
End Class
