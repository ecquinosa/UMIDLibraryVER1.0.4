Imports System.Windows.Forms
Imports System.Drawing
Imports ClassWinsCard


Public Class AllCardTech_PCSC

    Private rConversion As New AllCardTech_Util

    Private SendRequest As SCARD_IO_REQUEST
    Private ReceiveRequest As SCARD_IO_REQUEST
    Private ReaderState As SCARD_READERSTATE

#Region "Members"

    Private member_CardReturnCode As Long
    Private member_hContext As Long
    Private member_hCard As Long
    Private member_Protocol As Long

    Private member_SendBuffer(262) As Byte
    Private member_ReceiveBuffer(262) As Byte

    Private member_SendLength As Integer
    Private member_ReceiveLength As Integer
    Private member_BytesReturned As Integer
    Private member_RequestType As Integer
    Private member_AProtocol As Integer

    Private member_ConnectionActive As Boolean
    Private member_AutoDetect As Boolean
    Private member_ValidATS As Boolean

    Private member_dwProtocol As Integer
    Private member_cbPciLength As Integer
    Private member_dwState As Long
    Private member_dwActProtocol As Long

    Private member_Response As String
    Private member_LastByteSent As String
    Private member_LastByteReceived As String

    Private member_ATR(64) As Byte
    Private member_ATRLength As Long

#End Region

#Region "Properties"
    Dim CurrentReaderName As Object
    Dim SmartCardUID As String

    Public Property CardReturnCode() As Long
        Get
            Return member_CardReturnCode
        End Get
        Set(ByVal value As Long)
            member_CardReturnCode = value
        End Set
    End Property

    Public Property hContext() As Long
        Get
            Return member_hContext
        End Get
        Set(ByVal value As Long)
            member_hContext = value
        End Set
    End Property

    Public Property hCard() As Long
        Get
            Return member_hCard
        End Get
        Set(ByVal value As Long)
            member_hCard = value
        End Set
    End Property

    Public Property Protocol() As Long
        Get
            Return member_Protocol
        End Get
        Set(ByVal value As Long)
            member_Protocol = value
        End Set
    End Property

    Public Property SendBuffer() As Byte()
        Get
            Return member_SendBuffer
        End Get
        Set(ByVal value As Byte())
            member_SendBuffer = value
        End Set
    End Property

    Public Property ReceiveBuffer() As Byte()
        Get
            Return member_ReceiveBuffer
        End Get
        Set(ByVal value As Byte())
            member_ReceiveBuffer = value
        End Set
    End Property

    Public Property SendLength() As Long
        Get
            Return member_SendLength
        End Get
        Set(ByVal value As Long)
            member_SendLength = value
        End Set
    End Property

    Public Property ReceiveLength() As Long
        Get
            Return member_ReceiveLength
        End Get
        Set(ByVal value As Long)
            member_ReceiveLength = value
        End Set
    End Property

    Public Property BytesReturned() As Long
        Get
            Return member_BytesReturned
        End Get
        Set(ByVal value As Long)
            member_BytesReturned = value
        End Set
    End Property

    Public Property RequestType() As Long
        Get
            Return member_RequestType
        End Get
        Set(ByVal value As Long)
            member_RequestType = value
        End Set
    End Property

    Public Property AProtocol() As Long
        Get
            Return member_AProtocol
        End Get
        Set(ByVal value As Long)
            member_AProtocol = value
        End Set
    End Property

    Public Property ConnectionActive() As Boolean
        Get
            Return member_ConnectionActive
        End Get
        Set(ByVal value As Boolean)
            member_ConnectionActive = value
        End Set
    End Property

    Public Property AutoDetect() As Boolean
        Get
            Return member_AutoDetect
        End Get
        Set(ByVal value As Boolean)
            member_AutoDetect = value
        End Set
    End Property

    Public Property ValidATS() As Boolean
        Get
            Return member_ValidATS
        End Get
        Set(ByVal value As Boolean)
            member_ValidATS = value
        End Set
    End Property

    Public Property dwProtocol() As Integer
        Get
            Return member_dwProtocol
        End Get
        Set(ByVal value As Integer)
            member_dwProtocol = value
        End Set
    End Property

    Public Property cbPciLengtha() As Integer
        Get
            Return member_cbPciLength
        End Get
        Set(ByVal value As Integer)
            member_cbPciLength = value
        End Set
    End Property

    Public Property dwState() As Long
        Get
            Return member_dwState
        End Get
        Set(ByVal value As Long)
            member_dwState = value
        End Set
    End Property

    Public Property dwActProtocol() As Long
        Get
            Return member_dwActProtocol
        End Get
        Set(ByVal value As Long)
            member_dwActProtocol = value
        End Set
    End Property

    Public Property Response() As String
        Get
            Return member_Response
        End Get
        Set(ByVal value As String)
            member_Response = value
        End Set
    End Property

    Public Property LastByteSent() As String
        Get
            Return member_LastByteSent
        End Get
        Set(ByVal value As String)
            member_LastByteSent = value
        End Set
    End Property

    Public Property LastByteReceived() As String
        Get
            Return member_LastByteReceived
        End Get
        Set(ByVal value As String)
            member_LastByteReceived = value
        End Set
    End Property

    Public Property ATR() As Byte()
        Get
            Return member_ATR
        End Get
        Set(ByVal value As Byte())
            member_ATR = value
        End Set
    End Property

    Public Property ATRLength() As Long
        Get
            Return member_ATRLength
        End Get
        Set(ByVal value As Long)
            member_ATRLength = value
        End Set
    End Property
