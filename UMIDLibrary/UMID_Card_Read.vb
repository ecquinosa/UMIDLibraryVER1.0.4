'Imports System.Drawing
'Imports System.Windows.Forms
'Imports UMIDLibrary.umid

'Friend Class UMID_Card_Read
'    Inherits PCSCRoutines

'    Private UMID_Card As AllCardTech_Smart_Card
'    Private SAM As AllCardTech_Smart_Card

'    Private Bin As String = Environment.GetEnvironmentVariable("temp")

'    Private m_UMID As Integer
'    Private m_SAM As Integer
'    Private m_PIN As String = ""

'#Region "Members"

'    Private m_CRN As String = ""
'    Private m_FirstName As String = ""
'    Private m_MiddleName As String = ""
'    Private m_LastName As String = ""
'    Private m_Suffix As String = ""
'    Private m_Gender As String = ""
'    Private m_CivilStatus As String = ""
'    Private m_Height As String = ""
'    Private m_Weight As String = ""
'    Private m_BirthDate As String = ""
'    Private m_BirthCity As String = ""
'    Private m_BirthProvince As String = ""
'    Private m_BirthCountry As String = ""
'    Private m_Features As String = ""
'    Private m_CardCreationDate As String = ""
'    Private m_Father_FirstName As String = ""
'    Private m_Father_MiddleName As String = ""
'    Private m_Father_LastName As String = ""
'    Private m_Father_Suffix As String = ""
'    Private m_Mother_FirstName As String = ""
'    Private m_Mother_MiddleName As String = ""
'    Private m_Mother_LastName As String = ""
'    Private m_Mother_Suffix As String = ""
'    Private m_Room As String = ""
'    Private m_House As String = ""
'    Private m_StreetName As String = ""
'    Private m_Subdivision As String = ""
'    Private m_Barangay As String = ""
'    Private m_City As String = ""
'    Private m_Province As String = ""
'    Private m_PostalCode As String = ""
'    Private m_CountryCode As String = ""
'    Private m_TIN As String = ""

'    Private m_FingerCode_LeftPrimary As String = ""
'    Private m_FingerCode_LeftBackup As String = ""
'    Private m_FingerCode_RightPrimary As String = ""
'    Private m_FingerCode_RightBackup As String = ""

'    Private m_Photo_Path As String = ""
'    Private m_Signature_Path As String = ""
'    Private m_Biometric_LeftPrimary As String = ""
'    Private m_Biometric_LeftBackup As String = ""
'    Private m_Biometric_RightPrimary As String = ""
'    Private m_Biometric_RightBackup As String = ""

'    Private m_Photo As Image
'    Private m_Signature As Image

'#End Region

'#Region "Properties"

'    Public Property Photo_Path() As String
'        Get
'            Return m_Photo_Path
'        End Get
'        Set(ByVal value As String)
'            m_Photo_Path = value
'        End Set
'    End Property

'    Public Property Signature_Path() As String
'        Get
'            Return m_Signature_Path
'        End Get
'        Set(ByVal value As String)
'            m_Signature_Path = value
'        End Set
'    End Property

'    Public Property FingerCode_LeftPrimary() As String
'        Get
'            Return m_FingerCode_LeftPrimary
'        End Get
'        Set(ByVal value As String)
'            m_FingerCode_LeftPrimary = value
'        End Set
'    End Property

'    Public Property FingerCode_RightPrimary() As String
'        Get
'            Return m_FingerCode_RightPrimary
'        End Get
'        Set(ByVal value As String)
'            m_FingerCode_RightPrimary = value
'        End Set
'    End Property

'    Public Property FingerCode_LeftBackup() As String
'        Get
'            Return m_FingerCode_LeftBackup
'        End Get
'        Set(ByVal value As String)
'            m_FingerCode_LeftBackup = value
'        End Set
'    End Property

'    Public Property FingerCode_RightBackup() As String
'        Get
'            Return m_FingerCode_RightBackup
'        End Get
'        Set(ByVal value As String)
'            m_FingerCode_RightBackup = value
'        End Set
'    End Property

'    Public Property Biometric_LeftPrimary() As String
'        Get
'            Return m_Biometric_LeftPrimary
'        End Get
'        Set(ByVal value As String)
'            m_Biometric_LeftPrimary = value
'        End Set
'    End Property

