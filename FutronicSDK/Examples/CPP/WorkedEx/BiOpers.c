
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * BiOpers.c  - call SDK biometric operations.
  *
  */

#include "WorkedEx.h"
#include <Process.h>


 /******************************************************************************
  *
  * Data types, constants and macros.
  *
  */
 // operation parameters
typedef struct
{
   int      ioType;                    // input/output device. Depend on operatin
   int      nTimes;                    // how many operation repete
   int      nCurr;                     // current cycle
   char     fMask[_MAX_FNAME];         // file mask for work with file input/output
} BIOPERPARAMS, * LPBIOPERPARAMS;

 // callback function contex
typedef struct
{
   int      oType;                     // current biometric operation
   HWND     hPrgWnd;                   // progress bar window handler. Use with
                                       // enroll operation
   HWND     hTextWnd;                  // text window handler. Use with enroll
                                       // operation
} BIOPERCONTEXT, * LPBIOPERCONTEXT;

 // biometric operation types
#define     BO_CAPTURE        0        // capture operation
#define     BO_ENROLL         1        // enroll --"--
#define     BO_VERIFY         2        // verify --"--
#define     BO_IDENTIFY       3        // identify --"--


 /******************************************************************************
  *
  * Global variables.
  *
  */
BIOPERPARAMS   bop;
 // use for cancel current biometric operation
BOOL           bCancelOperation  = FALSE;
// int            iDebugCount       = 0;


 /******************************************************************************
  *
  * Functions prototypes.
  *
  */

 /*
  * CaptureThread - capture operation thread.
  */
void CaptureThread( void *param );

 /*
  * WriteToFileDlgProc  - write file parameters dialog pocedure.
  */
INT_PTR CALLBACK WriteToFileDlgProc( HWND  hDWnd, UINT  uMsg, WPARAM wParam, LPARAM lParam );

 /*
  * EnrollThread - enroll operation thread.
  */
void EnrollThread( void *param );

 /*
  * EnrollDlgProc - set user name for write template.
  */
INT_PTR CALLBACK EnrollDlgProc( HWND hDWnd, UINT uMsg, WPARAM wParam, LPARAM lParam );

 /*
  * VerifyThread - verify operation thread.
  */
void VerifyThread( void *param );

 /*
  * VerifyDlgProc - select file template for verify.
  */
INT_PTR CALLBACK VerifyDlgProc( HWND hDWnd, UINT uMsg, WPARAM wParam, LPARAM lParam );

 /*
  * IdentifyThread - identify operation thread.
  */
void IdentifyThread( void *param );


 /******************************************************************************
  *
  * cbControl  - user's callback function for control the enrollment or
  *              verification execution flow.
  * Syntax:
  *   void cbControl( FTR_USER_CTX Context, FTR_STATE StateMask,
  *      FTR_RESPONSE *pResponse, FTR_SIGNAL Signal, FTR_BITMAP_PTR pBitmap );
  * Argument list:
  *   Context (input)      - user-defined context information;
  *   StateMask (input)    - a bit mask indicating what arguments are provided;
  *   pResponse (output)   - API function execution control is achieved through
  *                          this value;
  *   Signal (input)       - this signal should be used to interact with a user;
  *   pBitmap (input)      - a pointer to the bitmap to be displayed.
  *
  */
