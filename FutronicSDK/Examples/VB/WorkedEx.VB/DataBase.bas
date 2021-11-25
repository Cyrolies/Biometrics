Attribute VB_Name = "DataBase"
' max file name length
Public Const MAX_PATH = 260

' database initial settings
Type DBSET
    dbFolder As String          ' database folder root
    dbImages As String          ' fingerprint images folder name
    dbTemplates As String       ' templates folder name
End Type

' DB settings
Public dbs As DBSET
Public isDBExist As Boolean     ' may be need in future

Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
    (Destination As Any, Source As Any, ByVal Length As Long)

Private Const S_OK = &H0

Private Const CSIDL_PERSONAL = &H5

Private Const SHGFP_TYPE_CURRENT = 0

Private Declare Function SHGetFolderPath Lib "shfolder" _
    Alias "SHGetFolderPathA" _
    (ByVal hwndOwner As Long, ByVal nFolder As Long, _
    ByVal hToken As Long, ByVal dwFlags As Long, _
    ByVal pszPath As String) As Long

'
' dbInit  - load DB initial settings
'
Sub dbInit()
    Dim fsoObj As New FileSystemObject
    Dim sPath As String
    Dim RetVal As Long
    
    sPath = String(MAX_PATH, 0)
    
    RetVal = SHGetFolderPath(0, CSIDL_PERSONAL, 0, SHGFP_TYPE_CURRENT, sPath)
    If RetVal = S_OK Then
        sPath = Left(sPath, InStr(1, sPath, Chr(0)) - 1)
    Else
        sPath = ""
    End If
    
    sPath = sPath + "\Futronic"
    If fsoObj.FolderExists(sPath) = False Then
        fsoObj.CreateFolder (sPath)
    End If
    
    sPath = sPath + "\SDK 4.0"
    If fsoObj.FolderExists(sPath) = False Then
        fsoObj.CreateFolder (sPath)
    End If
    
    sPath = sPath + "\DataBase"
    
    ' store DB settings
    dbs.dbFolder = sPath
    dbs.dbImages = dbs.dbFolder + "\Bmp"
    dbs.dbTemplates = dbs.dbFolder
    
    ' DB not exist. Create it now
    If fsoObj.FolderExists(dbs.dbFolder) = False Then
        fsoObj.CreateFolder (dbs.dbFolder)
    End If
    
    If fsoObj.FolderExists(dbs.dbImages) = False Then
        fsoObj.CreateFolder (dbs.dbImages)
    End If
    
End Sub

'
' EmptyDB - check DB is empty.
'
Function EmptyDB() As Boolean
    Dim bRet As Boolean
    Dim fMask As String
    
    bRet = True
    fMask = dbs.dbTemplates + "\*.tml"
    
    Dim hFind As Long
    Dim ff As WIN32_FIND_DATA
    hFind = FindFirstFile(fMask, ff)
    If hFind = INVALID_HANDLE_VALUE Then
        bRet = False
    Else
        CloseHandle (hFind)
    End If
    
    EmptyDB = bRet
End Function

'
' AddRecord  - add new record to database.
' Argument list:
'   data  - pointer of data for store;
'   dLen  - size of data.
Sub AddRecord(data As Long, dLen As Long)
    Dim key As String * 16
    Dim keyB() As Byte
    Dim iCyc As Integer
     
    ' get file name for template store
    MainWindow.CommonDialog1.InitDir = dbs.dbFolder
    MainWindow.CommonDialog1.Flags = cdlOFNExplorer + cdlOFNHideReadOnly + _
                                     cdlOFNLongNames + cdlOFNNoChangeDir + _
                                     cdlOFNOverwritePrompt
    MainWindow.CommonDialog1.FileName = Empty
    MainWindow.CommonDialog1.ShowSave
    
    If MainWindow.CommonDialog1.FileName = Empty Then
        MainWindow.PrintTextMsg ("Template not stored")
    Else
        key = VBA.Left(MainWindow.CommonDialog1.FileTitle, _
                   InStrRev(MainWindow.CommonDialog1.FileTitle, ".tml") - 1)
        Call String2BA(keyB, key)
                   
        ' write "DB record" LBound
        Open MainWindow.CommonDialog1.FileName For Binary Access Write As #1
        Put #1, , dLen
        Put #1, , keyB
        Dim DataArr() As Byte
        ReDim DataArr(dLen)
        Call CopyMemory(DataArr(0), ByVal data, dLen)
        Put #1, , DataArr
        Close #1
        
        Dim txtmsg As String
        txtmsg = "Template stored as " + key
        MainWindow.PrintTextMsg (txtmsg)
    End If

End Sub

'
' SelectRecordName - select tml-file from DB
' Return value:
'   - empty value if user cancel operation
'   - file name of record
Function SelectRecordName() As String

    ' get file name for template open
    MainWindow.CommonDialog1.InitDir = dbs.dbFolder
    MainWindow.CommonDialog1.Flags = cdlOFNExplorer + cdlOFNHideReadOnly + _
                                     cdlOFNLongNames + cdlOFNNoChangeDir + _
                                     cdlOFNOverwritePrompt
    MainWindow.CommonDialog1.FileName = Empty
    MainWindow.CommonDialog1.ShowOpen
    
    SelectRecordName = MainWindow.CommonDialog1.FileName
    
End Function

'
' GetRecord - read record by name
' Argument list:
'   recName - name of tml-file
'   template (output ) - template from DB
'   key - DB record key
'
Sub GetRecord(recName As String, template As FTR_DATA, key As Long)
    Dim tmlBytes() As Byte
    Dim aKey(15) As Byte
    
    Open recName For Binary Access Read As #1
    Get #1, , template.dwSize
    Get #1, , aKey
    ReDim tmlBytes(template.dwSize)
    Get #1, , tmlBytes
    Call CopyMemory(ByVal key, aKey(0), 16)
    Call CopyMemory(ByVal template.pData, tmlBytes(0), template.dwSize)
    Close #1
End Sub

'
' GetRecordNum - get database record nubmer
' Return value:
'   0 - database is empty
'     - number of records
'
Function GetRecordNum() As Integer
    Dim fMask As String
    fMask = dbs.dbTemplates + "\*.tml"
    Dim hFind As Long
    Dim ff As WIN32_FIND_DATA
    
    GetRecordNum = 0
    hFind = FindFirstFile(fMask, ff)
    If hFind = INVALID_HANDLE_VALUE Then
        Exit Function
    End If
    
    Dim bRet As Boolean
    bRet = True
    While bRet
        GetRecordNum = GetRecordNum + 1
        bRet = FindNextFile(hFind, ff)
    Wend
    
    CloseHandle (hFind)
End Function
