
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * ControlDlg.c  - application's control dialog.
  *
  */

#include "WorkedEx.h"


void SetVersionCompatibleCtrl( HWND hDWnd, FTR_VERSION Version );
FTR_VERSION GetVersionCompatibleCtrl( HWND hDWnd );

 /******************************************************************************
  *
  * MainDlgProc   - control dialog pocedure.
  *
  */
INT_PTR CALLBACK MainDlgProc( HWND  hDWnd, UINT  uMsg, WPARAM wParam, LPARAM lParam )
{
   switch( uMsg )
   {
   case WM_INITDIALOG:
      {
         int   iCyc;
         char  cbItem[16];
         HWND  hCBWnd;
         FTRAPI_RESULT  rCode;   // SDK return code
         FTR_VERSION Version;

         // define radio buttons state
         CheckDlgButton( hDWnd, IDC_CAPTOSCREEN, BST_CHECKED );
         CheckDlgButton( hDWnd, IDC_FROMSCANER, BST_CHECKED );
         CheckDlgButton( hDWnd, IDC_DETECT_FAKE, BST_UNCHECKED );
         CheckDlgButton( hDWnd, IDC_FAST_MODE, BST_UNCHECKED );
         CheckDlgButton( hDWnd, IDC_DETECT_MIOT, BST_UNCHECKED );

         // fill IDC_CMAX_FRAMES control
         hCBWnd = GetDlgItem( hDWnd, IDC_CMAX_FRAMES );
         for( iCyc = 1; iCyc < 11; iCyc++ )
         {
            sprintf( cbItem, "%d", iCyc );
            SendMessage( hCBWnd, CB_ADDSTRING, 0, (LPARAM)cbItem );
         }
         SendMessage( hCBWnd, CB_SETCURSEL, (WPARAM)4, 0L );

         // fill IDC_MEASURE control
         hCBWnd = GetDlgItem( hDWnd, IDC_MEASURE );
         for( iCyc = 0; iCyc < M2PNUM; iCyc++ )
            SendMessage( hCBWnd, CB_ADDSTRING, 0, (LPARAM)M2PArray[iCyc].meas );
         SendMessage( hCBWnd, CB_SETCURSEL, (WPARAM)M2PDEFITEM, 0L );

                    
         rCode = FTRGetParam( FTR_PARAM_VERSION, (FTR_PARAM_VALUE)&Version );
         if( rCode != FTR_RETCODE_OK )
              wePrintError( rCode, 0 );

         SetVersionCompatibleCtrl( hDWnd, Version );

         return TRUE;
      }                                // case WM_INITDIALOG:

   case WM_COMMAND:
      {
         switch( LOWORD( wParam ) )
         {
         case ID_CAPTURE:
            {
               BOOL  bRet;
               bCancelOperation  = FALSE;
               if( IsDlgButtonChecked( hDWnd, IDC_CAPTOSCREEN ) == BST_CHECKED )
                  bRet = CaptureStarter( CAPTURE_TO_SCREEN );
               else
                  bRet = CaptureStarter( CAPTURE_TO_FILE );
               if( bRet )
                  UpdateControl( FALSE );
               else
                  PrintTextMsg( "Capture start is fail" );
               break;
            }                          // case ID_CAPTURE:

         case ID_ENROLL:
            {
               BOOL  bRet;
               bCancelOperation  = FALSE;
               if( IsDlgButtonChecked( hDWnd, IDC_FROMSCANER ) == BST_CHECKED )
                  bRet = EnrollStarter( CAPTURE_FROM_SCANER );
               else
                  bRet = EnrollStarter( CAPTURE_FROM_FILE );
               if( bRet )
                  UpdateControl( FALSE );
               else
                  PrintTextMsg( "Enroll start is fail" );
               break;
            }                          // case ID_ENROLL:

         case ID_VERIFY:
            {
               BOOL  bRet;
               bCancelOperation  = FALSE;
               if( IsDlgButtonChecked( hDWnd, IDC_FROMSCANER ) == BST_CHECKED )
                  bRet = VerifyStarter( CAPTURE_FROM_SCANER );
               else
                  bRet = VerifyStarter( CAPTURE_FROM_FILE );
               if( bRet )
                  UpdateControl( FALSE );
               else
                  PrintTextMsg( "Verify start is fail" );
               break;
            }                          // case ID_VERIFY:

         case ID_IDENTIFY:
            {
               BOOL  bRet;
               bCancelOperation  = FALSE;
               if( IsDlgButtonChecked( hDWnd, IDC_FROMSCANER ) == BST_CHECKED )
                  bRet = IdentifyStarter( CAPTURE_FROM_SCANER );
               else
                  bRet = IdentifyStarter( CAPTURE_FROM_FILE );
               if( bRet )
                  UpdateControl( FALSE );
               else
                  PrintTextMsg( "Identify start is fail" );
               break;
            }                          // case ID_IDENTIFY:

         case ID_STOPOPERATION:
            bCancelOperation  = TRUE;
            //UpdateControl( TRUE );
            break;

         case ID_EXIT:
            DestroyWindow( hMainWnd );
            break;

         case IDC_DETECT_FAKE:
            if( HIWORD( wParam ) == BN_CLICKED )
            {
               // "Detect fake finger" mode was changed
               BOOL  bValue;
               FTRAPI_RESULT  rCode;   // SDK return code

               if( IsDlgButtonChecked( hDWnd, IDC_DETECT_FAKE ) == BST_CHECKED )
                  bValue = TRUE;
               else
                  bValue = FALSE;
               rCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, (FTR_PARAM_VALUE)bValue );
               if( rCode != FTR_RETCODE_OK )
                  wePrintError( rCode, 0 );
            }
            break;                     // case IDC_DETECT_FAKE:

         case IDC_FAST_MODE:
            if( HIWORD( wParam ) == BN_CLICKED )
            {
               // Fast Mode was changed
               BOOL  bValue;
               FTRAPI_RESULT  rCode;   // SDK return code

               if( IsDlgButtonChecked( hDWnd, IDC_FAST_MODE ) == BST_CHECKED )
                  bValue = TRUE;
               else
                  bValue = FALSE;
               rCode = FTRSetParam( FTR_PARAM_FAST_MODE, (FTR_PARAM_VALUE)bValue );
               if( rCode != FTR_RETCODE_OK )
                  wePrintError( rCode, 0 );
            }
            break;                     // case IDC_FAST_MODE:

         case IDC_DETECT_MIOT:
            if( HIWORD( wParam ) == BN_CLICKED )
            {
               // MIOT mode was changed
               BOOL  bValue;
               FTRAPI_RESULT  rCode;   // SDK return code

               if( IsDlgButtonChecked( hDWnd, IDC_DETECT_MIOT ) == BST_CHECKED )
                  bValue = TRUE;
               else
                  bValue = FALSE;
               rCode = FTRSetParam( FTR_PARAM_MIOT_CONTROL, (FTR_PARAM_VALUE)bValue );
               if( rCode != FTR_RETCODE_OK )
                  wePrintError( rCode, 0 );
            }
            break;                     // case IDC_DETECT_MIOT:

         case IDC_CMAX_FRAMES:
            {
               if( HIWORD( wParam ) == CBN_SELCHANGE )
               {
                  HWND  hCBWnd;
                  char  cMaxFrames[16];
                  int   iMaxFrames;
                  FTRAPI_RESULT  rCode;// SDK return code

                  hCBWnd = GetDlgItem( hDWnd, IDC_CMAX_FRAMES );
                  iMaxFrames = (int)SendMessage( hCBWnd, CB_GETCURSEL, 0, 0L );
                  SendMessage( hCBWnd, CB_GETLBTEXT, (WPARAM)iMaxFrames, (LPARAM)cMaxFrames );
                  iMaxFrames = atoi( cMaxFrames );
                  rCode = FTRSetParam( FTR_PARAM_MAX_MODELS, (FTR_PARAM_VALUE)iMaxFrames );
                  if( rCode != FTR_RETCODE_OK )
                     wePrintError( rCode, 0 );
               }
               break;
            }                          // case IDC_CMAX_FRAMES:

         case IDC_MEASURE:
            {
               if( HIWORD( wParam ) == CBN_SELCHANGE )
               {
                  HWND  hCBWnd;
                  int   iItem;
                  char  cItemText[4];
                  long  prob;
                  FTRAPI_RESULT  rCode;// SDK return code

                  hCBWnd = GetDlgItem( hDWnd, IDC_MEASURE );
                  iItem = (int)SendMessage( hCBWnd, CB_GETCURSEL, 0, 0L );
                  SendMessage( hCBWnd, CB_GETLBTEXT, (WPARAM)iItem, (LPARAM)cItemText );
                  /* Use FAR option
                  prob = CalcPfromM( cItemText );
                  if( prob == -1 )
                     PrintTextMsg( "Unknown measure value" );
                  else
                  {
                     rCode = FTRSetParam( FTR_PARAM_MAX_FAR_REQUESTED, (FTR_PARAM_VALUE)prob );
                     if( rCode != FTR_RETCODE_OK )
                        wePrintError( rCode, 0 );
                  }
                  */
                  // Use FARN option
                  if( strcmp( cItemText, "Max" ) == 0 )
                     prob = 1000;
                  else
                     prob = atol( cItemText );
                  rCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, (FTR_PARAM_VALUE)prob );
                  if( rCode != FTR_RETCODE_OK )
                     wePrintError( rCode, 0 );
               }
               break;
            } 
            // case IDC_MEASURE:

         case IDC_SDK3_RADIO:
         case IDC_SDK35_RADIO:
         case IDC_SDKBOTH_RADIO:
             {
                if( HIWORD( wParam ) == BN_CLICKED )
                {
                    FTR_VERSION NewVersion = GetVersionCompatibleCtrl( hDWnd );
                    FTRAPI_RESULT  rCode = FTRSetParam( FTR_PARAM_VERSION, (FTR_PARAM_VALUE)NewVersion );

                    if( rCode != FTR_RETCODE_OK )
                     wePrintError( rCode, 0 );
             
                }
                break;
             }

         }                             // switch( LOWORD( wParam ) )
         break;
      }                                // case WM_COMMAND:

   }                                   // switch( uMsg )
   return FALSE;
}


 /******************************************************************************
  *
  * PrintTextMsg  - print text message into IDC_TEXTMESSAGE dialog element in
  *                 application's control dialog.
  * Syntax:
  *   void PrintTextMsg( LPCSTR lpTextMsg );
  * Argument list:
  *   lpTextMsg   - text message.
  *
  */