void FTR_CBAPI cbControl( FTR_USER_CTX Context, FTR_STATE StateMask, FTR_RESPONSE *pResponse,
                          FTR_SIGNAL Signal, FTR_BITMAP_PTR pBitmap )
{
   LPBIOPERCONTEXT   lpboc;            // biometric operation context
   FTR_PROGRESS      *lpPrgData;       // current progress data
   char              prgTitle[64];     // progress window text

   lpboc = (LPBIOPERCONTEXT)Context;
   lpPrgData = (FTR_PROGRESS *)pResponse;

   // frame show
   if( (StateMask & FTR_STATE_FRAME_PROVIDED) )
   {
      UpdateImage( (void *)pBitmap->Bitmap.pData );
      // WriteBMPFile( (void *)pBitmap->Bitmap.pData, dbs.dbImages, "Debug", iDebugCount++ );
   }

   // message print
   if( (StateMask & FTR_STATE_SIGNAL_PROVIDED) )
   {
      switch( Signal )
      {
      case FTR_SIGNAL_TOUCH_SENSOR:
         if( (lpboc->oType == BO_ENROLL) && (lpPrgData->dwCount == 1) &&
             (lpPrgData->bIsRepeated == FALSE) )
         {
            // setup progress bar
            SendMessage( lpboc->hPrgWnd, PBM_SETRANGE, 0, MAKELPARAM( 0, lpPrgData->dwTotal ) );
            SendMessage( lpboc->hPrgWnd, PBM_SETSTEP, (WPARAM)1, 0);
            SendMessage( lpboc->hPrgWnd, PBM_SETPOS, (WPARAM)0, 0);
            SetWindowText( lpboc->hTextWnd, "" );

            // show progress bar
            ShowWindow( lpboc->hPrgWnd, SW_SHOW );
            ShowWindow( lpboc->hTextWnd, SW_SHOW );
         }
         PrintTextMsg( "Put your finger on the scaner" );
         break;

      case FTR_SIGNAL_TAKE_OFF:
         if( lpboc->oType == BO_ENROLL )
         {
            // update progress
            SendMessage( lpboc->hPrgWnd, PBM_SETPOS, (WPARAM)lpPrgData->dwCount, 0);
            sprintf( prgTitle, "     %d of %d", lpPrgData->dwCount, lpPrgData->dwTotal );
            SetWindowText( lpboc->hTextWnd, prgTitle );
         }
         PrintTextMsg( "Take off your finger from the scaner" );
         break;

      case FTR_SIGNAL_FAKE_SOURCE:
         {
            int   iRet;
            iRet = MessageBox( hMainWnd, "Fake finger detected.\nContinue process?",
               "!!! Attention !!!", MB_YESNO | MB_ICONQUESTION );
            if( iRet == IDYES )
               *pResponse = FTR_CONTINUE;
            else
               *pResponse = FTR_CANCEL;
            return;
         }
      case FTR_SIGNAL_UNDEFINED:
         PrintTextMsg( "Baida signal value" );
         break;
      }                                // switch( Signal )
   }
   if( bCancelOperation == FALSE )
      *pResponse = FTR_CONTINUE;
   else
      *pResponse = FTR_CANCEL;
}


 /******************************************************************************
  *
  * CaptureStarter   - start capture operation thread.
  * Syntax:
  *   BOOL CaptureStarter( int tDest );
  * Argument list:
  *   tDest - if CAPTURE_TO_FILE, fingerprint image will be stored to file & viewed
  *           on screen;
  *           if CAPTURE_TO_SCREEN, fingerprint image will be viewed on screen only.
  * Return value:
  *   TRUE  - operatin started successfully;
  *   FALSE - error starting operation.
  *
  */
BOOL CaptureStarter( int tDest )
{
   INT_PTR        ipRet;
   uintptr_t ulRet;               // return value

   // prepare operation parameters
   ZeroMemory( (LPVOID)&bop, sizeof( BIOPERPARAMS ) );
   bop.ioType = tDest;
   if( bop.ioType == CAPTURE_TO_SCREEN )
      bop.nTimes  = 1;
   else
   {
      ipRet = DialogBox( hMainInst, MAKEINTRESOURCE( IDD_CAPTOFILE ), hMainWnd,
         (DLGPROC)WriteToFileDlgProc );
      if( ipRet == 0 )
         return FALSE;
   }

   // start capture thread
   ulRet = _beginthread( CaptureThread, 0, (void *)&bop );
   if( (ulRet == -1) || (ulRet == 0) )
      return FALSE;
   return TRUE;
}


 /******************************************************************************
  *
  * CaptureThread - capture operation thread.
  *
  */