'    Public Property Biometric_LeftBackup() As String
'        Get
'            Return m_Biometric_LeftBackup
'        End Get
'        Set(ByVal value As String)
'            m_Biometric_LeftBackup = value
'        End Set
'    End Property

'    Public Property Biometric_RightPrimary() As String
'        Get
'            Return m_Biometric_RightPrimary
'        End Get
'        Set(ByVal value As String)
'            m_Biometric_RightPrimary = value
'        End Set
'    End Property

'    Public Property Biometric_RightBackup() As String
'        Get
'            Return m_Biometric_RightBackup
'        End Get
'        Set(ByVal value As String)
'            m_Biometric_RightBackup = value
'        End Set
'    End Property

'    Public ReadOnly Property TIN() As String
'        Get
'            Return m_TIN
'        End Get
'    End Property

'    Public ReadOnly Property CRN() As String
'        Get
'            Return m_CRN
'        End Get
'    End Property

'    Public ReadOnly Property FirstName() As String
'        Get
'            Return m_FirstName
'        End Get
'    End Property

'    Public ReadOnly Property MiddleName() As String
'        Get
'            Return m_MiddleName
'        End Get
'    End Property

'    Public ReadOnly Property LastName() As String
'        Get
'            Return m_LastName
'        End Get
'    End Property

'    Public ReadOnly Property Suffix() As String
'        Get
'            Return m_Suffix
'        End Get
'    End Property

'    Public ReadOnly Property Father_FirstName() As String
'        Get
'            Return m_Father_FirstName
'        End Get
'    End Property

'    Public ReadOnly Property Father_MiddleName() As String
'        Get
'            Return m_Father_MiddleName
'        End Get
'    End Property

'    Public ReadOnly Property Father_LastName() As String
'        Get
'            Return m_Father_LastName
'        End Get
'    End Property

'    Public ReadOnly Property Father_Suffix() As String
'        Get
'            Return m_Father_Suffix
'        End Get
'    End Property

'    Public ReadOnly Property Mother_FirstName() As String
'        Get
'            Return m_Mother_FirstName
'        End Get
'    End Property

'    Public ReadOnly Property Mother_MiddleName() As String
'        Get
'            Return m_Mother_MiddleName
'        End Get
'    End Property

'    Public ReadOnly Property Mother_LastName() As String
'        Get
'            Return m_Mother_LastName
'        End Get
'    End Property

'    Public ReadOnly Property Mother_Suffix() As String
'        Get
'            Return m_Mother_Suffix
'        End Get
'    End Property

'    Public ReadOnly Property Gender() As String
'        Get
'            Return m_Gender
'        End Get
'    End Property

'    Public ReadOnly Property CivilStatus() As String
'        Get
'            Return m_CivilStatus
'        End Get
'    End Property

'    Public ReadOnly Property Height() As String
'        Get
'            Return m_Height
'        End Get
'    End Property

'    Public ReadOnly Property Weight() As String
'        Get
'            Return m_Weight
'        End Get
'    End Property

'    Public ReadOnly Property BirthDate() As String
'        Get
'            Return m_BirthDate
'        End Get
'    End Property

'    Public ReadOnly Property BirthCity() As String
'        Get
'            Return m_BirthCity
'        End Get
'    End Property

'    Public ReadOnly Property BirthProvince() As String
'        Get
'            Return m_BirthProvince
'        End Get
'    End Property

'    Public ReadOnly Property BirthCountry() As String
'        Get
'            Return m_BirthCountry
'        End Get
'    End Property

'    Public ReadOnly Property DistinguishingFeatures() As String
'        Get
'            Return m_Features
'        End Get
'    End Property

'    Public ReadOnly Property CardCreationDate() As String
'        Get
'            Return m_CardCreationDate
'        End Get
'    End Property

'    Public ReadOnly Property Address_Room() As String
'        Get
'            Return m_Room
'        End Get
'    End Property

'    Public ReadOnly Property Address_House() As String
'        Get
'            Return m_House
'        End Get
'    End Property

'    Public ReadOnly Property Address_StreetName() As String
'        Get
'            Return m_StreetName
'        End Get
'    End Property

