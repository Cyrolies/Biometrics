Attribute VB_Name = "BiOpers"
Public Type FILETIME
        dwLowDateTime As Long
        dwHighDateTime As Long
End Type

Public Type WIN32_FIND_DATA
        dwFileAttributes As Long
        ftCreationTime As FILETIME
        ftLastAccessTime As FILETIME
        ftLastWriteTime As FILETIME
        nFileSizeHigh As Long
        nFileSizeLow As Long
        dwReserved0 As Long
        dwReserved1 As Long
        cFileName As String * MAX_PATH
        cAlternate As String * 14
End Type

Public Const INVALID_HANDLE_VALUE = -1

Declare Function VarPtrArray Lib "msvbvm60.dll" Alias "VarPtr" (Var() As Any) As Long
Declare Function VarPtr Lib "msvbvm60.dll" (Var As Any) As Long
Public Declare Function FindFirstFile Lib "kernel32" Alias "FindFirstFileA" _
    (ByVal lpFileName As String, lpFindFileData As WIN32_FIND_DATA) As Long
Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Public Declare Function FindNextFile Lib "kernel32" Alias "FindNextFileA" _
    (ByVal hFindFile As Long, lpFindFileData As WIN32_FIND_DATA) As Long



'
' cbControl  - user's callback function for control the enrollment or
'              verification execution flow.
' Argument list:
'   usrContext (input)  - user-defined context information;
'   StateMask (input)   - a bit mask indicating what arguments are provided;
'   pResponse (output)  - API function execution control is achieved through
'                         this value;
'   Signal (input)      - this signal should be used to interact with a user;
'   pBitmap (input)     - a pointer to the bitmap to be displayed.
'
Sub cbControl(ByVal usrContext As Long, ByVal StateMask As Long, ByVal pResponse As Long, _
              ByVal Signal As Long, ByRef pBitmap As FTR_BITMAP)
    Dim forResponse As Long
    Dim PrgData As FTR_PROGRESS
    
    Call CopyMemory(PrgData, ByVal pResponse, Len(PrgData))

    ' frame show
    If (StateMask And FTR_STATE_FRAME_PROVIDED) = FTR_STATE_FRAME_PROVIDED Then
        UpdateImage (pBitmap.ftrBitmap.pData)
    End If

    ' message print
    If (StateMask And FTR_STATE_SIGNAL_PROVIDED) = FTR_STATE_SIGNAL_PROVIDED Then
        Select Case Signal
            Case FTR_SIGNAL_TOUCH_SENSOR
                If usrContext = 1 And PrgData.dwCount = 1 And PrgData.bIsRepeated = 0 Then
                    MainWindow.LabelProgressText.Visible = True
                End If
                MainWindow.PrintTextMsg ("Put your finger on the scaner")
            Case FTR_SIGNAL_TAKE_OFF
                If usrContext = 1 Then
                    MainWindow.LabelProgressText.Caption = Str(PrgData.dwCount) _
                                                           + "  of " + Str(PrgData.dwTotal)
                    MainWindow.LabelProgressText.Refresh
                End If
                MainWindow.PrintTextMsg ("Take off your finger from the scaner")
            Case FTR_SIGNAL_FAKE_SOURCE
                Dim bRet
                bRet = MsgBox("Fake finger detected. Continue process?", _
                              vbQuestion + vbYesNo, "!!! Attention !!!")
                If bRet = vbYes Then
                    forResponse = FTR_CONTINUE
                Else
                    forResponse = FTR_CANCEL
                End If
                Call CopyMemory(ByVal pResponse, forResponse, Len(forResponse))
                Exit Sub
            Case FTR_SIGNAL_UNDEFINED
                MainWindow.PrintTextMsg ("Baida signal value")
        End Select
    End If
    
    forResponse = FTR_CONTINUE
    Call CopyMemory(ByVal pResponse, forResponse, Len(forResponse))

End Sub

'
' Capture - call Futronic SDK API function FTRCaptureFrame
'
Sub Capture()
    Dim rCode As Integer
    Dim ImageSize As Long
    Dim lpImageBytes() As Byte

    ' prepare memory for image store
    rCode = FTRGetParam(FTR_PARAM_IMAGE_SIZE, ImageSize)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ReDim lpImageBytes(0 To ImageSize - 1)
    
    ' capture frame
    'LastSignal = -1
    rCode = FTRCaptureFrame(0&, lpImageBytes(0))
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    MainWindow.PrintTextMsg ("Capture successfully finished")
    
End Sub

'
' Enroll - enroll operation
'
Sub Enroll()
    Dim rCode As Integer
    Dim TemplateSize As Long
    Dim lpTemplateBytes() As Byte
    Dim template As FTR_DATA
    Dim eData As FTR_ENROLL_DATA
   
    ' prepare memory for template store
    rCode = FTRGetParam(FTR_PARAM_MAX_TEMPLATE_SIZE, TemplateSize)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ReDim lpTemplateBytes(0 To TemplateSize - 1)
    
    ' prepare arguments
    template.pData = VarPtr(lpTemplateBytes(0))
    eData.dwSize = Len(eData)
    
    ' enroll operation
    ' rCode = FTREnroll(1&, FTR_PURPOSE_ENROLL, template)
    rCode = FTREnrollX(1&, FTR_PURPOSE_ENROLL, template, eData)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    MainWindow.PrintTextMsg ("Enroll successfully finished. Quality is" + Str(eData.dwQuality) + " of 10")
    
    ' write template to DB
    AddRecord template.pData, template.dwSize
    
    MainWindow.LabelProgressText.Visible = False

