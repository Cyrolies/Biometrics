
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * MainWnd.c  - application main window.
  *
  */

#include "WorkedEx.h"


 /******************************************************************************
  *
  * Global variables.
  *
  */
// Main window class name
char const szClassName[] = "WExClass";
// Main window title
char const szWinName[] = "VC Example for Futronic SDK v.4.2";

HWND           hMainWnd;               // main window handler
HINSTANCE      hMainInst;              // application instance
HWND           hDlg;                   // dialog window handler
HWND           hShowWnd;               // view fingerprint window handler

// FAR <--> Measure array,  FAR probability comment
// See commentary for according FAR value
M2PREC M2PArray[] =
{
   { 738151462,   "  1" },    // 0,343728560
   { 638070147,   " 16" },    // 0,297124566
   { 427995129,   " 31" },    // 0,199300763
   { 206984582,   " 49" },    // 0,096384707
   { 104396832,   " 63" },    // 0,048613563
   {  20854379,   " 95" },    // 0,009711077
   {  10620511,   "107" },    // 0,004945561
   {   2066214,   "130" },    // 0,000962156
   {   1002950,   "136" },    // 0,000467035
   {    207859,   "155" },    // 0,000096792
   {    103930,   "166" },    // 0,000048396
   {     21002,   "190" },    // 0,000009780
   {      9694,   "199" },    // 0,000004514
   {      1885,   "221" },    // 0,000000878
   {       807,   "230" },    // 0,000000376
   {       256,   "245" },    // 0,000000119209 (0/129)
   {       128,   "265" },    // 0,000000059605 (0/153)
   {        64,   "286" },    // 0,000000029802 (0/174)
   {        32,   "305" },    // 0,000000014901 (0/205)
   {        16,   "325" },    // 0,000000007451 (0/231)
   {         8,   "345" },    // 0,000000003725 (0/294)
   {         4,   "365" },    // 0,000000001863 (0/362)
   {         2,   "385" },    // 0,000000000931 (0/439)
   {         1,   "405" },    // 0,000000000466 (0/542)
   {         0,   "Max" }     // Maximum possible measure value should be placed here.
};


 /******************************************************************************
  *
  * Functions prototypes.
  *
  */

 /*
  * weReg   - register necessary window classes.
  * Syntax:
  *   BOOL weReg( HINSTANCE hInst );
  * Argument list:
  *   hInst - application instance.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error while register classes.
  */
BOOL weReg( HINSTANCE hInst );

 /*
  * MainWndProc - main window's function
  */
__declspec( dllexport ) LRESULT WINAPI MainWndProc( HWND hWnd, UINT msg,
                                                    WPARAM wParam, LPARAM lParam );

 /*
  * GetFTRSDKVersion - get Futronic SDK version. This is FRTAPI.dll version.
  * Syntax:
  *   void GetFTRSDKVersion( char *verinfo, int buflen );
  * Argument list:
  *   verinfo (input/output)  - text buffer;
  *   buflen (input)          - text buffer size.
  */
void GetFTRSDKVersion( char *verinfo, int buflen );



 /******************************************************************************
  *
  * WinMain function.
  *
  */
int PASCAL WinMain( HINSTANCE hInstance, HINSTANCE hPrevInstance, 
                    LPSTR lpszCmdLine, int nCmdShow )
{
   int      wScr, hScr;                // screen width and height
   MSG        msg;                       // message structure
   FTRAPI_RESULT  rCode;               // SDK return code
   HANDLE   hAppRun;

   // check uniqueness WorkedEx copy
   hAppRun = CreateMutex( NULL, TRUE, "WorkedExMutex" );
   if( GetLastError( ) == ERROR_ALREADY_EXISTS )
   {
      SetActiveWindow( FindWindow( szClassName, szWinName ) );
      return 0;
   }

   // register necessary window classes
   InitCommonControls( );
   if( weReg( hInstance ) == FALSE )
   {
      MessageBox( NULL, "Can't register class",
         "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
      if( hAppRun ) CloseHandle( hAppRun );
      return 0;
   }

   hMainInst = hInstance;

   // get screen width and height
   wScr = GetSystemMetrics( SM_CXSCREEN );
   hScr = GetSystemMetrics( SM_CYSCREEN );

   // initialize SDK
   rCode = FTRInitialize( );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 0 );
      if( hAppRun ) CloseHandle( hAppRun );
      return FALSE;
   }

   // initialize DB
   dbInit( );

   // create main window
   hMainWnd = CreateWindow(
      szClassName,                     // class name
      szWinName,                       // window title
      WS_OVERLAPPED | WS_SYSMENU | WS_MINIMIZEBOX | WS_DLGFRAME,  // window style
      (wScr - 800) / 2,                // x-coordinate
      (hScr - 600) / 2,                // y-coordinate
      800,                             // width
      600,                             // height
      0,                               // parent window handle
      NULL,                            // menu handle
      hInstance,                       // application instance
      NULL );                          // pointer for additional parameters
   // creation error
   if (!hMainWnd)
   {
      MessageBox( NULL, "Can't create Main Window",
         "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
      if( hAppRun ) CloseHandle( hAppRun );
      return FALSE;
   }

   // show window
   ShowWindow( hMainWnd, nCmdShow );
   UpdateWindow( hMainWnd );

   // start message cycle processing
   while (GetMessage( &msg, 0, 0, 0 ))
   {
      if( !IsDialogMessage( hDlg, &msg ) )
      {
         TranslateMessage( &msg );
         DispatchMessage( &msg );
      }
   }

   // releases allocated resources for SDK
   FTRTerminate( );

   if( hAppRun ) CloseHandle( hAppRun );

   return (int)msg.wParam;
}


 /******************************************************************************
  *
  * weReg   - register necessary window classes.
  * Syntax:
  *   BOOL weReg( HINSTANCE hInst );
  * Argument list:
  *   hInst - application instance.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error while register classes.
  *
  */
