VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "ComDlg32.OCX"
Begin VB.Form MainWindow 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "VB Example for Futronic SDK v.4.2"
   ClientHeight    =   7845
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4110
   Icon            =   "MainWnd.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   523
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   274
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame Frame4 
      Caption         =   "Do image processing compatible to"
      Height          =   735
      Left            =   240
      TabIndex        =   21
      Top             =   6120
      Width           =   3615
      Begin VB.OptionButton SDKBOTHOption 
         Caption         =   "Both"
         Height          =   375
         Left            =   2280
         TabIndex        =   24
         Top             =   240
         Width           =   1095
      End
      Begin VB.OptionButton SDK35Option 
         Caption         =   "SDK 3.5"
         Height          =   375
         Left            =   1200
         TabIndex        =   23
         Top             =   240
         Width           =   975
      End
      Begin VB.OptionButton SDK30Option 
         Caption         =   "SDK 3.0"
         Height          =   375
         Left            =   120
         TabIndex        =   22
         Top             =   240
         Width           =   975
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   " Settings "
      Height          =   855
      Left            =   240
      TabIndex        =   0
      Top             =   2160
      Width           =   3612
      Begin VB.OptionButton Option2 
         Caption         =   "... screen"
         Enabled         =   0   'False
         Height          =   255
         Left            =   2160
         TabIndex        =   7
         Top             =   480
         Width           =   1092
      End
      Begin VB.OptionButton Option1 
         Caption         =   "... file"
         Enabled         =   0   'False
         Height          =   255
         Left            =   2160
         TabIndex        =   6
         Top             =   240
         Width           =   1092
      End
      Begin VB.Label Label1 
         Caption         =   "Capture to ..."
         Height          =   255
         Left            =   480
         TabIndex        =   8
         Top             =   360
         Width           =   1215
      End
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   120
      Top             =   7320
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DefaultExt      =   "tml"
      DialogTitle     =   "Enter your name"
      Filter          =   "tml - fingerprint templates"
   End
   Begin VB.CommandButton Command5 
      Caption         =   "E&xit"
      Height          =   372
      Left            =   2640
      TabIndex        =   5
      Top             =   7320
      Width           =   1212
   End
   Begin VB.Frame Frame3 
      Height          =   1935
      Left            =   240
      TabIndex        =   10
      Top             =   2880
      Width           =   3612
      Begin VB.ComboBox MeasureValueCB 
         Height          =   315
         Left            =   2640
         Style           =   2  'Dropdown List
         TabIndex        =   18
         Top             =   1440
         Width           =   735
      End
      Begin VB.CheckBox DisableMIOTCB 
         Alignment       =   1  'Right Justify
         Caption         =   "Disable MIOT:"
         Height          =   255
         Left            =   240
         TabIndex        =   16
         Top             =   1080
         Width           =   3135
      End
      Begin VB.ComboBox MaxFramesCB 
         Height          =   315
         ItemData        =   "MainWnd.frx":3F2A
         Left            =   2640
         List            =   "MainWnd.frx":3F4C
         Style           =   2  'Dropdown List
         TabIndex        =   15
         Top             =   600
         Width           =   735
      End
      Begin VB.CheckBox DetectFFCB 
         Alignment       =   1  'Right Justify
         Caption         =   "Detect fake finger:"
         Height          =   255
         Left            =   240
         TabIndex        =   13
         Top             =   240
         Width           =   3135
      End
      Begin VB.Label LabelSetMeasure 
         Caption         =   "Set measure value:"
         Height          =   255
         Left            =   240
         TabIndex        =   17
         Top             =   1500
         Width           =   2055
      End
      Begin VB.Label LabelSetMaxFrames 
         Caption         =   "Set max frames in template:"
         Height          =   255
         Left            =   240
         TabIndex        =   14
         Top             =   645
         Width           =   2055
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   " Operations "
      Height          =   1692
      Left            =   240
      TabIndex        =   9
      Top             =   120
      Width           =   3612
      Begin VB.CommandButton Command1 
         Caption         =   "&Capture"
         Default         =   -1  'True
         Height          =   372
         Left            =   480
         TabIndex        =   1
         Top             =   360
         Width           =   1092
      End
      Begin VB.CommandButton Command2 
         Caption         =   "&Enroll"
         Height          =   372
         Left            =   480
         TabIndex        =   3
         Top             =   1080
         Width           =   1092
      End
      Begin VB.CommandButton Command3 
         Caption         =   "&Verify"
         Height          =   372
         Left            =   2040
         TabIndex        =   2
         Top             =   360
         Width           =   1092
      End
      Begin VB.CommandButton Command4 
         Caption         =   "&Identify"
         Height          =   372
         Left            =   2040
         TabIndex        =   4
         Top             =   1080
         Width           =   1092
      End
   End
   Begin VB.Label LabelProgressText 
      Alignment       =   2  'Center
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   240
      TabIndex        =   20
      Top             =   5760
      Visible         =   0   'False
      Width           =   3615
   End
   Begin VB.Label LabelDLLVersion 
      Alignment       =   1  'Right Justify
      Height          =   255
      Left            =   240
      TabIndex        =   19
      Top             =   6960
      Width           =   3615
   End
   Begin VB.Label Label5 
      Alignment       =   2  'Center
      BorderStyle     =   1  'Fixed Single
      Height          =   255
      Left            =   240
      TabIndex        =   12
      Top             =   5280
      Width           =   3615
   End
   Begin VB.Label Label4 
      Alignment       =   2  'Center
      Caption         =   "Text message:"
      Height          =   255
      Left            =   600
      TabIndex        =   11
      Top             =   4920
      Width           =   2895
   End
