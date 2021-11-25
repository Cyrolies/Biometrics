unit SDK_API;

interface

uses
   Windows;


 {******************************************************************************
  *
  * Futronic SDK types
  *
  *}
type
FTRAPI_RESULT     = DWORD;

FTR_HANDLE        = DWORD;
FTR_HANDLE_PTR    = ^FTR_HANDLE;

// Types used for the parameter definition.
FTR_PARAM         = DWORD;
FTR_PARAM_VALUE   = ^DWORD;
FTR_PARAM_VALUE_PTR = ^FTR_PARAM_VALUE;

// Type used for the purpose definition.
FTR_PURPOSE       = DWORD;

// User callback context.
FTR_USER_CTX      = Pointer;

// Version definition type.
FTR_VERSION       = DWORD;

// State bit mask type.
FTR_STATE         = DWORD;

// Signal type.
FTR_SIGNAL        = DWORD;
FTR_SIGNAL_PTR    = ^DWORD;

// Response type.
FTR_RESPONSE      = DWORD;
FTR_RESPONSE_PTR  = ^FTR_RESPONSE;

// False acception ratio (FAR) type.
FTR_FAR           = DWORD;
FTR_FAR_PTR       = ^FTR_FAR;
FTR_FARN          = Integer;
FTR_FARN_PTR      = ^FTR_FARN;
FAR_ATTAINED      = DWORD;
FAR_ATTAINED_PTR  = ^FAR_ATTAINED;

// External key type.
FTR_DATA_KEY      = array[0..15] of Char;

// Generic byte data.
FTR_DATA          = packed record
   dwSize:     DWORD;                  // Length of data in bytes.
   pData:      Pointer;                // Data pointer.
end;
FTR_DATA_PTR      = ^FTR_DATA;

// Bitmap data type.
FTR_BITMAP        = packed record
   Width:      DWORD;
   Height:     DWORD;
   Bitmap:     FTR_DATA;
end;
FTR_BITMAP_PTR    = ^FTR_BITMAP;

// Data capture progress information.
FTR_PROGRESS      = packed record
   dwSize:     DWORD;                  // The size of the structure in bytes.
   dwCount:    DWORD;                  // Currently requested frame number.
   bIsRepeated:BOOL;                   // Flag indicating whether the frame is
                                       // requested not the first time.
   dwTotal:    DWORD;                  // Total number of frames to be captured.
end;
FTR_PROGRESS_PTR  = ^FTR_PROGRESS;

// Data types used for operation of identification.
FTR_IDENTIFY_RECORD  = packed record
   KeyValue:   FTR_DATA_KEY;
   pData:      FTR_DATA_PTR;
end;
FTR_IDENTIFY_RECORD_PTR = ^FTR_IDENTIFY_RECORD;

// Array of identify records.
FTR_IDENTIFY_ARRAY   = packed record
   TotalNumber:   DWORD;
   pMembers:      FTR_IDENTIFY_RECORD_PTR;
end;
FTR_IDENTIFY_ARRAY_PTR  = ^FTR_IDENTIFY_ARRAY;

// Match record description.
FTR_MATCHED_RECORD   = packed record
   KeyValue:      FTR_DATA_KEY;
   FarAttained:   FTR_FAR;
end;
FTR_MATCHED_RECORD_PTR  = ^FTR_MATCHED_RECORD;
FTR_MATCHED_X_RECORD = packed record
   KeyValue:      FTR_DATA_KEY;
   FarAttained:   FAR_ATTAINED;
end;
FTR_MATCHED_X_RECORD_PTR   = ^FTR_MATCHED_X_RECORD;

// Array of match records.
FTR_MATCHED_ARRAY    = packed record
   TotalNumber:   DWORD;
   pMembers:      FTR_MATCHED_RECORD_PTR;
end;
FTR_MATCHED_ARRAY_PTR   = ^FTR_MATCHED_ARRAY;
// Array of match records with numerical FAR value.
FTR_MATCHED_X_ARRAY  = packed record
   TotalNumber:   DWORD;
   pMembers:      FTR_MATCHED_X_RECORD_PTR;
