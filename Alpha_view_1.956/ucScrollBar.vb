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
            If TilesLoaded Then RaiseEvent ValueChanged(value)
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
        bar_width = 39 * Me.Height / 29 '* 1.3 '1.618
        'LoadTiles()
        Redraw()
    End Sub
    Public b1 As New SolidBrush(Color.FromArgb(174, 174, 174))
    Public b2 As New SolidBrush(Color.FromArgb(130, 130, 130))
    Public b3 As New SolidBrush(Color.FromArgb(195, 195, 195))
    Dim p1 As New Pen(Color.FromArgb(70, 70, 70))
    Dim FontMain As New Font("Verdana", 8, FontStyle.Regular)
    Public Pic As Bitmap
    Public IsPic As Boolean = False
    Dim md As Boolean = False
    Sub Redraw()
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(b3.Color)
            If TilesLoaded Then
                g.DrawLine(p1, 2, 0, bmp.Width - 3, 0)
                g.DrawLine(p1, 2, bmp.Height - 1, bmp.Width - 3, bmp.Height - 1)
                Dim pos As Long = (Me.Width - bar_width) * (value - min) / (max - min)
                g.DrawRectangle(New Pen(Color.FromArgb(70, 70, 70)), pos, 0, bar_width - 1, bmp.Height - 1)
                g.FillRectangle(b1, 1, 1, pos - 1, bmp.Height - 2)
                g.FillRectangle(b2, pos + 1, 1, bar_width - 2, bmp.Height - 2)

                'g.DrawImageUnscaled(Tiles(0), 0, 0)
                'g.DrawImageUnscaled(Tiles(3), bmp.Width - 2, 0)
                'g.DrawImageUnscaled(Tiles(1), pos, 0)
                'g.DrawImageUnscaled(Tiles(2), pos + bar_width - 2, 0)

                If ShowText Then
                    g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                    pos -= g.MeasureString(p_value.ToString + TextSuffix, FontMain).Width / 2 - bar_width / 2
                    g.DrawString(p_value.ToString + TextSuffix, FontMain, b3, pos, 7)
                End If
                g.DrawRectangle(New Pen(Color.FromArgb(70, 70, 70)), 0, 0, bmp.Width - 1, bmp.Height - 1)
            Else
                g.Clear(Me.BackColor) 'Color.FromArgb(200, 200, 200))
                Dim IndicatorWidth As Short = 9
                Dim IndicatorOffset As Short = (bmp.Height - IndicatorWidth) / 2
                Dim IndicatorColor As Color = Color.FromArgb(110, 110, 110) 'Color.FromArgb(19, 130, 206) 
                Dim pos As Long
                If IsPic Then
                    pos = (Me.Width - Pic.Width) * (p_value - min) / (max - min)
                    g.DrawImageUnscaled(Pic, pos, 0)
                    g.FillRectangle(New SolidBrush(IndicatorColor), 0, IndicatorOffset, pos, IndicatorWidth)
                    g.FillRectangle(Brushes.White, pos + Pic.Width, IndicatorOffset, bmp.Width - pos - Pic.Width, IndicatorWidth)
                    g.DrawLine(New Pen(Color.FromArgb(120, 0, 0, 0)), pos - 1, IndicatorOffset, pos - 1, IndicatorOffset + IndicatorWidth - 1)
                    g.DrawLine(New Pen(Color.FromArgb(40, 0, 0, 0)), pos - 2, IndicatorOffset, pos - 2, IndicatorOffset + IndicatorWidth - 1)
                    g.DrawLine(New Pen(Color.FromArgb(70, 0, 0, 0)), 0, IndicatorOffset + 1, 0, IndicatorOffset + IndicatorWidth - 1 - 1)
                    g.DrawLine(New Pen(Color.FromArgb(70, 0, 0, 0)), 1, IndicatorOffset + IndicatorWidth - 1, pos - 1, IndicatorOffset + IndicatorWidth - 1)
                    g.DrawLine(New Pen(Color.FromArgb(70, 0, 0, 0)), 1, IndicatorOffset, pos - 1, IndicatorOffset)
                Else
                    pos = (Me.Width - bar_width) * (p_value - min) / (max - min)

                    If md Then IndicatorColor = Color.FromArgb(19, 130, 206) Else IndicatorColor = Color.FromArgb(110, 110, 110)
                    Dim kk As Double = 1 '0.85
                    'If md Then IndicatorColor = Color.FromArgb(19 * kk, 130 * kk, 206 * kk) Else IndicatorColor = Color.FromArgb(19, 130, 206)


                    g.FillRectangle(New SolidBrush(IndicatorColor), 0, IndicatorOffset, pos, IndicatorWidth)
                    If md Then
                        g.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255)), pos + bar_width, IndicatorOffset, bmp.Width - pos - bar_width, IndicatorWidth)
                    Else
                        g.FillRectangle(New SolidBrush(Color.FromArgb(245, 245, 245)), pos + bar_width, IndicatorOffset, bmp.Width - pos - bar_width, IndicatorWidth)
                    End If
                    'g.DrawLine(New Pen(Color.FromArgb(120, 0, 0, 0)), pos - 1, IndicatorOffset, pos - 1, IndicatorOffset + IndicatorWidth - 1)
                    'g.DrawLine(New Pen(Color.FromArgb(40, 0, 0, 0)), pos - 2, IndicatorOffset, pos - 2, IndicatorOffset + IndicatorWidth - 1)
                    RectPoints(bmp, New Rectangle(0, IndicatorOffset, bmp.Width, IndicatorWidth))
                    'pos += bar_width - 1  'Shadow in the right side
                    'g.DrawLine(New Pen(Color.FromArgb(120, 0, 0, 0)), pos + 1, IndicatorOffset, pos + 1, IndicatorOffset + IndicatorWidth - 1)
                    'g.DrawLine(New Pen(Color.FromArgb(40, 0, 0, 0)), pos + 2, IndicatorOffset, pos + 2, IndicatorOffset + IndicatorWidth - 1)
                    'pos -= bar_width - 1


                    kk = 1
                    'If md Then b2.Color = Color.FromArgb(19 * kk, 130 * kk, 206 * kk) Else b2.Color = Color.FromArgb(75, 75, 75)
                    If md Then b2.Color = Color.FromArgb(75 * kk, 75 * kk, 75 * kk) Else b2.Color = Color.FromArgb(75, 75, 75)

                    Dim Borders = False 'True
                    If Borders Then
                        Dim k As Double = 0.9
                        Dim scrollerBorderColor As Color = Color.FromArgb(b2.Color.R * k, b2.Color.G * k, b2.Color.B * k) : k = 1 - (1 - k) / 2
                        g.FillRectangle(b2, pos, 0, bar_width, bmp.Height)
                        g.DrawRectangle(New Pen(scrollerBorderColor), pos, 0, bar_width - 1, bmp.Height - 1)
                        RectPoints(bmp, New Rectangle(pos, 0, bar_width, bmp.Height))
                        RectPoints(bmp, New Rectangle(pos + 1, 0 + 1, bar_width - 2, bmp.Height - 2), Color.FromArgb(b2.Color.R * k, b2.Color.G * k, b2.Color.B * k))
                    Else
                        g.FillRectangle(b2, pos, 0, bar_width, bmp.Height)
                        RectPoints(bmp, New Rectangle(pos, 0, bar_width, bmp.Height))
                    End If

                    If ShowText Then
                        Dim TextTop As Short = (Me.Height - FontMain.Height) / 2
                        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                        pos -= g.MeasureString(p_value.ToString + TextSuffix, FontMain).Width / 2 - bar_width / 2
                        If md Then
                            g.DrawString(p_value.ToString + TextSuffix, FontMain, Brushes.White, pos, TextTop)
                        Else
                            g.DrawString(p_value.ToString + TextSuffix, FontMain, b3, pos, TextTop)
                        End If
                    End If
                End If
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub
    Private Sub RectPoints(bmp As Bitmap, rect As Rectangle, color As Color)
        Try
            With bmp
                .SetPixel(rect.X + 0, rect.Y + 0, color)
                .SetPixel(rect.X + rect.Width - 1, rect.Y + rect.Height - 1, color)
                .SetPixel(rect.X + rect.Width - 1, rect.Y + 0, color)
                .SetPixel(rect.X + 0, rect.Y + rect.Height - 1, color)
            End With
        Catch
        End Try
    End Sub
    Private Sub RectPoints(bmp As Bitmap, rect As Rectangle)
        Try
            With bmp
                Dim ColorExternal As Color = Color.FromArgb(100, .GetPixel(rect.X + 0, rect.Y + 0)) 'b2.Color)
                .SetPixel(rect.X + 0, rect.Y + 0, ColorExternal)
                ColorExternal = Color.FromArgb(100, .GetPixel(rect.X + rect.Width - 1, rect.Y + rect.Height - 1))
                .SetPixel(rect.X + rect.Width - 1, rect.Y + rect.Height - 1, ColorExternal)
                ColorExternal = Color.FromArgb(100, .GetPixel(rect.X + rect.Width - 1, rect.Y + 0))
                .SetPixel(rect.X + rect.Width - 1, rect.Y + 0, ColorExternal)
                ColorExternal = Color.FromArgb(100, .GetPixel(rect.X + 0, rect.Y + rect.Height - 1))
                .SetPixel(rect.X + 0, rect.Y + rect.Height - 1, ColorExternal)
            End With
        Catch
        End Try
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
        md = True
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

    Private Sub ucScrollBar_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        md = False
        Redraw()
    End Sub
End Class
