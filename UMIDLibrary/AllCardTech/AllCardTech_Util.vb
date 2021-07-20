Imports System.Text

Public Class AllCardTech_Util

    Public Function ExactBlock(ByVal Sector As Integer, ByVal Block As Integer, ByVal IsSectorTrailer As Boolean) As Byte

        If IsSectorTrailer Then
            If Sector >= 32 Then
                Block = 15
            Else
                Block = 3
            End If
        End If

        Dim BLCK As Byte = CByte(Block)

        If CInt(Sector) >= 32 Then
            ExactBlock = CByte(((Sector - 32) * 16) + 128 + BLCK)
        Else
            ExactBlock = CByte((Sector * 4) + BLCK)
        End If

    End Function

    Public Function IsSectorTrailer(ByVal ExactBlock As Integer) As Boolean

        If ExactBlock >= 128 Then
            If ExactBlock + 1 Mod 16 = 0 Then
                Return True
            Else
                Return False
            End If
        Else
            If ExactBlock + 1 Mod 4 = 0 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    Private Function Bytes_To_String2(ByVal bytes_Input As Byte()) As String
        Dim strTemp As New StringBuilder(bytes_Input.Length * 2)
        For Each b As Byte In bytes_Input
            strTemp.Append(Conversion.Hex(b))
        Next
        Return strTemp.ToString()
    End Function

    Public Function AsciiToByteArray(ByVal value As String) As Byte()
        Return System.Text.Encoding.UTF8.GetBytes(value)
    End Function
    Public Function ByteArrayToAscii(ByVal bytes() As Byte) As String
        Dim count As Integer
        For Each b As Byte In bytes
            If Not b = &H0 Then
                count += 1
            End If
        Next
        If count = 0 Then
            Return ""
        End If
        Return System.Text.Encoding.ASCII.GetString(bytes, 0, count).Trim()
    End Function

    Public Function HexToBytes(ByVal HexString As String) As Byte()
        Dim Bytes() As Byte
        Dim HexPos As Integer
        Dim HexDigit As Integer
        Dim BytePos As Integer
        Dim Digits As Integer

        ReDim Bytes(Len(HexString) \ 2)  'Initial estimate.
        For HexPos = 1 To Len(HexString)
            HexDigit = InStr("0123456789ABCDEF", _
                             UCase$(Mid$(HexString, HexPos, 1))) - 1
            If HexDigit >= 0 Then
                If BytePos > UBound(Bytes) Then
                    'Add some room, we'll add room for 4 more to decrease
                    'how often we end up doing this expensive step:
                    ReDim Preserve Bytes(UBound(Bytes) + 4)
                End If
                Bytes(BytePos) = Bytes(BytePos) * &H10 + HexDigit
                Digits = Digits + 1
            End If
            If Digits = 2 Or HexDigit < 0 Then
                If Digits > 0 Then BytePos = BytePos + 1
                Digits = 0
            End If
        Next
        If Digits = 0 Then BytePos = BytePos - 1
        If BytePos < 0 Then
            'Bytes = "" 'Empty.
        Else
            ReDim Preserve Bytes(BytePos)
        End If
        HexToBytes = Bytes
    End Function

    Public Function ByteArrayToHexString(ByVal ByteArray As Byte()) As String

        Dim hStr As String = ""

        For i As Integer = 0 To ByteArray.Length - 1
            hStr += ByteArray(i).ToString("X2")
        Next

        Return hStr

    End Function

    Public Sub HexStringToByteArray(ByVal HexString As String, ByRef ByteArray As Byte())

        Dim indexHex As Integer = 1

        For i As Integer = 0 To ByteArray.Length - 1
            ByteArray(i) = CInt("&H" + Mid(HexString, indexHex, 2))
            indexHex += 2
        Next

    End Sub

    Public Function Str2Hex(ByVal strData As String) As String
        Str2Hex = ""

        For i As Integer = 1 To strData.Length
            Str2Hex += Hex(CInt(AscW(CChar(Mid(strData, i, 1))))) + " "
        Next

    End Function

    Public Function Hex2Str(ByVal strData As String) As String

        Dim CurrentHex As String = ""
        Dim i As Integer = strData.Length / 2

        Hex2Str = ""

        For index As Integer = 1 To i
            CurrentHex = Mid(strData, index * 2 - 1, 2)

            Hex2Str += ChrW(CInt("&H" + CurrentHex)).ToString

        Next

    End Function

    Function Spacer(ByVal strData As String, ByVal CharsNeeded As Integer) As String

        Spacer = ""

        If strData.Length = CharsNeeded Then Return strData

        CharsNeeded -= strData.Length

        For i As Integer = 1 To CharsNeeded
            strData += " "
        Next

        Return strData

    End Function

    Public Function StrHEX_Dec(ByVal StrHex As String) As Integer

        Select Case StrHex
            Case "1"
                StrHEX_Dec = 1
            Case "2"
                StrHEX_Dec = 2
            Case "3"
                StrHEX_Dec = 3
            Case "4"
                StrHEX_Dec = 4
            Case "5"
                StrHEX_Dec = 5
            Case "6"
                StrHEX_Dec = 6
            Case "7"
                StrHEX_Dec = 7
            Case "8"
                StrHEX_Dec = 8
            Case "9"
                StrHEX_Dec = 9
            Case "0"
                StrHEX_Dec = 0
            Case "A"
                StrHEX_Dec = 10
            Case "B"
                StrHEX_Dec = 11
            Case "C"
                StrHEX_Dec = 12
            Case "D"
                StrHEX_Dec = 13
            Case "E"
                StrHEX_Dec = 14
            Case "F"
                StrHEX_Dec = 15
        End Select

    End Function

    Function Get128Bytes(ByVal pByteArray() As Byte, ByVal OffSet As Integer) As String

        Dim tByteArray(199) As Byte
        Dim pOffset = OffSet * 128

        Buffer.BlockCopy(pByteArray, pOffset, tByteArray, 0, 128)

        Return ByteArrayToHexString(tByteArray)

    End Function

    Function Get32Bytes(ByVal pByteArray() As Byte, ByVal OffSet As Integer) As String
        Dim tByteArray(199) As Byte
        Dim pOffset = OffSet * 32
        Buffer.BlockCopy(pByteArray, pOffset, tByteArray, 0, 32)
        Return ByteArrayToHexString(tByteArray)
    End Function

    Function ConvertIntToHexStr(value As Integer, lsbOnly As Boolean, swap As Boolean) As String
        Dim str = ByteArrayToHexString(BitConverter.GetBytes(value))
        Dim lsb = str.Substring(0, 2)
        Dim msb = str.Substring(2, 2)
        If lsbOnly Then
            Return lsb
        End If
        If swap Then
            Return msb + lsb
        Else
            Return lsb + msb
        End If
    End Function


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Function Byte2Hex(data As Byte) As String
        Return data.ToString("X2")
    End Function

End Class
