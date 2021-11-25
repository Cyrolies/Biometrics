Attribute VB_Name = "ViewWnd"
Public Const WM_PAINT = &HF
Public Const WM_ERASEBKGND = &H14
Public Const WM_DESTROY = &H2
Public Const COLOR_WINDOWTEXT = 8
Public Const WS_CHILD = &H40000000
Public Const WS_VISIBLE = &H10000000
Public Const WS_EX_DLGMODALFRAME = &H1&
Public Const BI_RGB = 0&
Public Const DIB_RGB_COLORS = 0 '  color table in RGBs

Public Type RECT
        Left As Long
        Top As Long
        Right As Long
        Bottom As Long
End Type

Public Type PAINTSTRUCT
        hdc As Long
        fErase As Long
        rcPaint As RECT
        fRestore As Long
        fIncUpdate As Long
        rgbReserved(32) As Byte
End Type

Public Type WNDCLASS
        style As Long
        lpfnwndproc As Long
        cbClsextra As Long
        cbWndExtra2 As Long
        hInstance As Long
        hIcon As Long
        hCursor As Long
        hbrBackground As Long
        lpszMenuName As String
        lpszClassName As String
End Type

Public Type RGBQUAD
        rgbBlue As Byte
        rgbGreen As Byte
        rgbRed As Byte
        rgbReserved As Byte
End Type

Public Type PALETTEENTRY
        peRed As Byte
        peGreen As Byte
        peBlue As Byte
        peFlags As Byte
End Type

Public Type LOGPALETTE
        palVersion As Integer
        palNumEntries As Integer
        palPalEntry(255) As PALETTEENTRY
End Type

Public Type BITMAPINFOHEADER '40 bytes
        biSize As Long
        biWidth As Long
        biHeight As Long
        biPlanes As Integer
        biBitCount As Integer
        biCompression As Long
        biSizeImage As Long
        biXPelsPerMeter As Long
        biYPelsPerMeter As Long
        biClrUsed As Long
        biClrImportant As Long
End Type

Public Type BITMAPINFO
        bmiHeader As BITMAPINFOHEADER
        bmiColors As RGBQUAD
End Type

Public Declare Function BeginPaint Lib "user32" (ByVal hwnd As Long, lpPaint As PAINTSTRUCT) As Long

Public Declare Function EndPaint Lib "user32" (ByVal hwnd As Long, lpPaint As PAINTSTRUCT) As Long

