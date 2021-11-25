Attribute VB_Name = "CommonData"
' Measure <--> probability array record
Public Type M2PREC
    prob As Long
    meas As String
End Type

Public Const M2PNUM As Integer = 25     ' M2PArray records number
Public Const M2PDEFITEM As Integer = 10 ' default array item

Public M2PArray(M2PNUM - 1) As M2PREC

Public Declare Function GetFileVersionInfoSize Lib "version.dll" Alias "GetFileVersionInfoSizeA" _
    (ByVal lptstrFilename As String, lpdwHandle As Long) As Long
    
Public Declare Function GetFileVersionInfo Lib "version.dll" Alias "GetFileVersionInfoA" _
    (ByVal lptstrFilename As String, ByVal dwHandle As Long, ByVal dwLen As Long, lpData As Any) As Long

Public Declare Function VerQueryValue Lib "version.dll" Alias "VerQueryValueA" _
    (pBlock As Any, ByVal lpSubBlock As String, ByVal lplpBuffer As Long, puLen As Long) As Long

Public Type VS_FIXEDFILEINFO
        dwSignature As Long
        dwStrucVersion As Long         '  e.g. 0x00000042 = "0.42"
        dwFileVersionMS As Long        '  e.g. 0x00030075 = "3.75"
        dwFileVersionLS As Long        '  e.g. 0x00000031 = "0.31"
        dwProductVersionMS As Long     '  e.g. 0x00030010 = "3.10"
        dwProductVersionLS As Long     '  e.g. 0x00000031 = "0.31"
        dwFileFlagsMask As Long        '  = 0x3F for version "0.42"
        dwFileFlags As Long            '  e.g. VFF_DEBUG Or VFF_PRERELEASE
        dwFileOS As Long               '  e.g. VOS_DOS_WINDOWS16
        dwFileType As Long             '  e.g. VFT_DRIVER
        dwFileSubtype As Long          '  e.g. VFT2_DRV_KEYBOARD
        dwFileDateMS As Long           '  e.g. 0
        dwFileDateLS As Long           '  e.g. 0
End Type

Public Type HILO
    lo As Integer
    hi As Integer
End Type


'
' InitM2PArray  - measure <--> probability array initialization
'
Sub InitM2PArray()

    ' See commentary for according FAR value
    ' 0,343728560
    M2PArray(0).prob = 738151462
    M2PArray(0).meas = "  1"
    ' 0,297124566
    M2PArray(1).prob = 638070147
    M2PArray(1).meas = " 16"
    ' 0,199300763
    M2PArray(2).prob = 427995129
    M2PArray(2).meas = " 31"
    ' 0,096384707
    M2PArray(3).prob = 206984582
    M2PArray(3).meas = " 49"
    ' 0,048613563
    M2PArray(4).prob = 104396832
    M2PArray(4).meas = " 63"
    ' 0,009711077
    M2PArray(5).prob = 20854379
    M2PArray(5).meas = " 95"
    ' 0,004945561
    M2PArray(6).prob = 10620511
    M2PArray(6).meas = "107"
    ' 0,000962156
    M2PArray(7).prob = 2066214
    M2PArray(7).meas = "130"
    ' 0,000467035
    M2PArray(8).prob = 1002950
    M2PArray(8).meas = "136"
    ' 0,000096792
    M2PArray(9).prob = 207859
    M2PArray(9).meas = "155"
    ' 0,000048396
    M2PArray(10).prob = 103930
    M2PArray(10).meas = "166"
    ' 0,000009780
    M2PArray(11).prob = 21002
    M2PArray(11).meas = "190"
    ' 0,000004514
    M2PArray(12).prob = 9694
    M2PArray(12).meas = "199"
    ' 0,000000878
    M2PArray(13).prob = 1885
    M2PArray(13).meas = "221"
    ' 0,000000376
    M2PArray(14).prob = 807
    M2PArray(14).meas = "230"
    ' 0,000000119209 (0/129)
    M2PArray(15).prob = 256
    M2PArray(15).meas = "245"
    ' 0,000000059605 (0/153)
    M2PArray(16).prob = 128
    M2PArray(16).meas = "265"
    ' 0,000000029802 (0/174)
    M2PArray(17).prob = 64
    M2PArray(17).meas = "286"
    ' 0,000000014901 (0/205)
    M2PArray(18).prob = 32
    M2PArray(18).meas = "305"
    ' 0,000000007451 (0/231)
    M2PArray(19).prob = 16
    M2PArray(19).meas = "325"
    ' 0,000000003725 (0/294)
    M2PArray(20).prob = 8
    M2PArray(20).meas = "345"
    ' 0,000000001863 (0/362)
    M2PArray(21).prob = 4
    M2PArray(21).meas = "365"
    ' 0,000000000931 (0/439)
    M2PArray(22).prob = 2
    M2PArray(22).meas = "385"
    ' 0,000000000466 (0/542)
    M2PArray(23).prob = 1
    M2PArray(23).meas = "405"
    ' Maximum possible measure value should be placed here
    M2PArray(24).prob = 1
    M2PArray(24).meas = "Max"

