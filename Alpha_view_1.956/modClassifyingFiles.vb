Module modClassifyingFiles
    Dim MusicExtentions As String = " mp3 flac wav "
    Dim ImageExtentions As String = " jpg jpeg jpe gif png ico cur bmp jpg jpeg jpe ink gif png ico cur bmp tif "
    Dim FilmExtentions As String = " avi mp4 mov "

    Public Function IsMusicFile(Path As String) As Boolean
        If Path Is Nothing Then Return False
        Dim ext As String = ""
        Try
            ext = IO.Path.GetExtension(Path)
        Catch
            Return False
        End Try
        If ext.Length > 1 Then
            If InStr(MusicExtentions, " " + ext.Substring(1).ToLower + " ") > 0 Then Return True
        End If
        Return False
    End Function

    Public Function IsImageFile(Path As String) As Boolean
        If InStr(ImageExtentions, " " + LCase(GetFileExtention(Path)) + " ") > 0 Then Return True Else Return False
    End Function

    Public Function IsFilmFile(Path As String) As Boolean
        If InStr(FilmExtentions, " " + LCase(GetFileExtention(Path)) + " ") > 0 Then Return True Else Return False
    End Function
End Module
