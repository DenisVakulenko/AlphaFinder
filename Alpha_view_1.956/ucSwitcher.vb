Public Class ucSwitcher
    Event ValueChanged(ByVal Value As Boolean)
    Dim bmp As Bitmap = New Bitmap(Me.Width, Me.Height)
    Public p_text As String = ""
    Dim p_value As Boolean = False
    Public Property value() As Boolean
        Get
            value = p_value
        End Get
        Set(ByVal value As Boolean)
            p_value = value
            Redraw()
            RaiseEvent ValueChanged(p_value)
        End Set
    End Property
    Public Property caption() As String
        Get
            caption = p_text
        End Get
        Set(ByVal caption As String)
            p_text = caption
            Redraw()
        End Set
    End Property

    Dim Tiles(5) As Bitmap, TilesLoaded As Boolean = False
    Public Sub LoadCustomTiles(FName As String)
        Try
            Tiles(4) = Bitmap.FromFile(Application.StartupPath + "\check_box\" + FName + "_on.png")
            Tiles(5) = Bitmap.FromFile(Application.StartupPath + "\check_box\" + FName + "_off.png")
            TilesLoaded = True
            Redraw()
            Me.Size = Tiles(4).Size
        Catch
        End Try
    End Sub
    Public Sub LoadTiles()
        Try
            Tiles(0) = Bitmap.FromFile(Application.StartupPath + "\check_box\mm.png")
            Tiles(1) = Bitmap.FromFile(Application.StartupPath + "\check_box\on.png")
            Tiles(2) = Bitmap.FromFile(Application.StartupPath + "\check_box\mm_on.png")
            Tiles(3) = Bitmap.FromFile(Application.StartupPath + "\check_box\mm_off.png")
            'TilesLoaded = True
            Me.Height = Tiles(0).Height
        Catch
        End Try
    End Sub

    Dim FontMain As New Font("Verdana", 8, FontStyle.Regular)
    Sub RedrawMD()
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Me.BackColor)
            If TilesLoaded Then
                If value Then
                    g.DrawImageUnscaled(Tiles(2), 0, 0)
                    g.DrawImageUnscaled(Tiles(4), 0, 0)
                Else
                    g.DrawImageUnscaled(Tiles(3), 0, 0)
                    g.DrawImageUnscaled(Tiles(5), 0, 0)
                End If

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, Tiles(0).Width + 4, 9)
                'Me.Width = Tiles(0).Width + 4 + g.MeasureString(p_text.ToString, FontMain).Width + 7
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub
    Sub RedrawMM()
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Me.BackColor)
            If TilesLoaded Then
                If value Then
                    g.DrawImageUnscaled(Tiles(1), 0, 0)
                    g.DrawImageUnscaled(Tiles(4), 0, 0)
                Else
                    g.DrawImageUnscaled(Tiles(0), 0, 0)
                    g.DrawImageUnscaled(Tiles(5), 0, 0)
                End If

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, Tiles(0).Width + 4, 9)
                'Me.Width = Tiles(0).Width + 4 + g.MeasureString(p_text.ToString, FontMain).Width + 7
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub
    Sub Redraw()
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Me.BackColor)
            If TilesLoaded Then
                If value Then
                    g.DrawImageUnscaled(Tiles(1), 0, 0)
                    g.DrawImageUnscaled(Tiles(4), 0, 0)
                Else
                    g.FillRectangle(New SolidBrush(Color.FromArgb(200, 200, 200)), 1, 1, Tiles(1).Width - 2, Tiles(1).Height - 2)
                    g.DrawImageUnscaled(Tiles(5), 0, 0)
                End If

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, Tiles(0).Width + 4, 9)
                'Me.Width = Tiles(0).Width + 4 + g.MeasureString(p_text.ToString, FontMain).Width + 7
            Else
                g.DrawRectangle(Pens.Black, 0, 0, 24, 24)

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, 30, 7)
                'Me.Width = 37 + g.MeasureString(p_text.ToString, FontMain).Width
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub

    Private Sub ucCheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        value = Not (value)
    End Sub
    Private Sub ucCheckBox_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        value = Not (value)
    End Sub

    Private Sub ucCheckBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'bmp = New Bitmap(Me.Width, Me.Height)
        LoadTiles()
    End Sub

    Private Sub ucCheckBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        RedrawMM()
    End Sub

    Private Sub ucCheckBox_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Redraw()
    End Sub

    Private Sub ucCheckBox_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        bmp = New Bitmap(Me.Width, Me.Height)
        Redraw()
    End Sub

    Private Sub ucSwitcher_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        RedrawMD()
    End Sub
End Class