'    Public ReadOnly Property Address_Subdivision() As String
'        Get
'            Return m_Subdivision
'        End Get
'    End Property

'    Public ReadOnly Property Address_Barangay() As String
'        Get
'            Return m_Barangay
'        End Get
'    End Property

'    Public ReadOnly Property Address_City() As String
'        Get
'            Return m_City
'        End Get
'    End Property

'    Public ReadOnly Property Address_Province() As String
'        Get
'            Return m_Province
'        End Get
'    End Property

'    Public ReadOnly Property Address_PostalCode() As String
'        Get
'            Return m_PostalCode
'        End Get
'    End Property

'    Public ReadOnly Property Address_CountryCode() As String
'        Get
'            Return m_CountryCode
'        End Get
'    End Property

'    Public ReadOnly Property Photo() As Image
'        Get
'            Return m_Photo
'        End Get
'    End Property

'    Public ReadOnly Property Signature() As Image
'        Get
'            Return m_Signature
'        End Get
'    End Property

'    Public ReadOnly Property UMID_Exception() As String
'        Get
'            Return UMID_Card.Exception
'        End Get
'    End Property


'#End Region

'    Public Sub New(ByVal pUMID As Integer, ByVal pSAM As Integer, ByVal pPIN As String)


'        UMID_Card = New AllCardTech_Smart_Card
'        SAM = New AllCardTech_Smart_Card

'        UMID_Card.InitializeReaders()
'        SAM.InitializeReaders()

'        Try

'            SAM.CurrentReaderName = SAM.ReaderList(pSAM)
'            'SAM.ExtraGuard()
'            SAM.Dispose()

'            UMID_Card.CurrentReaderName = UMID_Card.ReaderList(pUMID)

'            m_UMID = pUMID
'            m_SAM = pSAM
'            m_PIN = pPIN

'        Catch ex As Exception

'            SAM.CurrentReaderName = SAM.ReaderList(0)
'            'SAM.ExtraGuard()
'            SAM.Dispose()

'            UMID_Card.CurrentReaderName = UMID_Card.ReaderList(0)

'            m_UMID = 0
'            m_SAM = 0
'            m_PIN = "123456"

'        End Try


'    End Sub

'    Public Function GetReaderList() As String()
'        Dim uReaderList As New AllCardTech_Smart_Card
'        uReaderList.InitializeReaders()
'        GetReaderList = uReaderList.ReaderList
'        uReaderList.Dispose()
'    End Function

'    Private Function ReadUmidField(ByVal UMID_Field As AllCardTech_Smart_Card.UMID_Fields) As String
'        Dim tByte(0) As Byte
'        Return System.Text.ASCIIEncoding.ASCII.GetString(UMID_Card.getUmidData(UMID_Field)).Replace(System.Text.ASCIIEncoding.ASCII.GetString(tByte), "")
'    End Function

'    Public Function DetectCard() As Boolean
'        Return UMID_Card.DetectCard
'    End Function

'    Public Sub Connect()
'        UMID_Card.SelectApplet(m_UMID, m_SAM)
'    End Sub

'    Public Sub Disconnect()
'        UMID_Card.DisconnectApplet()
'    End Sub

'    Public Function ReadCRN(ByRef Status As String) As String
'        If Not UMID_Card.IsAppletSelected Then
'            Return "Error"
'        Else
'            Return ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.CRN)
'        End If
'    End Function

'    Public Function ReadCardStatus(ByRef Status As String) As String
'        Dim res As String
'        If Not UMID_Card.IsAppletSelected Then
'            Return "Error"
'        Else
'            res = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.CARD_STATUS)
'            Return res
'            'AllCardTech_UMID.AllCardTech_Smart_Card.GSIS_FIELDS.
'        End If
'    End Function

'    Public Function ReadFirst_Name(ByRef Status As String) As String
'        If Not UMID_Card.IsAppletSelected Then
'            Return "Error"
'        Else
'            Return ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FIRST_NAME)
'        End If
'    End Function

'    Public Function ReadLast_Name(ByRef Status As String) As String
'        If Not UMID_Card.IsAppletSelected Then
'            Return "Error"
'        Else
'            Return ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LAST_NAME)
'        End If
'    End Function