BOOL weReg( HINSTANCE hInst )
{
   ATOM     aWndClass;                 // return code
   WNDCLASS    wc;                        // structure for register window class

   memset( &wc, 0, sizeof( wc ) );

   wc.style          = 0;
   wc.lpfnWndProc    = (WNDPROC)MainWndProc;
   wc.cbClsExtra     = 0;
   wc.cbWndExtra     = 0;
   wc.hInstance      = hInst;
   wc.hIcon          = LoadIcon( hInst, MAKEINTRESOURCE( IDI_FUTRONIC ) );
   wc.hCursor        = LoadCursor( NULL, IDC_ARROW );
   wc.hbrBackground    = (HBRUSH)(COLOR_WINDOWTEXT + 1);
   wc.lpszMenuName    = NULL;
   wc.lpszClassName    = (LPSTR)szClassName;

   aWndClass = RegisterClass( &wc );
   if (aWndClass == 0) return FALSE;
   return weRegShowWnd( hInst );
}


 /******************************************************************************
  *
  * MainWndProc - main window's function
  *
  */
__declspec( dllexport ) LRESULT WINAPI MainWndProc( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
   switch( msg )
   {

   case WM_CREATE:
      {
         RECT  rDlg;                   // dialog rectangle
         RECT  rShow;                  // show window rectangle
         int   hElems;                 // max height child windows
         int   hMain, wMain;           // width & height of main window
         int   wScr, hScr;             // screen width and height
         DWORD value;
         FTRAPI_RESULT  rCode;         // SDK return code
         BOOL  bValue;
         char  verinfo[32];            // SDK version information
         char  vertext[256];

         // default SDK settings:
         // 1. frame source
         value = FSD_FUTRONIC_USB;
         rCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, (FTR_PARAM_VALUE)value );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // 2. user's calbacks
         rCode = FTRSetParam( FTR_PARAM_CB_CONTROL, (FTR_PARAM_VALUE)cbControl );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // You may use FAR or FARN options on your own choice
         // 3. FAR setting
         /*
         value = M2PArray[M2PDEFITEM].prob;
         rCode = FTRSetParam( FTR_PARAM_MAX_FAR_REQUESTED, (FTR_PARAM_VALUE)value );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }
         */
         // 3. FARN setting
         value = atol( M2PArray[M2PDEFITEM].meas );
         rCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, (FTR_PARAM_VALUE)value );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // 4. fake mode setting
         bValue = FALSE;
         rCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, (FTR_PARAM_VALUE)bValue );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // 5. application takes a control over the Fake Finger Detection (FFD) event
         bValue = TRUE;
         rCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, (FTR_PARAM_VALUE)bValue );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // 6. MIOT mode setting
         bValue = FALSE;
         rCode = FTRSetParam( FTR_PARAM_MIOT_CONTROL, (FTR_PARAM_VALUE)bValue );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // 7. Fast Mode setting
         bValue = FALSE;
         rCode = FTRSetParam( FTR_PARAM_FAST_MODE, (FTR_PARAM_VALUE)bValue );
         if( rCode != FTR_RETCODE_OK )
         {
            wePrintError( rCode, 0 );
            return -1;
         }

         // create control dialog
         hDlg = CreateDialog( hMainInst, MAKEINTRESOURCE( IDD_MAINDIALOG ), hWnd,
            (DLGPROC)MainDlgProc );
         // define control buttons state
         if( EmptyDB( ) )
         {
            EnableWindow( GetDlgItem( hDlg, ID_VERIFY ), FALSE );
            EnableWindow( GetDlgItem( hDlg, ID_IDENTIFY ), FALSE );
         }
         GetWindowRect( hDlg, &rDlg );
         // get SDK version information
         ZeroMemory( (LPVOID)verinfo, 32 );
         GetFTRSDKVersion( verinfo, 31 );
         strcpy( vertext, "FTRAPI.dll version " );
         strcat( vertext, verinfo );
         SetDlgItemText( hDlg, IDC_VERINFO, vertext );
         // create show window
         hShowWnd = CreateShowWnd( hMainInst, hWnd, RECTW( rDlg ), 0 );
         if( !hShowWnd )
         {
            MessageBox( hWnd, "Can't create Show Window",
               "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
            return -1;
         }

         // resize windows
         GetWindowRect( hShowWnd, &rShow );
         hElems = max( RECTH( rDlg ), RECTH( rShow ) );
         if( RECTH( rDlg ) < hElems )
            MoveWindow( hDlg, 0, 0, RECTW( rDlg ), hElems, TRUE );
         hMain = hElems + GetSystemMetrics( SM_CYCAPTION ) +
            2*GetSystemMetrics( SM_CYDLGFRAME );
         wMain = RECTW( rDlg ) + RECTW( rShow ) +2*GetSystemMetrics( SM_CXDLGFRAME );
         wScr = GetSystemMetrics( SM_CXSCREEN );
         hScr = GetSystemMetrics( SM_CYSCREEN );
         MoveWindow( hWnd, (wScr - wMain) / 2, (hScr - hMain) / 2,
            wMain, hMain, TRUE );
      
         return 0;
      }                                // case WM_CREATE:

   case WM_CLOSE:
      {
         if( !IsWindowEnabled( GetDlgItem( hDlg, ID_EXIT ) ) )
         {
            return TRUE;
         }
      }                                // case WM_CLOSE:

   case WM_COMMAND:
      {
         switch( LOWORD( wParam ) )
         {
         case IDM_EXIT:
            DestroyWindow( hWnd );
            break;
         }                             // switch( LOWORD( wParam ) )

         break;
      }                                // case WM_COMMAND:

   case WM_DESTROY:
      PostQuitMessage( 0 );
      break;
   }                                   // switch( msg )

   return DefWindowProc( hWnd, msg, wParam, lParam );
}


 /******************************************************************************
  *
  * GetFTRSDKVersion - get Futronic SDK version. This is FTRAPI.dll version.
  * Syntax:
  *   void GetFTRSDKVersion( char *verinfo, int buflen );
  * Argument list:
  *   verinfo (input/output)  - text buffer;
  *   buflen (input)          - text buffer size.
  *
  */
