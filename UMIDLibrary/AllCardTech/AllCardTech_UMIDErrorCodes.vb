Public Class AllCardTech_UMIDErrorCodes

    Function getDescriptionByBytes(code As String) As Byte()
        Dim str As String = getDescription(code)
        Return System.Text.Encoding.UTF8.GetBytes(str)
    End Function

    Function getDescription(code As String) As String
        Select Case code.ToUpper.Trim()
            Case "62 83"
                Return "Selected File invalidated."
            Case "63 C1"
                Return "Verify fail, 1 try left."
            Case "63 C2"
                Return "Verify fail, 2 tries left."
            Case "63 C3"
                Return "Verify fail, 3 tries left."
            Case "67 00"
                Return "Wrong Length"
            Case "69 82"
                Return "Security Conditions Not Satisfied"
            Case "69 83"
                Return "Authentication method blocked"
            Case "69 85"
                Return "Conditions of use not satisfied."
            Case "69 86"
                Return "Command not allowed (no current EF)"
            Case "6A 80"
                Return "The parameters in the data field are incorrect."
            Case "6A 81"
                Return "Function not supported"
            Case "6A 82"
                Return "File not found"
            Case "6A 83"
                Return "Record not found"
            Case "6A 84"
                Return "There is insufficient memory space in record or file"
            Case "6A 88"
                Return "Referenced data not found"
            Case "6A F0"
                Return "Wrong parameter value"
            Case "6B 00"
                Return "Wrong parameter(s) P1-P2"
            Case "6D 00"
                Return "Instruction code not supported or invalid"
            Case "6E 00"
                Return "Class not supported"
            Case "94 00"
                Return "No EF selected."
            Case Else
                Return ""
        End Select
    End Function



End Class
