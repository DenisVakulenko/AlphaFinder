Public Class ucFilesFilter
    Public pMusicCount As Long = 0
    Public pAllCount As Long = 0
    Public pImagesCount As Long = 0
    Public pCustomCount As Long = 0

    Private x As Double
    Private area_width As Long, n_areas As Long
    Private selected_area As Long
    Private max_value As Long = 3

    Event ValueChanged()
    Dim bmp As Bitmap = New Bitmap(Me.Width, Me.Height)

    Public Property MusicCount() As Long
        Get
            Return pMusicCount
        End Get
        Set(value As Long)
            pMusicCount = value
            Redraw(False)
        End Set
    End Property
    Public Property AllCount() As Long
        Get
            Return pAllCount
        End Get
        Set(value As Long)
            pAllCount = value
            Redraw(False)
        End Set
    End Property
    Public Property ImagesCount() As Long
        Get
            Return pImagesCount
        End Get
        Set(value As Long)
            pImagesCount = value
            Redraw(False)
        End Set
    End Property
    Public Property CustomCount() As Long
        Get
            Return pCustomCount
        End Get
        Set(value As Long)
            pCustomCount = value
            Redraw(False)
        End Set
    End Property

    Public Property value() As Long
        Get
            Return selected_area
        End Get
        Set(ByVal value As Long)
            If value > max_value Then value = max_value
            If value < 0 Then value = 0
            If selected_area <> value Then
                BaseColor = Color.FromArgb(255, 19, 130, 206) : BaseColorFade = False
                selected_area = value
                tmrMain.Enabled = True
            End If
        End Set
    End Property

    Dim Tiles(10) As Bitmap, TilesLoaded As Boolean = False

    'maybe goood color(17, 72, 164)
    Public Sub LoadTiles()
        Try
            For i As Short = 0 To 4
                Tiles(i) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\" + (i).ToString + ".png")
            Next
            Tiles(5) = Bitmap.FromFile(Application.StartupPath + "\filter_bar\bg.png")
            TilesLoaded = True
            If IO.File.Exists(Application.StartupPath + "\frame_left.png") Then Tiles(6) = New Bitmap(Application.StartupPath + "\frame_left.png") Else TilesLoaded = False
            If IO.File.Exists(Application.StartupPath + "\frame_right.png") Then Tiles(7) = New Bitmap(Application.StartupPath + "\frame_right.png") Else TilesLoaded = False
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

        Redraw(False)
    End Sub
    Dim b1 As New SolidBrush(Color.FromArgb(174, 174, 174))
    Dim b2 As New SolidBrush(Color.FromArgb(116, 116, 116))
    Public b3 As New SolidBrush(Color.FromArgb(195, 195, 195))
    Dim p1 As New Pen(Color.FromArgb(127, 127, 127))
    Dim FontMain As New Font("Verdana", 8, FontStyle.Regular)

    Dim BaseColor As Color = Color.FromArgb(255, 110, 110, 110)

    Sub Redraw(ByVal MD As Boolean)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(b3.Color)
            If TilesLoaded Then
                g.DrawImageUnscaled(Tiles(0), 0, 0)

                Dim f As New Font("verdana", 5, FontStyle.Regular)
                Dim txt_brush As New SolidBrush(Color.FromArgb(100, 100, 100))
                Dim txt_left As Long = 2 '27
                Dim txt_top As Long = 20
                'g.TextContrast = 10
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                g.DrawString(AllCount.ToString, f, txt_brush, txt_left, txt_top)
                g.DrawString(ImagesCount.ToString, f, txt_brush, txt_left + 38, txt_top)
                g.DrawString(MusicCount.ToString, f, txt_brush, txt_left + 38 + 38, txt_top)

                Dim i As Long = MakeCorrect(Math.Round(x / area_width))
                Dim alpha As Double = Math.Abs(i * area_width - x) / 14

                Dim brush1 As New SolidBrush(Color.FromArgb(160, 245, 245, 245))
                If pAllCount = 0 Then
                    If i = 0 Then alpha += 0.5
                    g.FillRectangle(brush1, 1, 1, Tiles(1).Width - 2, Tiles(1).Height - 2)
                End If
                If pImagesCount = 0 Then
                    If i = 1 Then alpha += 0.5
                    g.FillRectangle(brush1, 1 + 38, 1, Tiles(1).Width - 2, Tiles(1).Height - 2)
                End If
                If pMusicCount = 0 Then
                    If i = 2 Then alpha += 0.5
                    g.FillRectangle(brush1, 1 + 38 + 38, 1, Tiles(1).Width - 2, Tiles(1).Height - 2)
                End If
                If pCustomCount = 0 Then
                    If i = 3 Then alpha += 0.5
                    g.FillRectangle(brush1, 1 + 38 + 38 + 38, 1, Tiles(1).Width - 2, Tiles(1).Height - 2)
                End If

                Dim bb As New SolidBrush(BaseColor)
                g.FillRectangle(bb, CInt(x), 0, Tiles(1).Width, Tiles(1).Height)
                bmp.SetPixel(CInt(x), 1, Color.FromArgb(80, bb.Color))
                bmp.SetPixel(CInt(x), Tiles(1).Height - 2, Color.FromArgb(80, bb.Color))
                bmp.SetPixel(CInt(x) + Tiles(1).Width - 1, 1, Color.FromArgb(80, bb.Color))
                bmp.SetPixel(CInt(x) + Tiles(1).Width - 1, Tiles(1).Height - 2, Color.FromArgb(80, bb.Color))

                Dim r1 As New Rectangle(x + 3, 3, Tiles(1).Width - 6, Tiles(1).Height - 6)
                Dim r2 As New Rectangle(3, 3, Tiles(1).Width - 6, Tiles(1).Height - 6)
                g.DrawImage(Tiles(i + 1), r1, r2, GraphicsUnit.Pixel)

                txt_left = x + 2
                Select Case i
                    Case 0
                        g.DrawString(pAllCount.ToString, f, Brushes.White, txt_left, txt_top)
                    Case 1
                        g.DrawString(pImagesCount.ToString, f, Brushes.White, txt_left, txt_top)
                    Case 2
                        g.DrawString(pMusicCount.ToString, f, Brushes.White, txt_left, txt_top)
                End Select

                If alpha > 1 Then alpha = 1
                bb = New SolidBrush(Color.FromArgb(alpha * 255, BaseColor))
                g.FillRectangle(bb, CInt(x) + 3, 3, Tiles(1).Width - 6, Tiles(1).Height - 5)

                g.DrawRectangle(New Pen(Color.FromArgb(200, 200, 200)), 0, 0, Me.Width - 1, Me.Height - 1)
                g.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 1, Me.Height - 1, Me.Width - 2, Me.Height - 1)
                g.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 1, 0, Me.Width - 2, 0)

                g.DrawImageUnscaled(Tiles(6), 0, 0)
                g.DrawImageUnscaled(Tiles(7), Me.Width - 2, 0)
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

    Dim BaseColorFade As Boolean = True
    Private Sub tmrMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMain.Tick
        x -= (x - selected_area * area_width) * 0.2
        If BaseColorFade Then BaseColor = Color.FromArgb(BaseColor.R + CDbl((110 - BaseColor.R) * 0.08), _
                                      BaseColor.G + CDbl((110 - BaseColor.G) * 0.08), _
                                      BaseColor.B + CDbl((110 - BaseColor.B) * 0.08))
        'If BaseColorFade Then BaseColor = Color.FromArgb(BaseColor.R + Math.Sign((110 - BaseColor.R)), _
        '                               BaseColor.G + Math.Sign((110 - BaseColor.G)), _
        '                               BaseColor.B + Math.Sign((110 - BaseColor.B)))
        If Math.Abs(x - selected_area * area_width) < 0.3 Then
            BaseColorFade = True
            x = selected_area * area_width
            If Math.Abs(BaseColor.R - 110) < 10 And Math.Abs(BaseColor.G - 110) < 10 And Math.Abs(BaseColor.B - 110) < 10 Then tmrMain.Enabled = False : BaseColor = Color.FromArgb(110, 110, 110)
        End If
        Redraw(False)
    End Sub

