
 {******************************************************************************
  *
  * WorkedExBD - worked example using Futronic SDK.
  *
  * Errors.pas - processing SDK's return codes.
  *
  *}

unit Errors;

interface

uses
   Windows, StdCtrls, SDK_API;
   
//procedure TypeMsg( msgText: LPCSTR ); external 'TMainForm.pas';

const
 // Table of error return codes
TabErr: array[0..11] of PChar =
(
   'No errors',                           // FTR_RETCODE_OK
   'Unknown error code',                  // correspond FTR_RETCODE_ERROR_BASE value,
                                          // unlegal code
   'No memory',                           // FTR_RETCODE_NO_MEMORY
   'Invalid argument function call',      // FTR_RETCODE_INVALID_ARG
   'Repetition use some object (service)',// FTR_RETCODE_ALREADY_IN_USE
   'Sample is not correspond purpose',    // FTR_RETCODE_INVALID_PURPOSE
   'Internal SDK error',                  // FTR_RETCODE_INTERNAL_ERROR

   // Information return codes (may be)
   'Unable to capture',                   // FTR_RETCODE_UNABLE_TO_CAPTURE
   'Operation canceled by User',          // FTR_RETCODE_CANCELED_BY_USER
   'Number of retries is overflow',       // FTR_RETCODE_NO_MORE_RETRIES
   '',                                    // reserved
   'Source sampling is inconsistent'      // FTR_RETCODE_INCONSISTENT_SAMPLING
);

 // Table of device return codes
TabDev: array[0..8] of PChar =
(
   'Unknown error code',               // correspond FTR_RETCODE_DEVICE_BASE value,
                                       // unlegal code
   'Frame source not set',             // FTR_RETCODE_FRAME_SOURCE_NOT_SET
   'Device not connected',             // FTR_RETCODE_DEVICE_NOT_CONNECTED
   'Device failure',                   // FTR_RETCODE_DEVICE_FAILURE
   'Empty frame was returned',         // FTR_RETCODE_EMPTY_FRAME
   'Fake finger was detected',         // FTR_RETCODE_FAKE_SOURCE
   'Incompatible hardware detected',   // FTR_RETCODE_INCOMPATIBLE_HARDWARE
   'Upgrade the device firmware',      // FTR_RETCODE_INCOMPATIBLE_FIRMWARE
   'Frame source has been changed'     // FTR_RETCODE_FRAME_SOURCE_CHANGED
);

var
   MessageLabel:  ^TLabel;             // message reference

procedure wePrintError( rCode: FTRAPI_RESULT; outType: Integer );
function weGetErrorText( rCode: FTRAPI_RESULT ): LPCSTR;
procedure PrintTextMsg( lpTextMsg: LPCSTR );

implementation


 {******************************************************************************
  *
  * weGetErrorText   - get error text by code.
  * Syntax:
  *   function weGetErrorText( rCode: FTRAPI_RESULT ): LPCSTR;
  * Argument list:
  *   rCode - error code.
  * Return value:
  *   Error text pointer.
  *
  *}
function weGetErrorText( rCode: FTRAPI_RESULT ): LPCSTR;
var
   rType:   Integer;
   trCode:  FTRAPI_RESULT;
begin
   // define type of return code
   if rCode > FTR_RETCODE_DEVICE_BASE then
   begin
      rType := 1;
      trCode := rCode - FTR_RETCODE_DEVICE_BASE;
   end
   else
   begin
      rType := 0;
      trCode := rCode;
   end;

   // decoding
   if (rType = 1) and (rCode <= FTR_RETCODE_FRAME_SOURCE_CHANGED) then
   begin
      weGetErrorText := TabDev[trCode];
      Exit;
   end;
   if (rType = 0) and (rCode <= FTR_RETCODE_INCONSISTENT_SAMPLING) then
   begin
      weGetErrorText := TabErr[trCode];
      Exit;
   end;
   weGetErrorText := TabDev[0]; 
end;


 {******************************************************************************
  *
  * wePrintError  - print error text.
  * Syntax:
  *   procedure wePrintError( rCode: FTRAPI_RESULT; outType Integer );
  * Argument list:
  *   rCode    - error code;
  *   outType  - if 0 - output by MessageBox;
  *              if 1 - output by PrintTextMsg.
  *
  *}
procedure wePrintError( rCode: FTRAPI_RESULT; outType: Integer );
var
   errText:    LPCSTR;                 // error text
   errTitle:   array[0..31] of Char;   // error tytle for MessageBox
begin
   // get error text
   errText := weGetErrorText( rCode );

   // output
   if outType = 0 then
   begin
      if rCode > FTR_RETCODE_DEVICE_BASE then
         errTitle := 'Device return code'
      else
         errTitle := 'SDK return code';
      MessageBox( 0, errText, errTitle, MB_OK or MB_ICONINFORMATION );
      Exit;
   end
   else
   begin
      MessageLabel.Caption := errText;
      MessageBeep( MB_ICONEXCLAMATION );
   end;
end;


 {******************************************************************************
  *
  * PrintTextMsg  - print text message.
  * Syntax:
  *   procedure PrintTextMsg( lpTextMsg: LPCSTR );
  * Argument list:
  *   lpTextMsg   - text message.
  *
  *}
procedure PrintTextMsg( lpTextMsg: LPCSTR );
begin
   MessageLabel.Caption := lpTextMsg;
   MessageLabel.Repaint( );
   MessageBeep( MB_ICONEXCLAMATION );
end;

end.