End Sub

'
' Convert array of bytes to string
'
Function BA2String(ba() As Byte) As String
    Dim res As String
    Dim iCyc As Integer
    
    For iCyc = 0 To 14
        If ba(iCyc) <> 0 Then
            res = res + Chr(ba(iCyc))
        End If
    Next
    BA2String = res
End Function

'
' Convert string to array of bytes
' Argument list:
'   ba  - destination array of bytes
'   st  - source string
'
Sub String2BA(ba() As Byte, st As String)
    Dim iCyc As Integer
    
    ba = StrConv(st, vbFromUnicode)
    For iCyc = LBound(ba) To UBound(ba)
        If ba(iCyc) = 32 Then
            ba(iCyc) = 0
        End If
    Next
End Sub


  '
  ' CalcMfromP - calculate mesure value for probability.
  ' Argument list:
  '   prob  - source probability value.
  ' Return value:
  '         -  mesure value from M2PArray.
  '
Function CalcMfromP(prob As Long) As String
    Dim iCyc As Integer
    
    For iCyc = 0 To M2PNUM - 1
        If M2PArray(iCyc).prob <= prob Then
            CalcMfromP = M2PArray(iCyc).meas
            Exit Function
        End If
    Next
    CalcMfromP = "Unknown"
End Function

  '
  ' GetFTRSDKVersion - get Futronic SDK version. This is FTRAPI.dll version.
  ' Argument list:
  '   verinfo (input/output)  - text buffer;
  '   buflen (input)          - text buffer size.
  '
Function GetFTRSDKVersion() As String
    Dim dwZero As Long
    Dim viLen As Long
    Dim verBuf() As Byte
    Dim bRet As Long
    Dim FI As VS_FIXEDFILEINFO
    Dim lpFI As Long
    Dim cbTranslate As Long
    Dim HVMS As Integer
    Dim LVMS As Integer
    Dim HVLS As Integer
    Dim LVLS As Integer
    
    viLen = GetFileVersionInfoSize("FtrAPI.dll", dwZero)
    If viLen = 0 Then
        GetFTRSDKVersion = "unknown version"
        Exit Function
    End If
    ReDim verBuf(viLen)
    
    bRet = GetFileVersionInfo("FtrAPI.dll", dwZero, viLen, verBuf(0))
    If bRet = 0 Then
        GetFTRSDKVersion = "unknown version"
        Exit Function
    End If
    
    bRet = VerQueryValue(verBuf(0), "\", VarPtr(lpFI), cbTranslate)
    If bRet = 0 Then
        GetFTRSDKVersion = "unknown version"
        Exit Function
    End If
    Call CopyMemory(FI, ByVal lpFI, Len(FI))
    
    HVMS = HIWORD(FI.dwFileVersionMS)
    LVMS = LOWORD(FI.dwFileVersionMS)
    HVLS = HIWORD(FI.dwFileVersionLS)
    LVLS = LOWORD(FI.dwFileVersionLS)
    
    MainWindow.LabelDLLVersion.Caption = "FTRAPI.dll version " + Str(HVMS) + "." _
                                         + Str(LVMS) + "." + Str(HVLS) + "." + Str(LVLS)
       
End Function

'
' LOWORD - get low word for long
' Argument list:
'   l   - long type data
'
Function LOWORD(l As Long) As Integer
    Dim hl As HILO
    
    Call CopyMemory(hl, l, Len(l))
    LOWORD = hl.lo
End Function

'
' HIWORD - get high word for long
' Argument list:
'   l   - long type data
'
Function HIWORD(l As Long) As Integer
    Dim hl As HILO
    
    Call CopyMemory(hl, l, Len(l))
    HIWORD = hl.hi
End Function