'    Public Function ReadMiddle_Name(ByRef Status As String) As String
'        If Not UMID_Card.IsAppletSelected Then
'            Return "Error"
'        Else
'            Return ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MIDDLE_NAME)
'        End If
'    End Function

'    Public Function ReadSuffix_Name(ByRef Status As String) As String
'        If Not UMID_Card.IsAppletSelected Then
'            Return "Error"
'        Else
'            Return ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.SUFFIX)
'        End If
'    End Function

'    Public Function AuthenticateSL1() As Boolean
'        Return UMID_Card.AuthenticateSL1
'    End Function

'    Public Function AuthenticateSL2(ByVal pPIN As String) As Boolean
'        Dim Pin() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(pPIN)
'        Return UMID_Card.AuthenticateSL2(Pin)
'    End Function

'    Public Sub AuthenticateSL3()
'        UMID_Card.AuthenticateSL3()
'    End Sub

'    '------------- without PIN and Fingerprint ---------------------basta with SAM
'    Public Sub ReadSL1(ByRef Status As String)

'        If UMID_Card.IsAppletSelected Then
'            If AuthenticateSL1() Then
'                m_CRN = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.CRN)
'                m_FirstName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FIRST_NAME)
'                m_MiddleName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MIDDLE_NAME)
'                m_LastName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LAST_NAME)
'                m_Suffix = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.SUFFIX)
'                m_Gender = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.GENDER)
'                m_BirthDate = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.DATE_OF_BIRTH)
'                m_Room = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_ROOM_OR_FLOOR_OR_UNIT_NO_AND_BUILDING_NAME)
'                m_House = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_HOUSE_OR_LOT_AND_BLOCK_NUMBER)
'                m_StreetName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_STREET_NAME)
'                m_Subdivision = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_SUBDIVISION)
'                m_Barangay = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_BARANGAY_OR_DISTRIC_OR_LOCALITY)
'                m_City = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_CITY_OR_MUNICIPALITY)
'                m_Province = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_PROVINCIAL_OR_STATE)
'                m_PostalCode = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_POSTAL_CODE)
'                m_CountryCode = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_COUNTRY)

'                GetPhoto()
'            Else
'                Status = "Unable to authenticate UMID"
'            End If
'        Else
'            Status = "Unable to read UMID"
'        End If

'    End Sub

'    '----------------------------------------- REQ PIN ------------------------------- With SAM
'    Public Sub ReadSL2(ByRef Status As String, ByVal pPin As String)
'        If UMID_Card.IsAppletSelected Then
'            If AuthenticateSL1() Then
'                If AuthenticateSL2(pPin) Then


'                    '------------ Fingerprint---------------- with SAM
'                    AuthenticateSL3()
'                    '-----------------------------------------------

'                    GetFingerprintLeftPrimary()
'                    GetFingerprintRightPrimary()
'                    GetFingerprintLeftBackup()
'                    GetFingerprintRightBackup()

'                    m_CivilStatus = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MARITAL_STATUS)
'                    m_Height = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.HEIGHT)
'                    m_Weight = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.WEIGHT)
'                    m_BirthCity = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_CITY)
'                    m_BirthProvince = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_PROVINCE)
'                    m_BirthCountry = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_COUNTRY)
'                    m_Features = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.DISTINGUISHING_FEATURES)
'                    m_CardCreationDate = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.CARD_CREATION_DATE)
'                    m_Father_FirstName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_FIRST_NAME)
'                    m_Father_MiddleName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_MIDDLE_NAME)
'                    m_Father_LastName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_LAST_NAME)
'                    m_Father_Suffix = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_SUFFIX)
'                    m_Mother_FirstName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_FIRST_NAME)
'                    m_Mother_MiddleName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_MIDDLE_NAME)
'                    m_Mother_LastName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_LAST_NAME)
'                    m_Mother_Suffix = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_SUFFIX)
'                    m_TIN = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.TIN)

'                    m_FingerCode_LeftBackup = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LEFT_SECONDARY_FINGER_CODE)
'                    m_FingerCode_RightPrimary = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.RIGHT_PRIMARY_FINGER_CODE)
'                    m_FingerCode_RightBackup = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.RIGHT_SECONDARY_FINGER_CODE)
'                    m_FingerCode_LeftPrimary = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LEFT_PRIMARY_FINGER_CODE)

