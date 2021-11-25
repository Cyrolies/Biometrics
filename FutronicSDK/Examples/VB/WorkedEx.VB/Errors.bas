Attribute VB_Name = "Errors"
Public Declare Function MessageBeep Lib "user32" (ByVal wType As Long) As Long
Public Declare Function UpdateWindow Lib "user32" (ByVal hwnd As Long) As Long


'
' weGetErrorText - get error text by code.
' Argument list:
'   rCode - error code.
' Return value:
'   string of error text.
'
Function weGetErrorText(rCode As Integer) As String
    Dim rType As Integer
    rType = 0
    Dim sErrText As String
    sErrText = "Unknown error"
    
    ' decoding
    If rCode > FTR_RETCODE_DEVICE_BASE Then
        Select Case rCode
            Case FTR_RETCODE_FRAME_SOURCE_NOT_SET
                sErrText = "Frame source not set"
            Case FTR_RETCODE_DEVICE_NOT_CONNECTED
                sErrText = "Device not connected"
            Case FTR_RETCODE_DEVICE_FAILURE
                sErrText = "Device failure"
            Case FTR_RETCODE_EMPTY_FRAME
                sErrText = "Empty frame was returned"
            Case FTR_RETCODE_FAKE_SOURCE
                sErrText = "Fake finger was detected"
            Case FTR_RETCODE_INCOMPATIBLE_HARDWARE
                sErrText = "Incompatible hardware detected"
            Case FTR_RETCODE_INCOMPATIBLE_FIRMWARE
                sErrText = "Upgrade the device firmware"
            Case FTR_RETCODE_FRAME_SOURCE_CHANGED
                sErrText = "Frame source has been changed"
        End Select
    Else
        Select Case rCode
            Case FTR_RETCODE_OK
                sErrText = "No errors"
            Case FTR_RETCODE_NO_MEMORY
                sErrText = "No memory"
            Case FTR_RETCODE_INVALID_ARG
                sErrText = "Invalid argument function call"
            Case FTR_RETCODE_ALREADY_IN_USE
                sErrText = "Repetition use some object (service)"
            Case FTR_RETCODE_INVALID_PURPOSE
                sErrText = "Sample is not correspond purpose"
            Case FTR_RETCODE_INTERNAL_ERROR
                sErrText = "Internal SDK error"
            ' Information return codes (may be)
            Case FTR_RETCODE_UNABLE_TO_CAPTURE
                sErrText = "Unable to capture"
            Case FTR_RETCODE_CANCELED_BY_USER
                sErrText = "Operation canceled by User"
            Case FTR_RETCODE_NO_MORE_RETRIES
                sErrText = "Number of retries is overflow"
            Case FTR_RETCODE_INCONSISTENT_SAMPLING
                sErrText = "Source sampling is inconsistent"
        End Select
    End If
    
    weGetErrorText = sErrText
    
End Function

' wePrintError - print error text.
' Argument list:
'   rCode   - error code;
'   outType - if 0 - output by MessageBox;
'             if 1 - output by PrintTextMsg.
Sub wePrintError(rCode As Integer, outType As Integer)
    Dim sErrText As String
    Dim errTitle As String
    
    ' get error text
    sErrText = weGetErrorText(rCode)
    
    'output
    If outType = 0 Then
        If rCode > FTR_RETCODE_DEVICE_BASE Then
            errTitle = "Device return code"
        Else
            errTitle = "SDK return code"
        End If
        ' print error
        MsgBox sErrText, vbOKOnly + vbCritical, errTitle
    Else
        MainWindow.PrintTextMsg (sErrText)
    End If
End Sub