void CaptureThread( void *param )
{
   FTRAPI_RESULT  rCode;
   LPBIOPERPARAMS lpBOP = (LPBIOPERPARAMS)param;
   int            ImageSize;
   void           *lpImageBytes;
   BOOL           bRet;
   BIOPERCONTEXT  boc;

   // select memory
   rCode = FTRGetParam( FTR_PARAM_IMAGE_SIZE, (FTR_PARAM_VALUE)&ImageSize );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto CaptureThreadExit;
   }
   lpImageBytes = malloc( (size_t)ImageSize );
   if( lpImageBytes == NULL )
   {
      PrintTextMsg( "CaptureThread: no memory" );
      goto CaptureThreadExit;
   }

   // prepare callback context
   boc.oType   = BO_CAPTURE;

   // capture frame
   for( lpBOP->nCurr = 0; lpBOP->nCurr < lpBOP->nTimes; lpBOP->nCurr++ )
   {
      rCode = FTRCaptureFrame( (FTR_USER_CTX)&boc, lpImageBytes );
      if( rCode != FTR_RETCODE_OK )
      {
         wePrintError( rCode, 1 );
         goto CaptureThreadExit1;
      }
      if( lpBOP->ioType == CAPTURE_TO_FILE )
      {
         bRet = WriteBMPFile( lpImageBytes, dbs.dbImages, lpBOP->fMask, lpBOP->nCurr );
         if( !bRet )
         {
            PrintTextMsg( "File(s) not write" );
            goto CaptureThreadExit1;
         }
      }
      if( bCancelOperation )
      {
         PrintTextMsg( "Canceled by user" );
         goto CaptureThreadExit1;
      }
      Sleep( 1000 );
   }

   // finish operation
CaptureThreadExit1:
   free( lpImageBytes );
CaptureThreadExit:
   UpdateControl( TRUE );
   _endthread( );
}


 /******************************************************************************
  *
  * WriteToFileDlgProc  - write file parameters dialog pocedure.
  *
  */
INT_PTR CALLBACK WriteToFileDlgProc( HWND hDWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch( uMsg )
   {
   case WM_INITDIALOG:
      SetDlgItemText( hDWnd, IDC_FILENUM, "1" );
      return TRUE;

   case WM_COMMAND:
      {
         switch( LOWORD( wParam ) )
         {
         case IDOK:
            {
               char  fName[_MAX_FNAME];
               char  fNum[6];

               GetDlgItemText( hDWnd, IDC_FILEMASK, fName, _MAX_FNAME );
               if( (strpbrk( fName, "\\?|><:/*\"" ) != NULL) ||
                   (strcmp( fName, "" ) == 0) )
               {
                  MessageBox( hDWnd, "Wrong file name", "Error", MB_OK | MB_ICONSTOP );
                  break;
               }
               GetDlgItemText( hDWnd, IDC_FILENUM, fNum, 6 );
               strcpy( bop.fMask, fName );
               bop.nTimes = atoi( fNum );
               EndDialog( hDWnd, 1 );
               break;
            }

         case IDCANCEL:
            EndDialog( hDWnd, 0 );
            break;
         }                             // switch( LOWORD( wParam ) )
         break;
      }                                // case WM_COMMAND:

   }                                   // switch( uMsg )
   return FALSE;
}


 /******************************************************************************
  *
  * EnrollStarter - start enroll operation thread.
  * Syntax:
  *   BOOL EnrollStarter( int tSrc );
  * Argument list:
  *   tSrc  - if CAPTURE_FROM_FILE, fingerprint images will be reading from file
  *           for enroll operation;
  *           if CAPTURE_FROM_SCANER, fingerprint images will reading from scaner
  *           for enroll operation.
  * Return value:
  *   TRUE  - operatin started successfully;
  *   FALSE - error starting operation.
  *
  */
BOOL EnrollStarter( int tSrc )
{
   INT_PTR ipRet;
   uintptr_t ulRet;

   // prepare operation parameters
   ZeroMemory( (LPVOID)&bop, sizeof( BIOPERPARAMS ) );
   bop.ioType = tSrc;
   ipRet = DialogBox( hMainInst, MAKEINTRESOURCE( IDD_YOURNAME ), hMainWnd,
      (DLGPROC)EnrollDlgProc );
   if( ipRet == 0 )
         return FALSE;

   // start enroll thread
   ulRet = _beginthread( EnrollThread, 0, (void *)&bop );
   if( (ulRet == -1) || (ulRet == 0) )
      return FALSE;
   return TRUE;
}


 /******************************************************************************
  *
  * EnrollThread - enroll operation thread.
  *
  */
