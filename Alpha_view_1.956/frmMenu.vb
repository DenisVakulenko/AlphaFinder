Public Class frmMenu
    Public Items() As String
    Public NItems As Short = -1

    Public Ansver As Short = -1

    Public w As Short = 100
    Public h As Short = 32
    Private bmpMain As Bitmap
    Private g As Graphics

    Private fontMain As New Font("verdana", 10)

    Private Sub frmMenu_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        RedrawBmp(e.Y, True)
    End Sub

    Private Sub frmMenu_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        'Ansver = -1
        If MousePosition.Y < Me.Top + Me.Height And MousePosition.Y > Me.Top Then
            RedrawBmp(Ansver * h + h / 2, True)
            DestOpacity = 0
            Timer1.Enabled = True
        Else
            Ansver = -1
            Me.Close()
        End If
        'Me.Close()
    End Sub

    Private Sub frmMenu_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If DestOpacity = 1 Then RedrawBmp(e.Y, e.Button <> Windows.Forms.MouseButtons.None)
        Ansver = Math.Truncate(e.Y / h) '!!
    End Sub

    Private Sub frmMenu_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Left = MousePosition.X - w / 2
        Me.Top = MousePosition.Y - h / 2 - 4

        Me.Width = w
        Me.Height = h * (NItems + 1)

        bmpMain = New Bitmap(w, h * (NItems + 1))
        g = Graphics.FromImage(bmpMain)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        RedrawBmp(16, False)
    End Sub


    Private Sub RedrawBmp(y As Short, MD As Boolean)
        g.Clear(Color.LightGray)
        g.DrawRectangle(Pens.Black, 0, 0, w - 1, h * (NItems + 1) - 1)
        For i As Long = 0 To NItems
            If Math.Truncate(y / 32) = i Then
                If MD Then
                    g.FillRectangle(New SolidBrush(Color.FromArgb(19, 130, 206)), 1, i * h + 1, w - 2, h - 2)
                    g.DrawString(Items(i), fontMain, Brushes.White, 20, i * h + 8)
                Else
                    g.FillRectangle(Brushes.White, 1, i * 32 + 1, w - 2, h - 2)
                    g.DrawString(Items(i), fontMain, Brushes.Black, 20, i * h + 8)
                End If
            Else
                g.DrawString(Items(i), fontMain, Brushes.Black, 20, i * h + 8)
            End If
        Next
        Me.BackgroundImage = bmpMain
        Me.Refresh()
    End Sub

    Private Sub frmMenu_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Ansver = Math.Truncate(e.Y / h)
        Me.Close()
    End Sub

    Dim DestOpacity As Double = 1
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Opacity += (DestOpacity - Me.Opacity) * 0.2 '0.15
        If DestOpacity = 1 Then Me.Top += 1.2 - Me.Opacity Else Me.Top += Me.Opacity
        If Math.Abs(Me.Opacity - DestOpacity) < 0.05 Or (DestOpacity = 0 And Me.Opacity < 0.5) Then
            Timer1.Enabled = False : Me.Opacity = DestOpacity
            If DestOpacity = 0 Then Me.Close()
        End If
    End Sub
End Class