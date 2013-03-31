Public Class frmMMMenu
    Public MainText As String
    Public LeftText As String
    Public LeftTextLeft As Short
    Public RightText As String
    Public RightTextLeft As Short
    Public NItems As Short = -1

    Public StartX As Short = -1, StartY As Short = -1

    Public Ansver As Short = 0

    Private w As Short = 60
    Private h As Short = 60
    Private textTop As Short

    Private bmpMain As Bitmap
    Private g As Graphics

    'Private fontMain As New Font("Lucida Sans Unicode", 10, FontStyle.Bold)
    Private fontMain As New Font("Segoe UI", 10, FontStyle.Regular)
    Private fontMini As New Font("Lucida Sans Unicode", 7)

    Private Sub frmMMMenu_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        UcOnAirRecognizer1.StopRecording()
    End Sub

    Private Sub frmMMMenu_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Y Or e.KeyCode = Keys.Space Or e.KeyCode = Keys.Right Then
            Ansver = 1
        End If
        If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Escape Or e.KeyCode = Keys.N Or e.KeyCode = Keys.Left Then
            If (LeftText <> "no") Then Ansver = -1 Else Ansver = 0
        End If
        RedrawBmp(Ansver * w + w / 2, 10, True)
        DestOpacity = 0
        Timer1.Enabled = True
    End Sub

    Private Sub frmMenu_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        RedrawBmp(e.X, e.Y, True)
    End Sub
    Private Sub frmMenu_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If DestOpacity = 1 Then RedrawBmp(e.X, e.Y, e.Button <> Windows.Forms.MouseButtons.None)
        If e.X > w Then Ansver = 1 Else Ansver = 0
    End Sub
    Private Sub frmMenu_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        If MousePosition.Y < Me.Top + Me.Height And MousePosition.Y > Me.Top Then
            RedrawBmp(Ansver * w + w / 2, 10, True)
            DestOpacity = 0
            Timer1.Enabled = True
        Else
            Ansver = -1
            Me.Close()
        End If
        'If MousePosition.Y < Me.Top + Me.Height - 5 And MousePosition.Y >= Me.Top Then
        'End If
    End Sub

    Private Sub frmMMMenu_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        UcOnAirRecognizer1.StartListening(600)

        If StartX = -1 And StartY = -1 Then
            Me.Left = MousePosition.X - w / 2
            Me.Top = MousePosition.Y - h / 2
        Else
            Me.Left = StartX - w
            Me.Top = StartY - h / 2
        End If

        Me.Width = w * 2
        Me.Height = h

        bmpMain = New Bitmap(w * 2, h)
        g = Graphics.FromImage(bmpMain)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed

        LeftTextLeft = (w - g.MeasureString(LeftText, fontMain).Width) / 2 '- 1
        RightTextLeft = (w - g.MeasureString(RightText, fontMain).Width) / 2 '- 1

        textTop = h / 2 - Font.Size - 2
        If InStr(LeftText, vbNewLine) <> 0 Then textTop -= 10

        RedrawBmp(w / 2, 0, False)
    End Sub


    Private Sub RedrawBmp(x As Short, y As Short, MD As Boolean)
        Dim textbrushDark As New SolidBrush(Color.FromArgb(70, 70, 70))
        g.Clear(Color.FromArgb(230, 231, 232))
        If Ansver = 0 Then 'x < w Then
            If MD Then
                g.DrawString("default", fontMini, Brushes.LightGray, 4, h - 8 - Font.Size)
                g.FillRectangle(New SolidBrush(Color.FromArgb(19, 130, 206)), 1, 1, w - 2, h - 2)
                g.DrawString(LeftText, fontMain, Brushes.White, LeftTextLeft, textTop)
            Else
                g.DrawString("default", fontMini, Brushes.Gray, 4, h - 8 - Font.Size)
                g.FillRectangle(Brushes.White, 1, 1, w - 2, h - 2)
                g.DrawString(LeftText, fontMain, textbrushDark, LeftTextLeft, textTop)
            End If
            g.FillRectangle(New SolidBrush(Color.FromArgb(230, 231, 232)), 1 + w, 1, w - 2, h - 2)
            g.DrawString(RightText, fontMain, textbrushDark, RightTextLeft + w, textTop)
        ElseIf Ansver = 1 Then
            g.DrawString("default", fontMini, Brushes.Gray, 4, h - 8 - Font.Size)
            If MD Then
                g.FillRectangle(New SolidBrush(Color.FromArgb(19, 130, 206)), 1 + w, 1, w - 2, h - 2)
                g.DrawString(RightText, fontMain, Brushes.White, RightTextLeft + w, textTop)
            Else
                g.FillRectangle(Brushes.White, 1 + w, 1, w - 2, h - 2)
                g.DrawString(RightText, fontMain, textbrushDark, RightTextLeft + w, textTop)
            End If
            g.FillRectangle(New SolidBrush(Color.FromArgb(230, 231, 232)), 1, 1, w - 2, h - 2)
            g.DrawString(LeftText, fontMain, textbrushDark, LeftTextLeft, textTop)
        End If

        g.DrawRectangle(New Pen(textbrushDark.Color, 1), 0, 0, w * 2 - 1, h - 1)
        'g.DrawRectangle(New Pen(Color.FromArgb(230, 231, 232), 3), 0, 0, w * 2 - 1, h - 1)
        'g.DrawRectangle(New Pen(Color.FromArgb(19, 130, 206), 5), 0, 0, w * 2 - 1, h - 1)

        Me.BackgroundImage = bmpMain
        Me.Refresh()
    End Sub

    Private Sub frmMenu_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If e.X > w Then Ansver = 1 Else Ansver = 0
        Me.Close()
    End Sub

    Dim DestOpacity As Double = 1
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If DestOpacity = 1 Then
            Me.Opacity += (DestOpacity - Me.Opacity) * 0.2
        Else
            If Opacity > 0.9 Then
                Me.Opacity -= 0.02
            Else
                Me.Opacity -= 0.1
            End If
        End If
        If Math.Abs(Me.Opacity - DestOpacity) < 0.05 Or (DestOpacity = 0 And Me.Opacity < 0.5) Then
            Timer1.Enabled = False : Me.Opacity = DestOpacity
            If DestOpacity = 0 Then Me.Close()
        End If
    End Sub

    Private Sub UcOnAirRecognizer1_SmthRecognized(text() As String) Handles UcOnAirRecognizer1.SmthRecognized
        Dim ans As Long = -100

        Dim a1, a2, rec As String

        a1 = RightText.ToLower
        If (a1.IndexOf(" ") > 0) Then a1 = a1.Substring(0, a1.IndexOf(" "))
        a2 = LeftText.ToLower
        If (a2.IndexOf(" ") > 0) Then a2 = a2.Substring(0, a2.IndexOf(" ")) ': MsgBox(a1 + a2)

        For i As Long = 0 To Math.Min(4, text.Length - 1)
            rec = LCase(text(i))
            If rec = a1 Then ans = 1
            If rec = a2 Then ans = 0
            If (rec = "cancel" Or rec = "exit" Or rec = "no" Or rec = "close") Then
                If (LeftText <> "no") Then ans = -1 Else ans = 0
            End If
            If (ans = -100) Then Exit For
        Next
        If ans <> -100 Then
            Ansver = ans
            RedrawBmp(Ansver * w + w / 2, 10, True)
            DestOpacity = 0
            Timer1.Enabled = True
        End If
    End Sub

    Private Sub frmMMMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub UcOnAirRecognizer1_Load(sender As Object, e As EventArgs) Handles UcOnAirRecognizer1.Load

    End Sub

    Private Sub frmMMMenu_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        If e.KeyCode = Keys.Escape Then e.IsInputKey = True
    End Sub
End Class