End
Attribute VB_Name = "MainWindow"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Private Sub Command1_Click()
    ControlsDisable
    Capture
    ControlsEnable
End Sub

Private Sub Command2_Click()
    ControlsDisable
    Enroll
    ControlsEnable
End Sub

Private Sub Command3_Click()
    Dim szRNVer As String
    
    szRNVer = SelectRecordName
    If szRNVer = Empty Then
        PrintTextMsg ("Verify canceled by User")
    Else
        ControlsDisable
        Verify (szRNVer)
        ControlsEnable
    End If
End Sub

Private Sub Command4_Click()
    ControlsDisable
    Identify
    ControlsEnable
End Sub

Private Sub Command5_Click()
    Unload Me       ' unload the form
    'End             ' end the application
End Sub

Private Sub DetectFFCB_Click()
    Dim rCode As Integer
    Dim bValue As Long
    
    bValue = DetectFFCB.value
    rCode = FTRSetParam(FTR_PARAM_FAKE_DETECT, bValue)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
End Sub

Private Sub DisableMIOTCB_Click()
    Dim rCode As Integer
    Dim bValue As Long

    bValue = DisableMIOTCB.value
    rCode = FTRSetParam(FTR_PARAM_MIOT_CONTROL, bValue)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
End Sub

Private Sub Form_Load()
    Dim rCode As Integer
    
    ' initialize SDK
    rCode = FTRInitialize
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 0
        Unload Me       ' unload the form
        End             ' end the application
    End If
    
    ' Measure <--> probability array initialization
    InitM2PArray
    
    ' set dSDK settings:
    ' 1. frame source
    Dim value As Long
    value = FSD_FUTRONIC_USB
    rCode = FTRSetParam(FTR_PARAM_CB_FRAME_SOURCE, value)
    If rCode <> FTR_RETCODE_OK Then
        ControlsDisable
        Command5.Enabled = True
        wePrintError rCode, 1
    End If
    ' 2. user's calbacks
    rCode = FTRSetParam(FTR_PARAM_CB_CONTROL, AddressOf cbControl)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ' You may use FAR or FARN options on your own choice
    ' 3. FAR setting
    ' value = M2PArray(M2PDEFITEM).prob
    ' rCode = FTRSetParam(FTR_PARAM_MAX_FAR_REQUESTED, value)
    ' 3. FARN setting
    value = M2PArray(M2PDEFITEM).meas
    rCode = FTRSetParam(FTR_PARAM_MAX_FARN_REQUESTED, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ' 4. fake mode setting
    value = 0
    rCode = FTRSetParam(FTR_PARAM_FAKE_DETECT, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ' 5. application takes a control over the Fake Finger Detection (FFD) event
    value = 1
    rCode = FTRSetParam(FTR_PARAM_FFD_CONTROL, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ' 6. MIOT mode setting
    value = 0
    rCode = FTRSetParam(FTR_PARAM_MIOT_CONTROL, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    
    ' initialize example's DB
    dbInit
    
    ' set dialog controls
    If EmptyDB = False Then
        Command3.Enabled = False
        Command4.Enabled = False
    End If
    ' set MaxFramesCB control
    MaxFramesCB.ListIndex = 4
    ' set MeasureValueCB control
    Dim iCyc As Integer
    For iCyc = 0 To M2PNUM - 1
        MeasureValueCB.AddItem (M2PArray(iCyc).meas)
    Next
    MeasureValueCB.ListIndex = M2PDEFITEM
    
    ' create window for view fingerprint image
    Dim x As Long, y As Long
    Dim w As Long, h As Long
    ' 1. define x and y
    x = Width / Screen.TwipsPerPixelX
    y = 15
    ' 2. define w and h
    rCode = FTRGetParam(FTR_PARAM_IMAGE_WIDTH, w)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    rCode = FTRGetParam(FTR_PARAM_IMAGE_HEIGHT, h)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    ' 3. resize form
    Width = Width + (w + 20) * Screen.TwipsPerPixelX
    If Height < (h + 60) * Screen.TwipsPerPixelY Then
        Height = (h + 60) * Screen.TwipsPerPixelY
    End If
    ' 4. at last...
    Dim hSWnd As Long
    hSWnd = CreateShowWnd(MainWindow.hwnd, x, y, w, h)
    
    ' get SDK version information
    Dim verinfo As String
    Dim vertext As String
    verinfo = GetFTRSDKVersion()
    
    ' set version compability ctrls
    Dim VerCompatible As Long
    
    rCode = FTRGetParam(FTR_PARAM_VERSION, VerCompatible)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
    
    If VerCompatible = FTR_VERSION_PREVIOUS Then
        SDK30Option.value = True
    End If
    
    If VerCompatible = FTR_VERSION_CURRENT Then
        SDK35Option.value = True
    End If
    
    If VerCompatible = FTR_VERSION_COMPATIBLE Then
        SDKBOTHOption.value = True
    End If
   
    
End Sub

Private Sub Form_Unload(Cancel As Integer)
    FTRTerminate
End Sub

' PrintTextMsg  - print text message into Label5 dialog element in application's form
' Argument list:
'   lpTextMsg   - text message.
Public Sub PrintTextMsg(lpTextMsg As String)
    Label5.Caption = lpTextMsg
    UpdateWindow (Me.hwnd)
    MessageBeep (-1)
End Sub

Private Sub MaxFramesCB_Click()
    Dim rCode As Integer
    Dim nFrames As Long
    
    nFrames = MaxFramesCB.List(MaxFramesCB.ListIndex)
    rCode = FTRSetParam(FTR_PARAM_MAX_MODELS, nFrames)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
End Sub

Private Sub MeasureValueCB_Click()
    Dim rCode As Integer
    Dim prob As Long
    
    ' Use FAR option
    ' prob = M2PArray(MeasureValueCB.ListIndex).prob
    ' rCode = FTRSetParam(FTR_PARAM_MAX_FAR_REQUESTED, prob)
    ' Use FARN option
    If M2PArray(MeasureValueCB.ListIndex).meas = "Max" Then
        prob = 1000
    Else
        prob = M2PArray(MeasureValueCB.ListIndex).meas
    End If
    rCode = FTRSetParam(FTR_PARAM_MAX_FARN_REQUESTED, prob)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If

End Sub

Private Sub ControlsDisable()
    Command1.Enabled = False
    Command2.Enabled = False
    Command3.Enabled = False
    Command4.Enabled = False
    Command5.Enabled = False
    DetectFFCB.Enabled = False
    MaxFramesCB.Enabled = False
    DisableMIOTCB.Enabled = False
    MeasureValueCB.Enabled = False
End Sub

Private Sub ControlsEnable()
    Command1.Enabled = True
    Command2.Enabled = True
    If EmptyDB = False Then
        Command3.Enabled = False
        Command4.Enabled = False
    Else
        Command3.Enabled = True
        Command4.Enabled = True
    End If
    Command5.Enabled = True
    DetectFFCB.Enabled = True
    MaxFramesCB.Enabled = True
    DisableMIOTCB.Enabled = True
    MeasureValueCB.Enabled = True
End Sub

Private Sub SDK30Option_Click()
    Dim rCode As Integer
    Dim value As Long
    value = FTR_VERSION_PREVIOUS
    rCode = FTRSetParam(FTR_PARAM_VERSION, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
End Sub

Private Sub SDK35Option_Click()
    Dim rCode As Integer
    Dim value As Long
    value = FTR_VERSION_CURRENT
    rCode = FTRSetParam(FTR_PARAM_VERSION, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
End Sub

Private Sub SDKBOTHOption_Click()
    Dim rCode As Integer
    Dim value As Long
    value = FTR_VERSION_COMPATIBLE
    rCode = FTRSetParam(FTR_PARAM_VERSION, value)
    If rCode <> FTR_RETCODE_OK Then
        wePrintError rCode, 1
    End If
End Sub