end;
FTR_MATCHED_X_ARRAY_PTR   = ^FTR_MATCHED_X_ARRAY;

// Data types used for enrollment.
// Results of the enrollment process.
FTR_ENROLL_DATA      = packed record
   dwSize:     DWORD;                  // The size of the structure in bytes.
   dwQuality:  DWORD;                  // Estimation of a template quality in
                                       // terms of recognition:
                                       // 1 corresponds to the worst quality,
                                       // 10 denotes the best.
end;
FTR_ENROLL_DATA_PTR     = ^FTR_ENROLL_DATA;

// A callback function that is supplied by an application and used
// to control capture, enrollment or verification execution flow.
FTR_CB_STATE_CONTROL =
   procedure(
      Context:    FTR_USER_CTX;        // (input) - user-defined context information.
      StateMask:  FTR_STATE;           // (input) - a bit mask indicating what
                                       //           arguments are provided.
      pResponse:  FTR_RESPONSE_PTR;    // (output) - API function execution control
                                       //            is achieved through this value.
      Signal:     FTR_SIGNAL;          // (input) - this signal should be used
                                       //           to interact with a user.
      pBitmap:    FTR_BITMAP_PTR );    // (input) - a pointer to the bitmap to be
                                       //           displayed.



 {******************************************************************************
  *
  * Futronic SDK constants
  *
  *}
const
 //
 // Return code values
 //
FTR_RETCODE_ERROR_BASE:    DWORD = 1;     // Base value for the error codes
FTR_RETCODE_DEVICE_BASE:   DWORD = 200;   // Base value for the device error codes

FTR_RETCODE_OK:            DWORD = 0;     // Successful function completion

FTR_RETCODE_NO_MEMORY:              DWORD = 1 + 1;
FTR_RETCODE_INVALID_ARG:            DWORD = 1 + 2;
FTR_RETCODE_ALREADY_IN_USE:         DWORD = 1 + 3;
FTR_RETCODE_INVALID_PURPOSE:        DWORD = 1 + 4;
FTR_RETCODE_INTERNAL_ERROR:         DWORD = 1 + 5;
FTR_RETCODE_UNABLE_TO_CAPTURE:      DWORD = 1 + 6;
FTR_RETCODE_CANCELED_BY_USER:       DWORD = 1 + 7;
FTR_RETCODE_NO_MORE_RETRIES:        DWORD = 1 + 8;
FTR_RETCODE_INCONSISTENT_SAMPLING:  DWORD = 1 + 10;
FTR_RETCODE_TRIAL_EXPIRED:          DWORD = 1 + 11;

FTR_RETCODE_FRAME_SOURCE_NOT_SET:   DWORD = 200 + 1;
FTR_RETCODE_DEVICE_NOT_CONNECTED:   DWORD = 200 + 2;
FTR_RETCODE_DEVICE_FAILURE:         DWORD = 200 + 3;
FTR_RETCODE_EMPTY_FRAME:            DWORD = 200 + 4;
FTR_RETCODE_FAKE_SOURCE:            DWORD = 200 + 5;
FTR_RETCODE_INCOMPATIBLE_HARDWARE:  DWORD = 200 + 6;
FTR_RETCODE_INCOMPATIBLE_FIRMWARE:  DWORD = 200 + 7;
FTR_RETCODE_FRAME_SOURCE_CHANGED:   DWORD = 200 + 8;

//
// Values used for the parameter definition.
//
FTR_PARAM_IMAGE_WIDTH:              DWORD = 1;
FTR_PARAM_IMAGE_HEIGHT:             DWORD = 2;
FTR_PARAM_IMAGE_SIZE:               DWORD = 3;

FTR_PARAM_CB_FRAME_SOURCE:          DWORD = 4;
FTR_PARAM_CB_CONTROL:               DWORD = 5;

FTR_PARAM_MAX_MODELS:               DWORD = 10;
FTR_PARAM_MAX_TEMPLATE_SIZE:        DWORD = 6;
FTR_PARAM_MAX_FAR_REQUESTED:        DWORD = 7;
FTR_PARAM_MAX_FARN_REQUESTED:       DWORD = 13;

