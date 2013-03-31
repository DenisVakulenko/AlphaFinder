Public Class classFileTags
    Structure structSong
        Dim Name As String
        Dim Singer As String
        Dim Alboom As String
        Dim Genre As String
        Dim Romantic As Boolean
        Dim Learn As Boolean
        Dim Mood As String '  '=('  '=)'  '=O'  'zZ'
    End Structure
    Structure structTags
        Dim Rating As Short
        Dim LaunchingTimes As Long
        Dim Type As String '  'song'  'image'
        Dim Name As String
        Dim Song As structSong
    End Structure
    Public N As Long
    Public Files(500000) As String
    Public Tags(500000) As structTags

    Public Sub Load()
        If IO.File.Exists(Application.StartupPath + "\tags\tags.txt") Then
            Dim str As String
            FileOpen(1, Application.StartupPath + "\tags\tags.txt", OpenMode.Input)

            N = 0
            While Not EOF(1)
                str = LineInput(1).Trim
                If (Mid(str, 1, 2) = ">>") Then
                    N += 1
                    Files(N) = LCase(Mid(str, 3))
                    '.jpg .jpeg .jpe .gif .png .ico .cur .bmp .tif 
                    If IsImageFile(Files(N)) Then
                        Tags(N).Type = "image"
                    End If
                    If IsMusicFile(Files(N)) Then
                        Tags(N).Song.Name = "no!"
                        Tags(N).Song.Singer = "no!"
                        Tags(N).Type = "music"
                        '    If InStrRev(IO.Path.GetFileName(Files(N)), " - ") Then
                        '        Tags(N).Song.Singer = Mid(IO.Path.GetFileName(Files(N)), 1, InStrRev(IO.Path.GetFileName(Files(N)), " - "))
                        '        Tags(N).Song.Singer = UCase(Mid(Tags(N).Song.Singer, 1, 1)) + Mid(Tags(N).Song.Singer, 2)
                        '        Tags(N).Song.Name = Mid(IO.Path.GetFileNameWithoutExtension(Files(N)), InStrRev(IO.Path.GetFileNameWithoutExtension(Files(N)), " - ") + 3)
                        '        Tags(N).Song.Name = UCase(Mid(Tags(N).Song.Name, 1, 1)) + Mid(Tags(N).Song.Name, 2)
                        '    Else
                        '        Tags(N).Song.Singer = "noname"
                        '        Tags(N).Song.Name = IO.Path.GetFileNameWithoutExtension(Files(N))
                        '        Tags(N).Song.Name = UCase(Mid(Tags(N).Song.Name, 1, 1)) + Mid(Tags(N).Song.Name, 2)
                        '    End If

                        '    If Tags(N).Song.Name.Length > 2 Then
                        '        Tags(N).Song.Name = Tags(N).Song.Name.Replace("_", " ")

                        '        Dim p As Long
                        '        Dim Name As String = Tags(N).Song.Name
                        '        If Mid(Name, Name.Length, 1) = "g" Then
                        '            Tags(N).Song.Learn = True
                        '            Name = Mid(Name, 1, Name.Length - 1)
                        '        End If
                        '        If Mid(Name, Name.Length, 1) = "r" Then
                        '            Tags(N).Song.Romantic = True
                        '            Name = Mid(Name, 1, Name.Length - 1)
                        '        End If
                        '        Tags(N).Rating = 0
                        '        For p = Name.Length To 1 Step -1
                        '            If Mid(Name, p, 1) = "+" Then
                        '                Name = Mid(Name, 1, p - 1) + Mid(Name, p + 1)
                        '                Tags(N).Rating += 1
                        '            End If
                        '        Next
                        '        Tags(N).Song.Name = Name
                        '    End If
                    End If
                Else
                    If (Mid(str, 1, 6) = "rating") Then Tags(N).Rating = Mid(str, 7)
                    If (Mid(str, 1, 8) = "launched") Then Tags(N).LaunchingTimes = Mid(str, 9)
                    If (Mid(str, 1, 9) = "song-name") Then Tags(N).Song.Name = Mid(str, 10)
                    If (Mid(str, 1, 11) = "song-singer") Then Tags(N).Song.Singer = Mid(str, 12)
                    'If (Mid(str, 1, 4) = "mood") Then Tags(N).Mood = Mid(str, 5)
                End If
            End While

            FileClose(1)
        Else
            N = 0
            IO.File.Create(Application.StartupPath + "\tags\tags.txt")
        End If
    End Sub
    Public Sub Save()
        If N Then
            Try
                FileOpen(1, Application.StartupPath + "\tags\tags.txt", OpenMode.Output)
                For I As Long = 1 To N
                    If (Files(I) <> "") Then
                        PrintLine(1, ">>" + Files(I))

                        If Tags(I).Rating <> 0 Then PrintLine(1, "rating" + Tags(I).Rating.ToString)
                        If Tags(I).LaunchingTimes > 0 Then PrintLine(1, "launched" + Tags(I).LaunchingTimes.ToString)

                        If Tags(I).Song.Singer <> "" And Tags(I).Song.Singer <> "no!" Then PrintLine(1, "song-singer" + Tags(I).Song.Singer)
                        If Tags(I).Song.Name <> "" And Tags(I).Song.Name <> "no!" Then PrintLine(1, "song-name" + Tags(I).Song.Name)
                        If Tags(I).Song.Romantic = True Then PrintLine(1, "song-romantic")

                        'If Tags(I).Mood <> "" Then PrintLine(1, "mood" + Tags(I).Mood)
                    End If
                Next
                FileClose(1)
            Catch
            End Try
        End If
    End Sub

    'Public Function FindByRating(ByVal rating As Long) As Long
    '    For I As Long = 1 To N
    '        If (Tags(I).Rating = rating) Then Return I
    '    Next
    '    Return 0
    'End Function

    Public Sub SetTagsForSong(N As Long)
        Tags(N).Type = "music"

        Dim pr As New TestHelpers.TagClass
        Dim s As String = "", t As String = "", l As String = ""
        pr.Get(Files(N), s, t, l)

        If s <> "" And t <> "" Then
            Tags(N).Song.Name = t + " (" + l + ")"
            Tags(N).Song.Singer = s

            If Tags(N).Song.Name.Length > 2 Then
                Dim p As Long
                Dim Name As String = Files(N).Replace("_", " ")
                If Mid(Name, Name.Length, 1) = "g" Then
                    Tags(N).Song.Learn = True
                    Name = Mid(Name, 1, Name.Length - 1)
                End If
                If Mid(Name, Name.Length, 1) = "r" Then
                    Tags(N).Song.Romantic = True
                    Name = Mid(Name, 1, Name.Length - 1)
                End If
                Tags(N).Rating = 0
                For p = Name.Length To 1 Step -1
                    If Mid(Name, p, 1) = "+" Then
                        Name = Mid(Name, 1, p - 1) + Mid(Name, p + 1)
                        Tags(N).Rating += 1
                    End If
                Next
            End If
        Else
            Dim str As String = IO.Path.GetFileName(Files(N)).Replace("_", " ")
            If InStrRev(str, " - ") Then
                Tags(N).Song.Singer = Mid(str, 1, InStrRev(str, " - "))
                Tags(N).Song.Singer = UCase(Mid(Tags(N).Song.Singer, 1, 1)) + Mid(Tags(N).Song.Singer, 2)
                Tags(N).Song.Name = Mid(IO.Path.GetFileNameWithoutExtension(Files(N)), Tags(N).Song.Singer.Length + 3)
                Tags(N).Song.Name = UCase(Mid(Tags(N).Song.Name, 1, 1)) + Mid(Tags(N).Song.Name, 2)
            Else
                Tags(N).Song.Singer = "noname"
                Tags(N).Song.Name = IO.Path.GetFileNameWithoutExtension(Files(N))
                Tags(N).Song.Name = UCase(Mid(Tags(N).Song.Name, 1, 1)) + Mid(Tags(N).Song.Name, 2)
            End If

            If Tags(N).Song.Name.Length > 2 Then
                Tags(N).Song.Name = Tags(N).Song.Name.Replace("_", " ")

                Dim p As Long
                Dim Name As String = Tags(N).Song.Name
                If Mid(Name, Name.Length, 1) = "g" Then
                    Tags(N).Song.Learn = True
                    Name = Mid(Name, 1, Name.Length - 1)
                End If
                If Mid(Name, Name.Length, 1) = "r" Then
                    Tags(N).Song.Romantic = True
                    Name = Mid(Name, 1, Name.Length - 1)
                End If
                Tags(N).Rating = 0
                For p = Name.Length To 1 Step -1
                    If Mid(Name, p, 1) = "+" Then
                        Name = Mid(Name, 1, p - 1) + Mid(Name, p + 1)
                        Tags(N).Rating += 1
                    ElseIf Mid(Name, p, 1) = "\" Then
                        Exit For
                    End If
                Next
                Tags(N).Song.Name = Name
            End If
        End If
        'End If
    End Sub

    Public Function FindByFileName(ByVal Name As String) As Long
        Name = LCase(Name)
        For I As Long = 1 To Me.N
            'MsgBox(Files(I) + "  " + Name)
            If (Files(I) = Name) Then
                If Tags(I).Song.Name = "no!" And Tags(I).Song.Singer = "no!" And Tags(I).Type = "music" Then SetTagsForSong(I)
                Return I
            End If
        Next
        Return 0
    End Function
    Public Function Add(ByVal FileName As String) As Long
        N = N + 1
        Files(N) = LCase(FileName)

        If IsImageFile(Files(N)) Then
            Tags(N).Type = "image"
        End If
        If IsMusicFile(Files(N)) Then
            SetTagsForSong(N)
        End If

        Return N
    End Function
    'Public Sub Add(ByVal FileName As String, ByVal Rating As Long)
    '    N = N + 1
    '    Files(N) = FileName
    '    Tags(N).LaunchingTimes = 1
    '    Tags(N).Rating = Rating
    'End Sub
End Class