#Region "mouse"
    Dim mdx As Long
    Dim firstMX As Short
    Private Sub ucScrollBar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        tmrMain.Enabled = False
        If Not (e.X > x And e.X < x + area_width) Then
            selected_area = MakeCorrect(Math.Truncate(e.X / area_width))
            tmrMain.Enabled = True
            RaiseEvent ValueChanged()
            BaseColorFade = False
        End If
        mdx = e.X - x
        firstMX = e.X

        BaseColor = Color.FromArgb(255, 19, 130, 206)
        Redraw(True)
    End Sub

    Private Sub ucScrollBar_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If e.Button <> Windows.Forms.MouseButtons.None Then
            If tmrMain.Enabled = False Then
                If e.X - mdx <> x Then
                    x = e.X - mdx
                    If x > Me.Width - area_width - 1 Then x = Me.Width - area_width - 1
                    If x < 0 Then x = 0
                    Redraw(True)
                End If
            Else
                If (e.X > x And e.X < x + area_width) And Math.Abs(firstMX - e.X) > 1 Then
                    tmrMain.Enabled = False
                    mdx = e.X - x
                    BaseColor = Color.FromArgb(255, 19, 130, 206)
                End If
            End If
        End If
    End Sub
    Private Sub ucFilesFilter_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If tmrMain.Enabled = False Then
            selected_area = MakeCorrect(Math.Round(x / area_width))
            RaiseEvent ValueChanged()

            tmrMain.Enabled = True
            BaseColorFade = True
        End If
    End Sub
#End Region
End Class