'                    GetSignature()
'                End If
'            Else
'                Status = "Unable to authenticate PIN"
'            End If
'        Else
'            Status = "Unable to read UMID"
'        End If
'    End Sub

'    Public Sub ReadDetails(ByRef Status As String)

'        'Dim Pin() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(m_PIN)

'        If AuthenticateSL1() Then

'            m_CRN = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.CRN)
'            m_FirstName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FIRST_NAME)
'            m_MiddleName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MIDDLE_NAME)
'            m_LastName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LAST_NAME)
'            m_Suffix = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.SUFFIX)
'            m_Gender = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.GENDER)
'            m_BirthDate = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.DATE_OF_BIRTH)
'            'm_Room = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_ROOM_OR_FLOOR_OR_UNIT_NO_AND_BUILDING_NAME)
'            'm_House = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_HOUSE_OR_LOT_AND_BLOCK_NUMBER)
'            'm_StreetName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_STREET_NAME)
'            'm_Subdivision = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_SUBDIVISION)
'            'm_Barangay = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_BARANGAY_OR_DISTRIC_OR_LOCALITY)
'            'm_City = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_CITY_OR_MUNICIPALITY)
'            'm_Province = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_PROVINCIAL_OR_STATE)
'            'm_PostalCode = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_POSTAL_CODE)
'            'm_CountryCode = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.ADDRESS_COUNTRY)


'            ' GetPhoto()

'            If Not AuthenticateSL2(m_PIN) Then

'                Status = "Unable to authenticate PIN..."
'                Exit Sub

'            Else

'                'GetFingerprintLeftPrimary()
'                'GetFingerprintRightPrimary()
'                'GetFingerprintLeftBackup()
'                'GetFingerprintRightBackup()

'                AuthenticateSL3()

'                'm_CivilStatus = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MARITAL_STATUS)
'                'm_Height = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.HEIGHT)
'                'm_Weight = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.WEIGHT)
'                'm_BirthCity = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_CITY)
'                'm_BirthProvince = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_PROVINCE)
'                'm_BirthCountry = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.PLACE_OF_BIRTH_COUNTRY)
'                'm_Features = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.DISTINGUISHING_FEATURES)

'                m_CardCreationDate = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.CARD_CREATION_DATE)

'                'm_Father_FirstName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_FIRST_NAME)
'                'm_Father_MiddleName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_MIDDLE_NAME)
'                'm_Father_LastName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_LAST_NAME)
'                'm_Father_Suffix = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.FATHER_SUFFIX)
'                'm_Mother_FirstName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_FIRST_NAME)
'                'm_Mother_MiddleName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_MIDDLE_NAME)
'                'm_Mother_LastName = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_LAST_NAME)
'                'm_Mother_Suffix = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.MOTHER_SUFFIX)
'                'm_TIN = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.TIN)

'                'm_FingerCode_LeftBackup = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LEFT_SECONDARY_FINGER_CODE)
'                'm_FingerCode_RightPrimary = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.RIGHT_PRIMARY_FINGER_CODE)
'                'm_FingerCode_RightBackup = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.RIGHT_SECONDARY_FINGER_CODE)
'                'm_FingerCode_LeftPrimary = ReadUmidField(AllCardTech_Smart_Card.UMID_Fields.LEFT_PRIMARY_FINGER_CODE)

'                'GetSignature()

'            End If

'        Else
'            Status = "Unable to authenticate SAM..."
'            Exit Sub
'        End If

'    End Sub

'    Public Sub GetPhoto()
'        Dim Path_Photo As String = Bin + "\UMID_Photo.jpg"
'        Dim Data() As Byte
'        Dim ErrMessage(1023) As Byte
'        Dim Result As Boolean

'        If IO.File.Exists(Path_Photo) Then
'            IO.File.Delete(Path_Photo)
'        End If

'        Data = System.Text.ASCIIEncoding.ASCII.GetBytes(Path_Photo)

'        Result = UMID_Card.getUmidFile(Path_Photo, AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_PICTURE)

