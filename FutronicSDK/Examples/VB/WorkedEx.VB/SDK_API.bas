Attribute VB_Name = "Ftr_SDK_API"
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Futronic SDK types
'

' Generic byte data.
Type FTR_DATA
    dwSize As Long          ' Length of data in bytes.
    pData As Long           ' Data pointer.
End Type

' Futronic SDK image data
Type FTR_BITMAP
    ftrWidth As Long        ' width in pixels
    ftrHeight As Long       ' height in pixels
    ftrBitmap As FTR_DATA   ' bitmap as FTR_DATA type
End Type

' Data types used for enrollment.
' Results of the enrollment process.
Type FTR_ENROLL_DATA
    dwSize As Long          ' The size of the structure in bytes.
    dwQuality As Long       ' Estimation of a template quality in terms of recognition:
                            ' 1 corresponds to the worst quality, 10 denotes the best.
End Type

' Array of identify records
Type FTR_IDENTIFY_ARRAY
    TotalNumber As Long     ' number of FTR_IDENTIFY_RECORD
    pMembers As Long        ' pointer on FTR_IDENTIFY_RECORD type
End Type

' Identify record description
Type FTR_IDENTIFY_RECORD
    KeyValue(15) As Byte    ' external key
    pData As Long           ' pointer on FTR_DATA type
End Type

' Match record description
Type FTR_MATCHED_RECORD
    KeyValue(15) As Byte    ' external key
    FarAttained As Long
End Type

' Array of match records
Type FTR_MATCHED_ARRAY
    TotalNumber As Long     ' number of FTR_MATCHED_RECORD
    pMembers As Long        ' pointer on FTR_MATCHED_RECORD
End Type

' Data capture progress information.
Type FTR_PROGRESS
    dwSize As Long          ' The size of the structure in bytes.
    dwCount As Long         ' Currently requested frame number.
    bIsRepeated As Long     ' Flag indicating whether the frame is requested not the first time.
    dwTotal As Long         ' Total number of frames to be captured.
End Type


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Futronic SDK constants
'

' Return code values.
Public Const FTR_RETCODE_ERROR_BASE As Integer = 1      ' Base value for the error codes.
Public Const FTR_RETCODE_DEVICE_BASE As Integer = 200   ' Base value for the device error codes.

Public Const FTR_RETCODE_OK As Integer = 0              ' Successful function completion.

Public Const FTR_RETCODE_NO_MEMORY As Integer = FTR_RETCODE_ERROR_BASE + 1
Public Const FTR_RETCODE_INVALID_ARG As Integer = FTR_RETCODE_ERROR_BASE + 2
Public Const FTR_RETCODE_ALREADY_IN_USE As Integer = FTR_RETCODE_ERROR_BASE + 3
Public Const FTR_RETCODE_INVALID_PURPOSE As Integer = FTR_RETCODE_ERROR_BASE + 4
Public Const FTR_RETCODE_INTERNAL_ERROR As Integer = FTR_RETCODE_ERROR_BASE + 5

Public Const FTR_RETCODE_UNABLE_TO_CAPTURE As Integer = FTR_RETCODE_ERROR_BASE + 6
Public Const FTR_RETCODE_CANCELED_BY_USER As Integer = FTR_RETCODE_ERROR_BASE + 7
Public Const FTR_RETCODE_NO_MORE_RETRIES As Integer = FTR_RETCODE_ERROR_BASE + 8
Public Const FTR_RETCODE_INCONSISTENT_SAMPLING As Integer = FTR_RETCODE_ERROR_BASE + 10

Public Const FTR_RETCODE_FRAME_SOURCE_NOT_SET As Integer = FTR_RETCODE_DEVICE_BASE + 1
Public Const FTR_RETCODE_DEVICE_NOT_CONNECTED As Integer = FTR_RETCODE_DEVICE_BASE + 2
Public Const FTR_RETCODE_DEVICE_FAILURE As Integer = FTR_RETCODE_DEVICE_BASE + 3
Public Const FTR_RETCODE_EMPTY_FRAME As Integer = FTR_RETCODE_DEVICE_BASE + 4
Public Const FTR_RETCODE_FAKE_SOURCE As Integer = FTR_RETCODE_DEVICE_BASE + 5
Public Const FTR_RETCODE_INCOMPATIBLE_HARDWARE As Integer = FTR_RETCODE_DEVICE_BASE + 6
Public Const FTR_RETCODE_INCOMPATIBLE_FIRMWARE As Integer = FTR_RETCODE_DEVICE_BASE + 7
Public Const FTR_RETCODE_FRAME_SOURCE_CHANGED  As Integer = FTR_RETCODE_DEVICE_BASE + 8

