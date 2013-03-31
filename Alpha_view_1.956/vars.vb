Module vars
    Public FileTags As New classFileTags()
    Public DirTags As New classDirTags()
    Public Structure foto_class
        Public X As Double
        Public Y As Double
        Public Name As String
        Public FileName As String
        Public Loaded As Boolean
        Dim Type As String
        'Public V As Point
    End Structure

    Public Structure fly_class
        Public X As Long
        Public Y As Long
        Public Make As Boolean
    End Structure

    Public foto_p(1500) As Long
End Module
