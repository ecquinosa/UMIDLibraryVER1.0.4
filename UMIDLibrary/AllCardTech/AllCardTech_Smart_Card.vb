Imports System.Windows.Forms
Imports UMIDLibrary.umid

Public Class AllCardTech_Smart_Card

    Dim iUMID As Integer
    Dim iSAM As Integer

    Public Sub Dispose()
        GC.SuppressFinalize(Me)
    End Sub

    Private Sub Log(ByVal l_What As String)
        If l_What Is Nothing Then
            Return
        End If

        If l_What.Contains("Info:") Then
            Message = l_What
        ElseIf l_What.Contains("Error:") Then
            member_Exception = l_What
        End If

        l_What = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff") & "(" & l_What & ")"
        Console.WriteLine(l_What)

        Dim fileLog As String = "C:\Allcard\SSS UMID\UMID_LOG.txt"

        If Not IO.File.Exists(fileLog) Then IO.File.Create(fileLog).Close()

        Dim sw As New IO.StreamWriter(fileLog, True)
        sw.WriteLine(l_What)
        sw.Flush()
        sw.Close()
    End Sub

#Region "PCSC Routines"

    Private mPCSC As New AllCardTech_PCSC
    Private mPCSC_UMID As New AllCardTech_PCSC
    Private mPCSC_SAM As New AllCardTech_PCSC

    Private mUMID2 As New AllCardTech_UMID2Opertaions(mPCSC_UMID, mPCSC_SAM)
    Private util As New AllCardTech_Util()
    Private member_Message As String
    Private member_CurrentReaderName As String
    Private member_SmartCardUID As String
    Private SL1 As Boolean = False

    Public Shared UMID0 As String = "1"
    Public Shared UMID1 As String = "2"

    Private OldUMIDVersion As Integer = 1
    Private NewUMIDVersion As Integer = 2

    Public Event CardInserted()

    Public ReaderList(19) As String

    Public Property Message() As String
        Get
            Return member_Message
        End Get
        Set(ByVal value As String)
            member_Message = value
        End Set
    End Property

    Public Property CurrentReaderName() As String
        Get
            Return member_CurrentReaderName
        End Get
        Set(ByVal value As String)
            member_CurrentReaderName = value
        End Set
    End Property

    Public Property SmartCardUID() As String
        Get
            Return member_SmartCardUID
        End Get
        Set(ByVal value As String)
            member_SmartCardUID = value
        End Set
    End Property

    Public ReadOnly Property CardConnected() As Boolean
        Get
            Return mPCSC.ConnectionActive
        End Get
    End Property

    Public Sub InitializeReaders()
        Dim cmbReader As New ComboBox
        mPCSC_SAM.InitializeReaders(cmbReader, Message)
        mPCSC_UMID.InitializeReaders(cmbReader, Message)

        Dim sReaderList As String = ""
        Dim ReaderCount As Integer
        Dim ctr As Integer

        For ctr = 0 To 1024
            sReaderList = sReaderList + vbNullChar
        Next

        ReaderCount = 1024

        mPCSC.CardReturnCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, mPCSC.hContext)
        If mPCSC.CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            Log("Error: Unable to establish context with card reader.")
            Exit Sub
        End If

        'ReaderCount = ModWinsCard.SCARD_E_NO_READERS_AVAILABLE

        mPCSC.CardReturnCode = ModWinsCard.SCardListReaders(mPCSC.hContext, "", sReaderList, ReaderCount)
        If mPCSC.CardReturnCode <> ModWinsCard.SCARD_S_SUCCESS Then
            Log("Error: Unable to collect reader list.")
            Exit Sub
        End If

        mPCSC.LoadListToControl(cmbReader, sReaderList)

        cmbReader.SelectedIndex = 0

        CurrentReaderName = cmbReader.Text.Trim

        Dim iCtr = 0

        For Each Str As String In cmbReader.Items
            ReaderList(iCtr) = Str
            iCtr += 1
        Next
        cmbReader.Dispose()
        Log("Info: Readers Initialized")
    End Sub

    Private Sub ReleaseReader()
        ModWinsCard.SCardReleaseContext(mPCSC.hContext)
        Log("Info: Readers Released")
    End Sub

    Public Function DetectCard() As Boolean
        DisconnectApplet()
        Console.WriteLine("Starting to detect card from " + CurrentReaderName)
        mPCSC.ConnectCard(CurrentReaderName, Message)
        mPCSC.GetUID(Message)

        If Not SmartCardUID.Trim = "" Then
            Console.WriteLine("Card Detected...")
            DetectCard = True
            Log("Info: Card Detected")
        Else
            Console.WriteLine("No Card Was Detected...")
            DetectCard = False
        End If

        Console.Write(DetectCard.ToString)

        mPCSC.DisconnectCard(CurrentReaderName)

    End Function

#End Region

#Region "APDUs"

    Private Con As New AllCardTech_Util

    Private member_SelectedApplet As Boolean = False
    Private member_AppletVersion As Integer = 0
    Private member_Exception As String = ""

    Public ReadOnly Property Exception() As String
        Get
            Return member_Exception
        End Get
    End Property

    Public ReadOnly Property IsAppletSelected() As Boolean
        Get
            Return member_SelectedApplet
        End Get
    End Property

#End Region

