Imports System.Text
Imports System.IO

Public Class AllCardTech_UMID2Opertaions

    Dim errorCode As New AllCardTech_UMIDErrorCodes
    Dim PCSC_UMID As AllCardTech_PCSC
    Dim PCSC_SAM As AllCardTech_PCSC
    Dim Util As New AllCardTech_Util
    Dim instanceAID As String = "A000000151000000"
    Dim umidAID As String = "412a2a556e6974546573742a2a2a"
    Dim sssAID As String = "535353534543555245424c4f434b2a2a"
    Dim gsisAID As String = "47534953534543555245424C4F434B2A"
    Dim pagibigAID As String = "5048534543555245424C4F434B2A2A2A"
    Dim philhealthAID As String = "5049534543555245424C4F434B2A2A2A"
    Dim reserve1AID As String = "52455331534543555245424C4F434B2A"
    Dim samAID As String = "554D494453414D31"
    Dim Success = ModWinsCard.SCARD_S_SUCCESS
    Dim LastSelectedEF As SharedData
    Dim isDirty As Boolean = False

    Private Enum SharedData
        EFNONE
        EF1
        EF2
        EF3
        EF4
        EF5
        EF6
        EF7
        EF8
        EF9
    End Enum

    Dim GetFingerPrint As Boolean
    Dim mutualAuthSet As Boolean
    Dim LastAgencySelected As Integer

    Sub New(ByVal PCSC_UMID As AllCardTech_PCSC, ByVal PCSC_SAM As AllCardTech_PCSC)
        Me.PCSC_UMID = PCSC_UMID
        Me.PCSC_SAM = PCSC_SAM
    End Sub

    Function SelectUMIDApplet() As Boolean
        mutualAuthSet = False
        Dim t As Integer = 0

        WriteToLog("SelectUMIDApplet")
        PCSC_SAM.ReconnectCard() ' reset
        PCSC_UMID.ReconnectCard()

        GetFingerPrint = False
        LastSelectedEF = 0
        LastAgencySelected = 0
        PCSC_UMID.SendAPDU("00A404000e" & umidAID)
        WriteToLog("Send APDU: 00A404000e" & umidAID)
        SelectUMIDApplet = PCSC_UMID.LastByteReceived.Contains("90 00")
        WriteToLog("Response: " & PCSC_UMID.Response)
        PCSC_UMID.ConnectionActive = SelectUMIDApplet

        PCSC_SAM.SendAPDU("00A4040008" & samAID)
        WriteToLog("Send APDU: 00A4040008" & samAID)
        SelectUMIDApplet = PCSC_SAM.LastByteReceived.Contains("90 00")
        WriteToLog("Response: " & PCSC_UMID.Response)
        PCSC_SAM.ConnectionActive = SelectUMIDApplet
        Return PCSC_SAM.ConnectionActive And PCSC_UMID.ConnectionActive
    End Function

    Private Function SecureMessagingSelect(elementNo As String, keyNo As String, ByRef err As Byte()) As Boolean
        Dim tmp As String = ""
        PCSC_SAM.SendAPDU("0011" + keyNo + elementNo)
        tmp = PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6)
        PCSC_UMID.SendAPDU(tmp.Replace(" ", ""))
        tmp = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)
        Dim len As Integer = tmp.Replace(" ", "").Length / 2
        PCSC_SAM.SendAPDU(("00080000" + len.ToString("X2") + tmp).Replace(" ", ""))
        err = errorCode.getDescriptionByBytes(PCSC_SAM.LastByteReceived)
        SecureMessagingSelect = PCSC_SAM.LastByteReceived.Contains("90 00")
        Console.WriteLine("SecureMessaging Select :" & SecureMessagingSelect.ToString())
    End Function

    Function SecureMessagingUpdate(ByVal Data As Byte(), ByVal offset As Integer, ByRef err As Byte()) As Boolean
        Dim str As String = ""
        Dim block As Integer = 119
        Dim len As Integer = 0
        Dim remaining As Integer = Data.Length
        Dim pos As Integer = offset
        Dim cursor As Integer = 0

        While remaining > 0

            If remaining <= block Then
                len = remaining
            Else
                len = block
            End If

            Dim sBytes(len - 1) As Byte
            Array.Copy(Data, cursor, sBytes, 0, len)
            Dim tmp As String = ""

            PCSC_SAM.SendAPDU(("0005" + Util.ConvertIntToHexStr(pos, False, False) + Util.ConvertIntToHexStr(len, True, False) + Util.ByteArrayToHexString(sBytes)).Replace(" ", ""))
            tmp = PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6)

            PCSC_UMID.SendAPDU(tmp.Replace(" ", ""))
            tmp = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)

            Dim respLen = tmp.Replace(" ", "").Length / 2
            PCSC_SAM.SendAPDU(("00080000" + Util.ConvertIntToHexStr(respLen, True, True) + tmp).Replace(" ", ""))

            If Not PCSC_SAM.LastByteReceived.Contains("90 00") Then
                err = errorCode.getDescriptionByBytes(PCSC_SAM.LastByteReceived)
                LastAgencySelected = 0
                mutualAuthSet = False
                Console.WriteLine("SecureMessagingUpdate : False")
                Return False
            End If

            remaining -= len
            cursor += len
            pos += len
        End While
        Console.WriteLine("SecureMessagingUpdate : True")
        Return True
    End Function

    Private Function SecureMessagingRead(ByRef Data As Byte(), offset As Integer, len As Integer, err As Byte()) As Boolean
        Dim str As String = ""
        Dim block As Integer = 119
        Dim remaining As Integer = len
        Dim pos As Integer = offset

        While remaining > 0

            If remaining <= block Then
                len = remaining
            Else
                len = block
            End If

            Dim tmp As String = ""

            PCSC_SAM.SendAPDU(("0007" + Util.ConvertIntToHexStr(pos, False, False) + "01" + len.ToString("X2")).Replace(" ", ""))
            tmp = PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6)

            PCSC_UMID.SendAPDU(tmp.Replace(" ", ""))
            tmp = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)

            Dim oLen As Integer = (tmp.Replace(" ", "").Length) / 2
            PCSC_SAM.SendAPDU(("00100000" + oLen.ToString("X2") + tmp).Replace(" ", ""))

            str = str + PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6)
            len = (PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", "").Length) / 2

            If Not PCSC_SAM.LastByteReceived.Contains("90 00") Then
                err = errorCode.getDescriptionByBytes(PCSC_SAM.LastByteReceived)
                LastAgencySelected = 0
                mutualAuthSet = False
                Console.WriteLine("SecureMessagingRead : False")
                Return False
            End If

            remaining = remaining - len
            pos += len

        End While

        Data = Util.HexToBytes(str)
        Console.WriteLine("SecureMessagingRead : True")
        Return True
    End Function

    Function MutualAuthenticateSecurityDomain(ByRef err As Byte(), ByVal keyNo As String) As Boolean
        Dim tmp As String = ""
        Dim serial As String = ""
        Dim random As String = ""

        PCSC_SAM.SendAPDU("80020100")
        PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
        serial = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6).Replace(" ", "")

        PCSC_SAM.SendAPDU("8002020008" & serial)
        PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))

        random = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6).Replace(" ", "")
        PCSC_SAM.SendAPDU("8002030110" + random.Substring(24, 32))

        If Not PCSC_SAM.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("MutualAuthenticateSecurityDomain : False")
            Return False
        End If

        PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
        MutualAuthenticateSecurityDomain = PCSC_UMID.LastByteReceived.Contains("90 00")
        Console.WriteLine("MutualAuthenticateSecurityDomain : " & MutualAuthenticateSecurityDomain.ToString())
    End Function

    Function MutualAuthenticateSecurityDomain(ByRef err As Byte(), ByVal keyNo As String, ByRef is6f00 As Boolean) As Boolean
        Dim tmp As String = ""
        Dim serial As String = ""
        Dim random As String = ""

        PCSC_SAM.SendAPDU("80020100")
        PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
        serial = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6).Replace(" ", "")

        PCSC_SAM.SendAPDU("8002020008" & serial)
        PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))

        random = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6).Replace(" ", "")
        PCSC_SAM.SendAPDU("8002030110" + random.Substring(24, 32))

        If Not PCSC_SAM.LastByteReceived.Contains("90 00") Then

            If PCSC_SAM.LastByteReceived.Contains("6F 00") Then
                is6f00 = True
                Return False
            End If

            Console.WriteLine("MutualAuthenticateSecurityDomain : False")
            Return False
        End If

        PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
        MutualAuthenticateSecurityDomain = PCSC_UMID.LastByteReceived.Contains("90 00")
        Console.WriteLine("MutualAuthenticateSecurityDomain : " & MutualAuthenticateSecurityDomain.ToString())
    End Function

    Public Function DeleteUMIDInstance() As Boolean
        Dim err(1023) As Byte
        PCSC_UMID.Reset()
        PCSC_UMID.SendAPDU("00a4040c08" & instanceAID)
        If MutualAuthenticateSecurityDomain(err, "01") Then
            'A0000000003F0000            
            PCSC_SAM.SendAPDU("00ad00000e80E40080094F07A0000000003F00")
            PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
            If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then

            End If
            Return PCSC_SAM.LastByteReceived.Contains("90 00")
        End If
        Return False
    End Function

    Function ChangeKey() As Boolean
        Dim p1 As String = "01"



        Return False
    End Function

    Function MutualAuthenticate(ByRef err As Byte(), ByVal keyNo As String) As Boolean
        Dim tmp As String = ""
        Dim serial As String = ""
        Dim challenge As String = ""

        PCSC_UMID.SendAPDU("00cb7fff08")
        serial = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)

        PCSC_UMID.SendAPDU("0022C1A40680010C83018200")

        PCSC_UMID.SendAPDU("0084000008")
        challenge = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)

        PCSC_SAM.SendAPDU(("800402" + keyNo + "10" + serial + challenge).Replace(" ", ""))
        tmp = PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6)

        PCSC_UMID.SendAPDU(tmp.Replace(" ", ""))
        tmp = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)

        PCSC_SAM.SendAPDU(("8004030048" + tmp).Replace(" ", ""))
        err = errorCode.getDescriptionByBytes(PCSC_SAM.LastByteReceived)
        MutualAuthenticate = PCSC_SAM.LastByteReceived.Contains("90 00")
        Console.WriteLine("MutualAuthenticate : " + MutualAuthenticate.ToString())
    End Function

    Function AuthenticateRewriting(ByRef Err As Byte()) As Boolean
        AuthenticateRewriting = False

        PCSC_SAM.SendAPDU("00a404000d47534953524557524954455231")

        AuthenticateRewriting = PCSC_SAM.LastByteReceived.Contains("90 00")

        If Not AuthenticateRewriting Then
            GoTo _Error
        End If

        PCSC_SAM.SendAPDU("000a0000")

        Dim q As Integer = BitConverter.ToInt16(Util.HexToBytes(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", "")), 0)
        q = q * 2 + 5

        PCSC_SAM.SendAPDU("00aa000002" + Util.ConvertIntToHexStr(q, False, False))

        AuthenticateRewriting = PCSC_SAM.LastByteReceived.Contains("90 00")

_Error:
        Err = errorCode.getDescriptionByBytes(PCSC_SAM.LastByteReceived.Substring(PCSC_SAM.LastByteReceived.Length - 5, 5))
            Console.WriteLine("AuthenticateSL1 : " & AuthenticateRewriting.ToString())
    End Function

    Function AuthenticateSL1(ByRef Err As Byte(), ByVal keyNo As String) As Boolean
        Dim tmp As String = ""
        Dim serial As String = ""
        PCSC_UMID.SendAPDU("00cb7fff08")
        serial = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)
        PCSC_UMID.SendAPDU("00 22 81 a4 06 80 01 1c 83 01 82".Replace(" ", ""))
        PCSC_UMID.SendAPDU("00 84 00 00 08 ".Replace(" ", ""))
        tmp = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)
        PCSC_SAM.SendAPDU("80 13 02 00 10".Replace(" ", "") + tmp.Replace(" ", "") + serial.Replace(" ", "") + keyNo)
        tmp = PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6)
        PCSC_UMID.SendAPDU(tmp.Replace(" ", ""))
        Err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
        AuthenticateSL1 = PCSC_UMID.LastByteReceived.Contains("90 00")
        Console.WriteLine("AuthenticateSL1 : " & AuthenticateSL1.ToString())
    End Function

    Function AuthenticateSL2(UserPin As Byte(), UserPinLength As Integer, ByRef Err As Byte()) As Boolean
        WriteToLog("AuthenticateSL2")
        Dim pin = Util.ByteArrayToHexString(UserPin).Replace(" ", "")
        PCSC_UMID.SendAPDU("0020008108" + pin.PadRight(16, "0"))
        WriteToLog("Send APDU: 0020008108" + pin.PadRight(16, "0"))
        Err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
        WriteToLog("Response: " & Util.ByteArrayToAscii(Err))
        AuthenticateSL2 = PCSC_UMID.LastByteReceived.Contains("90 00")
        WriteToLog("Response: " & PCSC_UMID.Response)
        Console.WriteLine("AuthenticateSL2 : " & AuthenticateSL2.ToString())
        WriteToLog("AuthenticateSL2 : " & AuthenticateSL2.ToString())
    End Function

    Private Sub WriteToLog(ByVal data As String)
        '"D:\apdu_log.txt", True)
        Dim fileLog As String = "C:\Allcard\SSS UMID\apdu_log.txt"

        'Dim sw As New StreamWriter(System.Windows.Forms.Application.StartupPath & "\apdu_log.txt", True)
        Dim sw As New StreamWriter(fileLog, True)
        sw.WriteLine(data)
        sw.Close()
        sw.Dispose()
        sw = Nothing
    End Sub

    Function AuthenticateSL3(ByRef Err As Byte()) As Boolean
        If GetFingerPrint Then
            If AuthenticateSL1(Err, "02") Then
                Console.WriteLine("AuthenticateSL3 : True")
                Return True
            End If
        Else
            Err = Util.AsciiToByteArray("Fingerprint matching is required.")
        End If
        Console.WriteLine("AuthenticateSL3 : False")
        Return False
    End Function

    'Function UMIDCard_Change_PIN(ByVal UserPin As Byte(), ByVal iUserPin As Integer, ByRef ErrorMessage As Byte()) As Boolean

    '    Dim UserPinLength As Integer = iUserPin
    '    If (UserPinLength > 8) Then
    '        ErrMessage = Util.AsciiToByteArray("Invalid Pin Length")
    '        Console.WriteLine("UMIDCARD_Activate : False")
    '        Return False
    '    End If

    '    'change pin using userpin
    '    'the actual pin size is 8 just pad with 00 if length provided is less        
    '    Dim pin = Util.ByteArrayToHexString(UserPin)
    '    While (Not (Len(pin) / 2) Mod 8 = 0)
    '        pin += "0"
    '    End While

    '    PCSC_UMID.SendAPDU("00240081103132333435363738" + pin)
    '    If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
    '        Console.WriteLine("UMIDCARD_Activate : False")
    '        Return False
    '    End If
    'End Function

    Function UMIDCARD_Activate(UserPin As Byte(), UserPinLength As Integer, ByRef ErrMessage As Byte()) As Boolean
        If Not GetFingerPrint Then
            ErrMessage = Util.AsciiToByteArray("Fingerprint Matching is Required")
            Return False
        End If
        If (UserPinLength > 8) Then
            ErrMessage = Util.AsciiToByteArray("Invalid Pin Length")
            Return False
        End If

        'change pin using userpin
        'the actual pin size is 8 just pad with 00 if length provided is less        
        Dim pin = Util.ByteArrayToHexString(UserPin)
        While (Not (Len(pin) / 2) Mod 8 = 0)
            pin += "0"
        End While

        PCSC_UMID.SendAPDU("00240081103132333435363738" + pin)
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            PCSC_UMID.SendAPDU("00240081103132333435360000" + pin)
            If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                Console.WriteLine("UMIDCARD_Activate : False")
                Return False
            End If
        End If

        'activate shared EFS        

        PCSC_UMID.Reset()
        PCSC_SAM.SendAPDU("00A4040c08" & samAID)
        PCSC_UMID.SendAPDU("00A4040c0e" & umidAID)
        AuthenticateSL1(ErrMessage, "02")

        'EF2
        PCSC_UMID.SendAPDU("00a40204020002")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If
        'EF3
        PCSC_UMID.SendAPDU("00a40204020003")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If
        'EF4
        PCSC_UMID.SendAPDU("00a40204020004")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If
        'EF5
        PCSC_UMID.SendAPDU("00a40204020005")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If
        'EF6
        PCSC_UMID.SendAPDU("00a40204020006")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If
        'EF8
        PCSC_UMID.SendAPDU("00a40204020008")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If
        'EF9
        PCSC_UMID.SendAPDU("00a40204020009")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If

        'activate agency EFS

        'sss
        PCSC_UMID.Reset()
        PCSC_SAM.SendAPDU("00A4040c08" & samAID)
        PCSC_UMID.SendAPDU("00A4040c10" & sssAID)
        AuthenticateSL1(ErrMessage, "03")

        PCSC_UMID.SendAPDU("00a40204025301")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If

        'gsis        

        PCSC_UMID.Reset()
        PCSC_SAM.SendAPDU("00A4040c08" & samAID)
        PCSC_UMID.SendAPDU("00A4040c10" & gsisAID)
        AuthenticateSL1(ErrMessage, "04")

        PCSC_UMID.SendAPDU("00a40204024701")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If

        'pagibig        
        PCSC_UMID.Reset()
        PCSC_SAM.SendAPDU("00A4040c08" & samAID)
        PCSC_UMID.SendAPDU("00A4040c10" & pagibigAID)
        AuthenticateSL1(ErrMessage, "05")

        PCSC_UMID.SendAPDU("00a40204025001")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If

        'philhealth
        'SelectUMIDApplet()
        PCSC_UMID.Reset()
        PCSC_SAM.SendAPDU("00A4040c08" & samAID)
        PCSC_UMID.SendAPDU("00A4040c10" & philhealthAID)
        AuthenticateSL1(ErrMessage, "06")

        PCSC_UMID.SendAPDU("00a40204024901")
        PCSC_UMID.SendAPDU("0044000000")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If

        'reserve1
        'PCSC_UMID.Reset()
        'PCSC_SAM.SendAPDU("00A4040c08" & samAID)
        'PCSC_UMID.SendAPDU("00A4040c10" & reserve1AID)
        'AuthenticateSL1(ErrMessage, "02")

        'PCSC_UMID.SendAPDU("00a4020402FF01")
        'PCSC_UMID.SendAPDU("0044000000")
        'If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
        '    Console.WriteLine("UMIDCARD_Activate : False")
        '    Return False
        'End If

        PCSC_UMID.Reset()

        PCSC_UMID.SendAPDU("00a4040008" & instanceAID)
        If PCSC_UMID.LastByteReceived.Contains("90 00") Then

            Dim is6F00 As Boolean
            If Not MutualAuthenticateSecurityDomain(ErrMessage, "01", is6F00) Then
                If Not is6F00 Then
                    Console.WriteLine("UMIDCARD_Activate : False")
                    Return False
                End If
            End If

            If Not is6F00 Then
                'SET TO SECURED
                PCSC_SAM.SendAPDU("00ad00000c80f0800F08" & instanceAID)
                PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
                If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                    ErrMessage = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
                    Console.WriteLine("UMIDCARD_Activate : False")
                    Return False
                End If
            End If

        Else
            Console.WriteLine("UMIDCARD_Activate : False")
            Return False
        End If

        SelectUMIDApplet()
        Console.WriteLine("UMIDCARD_Activate : True")
        Return True
    End Function

    Function Read_CRN(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 0
        Dim len As Integer = 12
        Dim b As Boolean = ReadSharedData(SharedData.EF1, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    Function ReadCSN(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 12
        Dim len As Integer = 20
        Dim b As Boolean = ReadSharedData(SharedData.EF1, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    Function ReadCardCreationDate(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 32
        Dim len As Integer = 8
        Dim b As Boolean = ReadSharedData(SharedData.EF1, tmp, offset, len, Err)
        If (b) Then            
            Data = Util.HexToBytes(tmp)
            If (Data(4) = 0 And Data(5) = 0 And Data(6) = 0 And Data(7) = 0) Then
                Data = Util.AsciiToByteArray(tmp.Replace(" ", "").Substring(0, 8))
            End If
        End If
        Return b
    End Function

    ' Name - First		1	Char	40
    Function ReadFirstName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 0
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    ' Name - Middle		1	Char	40
    Function ReadMiddleName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 40
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    'Name - Last		1	Char	40
    Function ReadLastName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 80
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    ' 5	Name - Suffix		1	Char	10
    Function ReadSuffix(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 120
        Dim len As Integer = 10
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '6	Address - Postal Code		11	Char	6
    Function ReadPostalCode(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 130
        Dim len As Integer = 6
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '7	Address - Country (3 letter code)		11	Char	3
    Function ReadCountry(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 136
        Dim len As Integer = 3
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '8	Address - Province/State		11	Char	30
    Function ReadProvinceState(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 139
        Dim len As Integer = 30
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '9	Address - City/Municipality		11	Char	30
    Function ReadCityMunicipality(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""

        Dim offset As Integer = 169
        Dim len As Integer = 30
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '10	Address - Barangay/District/Locality		11	Char	30
    Function ReadBarangayDistrictLocality(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""

        Dim offset As Integer = 199
        Dim len As Integer = 30
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '11	Address - Subdivision (if applicable)		11	Char	40
    Function ReadSubdivision(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 229
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '12	Address - Street Name		11	Char	40
    Function ReadStreetName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 269
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '13	Address - House or Lot and Block Number		11	Char	15
    Function ReadHouseLotBlock(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""

        Dim offset As Integer = 309
        Dim len As Integer = 15
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '14	Address - Rm/Flr/Unit No and Bldg Name (if applicable)		11	Char	40
    Function ReadRmFlrUnitBldg(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 324
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '15	Gender		1	Char	1
    Function ReadGender(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 364
        Dim len As Integer = 1
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '16	Date of Birth YYYYMMDD+B47		1	Date	8
    Function ReadDateofBirth(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 365
        Dim len As Integer = 8
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '33	Left primary finger code		12	Char	1
    Function ReadLeftPrimary(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 373
        Dim len As Integer = 1
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '34	Right primary finger code		12	Char	1
    Function ReadRightPrimary(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 374
        Dim len As Integer = 1
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    '35	Left backup finger code		12	Char	1
    Function ReadLeftbackup(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 375
        Dim len As Integer = 1
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '36	Right backup finger code		12	Char	1
    Function ReadRightbackup(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 376
        Dim len As Integer = 1
        Dim b As Boolean = ReadSharedData(SharedData.EF3, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '17	Place of Birth - City/Municipality		11	Char	30
    Function ReadPlaceofBirthCityMunicipality(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 0
        Dim len As Integer = 30
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '18	Place of Birth - Province/State		11	Char	30
    Function ReadPlaceofBirthProvinceState(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 30
        Dim len As Integer = 30
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '19	Place of Birth - Country		11	Char	3
    Function ReadPlaceofBirthCountry(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 60
        Dim len As Integer = 3
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '21	Father's Name - First		11	Char	40
    Function ReadFatherFirstName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 63
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '22	Father's Name - Middle		11	Char	40
    Function ReadFatherMiddleName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 103
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '23	Father's Name - Last		11	Char	40
    Function ReadFatherLastName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 143
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '24	Father's Name - Suffix		11	Char	10
    Function ReadFatherSuffix(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 183
        Dim len As Integer = 10
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '25	Mother's Name - First		11	Char	40
    Function ReadMotherFirstName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 193
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '26	Mother's Name - Middle		11	Char	40
    Function ReadMotherMiddleName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 233
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '27	Mother's Name - Last		11	Char	40
    Function ReadMotherLastName(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 273
        Dim len As Integer = 40
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '28	Mother's Name - Suffix		11	Char	10
    Function ReadMotherSuffix(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 313
        Dim len As Integer = 10
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    '32	Tax Identification Number (TIN)		11	Char	10
    Function ReadTIN(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 323
        Dim len As Integer = 10
        Dim b As Boolean = ReadSharedData(SharedData.EF8, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    Function ReadMaritalStatus(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 0
        Dim len As Integer = 1
        Dim b As Boolean = ReadSharedData(SharedData.EF2, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    'Height (in centimeters)		11	Numeric	3
    Function ReadHeight(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 0
        Dim len As Integer = 3
        Dim b As Boolean = ReadSharedData(SharedData.EF5, tmp, offset, len, Err)
        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    'Weight (in kilograms)		11	Numeric	3
    Function ReadWeight(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 3
        Dim len As Integer = 3
        Dim b As Boolean = ReadSharedData(SharedData.EF5, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function
    'Distinguishing Features		11	Char (Long)	70
    Function ReadDistinguishingFeatures(ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        Dim tmp As String = ""
        Dim offset As Integer = 6
        Dim len As Integer = 70
        Dim b As Boolean = ReadSharedData(SharedData.EF5, tmp, offset, len, Err)

        If (b) Then
            Data = Util.HexToBytes(tmp)
        End If
        Return b
    End Function

    Function GetPicture(DumpPath As String, ErrMessage As Byte()) As Boolean
        Dim data As Byte() = Nothing
        If GetBioData(SharedData.EF6, data, 15360, 0, ErrMessage) Then
            File.WriteAllBytes(DumpPath, data)
            Return True
        End If
        Return False
    End Function

    Function GetLeftPrimaryFingerPrint(DumpPath As String, ErrMessage As Byte()) As Boolean
        Dim data As Byte() = Nothing
        If GetBioData(SharedData.EF7, data, 1024, 0, ErrMessage) Then
            Console.WriteLine(Util.ByteArrayToHexString(data))
            GetFingerPrint = True
            Dim len = Integer.Parse(Util.ByteArrayToHexString(data).Substring(0, 4).Replace(" ", ""))
            If len = 0 Then
                Return False

            End If
            Dim newData(len - 1) As Byte
            Array.Copy(data, 2, newData, 0, len - 1)
            File.WriteAllBytes(DumpPath, newData)

            Return True
        End If
        Return False
    End Function

    Function GetRightPrimaryFingerPrint(DumpPath As String, ErrMessage As Byte()) As Boolean
        Dim data As Byte() = Nothing
        If GetBioData(SharedData.EF7, data, 1024, 1024, ErrMessage) Then
            Console.WriteLine(Util.ByteArrayToHexString(data))
            GetFingerPrint = True
            Dim len = Integer.Parse(Util.ByteArrayToHexString(data).Substring(0, 4).Replace(" ", ""))
            If len = 0 Then
                Return False

            End If
            Dim newData(len - 1) As Byte
            Array.Copy(data, 2, newData, 0, len - 1)
            File.WriteAllBytes(DumpPath, newData)
            Return True
        End If
        Return False
    End Function

    Function GetLeftSecondaryFingerPrint(DumpPath As String, ErrMessage As Byte()) As Boolean
        Dim data As Byte() = Nothing
        If GetBioData(SharedData.EF7, data, 1024, 2048, ErrMessage) Then
            Console.WriteLine(Util.ByteArrayToHexString(data))
            GetFingerPrint = True
            Dim len = Integer.Parse(Util.ByteArrayToHexString(data).Substring(0, 4).Replace(" ", ""))
            If len = 0 Then
                Return False
            End If
            Dim newData(len - 1) As Byte
            Array.Copy(data, 2, newData, 0, len - 1)
            File.WriteAllBytes(DumpPath, newData)
            Return True
        End If
        Return False
    End Function

    Function GetRightSecondaryFingerPrint(DumpPath As String, ErrMessage As Byte()) As Boolean
        Dim data As Byte() = Nothing
        If GetBioData(SharedData.EF7, data, 1024, 3072, ErrMessage) Then
            Console.WriteLine(Util.ByteArrayToHexString(data))
            GetFingerPrint = True
            Dim len = Integer.Parse(Util.ByteArrayToHexString(data).Substring(0, 4).Replace(" ", ""))
            If len = 0 Then
                Return False

            End If
            Dim newData(len - 1) As Byte
            Array.Copy(data, 2, newData, 0, len - 1)
            File.WriteAllBytes(DumpPath, newData)
            Return True
        End If
        Return False
    End Function

    Function GetSignature(DumpPath As String, ErrMessage As Byte()) As Boolean
        Dim data As Byte() = Nothing
        If GetBioData(SharedData.EF4, data, 1536, 0, ErrMessage) Then
            File.WriteAllBytes(DumpPath, data)
            Return True
        End If
        Return False
    End Function

    Private Function ReadSharedData(sharedData As SharedData, ByRef Data As String, ByVal offset As Integer, ByVal len As Integer, ByRef Err As Byte()) As Boolean
        Dim cmd = New String("00B0")
        Dim tmpLastSelectedEF As SharedData
        Select Case sharedData
            Case AllCardTech_UMID2Opertaions.SharedData.EF1
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + Util.ConvertIntToHexStr(len, True, False)) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020001")
                    ' AuthenticateSL1()                    
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF1
            Case AllCardTech_UMID2Opertaions.SharedData.EF2
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020002")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF2

            Case AllCardTech_UMID2Opertaions.SharedData.EF3
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020003")
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF3
                'PCSC_UMID.SendAPDU("00a40204020003")
                'AuthenticateSL1()
                'PCSC_UMID.SendAPDU(cmd + "0179") '377 BYTE
            Case AllCardTech_UMID2Opertaions.SharedData.EF4
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020004")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF4
            Case AllCardTech_UMID2Opertaions.SharedData.EF5

                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020005")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF5
            Case AllCardTech_UMID2Opertaions.SharedData.EF6
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020006")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF6
            Case AllCardTech_UMID2Opertaions.SharedData.EF7
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020007")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF7
            Case AllCardTech_UMID2Opertaions.SharedData.EF8
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020008")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF8
            Case AllCardTech_UMID2Opertaions.SharedData.EF9
                If LastSelectedEF = sharedData Then
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                Else
                    PCSC_UMID.SendAPDU("00a40204020009")
                    'AuthenticateSL1()
                    PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(offset, False, True) + len.ToString("X2")) '40 BYTE
                End If
                tmpLastSelectedEF = AllCardTech_UMID2Opertaions.SharedData.EF9
        End Select

        If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
            Data = PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6)
            LastSelectedEF = tmpLastSelectedEF
            Console.WriteLine("ReadSharedData : True")
            Return True
        Else
            Err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
            Console.WriteLine("ReadSharedData : False")
            Return False
        End If
    End Function

    Private Function GetBioData(sharedData As SharedData, ByRef oData As Byte(), pLen As Integer, pOffset As Integer, ByRef err As Byte()) As Boolean

        Select Case sharedData
            Case AllCardTech_UMID2Opertaions.SharedData.EF4
                PCSC_UMID.SendAPDU("00a40204020004")
            Case AllCardTech_UMID2Opertaions.SharedData.EF6
                PCSC_UMID.SendAPDU("00a40204020006")
            Case AllCardTech_UMID2Opertaions.SharedData.EF7
                PCSC_UMID.SendAPDU("00a40204020007")
        End Select

        Dim cmd = New String("00B0")
        Dim block As Integer = 255
        Dim pos As Integer = pOffset
        Dim cur As Integer = 0
        Dim remaining As Integer = pLen
        ReDim oData(pLen)
        While (remaining > 0)
            If remaining > block Then
                '00b000ffff
                PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(pos, False, True) + block.ToString("X2"))
            Else
                PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(pos, False, True) + remaining.ToString("X2"))
            End If
            If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
                Dim tmp = Util.HexToBytes(PCSC_UMID.LastByteReceived.Replace(" ", ""))
                Array.Copy(tmp, 0, oData, cur, tmp.Length - 2)
                remaining = remaining - (tmp.Length - 2)
                pos = pos + (tmp.Length - 2)
                cur = cur + (tmp.Length - 2)
            Else
                err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
                Console.WriteLine("Get Bio Data: False")
                Return False
            End If
        End While
        Console.WriteLine("Get Bio Data: True")
        Return True
    End Function

    Function UMIDCard_Update_Height(data As Byte(), ErrorMessage As Byte()) As Boolean
        PCSC_UMID.SendAPDU("00a40204020005")
        MutualAuthenticate(ErrorMessage, "02")
        If (SecureMessagingUpdate(Util.HexToBytes(data(0).ToString("X2") + data(1).ToString("X2") + data(2).ToString("X2")),
                                  0, ErrorMessage)) Then
            Return True
        Else
            Return False
        End If
        SelectUMIDApplet()
    End Function

    Function UMIDCard_Update_Weight(data As Byte(), ErrorMessage As Byte()) As Boolean
        PCSC_UMID.SendAPDU("00a40204020005")
        MutualAuthenticate(ErrorMessage, "02")
        If (SecureMessagingUpdate(Util.HexToBytes(data(0).ToString("X2") + data(1).ToString("X2") + data(2).ToString("X2")),
                                  3, ErrorMessage)) Then
            Return True
        Else
            Return False
        End If
        SelectUMIDApplet()
    End Function

    Function UMIDCard_Update_Distinguishing(b As Byte(), ErrorMessage As Byte()) As Boolean
        PCSC_UMID.SendAPDU("00a40204020005")
        MutualAuthenticate(ErrorMessage, "02")
        If (SecureMessagingUpdate(b, 6, ErrorMessage)) Then
            Return True
        Else
            Return False
        End If
        SelectUMIDApplet()
    End Function

    Function UMIDCard_Update_Marital(data As Byte(), ErrorMessage As Byte()) As Boolean
        PCSC_UMID.SendAPDU("00a40204020002")
        MutualAuthenticate(ErrorMessage, "02")
        If (SecureMessagingUpdate(data, 0, ErrorMessage)) Then
            Return True
        Else
            Return False
        End If
        'PCSC_UMID.SendAPDU("00D6000001" + data(0).ToString("X2"))
        'If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
        '    Return True
        'Else
        '    ErrorMessage = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
        '    Return False
        'End If
        SelectUMIDApplet()
    End Function

    Private Function AuthReadWriteSector(ByVal agencyNo As Integer, ByVal keyNo As String, ByVal index As Integer, ByRef Err() As Byte) As Boolean
        If agencyNo = 1 Then
            If Not LastAgencySelected = 1 Then
                PCSC_UMID.Reset()
                PCSC_UMID.SendAPDU("00a4040010" & sssAID)
                If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                    Return False
                End If

                If Not mutualAuthSet Then
                    If Not MutualAuthenticate(Err, keyNo) Then
                        Return False
                    End If
                    mutualAuthSet = True
                End If

                LastAgencySelected = 1
            End If
        ElseIf agencyNo = 2 Then
            If Not LastAgencySelected = 2 Then
                PCSC_UMID.Reset()
                PCSC_UMID.SendAPDU("00a4040010" & gsisAID)

                If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                    Return False
                End If
                If Not mutualAuthSet Then
                    If Not MutualAuthenticate(Err, keyNo) Then
                        Return False
                    End If
                    mutualAuthSet = True
                End If
                LastAgencySelected = 2
            End If
        ElseIf agencyNo = 3 Then
            If Not LastAgencySelected = 3 Then
                PCSC_UMID.Reset()
                PCSC_UMID.SendAPDU("00a4040010" & pagibigAID)

                If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                    Return False
                End If

                If Not mutualAuthSet Then
                    If Not MutualAuthenticate(Err, keyNo) Then
                        Return False
                    End If
                    mutualAuthSet = True
                End If

                LastAgencySelected = 3
            End If
        ElseIf agencyNo = 4 Then
            If Not LastAgencySelected = 4 Then
                PCSC_UMID.Reset()
                PCSC_UMID.SendAPDU("00a4040010" & philhealthAID)

                If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                    Return False
                End If

                If Not mutualAuthSet Then
                    If Not MutualAuthenticate(Err, keyNo) Then
                        Return False
                    End If
                    mutualAuthSet = True
                End If

                LastAgencySelected = 4
            End If
        End If

        If Not SecureMessagingSelect(index.ToString("X2"), keyNo, Err) Then
            Return False
        Else
            Return True
        End If

    End Function


    Public Function ReadSector(keyNo As String, index As Integer, ByRef Data As Byte(), iOffSet As Integer, iLength As Integer, agencyNo As Integer, ByRef err As Byte()) As Boolean
        If AuthReadWriteSector(agencyNo, keyNo, index, err) Then
            ReadSector = SecureMessagingRead(Data, iOffSet, iLength, err)
        Else
            ReadSector = False
        End If
    End Function

    Public Function WriteSector(keyNo As String, index As Integer, pData As Byte(), iOffset As Integer, iLength As Integer, agencyNo As Integer, Err As Byte()) As Boolean
        If AuthReadWriteSector(agencyNo, keyNo, index, Err) Then
            WriteSector = SecureMessagingUpdate(pData, iOffset, Err)
        Else
            WriteSector = False
        End If
    End Function

    Function BlockCard(err As Byte()) As Boolean
        PCSC_UMID.Reset()
        PCSC_UMID.SendAPDU("00a4040008" & instanceAID)
        If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
            If MutualAuthenticateSecurityDomain(err, "01") Then
                PCSC_SAM.SendAPDU("00ad00000c80f080ff08" & instanceAID)
                PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
                If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Function GetStatus(ByRef status As String, err As Byte()) As Boolean
        PCSC_UMID.Reset()
        PCSC_UMID.SendAPDU("00a4040008" & instanceAID)
        If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
            Dim is6F00 As Boolean = False
            If MutualAuthenticateSecurityDomain(err, "01", is6F00) Then
                'PCSC_SAM.SendAPDU("00ad00000c80f0800708" & instanceAID)
                PCSC_SAM.SendAPDU("00ad00000c80f2800008" & instanceAID)
                PCSC_UMID.SendAPDU(PCSC_SAM.LastByteReceived.Substring(0, PCSC_SAM.LastByteReceived.Length - 6).Replace(" ", ""))
                If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
                    If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 11, 3).Contains("0F") Then
                        status = "CARD_ACTIVE"
                    ElseIf PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 11, 3).Contains("FF") Then
                        status = "CARD_BLOCKED"
                    ElseIf PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 11, 8).Contains("07") Then
                        status = "CARD_INACTIVE"
                    Else
                        status = "ERROR"
                    End If
                    GetStatus = True
                Else
                    GetStatus = False
                End If
            Else


                PCSC_UMID.ReconnectCard()
                PCSC_UMID.SendAPDU("00A404000e" & umidAID)
                If PCSC_UMID.LastByteReceived.Contains("90 00") And AuthenticateSL1(err, "02") Then
                    Dim data(100) As Byte
                    If ReadFirstName(data, err) Then
                        GetStatus = True
                        status = "CARD_ACTIVE"
                    Else
                        status = "CARD_INACTIVE"
                    End If
                    Return True
                End If
                GetStatus = False


            End If
        Else
            If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "6A 82" Then
                status = "CARD_BLOCKED"
                GetStatus = True
            Else
                GetStatus = False
            End If
        End If
        SelectUMIDApplet()
    End Function

    Function UMIDCARD_ChangePin(oldData As Byte(), newData As Byte(), ByRef ErrMessage() As Byte) As Boolean

        If (oldData.Length > 8 Or newData.Length > 8) Then
            ErrMessage = Util.AsciiToByteArray("Invalid Pin Length")
            Console.WriteLine("UMIDCARD_ChangePin : False")
            Return False
        End If

        'change pin using userpin
        'the actual pin size is 8 just pad with 00 if length provided is less        
        Dim pin = Util.ByteArrayToHexString(newData)
        While (Not (Len(pin) / 2) Mod 8 = 0)
            pin += "0"
        End While

        Dim oldpin = Util.ByteArrayToHexString(oldData)
        While (Not (Len(oldpin) / 2) Mod 8 = 0)
            oldpin += "0"
        End While

        PCSC_UMID.SendAPDU("0024008110" + oldpin + pin)
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_ChangePin : False")
            Return False
        End If
        Console.WriteLine("UMIDCARD_ChangePin : True")
        Return True
    End Function


    Function ReadReserve1(iOffSet As Integer, iLength As Integer, ByRef Data As Byte(), ByRef Err As Byte()) As Boolean
        PCSC_UMID.Reset()
        PCSC_SAM.Reset()

        PCSC_SAM.SendAPDU("00A4040008" & samAID)
        If Not PCSC_SAM.LastByteReceived.Contains("90 00") Then
            Err = Util.AsciiToByteArray("UMID SAM not found")
            Return False
        End If

        PCSC_UMID.SendAPDU("00a4040010" & reserve1AID)
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_ChangePin : False")
            Err = Util.AsciiToByteArray("Reserve 1 File not found")
            Return False
        End If

        If Not (AuthenticateSL1(Err, "02")) Then
            Return False
        End If

        PCSC_UMID.SendAPDU("00a4020402FF01")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Err = Util.AsciiToByteArray("Reserve 1 File not found")
            Return False
        End If

        Dim cmd = New String("00B0")

        PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(iOffSet, False, True) + Util.ConvertIntToHexStr(iLength, True, False))

        If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
            Data = Util.HexToBytes(PCSC_UMID.LastByteReceived.Substring(0, PCSC_UMID.LastByteReceived.Length - 6))
            Console.WriteLine("ReadReserve1 : True")
            Return True
        Else
            Err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
            Console.WriteLine("ReadReserve1 : False")
            Return False
        End If

    End Function

    Function WriteReserve1(iOffSet As Integer, iLength As Integer, Data As Byte(), ByRef Err As Byte()) As Boolean
        PCSC_UMID.Reset()
        PCSC_SAM.Reset()

        PCSC_SAM.SendAPDU("00A4040008" & samAID)
        If Not PCSC_SAM.LastByteReceived.Contains("90 00") Then
            Err = Util.AsciiToByteArray("UMID SAM not found")
            Return False
        End If

        PCSC_UMID.SendAPDU("00a4040010" & reserve1AID)
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Console.WriteLine("UMIDCARD_ChangePin : False")
            Err = Util.AsciiToByteArray("Reserve 1 File not found")
            Return False
        End If

        If Not (AuthenticateSL1(Err, "02")) Then
            Return False
        End If

        PCSC_UMID.SendAPDU("00a4020402FF01")
        If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
            Err = Util.AsciiToByteArray("Reserve 1 File not found")
            Return False
        End If

        Dim cmd = New String("00D6")

        PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(iOffSet, False, True) + Util.ConvertIntToHexStr(iLength, True, False) + Util.ByteArrayToHexString(Data))

        If PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5) = "90 00" Then
            Console.WriteLine("ReadReserve1 : True")
            Return True
        Else
            Err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived.Substring(PCSC_UMID.LastByteReceived.Length - 5, 5))
            Console.WriteLine("ReadReserve1 : False")
            Return False
        End If

    End Function

    Function RewriteFingerprint(iOffSet As Integer, Data As Byte(), ByRef Err As Byte()) As Boolean

        Dim str As String = Util.ByteArrayToHexString(Data)
        str = (str.Length / 2).ToString().PadLeft(4, "0") + str
        str = str.PadRight(2048, "0")

        Data = Util.HexToBytes(str)

        Dim block As Integer = 200
        Dim len As Integer = 0
        Dim remaining As Integer = Data.Length
        Dim pos As Integer = iOffSet
        Dim cursor As Integer = 0

        If (remaining > 1024) Then
            Err = Util.AsciiToByteArray("Invalid Fingerprint data size")
            Return False
        End If

        PCSC_UMID.SendAPDU("00a40204020007")

        While remaining > 0

            If remaining <= block Then
                len = remaining
            Else
                len = block
            End If

            Dim sBytes(len - 1) As Byte
            Array.Copy(Data, cursor, sBytes, 0, len)
            Dim tmp As String = ""

            Dim cmd = New String("00D6")
            PCSC_UMID.SendAPDU(cmd + Util.ConvertIntToHexStr(pos, False, True) + Util.ConvertIntToHexStr(len, True, False) + Util.ByteArrayToHexString(sBytes).Replace(" ", ""))

            If Not PCSC_UMID.LastByteReceived.Contains("90 00") Then
                Err = errorCode.getDescriptionByBytes(PCSC_UMID.LastByteReceived)
                Console.WriteLine("RewriteFingerprint : False")
                Return False
            End If

            remaining -= len
            cursor += len
            pos += len
        End While
        Console.WriteLine("RewriteFingerprint : True")
        Return True

    End Function



End Class