FTR_PARAM_SYS_ERROR_CODE:           DWORD = 8;

FTR_PARAM_FAKE_DETECT:              DWORD = 9;
FTR_PARAM_FFD_CONTROL:              DWORD = 11;

FTR_PARAM_MIOT_CONTROL:             DWORD = 12;

FTR_PARAM_VERSION:                  DWORD = 14;

FTR_PARAM_CHECK_TRIAL:              DWORD = 15;

// Available frame sources. These device identifiers are intended to be used
// with the FTR_PARAM_CB_FRAME_SOURCE parameter.
FSD_UNDEFINED:                      DWORD = 0;  // No device attached.
FSD_FUTRONIC_USB:                   DWORD = 1;  // Futronic USB Fingerprint
                                                // Scanner Device.
// Values used for the purpose definition.
//FTR_PURPOSE_VERIFY:                 DWORD = 1;
FTR_PURPOSE_IDENTIFY:               DWORD = 2;
FTR_PURPOSE_ENROLL:                 DWORD = 3;
FTR_PURPOSE_COMPATIBILITY:          DWORD = 4;

// Values used for the version definition.
FTR_VERSION_PREVIOUS:               DWORD = 1;
FTR_VERSION_COMPATIBLE:             DWORD = 2;
FTR_VERSION_CURRENT:                DWORD = 3;

// State bit mask values.
FTR_STATE_FRAME_PROVIDED:           DWORD = 1;
FTR_STATE_SIGNAL_PROVIDED:          DWORD = 2;

// Signal values.
FTR_SIGNAL_UNDEFINED:               DWORD = 0;
FTR_SIGNAL_TOUCH_SENSOR:            DWORD = 1;
FTR_SIGNAL_TAKE_OFF:                DWORD = 2;
FTR_SIGNAL_FAKE_SOURCE:             DWORD = 3;

// Response values.
FTR_CANCEL:                         DWORD = 1;
FTR_CONTINUE:                       DWORD = 2;


 {******************************************************************************
  *
  * Futronic SDK function prototypes
  *
  *}

{*
 * FTRInitialize - activates the Futronic API interface. This function must be called
 *   before any other API call.
 *
 * Parameters:
 *   This function has no parameters.
 *
 *  Returns result code.
 *   FTR_RETCODE_OK - success, to finish the API usage call the FTRTerminate function;
 *   FTR_RETCODE_ALREADY_IN_USE - the API has been already initialized;
 *   FTR_RETCODE_NO_MEMORY - not enough memory to perform the operation.
 *}
function FTRInitialize( ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRTerminate - releases all previously allocated resources and completes the API usage.
 *   This call must be the last API call in the case of SUCCESSFULL FTRInitialize return.
 *
 * Parameters:
 *   This function has no parameters.
 *
 * Returns void.
 *}
procedure FTRTerminate( ); stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRSetParam - sets the indicated parameter value.
 *
 * Parameters:
 *   Param: FTR_PARAM - this argument indicates the parameter which value is passed through
 *                      the Value argument;
 *
 *   Value: FTR_PARAM_VALUE - the value to be set.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success,
 *   FTR_RETCODE_INVALID_ARG - the value of the required parameter could not be set,
 *   FTR_RETCODE_NO_MEMORY - not enough memory to perform the operation.
 *}
function FTRSetParam( Param: FTR_PARAM; Value: FTR_PARAM_VALUE ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRGetParam - gets the value of the specified parameter.
 *
 * Parameters:
 *   Param: FTR_PARAM - this argument indicates the parameter which value must be placed
 *                      at the address passed through the pValue argument;
 *
 *   pValue: FTR_PARAM_VALUE_PTR - a pointer to the value.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success, the required value is written at the pValue address.
 *}
