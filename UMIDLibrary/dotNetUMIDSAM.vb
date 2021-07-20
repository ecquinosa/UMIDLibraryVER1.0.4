Imports System.Runtime.InteropServices

Namespace umid

    Friend Class UMIDSAM

        Public Const _dll As String = "UMIDSAM.dll"

        <DllImport(_dll)> _
        Public Shared Function SmartReader_Connect_Debug(ByVal iUMID As Integer, ByVal iSAM As Integer, ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function SmartReader_Connect(ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDSAM_Connect(ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Sub UMIDSAM_DisConnect()
        End Sub

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Connect(ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Sub UMIDCard_DisConnect()
        End Sub

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Active(ByVal UserPIN As Byte(), ByVal iUserPIN As Integer, ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_ApplicationBlock(ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_CRN(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_FirstName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_MiddleName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_LastName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_SuffixName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Gender(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_DateOfBirth(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_PostalCode(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Country(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Province(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_City(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Barangay(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Subdivision(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Street(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_House(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Rm(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Birth_City(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Birth_Province(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Birth_Country(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Marital(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Update_Marital(ByVal DataUpdate As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_FirstName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_MiddleName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_LastName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_SuffixName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_FirstName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_MiddleName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_LastName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_SuffixName(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Height(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Weight(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Update_HeightWeight(ByVal DataUpdate As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Distinguishing(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Update_Distinguishing(ByVal DataUpdate As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_TIN(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_LeftPrimaryFingerCode(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_RightPrimaryFingerCode(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_LeftBackupFingerCode(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_RightBackupFingerCode(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Change_PIN(ByVal UserPin As Byte(), ByVal iUserPin As Integer, ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_CardCreationDate(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Get_Picture(ByVal FilePath As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Get_Signature(ByVal FilePath As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Get_FingerPrint(ByVal FingerCode As Byte, ByVal FilePath As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_SL1(ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_SL2(ByVal UserPin As Byte(), ByVal iUserPin As Integer, ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Sub UMIDCard_SL3(ByVal ErrorMessage As Byte())
        End Sub

        '<DllImport(_dll)> _
        'public Shared Function UMIDCard_SL3(ByVal FingerCode As Byte, ByVal FingerData As Byte(), ByVal DataInLen As Integer, ByVal ErrorMessage As Byte()) As [Boolean]
        'End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_SectorKeyAuth(ByVal KeyType As Integer, ByVal KeyID As Integer, ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_ReadSectorData(ByVal SectorID As Integer, ByVal Offset As Integer, ByVal DateLen As Integer, ByVal SectorData As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_WriteSectorData(ByVal SectorID As Integer, ByVal Offset As Integer, ByVal DateLen As Integer, ByVal SectorData As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_CRN_QC(ByVal Data() As Byte, ByVal ErrorMessage() As Byte) As Boolean
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Autn_QC(ByVal ErrorMessage() As Byte) As Boolean
        End Function

        'Added
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_UpdateDataGroup(ByVal SectorID As Integer, ByVal Offset As Integer, ByVal DateLen As Integer, ByVal SectorData As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_FirstName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_MiddleName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_LastName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_SuffixName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function


        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Gender_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_DateOfBirth_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_PostalCode_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Country_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Province_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_City_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Barangay_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Subdivision_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Street_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_House_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Address_Rm_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Birth_City_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Birth_Province_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Birth_Country_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Marital_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_FirstName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_MiddleName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_LastName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Father_SuffixName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_FirstName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_MiddleName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function


        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_LastName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function


        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Mother_SuffixName_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function


        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Height_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Weight_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_Distinguishing_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_TIN_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_LeftPrimaryFingerCode_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_RightPrimaryFingerCode_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_LeftBackupFingerCode_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_RightBackupFingerCode_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Read_CardCreationDate_QC(ByVal DataRead As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Get_Picture_QC(ByVal FilePath As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Get_Signature_QC(ByVal FilePath As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function

        <DllImport(_dll)> _
        Public Shared Function UMIDCard_Get_FingerPrint_QC(ByVal FingerCode As Byte, ByVal FilePath As Byte(), ByVal ErrorMessage As Byte()) As [Boolean]
        End Function
    End Class

End Namespace


