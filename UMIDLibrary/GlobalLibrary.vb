'Imports System.Collections.Specialized
'Imports UMIDLibrary.PCSCRoutines
'Imports System.Windows.Forms

'Public Class GlobalLibrary

'    Private Sub InitializeReaderList()

'        Dim sReaderList As String = ""
'        Dim ReaderCount As Integer
'        Dim ctr As Integer

'        For ctr = 0 To 255
'            sReaderList = sReaderList + vbNullChar
'        Next

'        ReaderCount = 255

'        retCode = ClassWinsCard.ClassWinsCard.SCardEstablishContext(ClassWinsCard.ClassWinsCard.SCARD_SCOPE_USER, 0, 0, hContext)

'        If retCode <> ClassWinsCard.ClassWinsCard.SCARD_S_SUCCESS Then
'            'displayOut(1, retCode, "", lstBoxLog)
'            Exit Sub
'        End If

'        retCode = ClassWinsCard.ClassWinsCard.SCardListReaders(hContext, "", sReaderList, ReaderCount)

'        If retCode <> ClassWinsCard.ClassWinsCard.SCARD_S_SUCCESS Then
'            'displayOut(1, retCode, "", lstBoxLog)
'            Exit Sub

'        End If

'        'LoadListToControl(cmbReader, sReaderList)
'        ' cmbReader.SelectedIndex = 0

'    End Sub

'    Private pCRN As String
'    Public Property CRN As String
'        Get
'            Return pCRN
'        End Get
'        Set(ByVal value As String)
'            pCRN = value
'        End Set
'    End Property


'    Private pCardStatus As String
'    Public Property CardStatus As String
'        Get
'            Return pCardStatus
'        End Get
'        Set(ByVal value As String)
'            pCardStatus = value
'        End Set
'    End Property

'    Private pFirstname As String
'    Public Property Firstname As String
'        Get
'            Return pFirstname
'        End Get
'        Set(ByVal value As String)
'            pFirstname = value
'        End Set
'    End Property


'    Private pMiddlename As String
'    Public Property Middlename As String
'        Get
'            Return pMiddlename
'        End Get
'        Set(ByVal value As String)
'            pMiddlename = value
'        End Set
'    End Property


'    Private pLastname As String
'    Public Property Lastname As String
'        Get
'            Return pLastname
'        End Get
'        Set(ByVal value As String)
'            pLastname = value
'        End Set
'    End Property


'    Private pSuffix As String
'    Public Property Suffix As String
'        Get
'            Return pSuffix
'        End Get
'        Set(ByVal value As String)
'            pSuffix = value
'        End Set
'    End Property


'    Private pDateOfBirth As String
'    Public Property DateofBirth As String
'        Get
'            Return pDateOfBirth
'        End Get
'        Set(ByVal value As String)
'            pDateOfBirth = value
'        End Set
'    End Property


'    Private pGender As String
'    Public Property Gender As String
'        Get
'            Return pGender
'        End Get
'        Set(ByVal value As String)
'            pGender = value
'        End Set
'    End Property


'    Private pCreationDate As String
'    Public Property CreationDate As String
'        Get
'            Return pCreationDate
'        End Get
'        Set(ByVal value As String)
'            pCreationDate = value
'        End Set
'    End Property

'    Private reader As String

'    Public Function ConnectToUMID(ByVal readername As String) As String
'        Dim cardRead As UMID_Card_Read
'        Dim status As String = ""

'        If readername.Contains("OMNIKEY") Then
'            reader = "OMNIKEY CardMan 5x21-CL 0"
'        ElseIf readername.Contains("ACS") Then
'            reader = "ACS ACR128U PICC interface 0"
'        End If

'        If readername.Contains("OMNIKEY") Then
'            cardRead = New UMID_Card_Read(1, 0, "123456")
'        ElseIf readername.Contains("ACR") Then
'            cardRead = New UMID_Card_Read(1, 2, "123456")
'        End If

'        If Not cardRead.DetectCard Then
'            'MessageBox.Show("Unable to detect UMID Card...", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            'Console.WriteLine("Unable to detect UMID Card...")
'            Return "Unable to detect UMID Card."
'        End If

'        cardRead.Connect()
'        Try
'            cardRead.ReadDetails(status)
'            cardRead.Disconnect()

'            Console.WriteLine(status)
'            pCRN = cardRead.CRN
'            CheckActivation(pCardStatus, pCRN)
'            pFirstname = cardRead.FirstName
'            pMiddlename = cardRead.MiddleName
'            pLastname = cardRead.LastName
'            pSuffix = cardRead.Suffix
'            pDateOfBirth = cardRead.BirthDate
'            pGender = cardRead.Gender
'            pCreationDate = cardRead.CardCreationDate
'            Return "Success"
'        Catch ex As Exception
'            Return ex.Message
'        End Try


'    End Function

'    Private Function CheckActivation(ByRef Status_Code As String, ByRef crn As String) As Boolean

'        Dim cardRead As UMID_Card_Read
'        Dim status As String = ""
'        Dim list As New ListBox

'        If reader.Contains("OMNIKEY") Then
'            cardRead = New UMID_Card_Read(1, 0, "123456")
'        ElseIf reader.Contains("ACR") Then
'            cardRead = New UMID_Card_Read(1, 2, "123456")
'        End If

'        InitializeReaderList()
'        cardRead.ConnectToCard(reader, list)
'        cardRead.Connect()

'        If Not cardRead.SelectUMIDCard(status) Then
'            'ChangeStatus(Status, "Select Applet Failed...", progress, 1)
'            'DisconnectCard(SmartCardReader)
'            Status_Code = "ERROR_APPLET"
'            Return False
'        End If

'        If cardRead.GetCSN(list) Then
'            Dim Card_Status(0) As Byte
'            Dim bCRN(11) As Byte
'            Array.Copy(RecvBuff, 16, Card_Status, 0, 1)
'            Array.Copy(RecvBuff, 0, bCRN, 0, 12)

'            crn = System.Text.ASCIIEncoding.ASCII.GetString(bCRN)

'            Select Case Card_Status(0)
'                Case 48
'                    Status_Code = "CARD_INACTIVE"
'                Case 49
'                    Status_Code = "CARD_ACTIVE"
'                Case 57
'                    Status_Code = "CARD_BLOCKED"
'                Case Else
'                    Status_Code = "ERROR"
'            End Select

'        Else
'            'ChangeStatus(Status, "Get CSN Failed...", progress, 1)
'            Status_Code = "ERROR_CSN"
'            Return False
'        End If

'        ' DisconnectCard(SmartCardReader)

'        Return True

'    End Function

'End Class
