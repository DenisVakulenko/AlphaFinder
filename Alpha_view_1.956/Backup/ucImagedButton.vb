Public Class ucImagedButton
    Public Shadows Event Click()
    Dim Picture As Button_pic_class
    Dim AllowSel As Boolean



    Public Sub LoadImages(ByVal path_normal As String, ByVal path_move As String, ByVal path_down As String, ByVal path_selection As String)
        If path_selection <> "" Then
            Try
                Picture.selection = Image.FromFile(Application.StartupPath + "\" + path_selection)
                Picture.normal = Image.FromFile(Application.StartupPath + "\" + path_normal)
                Picture.mouse_move = Image.FromFile(Application.StartupPath + "\" + path_move)
                Picture.mouse_down = Image.FromFile(Application.StartupPath + "\" + path_down)
            Catch ex As Exception
                MsgBox("Fail!  " + path_normal)
            End Try

            AllowSel = True
            pic.Size = Picture.normal.Size
            pic.Image = Picture.normal
            picSel.Size = Picture.selection.Size
            picSel.Image = Picture.selection
            picSel.Visible = False

            Me.Size = picSel.Size
            Me.Left = Me.Left - 2
            Me.Top = Me.Top - 2
            pic.Left = 2
            pic.Top = 2
        Else
            Try
                Picture.normal = Image.FromFile(Application.StartupPath + "\" + path_normal)
                Picture.mouse_move = Image.FromFile(Application.StartupPath + "\" + path_move)
                Picture.mouse_down = Image.FromFile(Application.StartupPath + "\" + path_down)
            Catch ex As Exception
                MsgBox("Fail!  " + path_normal)
            End Try

            AllowSel = False
            pic.Size = Picture.normal.Size
            pic.Image = Picture.normal
            Me.Size = pic.Size
            Me.TabStop = False
        End If
        Me.Visible = True
    End Sub

    Private Sub ButtonP_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        If AllowSel Then
            picSel.Visible = True
            'Me.Size = picSel.Size
            'Me.Left = Me.Left - 2
            'Me.Top = Me.Top - 2
            'pic.Left = 2
            'pic.Top = 2
        End If
    End Sub
    Private Sub ButtonP_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        If AllowSel Then
            picSel.Visible = False
            'Me.Size = pic.Size
            'Me.Left = Me.Left + 2
            'Me.Top = Me.Top + 2
            'pic.Left = 0
            'pic.Top = 0
        End If
    End Sub

    Public Sub btnDown()
        pic.Image = Picture.mouse_down
    End Sub
    Public Sub btnNormal()
        pic.Image = Picture.normal
    End Sub


    Private Sub ButtonP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If AllowSel Then If e.KeyCode = 13 Then btnDown()
    End Sub
    Private Sub ButtonP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If AllowSel Then
            btnNormal()
            If e.KeyCode = 13 Then RaiseEvent Click()
        End If
    End Sub

    Private Sub ButtonP_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pic.MouseDown
        btnDown()
        If AllowSel Then Me.Select()
    End Sub
    Private Sub ButtonP_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pic.MouseLeave
        pic.Image = Picture.normal
    End Sub
    Private Sub ButtonP_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pic.MouseMove
        If e.Button = Windows.Forms.MouseButtons.None Then
            pic.Image = Picture.mouse_move
        Else
            btnDown()
        End If
    End Sub

    Private Sub pic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pic.Click
        RaiseEvent Click()
    End Sub
    Private Sub pic_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pic.DoubleClick
        RaiseEvent Click()
    End Sub

    Private Sub ucImagedButton_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