void PrintTextMsg( LPCSTR lpTextMsg )
{
   SetDlgItemText( hDlg, IDC_TEXTMESSAGE, lpTextMsg );
   MessageBeep( -1 );
}


 /******************************************************************************
  *
  * UpdateControl - enable/disable control buttons while process operation.
  * Syntax:
  *   void UpdateControl( BOOL sw );
  * Argument list:
  *   sw - TRUE switch ON all control biometric buttons;
  *        FALSE switch OFF all control biometric buttons.
  *
  */
void UpdateControl( BOOL sw )
{
   if( sw )
   {
      EnableWindow( GetDlgItem( hDlg, ID_EXIT ), TRUE );
      EnableWindow( GetDlgItem( hDlg, ID_CAPTURE ), TRUE );
      EnableWindow( GetDlgItem( hDlg, ID_ENROLL ), TRUE );
      if( EmptyDB( ) )
      {
         EnableWindow( GetDlgItem( hDlg, ID_VERIFY ), FALSE );
         EnableWindow( GetDlgItem( hDlg, ID_IDENTIFY ), FALSE );
      }
      else
      {
         EnableWindow( GetDlgItem( hDlg, ID_VERIFY ), TRUE );
         EnableWindow( GetDlgItem( hDlg, ID_IDENTIFY ), TRUE );
      }

      EnableWindow( GetDlgItem( hDlg, ID_STOPOPERATION ), FALSE );
      EnableWindow( GetDlgItem( hDlg, IDC_CAPTOFILE ), TRUE );
      EnableWindow( GetDlgItem( hDlg, IDC_CAPTOSCREEN ), TRUE );
      EnableWindow( GetDlgItem( hDlg, IDC_DETECT_FAKE ), TRUE );
      EnableWindow( GetDlgItem( hDlg, IDC_FAST_MODE ), TRUE );
      EnableWindow( GetDlgItem( hDlg, IDC_DETECT_MIOT ), TRUE );
      EnableWindow( GetDlgItem( hDlg, IDC_CMAX_FRAMES ), TRUE );
      EnableWindow( GetDlgItem( hDlg, IDC_MEASURE ), TRUE );
      return;
   }

   EnableWindow( GetDlgItem( hDlg, ID_EXIT ), FALSE );
   EnableWindow( GetDlgItem( hDlg, ID_CAPTURE ), FALSE );
   EnableWindow( GetDlgItem( hDlg, ID_VERIFY ), FALSE );
   EnableWindow( GetDlgItem( hDlg, ID_ENROLL ), FALSE );
   EnableWindow( GetDlgItem( hDlg, ID_IDENTIFY ), FALSE );
   EnableWindow( GetDlgItem( hDlg, ID_STOPOPERATION ), TRUE );
   EnableWindow( GetDlgItem( hDlg, IDC_CAPTOFILE ), FALSE );
   EnableWindow( GetDlgItem( hDlg, IDC_CAPTOSCREEN ), FALSE );
   EnableWindow( GetDlgItem( hDlg, IDC_DETECT_FAKE ), FALSE );
   EnableWindow( GetDlgItem( hDlg, IDC_FAST_MODE ), FALSE );
   EnableWindow( GetDlgItem( hDlg, IDC_DETECT_MIOT ), FALSE );
   EnableWindow( GetDlgItem( hDlg, IDC_CMAX_FRAMES ), FALSE );
   EnableWindow( GetDlgItem( hDlg, IDC_MEASURE ), FALSE );
}


