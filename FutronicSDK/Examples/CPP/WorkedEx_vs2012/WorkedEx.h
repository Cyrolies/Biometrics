
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * WorkedEx.h - header file for application.
  *
  */

#ifndef  _WorkedEx_Inc_
#define  _WorkedEx_Inc_

#if defined _WIN32
#  define _CRT_SECURE_NO_WARNINGS
#endif

#include <Windows.h>
#include <StdIO.h>
#include <commctrl.h>

#include "FTRAPI.h"
#include "rcInc.h"


 /******************************************************************************
  *
  * Data types, constants and macros.
  *
  */
#define  RECTW( r ) ((r).right - (r).left)
#define  RECTH( r ) ((r).bottom - (r).top)

#define  CAPTURE_TO_FILE      (1)      // capture and store image to file
#define  CAPTURE_TO_SCREEN    (2)      // capture and view image in window
#define  CAPTURE_FROM_FILE    (3)      // enroll, verify & identify get images
                                       // from file
#define  CAPTURE_FROM_SCANER  (4)      // enroll, verify & identify get images
                                       // from scaner
#pragma pack( push, 1 )

 // database initial settings
typedef struct
{
   char     dbFolder[_MAX_DIR];        // database folder root
   char     dbImages[_MAX_DIR];        // fingerprint images folder name
   char     dbTemplates[_MAX_DIR];     // templates folder name
} DBSET, *LPDBSET;

 // database record in memory
typedef struct
{
   char     key[16];                   // unique key within DB
   DWORD    len;                       // size of data
   void     *data;                     // record's data
} DBREC, * LPDBREC;

 // Measure <--> probability array record
typedef struct
{
   long     prob;                      // probability value
   char     *meas;                     // measure value
} M2PREC, *LPM2PREC;

#pragma pack( pop )

#define  M2PNUM               (25)     // M2PArray records number
#define  M2PDEFITEM           (10)     // default array item

 /******************************************************************************
  *
  * Global variables.
  *
  */
extern   HINSTANCE      hMainInst;     // application instance
extern   HWND           hMainWnd;      // main window handler
extern   HWND           hDlg;          // dialog window handler
extern   HWND           hShowWnd;      // view fingerprint window handler
extern   DBSET          dbs;           // DB settings
 // use for cancel current biometric operation
extern   BOOL           bCancelOperation;
 // Measure <--> probability array
extern   M2PREC         M2PArray[];


 /******************************************************************************
  *
  * Functions prototypes.
  *
  */

 /********************* ControlDlg.c *********************/

 /*
  * MainDlgProc   - control dialog pocedure.
  */
INT_PTR CALLBACK MainDlgProc( HWND  hDWnd, UINT  uMsg, WPARAM wParam, LPARAM lParam );

 /*
  * PrintTextMsg  - print text message into IDC_TEXTMESSAGE dialog element in
  *                 application's control dialog.
  * Syntax:
  *   void PrintTextMsg( LPCSTR lpTextMsg );
  * Argument list:
  *   lpTextMsg   - text message.
  */
void PrintTextMsg( LPCSTR lpTextMsg );

 /*
  * UpdateControl - enable/disable control buttons while process operation.
  * Syntax:
  *   void UpdateControl( BOOL sw );
  * Argument list:
  *   sw - TRUE switch ON all control biometric buttons;
  *        FALSE switch OFF all control biometric buttons.
  */
void UpdateControl( BOOL sw );


 /********************* Errors.c *********************/

 /*
  * weGetErrorText   - get error text by code.
  * Syntax:
  *   LPCSTR weGetErrorText( FTRAPI_RESULT rCode );
  * Argument list:
  *   rCode - error code.
  * Return value:
  *   Error text pointer.
  */
LPCSTR weGetErrorText( FTRAPI_RESULT rCode );

 /*
  * wePrintError  - print error text.
  * Syntax:
  *   void wePrintError( FTRAPI_RESULT rCode, int outType );
  * Argument list:
  *   rCode    - error code;
  *   outType  - if 0 - output by MessageBox;
  *              if 1 - output by PrintTextMsg.
  */