void EnrollThread( void *param )
{
   FTRAPI_RESULT     rCode;
   LPBIOPERPARAMS    lpBOP = (LPBIOPERPARAMS)param;
   int               TemplateSize;
   void              *lpTemplateBytes;
   FTR_DATA          Template;
   FTR_ENROLL_DATA   eData;
   BOOL              bRet;
   char              msg[_MAX_FNAME];
   BIOPERCONTEXT     boc;

   // select memory
   rCode = FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE, (FTR_PARAM_VALUE)&TemplateSize );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto EnrollThreadExit;
   }
   lpTemplateBytes = malloc( (size_t)TemplateSize );
   if( lpTemplateBytes == NULL )
   {
      PrintTextMsg( "EnrollThread: no memory" );
      goto EnrollThreadExit;
   }

   // prepare arguments
   Template.dwSize = TemplateSize;
   Template.pData = lpTemplateBytes;
   eData.dwSize   = sizeof( FTR_ENROLL_DATA );

   // prepare callback context
   boc.oType      = BO_ENROLL;
   boc.hPrgWnd    = GetDlgItem( hDlg, IDC_ENROLL_PROGRESS );
   boc.hTextWnd   = GetDlgItem( hDlg, IDC_PROGRESS_TEXT );

   // enroll operation
   rCode = FTREnrollX( (FTR_USER_CTX)&boc, FTR_PURPOSE_ENROLL, &Template, &eData );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto EnrollThreadExit1;
   }

   // write template
   bRet = AddRecord( lpBOP->fMask, Template.pData, Template.dwSize );
   strcpy( msg, lpBOP->fMask );
   if( bRet )
   {
      char  cQuality[64];
      strcat( msg, " successfully stored. Quality is " );
      sprintf( cQuality, "%d of 10", eData.dwQuality );
      strcat( msg, cQuality );
   }
   else
      strcat( msg, " not stored" );
   PrintTextMsg( msg );

   // finish operation
EnrollThreadExit1:
   ShowWindow( boc.hPrgWnd, SW_HIDE );
   ShowWindow( boc.hTextWnd, SW_HIDE );
   free( lpTemplateBytes );
EnrollThreadExit:
   UpdateControl( TRUE );
   _endthread( );
}


 /******************************************************************************
  *
  * EnrollDlgProc - set user name for write template.
  *
  */
INT_PTR CALLBACK EnrollDlgProc( HWND hDWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch( uMsg )
   {

   case WM_COMMAND:
      {
         switch( LOWORD( wParam ) )
         {
         case IDOK:
            {
               GetDlgItemText( hDWnd, IDC_YOURNAME, bop.fMask, _MAX_FNAME );
               if( (strpbrk( bop.fMask, "\\?|><:/*\"" ) != NULL) ||
                   (strcmp( bop.fMask, "" ) == 0) )
               {
                  MessageBox( hDWnd, "Wrong name", "Error", MB_OK | MB_ICONSTOP );
                  break;
               }
               EndDialog( hDWnd, 1 );
               break;
            }

         case IDCANCEL:
            EndDialog( hDWnd, 0 );
            break;
         }                             // switch( LOWORD( wParam ) )
         break;
      }                                // case WM_COMMAND:

   }                                   // switch( uMsg )
   return FALSE;
}


 /******************************************************************************
  *
  * VerifyStarter - start verify operation thread.
  * Syntax:
  *   BOOL VerifyStarter( int tSrc );
  * Argument list:
  *   tSrc  - if CAPTURE_FROM_FILE, fingerprint images will be reading from file
  *           for verify operation;
  *           if CAPTURE_FROM_SCANER, fingerprint images will reading from scaner
  *           for verify operation.
  * Return value:
  *   TRUE  - operatin started successfully;
  *   FALSE - error starting operation.
  *
  */
