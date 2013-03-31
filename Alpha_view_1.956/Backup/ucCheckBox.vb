Public Class ucCheckBox
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

    Dim Tiles(2) As Bitmap, TilesLoaded As Boolean = False
    Public Sub LoadTiles()
        Try
            For i As Short = 0 To 1
                Tiles(i) = Bitmap.FromFile(Application.StartupPath + "\check_box\" + (i).ToString + ".png")
            Next
            TilesLoaded = True
        Catch
        End Try
    End Sub

    Dim FontMain As New Font("Verdana", 8, FontStyle.Regular)
    Sub RedrawMM()
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Me.BackColor)
            If TilesLoaded Then
                g.DrawLine(Pens.Gray, 24, 0, bmp.Width - 2, 0)
                g.DrawLine(Pens.Gray, 24, bmp.Height - 1, bmp.Width - 2, bmp.Height - 1)
                g.DrawLine(Pens.Gray, bmp.Width - 1, 1, bmp.Width - 1, bmp.Height - 2)
                If value Then
                    g.DrawImageUnscaled(Tiles(1), 0, 0)
                Else
                    g.DrawImageUnscaled(Tiles(0), 0, 0)
                End If

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, 30, 7)
                Me.Width = 37 + g.MeasureString(p_text.ToString, FontMain).Width
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
                Else
                    g.DrawImageUnscaled(Tiles(0), 0, 0)
                End If

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, 30, 7)
                Me.Width = 37 + g.MeasureString(p_text.ToString, FontMain).Width
            Else
                g.DrawRectangle(Pens.Black, 0, 0, 24, 24)

                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(p_text, FontMain, Brushes.Black, 30, 7)
                Me.Width = 37 + g.MeasureString(p_text.ToString, FontMain).Width
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub

    Private Sub ucCheckBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        p_value = Not (p_value)
        Redraw()
        RaiseEvent ValueChanged(p_value)
    End Sub
    Private Sub ucCheckBox_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        p_value = Not (p_value)
        Redraw()
        RaiseEvent ValueChanged(p_value)
    End Sub

    Private Sub ucCheckBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'bmp = New Bitmap(Me.Width, Me.Height)
        LoadTiles()
        Redraw()
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
End Class