void wePrintError( FTRAPI_RESULT rCode, int outType );


 /********************* weShowWnd.c *********************/

 /*
  * weRegShowWnd  - register window class for viewing fingerprint image.
  * Syntax:
  *   BOOL weRegShowWnd( HINSTANCE hInst );
  * Argument list:
  *   hInst - application instance.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error while register classes.
  */
BOOL weRegShowWnd( HINSTANCE hInst );

 /*
  * CreateShowWnd - create window for viewing fingerprint image.
  * Syntax:
  *   HWND CreateShowWnd( HINSTANCE hInst, HWND hParent, int x, int y );
  * Argument list:
  *   hInst    - application instance;
  *   hParent  - parent window;
  *   x, y     - upper left corner in parent.
  * Return value:
  *   NULL  - error creating;
  *         - success.
  *
  */
HWND CreateShowWnd( HINSTANCE hInst, HWND hParent, int x, int y );

 /*
  * UpdateImage   - updates data and shows the current fingerprint image.
  * Syntax:
  *   void UpdateImage( void *srcData );
  * Argument list:
  *   srcData  - new fingerprint image.
  */
void UpdateImage( void *srcData );

 /*
  * WriteBMPFile - write fingerprint image to DB as BMP-file.
  * Syntax:
  *   BOOL WriteBMPFile( void *lpBits, char *dbBMPFolder, char *fMask, int num );
  * Argument list:
  *   lpBits      - fingerprint image;
  *   dbBMPFolder - DB folder name for bitmap store;
  *   fMask       - base of file name;
  *   num         - fingerprint image number.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error.
  */
BOOL WriteBMPFile( void *lpBits, char *dbBMPFolder, char *fMask, int num );


 /********************* DataBase.c *********************/

 /*
  * dbInit  - load DB initial settings.
  * Syntax:
  *   void dbInit( void );
  */
void dbInit( void );

 /*
  * EmptyDB - check DB is empty.
  * Syntax:
  *   BOOL EmptyDB( void );
  * Return value:
  *   TRUE  - DB is empty;
  *   FALSE - DB is't empty.
  */
BOOL EmptyDB( void );

 /*
  * AddRecord  - add new record to database.
  * Syntax:
  *   BOOL AddRecord( char *name, void *data, DWORD dLen );
  * Argument list:
  *   name  - identifier of record (in this case it is the file name);
  *   data  - data for store;
  *   dLen  - size of data.
  * Return value:
  *   TRUE  - record was writen;
  *   FALSE - record was't writen.
  */
BOOL AddRecord( char *name, void *data, DWORD dLen );

 /*
  * FreeRecordMem - free memory for DB record.
  * Syntax:
  *   void FreeRecordMem( LPDBREC lpRec );
  * Argument list:
  *   lpRec - pointer on previously allocated DB record.
  */
void FreeRecordMem( LPDBREC lpRec );

 /*
  * GetRecord  - read record from DB.
  * Syntax:
  *   LPDBREC GetRecord( char *key );
  * Argument list:
  *   key   - record identificator (file name without extension).
  * Return value:
  *   NULL  - error.
  */
LPDBREC GetRecord( char *key );


 /********************* BiOpers.c *********************/

 /*
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
  */
void FTR_CBAPI cbControl( FTR_USER_CTX Context, FTR_STATE StateMask, FTR_RESPONSE *pResponse,
                          FTR_SIGNAL Signal, FTR_BITMAP_PTR pBitmap );

 /*
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
  */
BOOL CaptureStarter( int tDest );

 /*
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
  */
BOOL EnrollStarter( int tSrc );

 /*
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
  */
BOOL VerifyStarter( int tSrc );

 /*
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
  */
BOOL IdentifyStarter( int tSrc );

 /*
  * CalcPfromM - calculate probability value for mesure.
  * Syntax:
  *   long CalcPfromM( char *meas );
  * Argument list:
  *   meas  - source mesure value.
  * Return value:
  *   -1 -  bad meas parameter;
  *      -  probability value from M2PArray.
  */
long CalcPfromM( char *meas );

 /*
  * CalcMfromP - calculate mesure value for probability.
  * Syntax:
  *   char *CalcMfromP( long prob );
  * Argument list:
  *   prob  - source probability value.
  * Return value:
  *   NULL  -  bad prob parameter;
  *         -  mesure value from M2PArray.
  */
char *CalcMfromP( long prob );


#endif   // _WorkedEx_Inc_