function FTRGetParam( Param: FTR_PARAM; pValue: FTR_PARAM_VALUE_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRCaptureFrame - gets an image from the current frame source.
 *
 * Parameters:
 *   UserContext: FTR_USER_CTX - optional caller-supplied value that is passed to callback
 *                               functions. This value is provided for convenience in
 *                               application design.
 *
 *   pFrameBuf: Pointer - points to a buffer large enough to hold the frame data.
 *                        The size of a frame can be determined through the FTRGetParam call
 *                        with the FTR_PARAM_IMAGE_SIZE value of the first argument.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *}
function FTRCaptureFrame( UserContext: FTR_USER_CTX; pFrameBuf: Pointer ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTREnrollX - creates the fingerprint template for the desired purpose.
 *
 * Parameters:
 *   UserContext: FTR_USER_CTX - optional caller-supplied value that is passed to callback
 *                               functions. This value is provided for convenience in
 *                               application design.
 *
 *   Purpose: FTR_PURPOSE - the purpose of template building. This value designates the
 *                          intended way of further template usage and can be one of the
 *                          following:
 *
 *                          FTR_PURPOSE_ENROLL - the created template is suitable for both
 *                          identification and verification purpose.
 *
 *                          FTR_PURPOSE_IDENTIFY - corresponding template can be used only
 *                          for identification as an input for the FTRSetBaseTemplate function.
 *
 *   pTemplate: FTR_DATA_PTR - pointer to a result memory buffer. The space for this buffer
 *                             must be reservered by a caller. Maximum space amount can be
 *                             determined through the FTRGetParam call with the
 *                             FTR_PARAM_MAX_TEMPLATE_SIZE value of the first argument.
 *
 *   pEData: FTR_ENROLL_DATA_PTR - optional pointer to the FTR_ENROLL_DATA structure that
 *                                 receives on output additional information on the results of
 *                                 the enrollment process. The caller must set the dwSize member
 *                                 of this structure to sizeof(FTR_ENROLL_DATA) in order to
 *                                 identify the version of the structure being passed.
 *                                 If a caller does not initialize dwSize, the function fails.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *}
function FTREnrollX( UserContext: FTR_USER_CTX; Purpose: FTR_PURPOSE;
                     pTemplate: FTR_DATA_PTR; pEData: FTR_ENROLL_DATA_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTREnroll - creates the fingerprint template for the desired purpose.
 *
 * Parameters:
 *   UserContext: FTR_USER_CTX - optional caller-supplied value that is passed to callback
 *                               functions. This value is provided for convenience in
 *                               application design.
 *
 *   Purpose: FTR_PURPOSE - the purpose of template building. This value designates the
 *                          intended way of further template usage and can be one of the
 *                          following:
 *
 *                          FTR_PURPOSE_ENROLL - the created template is suitable for both
 *                          identification and verification purpose.
 *
 *                          FTR_PURPOSE_IDENTIFY - corresponding template can be used only
 *                          for identification as an input for the FTRSetBaseTemplate function.
 *
 *   pTemplate: FTR_DATA_PTR - pointer to a result memory buffer. The space for this buffer
 *                             must be reservered by a caller. Maximum space amount can be
 *                             determined through the FTRGetParam call with the
 *                             FTR_PARAM_MAX_TEMPLATE_SIZE value of the first argument.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *}
function FTREnroll( UserContext: FTR_USER_CTX; Purpose: FTR_PURPOSE;
                    pTemplate: FTR_DATA_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRVerify - this function captures an image from the currently attached frame source,
 *   builds the corresponding template and compares it with the source template passed
 *   in the pTemplate parameter.
 *
 * Parameters:
 *   UserContext: FTR_USER_CTX - optional caller-supplied value that is passed to callback
 *                               functions. This value is provided for convenience in
 *                               application design.
 *
 *   pTemplate: FTR_DATA_PTR - pointer to a source template for verification.
 *
 *   pResult: Pointer - points to a bool value indicating whether the captured image matched to
 *                      the source template.
 *
 *   pFARVerify: FTR_FAR_PTR - points to the optional FAR level achieved.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - the input template was not built with the FTR_PURPOSE_ENROLL purpose;
 *   FTR_RETCODE_INVALID_ARG - the template is corrupted or has invalid data.                            
 *}