BOOL VerifyStarter( int tSrc )
{
   INT_PTR ipRet;
   uintptr_t ulRet;

   // prepare operation parameters
   ZeroMemory( (LPVOID)&bop, sizeof( BIOPERPARAMS ) );
   bop.ioType = tSrc;
   ipRet = DialogBox( hMainInst, MAKEINTRESOURCE( IDD_VERIFYDIALOG ), hMainWnd,
      (DLGPROC)VerifyDlgProc );
   if( ipRet == 0 )
         return FALSE;

   // start verify thread
   ulRet = _beginthread( VerifyThread, 0, (void *)&bop );
   if( (ulRet == -1) || (ulRet == 0) )
      return FALSE;
   return TRUE;
}


 /******************************************************************************
  *
  * VerifyThread - verify operation thread.
  *
  */
void VerifyThread( void *param )
{
   FTRAPI_RESULT  rCode;
   LPBIOPERPARAMS lpBOP = (LPBIOPERPARAMS)param;
   LPDBREC        lpRec;               // template from DB
   BOOL           vResult;
   // FAR or FARN choise
   //FTR_FAR        vFAR;
   FTR_FARN       vFARN;
   BIOPERCONTEXT  boc;

   // read template from DB
   lpRec = GetRecord( lpBOP->fMask );
   if( lpRec == NULL )
   {
      PrintTextMsg( "DB reading error" );
      goto VerifyThreadExit;
   }

   // prepare callback context
   boc.oType   = BO_VERIFY;

   // call verify operation
   // FAR or FARN choise
   //rCode = FTRVerify( (FTR_USER_CTX)&boc, (FTR_DATA_PTR)&lpRec->len, &vResult, &vFAR );
   rCode = FTRVerifyN( (FTR_USER_CTX)&boc, (FTR_DATA_PTR)&lpRec->len, &vResult, &vFARN );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto VerifyThreadExit1;
   }

   // print result
   if( vResult )
   {
      char  msg[_MAX_FNAME];
      // FAR or FARN choise
      //sprintf( msg, "Verification is successful (%s)", CalcMfromP( (long)vFAR ) );
      sprintf( msg, "Verification is successful (%d)", vFARN );
      PrintTextMsg( msg );
   }
   else
      PrintTextMsg( "Verification is wrong" );

   // finish operation
VerifyThreadExit1:
   FreeRecordMem( lpRec );
VerifyThreadExit:
   UpdateControl( TRUE );
   _endthread( );
}


 /******************************************************************************
  *
  * VerifyDlgProc - select file template for verify.
  *
  */
