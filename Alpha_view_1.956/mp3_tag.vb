Imports System.IO

Public Class mp3_tag
    'Private Mp3HeaderBitsStr As Mp3HeaderBitsString
    'Private MP3HeaderBits As BitArray
    'Private Mp3FInfo As FileInfo
    'Private XingHeaderBytes(115) As Byte
    'Private Mp3HeaderPosition, Mp3ClearSize As Integer

    'Private Structure Mp3HeaderBitsString
    '    Dim MPEGTypeBitsString As String
    '    Dim LayerBitsString As String
    '    Dim BitrateBitsString As String
    '    Dim FreqBitsString As String
    '    Dim ChannelModeBitsString As String
    'End Structure

    'Public Structure ID3v1TAG
    '    Dim SongTitle As String
    '    Dim Artist As String
    '    Dim Album As String
    '    Dim Year As String
    '    Dim Comment As String
    '    Dim GenreID As Integer
    'End Structure

    'Public Structure XingHeader
    '    Dim NumberOfFrames As Integer
    '    Dim FileLenght As Integer
    '    Dim TOC() As Integer
    '    Dim VBRScale As Integer
    'End Structure

    'Public Enum MPEGType
    '    MPEG1
    '    MPEG2
    '    MPEG2_5
    'End Enum

    'Public Enum LayerType
    '    LayerI
    '    LayerII
    '    LayerIII
    'End Enum

    'Public Enum ProtectionType
    '    ProtectedByCRC
    '    NotProtected
    'End Enum

    'Public Enum CopyRight
    '    CopyRighted
    '    NotCopyRighted
    'End Enum

    'Public Enum ChannelMode
    '    Stereo
    '    JointStereo_Stereo
    '    DualChannel_2MonoChannels
    '    SingleChannel_Mono
    'End Enum

    ''-------------------------------------------------------------------------------------------------------
    ''The mp3’s ID3v1, this read only property returns an Mp3Class.ID3v1TAG structure.
    ''-------------------------------------------------------------------------------------------------------
    'Public ReadOnly Property ID3v1() As ID3v1TAG
    '    Get
    '        Return GetID3v1TAG(Mp3FInfo.FullName)
    '    End Get
    'End Property

    ''-------------------------------------------------------------------------------------------------------
    ''This property returns the System.IO.FileInfo of the current Mp3 file.
    ''-------------------------------------------------------------------------------------------------------
    'Public ReadOnly Property Mp3FileInfo()
    '    Get
    '        Return Mp3FInfo
    '    End Get
    'End Property

    ''-------------------------------------------------------------------------------------------------------
    ''Constructor
    ''-------------------------------------------------------------------------------------------------------
    'Public Sub New(ByVal MP3FilePath As String)
    '    'Check the Mp3 file existence and throw an exception if the file doesn't exist.
    '    If File.Exists(MP3FilePath) Then
    '        'Take the Mp3FileInfo from the given path.
    '        Mp3FInfo = New FileInfo(MP3FilePath)

    '        If Not GetMP3HeaderBytes(Mp3FInfo.FullName) Then
    '            'If the GetMP3HeaderBytes() fails throw exception.
    '            Throw New Exception("Given file '" & MP3FilePath & "' isn't a valid Mp3 file.")
    '        End If

    '    Else
    '        Throw New Exception("Given file '" & MP3FilePath & "' doesn't exist.")
    '    End If
    'End Sub

    ''**************************************** - Private Procedures - ***************************************

    ''-------------------------------------------------------------------------------------------------------
    ''GetMp3FileStream(ByVal MP3FilePath As String) As FileStream : Returns a System.IO.FileStream
    '' from the given path.
    ''-------------------------------------------------------------------------------------------------------
    'Private Function GetMp3FileStream(ByVal MP3FilePath As String) As FileStream
    '    Dim MP3FileStream As FileStream
    '    Try
    '        MP3FileStream = New FileStream(MP3FilePath, FileMode.Open)
    '    Catch Exc As IOException
    '        Throw New Exception("Can't open file '" & MP3FilePath & "'.")
    '        Exit Function
    '    Catch Exc As Exception
    '        Throw New Exception("An error occurred while trying to open file '" & MP3FilePath & "'.")
    '        Exit Function
    '    End Try
    '    If MP3FileStream.CanRead Then
    '        MP3FileStream.Position = 0
    '    Else
    '        Throw New Exception("Can't read file '" & MP3FilePath & "'.")
    '        Exit Function
    '    End If
    '    Return MP3FileStream
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetMP3HeaderBytes(ByVal MP3FilePath As String) As Boolean : Gets a BitArray with the Mp3 header bits and 
    '' an Mp3HeaderBitsStr structure. It returns a boolean value , false if it fails.
    '' For more info about Mp3Header check http://www.mp3-tech.org/
    ''-------------------------------------------------------------------------------------------------------
    'Private Function GetMP3HeaderBytes(ByVal MP3FilePath As String) As Boolean
    '    Dim MP3FileStream As FileStream = GetMp3FileStream(MP3FilePath)
    '    Dim TempMP3HeaderBits As BitArray
    '    Dim MP3HeaderBytes(2) As Byte
    '    Dim IsSyncByte As Boolean
    '    Dim i, j, Index, BitOffSet, StartPosition As Integer
    '    MP3HeaderBits = New BitArray(24)

    '    Try
    '        While (MP3FileStream.Position + 4) <= MP3FileStream.Length

    '            'Read a byte from file and check if its bits are "11111111" 
    '            '(this corresponds to an integer with value = 255)
    '            If MP3FileStream.ReadByte = 255 Then
    '                Mp3HeaderPosition = MP3FileStream.Position
    '                IsSyncByte = True
    '            End If

    '            While IsSyncByte

    '                'Read the next 3 bytes
    '                MP3FileStream.Read(MP3HeaderBytes, 0, 3)
    '                TempMP3HeaderBits = New BitArray(MP3HeaderBytes)

    '                'Check the bits no 9,10,11 to ensure we have the sync => the Mp3 Header bytes
    '                For i = 7 To 5 Step -1
    '                    If Not TempMP3HeaderBits.Item(i) Then
    '                        IsSyncByte = False
    '                        MP3FileStream.Position -= 3
    '                        Exit While
    '                    End If
    '                Next
    '                Index = 0
    '                BitOffSet = 0

    '                'Now we have the Header bits from the file but in the MP3HeaderBits array ,the bits are
    '                'in the form of "8 7 6 5 4 3 2 1 16 15 14...", we just put them in the form of 
    '                '"1 2 3 4 5 6 7 8 10 11..." with the following 2 loops.
    '                For j = 0 To 2
    '                    For i = 7 To 0 Step -1
    '                        MP3HeaderBits.Item(Index) = TempMP3HeaderBits.Item(BitOffSet + i)
    '                        Index += 1
    '                    Next
    '                    BitOffSet += 8
    '                Next

    '                'Get the Mp3HeaderBitsStr structure 
    '                With Mp3HeaderBitsStr
    '                    .MPEGTypeBitsString = GetBitsString(3, 4)
    '                    .LayerBitsString = GetBitsString(5, 6)
    '                    .BitrateBitsString = GetBitsString(8, 11)
    '                    .FreqBitsString = GetBitsString(12, 13)
    '                    .ChannelModeBitsString = GetBitsString(16, 17)

    '                    'Check the MPEGTypeBits for a bad value
    '                    If .MPEGTypeBitsString = "01" Then
    '                        IsSyncByte = False
    '                        MP3FileStream.Position -= 3
    '                        Exit While
    '                    End If

    '                    'Check the LayerBits for a bad value, we only want LayerIII too!
    '                    If .LayerBitsString = "11" Or .LayerBitsString = "10" Or .LayerBitsString = "00" Then
    '                        IsSyncByte = False
    '                        MP3FileStream.Position -= 3
    '                        Exit While
    '                    End If

    '                    'Check the bitrate bits for a bad value , they can't be "1111"
    '                    If .BitrateBitsString = "1111" Then
    '                        IsSyncByte = False
    '                        MP3FileStream.Position -= 3
    '                        Exit While
    '                    End If

    '                    'Check the FrequencyBits for a bad value
    '                    If .FreqBitsString = "11" Then
    '                        IsSyncByte = False
    '                        MP3FileStream.Position -= 3
    '                        Exit While
    '                    Else
    '                        'If all the above are ok , we have our header!
    '                        Mp3ClearSize = (MP3FileStream.Length - Mp3HeaderPosition - 128)
    '                        Return True
    '                    End If
    '                End With

    '            End While
    '        End While

    '        Return False
    '    Catch Exc As Exception
    '        Throw New Exception(Exc.Message)
    '    Finally
    '        MP3FileStream.Close()
    '    End Try
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''CheckForZeroBytes(ByVal ByteToCheck As Byte) As Byte
    ''-------------------------------------------------------------------------------------------------------
    'Private Function CheckForZeroBytes(ByVal ByteToCheck As Byte) As Byte
    '    Dim ByteToStringConv As New System.Text.UTF8Encoding()
    '    Dim EmptyCharByte() As Byte = ByteToStringConv.GetBytes(" ")
    '    If ByteToCheck = 0 Then
    '        Return EmptyCharByte(0)
    '    Else
    '        Return ByteToCheck
    '    End If
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetBitsString(ByVal StartIndex As Integer, ByVal EndIndex As Integer) As String
    ''-------------------------------------------------------------------------------------------------------
    'Private Function GetBitsString(ByVal StartIndex As Integer, ByVal EndIndex As Integer) As String
    '    Dim BitsString As String
    '    Dim i As Integer
    '    For i = StartIndex To EndIndex
    '        Select Case MP3HeaderBits.Item(i)
    '            Case True : BitsString += "1"
    '            Case False : BitsString += "0"
    '        End Select
    '    Next
    '    Return BitsString
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetID3v1TAG(ByVal MP3FilePath) As ID3v1TAG. For more info check http://www.mp3-tech.org/
    ''-------------------------------------------------------------------------------------------------------
    'Private Function GetID3v1TAG(ByVal MP3FilePath) As ID3v1TAG
    '    Dim MP3FileStream As FileStream = GetMp3FileStream(MP3FilePath)
    '    Dim ByteToStringConv As New System.Text.UTF8Encoding()
    '    Dim CID3v1 As ID3v1TAG
    '    Dim ID3v1Bytes(127), SongTitleBytes(29), ArtistBytes(29) As Byte
    '    Dim AlbumBytes(29), CommentBytes(29), YearBytes(3) As Byte
    '    Dim i As Integer

    '    Try
    '        'We get the last 128 bytes from the file...
    '        MP3FileStream.Position = MP3FileStream.Length - 128
    '        MP3FileStream.Read(ID3v1Bytes, 0, 128)
    '    Catch
    '        Throw New Exception("Can't read file '" & MP3FilePath & "'.")
    '        Exit Function
    '    Finally
    '        MP3FileStream.Close()
    '    End Try

    '    '....and put them in individual arrays of bytes to make our life easier 
    '    For i = 0 To 29
    '        SongTitleBytes(i) = CheckForZeroBytes(ID3v1Bytes(3 + i))
    '        ArtistBytes(i) = CheckForZeroBytes(ID3v1Bytes(33 + i))
    '        AlbumBytes(i) = CheckForZeroBytes(ID3v1Bytes(63 + i))
    '        CommentBytes(i) = CheckForZeroBytes(ID3v1Bytes(97 + i))
    '    Next

    '    For i = 0 To 3
    '        YearBytes(i) = CheckForZeroBytes(ID3v1Bytes(93 + i))
    '    Next

    '    'We set our CID3v1 structure and return it!
    '    With CID3v1
    '        .SongTitle = ByteToStringConv.GetString(SongTitleBytes)
    '        .Artist = ByteToStringConv.GetString(ArtistBytes)
    '        .Album = ByteToStringConv.GetString(AlbumBytes)
    '        .Comment = ByteToStringConv.GetString(CommentBytes)
    '        .Year = ByteToStringConv.GetString(YearBytes)
    '        .GenreID = CType(ID3v1Bytes(127), Integer)
    '    End With
    '    Return CID3v1
    'End Function

    ''***************************************** - Public Procedures - ***************************************

    ''-------------------------------------------------------------------------------------------------------
    ''IsVBR() : Returns true if the mp3 is VBR.
    '' For more info see : http://www.multiweb.cz/twoinches/MP3inside.htm#MP3FileStructure
    ''-------------------------------------------------------------------------------------------------------
    'Public Function IsVBR() As Boolean
    '    Dim MP3FileStream As FileStream = GetMp3FileStream(Mp3FInfo.FullName)
    '    Dim ByteToStringConv As New System.Text.UTF8Encoding()
    '    Dim XingBytes(3) As Byte

    '    Try
    '        If Not GetChannelMode() = ChannelMode.SingleChannel_Mono Then
    '            MP3FileStream.Position = Mp3HeaderPosition + 35
    '            MP3FileStream.Read(XingBytes, 0, 4)
    '        Else
    '            MP3FileStream.Position = Mp3HeaderPosition + 20
    '            MP3FileStream.Read(XingBytes, 0, 4)
    '        End If

    '        If ByteToStringConv.GetString(XingBytes) = "Xing" Then
    '            MP3FileStream.Read(XingHeaderBytes, 0, 116)
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Catch
    '        Throw New Exception("Can't read file '" & Mp3FInfo.FullName & "'.")
    '        Exit Function
    '    Finally
    '        MP3FileStream.Close()
    '    End Try

    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetXingHeader() : Returns a XingHeader structure.
    '' For more info see : http://www.multiweb.cz/twoinches/MP3inside.htm#MP3FileStructure
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetXingHeader() As XingHeader
    '    Dim CXingHeader As XingHeader
    '    Dim BitConv As BitConverter
    '    Dim FrameCountBytes(3), FileLenghtBytes(3), VBRScaleBytes(3) As Byte
    '    Dim Index, TOC(99), i As Integer

    '    If IsVBR() Then
    '        For i = 3 To 0 Step -1
    '            VBRScaleBytes(Index) = XingHeaderBytes(112 + i)
    '            FrameCountBytes(Index) = XingHeaderBytes(4 + i)
    '            FileLenghtBytes(Index) = XingHeaderBytes(8 + i)
    '            Index += 1
    '        Next

    '        For i = 0 To 99
    '            TOC(i) = CType(XingHeaderBytes(12 + i), Integer)
    '        Next
    '    Else
    '        Throw New Exception("'" & Mp3FInfo.FullName & "' is not VBR")
    '        Exit Function
    '    End If

    '    With CXingHeader
    '        .FileLenght = BitConv.ToInt32(FileLenghtBytes, 0)
    '        .NumberOfFrames = BitConv.ToInt32(FrameCountBytes, 0)
    '        .VBRScale = BitConv.ToInt32(VBRScaleBytes, 0)
    '        .TOC = TOC
    '    End With
    '    Return CXingHeader

    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetMPEGType() : Returns the Mpeg type of  the current Mpeg file (Mp3) as an
    '' Mp3Class.MPEGType enumeration.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetMPEGType() As MPEGType

    '    Select Case Mp3HeaderBitsStr.MPEGTypeBitsString
    '        Case "11" : Return MPEGType.MPEG1
    '        Case "10" : Return MPEGType.MPEG2
    '        Case "00" : Return MPEGType.MPEG2_5
    '    End Select
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetLayer() : Returns the Layer type of  the current Mpeg file (Mp3) as an 
    '' Mp3Class.LayerType enumeration.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetLayer() As LayerType

    '    Select Case Mp3HeaderBitsStr.LayerBitsString
    '        Case "01" : Return LayerType.LayerIII
    '        Case "10" : Return LayerType.LayerII
    '        Case "11" : Return LayerType.LayerI
    '    End Select
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetProtection() as ProtectionType : Returns the Protection type of the current Mp3 file as an 
    '' Mp3Class.ProtectionType enumeration.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetProtection() As ProtectionType
    '    If Not MP3HeaderBits.Item(7) Then
    '        Return ProtectionType.ProtectedByCRC
    '    Else
    '        Return ProtectionType.NotProtected
    '    End If
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetBitrate() as Integer : Returns the bitrate of the current Mp3 file in bits per second.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetBitrate() As Integer
    '    Dim BitrateArray() As Integer

    '    If GetMPEGType() = MPEGType.MPEG1 Then
    '        Dim TmpBitrateArray() As Integer = {32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320}
    '        BitrateArray = TmpBitrateArray
    '    Else
    '        Dim TmpBitrateArray() As Integer = {8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160}
    '        BitrateArray = TmpBitrateArray
    '    End If

    '    If Not IsVBR() Then
    '        Select Case Mp3HeaderBitsStr.BitrateBitsString
    '            Case "0001" : Return BitrateArray(0) * 1000
    '            Case "0010" : Return BitrateArray(1) * 1000
    '            Case "0011" : Return BitrateArray(2) * 1000
    '            Case "0100" : Return BitrateArray(3) * 1000
    '            Case "0101" : Return BitrateArray(4) * 1000
    '            Case "0110" : Return BitrateArray(5) * 1000
    '            Case "0111" : Return BitrateArray(6) * 1000
    '            Case "1000" : Return BitrateArray(7) * 1000
    '            Case "1001" : Return BitrateArray(8) * 1000
    '            Case "1010" : Return BitrateArray(9) * 1000
    '            Case "1011" : Return BitrateArray(10) * 1000
    '            Case "1100" : Return BitrateArray(11) * 1000
    '            Case "1101" : Return BitrateArray(12) * 1000
    '            Case "1110" : Return BitrateArray(13) * 1000
    '        End Select
    '    Else
    '        Dim CXingHeader As XingHeader = GetXingHeader()
    '        Dim LastByte, AverageFrameLenght, AverageBitrate As Integer

    '        'For more info see : http://www.multiweb.cz/twoinches/MP3inside.htm#MP3FileStructure
    '        With CXingHeader
    '            LastByte = Math.Round((.TOC(99) / 256) * .FileLenght, 0)
    '            AverageFrameLenght = Math.Round(.FileLenght / .NumberOfFrames, 0)
    '            AverageBitrate = Math.Round(((AverageFrameLenght * GetSamplingRateFreq()) / 144) / 1000, 0)
    '        End With
    '        Return AverageBitrate * 1000
    '    End If
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetSamplingRateFreq() as Integer : Returns the sampling rate Frequency of the current Mp3 file in Hz.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetSamplingRateFreq() As Integer

    '    Select Case Mp3HeaderBitsStr.FreqBitsString
    '        Case "00" : Return 44100
    '        Case "01" : Return 48000
    '        Case "10" : Return 32000
    '    End Select
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetChannelMode() as ChannelMode : Returns the Channel mode of  the current Mp3 file as an
    '' Mp3Class.ChannelMode enumeration.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetChannelMode() As ChannelMode

    '    Select Case Mp3HeaderBitsStr.ChannelModeBitsString
    '        Case "00" : Return ChannelMode.Stereo
    '        Case "01" : Return ChannelMode.JointStereo_Stereo
    '        Case "10" : Return ChannelMode.DualChannel_2MonoChannels
    '        Case "11" : Return ChannelMode.SingleChannel_Mono
    '    End Select
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetCopyRight() as CopyRight : Returns the Copyright type of the current Mp3 file as an
    '' Mp3Class.CopyRight enumeration.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetCopyRight() As CopyRight
    '    If MP3HeaderBits.Item(20) Then
    '        Return CopyRight.CopyRighted
    '    Else
    '        Return CopyRight.NotCopyRighted
    '    End If
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetDuration() as Integer : Returns the duration of the current Mp3 file in seconds.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetDuration() As Integer
    '    Dim Duration As Integer = Math.Round(((Mp3ClearSize * 8) / GetBitrate()), 0)
    '    Return Duration
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetDurationString() as String : Returns a string with the duration of the current Mp3 file
    '' in the form of : “hh:mm:ss”.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetDurationString() As String
    '    Dim DurationString As String
    '    Dim CurrentDuration As Integer = GetDuration()
    '    Dim DurationHour As Integer = CurrentDuration \ 3600
    '    Dim DurationMin As Integer
    '    Dim DurationSec As Integer

    '    If DurationHour >= 1 Then
    '        DurationMin = ((CurrentDuration Mod 3600) \ 60)
    '        DurationSec = ((CurrentDuration Mod 3600) Mod 60)
    '        DurationString = Format(DurationHour, "00") & ":" & Format(DurationMin, "00") & ":" & Format(DurationSec, "00")
    '    Else
    '        DurationMin = CurrentDuration \ 60
    '        DurationSec = CurrentDuration Mod 60
    '        DurationString = Format(DurationMin, "00") & ":" & Format(DurationSec, "00")
    '    End If
    '    Return DurationString
    'End Function

    ''-------------------------------------------------------------------------------------------------------
    ''GetGenreString(ByVal GenreID As Integer) As String : Returns the name of the genre which
    '' corresponds in the Current genreID.
    ''-------------------------------------------------------------------------------------------------------
    'Public Function GetGenreString(ByVal GenreID As Integer) As String
    '    Dim AvailableGenres() As String = {"Blues", "Classic Rock", "Country", "Dance", "Disco", "Funk", "Grunge", _
    '    "Hip - Hop", "Jazz", "Metal", "New Age", "Oldies", "Other", "Pop", "R&B", "Rap", "Reggae", "Rock", "Techno", _
    '    "Industrial", "Alternative", "Ska", "Death Metal", "Pranks", "Soundtrack", "Euro -Techno", "Ambient", _
    '    "Trip -Hop", "Vocal", "Jazz Funk", "Fusion", "Trance", "Classical", "Instrumental", "Acid", "House", "Game", _
    '    "Sound Clip", "Gospel", "Noise", "AlternRock", "Bass", "Soul", "Punk", "Space", "Meditative", _
    '    "Instrumental Pop", "Instrumental Rock", "Ethnic", "Gothic", "Darkwave", "Techno -Industrial", "Electronic", _
    '    "Pop -Folk", "Eurodance", "Dream", "Southern Rock", "Comedy", "Cult", "Gangsta", "Top 40", "Christian Rap", _
    '    "Pop/Funk", "Jungle", "Native American", "Cabaret", "New Wave", "Psychadelic", "Rave", "Showtunes", "Trailer", _
    '    "Lo - Fi", "Tribal", "Acid Punk", "Acid Jazz", "Polka", "Retro", "Musical", "Rock & Roll", "Hard Rock", _
    '    "Folk", "Folk/Rock", "National Folk", "Swing", "Bebob", "Latin", "Revival", "Celtic", "Bluegrass", "Avantgarde", _
    '    "Gothic Rock", "Progressive Rock", "Psychedelic Rock", "Symphonic Rock", "Slow Rock", "Big Band", "Chorus", _
    '    "Easy Listening", "Acoustic", "Humour", "Speech", "Chanson", "Opera", "Chamber Music", "Sonata", "Symphony", _
    '    "Booty Bass", "Primus", "Porn Groove", "Satire", "Slow Jam", "Club", "Tango", "Samba", "Folklore", "Ballad", _
    '    "Power Ballad", "Rhythmic Soul", "Freestyle", "Duet", "Punk Rock", "Drum Solo", "A Cappella", "Euro - House", _
    '     "Dance Hall", "Goa", "Drum & Bass", "Club - House", "Hardcore", "Terror", "Indie", "BritPop", "Negerpunk", _
    '    "Polsk Punk", "Beat", "Christian Gangsta Rap", "Heavy Metal", "Black Metal", "Crossover", "Contemporary Christian", _
    '    "Christian Rock", "Merengue", "Salsa", "Thrash Metal", "Anime", "JPop", "Synthpop"}

    '    Try
    '        Return AvailableGenres(GenreID)
    '    Catch
    '        Return " "
    '    End Try
    'End Function



End Class