void GetFTRSDKVersion( char *verinfo, int buflen )
{
   DWORD    dwZero;
   int      viLen;
   void     *verBuf;
   struct LANGANDCODEPAGE
   {
      WORD wLanguage;
      WORD wCodePage;
   }        *lpTranslate;
   int      cbTranslate;
   char     SubBlock[64];
   BOOL     bRet;
   char     *lpBuffer;
   DWORD    dwBytes;

   viLen = GetFileVersionInfoSize( "FTRAPI.dll", &dwZero );
   if( viLen == 0 )
   {
      strcpy( verinfo, "unknown version" );
      return;
   }

   if( (verBuf = malloc( viLen )) == NULL )
   {
      strcpy( verinfo, "unknown version" );
      return;
   }

   bRet = GetFileVersionInfo( "FTRAPI.dll", dwZero, viLen, verBuf );
   if( bRet == FALSE )
   {
      strcpy( verinfo, "unknown version" );
      return;
   }

   bRet = VerQueryValue( verBuf, TEXT("\\VarFileInfo\\Translation"),
      (LPVOID *)&lpTranslate, &cbTranslate );
   if( bRet == FALSE )
   {
      strcpy( verinfo, "unknown version" );
      return;
   }

   wsprintf( SubBlock, TEXT("\\StringFileInfo\\%04x%04x\\ProductVersion"),
            lpTranslate->wLanguage, lpTranslate->wCodePage);
   bRet = VerQueryValue( verBuf, SubBlock, &lpBuffer, &dwBytes );
   if( bRet == FALSE )
   {
      strcpy( verinfo, "unknown version" );
      return;
   }

   strncpy( verinfo, lpBuffer, __min( (DWORD)buflen, dwBytes ) );
   free( verBuf ); 
}