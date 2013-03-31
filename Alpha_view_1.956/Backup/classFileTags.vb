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
                str = LineInput(1).ToLower.Trim
                If (Mid(str, 1, 2) = ">>") Then
                    N += 1
                    Files(N) = LCase(Mid(str, 3))

                    If InStr(" .jpg .jpeg .jpe .gif .png .ico .cur .bmp ", " " + LCase(IO.Path.GetExtension(Files(N))) + " ") Then
                        Tags(N).Type = "image"
                    End If
                    If InStr(" .mp3 .flac .vav ", " " + LCase(IO.Path.GetExtension(Files(N))) + " ") Or LCase(IO.Path.GetExtension(Files(N))) = ".mp3" Then
                        Tags(N).Type = "music"
                        If InStrRev(IO.Path.GetFileName(Files(N)), " - ") Then
                            Tags(N).Song.Singer = Mid(IO.Path.GetFileName(Files(N)), 1, InStrRev(IO.Path.GetFileName(Files(N)), " - "))
                            Tags(N).Song.Singer = UCase(Mid(Tags(N).Song.Singer, 1, 1)) + Mid(Tags(N).Song.Singer, 2)
                            Tags(N).Song.Name = Mid(IO.Path.GetFileNameWithoutExtension(Files(N)), InStrRev(IO.Path.GetFileNameWithoutExtension(Files(N)), " - ") + 3)
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
                                End If
                            Next
                            Tags(N).Song.Name = Name
                        End If
                    End If
                Else
                    If (Mid(str, 1, 6) = "rating") Then Tags(N).Rating = Mid(str, 7)
                    If (Mid(str, 1, 8) = "launched") Then Tags(N).LaunchingTimes = Mid(str, 9)
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
            FileOpen(1, Application.StartupPath + "\tags\tags.txt", OpenMode.Output)
            For I As Long = 1 To N
                PrintLine(1, ">>" + Files(I))

                If Tags(I).Rating <> 0 Then PrintLine(1, "rating" + Tags(I).Rating.ToString)
                If Tags(I).LaunchingTimes > 0 Then PrintLine(1, "launched" + Tags(I).LaunchingTimes.ToString)
                'If Tags(I).Mood <> "" Then PrintLine(1, "mood" + Tags(I).Mood)
            Next
            FileClose(1)
        End If
    End Sub

    'Public Function FindByRating(ByVal rating As Long) As Long
    '    For I As Long = 1 To N
    '        If (Tags(I).Rating = rating) Then Return I
    '    Next
    '    Return 0
    'End Function
    Public Function FindByFileName(ByVal Name As String) As Long
        Name = LCase(Name)
        For I As Long = 1 To Me.N
            '            MsgBox(Files(I) + "  " + Name)
            If (Files(I) = Name) Then Return I
        Next
        Return 0
    End Function
    Public Function Add(ByVal FileName As String) As Long
        N = N + 1
        Files(N) = LCase(FileName)

        If InStr(" .jpg .jpeg .jpe .gif .png .ico .cur .bmp ", " " + LCase(IO.Path.GetExtension(Files(N))) + " ") Then
            Tags(N).Type = "image"
        End If
        If InStr(" .flac .mp3 .vav ", " " + LCase(IO.Path.GetExtension(Files(N))) + " ") Then
            Tags(N).Type = "music"
            If InStrRev(IO.Path.GetFileName(Files(N)), " - ") Then
                Tags(N).Song.Singer = Mid(IO.Path.GetFileName(Files(N)), 1, InStrRev(IO.Path.GetFileName(Files(N)), " - "))
                Tags(N).Song.Singer = UCase(Mid(Tags(N).Song.Singer, 1, 1)) + Mid(Tags(N).Song.Singer, 2)
                Tags(N).Song.Name = Mid(IO.Path.GetFileNameWithoutExtension(Files(N)), InStrRev(IO.Path.GetFileNameWithoutExtension(Files(N)), " - ") + 3)
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
                    End If
                Next
                Tags(N).Song.Name = Name
            End If
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
