Module mod_buttons
    Public Structure Button_pic_class
        Dim selection As Bitmap
        Dim normal As Bitmap
        Dim mouse_move As Bitmap
        Dim mouse_down As Bitmap
    End Structure

    Public BG_Settings, BG_Loading, BG_35photo As Bitmap

    Public Sub LoadBGPicturies()
        BG_Settings = New Bitmap(Application.StartupPath + "\resources\bg_settings.gif")
        BG_Loading = New Bitmap(Application.StartupPath + "\resources\bg_loading.gif")
        BG_35photo = New Bitmap(Application.StartupPath + "\resources\bg_35.gif")
    End Sub
End Module