#End Region

    Public ReaderList(19) As String

#Region "PCSC Routines"

    Public Sub ClearBuffers()
        For indx As Long = 0 To 262
            ReceiveBuffer(indx) = &H0
            SendBuffer(indx) = &H0
        Next indx
    End Sub

    Public Sub DisplayOut(ByVal errType As Integer, ByVal retVal As Integer, ByVal PrintText As String, ByVal mMsg As ListBox)

        Select Case errType

            Case 0
                mMsg.ForeColor = Color.Teal
            Case 1
                mMsg.ForeColor = Color.Red
                PrintText = ModWinsCard.GetScardErrMsg(retVal)
            Case 2
                mMsg.ForeColor = Color.Black
                PrintText = "<" + PrintText
            Case 3
                mMsg.ForeColor = Color.Black
                PrintText = ">" + PrintText

        End Select

        mMsg.Items.Add(PrintText)
        mMsg.ForeColor = Color.Black
        mMsg.Focus()
        mMsg.SelectedIndex = mMsg.Items.Count - 1

    End Sub

    Public Sub LoadListToControl(ByVal Ctrl As ComboBox, ByVal ReaderList As String)

        Dim sTemp As String
        Dim indx As Integer

        indx = 1
        sTemp = ""
        Ctrl.Items.Clear()

        While (Mid(ReaderList, indx, 1) <> vbNullChar)

            While (Mid(ReaderList, indx, 1) <> vbNullChar)
                sTemp = sTemp + Mid(ReaderList, indx, 1)
                indx = indx + 1
            End While

            indx = indx + 1

            Ctrl.Items.Add(sTemp)

            sTemp = ""
        End While

    End Sub

    Public Sub GetCardName(ByRef CardNameText As String)

        CardNameText = "UNKNOWN CARD"
        ' atr with old driver
        If ATRLength = 17 Then
            If ATR(1) = 15 Then
                If ATR(16) = 17 Then
                    CardNameText = "Mifare Standard 1K"
                End If
                If ATR(16) = 33 Then
                    CardNameText = "Mifare Standard 4K"
                End If
                If ATR(16) = 49 Then
                    CardNameText = "Mifare Ultra Light"
                End If
                If ATR(16) = 22 Then
                    CardNameText = "AT88RF020"
                End If
                If ATR(16) = 38 Then
                    CardNameText = "AT88SC6416CRF"
                End If
                If ATR(16) = 229 Then
                    CardNameText = "STm SR176"
                End If
                If ATR(16) = 245 Then
                    CardNameText = "STm SRI X4K"
                End If
                If ATR(16) = 24 Then
                    CardNameText = "I.CODE 1"
                End If
                If ATR(16) = 131 Then
                    CardNameText = "iClass"
                End If
                If ATR(16) = 212 Then
                    CardNameText = "KSW TempSens"
                End If
                If ATR(16) = 20 Then
                    CardNameText = "SRF55V10P"
                End If
                If ATR(16) = 180 Then
                    CardNameText = "I.CODE SLI"
                End If
                If ATR(16) = 148 Then
                    CardNameText = "Tag It"
                End If
                If ATR(16) = 164 Then
                    CardNameText = "X-ident STm LRI 512"
                End If
                If ATR(16) = 195 Then
                    CardNameText = "iCLASS 2K"
                End If
                If ATR(16) = 147 Then
                    CardNameText = "iCLASS 2KS"
                End If
                If ATR(16) = 211 Then
                    CardNameText = "iCLASS 16K"
                End If
                If ATR(16) = 163 Then
                    CardNameText = "iCLASS 16KS"
                End If
                If ATR(16) = 227 Then
                    CardNameText = "iCLASS 8x2K"
                End If
                If ATR(16) = 179 Then
                    CardNameText = "iCLASS 8x2KS"
                End If
            End If
        End If
        ' atr with new driver Pc/Sc
        If ATRLength = 20 Then
            If ATR(13) = 0 Then
                If ATR(14) = 1 Then
                    CardNameText = "Mifare Standard 1K"
                End If
                If ATR(14) = 2 Then
                    CardNameText = "Mifare Standard 4K"
                End If
                If ATR(14) = 3 Then
                    CardNameText = "Mifare Ultra Light"
                End If
                If ATR(14) = 4 Then
                    CardNameText = "SLE55R_XXXX"
                End If
                If ATR(14) = 6 Then
                    CardNameText = "SR176"
                End If
                If ATR(14) = 7 Then
                    CardNameText = "SRI_X4K"
                End If
                If ATR(14) = 8 Then
                    CardNameText = "AT88RF020"
                End If
                If ATR(14) = 9 Then
                    CardNameText = "AT88SC0204CRF"
                End If
                If ATR(14) = 10 Then
                    CardNameText = "AT88SC0808CRF"
                End If
                If ATR(14) = 11 Then
                    CardNameText = "AT88SC1616RF"
                End If
                If ATR(14) = 12 Then
                    CardNameText = "AT88SC3216CRF"
                End If
                If ATR(14) = 13 Then
                    CardNameText = "AT88SC6416CRF"
                End If
                If ATR(14) = 14 Then
                    CardNameText = "SRF55V10P"
                End If
                If ATR(14) = 15 Then
                    CardNameText = "SRF55V02P"
                End If
                If ATR(14) = 16 Then
                    CardNameText = "SRF55V10S"
                End If
                If ATR(14) = 17 Then
                    CardNameText = "SRF55V02S"
                End If
                If ATR(14) = 18 Then
                    CardNameText = "TAG_IT"
                End If
                If ATR(14) = 19 Then
                    CardNameText = "LRI512"
                End If
                If ATR(14) = 20 Then
                    CardNameText = "ICODE.SII"
                End If
                If ATR(14) = 21 Then
                    CardNameText = "TEMPSENS"
                End If
                If ATR(14) = 22 Then
                    CardNameText = "i.CODE1"
                End If
                If ATR(14) = 24 Then
                    CardNameText = "iCLASS2KS"
                End If
                If ATR(14) = 26 Then
                    CardNameText = "iCLASS16KS"
                End If
                If ATR(14) = 28 Then
                    CardNameText = "iCLASS8x2KS"
                End If
                If ATR(14) = 29 Then
                    CardNameText = "iCLASS32KS_16_16"
                End If
                If ATR(14) = 30 Then
                    CardNameText = "iCLASS32KS_16_8x2"
                End If
                If ATR(14) = 31 Then
                    CardNameText = "iCLASS32KS_8x2_16"
                End If
                If ATR(14) = 32 Then
                    CardNameText = "iCLASS32KS_8x2_8x2"
                End If
            End If
        End If

    End Sub

    Public Function GetSmartCardResponse() As String
        On Error GoTo RET

        Response = ""
        Dim tempByte As String

        For i As Integer = 0 To ReceiveLength - 1
            tempByte = Hex(ReceiveBuffer(i)).ToString
            Response += " " + IIf(tempByte.Length = 1, "0" + tempByte, tempByte)
        Next
        Return Response

