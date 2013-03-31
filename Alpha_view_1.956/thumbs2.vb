Module thumbs
    Public Thumbnails(10000) As String, NThumbs As Long

    Public Sub LoadThumbsInfo()
        NThumbs = 0
        FileOpen(1, "config\base.txt", OpenMode.Input)
        While Not EOF(1)
            NThumbs += 1
            Thumbnails(NThumbs) = LineInput(1).ToLower.Trim
        End While
        FileClose(1)
    End Sub
    Public Sub SaveThumbsInfo()
        FileOpen(1, "config\base.txt", OpenMode.Output)
        For i As Long = 1 To NThumbs
            Print(1, Thumbnails(i) + Chr(13) + Chr(10))
        Next
        FileClose(1)
    End Sub
    Public Function SearchInThumbs(ByVal path As String) As Long
        path = path.ToLower
        For i As Long = 1 To NThumbs
            If path = Thumbnails(i) Then Return (i) : Exit For
        Next
    End Function
End Module