void SetVersionCompatibleCtrl( HWND hDWnd, FTR_VERSION Version )
{
    CheckDlgButton( hDWnd, IDC_SDK3_RADIO, BST_UNCHECKED );
    CheckDlgButton( hDWnd, IDC_SDK35_RADIO, BST_UNCHECKED );  
    CheckDlgButton( hDWnd, IDC_SDKBOTH_RADIO, BST_UNCHECKED ); 

    switch( Version )
    {
    case FTR_VERSION_PREVIOUS:
        CheckDlgButton( hDWnd, IDC_SDK3_RADIO, BST_CHECKED );
        break;

    case FTR_VERSION_CURRENT:
        CheckDlgButton( hDWnd, IDC_SDK35_RADIO, BST_CHECKED );  
        break;

    case FTR_VERSION_COMPATIBLE:
        CheckDlgButton( hDWnd, IDC_SDKBOTH_RADIO, BST_CHECKED ); 
        break;
    }


}


FTR_VERSION GetVersionCompatibleCtrl( HWND hDWnd )
{
    FTR_VERSION Result = FTR_VERSION_COMPATIBLE;
    
    if( IsDlgButtonChecked( hDWnd, IDC_SDK3_RADIO ) == BST_CHECKED )
    {
        Result = FTR_VERSION_PREVIOUS;
    }

    if( IsDlgButtonChecked( hDWnd, IDC_SDK35_RADIO ) == BST_CHECKED )
    {
        Result = FTR_VERSION_CURRENT;
    }

    if( IsDlgButtonChecked( hDWnd, IDC_SDKBOTH_RADIO ) == BST_CHECKED )
    {
        Result = FTR_VERSION_COMPATIBLE;
    }

    return Result;
}