#Region "UMID_DATA"

    Public Enum SSS_FIELDS
        SSS_1 = 1
        SSS_2 = 2
        SSS_3 = 3
        SSS_4 = 4
        SSS_5 = 5
        SSS_6 = 6
        SSS_7 = 7
        SSS_8 = 8
        SSS_9 = 9
        SSS_10 = 10
        SSS_11 = 11
        SSS_12 = 12
        SSS_13 = 13
        SSS_14 = 14
        SSS_15 = 15
        SSS_16 = 16
        SSS_17 = 17
        SSS_18 = 18
        SSS_19 = 19
        SSS_20 = 20
        SSS_21 = 21
        SSS_22 = 22
        SSS_23 = 23
        SSS_24 = 24
        SSS_25 = 25
        SSS_26 = 26
        SSS_27 = 27
        SSS_28 = 28
        SSS_29 = 29
        SSS_30 = 30
        SSS_31 = 31
        SSS_32 = 32
        SSS_33 = 33
        SSS_34 = 34
        SSS_35 = 35
        SSS_36 = 36
    End Enum

    Public Enum GSIS_FIELDS
        GSIS_1
        GSIS_2
        GSIS_3
        GSIS_4
        GSIS_5
        GSIS_6
        GSIS_7
        GSIS_8
        GSIS_9
        GSIS_10
        GSIS_11
        GSIS_12
        GSIS_13
        GSIS_14
        GSIS_15
        GSIS_16
        GSIS_17
        GSIS_18
        GSIS_19
        GSIS_20
        GSIS_21
        GSIS_22
        GSIS_23
        GSIS_24
        GSIS_25
        GSIS_26
        GSIS_27
        GSIS_28
        GSIS_29
        GSIS_30
        GSIS_31
        GSIS_32
        GSIS_33
        GSIS_34
        GSIS_35
        GSIS_36
    End Enum

    Public Enum PAGIBIG_FIELDS
        PAGIBIG_1
        PAGIBIG_2
        PAGIBIG_3
        PAGIBIG_4
        PAGIBIG_5
        PAGIBIG_6
        PAGIBIG_7
        PAGIBIG_8
        PAGIBIG_9
        PAGIBIG_10
        PAGIBIG_11
        PAGIBIG_12
        PAGIBIG_13
        PAGIBIG_14
        PAGIBIG_15
        PAGIBIG_16
        PAGIBIG_17
        PAGIBIG_18
        PAGIBIG_19
        PAGIBIG_20
        PAGIBIG_21
        PAGIBIG_22
        PAGIBIG_23
        PAGIBIG_24
        PAGIBIG_25
        PAGIBIG_26
        PAGIBIG_27
        PAGIBIG_28
        PAGIBIG_29
        PAGIBIG_30
        PAGIBIG_31
        PAGIBIG_32
        PAGIBIG_33
        PAGIBIG_34
        PAGIBIG_35
        PAGIBIG_36
    End Enum

    Public Enum PHILHEALTH_FIELDS
        PHILHEALTH_1
        PHILHEALTH_2
        PHILHEALTH_3
        PHILHEALTH_4
        PHILHEALTH_5
        PHILHEALTH_6
        PHILHEALTH_7
        PHILHEALTH_8
        PHILHEALTH_9
        PHILHEALTH_10
        PHILHEALTH_11
        PHILHEALTH_12
        PHILHEALTH_13
        PHILHEALTH_14
        PHILHEALTH_15
        PHILHEALTH_16
        PHILHEALTH_17
        PHILHEALTH_18
        PHILHEALTH_19
        PHILHEALTH_20
        PHILHEALTH_21
        PHILHEALTH_22
        PHILHEALTH_23
        PHILHEALTH_24
        PHILHEALTH_25
        PHILHEALTH_26
        PHILHEALTH_27
        PHILHEALTH_28
        PHILHEALTH_29
        PHILHEALTH_30
        PHILHEALTH_31
        PHILHEALTH_32
        PHILHEALTH_33
        PHILHEALTH_34
        PHILHEALTH_35
        PHILHEALTH_36
    End Enum

    Public Enum UMID_Fields
        CRN
        FIRST_NAME
        MIDDLE_NAME
        LAST_NAME
        SUFFIX
        ADDRESS_POSTAL_CODE
        ADDRESS_COUNTRY
        ADDRESS_PROVINCIAL_OR_STATE
        ADDRESS_CITY_OR_MUNICIPALITY
        ADDRESS_BARANGAY_OR_DISTRIC_OR_LOCALITY
        ADDRESS_SUBDIVISION
        ADDRESS_STREET_NAME
        ADDRESS_HOUSE_OR_LOT_AND_BLOCK_NUMBER
        ADDRESS_ROOM_OR_FLOOR_OR_UNIT_NO_AND_BUILDING_NAME
        GENDER
        DATE_OF_BIRTH
        PLACE_OF_BIRTH_CITY
        PLACE_OF_BIRTH_PROVINCE
        PLACE_OF_BIRTH_COUNTRY
        MARITAL_STATUS
        FATHER_FIRST_NAME
        FATHER_MIDDLE_NAME
        FATHER_LAST_NAME
        FATHER_SUFFIX
        MOTHER_FIRST_NAME
        MOTHER_MIDDLE_NAME
        MOTHER_LAST_NAME
        MOTHER_SUFFIX
        HEIGHT
        WEIGHT
        DISTINGUISHING_FEATURES
        TIN
        LEFT_PRIMARY_FINGER_CODE
        LEFT_SECONDARY_FINGER_CODE
        RIGHT_PRIMARY_FINGER_CODE
        RIGHT_SECONDARY_FINGER_CODE
        BIOMETRIC_LEFT_PRIMARY_FINGER
        BIOMETRIC_LEFT_SECONDARY_FINGER
        BIOMETRIC_RIGHT_PRIMARY_FINGER
        BIOMETRIC_RIGHT_SECONDARY_FINGER
        BIOMETRIC_PICTURE
        BIOMETRIC_SIGNATURE
        PIN
        CARD_CREATION_DATE
        CARD_STATUS
        CSN

    End Enum

    Public Function DeleteInstance() As Boolean
        Return mUMID2.DeleteUMIDInstance()
    End Function

    Public Function CheckVersion() As String
        Return member_AppletVersion
    End Function

    Public Function SelectApplet(ByVal iUMID As Integer, ByVal iSAM As Integer) As Boolean
        Me.iSAM = iSAM
        Me.iUMID = iUMID
        SelectApplet = False
        member_SelectedApplet = False
        SL1 = False

        If member_AppletVersion = OldUMIDVersion Then
            umid.UMIDSAM.UMIDCard_DisConnect()
            umid.UMIDSAM.UMIDSAM_DisConnect()
            mPCSC_UMID.Reset(ReaderList(iUMID))
            mPCSC_SAM.Reset(ReaderList(iSAM))
        End If

        member_AppletVersion = NewUMIDVersion

        umid.UMIDSAM.UMIDCard_DisConnect()
        umid.UMIDSAM.UMIDSAM_DisConnect()

        mPCSC_UMID.DisconnectCard(ReaderList(iUMID))
        mPCSC_SAM.DisconnectCard(ReaderList(iSAM))

        'mPCSC_UMID = Nothing
        'mPCSC_SAM = Nothing

        'mPCSC_UMID = New AllCardTech_PCSC
        'mPCSC_SAM = New AllCardTech_PCSC

        'InitializeReaders()

        Dim ErrMessage(1023) As Byte
        Dim Err As String = ""

        mPCSC_UMID.SetCardReader(iUMID)
        mPCSC_SAM.SetCardReader(iSAM)

        'If Not (mPCSC_UMID.ConnectCard(iUMID, Message) And mPCSC_SAM.ConnectCard(iSAM, Message)) Then
        '    mPCSC_UMID.Reset(ReaderList(iUMID))
        '    mPCSC_SAM.Reset(ReaderList(iSAM))
        '    Log("Error: Smart Card Reader Connection Failed NEW")
        '    Return False
        'End If

        If mUMID2.SelectUMIDApplet() Then
            Log("Info: New UMID Applet Selected")
            member_AppletVersion = NewUMIDVersion
            SelectApplet = True
            member_SelectedApplet = True
            Return True
        End If

        mPCSC_UMID.DisconnectCard(ReaderList(iUMID))
        mPCSC_SAM.DisconnectCard(ReaderList(iSAM))

        If Not umid.UMIDSAM.SmartReader_Connect_Debug(iUMID, iSAM, ErrMessage) Then
            Log("Error: Smart Card Reader Connection Failed OLD")
            Return False
        End If

        If Not umid.UMIDSAM.UMIDSAM_Connect(ErrMessage) Then
            Log("Error: UMID SAM Module Connection Failed" + vbNewLine + Err)
        End If

        If umid.UMIDSAM.UMIDCard_Connect(ErrMessage) Then
            Log("Info: Old UMID Applet Selected...")
            member_AppletVersion = OldUMIDVersion
            member_SelectedApplet = True
            SelectApplet = True
        End If
        Log("Error: " + util.ByteArrayToAscii(ErrMessage))


    End Function

    Public Sub DisconnectApplet()
        On Error Resume Next
        If IsAppletSelected Then
            member_SelectedApplet = False
            UMIDSAM.UMIDCard_DisConnect()
            UMIDSAM.UMIDSAM_DisConnect()
            Log("Info: Applet Disconnected")
        End If
    End Sub

    Public Function UMIDCard_Change_PIN(ByVal oldPin As String, ByVal newPin As String) As Boolean
        If Not IsAppletSelected Then
            Log("Error: " & member_Exception)
            Return False
        End If

        Dim oldData As Byte() = util.AsciiToByteArray(oldPin)
        Dim newData As Byte() = util.AsciiToByteArray(newPin)

        Dim err(1023) As Byte

        If member_AppletVersion = OldUMIDVersion Then
            UMIDCard_Change_PIN = UMIDSAM.UMIDCard_Change_PIN(newData, newData.Length, err)
            Log("Error: " & util.ByteArrayToAscii(err))
        Else
            UMIDCard_Change_PIN = mUMID2.UMIDCARD_ChangePin(oldData, newData, err)
            Log("Error: " & util.ByteArrayToAscii(err))
        End If
    End Function


    Public Function UMIDCard_Activate(ByVal UserPin As Byte(), ByVal UserPinLength As Integer) As Boolean ' pending
        If Not IsAppletSelected Then
            Log("Error: " & member_Exception)
            Return False
        End If

        Dim Result As Boolean
        Dim ErrMessage As Byte() = New Byte(1023) {}
        Dim Err As String = ""

        If member_AppletVersion = OldUMIDVersion Then
            Result = UMIDSAM.UMIDCard_Active(UserPin, UserPinLength, ErrMessage)
        Else
            Result = mUMID2.UMIDCARD_Activate(UserPin, UserPinLength, ErrMessage)
        End If

        If Not Result Then
            Err = util.ByteArrayToAscii(ErrMessage)
            Log("Error: Activation Failed " & Err)
            Return False
        End If
        Log("Info: UMID Card Activated")
        Return True
    End Function

    Private Function ReadUMID(ByRef Data As Byte(), ByRef Err As Byte(), ByVal UMID As UMID_Fields) As Boolean
        ReadUMID = False

        If Not IsAppletSelected Then
            Log("Error: Please Select Applet First")
            Return False
        End If

        Log("Info: Reading UMID" + UMID.ToString)

        If member_AppletVersion = OldUMIDVersion Then
            Select Case UMID
                Case UMID_Fields.CARD_CREATION_DATE
                    ReadUMID = UMIDSAM.UMIDCard_Read_CardCreationDate(Data, Err)
                Case UMID_Fields.CSN
                    Log("Error: Not supported for Old UMID Card")
                    Return False
                Case UMID_Fields.CRN
                    ReadUMID = UMIDSAM.UMIDCard_Read_CRN(Data, Err)
                Case UMID_Fields.FIRST_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_FirstName(Data, Err)
                Case UMID_Fields.MIDDLE_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_MiddleName(Data, Err)
                Case UMID_Fields.LAST_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_LastName(Data, Err)
                Case UMID_Fields.SUFFIX
                    ReadUMID = UMIDSAM.UMIDCard_Read_SuffixName(Data, Err)
                Case UMID_Fields.ADDRESS_POSTAL_CODE
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_PostalCode(Data, Err)
                Case UMID_Fields.ADDRESS_COUNTRY
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_Country(Data, Err)
                Case UMID_Fields.ADDRESS_PROVINCIAL_OR_STATE
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_Province(Data, Err)
                Case UMID_Fields.ADDRESS_CITY_OR_MUNICIPALITY
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_City(Data, Err)
                Case UMID_Fields.ADDRESS_BARANGAY_OR_DISTRIC_OR_LOCALITY
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_Barangay(Data, Err)
                Case UMID_Fields.ADDRESS_SUBDIVISION
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_Subdivision(Data, Err)
                Case UMID_Fields.ADDRESS_STREET_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_Street(Data, Err)
                Case UMID_Fields.ADDRESS_HOUSE_OR_LOT_AND_BLOCK_NUMBER
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_House(Data, Err)
                Case UMID_Fields.ADDRESS_ROOM_OR_FLOOR_OR_UNIT_NO_AND_BUILDING_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Address_Rm(Data, Err)
                Case UMID_Fields.GENDER
                    ReadUMID = UMIDSAM.UMIDCard_Read_Gender(Data, Err)
                Case UMID_Fields.DATE_OF_BIRTH
                    ReadUMID = UMIDSAM.UMIDCard_Read_DateOfBirth(Data, Err)
                Case UMID_Fields.PLACE_OF_BIRTH_CITY
                    ReadUMID = UMIDSAM.UMIDCard_Read_Birth_City(Data, Err)
                Case UMID_Fields.PLACE_OF_BIRTH_PROVINCE
                    ReadUMID = UMIDSAM.UMIDCard_Read_Birth_Province(Data, Err)
                Case UMID_Fields.PLACE_OF_BIRTH_COUNTRY
                    ReadUMID = UMIDSAM.UMIDCard_Read_Birth_Country(Data, Err)
                Case UMID_Fields.MARITAL_STATUS
                    ReadUMID = UMIDSAM.UMIDCard_Read_Marital(Data, Err)
                Case UMID_Fields.FATHER_FIRST_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Father_FirstName(Data, Err)
                Case UMID_Fields.FATHER_MIDDLE_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Father_MiddleName(Data, Err)
                Case UMID_Fields.FATHER_LAST_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Father_LastName(Data, Err)
                Case UMID_Fields.FATHER_SUFFIX
                    ReadUMID = UMIDSAM.UMIDCard_Read_Father_SuffixName(Data, Err)
                Case UMID_Fields.MOTHER_FIRST_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Mother_FirstName(Data, Err)
                Case UMID_Fields.MOTHER_MIDDLE_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Mother_MiddleName(Data, Err)
                Case UMID_Fields.MOTHER_LAST_NAME
                    ReadUMID = UMIDSAM.UMIDCard_Read_Mother_LastName(Data, Err)
                Case UMID_Fields.MOTHER_SUFFIX
                    ReadUMID = UMIDSAM.UMIDCard_Read_Mother_SuffixName(Data, Err)
                Case UMID_Fields.HEIGHT
                    ReadUMID = UMIDSAM.UMIDCard_Read_Height(Data, Err)
                Case UMID_Fields.WEIGHT
                    ReadUMID = UMIDSAM.UMIDCard_Read_Weight(Data, Err)
                Case UMID_Fields.DISTINGUISHING_FEATURES
                    ReadUMID = UMIDSAM.UMIDCard_Read_Distinguishing(Data, Err)
                Case UMID_Fields.TIN
                    ReadUMID = UMIDSAM.UMIDCard_Read_TIN(Data, Err)
                Case UMID_Fields.LEFT_PRIMARY_FINGER_CODE
                    ReadUMID = UMIDSAM.UMIDCard_Read_LeftPrimaryFingerCode(Data, Err)
                Case UMID_Fields.RIGHT_PRIMARY_FINGER_CODE
                    ReadUMID = UMIDSAM.UMIDCard_Read_RightPrimaryFingerCode(Data, Err)
                Case UMID_Fields.LEFT_SECONDARY_FINGER_CODE
                    ReadUMID = UMIDSAM.UMIDCard_Read_LeftBackupFingerCode(Data, Err)
                Case UMID_Fields.RIGHT_SECONDARY_FINGER_CODE
                    ReadUMID = UMIDSAM.UMIDCard_Read_RightBackupFingerCode(Data, Err)
                Case Else
                    Return False
            End Select
        End If

        If member_AppletVersion = NewUMIDVersion Then
            Select Case UMID
                Case UMID_Fields.CARD_CREATION_DATE
                    ReadUMID = mUMID2.ReadCardCreationDate(Data, Err)
                Case UMID_Fields.CSN
                    ReadUMID = mUMID2.ReadCSN(Data, Err)
                Case UMID_Fields.CRN
                    ReadUMID = mUMID2.Read_CRN(Data, Err)
                Case UMID_Fields.FIRST_NAME
                    ReadUMID = mUMID2.ReadFirstName(Data, Err)
                Case UMID_Fields.MIDDLE_NAME
                    ReadUMID = mUMID2.ReadMiddleName(Data, Err)
                Case UMID_Fields.LAST_NAME
                    ReadUMID = mUMID2.ReadLastName(Data, Err)
                Case UMID_Fields.SUFFIX
                    ReadUMID = mUMID2.ReadSuffix(Data, Err)
                Case UMID_Fields.ADDRESS_POSTAL_CODE
                    ReadUMID = mUMID2.ReadPostalCode(Data, Err)
                Case UMID_Fields.ADDRESS_COUNTRY
                    ReadUMID = mUMID2.ReadCountry(Data, Err)
                Case UMID_Fields.ADDRESS_PROVINCIAL_OR_STATE
                    ReadUMID = mUMID2.ReadProvinceState(Data, Err)
                Case UMID_Fields.ADDRESS_CITY_OR_MUNICIPALITY
                    ReadUMID = mUMID2.ReadCityMunicipality(Data, Err)
                Case UMID_Fields.ADDRESS_BARANGAY_OR_DISTRIC_OR_LOCALITY
                    ReadUMID = mUMID2.ReadBarangayDistrictLocality(Data, Err)
                Case UMID_Fields.ADDRESS_SUBDIVISION
                    ReadUMID = mUMID2.ReadBarangayDistrictLocality(Data, Err)
                Case UMID_Fields.ADDRESS_STREET_NAME
                    ReadUMID = mUMID2.ReadStreetName(Data, Err)
                Case UMID_Fields.ADDRESS_HOUSE_OR_LOT_AND_BLOCK_NUMBER
                    ReadUMID = mUMID2.ReadHouseLotBlock(Data, Err)
                Case UMID_Fields.ADDRESS_ROOM_OR_FLOOR_OR_UNIT_NO_AND_BUILDING_NAME
                    ReadUMID = mUMID2.ReadRmFlrUnitBldg(Data, Err)
                Case UMID_Fields.GENDER
                    ReadUMID = mUMID2.ReadGender(Data, Err)
                Case UMID_Fields.DATE_OF_BIRTH
                    ReadUMID = mUMID2.ReadDateofBirth(Data, Err)
                Case UMID_Fields.PLACE_OF_BIRTH_CITY
                    ReadUMID = mUMID2.ReadPlaceofBirthCityMunicipality(Data, Err)
                Case UMID_Fields.PLACE_OF_BIRTH_PROVINCE
                    ReadUMID = mUMID2.ReadPlaceofBirthProvinceState(Data, Err)
                Case UMID_Fields.PLACE_OF_BIRTH_COUNTRY
                    ReadUMID = mUMID2.ReadPlaceofBirthCountry(Data, Err)
                Case UMID_Fields.MARITAL_STATUS
                    ReadUMID = mUMID2.ReadMaritalStatus(Data, Err)
                Case UMID_Fields.FATHER_FIRST_NAME
                    ReadUMID = mUMID2.ReadFatherFirstName(Data, Err)
                Case UMID_Fields.FATHER_MIDDLE_NAME
                    ReadUMID = mUMID2.ReadFatherMiddleName(Data, Err)
                Case UMID_Fields.FATHER_LAST_NAME
                    ReadUMID = mUMID2.ReadFatherLastName(Data, Err)
                Case UMID_Fields.FATHER_SUFFIX
                    ReadUMID = mUMID2.ReadFatherSuffix(Data, Err)
                Case UMID_Fields.MOTHER_FIRST_NAME
                    ReadUMID = mUMID2.ReadMotherFirstName(Data, Err)
                Case UMID_Fields.MOTHER_MIDDLE_NAME
                    ReadUMID = mUMID2.ReadMotherMiddleName(Data, Err)
                Case UMID_Fields.MOTHER_LAST_NAME
                    ReadUMID = mUMID2.ReadMotherLastName(Data, Err)
                Case UMID_Fields.MOTHER_SUFFIX
                    ReadUMID = mUMID2.ReadMotherSuffix(Data, Err)
                Case UMID_Fields.HEIGHT
                    ReadUMID = mUMID2.ReadHeight(Data, Err)
                Case UMID_Fields.WEIGHT
                    ReadUMID = mUMID2.ReadWeight(Data, Err)
                Case UMID_Fields.DISTINGUISHING_FEATURES
                    ReadUMID = mUMID2.ReadDistinguishingFeatures(Data, Err)
                Case UMID_Fields.TIN
                    ReadUMID = mUMID2.ReadTIN(Data, Err)
                Case UMID_Fields.LEFT_PRIMARY_FINGER_CODE
                    ReadUMID = mUMID2.ReadLeftPrimary(Data, Err)
                Case UMID_Fields.RIGHT_PRIMARY_FINGER_CODE
                    ReadUMID = mUMID2.ReadRightPrimary(Data, Err)
                Case UMID_Fields.LEFT_SECONDARY_FINGER_CODE
                    ReadUMID = mUMID2.ReadLeftbackup(Data, Err)
                Case UMID_Fields.RIGHT_SECONDARY_FINGER_CODE
                    ReadUMID = mUMID2.ReadRightbackup(Data, Err)
                Case Else
                    Return False
            End Select
        End If
    End Function

    Private Sub ClearBuffer(ByRef Buffer() As Byte)
        For i As Integer = 0 To Buffer.Length - 1
            Buffer(i) = 0
        Next
    End Sub

    Public Function AuthenticateWrite(ByVal KeyID As Integer) As Boolean
        Dim Err(1023) As Byte
        Return UMIDSAM.UMIDCard_SectorKeyAuth(1, KeyID, Err)
    End Function

    Public Function AuthenticateRead(ByVal KeyID As Integer) As Boolean
        Dim Err(1023) As Byte
        Return UMIDSAM.UMIDCard_SectorKeyAuth(0, KeyID, Err)
    End Function

    Public Function ReadSector(ByVal SectorID As Integer, ByVal iOffSet As Integer, ByVal iLength As Integer) As Byte()
        Dim Err(1023) As Byte
        Dim Data(1023) As Byte
        ClearBuffer(Data)
        If member_AppletVersion = OldUMIDVersion Then
            If UMIDSAM.UMIDCard_SectorKeyAuth(0, SectorID, Err) Then
                If UMIDSAM.UMIDCard_ReadSectorData(SectorID, iOffSet, iLength, Data, Err) Then
                    Return Data
                End If
            End If
            ClearBuffer(Data)
            Log("Error: Read Fail " + util.ByteArrayToAscii(Err))
        Else
            Dim ctr As Integer = SectorID
            Dim keyNo As String = "00"
            Dim agencyNo As Integer = 0
            If ctr > 0 And ctr <= 36 Then
                agencyNo = 1
                keyNo = "03"
            ElseIf ctr > 36 And ctr <= 72 Then
                agencyNo = 2
                ctr = ctr - 36
                keyNo = "04"
            ElseIf ctr > 72 And ctr <= 108 Then
                agencyNo = 3
                ctr = ctr - 72
                keyNo = "05"
            ElseIf ctr > 108 And ctr <= 144 Then
                agencyNo = 4
                ctr = ctr - 108
                keyNo = "06"
            End If

            If Not mUMID2.ReadSector(keyNo, ctr, Data, iOffSet, iLength, agencyNo, Err) Then
                ClearBuffer(Data)
                Log("Error: " + util.ByteArrayToAscii(Err))
            End If
            member_SelectedApplet = False
        End If
        Return Data
    End Function

    Public Function WriteSector(ByVal SectorID As Integer, ByVal iOffset As Integer, ByVal iLength As Integer, ByVal pData As Byte()) As Boolean
        Console.WriteLine(util.ByteArrayToHexString(pData))
        Dim Err(1023) As Byte
        If member_AppletVersion = OldUMIDVersion Then
            If UMIDSAM.UMIDCard_SectorKeyAuth(1, SectorID, Err) Then
                If UMIDSAM.UMIDCard_WriteSectorData(SectorID, iOffset, iLength, pData, Err) Then
                    Return True
                End If
            End If
            Log("Error: " + util.ByteArrayToAscii(Err))
            Return False
        Else
            Dim ctr As Integer = SectorID
            Dim keyNo As String = "00"
            Dim agencyNo As Integer = 0
            If ctr > 0 And ctr <= 36 Then
                agencyNo = 1
                keyNo = "03"
            ElseIf ctr > 36 And ctr <= 72 Then
                agencyNo = 2
                ctr = ctr - 36
                keyNo = "04"
            ElseIf ctr > 72 And ctr <= 108 Then
                agencyNo = 3
                ctr = ctr - 72
                keyNo = "05"
            ElseIf ctr > 108 And ctr <= 144 Then
                agencyNo = 4
                ctr = ctr - 108
                keyNo = "06"
            End If

            If Not mUMID2.WriteSector(keyNo, ctr, pData, iOffset, iLength, agencyNo, Err) Then
                Log("Error: " + util.ByteArrayToAscii(Err))
                WriteSector = False
            Else
                WriteSector = True
            End If
            member_SelectedApplet = False
        End If
    End Function

    Private Function ReadReserve1(ByVal iOffSet As Integer, ByVal iLength As Integer) As Byte()
        Dim Err(1023) As Byte
        Dim Data(1023) As Byte
        ClearBuffer(Data)

        If member_AppletVersion = OldUMIDVersion Then
            ClearBuffer(Data)
            Log("Error: Read Fail, Not supported on old UMID version")
            Return Data
        Else

            If Not mUMID2.ReadReserve1(iOffSet, iLength, Data, Err) Then
                ClearBuffer(Data)
                Log("Error: " + util.ByteArrayToAscii(Err))
            End If

            member_SelectedApplet = False

        End If
        Return Data
    End Function

    Private Function WriteReserve1(ByVal iOffSet As Integer, ByVal iLength As Integer, ByVal data As Byte()) As Boolean
        Dim Err(1023) As Byte

        If member_AppletVersion = OldUMIDVersion Then
            ClearBuffer(data)
            Log("Error: Read Fail, Not supported on old UMID version")
            Return False
        Else

            If Not mUMID2.WriteReserve1(iOffSet, iLength, data, Err) Then
                Log("Error: " + util.ByteArrayToAscii(Err))
            End If

            member_SelectedApplet = False

        End If
        Return True
    End Function

    Private Function LogLatestUMIDRewriting(ByVal initials As String, ByVal terminalId As String) As Boolean

        If initials.Length > 50 Then
            initials = initials.Substring(0, 50)
        End If

        'If String.IsNullOrEmpty(initials) Or initials.Length <> 3 Then


        'End If

        'If String.IsNullOrEmpty(terminalId) Or terminalId.Length > 20 Then
        '    terminalId = terminalId.Substring(0, 20)
        'End If

        If terminalId.Length > 20 Then
            terminalId = terminalId.Substring(0, 20)
        End If

        initials = initials.PadRight(50, " ")
        terminalId = terminalId.PadRight(20, " ")
        Dim current As Date = Date.Now
        Dim dateStr As String = current.ToString("MMddyy")
        Dim timeStr As String = current.ToString("HHmmss")

        Dim data As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(initials + dateStr + timeStr + terminalId)

        Return WriteReserve1(0, data.Length, data)
    End Function

    Public Function ReadLatestUMIDRewritingLog(ByRef initials As String, ByRef dateLog As Date, ByRef terminalId As String) As Boolean
        Dim data As Byte() = ReadReserve1(0, 82)

        If (data Is Nothing) Then
            Return False
        End If

        Try
            Dim str As String = System.Text.ASCIIEncoding.ASCII.GetString(data)
            Dim pos = 0
            initials = str.Substring(pos, 50)
            pos += 50
            Dim edate As String = str.Substring(pos, 6)
            pos += 6
            edate += str.Substring(pos, 6)
            pos += 6
            dateLog = Date.ParseExact(edate, "MMddyyHHmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            terminalId = str.Substring(pos, 20).Trim()
            Return True
        Catch ex As Exception
            Log("Error retrieving latest umid rewriting " + ex.Message)
        End Try

        Return False

    End Function


    Public Function AuthenticateRewriting() As Boolean
        Dim err As Byte() = Nothing

        If Not SL1 Then
            Log("Error: Security Level 1 Required")
            Return False
        End If

        AuthenticateRewriting = mUMID2.AuthenticateRewriting(err)

        If AuthenticateRewriting Then
            SelectApplet(Me.iUMID, Me.iSAM)
            AuthenticateSL1()
        End If

    End Function

    Public Function RewriteFingerprintLeftPrimary(user As String, terminalId As String, Data As Byte()) As Boolean
        If member_AppletVersion = OldUMIDVersion Then
            ClearBuffer(Data)
            Log("Error: Read Fail, Not supported on old UMID version")
            Return False
        End If

        'If Not AuthenticateRewriting() Then
        '    ClearBuffer(Data)
        '    Log("Error: Failed to authenticate rewriting")
        '    Return False
        'End If

        Dim err As Byte() = Nothing
        RewriteFingerprintLeftPrimary = mUMID2.RewriteFingerprint(0, Data, err)

        'If RewriteFingerprintLeftPrimary Then
        '    LogLatestUMIDRewriting(user, terminalId)
        'End If

        If Not err Is Nothing Then
            Log("Error: " + util.ByteArrayToAscii(err))
        End If
    End Function

    Public Function RewriteFingerprintRightPrimary(user As String, terminalId As String, Data As Byte()) As Boolean
        If member_AppletVersion = OldUMIDVersion Then
            ClearBuffer(Data)
            Log("Error: Read Fail, Not supported on old UMID version")
            Return False
        End If

        'If Not AuthenticateRewriting() Then
        '    ClearBuffer(Data)
        '    Log("Error: Failed to authenticate rewriting")
        '    Return False
        'End If

        Dim err As Byte() = Nothing
        RewriteFingerprintRightPrimary = mUMID2.RewriteFingerprint(1024, Data, err)

        'If RewriteFingerprintRightPrimary Then
        '    LogLatestUMIDRewriting(user, terminalId)
        'End If

        If Not err Is Nothing Then
            Log("Error: " + util.ByteArrayToAscii(err))
        End If
    End Function

    Public Function RewriteFingerprintLeftSecondary(user As String, terminalId As String, Data As Byte()) As Boolean
        If member_AppletVersion = OldUMIDVersion Then
            ClearBuffer(Data)
            Log("Error: Read Fail, Not supported on old UMID version")
            Return False
        End If

        'If Not AuthenticateRewriting() Then
        '    ClearBuffer(Data)
        '    Log("Error: Failed to authenticate rewriting")
        '    Return False
        'End If

        Dim err As Byte() = Nothing
        RewriteFingerprintLeftSecondary = mUMID2.RewriteFingerprint(2048, Data, err)

        'If RewriteFingerprintLeftSecondary Then
        '    LogLatestUMIDRewriting(user, terminalId)
        'End If

        If Not err Is Nothing Then
            Log("Error: " + util.ByteArrayToAscii(err))
        End If
    End Function

    Public Function RewriteFingerprintRightSecondary(user As String, terminalId As String, Data As Byte()) As Boolean
        If member_AppletVersion = OldUMIDVersion Then
            ClearBuffer(Data)
            Log("Error: Read Fail, Not supported on old UMID version")
            Return False
        End If

        'If Not AuthenticateRewriting() Then
        '    ClearBuffer(Data)
        '    Log("Error: Failed to authenticate rewriting")
        '    Return False
        'End If

        Dim err As Byte() = Nothing
        RewriteFingerprintRightSecondary = mUMID2.RewriteFingerprint(3072, Data, err)

        'If RewriteFingerprintRightSecondary Then
        '    LogLatestUMIDRewriting(user, terminalId)
        'End If

        If Not err Is Nothing Then
            Log("Error: " + util.ByteArrayToAscii(err))
        End If
    End Function

    Public Function WriteMartialStatus(ByVal data As String) As Boolean
        Dim ErrorMessage(1023) As Byte
        Dim b() As Byte = util.AsciiToByteArray(data)
        If member_AppletVersion = OldUMIDVersion Then
            WriteMartialStatus = UMIDSAM.UMIDCard_Update_Marital(b, ErrorMessage)
        Else
            WriteMartialStatus = mUMID2.UMIDCard_Update_Marital(b, ErrorMessage)
        End If
        If Not WriteMartialStatus Then
            Log("Error: " + util.ByteArrayToAscii(ErrorMessage))
        End If
    End Function

    Public Function WriteHeightWeight(ByVal height As Integer, ByVal weight As Integer) As Boolean
        Dim ErrorMessage(1023) As Byte
        Dim b() As Byte = util.AsciiToByteArray(CStr(height).PadLeft(3, "0") + CStr(weight).PadLeft(3, "0"))
        If member_AppletVersion = OldUMIDVersion Then
            WriteHeightWeight = UMIDSAM.UMIDCard_Update_HeightWeight(b, ErrorMessage)
        Else
            WriteHeightWeight = mUMID2.UMIDCard_Update_Height(util.AsciiToByteArray(CStr(height).PadLeft(3, "0")), ErrorMessage)
            WriteHeightWeight = mUMID2.UMIDCard_Update_Weight(util.AsciiToByteArray(CStr(weight).PadLeft(3, "0")), ErrorMessage)
        End If
        If Not WriteHeightWeight Then
            Log("Error: " + util.ByteArrayToAscii(ErrorMessage))
        End If
    End Function

    Public Function WriteDistinguishing(ByVal data As String) As Boolean
        Dim ErrorMessage(1023) As Byte
        Dim b() As Byte = util.AsciiToByteArray(data)
        If member_AppletVersion = OldUMIDVersion Then
            WriteDistinguishing = UMIDSAM.UMIDCard_Update_Distinguishing(b, ErrorMessage)
        Else
            WriteDistinguishing = mUMID2.UMIDCard_Update_Distinguishing(b, ErrorMessage)
        End If
        If Not WriteDistinguishing Then
            Log("Error: " + util.ByteArrayToAscii(ErrorMessage))
        End If
    End Function


    Public Function AuthenticateSL1() As Boolean
        AuthenticateSL1 = ValidateAppletSelected()
        If AuthenticateSL1 Then
            Dim Err(1023) As Byte
            If member_AppletVersion = OldUMIDVersion Then
                AuthenticateSL1 = UMIDSAM.UMIDCard_SL1(Err)
            Else
                AuthenticateSL1 = mUMID2.AuthenticateSL1(Err, "02")
            End If
            If Not AuthenticateSL1 Then
                Log("Error: " + util.ByteArrayToAscii(Err))
            End If
        End If

        SL1 = AuthenticateSL1
    End Function

    Public Function AuthenticateSL2(ByVal UserPin As Byte()) As Boolean
        Dim UserPinLength As Integer = 6
        AuthenticateSL2 = ValidateAppletSelected()
        If AuthenticateSL2 Then
            Dim Err(1023) As Byte
            If member_AppletVersion = OldUMIDVersion Then
                AuthenticateSL2 = UMIDSAM.UMIDCard_SL2(UserPin, UserPinLength, Err)
            Else
                AuthenticateSL2 = mUMID2.AuthenticateSL2(UserPin, UserPinLength, Err)
            End If

            If Not AuthenticateSL2 Then
                Log("Error: " + util.ByteArrayToAscii(Err))
            End If
        End If
    End Function

    Public Function AuthenticateSL3() As Boolean
        AuthenticateSL3 = ValidateAppletSelected()
        If AuthenticateSL3 Then
            Dim Err(1023) As Byte
            If member_AppletVersion = OldUMIDVersion Then
                UMIDSAM.UMIDCard_SL3(Err)
            Else
                AuthenticateSL3 = mUMID2.AuthenticateSL3(Err)
            End If
            If Not AuthenticateSL3 Then
                Log("Error: " + util.ByteArrayToAscii(Err))
            End If
        End If
    End Function

    Public Function getUmidFile(ByVal DumpPath As String, ByVal pUMID As UMID_Fields) As Boolean
        Dim encoding As New System.Text.ASCIIEncoding()
        Dim PathByte As Byte() = encoding.GetBytes(DumpPath)
        Dim ErrMessage(1023) As Byte
        If Not IsAppletSelected Then
            Log("Error: Please select applet first")
            Return False
        End If

        If member_AppletVersion = OldUMIDVersion Then
            Dim LeftPrimary As Byte = &HD
            Dim RightPrimary As Byte = &HE
            Dim LeftBackup As Byte = &HF
            Dim RightBackup As Byte = &H10

            Dim LeftThumb(1023) As Byte
            Dim leftPrim(1023) As Byte
            Dim RightThumb(1023) As Byte
            Dim RightPrim(1023) As Byte

            Log("Info: Reading UMID File: " + pUMID.ToString)

            Select Case pUMID
                Case UMID_Fields.BIOMETRIC_PICTURE
                    getUmidFile = UMIDSAM.UMIDCard_Get_Picture(PathByte, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_SIGNATURE
                    getUmidFile = UMIDSAM.UMIDCard_Get_Signature(PathByte, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_LEFT_PRIMARY_FINGER
                    getUmidFile = UMIDSAM.UMIDCard_Get_FingerPrint(LeftPrimary, PathByte, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_LEFT_SECONDARY_FINGER
                    getUmidFile = UMIDSAM.UMIDCard_Get_FingerPrint(LeftBackup, PathByte, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_RIGHT_PRIMARY_FINGER
                    getUmidFile = UMIDSAM.UMIDCard_Get_FingerPrint(RightPrimary, PathByte, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_RIGHT_SECONDARY_FINGER
                    getUmidFile = UMIDSAM.UMIDCard_Get_FingerPrint(RightBackup, PathByte, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case Else
                    Log("Error: For UMID Picture, Signature, Fingerprint fields only.")
                    Return False
            End Select
        Else
            Select Case pUMID
                Case UMID_Fields.BIOMETRIC_PICTURE
                    getUmidFile = mUMID2.GetPicture(DumpPath, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_LEFT_PRIMARY_FINGER
                    getUmidFile = mUMID2.GetLeftPrimaryFingerPrint(DumpPath, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_LEFT_SECONDARY_FINGER
                    getUmidFile = mUMID2.GetLeftSecondaryFingerPrint(DumpPath, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_RIGHT_PRIMARY_FINGER
                    getUmidFile = mUMID2.GetRightPrimaryFingerPrint(DumpPath, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_RIGHT_SECONDARY_FINGER
                    getUmidFile = mUMID2.GetRightSecondaryFingerPrint(DumpPath, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case UMID_Fields.BIOMETRIC_SIGNATURE
                    getUmidFile = mUMID2.GetSignature(DumpPath, ErrMessage)
                    member_Exception = util.ByteArrayToAscii(ErrMessage)
                Case Else
                    Log("For UMID Picture, Signature, Fingerprint fields only.")
                    Return False
            End Select
        End If
        If getUmidFile = False Then
            Log(member_Exception)
        End If
    End Function

    Public ReadOnly Property getUmidData(ByVal pUMID As UMID_Fields) As Byte()
        Get
            Dim Data(1023) As Byte
            Dim Err(1023) As Byte
            If Not ReadUMID(Data, Err, pUMID) Then
                ClearBuffer(Data)
                member_Exception = util.ByteArrayToAscii(Err)
                Log(member_Exception)
            End If
            Return Data
        End Get
    End Property

    Public Function GetCardStatus(ByRef statusCode As String) As Boolean
        '48 = inactive, 49 = active, 57 = blocked
        If member_AppletVersion = OldUMIDVersion Then
            UMIDSAM.UMIDCard_DisConnect()
            Dim Err As String
            mPCSC.InitializeReaders(New ComboBox(), Err)
            If mPCSC.ConnectCard(iUMID, Err) Then
                mPCSC.SendAPDU("00A4040007A082273911020100".ToUpper())
                If Not mPCSC.SmartCardErrorCode() = "Success" Then
                    Return False
                End If
                Dim Card_Status(0) As Byte
                mPCSC.SendAPDU("80B0000011")
                Array.Copy(mPCSC.ReceiveBuffer, 16, Card_Status, 0, 1)
                Select Case Card_Status(0)
                    Case 48
                        statusCode = "CARD_INACTIVE"
                    Case 49
                        statusCode = "CARD_ACTIVE"
                    Case 57
                        statusCode = "CARD_BLOCKED"
                    Case Else
                        statusCode = "ERROR"
                End Select
                mPCSC.Reset()
                Dim errM(1023) As Byte
                UMIDSAM.UMIDCard_Connect(errM)
                Return True
            End If

            Dim errmsg(1023) As Byte
            UMIDSAM.UMIDCard_Connect(errmsg)
            Return False
        Else
            Dim err(1023) As Byte
            GetCardStatus = mUMID2.GetStatus(statusCode, err)
            If Not GetCardStatus Then
                Log("Error: " + util.ByteArrayToAscii(err))
            End If
            Dim errmsg(1023) As Byte
            UMIDSAM.UMIDCard_Connect(errmsg)
        End If
    End Function

    Public Function ApplicationBlock() As Boolean
        Dim err(1023) As Byte
        If member_AppletVersion = OldUMIDVersion Then
            ApplicationBlock = UMIDSAM.UMIDCard_ApplicationBlock(err)
        Else
            ApplicationBlock = mUMID2.BlockCard(err)
        End If
        If Not ApplicationBlock Then
            Log("Error: " + util.ByteArrayToAscii(err))
        End If
    End Function

    Public ReadOnly Property getSSSData(ByVal pSSS As SSS_FIELDS, ByVal iOffset As Integer, ByVal iLength As Integer) As Byte()
        Get
            Dim sectorID As Integer = pSSS
            Return ReadSector(sectorID, iOffset, iLength)
        End Get
    End Property

    Public Function setSSSData(ByVal pSSS As SSS_FIELDS, ByVal pData As Byte(), ByVal iOffSet As Integer, ByVal iLength As Integer) As Boolean
        Dim sectorID As Integer = pSSS
        Return WriteSector(sectorID, iOffSet, iLength, pData)
    End Function

    Public ReadOnly Property getGSISData(ByVal pGSIS As GSIS_FIELDS, ByVal iOffset As Integer, ByVal iLength As Integer) As Byte()
        Get
            Dim sectorID As Integer = pGSIS + 36
            Return ReadSector(sectorID, iOffset, iLength)
        End Get
    End Property

    Public Function setGSISData(ByVal pGSIS As GSIS_FIELDS, ByVal pData As Byte(), ByVal iOffSet As Integer, ByVal iLength As Integer) As Boolean
        Dim sectorID As Integer = pGSIS + 36
        Return WriteSector(sectorID, iOffSet, iLength, pData)
    End Function

    Public ReadOnly Property getPAGIBIGData(ByVal pPAGIBIG As PAGIBIG_FIELDS, ByVal iOffset As Integer, ByVal iLength As Integer) As Byte()
        Get
            Dim sectorID As Integer = pPAGIBIG + 36 + 36
            Return ReadSector(sectorID, iOffset, iLength)
        End Get
    End Property

    Public Function setPAGIBIGData(ByVal pPAGIBIG As PAGIBIG_FIELDS, ByVal pData As Byte(), ByVal iOffSet As Integer, ByVal iLength As Integer) As Boolean
        Dim sectorID As Integer = pPAGIBIG + 36 + 36
        Return WriteSector(sectorID, iOffSet, iLength, pData)
    End Function

    Public ReadOnly Property getPHILHEALTHData(ByVal pPHILHEALTH As PHILHEALTH_FIELDS, ByVal iOffset As Integer, ByVal iLength As Integer) As Byte()
        Get
            Dim sectorID As Integer = pPHILHEALTH + 36 + 36 + 36
            Return ReadSector(sectorID, iOffset, iLength)
        End Get
    End Property

    Public Function setPHILHEALTHData(ByVal pPHILHEALTH As PHILHEALTH_FIELDS, ByVal pData As Byte(), ByVal iOffSet As Integer, ByVal iLength As Integer) As Boolean
        Dim sectorID As Integer = pPHILHEALTH + 36 + 36 + 36
        Return WriteSector(sectorID, iOffSet, iLength, pData)
    End Function

#End Region

    Private Function ValidateAppletSelected() As Boolean
        If Not IsAppletSelected Then
            Log("Error: Please select applet first")
            Return False
        End If
        Return True
    End Function

End Class
