
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * Errors.c   - processing SDK's return codes.
  *
  */

#include "WorkedEx.h"


 /******************************************************************************
  *
  * Global variables.
  *
  */
 // Table of error return codes
char  TabErr[][64] = 
{
   "No errors",                        // FTR_RETCODE_OK
   "Unknown error code",               // correspond FTR_RETCODE_ERROR_BASE value,
                                       // unlegal code
   "No memory",                        // FTR_RETCODE_NO_MEMORY
   "Invalid argument function call",   // FTR_RETCODE_INVALID_ARG
   "Repetition use some object (service)",   // FTR_RETCODE_ALREADY_IN_USE
   "Sample is not correspond purpose", // FTR_RETCODE_INVALID_PURPOSE
   "Internal SDK error",               // FTR_RETCODE_INTERNAL_ERROR

   // Information return codes (may be)
   "Unable to capture",                // FTR_RETCODE_UNABLE_TO_CAPTURE
   "Operation canceled by User",       // FTR_RETCODE_CANCELED_BY_USER
   "Number of retries is overflow",    // FTR_RETCODE_NO_MORE_RETRIES
   "",                                 // reserved
   "Source sampling is inconsistent"   // FTR_RETCODE_INCONSISTENT_SAMPLING
};

 // Table of device return codes
char TabDev[][64] =
{
   "Unknown error code",               // correspond FTR_RETCODE_DEVICE_BASE value,
                                       // unlegal code
   "Frame source not set",             // FTR_RETCODE_FRAME_SOURCE_NOT_SET
   "Device not connected",             // FTR_RETCODE_DEVICE_NOT_CONNECTED
   "Device failure",                   // FTR_RETCODE_DEVICE_FAILURE
   "Empty frame was returned",         // FTR_RETCODE_EMPTY_FRAME
   "Fake finger was detected",         // FTR_RETCODE_FAKE_SOURCE
   "Incompatible hardware detected",   // FTR_RETCODE_INCOMPATIBLE_HARDWARE
   "Upgrade the device firmware",      // FTR_RETCODE_INCOMPATIBLE_FIRMWARE
   "Frame source has been changed"     // FTR_RETCODE_FRAME_SOURCE_CHANGED
};


 /******************************************************************************
  *
  * weGetErrorText   - get error text by code.
  * Syntax:
  *   LPCSTR weGetErrorText( FTRAPI_RESULT rCode );
  * Argument list:
  *   rCode - error code.
  * Return value:
  *   Error text pointer.
  *
  */
LPCSTR weGetErrorText( FTRAPI_RESULT rCode )
{
   int            rType = 0;
   FTRAPI_RESULT  trCode;

   // define type of return code
   if( rCode > FTR_RETCODE_DEVICE_BASE )
   {
      rType = 1;
      trCode = rCode - FTR_RETCODE_DEVICE_BASE;
   }
   else
      trCode = rCode;

   // decoding
   if( rType && (rCode <= FTR_RETCODE_FRAME_SOURCE_CHANGED) )
      return TabDev[trCode];
   if( !rType && (rCode <= FTR_RETCODE_INCONSISTENT_SAMPLING) )
      return TabErr[trCode];
   return TabErr[1];
}


 /******************************************************************************
  *
  * wePrintError  - print error text.
  * Syntax:
  *   void wePrintError( FTRAPI_RESULT rCode, int outType );
  * Argument list:
  *   rCode    - error code;
  *   outType  - if 0 - output by MessageBox;
  *              if 1 - output by PrintTextMsg.
  *
  */
void wePrintError( FTRAPI_RESULT rCode, int outType )
{
   LPCSTR   errText;                   // error text

   // get error text
   errText = weGetErrorText( rCode );

   // output
   if( outType == 0 )
   {
      char  errTitle[32];

      if( rCode > FTR_RETCODE_DEVICE_BASE )
         strcpy( errTitle, "Device return code" );
      else
         strcpy( errTitle, "SDK return code" );

      MessageBox( hMainWnd, errText, errTitle, MB_OK | MB_ICONINFORMATION );
   }
   else
      PrintTextMsg( errText );
}