RET:
        Return "Error."
    End Function

    Public Function GetSmartCardResponse2() As String
        On Error GoTo RET

        Response = ""

        Dim tempByte As String

        For i As Integer = 1 To ReceiveLength - 1
            tempByte = Hex(ReceiveBuffer(i)).ToString
            Response += " " + IIf(tempByte.Length = 1, "0" + tempByte, tempByte)
        Next

        Return Response

RET:
        Return "00"
    End Function

    Public Function GetSmartCardResponse3() As String
        On Error GoTo RET

        Response = ""

        Dim tempByte As String

        For i As Integer = 1 To ReceiveLength - 3
            tempByte = Hex(ReceiveBuffer(i)).ToString
            Response += " " + IIf(tempByte.Length = 1, "0" + tempByte, tempByte)
        Next

        Return Response

RET:
        Return "00"
    End Function

    Public Function GetSmartCardResponse4() As String
        On Error GoTo RET

        Response = ""

        Dim tempByte As String

        For i As Integer = 0 To ReceiveLength - 3
            tempByte = Hex(ReceiveBuffer(i)).ToString
            Response += " " + IIf(tempByte.Length = 1, "0" + tempByte, tempByte)
        Next

        Return Response

RET:
        Return "00"
    End Function

    Public Function GetSmartCardResponse5() As String
        On Error GoTo RET

        Response = ""

        Dim tempByte As String

        For i As Integer = ReceiveLength - 2 To ReceiveLength - 1
            tempByte = Hex(ReceiveBuffer(i)).ToString
            Response += " " + IIf(tempByte.Length = 1, "0" + tempByte, tempByte)
        Next

        Return Response