INT_PTR CALLBACK VerifyDlgProc( HWND hDWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{
   switch( uMsg )
   {
   case WM_INITDIALOG:
      {
         HANDLE            hFind;
         WIN32_FIND_DATA   ff;
         char              fMask[_MAX_FNAME];
         BOOL              bRet = TRUE;
         HWND              hListWnd;
         char              Item[_MAX_FNAME];

         // fill filename listbox
         hListWnd = GetDlgItem( hDWnd, IDC_VERIFYLIST );
         strcat( strcpy( fMask, dbs.dbTemplates ), "\\*.tml" );
         hFind = FindFirstFile( fMask, &ff );
         while( bRet )
         {
            strcpy( Item, ff.cFileName );
            *strrchr( Item, '.' ) = '\000';
            SendMessage( hListWnd, LB_ADDSTRING, 0, (LPARAM)Item );
            bRet = FindNextFile( hFind, &ff );
         }
         FindClose( hFind );

         return TRUE;
      }                                // case WM_INITDIALOG:

   case WM_COMMAND:
      {
         if( (HIWORD( wParam ) == LBN_SELCHANGE) ||
             (HIWORD( wParam ) == LBN_DBLCLK) )
         {
            LRESULT  lRet;
            HWND     hListWnd;
            char     ItemText[_MAX_FNAME];

            hListWnd = GetDlgItem( hDWnd, IDC_VERIFYLIST );
            lRet = SendMessage( hListWnd, LB_GETCURSEL, 0, 0L );
            SendMessage( hListWnd, LB_GETTEXT, (WPARAM)lRet, (LPARAM)ItemText );
            SetDlgItemText( hDWnd, IDC_MYNAME, ItemText );
         }
         if( HIWORD( wParam ) == LBN_DBLCLK )
            PostMessage( hDWnd, WM_COMMAND, IDOK, 0L );

         switch( LOWORD( wParam ) )
         {
         case IDOK:
            {
               LRESULT  lRet;

               GetDlgItemText( hDWnd, IDC_MYNAME, bop.fMask, _MAX_FNAME );
               if( strpbrk( bop.fMask, "\\?|><:/*\"" ) != NULL )
               {
                  MessageBox( hDWnd, "Wrong name", "Error", MB_OK | MB_ICONSTOP );
                  break;
               }
               lRet = SendMessage( GetDlgItem( hDWnd, IDC_VERIFYLIST ), LB_FINDSTRINGEXACT, -1,
                  (LPARAM)bop.fMask );
               if( lRet == LB_ERR )
               {
                  MessageBox( hDWnd, "Wrong name", "Error", MB_OK | MB_ICONSTOP );
                  break;
               }
               EndDialog( hDWnd, 1 );
               break;
            }

         case IDCANCEL:
            EndDialog( hDWnd, 0 );
            break;
         }                             // switch( LOWORD( wParam ) )
         break;
      }                                // case WM_COMMAND:

   }                                   // switch( uMsg )
   return FALSE;
}


 /******************************************************************************
  *
  * IdentifyStarter - start identify operation thread.
  * Syntax:
  *   BOOL IdentifyStarter( int tSrc );
  * Argument list:
  *   tSrc  - if CAPTURE_FROM_FILE, fingerprint images will be reading from file
  *           for identify operation;
  *           if CAPTURE_FROM_SCANER, fingerprint images will reading from scaner
  *           for identify operation.
  * Return value:
  *   TRUE  - operatin started successfully;
  *   FALSE - error starting operation.
  *
  */
BOOL IdentifyStarter( int tSrc )
{
   uintptr_t ulRet;

   // prepare operation parameters
   ZeroMemory( (LPVOID)&bop, sizeof( BIOPERPARAMS ) );
   bop.ioType = tSrc;

   // start identify thread
   ulRet = _beginthread( IdentifyThread, 0, (void *)&bop );
   if( (ulRet == -1) || (ulRet == 0) )
      return FALSE;
   return TRUE;
}


 /******************************************************************************
  *
  * IdentifyThread - identify operation thread.
  *
  */
void IdentifyThread( void *param )
{
   FTRAPI_RESULT        rCode;
   FTR_DATA             Sample;        // template for identify
   WIN32_FIND_DATA      ff;
   HANDLE               hFind;
   char                 fMask[_MAX_FNAME];
   BOOL                 bRet  = TRUE;
   FTR_IDENTIFY_ARRAY   fromDB;
   FTR_IDENTIFY_RECORD  idRec;
   // FAR or FARN choise
   //FTR_MATCHED_ARRAY    resArr;
   //FTR_MATCHED_RECORD   resRec;
   FTR_MATCHED_X_ARRAY  resArrX;
   FTR_MATCHED_X_RECORD resRecX;
   LPDBREC              lpDBRec  = NULL;
   DWORD                resNum = 0;
   BIOPERCONTEXT  boc;

   // prepare callback context
   boc.oType   = BO_IDENTIFY;

   // create template for identify
   // 1. allocate memory
   rCode = FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE, (FTR_PARAM_VALUE)&Sample.dwSize );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto IdentifyThreadExit;
   }
   if( (Sample.pData = malloc( (size_t)Sample.dwSize )) == NULL )
   {
      PrintTextMsg( "IdentifyThread: no memory" );
      goto IdentifyThreadExit;
   }
   // 2. enroll operation
   rCode = FTREnroll( (FTR_USER_CTX)&boc, FTR_PURPOSE_IDENTIFY, &Sample );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto IdentifyThreadExit1;
   }

   // prepare arguments for identify
   // 1. set base template
   rCode = FTRSetBaseTemplate( &Sample );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 1 );
      goto IdentifyThreadExit1;
   }
   // 2. initialize array of identify records
   fromDB.TotalNumber   = 1;
   fromDB.pMembers      = &idRec;
   // 3. initialize result array
   // FAR or FARN choise
   //resArr.TotalNumber   = 1;
   //resArr.pMembers      = &resRec;
   resArrX.TotalNumber   = 1;
   resArrX.pMembers      = &resRecX;

   // prepare exhaustive search in DB
   strcat( strcpy( fMask, dbs.dbTemplates ), "\\*.tml" );
   hFind = FindFirstFile( fMask, &ff );
   if( hFind == INVALID_HANDLE_VALUE )
   {
      PrintTextMsg( "Database is empty" );
      goto IdentifyThreadExit1;
   }

   // identify cycle
   while( bRet )
   {
      // read record from DB
      *strrchr( ff.cFileName, '.' ) = '\000';
      lpDBRec = GetRecord( ff.cFileName );
      if( lpDBRec == NULL )
      {
         char  msg[_MAX_FNAME];
         strcat( strcpy( msg, "Can't read from DB " ), ff.cFileName );
         PrintTextMsg( msg );
         goto IdentifyThreadExit2;
      }

      // translate DB record to identify record
      strcpy( idRec.KeyValue, lpDBRec->key );
      idRec.pData = (FTR_DATA_PTR)&lpDBRec->len;

      // call identify
      // FAR or FARN choise
      //rCode = FTRIdentify( &fromDB, &resNum, &resArr );
      rCode = FTRIdentifyN( &fromDB, &resNum, &resArrX );
      if( rCode != FTR_RETCODE_OK )
      {
         wePrintError( rCode, 1 );
         goto IdentifyThreadExit3;
      }

      // search finish
      if( resNum )
      {
         char  msg[_MAX_FNAME];

         // FAR or FARN choise
         //sprintf( msg, "You are %s (%s)", resArr.pMembers[0].KeyValue, CalcMfromP( resArr.pMembers[0].FarAttained ) );
         sprintf( msg, "You are %s (%d)", resArrX.pMembers[0].KeyValue, resArrX.pMembers[0].FarAttained.N );
         PrintTextMsg( msg );
         goto IdentifyThreadExit3;
      }

      // search continue
      FreeRecordMem( lpDBRec );
      lpDBRec = NULL;
      bRet = FindNextFile( hFind, &ff );

      if( bCancelOperation )
      {
         PrintTextMsg( "Canceled by user" );
         goto IdentifyThreadExit3;
      }
   }

   // not found
   PrintTextMsg( "You are not found" );

   // finish operation
