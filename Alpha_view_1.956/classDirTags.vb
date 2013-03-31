Public Class classDirTags
    Structure structTags
        Dim Rating As Short
        Dim LaunchingTimes As Long

        Dim Type As String '  'songs'  'photos'

        Dim WireDX As Short
        Dim WireDY As Short
        Dim WireX As Short
        Dim WireY As Short
    End Structure
    Public N As Long
    Public Dirs(500000) As String
    Public Tags(500000) As structTags

    Public Sub Load()
        If IO.File.Exists(Application.StartupPath + "\tags\dir_tags.txt") Then
            Dim str As String
            FileOpen(1, Application.StartupPath + "\tags\dir_tags.txt", OpenMode.Input)

            N = 0
            While Not EOF(1)
                str = LineInput(1).ToLower.Trim
                If (Mid(str, 1, 2) = ">>") Then
                    N += 1
                    Dirs(N) = LCase(Mid(str, 3))
                Else
                    If (Mid(str, 1, 6) = "rating") Then Tags(N).Rating = Mid(str, 7)
                    If (Mid(str, 1, 8) = "launched") Then Tags(N).LaunchingTimes = Mid(str, 9)
                    If (Mid(str, 1, 4) = "type") Then Tags(N).Type = Mid(str, 5)
                End If
            End While

            FileClose(1)
        Else
            N = 0
            IO.File.Create(Application.StartupPath + "\tags\dir_tags.txt")
        End If
    End Sub
    Public Sub Save()
        If N Then
            Try
                FileOpen(1, Application.StartupPath + "\tags\dir_tags.txt", OpenMode.Output)
                For I As Long = 1 To N
                    PrintLine(1, ">>" + Dirs(I))

                    If Tags(I).Rating <> 0 Then PrintLine(1, "rating" + Tags(I).Rating.ToString)
                    If Tags(I).LaunchingTimes > 0 Then PrintLine(1, "launched" + Tags(I).LaunchingTimes.ToString)
                    If Tags(I).Type <> "" Then PrintLine(1, "type" + Tags(I).Type.ToString)
                Next
                FileClose(1)
            Catch
            End Try
        End If
    End Sub

    Public Function FindByName(ByVal Name As String) As Long
        Name = LCase(Name)
        If Mid(Name, Name.Length, 1) = "\" Then Name = Mid(Name, 1, Name.Length - 1)
        For I As Long = 1 To Me.N
            '            MsgBox(Files(I) + "  " + Name)
            If (Dirs(I) = Name) Then Return I
        Next
        Return 0
    End Function
    Public Function Add(ByVal Name As String) As Long
        N = N + 1
        Dirs(N) = LCase(Name)


        Return N
    End Function
End Class