' Values used for the parameter definition (FTRSetParam and FTRGetParam).
Public Const FTR_PARAM_IMAGE_WIDTH As Long = 1
Public Const FTR_PARAM_IMAGE_HEIGHT As Long = 2
Public Const FTR_PARAM_IMAGE_SIZE As Long = 3

Public Const FTR_PARAM_CB_FRAME_SOURCE As Long = 4
Public Const FTR_PARAM_CB_CONTROL As Long = 5

Public Const FTR_PARAM_MAX_MODELS As Long = 10
Public Const FTR_PARAM_MAX_TEMPLATE_SIZE As Long = 6
Public Const FTR_PARAM_MAX_FAR_REQUESTED As Long = 7
Public Const FTR_PARAM_MAX_FARN_REQUESTED As Long = 13

Public Const FTR_PARAM_SYS_ERROR_CODE As Long = 8

Public Const FTR_PARAM_FAKE_DETECT As Long = 9
Public Const FTR_PARAM_FFD_CONTROL As Long = 11

Public Const FTR_PARAM_MIOT_CONTROL As Long = 12

Public Const FTR_PARAM_VERSION As Long = 14

' Available frame sources. These device identifiers are intended to be used
' with the FTR_PARAM_CB_FRAME_SOURCE parameter.
Public Const FSD_UNDEFINED As Long = 0          ' No device attached.
Public Const FSD_FUTRONIC_USB As Long = 1       ' Futronic USB Fingerprint Scanner Device.

'
' User callback function definitions
'
' State bit mask values for user callback function.
Public Const FTR_STATE_FRAME_PROVIDED As Long = 1
Public Const FTR_STATE_SIGNAL_PROVIDED As Long = 2
' Signal values.
Public Const FTR_SIGNAL_UNDEFINED As Long = 0
Public Const FTR_SIGNAL_TOUCH_SENSOR As Long = 1
Public Const FTR_SIGNAL_TAKE_OFF As Long = 2
Public Const FTR_SIGNAL_FAKE_SOURCE As Long = 3
' Response values
Public Const FTR_CANCEL As Long = 1
Public Const FTR_CONTINUE As Long = 2

' Values used for the purpose definition
Public Const FTR_PURPOSE_IDENTIFY As Long = 2
Public Const FTR_PURPOSE_ENROLL As Long = 3

' Values used for the version definition.
Public Const FTR_VERSION_PREVIOUS   As Long = 1
Public Const FTR_VERSION_COMPATIBLE As Long = 2
Public Const FTR_VERSION_CURRENT    As Long = 3


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' Futronic SDK function prototypes
'
Declare Function FTRInitialize Lib "FtrAPI.DLL" () As Integer

Declare Function FTRTerminate Lib "FtrAPI.DLL" () As Integer

Declare Function FTRSetParam Lib "FtrAPI.DLL" _
    (ByVal Param As Long, ByVal value As Any) As Integer
    
Declare Function FTRGetParam Lib "FtrAPI.DLL" _
    (ByVal Param As Long, ByRef value As Any) As Integer
    
Declare Function FTRCaptureFrame Lib "FtrAPI.DLL" _
    (ByVal usrContext As Any, ByRef pFrameBuf As Any) As Integer
    
Declare Function FTREnroll Lib "FtrAPI.DLL" _
    (ByVal usrContext As Any, ByVal Purpose As Long, ByRef pTemplate As FTR_DATA) As Integer
    
Declare Function FTREnrollX Lib "FtrAPI.DLL" _
    (ByVal usrContext As Any, ByVal Purpose As Long, ByRef pTemplate As FTR_DATA, _
     ByRef eData As FTR_ENROLL_DATA) As Integer
    
Declare Function FTRVerify Lib "FtrAPI.DLL" _
    (ByVal usrContext As Any, ByRef pTemplate As FTR_DATA, ByRef pResult As Boolean, _
     ByRef pFARVerify As Long) As Integer
     
Declare Function FTRVerifyN Lib "FtrAPI.DLL" _
    (ByVal usrContext As Any, ByRef pTemplate As FTR_DATA, ByRef pResult As Boolean, _
     ByRef pFARVerify As Long) As Integer
     
Declare Function FTRSetBaseTemplate Lib "FtrAPI.DLL" _
    (ByRef pTemplate As FTR_DATA) As Integer
    
Declare Function FTRIdentify Lib "FtrAPI.DLL" _
    (ByRef pAIdent As FTR_IDENTIFY_ARRAY, ByRef pdwMatchCnt As Long, _
     ByRef pAMatch As FTR_MATCHED_ARRAY) As Long
     
Declare Function FTRIdentifyN Lib "FtrAPI.DLL" _
    (ByRef pAIdent As FTR_IDENTIFY_ARRAY, ByRef pdwMatchCnt As Long, _
     ByRef pAMatch As FTR_MATCHED_ARRAY) As Long