IdentifyThreadExit3:
   if( lpDBRec )
      FreeRecordMem( lpDBRec );
IdentifyThreadExit2:
   FindClose( hFind );
IdentifyThreadExit1:
   free( Sample.pData );
IdentifyThreadExit:
   UpdateControl( TRUE );
   _endthread( );
}


 /******************************************************************************
  *
  * CalcPfromM - calculate probability value for mesure.
  * Syntax:
  *   long CalcPfromM( char *meas );
  * Argument list:
  *   meas  - source mesure value.
  * Return value:
  *   -1 -  bad meas parameter;
  *      -  probability value from M2PArray.
  *
  */
long CalcPfromM( char *meas )
{
   int   iCyc;

   for( iCyc = 0; iCyc < M2PNUM; iCyc++ )
      if( strcmp( meas, M2PArray[iCyc].meas ) == 0 )
         return M2PArray[iCyc].prob;

   return -1;
}


 /******************************************************************************
  *
  * CalcMfromP - calculate mesure value for probability.
  * Syntax:
  *   char *CalcMfromP( long prob );
  * Argument list:
  *   prob  - source probability value.
  * Return value:
  *   NULL  -  bad prob parameter;
  *         -  mesure value from M2PArray.
  *
  */
char *CalcMfromP( long prob )
{
   int   iCyc;

   for( iCyc = 0; iCyc < M2PNUM-1; iCyc++ )
      if( M2PArray[iCyc].prob <= prob )
         break;
   return M2PArray[iCyc].meas;
}