function FTRVerify( UserContext: FTR_USER_CTX; pTemplate: FTR_DATA_PTR; pResult: Pointer;
                    pFARVerify: FTR_FAR_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRVerifyN - this function captures an image from the currently attached frame source,
 *   builds the corresponding template and compares it with the source template passed
 *   in the pTemplate parameter.
 *
 * Parameters:
 *   UserContext: FTR_USER_CTX - optional caller-supplied value that is passed to callback
 *                               functions. This value is provided for convenience in
 *                               application design.
 *
 *   pTemplate: FTR_DATA_PTR - pointer to a source template for verification.
 *
 *   pResult: Pointer - points to a value indicating whether the captured image matched to
 *                      the source template.
 *
 *   pFARVerify: FTR_FARN_PTR - points to the optional FAR Numerical level achieved.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - the input template was not built with the FTR_PURPOSE_ENROLL purpose;
 *   FTR_RETCODE_INVALID_ARG - the template is corrupted or has invalid data.                            
 *}
function FTRVerifyN( UserContext: FTR_USER_CTX; pTemplate: FTR_DATA_PTR; pResult: Pointer;
                     pFARVerify: FTR_FARN_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRSetBaseTemplate - installs a template as a base for identification process.
 *   The passed template must have been enrolled for identification purpose, i.e.
 *   the FTR_PURPOSE_IDENTIFY purpose value must be used for its enrollment.
 *   Identification process is organized in one or more FTRIdentify calls.
 *
 * Parameters:
 *   pTemplate: FTR_DATA_PTR - pointer to a previously enrolled template.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *   FTR_RETCODE_INVALID_PURPOSE - the template was not built with FTR_PURPOSE_IDENTIFY value.
 *                               
 *}
function FTRSetBaseTemplate( pTemplate: FTR_DATA_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

{*
 * FTRIdentify - compares the base template against a set of source templates. The
 *   matching is performed in terms of FAR (False Accepting Ratio), which designates
 *   the probability of falsely matching of the base template to the source template.
 *
 * Parameters:
 *   pAIdent: FTR_IDENTIFY_ARRAY_PTR - points to a set of the source templates.
 *
 *   pdwMatchCnt: Pointer - number of matched records in the array pointed to by the pAMatch
 *                          argument.
 *
 *   pAMatch: FTR_MATCHED_ARRAY_PTR - pointer to the array of matched records. A caller is
 *                                    responsible for reserving appropriate memory space and
 *                                    proper initialization of this structure.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - there is a template built with the purpose other than 
 *                                 FTR_PURPOSE_ENROLL value in the pAIdent array.
 *                               
 *}
function FTRIdentify( pAIdent: FTR_IDENTIFY_ARRAY_PTR; pdwMatchCnt: Pointer;
                      pAMatch: FTR_MATCHED_ARRAY_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';


{*
 * FTRIdentifyN - compares the base template against a set of source templates. The
 *   matching is performed in terms of FAR (False Accepting Ratio), which designates
 *   the probability of falsely matching of the base template to the source template.
 *
 * Parameters:
 *   pAIdent: FTR_IDENTIFY_ARRAY_PTR - points to a set of the source templates.
 *
 *   pdwMatchCnt: Pointer - number of matched records in the array pointed to by the pAMatch
 *                          argument.
 *
 *   pAMatch: FTR_MATCHED_X_ARRAY_PTR - pointer to the array of matched records. A caller is
 *                                      responsible for reserving appropriate memory space and
 *                                      proper initialization of this structure.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - there is a template built with the purpose other than 
 *                                 FTR_PURPOSE_ENROLL value in the pAIdent array.
 *                               
 *}
function FTRIdentifyN( pAIdent: FTR_IDENTIFY_ARRAY_PTR; pdwMatchCnt: Pointer;
                       pAMatch: FTR_MATCHED_X_ARRAY_PTR ): FTRAPI_RESULT; stdcall; far; external 'FtrAPI.DLL';

implementation

end.