'        If Not Result Then
'            m_Photo = Nothing
'        Else
'            Dim photoStream As New IO.FileStream(Path_Photo, IO.FileMode.Open, IO.FileAccess.Read)
'            m_Photo = Image.FromStream(photoStream)
'            photoStream.Dispose()
'        End If

'    End Sub

'    Public Sub GetFingerprintLeftPrimary()

'        Dim Path_Fingerprint As String = Bin + "\UMID_Fingerprint_LP.ansi-fmr"
'        Dim Data() As Byte
'        Dim ErrMessage(1023) As Byte
'        Dim Result As Boolean

'        If IO.File.Exists(Path_Fingerprint) Then
'            IO.File.Delete(Path_Fingerprint)
'        End If

'        Data = System.Text.ASCIIEncoding.ASCII.GetBytes(Path_Fingerprint)

'        Result = UMID_Card.getUmidFile(Path_Fingerprint, AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_PRIMARY_FINGER)

'    End Sub

'    Public Sub GetFingerprintRightPrimary()

'        Dim Path_Fingerprint As String = Bin + "\UMID_Fingerprint_RP.ansi-fmr"
'        Dim Data() As Byte
'        Dim ErrMessage(1023) As Byte
'        Dim Result As Boolean

'        If IO.File.Exists(Path_Fingerprint) Then
'            IO.File.Delete(Path_Fingerprint)
'        End If

'        Data = System.Text.ASCIIEncoding.ASCII.GetBytes(Path_Fingerprint)

'        Result = UMID_Card.getUmidFile(Path_Fingerprint, AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_RIGHT_PRIMARY_FINGER)

'    End Sub

'    Public Sub GetFingerprintLeftBackup()

'        Dim Path_Fingerprint As String = Bin + "\UMID_Fingerprint_LB.ansi-fmr"
'        Dim Data() As Byte
'        Dim ErrMessage(1023) As Byte
'        Dim Result As Boolean

'        If IO.File.Exists(Path_Fingerprint) Then
'            IO.File.Delete(Path_Fingerprint)
'        End If

'        Data = System.Text.ASCIIEncoding.ASCII.GetBytes(Path_Fingerprint)

'        Result = UMID_Card.getUmidFile(Path_Fingerprint, AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_LEFT_SECONDARY_FINGER)

'    End Sub

'    Public Sub GetFingerprintRightBackup()

'        Dim Path_Fingerprint As String = Bin + "\UMID_Fingerprint_RB.ansi-fmr"
'        Dim Data() As Byte
'        Dim ErrMessage(1023) As Byte
'        Dim Result As Boolean

'        If IO.File.Exists(Path_Fingerprint) Then
'            IO.File.Delete(Path_Fingerprint)
'        End If

'        Data = System.Text.ASCIIEncoding.ASCII.GetBytes(Path_Fingerprint)

'        Result = UMID_Card.getUmidFile(Path_Fingerprint, AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_RIGHT_SECONDARY_FINGER)

'    End Sub

'    Public Sub GetSignature()

'        Dim Path_Photo As String = Bin + "\UMID_Signature.tiff"
'        Dim Data() As Byte
'        Dim ErrMessage(1023) As Byte
'        Dim Result As Boolean

'        If IO.File.Exists(Path_Photo) Then
'            IO.File.Delete(Path_Photo)
'        End If

'        Data = System.Text.ASCIIEncoding.ASCII.GetBytes(Path_Photo)

'        Result = UMID_Card.getUmidFile(Path_Photo, AllCardTech_Smart_Card.UMID_Fields.BIOMETRIC_SIGNATURE)

'        If Not Result Then
'            m_Signature = Nothing
'        Else
'            Dim sigStream As New IO.FileStream(Path_Photo, IO.FileMode.Open, IO.FileAccess.Read)
'            m_Signature = Image.FromStream(sigStream)
'            sigStream.Dispose()
'        End If

'    End Sub

'    Public Function ReadSector(ByVal SectorID As Integer, ByVal iOffset As Integer, ByVal iLength As Integer) As Byte()
'        If UMID_Card.AuthenticateRead(SectorID) Then
'            Return UMID_Card.ReadSector(SectorID, iOffset, iLength)
'        Else
'            Dim tByte(0) As Byte
'            Return tByte
'        End If
'    End Function

