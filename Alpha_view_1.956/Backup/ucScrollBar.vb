Public Class ucScrollBar
    Event ValueChanged(ByVal Value As Long)
    Dim bmp As Bitmap = New Bitmap(Me.Width, Me.Height)
    Public ShowText As Boolean = False
    Public TextSuffix As String '= "px"
    Public p_value As Long = 50
    Public Property value() As Integer
        Get
            value = p_value
        End Get
        Set(ByVal value As Integer)
            p_value = value
            If p_value > max Then p_value = max
            If p_value < min Then p_value = min
            Redraw()
        End Set
    End Property
    Public min As Long = 0
    Public max As Long = 100
    Public bar_width As Long
    Dim mdx As Long
    Dim Tiles(4) As Bitmap, TilesLoaded As Boolean = False
    Public Sub LoadTiles()
        Try
            For i As Short = 0 To 2 '3
                Tiles(i) = Bitmap.FromFile(Application.StartupPath + "\scroll_bar\" + (i + 1).ToString + ".bmp")
            Next
            Tiles(3) = Bitmap.FromFile(Application.StartupPath + "\scroll_bar\4.png")
            TilesLoaded = True
        Catch
        End Try
    End Sub
    Private Sub ucScrollBar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        bmp = New Bitmap(Me.Width, Me.Height)
        bar_width = Me.Height * 1.618
        LoadTiles()
        Redraw()
    End Sub
    Dim b1 As New SolidBrush(Color.FromArgb(174, 174, 174))
    Dim b2 As New SolidBrush(Color.FromArgb(116, 116, 116))
    Public b3 As New SolidBrush(Color.FromArgb(195, 195, 195))
    Dim p1 As New Pen(Color.FromArgb(127, 127, 127))
    Dim FontMain As New Font("Verdana", 8, FontStyle.Regular)
    Sub Redraw()
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(b3.Color)
            If TilesLoaded Then
                g.DrawLine(p1, 2, 0, bmp.Width - 3, 0)
                g.DrawLine(p1, 2, bmp.Height - 1, bmp.Width - 3, bmp.Height - 1)
                Dim pos As Long = (Me.Width - bar_width) * (value - min) / (max - min)
                g.DrawRectangle(Pens.Black, pos, 0, bar_width - 1, bmp.Height - 1)
                g.FillRectangle(b1, 1, 1, pos - 1, bmp.Height - 2)
                g.FillRectangle(b2, pos + 1, 1, bar_width - 2, bmp.Height - 2)

                g.DrawImageUnscaled(Tiles(0), 0, 0)
                g.DrawImageUnscaled(Tiles(3), bmp.Width - 2, 0)
                g.DrawImageUnscaled(Tiles(1), pos, 0)
                g.DrawImageUnscaled(Tiles(2), pos + bar_width - 2, 0)

                If ShowText Then
                    g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                    pos -= g.MeasureString(p_value.ToString + TextSuffix, FontMain).Width / 2 - bar_width / 2
                    g.DrawString(p_value.ToString + TextSuffix, FontMain, b3, pos, 7)
                End If
            Else
                g.DrawRectangle(p1, 0, 0, bmp.Width - 1, bmp.Height - 1)
                Dim pos As Long = (Me.Width - bar_width) * (p_value - min) / (max - min)
                g.DrawRectangle(Pens.Black, pos, 0, bar_width - 1, bmp.Height - 1)
                g.FillRectangle(b1, 1, 1, pos - 1, bmp.Height - 2)
                g.FillRectangle(b2, pos + 1, 1, bar_width - 2, bmp.Height - 2)
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub

    Private Sub ucScrollBar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Dim pos As Long = (Me.Width - bar_width) * (value - min) / (max - min)
        If e.X > pos And e.X < pos + bar_width Then
            mdx = e.X - pos
        Else
            mdx = bar_width / 2
        End If
        p_value = ((e.X - mdx) / (Me.Width - bar_width)) * (max - min) + min
        If p_value > max Then p_value = max
        If p_value < min Then p_value = min
        Redraw()
        RaiseEvent ValueChanged(value)
    End Sub

    Private Sub ucScrollBar_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            p_value = ((e.X - mdx) / (Me.Width - bar_width)) * (max - min) + min
            If p_value > max Then p_value = max
            If p_value < min Then p_value = min
            Redraw()
            RaiseEvent ValueChanged(p_value)
        End If
    End Sub
End Class