RET:
        Return "00"
    End Function

    Public Sub SendAPDU(ByVal ApduCommand As String)

        ClearBuffers()

        SendRequest.dwProtocol = 0
        SendRequest.cbPciLength = Len(SendRequest)

        Dim i2 As Integer = Len(SendRequest)
        Dim SendByte As String = ""

        LastByteSent = ""

        ReceiveLength = 256

        SendLength = Len(ApduCommand) / 2

        If SendLength > 0 Then
            For i As Integer = 1 To SendLength
                SendByte = Mid(ApduCommand, i * 2 - 1, 2)
                SendBuffer(i - 1) = CInt("&H" + SendByte)
                LastByteSent += " " + SendByte
            Next

            member_CardReturnCode = ModWinsCard.SCardTransmit(hCard, SendRequest, SendBuffer(0), SendLength, SendRequest, ReceiveBuffer(0), ReceiveLength)

            If member_CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
                LastByteReceived = "Error."
            Else
                LastByteReceived = GetSmartCardResponse()
            End If

        End If
    End Sub

    Public Function SmartCardErrorCode() As String

        Dim ErrCode As String = GetSmartCardResponse5.Replace(" ", "")

        Select Case ErrCode

            Case "9000" : Return "Success"
            Case "6581" : Return "Memory Failure"
            Case "6981" : Return "Incompatible Command"
            Case "6982" : Return "Authentication Failed"
            Case "6300" : Return "Authentication Failed"
            Case "6986" : Return "Command not allowed"
            Case "6A81" : Return "Function not supported"
            Case "6A82" : Return "Invalid Block Address"
            Case "6400" : Return "Card execution error"
            Case "6700" : Return "Invalid Length"
            Case "6800" : Return "Invalid Class"
            Case "6B00" : Return "Invalid Parameter"
            Case Else : Return "Unknown error"

        End Select

    End Function

    Public Function SendAPDUandDisplay(ByVal reqType As Integer, ByRef lstLog As ListBox) As Integer

        Dim indx As Integer
        Dim tmpStr As String

        SendRequest.dwProtocol = 2 '2Aprotocol
        SendRequest.cbPciLength = Len(SendRequest)

        ' Display Apdu In
        tmpStr = ""
        For indx = 0 To SendLength - 1

            tmpStr = tmpStr + " " + Format(Hex(SendBuffer(indx)), "")

        Next indx

        DisplayOut(2, 0, tmpStr, lstLog)

        member_CardReturnCode = ModWinsCard.SCardTransmit(hCard, SendRequest, SendBuffer(0), SendLength, SendRequest, ReceiveBuffer(0), ReceiveLength)

        If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then

            DisplayOut(1, CardReturnCode, "", lstLog)
            SendAPDUandDisplay = CardReturnCode
            Exit Function

        Else

            tmpStr = ""
            Select Case reqType

                Case 0  '  Display SW1/SW2 value
                    For indx = (ReceiveLength - 2) To (ReceiveLength - 1)

                        tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "

                    Next indx

                    If Trim(tmpStr) <> "90 00" Then

                        DisplayOut(4, 0, "Return bytes are not acceptable.", lstLog)

                    End If

                Case 1  ' Display ATR after checking SW1/SW2

                    For indx = (ReceiveLength - 2) To (ReceiveLength - 1)

                        tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "

                    Next indx

                    If tmpStr.Trim() <> "90 00" Then

                        tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "

                    Else

                        tmpStr = "ATR : "
                        For indx = 0 To (ReceiveLength - 3)

                            tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "

                        Next indx

                    End If

                Case 2  ' Display all data

                    For indx = 0 To (ReceiveLength - 1)

                        tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "

                    Next indx

                Case 3  ' Interpret SW1/SW2

                    For indx = (ReceiveLength - 2) To (ReceiveLength - 1)

                        tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "

                    Next indx

                    If tmpStr.Trim = "6A 81" Then

                        DisplayOut(0, 0, "The function is not supported.", lstLog)
                        SendAPDUandDisplay = member_CardReturnCode
                        Exit Select

                    End If

                    If tmpStr.Trim = "63 00" Then

                        DisplayOut(0, 0, "The operation failed.", lstLog)
                        SendAPDUandDisplay = CardReturnCode
                        Exit Select

                    End If

                    ValidATS = True

            End Select

            DisplayOut(3, 0, tmpStr.Trim(), lstLog)

        End If

        SendAPDUandDisplay = CardReturnCode

    End Function
    Public Sub InitializeReaders(ByVal cmbReader As ComboBox, ByRef Message As String)

        Dim sReaderList As String = ""
        Dim ReaderCount As Integer
        Dim ctr As Integer

        For ctr = 0 To 1024
            sReaderList = sReaderList + vbNullChar
        Next

        ReaderCount = 1024

        CardReturnCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, hContext)
        If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            Message = "Unable to establish context with card reader."
            Exit Sub
        End If

        'ReaderCount = ModWinsCard.SCARD_E_NO_READERS_AVAILABLE

        CardReturnCode = ModWinsCard.SCardListReaders(hContext, "", sReaderList, ReaderCount)
        If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            Message = "Unable to collect reader list."
            Exit Sub
        End If

        LoadListToControl(cmbReader, sReaderList)

        cmbReader.SelectedIndex = 0

        CurrentReaderName = cmbReader.Text.Trim

        Dim iCtr = 0

        For Each Str As String In cmbReader.Items
            ReaderList(iCtr) = Str
            iCtr += 1
        Next
        cmbReader.Dispose()
    End Sub
    Public Sub GetUID(ByRef Message As String)

        Dim tmpStr As String = ""
        Dim indx As Integer
        Dim lstBoxLog As New ListBox

        ValidATS = False

        ClearBuffers()

        SendBuffer(0) = &HFF                              ' CLA
        SendBuffer(1) = &HCA                              ' INS
        SendBuffer(2) = &H0                               ' P1 : Other cards
        SendBuffer(3) = &H0                               ' P2
        SendBuffer(4) = &H0                               ' Le : Full Length

        SendLength = SendBuffer(4) + 5
        ReceiveLength = &HFF

        CardReturnCode = SendAPDUandDisplay(3, lstBoxLog)

        If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            Message = "Failed to get smart card UID"
        End If

        If ValidATS Then

            For indx = 0 To (ReceiveLength - 3)
                tmpStr = tmpStr + Microsoft.VisualBasic.Right("00" & Hex(ReceiveBuffer(indx)), 2) + " "
            Next indx

            Message = "UID:" + tmpStr.Trim

        End If
        SmartCardUID = tmpStr
        lstBoxLog.Dispose()
    End Sub

    Sub SetCardReader(index As Integer)
        CurrentReaderName = ReaderList(index)
    End Sub

    Public Function ConnectCard(ByVal index As Integer, ByRef Message As String) As Boolean
        CurrentReaderName = ReaderList(index)
        ConnectCard(Message)
        Return ConnectionActive
    End Function

    Public Function ConnectCard(ByVal _CurrentReaderName As String, ByRef Message As String) As Boolean
        CurrentReaderName = _CurrentReaderName
        ConnectCard(Message)
        Return ConnectionActive
    End Function

    Private Sub ConnectCard(ByRef Message As String)
        If ConnectionActive Then
            CardReturnCode = ModWinsCard.SCardDisconnect(hCard, ModWinsCard.SCARD_UNPOWER_CARD)
        End If
        CardReturnCode = ModWinsCard.SCardConnect(hContext, CurrentReaderName, ModWinsCard.SCARD_SHARE_SHARED, ModWinsCard.SCARD_PROTOCOL_T0 Or ModWinsCard.SCARD_PROTOCOL_T1, hCard, Protocol)
        If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            If InStr(CurrentReaderName, "ACR128U SAM") > 0 Then
                CardReturnCode = ModWinsCard.SCardConnect(hContext, CurrentReaderName, ModWinsCard.SCARD_SHARE_DIRECT, 0, hCard, Protocol)
                If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
                    Message = "Unable to connect card."
                    ConnectionActive = False
                    Exit Sub
                Else
                    Message = "Succesful connection with " + CurrentReaderName
                End If
            Else
                Message = "Unable to connect card."
                ConnectionActive = False
                Exit Sub
            End If
        Else
            Message = "Successful connection to " & CurrentReaderName
        End If
        ConnectionActive = True
    End Sub

    Public Sub DisconnectCard(ByVal ReaderName As String)
        '    If ConnectionActive Then
        CardReturnCode = ModWinsCard.SCardDisconnect(hCard, ModWinsCard.SCARD_UNPOWER_CARD)
        If CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            Console.WriteLine("Could not disconnect " + ReaderName)
        End If
        '  End If
        ModWinsCard.SCardReleaseContext(hContext)
        ConnectionActive = False
    End Sub

    Public Sub ReconnectCard()
        CurrentReaderName = CurrentReaderName.ToString().Replace("Successful connection to ", "")
        ConnectionActive = False
        DisconnectCard(CurrentReaderName)
        ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, hContext)
        ConnectCard(CurrentReaderName)

    End Sub

#End Region

    Public Function Reset(ByVal readerName As String) As Boolean
        Dim len As Integer
        Dim state As Integer
        Dim protocol As Integer
        Dim atr As Byte
        Dim atrLen As Integer
        CardReturnCode = ModWinsCard.SCardStatus(hCard, readerName, len, state, protocol, atr, atrLen)
        Return CardReturnCode = ModWinsCard.SCARD_S_SUCCESS
    End Function

    Public Function Reset() As Boolean
        Dim len As Integer
        Dim state As Integer
        Dim protocol As Integer
        Dim atr As Byte
        Dim atrLen As Integer
        CardReturnCode = ModWinsCard.SCardStatus(hCard, CurrentReaderName, len, state, protocol, atr, atrLen)
        Return CardReturnCode = ModWinsCard.SCARD_S_SUCCESS
    End Function



End Class
