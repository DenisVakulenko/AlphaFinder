Module file_func
    Public Function GetFileName(ByVal path As String) As String
        Dim i As Long
        i = InStr(path, "\")
        While i <> 0
            path = Mid(path, i + 1)
            i = InStr(path, "\")
        End While

        Return path
    End Function
    Public Function GetFileExtention(ByVal path As String) As String
        Dim i As Long
        i = InStr(path, ".")
        While i <> 0
            path = Mid(path, i + 1)
            i = InStr(path, ".")
        End While

        Return path
    End Function
End Module