'    Public Sub Clear()
'        m_CRN = ""
'        m_FirstName = ""
'        m_MiddleName = ""
'        m_LastName = ""
'        m_Suffix = ""
'        m_Gender = ""
'        m_CivilStatus = ""
'        m_Height = ""
'        m_Weight = ""
'        m_BirthDate = ""
'        m_BirthCity = ""
'        m_BirthProvince = ""
'        m_BirthCountry = ""
'        m_Features = ""
'        m_CardCreationDate = ""
'        m_Father_FirstName = ""
'        m_Father_MiddleName = ""
'        m_Father_LastName = ""
'        m_Father_Suffix = ""
'        m_Mother_FirstName = ""
'        m_Mother_MiddleName = ""
'        m_Mother_LastName = ""
'        m_Mother_Suffix = ""
'        m_Room = ""
'        m_House = ""
'        m_StreetName = ""
'        m_Subdivision = ""
'        m_Barangay = ""
'        m_City = ""
'        m_Province = ""
'        m_PostalCode = ""
'        m_CountryCode = ""
'        m_Photo = Nothing
'        m_Signature = Nothing
'    End Sub


'    Public Function SelectUMIDCard(ByRef Status As String) As Boolean
'        Dim list As New ListBox
'        SendAPDU("00A4040007A082273911020100", list)
'        If Not SmartCardErrorCode() = "Success" Then
'            Return False
'        Else
'            Return True
'        End If

'    End Function

'    Public Sub ConnectToCard(ByVal ReaderName As String, ByRef lstBoxLog As ListBox)

'        If PCSCRoutines.connActive Then
'            retCode = ClassWinsCard.ClassWinsCard.SCardDisconnect(hCard, ClassWinsCard.ClassWinsCard.SCARD_UNPOWER_CARD)
'        End If

'        retCode = ClassWinsCard.ClassWinsCard.SCardConnect(hContext, ReaderName, ClassWinsCard.ClassWinsCard.SCARD_SHARE_SHARED, ClassWinsCard.ClassWinsCard.SCARD_PROTOCOL_T0 Or ClassWinsCard.ClassWinsCard.SCARD_PROTOCOL_T1, hCard, Protocol)

'        If retCode <> ClassWinsCard.ClassWinsCard.SCARD_S_SUCCESS Then

'            If InStr(ReaderName, "ACR128U SAM") > 0 Then
'                retCode = ClassWinsCard.ClassWinsCard.SCardConnect(hContext, ReaderName, ClassWinsCard.ClassWinsCard.SCARD_SHARE_DIRECT, 0, hCard, Protocol)
'                If retCode <> ClassWinsCard.ClassWinsCard.SCARD_S_SUCCESS Then
'                    displayOut(1, retCode, "", lstBoxLog)
'                    connActive = False
'                    Exit Sub
'                Else
'                    displayOut(0, 0, "Successful connection to " & ReaderName, lstBoxLog)
'                End If
'            Else
'                displayOut(1, retCode, "", lstBoxLog)
'                connActive = False
'                Exit Sub
'            End If
'        Else
'            displayOut(0, 0, "Successful connection to " & ReaderName, lstBoxLog)
'        End If

'        connActive = True
'    End Sub


'    Public Function GetCSN(ByRef lstBoxLog As ListBox) As Boolean
'        SendAPDU("80B0000011", lstBoxLog)
'        If SmartCardErrorCode() = "Success" Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function


'    'Function ConnectSmartCard() As Boolean

'    '    Dim ErrorMessage As Byte() = New Byte(1023) {}
'    '    Dim Result As Boolean = False

'    '    Result = UMIDSAM.SmartReader_Connect_Debug(1, 0, ErrorMessage)

'    '    If Not Result Then
'    '        Dim ERR As String = System.Text.ASCIIEncoding.ASCII.GetString(ErrorMessage)
'    '        label_status.Text = returnstr(ERR)
'    '        Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(ErrorMessage))
'    '    Else
'    '        label_status.Text = "Smart Card Initialized..."
'    '        Console.WriteLine("Smart Card Initialized...")
'    '    End If
'    '    Return Result

'    'End Function

'End Class