Public Declare Function DefWindowProc Lib "user32" Alias "DefWindowProcA" _
    (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
    
Public Declare Function LoadCursor Lib "user32" Alias "LoadCursorA" _
    (ByVal hInstance As Long, ByVal lpCursorName As String) As Long
    
Public Declare Function RegisterClass Lib "user32" Alias "RegisterClassA" (ByRef Class As WNDCLASS) As Long

Public Declare Function CreateWindowEx Lib "user32" Alias "CreateWindowExA" _
    (ByVal dwExStyle As Long, ByVal lpClassName As String, ByVal lpWindowName As String, _
     ByVal dwStyle As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, _
     ByVal nHeight As Long, ByVal hWndParent As Long, ByVal hMenu As Long, _
     ByVal hInstance As Long, lpParam As Any) As Long
     
Public Declare Function CreatePalette Lib "gdi32" (lpLogPalette As LOGPALETTE) As Long

Public Declare Function SelectPalette Lib "gdi32" (ByVal hdc As Long, _
    ByVal hPalette As Long, ByVal bForceBackground As Long) As Long

Public Declare Function RealizePalette Lib "gdi32" (ByVal hdc As Long) As Long

Public Declare Function SetDIBitsToDevice Lib "gdi32" _
    (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal dx As Long, ByVal dy As Long, _
     ByVal SrcX As Long, ByVal SrcY As Long, ByVal Scan As Long, ByVal NumScans As Long, _
     Bits As Any, BitsInfo As Any, ByVal wUsage As Long) As Long
     
Public Declare Function GetDC Lib "user32" (ByVal hwnd As Long) As Long

Public Declare Function ReleaseDC Lib "user32" (ByVal hwnd As Long, ByVal hdc As Long) As Long

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Application data
'
Public Const szShowClassName As String = "ShowWndClass"
    
Public hSWnd As Long            ' show window handle
Public w_SWnd As Long           ' width of show window
Public h_SWnd As Long           ' heigth of show window

Public lpDIBHeader() As Byte
Public lpDIBData() As Byte
Public lpPolLitra As LOGPALETTE
Public hPGrayscale As Long


'
' UpdateImage - updates data and shows the current fingerprint image.
' Argument list:
'   srcData - new fingerprint image.
'
Sub UpdateImage(ByVal srcData As Long)
    Dim iCyc As Long
    Dim hdc As Long

    ' rotate an image while copying data to the DIB data
    For iCyc = 0 To h_SWnd - 1
        Call CopyMemory(lpDIBData((h_SWnd - iCyc - 1) * w_SWnd), _
                         ByVal (srcData + iCyc * w_SWnd), w_SWnd)
    Next iCyc
    
    ' show the fingerprint image
    hdc = GetDC(hSWnd)
    DIBShow (hdc)
    Call ReleaseDC(hSWnd, hdc)
    
End Sub


'
' DIBShow - shows the captured fingerprint image.
' Argument list:
'   hdc - show window device context.
'
Sub DIBShow(ByVal hdc As Long)

    Call SelectPalette(hdc, hPGrayscale, 0)
    Call RealizePalette(hdc)
    
    Call SetDIBitsToDevice(hdc, 0, 0, w_SWnd, h_SWnd, _
        0, 0, 0, h_SWnd, lpDIBData(0), lpDIBHeader(0), DIB_RGB_COLORS)

End Sub

'
' PrepareView - allocates memory & initializes data for fingerprint viewing.
' Argument list:
'   w, h, d - width, height and color depth of fingerprint image.
' Return value:
'   True    - success;
'   False   - error.
'
Function PrepareView(w As Long, h As Long, d As Integer) As Boolean
    
    Dim dib_BMInfo As BITMAPINFO
    Dim dib_bmiColors(255) As RGBQUAD
    Dim iCyc As Long
    
    ' support the 256-colors DIB only
    If d <> 8 Then
        PrepareView = False
        Exit Function
    End If
    
    ' allocate memory for DIB image
    ReDim lpDIBHeader(Len(dib_BMInfo) + Len(dib_BMInfo.bmiColors) * 255)
    ReDim lpDIBData(w * h)
    
    ' fill the DIB header
    dib_BMInfo.bmiHeader.biSize = Len(dib_BMInfo.bmiHeader)
    dib_BMInfo.bmiHeader.biWidth = w
    dib_BMInfo.bmiHeader.biHeight = h
    dib_BMInfo.bmiHeader.biPlanes = 1
    dib_BMInfo.bmiHeader.biBitCount = d
    dib_BMInfo.bmiHeader.biCompression = BI_RGB
    
    ' fill the logical palette
    lpPolLitra.palVersion = 768
    lpPolLitra.palNumEntries = 256
    
    ' initialize logical and DIB palettes to grayscale
    For iCyc = 0 To 255
        dib_bmiColors(iCyc).rgbBlue = iCyc
        dib_bmiColors(iCyc).rgbGreen = iCyc
        dib_bmiColors(iCyc).rgbRed = iCyc
        lpPolLitra.palPalEntry(iCyc).peBlue = iCyc
        lpPolLitra.palPalEntry(iCyc).peGreen = iCyc
        lpPolLitra.palPalEntry(iCyc).peRed = iCyc
    Next iCyc
    
    ' prepare DIB header
    Call CopyMemory(lpDIBHeader(0), dib_BMInfo.bmiHeader, _
                    Len(dib_BMInfo.bmiHeader))
    Call CopyMemory(lpDIBHeader(40), dib_bmiColors(0), _
                    Len(dib_bmiColors(0)) * 256)
    
    ' create a grayscale palette
    hPGrayscale = CreatePalette(lpPolLitra)
    
    PrepareView = True
    
End Function


'
' GetFunAddr - get function address
'
Function GetFunAddr(ByVal lngFnPtr As Long) As Long
   GetFunAddr = lngFnPtr
End Function


'
' ShowWndProc - view window callback function
'
Function ShowWndProc(ByVal hwnd As Long, ByVal msg As Long, ByVal wParam As Long, _
                     ByVal lParam As Long) As Long
    Dim hdc As Long
    Dim ps As PAINTSTRUCT

    Select Case msg
        Case WM_PAINT
            hdc = BeginPaint(hwnd, ps)
            DIBShow (hdc)
            Call EndPaint(hwnd, ps)
        Case WM_ERASEBKGND
            ShowWndProc = 1
            Exit Function
        Case WM_DESTROY
            ShowWndProc = 0
            Exit Function
    End Select

    ShowWndProc = DefWindowProc(hwnd, msg, wParam, lParam)

End Function


'
' weRegShowWnd  - register window class for viewing fingerprint image
' Return value:
'   0 - error while register window class
Function weRegShowWnd() As Long
    Dim Class As WNDCLASS

    Class.style = 0
    Class.lpfnwndproc = GetFunAddr(AddressOf ShowWndProc)
    Class.cbClsextra = 0
    Class.cbWndExtra2 = 0
    Class.hInstance = VB.App.hInstance
    Class.hIcon = 0
    Class.hCursor = 0
    Class.hbrBackground = COLOR_WINDOWTEXT + 1
    Class.lpszMenuName = Empty
    Class.lpszClassName = szShowClassName

    weRegShowWnd = RegisterClass(Class)

End Function


'
' CreateShowWnd - create window for viewing fingerprint image
' Argument list:
'   hParent - parent window;
'   x, y    - upper left corner in parent;
'   w, h    - window's width and height.
' Return value:
'   0   - error creating.
'
Function CreateShowWnd(ByVal hParent As Long, ByVal x As Long, ByVal y As Long, _
    ByVal w As Long, ByVal h As Long) As Long
    
    ' store show window dimension
    w_SWnd = w
    h_SWnd = h
    
    ' register window class
    weRegShowWnd
    
    ' prepare data for view fingerprint image
    If PrepareView(w, h, 8) = False Then
        CreateShowWnd = 0
        Exit Function
    End If
    
    ' create show window
    hSWnd = CreateWindowEx(0, szShowClassName, 0&, WS_CHILD + WS_VISIBLE + WS_EX_DLGMODALFRAME, _
        x, y, w, h, hParent, 0, VB.App.hInstance, 0&)
    CreateShowWnd = hSWnd
    If hSWnd = 0 Then
        Exit Function
    End If
    UpdateWindow (hParent)
    
End Function

