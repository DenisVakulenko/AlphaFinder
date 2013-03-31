Public Class ucFilesFilter
    Private x As Double
    Private area_width As Long, n_areas As Long
    Private selected_area As Long
    Private max_value As Long

    Event ValueChanged()
    Dim bmp As Bitmap = New Bitmap(Me.Width, Me.Height)
    'Public ShowText As Boolean = False
    'Public TextSuffix As String '= "px"

    Public Property value() As Long
        Get
            Return selected_area
        End Get
        Set(ByVal value As Long)
            selected_area = value
            tmrMain.Enabled = True
        End Set
    End Property
    'Public Property value() As Integer
    '    Get
    '        value = p_value

    '    End Get
    '    Set(ByVal value As Integer)
    '        p_value = value
    '        If p_value > max Then p_value = max
    '        If p_value < min Then p_value = min
    '        Redraw()
    '    End Set
    'End Property
    Dim Tiles(10) As Bitmap, TilesLoaded As Boolean = False
    Public Sub LoadTiles()
        Try
            'For i As Short = 0 To 2 '3
            '    Tiles(i) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\" + (i + 1).ToString + ".bmp")
            'Next
            'Tiles(3) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\4.png")
            For i As Short = 1 To 4 '3
                Tiles(i) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\" + (i).ToString + ".bmp")
            Next
            'Tiles(4) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\5.png")
            'Tiles(5) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\6.png")
            'Tiles(6) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\7.png")
            'If IO.File.Exists(Application.StartupPath + "\filter_bar\10.bmp") Then
            '    Tiles(7) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\10.bmp")
            'End If
            Tiles(0) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\0.bmp")
            Tiles(5) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\bg.png")
            TilesLoaded = True
        Catch
        End Try
    End Sub
    Private Sub ucScrollBar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadTiles()

        If TilesLoaded Then
            area_width = Tiles(1).Width - 1
            Me.Width = Tiles(0).Width 'area_width * 3
            max_value = (Tiles(0).Width / area_width) - 1
            Me.Height = Tiles(0).Height
        Else
            area_width = 30
            Me.Width = 90
        End If

        bmp = New Bitmap(Me.Width, Me.Height)

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
                g.DrawImageUnscaled(Tiles(0), 0, 0)

                Dim i As Long = MakeCorrect(Math.Round(x / area_width))

                g.DrawImageUnscaled(Tiles(5), x - 1, 0)
                Dim r1 As New Rectangle(x + 3, 3, Tiles(1).Width - 6, Tiles(1).Height - 6)
                Dim r2 As New Rectangle(3, 3, Tiles(1).Width - 6, Tiles(1).Height - 6)
                g.DrawImage(Tiles(i + 1), r1, r2, GraphicsUnit.Pixel)

                Dim alpha As Double = Math.Abs(i * area_width - x) / 10
                If alpha > 1 Then alpha = 1
                Dim bb As New SolidBrush(Color.FromArgb(alpha * 255, 116, 116, 116))
                g.FillRectangle(bb, CInt(x) + 3, 3, Tiles(1).Width - 6, Tiles(1).Height - 6)
            Else
                g.DrawRectangle(p1, 0, 0, bmp.Width - 1, bmp.Height - 1)
                Dim pos As Long = x
                g.DrawRectangle(Pens.Black, pos, 0, area_width - 1, bmp.Height - 1)
                g.FillRectangle(b1, 1, 1, pos - 1, bmp.Height - 2)
                g.FillRectangle(b2, pos + 1, 1, area_width - 2, bmp.Height - 2)
            End If
        End Using
        Me.BackgroundImage = bmp
        Me.Refresh()
    End Sub

    Private Function MakeCorrect(ByVal i As Long) As Long
        If i < 0 Then i = 0
        If i > max_value Then i = max_value
        Return i
    End Function

    Private Sub tmrMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMain.Tick
        x -= (x - selected_area * area_width) * 0.2
        Redraw()
    End Sub

#Region "mouse"
    Dim mdx As Long
    Private Sub ucScrollBar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        tmrMain.Enabled = False
        If Not (e.X > x And e.X < x + area_width) Then
            x = MakeCorrect(Math.Truncate(e.X / area_width)) * area_width
        End If
        mdx = e.X - x

        Redraw()
        'RaiseEvent ValueChanged(selected_area)
    End Sub

    Private Sub ucScrollBar_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            x = e.X - mdx
            If x > Me.Width - area_width - 1 Then x = Me.Width - area_width - 1
            If x < 0 Then x = 0
            Redraw()
        End If
    End Sub
    Private Sub ucFilesFilter_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        selected_area = MakeCorrect(Math.Round(x / area_width))
        RaiseEvent ValueChanged()

        tmrMain.Enabled = True
    End Sub
#End Region
End Class