End Sub

'
' Verify - verify operation
' Argument list:
'   tmlFile - file name of templaite against verify operatin
'
Sub Verify(tmlFile As String)
    Dim rCode As Integer
    Dim tmlBytes() As Byte
    Dim key(15) As Byte
    Dim tml As FTR_DATA
    
    ' prepare data for read template from DB
    Dim maxTmlLen As Long           ' max template size
    rCode = FTRGetParam(FTR_PARAM_IMAGE_SIZE, maxTmlLen)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    ReDim tmlBytes(maxTmlLen)
    tml.pData = VarPtr(tmlBytes(0))
    
    ' read template
    Call GetRecord(tmlFile, tml, VarPtr(key(0)))
    
    ' verify operation
    Dim pResult As Boolean
    Dim pFARVerify As Long
    ' FAR or FARN choise
    ' rCode = FTRVerify(0&, tml, pResult, pFARVerify)
    rCode = FTRVerifyN(0&, tml, pResult, pFARVerify)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    
    ' print result
    Dim txtmsg As String
    If pResult Then
        ' FAR or FARN choise
        ' txtmsg = "Verification is successful"
        txtmsg = "Verification is successful (" + Str(pFARVerify) + " )"
    Else
        txtmsg = "Verification is wrong"
    End If
    MainWindow.PrintTextMsg (txtmsg)

End Sub

'
' Identify - identify operation
'
Sub Identify()
    Dim rCode As Integer
    Dim Sample As FTR_DATA              ' template for identify
    Dim lpSampleBytes() As Byte
    Dim maxTmlLen As Long               ' max template size
    
    ' create template for identify
    ' 1. prepare arguments
    rCode = FTRGetParam(FTR_PARAM_MAX_TEMPLATE_SIZE, maxTmlLen)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    ReDim lpSampleBytes(maxTmlLen)
    Sample.pData = VarPtr(lpSampleBytes(0))
    ' 2. enroll operation
    rCode = FTREnroll(0&, FTR_PURPOSE_IDENTIFY, Sample)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    
    ' prepare arguments for identify
    ' 1. set base template
    rCode = FTRSetBaseTemplate(Sample)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    
    ' 2. initialize array of identify records
    Dim nRec As Integer                 ' number templates in database
    nRec = GetRecordNum()
    
    Dim idTmlBytes() As Byte
    ReDim idTmlBytes(nRec * maxTmlLen)
    
    Dim idTml() As FTR_DATA
    ReDim idTml(nRec - 1)
    
    Dim idRec() As FTR_IDENTIFY_RECORD
    ReDim idRec(nRec - 1)
    
    Dim fromDB As FTR_IDENTIFY_ARRAY
    
    fromDB.TotalNumber = nRec
    fromDB.pMembers = VarPtr(idRec(0))
    
    ' 3. prepare to copy templates from DB to memory
    Dim fMask As String
    fMask = dbs.dbTemplates + "\*.tml"
    Dim hFind As Long
    Dim ff As WIN32_FIND_DATA
    hFind = FindFirstFile(fMask, ff)
    If hFind = INVALID_HANDLE_VALUE Then
        MainWindow.PrintTextMsg ("Database is empty")
        Exit Sub
    End If

    ' 4. initialize variables and copy templates from DB to memory
    Dim iCyc As Integer
    Dim fName As String
    Dim bRet As Boolean
    For iCyc = 0 To nRec - 1
        ' initialize variables
        idTml(iCyc).pData = VarPtr(idTmlBytes(maxTmlLen * iCyc))
        idRec(iCyc).pData = VarPtr(idTml(iCyc))
        ' copy templates from DB to memory
        fName = dbs.dbTemplates + "\" + ff.cFileName
        Call GetRecord(fName, idTml(iCyc), VarPtr(idRec(iCyc).KeyValue(0)))
        bRet = FindNextFile(hFind, ff)
    Next
    CloseHandle (hFind)
    
    ' 5. initialize result array
    Dim maxMat As Integer
    maxMat = nRec                           ' for example
    Dim resRec() As FTR_MATCHED_RECORD
    ReDim resRec(maxMat)
    For iCyc = 0 To maxMat - 1
        resRec(iCyc).FarAttained = 999
        resRec(iCyc).KeyValue(0) = 0
    Next
    Dim resArr As FTR_MATCHED_ARRAY
    resArr.TotalNumber = maxMat
    resArr.pMembers = VarPtr(resRec(0))
    
    ' identify cycle
    Dim resNum As Long
    Dim txtmsg As String
    resNum = 0
    ' FAR or FARN choise
    ' rCode = FTRIdentify(fromDB, resNum, resArr)
    rCode = FTRIdentifyN(fromDB, resNum, resArr)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
        Exit Sub
    End If
    ' search finish
    ' txtmsg = "Found records: " + Str(resNum)
    ' txtmsg = "You are " + BA2String(idRec(0).KeyValue)
    If resNum > 0 Then
        txtmsg = "You are " + BA2String(resRec(0).KeyValue)
        ' FAR or FARN choise
        ' txtmsg = txtmsg + " (" + CalcMfromP(resRec(0).FarAttained) + " )"
        txtmsg = txtmsg + " (" + Str(resRec(0).FarAttained) + " )"
    Else
        txtmsg = "Not found"
    End If
    
    MainWindow.PrintTextMsg (txtmsg)
    
End Sub
