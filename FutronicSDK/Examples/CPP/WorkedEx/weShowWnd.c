
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * weShowWnd.c   - window for viewing fingerprint image.
  *
  */

#include "WorkedEx.h"
#include <StdIO.h>
#include <direct.h>


 /******************************************************************************
  *
  * Global variables.
  *
  */
// Show window class name
char const szShowClassName[] = "ShowWndClass";

BOOL     bTitleShow  = TRUE;
HBITMAP  hbmTitle;                     // title bitmap

 // Data for show fingerprint image
BITMAPINFO     *lpDIBHeader   = NULL;
char           *lbDIBData     = NULL;
LPLOGPALETTE   lpPal          = NULL;
HPALETTE       hPGrayscale    = NULL;
BITMAPFILEHEADER  bmfHeader;



 /******************************************************************************
  *
  * Functions prototypes.
  *
  */

 /*
  * ShowWndProc   - the show window function.
  */
__declspec( dllexport ) LRESULT WINAPI ShowWndProc( HWND hWnd, UINT msg,
                                                    WPARAM wParam, LPARAM lParam );

 /*
  * DrawBitmap - Draws a bitmap onto a device.
  * ÑÈÍÒÀÊÑÈÑ: void DrawBitmap( HDC hDC, int x, int y, HBITMAP hBitmap );
  * ÂÕÎÄ:      hDC     - Pointer to a device context;
  *            x,y     - Upper-left corner of the destination rect;
  *            nBitmap - Handle of the bitmap.
  */
void DrawBitmap( HDC hDC, int x, int y, HBITMAP hBitmap );

 /*
  * PrepareView - allocates memory & initializes data for fingerprint viewing.
  * Syntax:
  *   BOOL PrepareView( int w, int h, int d );
  * Argument list:
  *   w, h, d  - width, height and color depth of fingerprint image.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error.
  */
BOOL PrepareView( int w, int h, int d );

 /*
  * DIBShow - shows the captured fingerprint image.
  * Syntax:
  *   void DIBShow( HDC hdc );
  * Argument list:
  *   hdc   - show window device context.
  */
void DIBShow( HDC hdc );


 /******************************************************************************
  *
  * weRegShowWnd  - register window class for viewing fingerprint image.
  * Syntax:
  *   BOOL weRegShowWnd( HINSTANCE hInst );
  * Argument list:
  *   hInst - application instance.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error while register classes.
  *
  */
BOOL weRegShowWnd( HINSTANCE hInst )
{
   ATOM     aWndClass;                 // return code
   WNDCLASS    wc;                        // structure for register window class

   memset( &wc, 0, sizeof( wc ) );

   wc.style          = 0;
   wc.lpfnWndProc    = (WNDPROC)ShowWndProc;
   wc.cbClsExtra     = 0;
   wc.cbWndExtra     = 0;
   wc.hInstance      = hInst;
   wc.hIcon          = NULL;
   wc.hCursor        = LoadCursor( NULL, IDC_ARROW );
   wc.hbrBackground    = (HBRUSH)(COLOR_WINDOWTEXT + 1);
   wc.lpszMenuName    = NULL;
   wc.lpszClassName    = szShowClassName;

   aWndClass = RegisterClass( &wc );
   if (aWndClass == 0) return FALSE;
      else return TRUE;
}


 /******************************************************************************
  *
  * ShowWndProc   - the show window function.
  *
  */
__declspec( dllexport ) LRESULT WINAPI ShowWndProc( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
   HDC          hDC;
   PAINTSTRUCT  ps;

   switch (msg)
   {

    case WM_PAINT:
      {
          hDC = BeginPaint( hWnd, &ps );
         // draw title bitmap
         if( bTitleShow )
            DrawBitmap( hDC, 50, 150, hbmTitle );
         // draw fingerprint image
         else
         {
            DIBShow( hDC );
         }
          EndPaint( hWnd, &ps );
         break;
      }                                // case WM_PAINT:

    case WM_ERASEBKGND:
       return 1;

   case WM_DESTROY:
      {
            if( hPGrayscale )
                DeleteObject( (HGDIOBJ)hPGrayscale );
            if( lpPal )
                free( (void *)lpPal );
            if( lbDIBData )
                free( (void *)lbDIBData );
            if( lpDIBHeader )
                free( (void *)lpDIBHeader );
         DeleteObject( (HGDIOBJ)hbmTitle );
         return 0;
      }                                // case WM_DESTROY:

   }                                   // switch (msg)

   return DefWindowProc( hWnd, msg, wParam, lParam );
}


 /******************************************************************************
  *
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
HWND CreateShowWnd( HINSTANCE hInst, HWND hParent, int x, int y )
{
   int               w, h;             // width & height show window
   FTRAPI_RESULT     rCode;            // SDK return code
   HWND              hSWnd;            // show window handle

   // get width & height show window
   rCode = FTRGetParam( FTR_PARAM_IMAGE_WIDTH, (FTR_PARAM_VALUE)&w );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 0 );
      return NULL;
   }
   rCode = FTRGetParam( FTR_PARAM_IMAGE_HEIGHT, (FTR_PARAM_VALUE)&h );
   if( rCode != FTR_RETCODE_OK )
   {
      wePrintError( rCode, 0 );
      return NULL;
   }

   // prepare data for view fingerprint image
   if( PrepareView( w, h, 8 ) == FALSE )
      return NULL;

   // load title bitmap
   hbmTitle = LoadBitmap( hMainInst, MAKEINTRESOURCE( IDB_FUTRONIC ) );

   // create show window
   hSWnd = CreateWindow(
      szShowClassName,                 // class name
      NULL,                            // window title
      WS_CHILD | WS_VISIBLE | WS_DLGFRAME,   // window style
      x,                               // x-coordinate
      y,                               // y-coordinate
      w + 2*GetSystemMetrics( SM_CXDLGFRAME ),  // width
      h + 2*GetSystemMetrics( SM_CYDLGFRAME ),  // height
      hParent,                         // parent window handle
      NULL,                            // menu handle
      hInst,                           // application instance
      NULL );                          // pointer for additional parameters
   // creation error
   if( hSWnd == NULL )
   {
        DeleteObject( (HGDIOBJ)hPGrayscale );
        free( (void *)lpPal );
        free( (void *)lbDIBData );
        free( (void *)lpDIBHeader );
      DeleteObject( (HGDIOBJ)hbmTitle );
      return NULL;
   }
   UpdateWindow( hMainWnd );

   return hSWnd;
}


 /*
  * DrawBitmap - Draws a bitmap onto a device.
  * ÑÈÍÒÀÊÑÈÑ: void DrawBitmap( HDC hDC, int x, int y, HBITMAP hBitmap );
  * ÂÕÎÄ:      hDC     - Pointer to a device context;
  *            x,y     - Upper-left corner of the destination rect;
  *            nBitmap - Handle of the bitmap.
  */
void DrawBitmap( HDC hDC, int x, int y, HBITMAP hBitmap )
{
 HBITMAP hOldbm;
 HDC     hMemDC;
 BITMAP  bm;
 POINT   ptSize, ptOrg;

   // Create an in-memory DC compatible with the
   // display DC we're using to paint
   hMemDC = CreateCompatibleDC( hDC );

   // Select the bitmap into the in-memory DC
   hOldbm = (HBITMAP)SelectObject( hMemDC, hBitmap );

   if (hOldbm)
     {
      SetMapMode( hMemDC, GetMapMode( hDC ) );
      GetObject( hBitmap, sizeof(BITMAP), (LPSTR)&bm );
      ptSize.x = bm.bmWidth;
      ptSize.y = bm.bmHeight;

      DPtoLP( hDC, &ptSize, 1 );
      ptOrg.x = 0;
      ptOrg.y = 0;

      DPtoLP( hMemDC, &ptOrg, 1 );

      BitBlt( hDC, x, y, ptSize.x, ptSize.y, hMemDC, ptOrg.x, ptOrg.y,
         SRCCOPY );

      SelectObject( hMemDC, hOldbm );
      }

   DeleteDC( hMemDC );
 }


 /******************************************************************************
  *
  * PrepareView - allocates memory & initializes data for fingerprint viewing.
  * Syntax:
  *   BOOL PrepareView( int w, int h, int d );
  * Argument list:
  *   w, h, d  - width, height and color depth of fingerprint image.
  * Return value:
  *   TRUE  - success;
  *   FALSE - error.
  *
  */
BOOL PrepareView( int w, int h, int d )
{
   int   iCyc;

   // support the 256-colors DIB only
   if( d != 8 ) return FALSE;

   // allocate memory for a DIB header
   if( (lpDIBHeader = (BITMAPINFO *)malloc( sizeof( BITMAPINFO ) +
                                            sizeof( RGBQUAD ) * 255 )) == NULL )
      return FALSE;
   ZeroMemory( (PVOID)lpDIBHeader, sizeof( BITMAPINFO ) + sizeof( RGBQUAD ) * 255 );

   // allocate memory for a DIB data
   if( (lbDIBData = (char *)malloc( sizeof( char ) * w * h )) == NULL )
      goto btCreateViewDataError1;
   FillMemory( (PVOID)lbDIBData, sizeof( char ) * w * h, 0xFF );

   // allocate memory for a logical palette
   if( (lpPal = (LPLOGPALETTE)malloc( sizeof( LOGPALETTE ) +
                                     sizeof( PALETTEENTRY ) * 255 )) == NULL )
      goto btCreateViewDataError2;

   // fill the DIB header
   lpDIBHeader->bmiHeader.biSize          = sizeof( BITMAPINFOHEADER );
   lpDIBHeader->bmiHeader.biWidth         = w;
   lpDIBHeader->bmiHeader.biHeight        = h;
   lpDIBHeader->bmiHeader.biPlanes        = 1;
   lpDIBHeader->bmiHeader.biBitCount      = d;
   lpDIBHeader->bmiHeader.biCompression   = BI_RGB;

   // fill the logical palette
   lpPal->palVersion    = 0x300;
   lpPal->palNumEntries = 256;

   // initialize logical and DIB palettes to grayscale
   for( iCyc = 0; iCyc < 256; iCyc++ )
   {
      lpDIBHeader->bmiColors[iCyc].rgbBlue = lpDIBHeader->bmiColors[iCyc].rgbGreen =
      lpDIBHeader->bmiColors[iCyc].rgbRed = lpPal->palPalEntry[iCyc].peBlue =
      lpPal->palPalEntry[iCyc].peGreen = lpPal->palPalEntry[iCyc].peRed = (BYTE)iCyc;
   }

   // create a grayscale palette
   if( (hPGrayscale = CreatePalette( lpPal )) == NULL )
      goto btCreateViewDataError3;

   // set BITMAPFILEHEADER structure
   ((char *)(&bmfHeader.bfType))[0] = 'B';
   ((char *)(&bmfHeader.bfType))[1] = 'M';
   bmfHeader.bfSize = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) + sizeof( RGBQUAD ) * 255
      + sizeof( char ) * w * h;
   bmfHeader.bfOffBits = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO )
      + sizeof( RGBQUAD ) * 255;

   // normal return
   return TRUE;

   // error detected
btCreateViewDataError3:
   free( (void *)lpPal );
btCreateViewDataError2:
   free( (void *)lbDIBData );
btCreateViewDataError1:
   free( (void *)lpDIBHeader );
   return FALSE;
}


 /******************************************************************************
  *
  * DIBShow - shows the captured fingerprint image.
  * Syntax:
  *   void DIBShow( HDC hdc );
  * Argument list:
  *   hdc   - show window device context.
  *
  */
void DIBShow( HDC hdc )
{
   SelectPalette( hdc, hPGrayscale, FALSE );
   RealizePalette( hdc );

    SetDIBitsToDevice(
      hdc,                             // handle of device context
      0,                               // x-coordinate origin of destination rect
      0,                               // y-coordinate origin of destination rect
      lpDIBHeader->bmiHeader.biWidth,  // rectangle width
      lpDIBHeader->bmiHeader.biHeight, // rectangle height
      0,                               // x-coordinate origin of source rect
      0,                               // y-coordinate origin of source rect
      0,                               // number of the first scan line in array
      lpDIBHeader->bmiHeader.biHeight, // number of scan lines
      lbDIBData,                       // address of array with DIB bits
      lpDIBHeader,                     // address of structure with bitmap info
      DIB_RGB_COLORS);                 // RGB or palette indices
}


 /******************************************************************************
  *
  * UpdateImage   - updates data and shows the current fingerprint image.
  * Syntax:
  *   void UpdateImage( void *srcData );
  * Argument list:
  *   srcData  - new fingerprint image.
  *
  */
void UpdateImage( void *srcData )
{
   HDC   hdc;
   int   iCyc;
   char  *cptrData;
   char  *cptrDIBData;

   // turn off show title bitmap
   bTitleShow = FALSE;

   // rotate an image while copying data to the DIB data
   cptrData = (char *)srcData + (lpDIBHeader->bmiHeader.biHeight - 1) *
      lpDIBHeader->bmiHeader.biWidth;
   cptrDIBData = lbDIBData;
   for( iCyc = 0; iCyc < lpDIBHeader->bmiHeader.biHeight; iCyc++ )
   {
      memcpy( cptrDIBData, cptrData, lpDIBHeader->bmiHeader.biWidth );
      cptrData = cptrData - lpDIBHeader->bmiHeader.biWidth;
      cptrDIBData = cptrDIBData + lpDIBHeader->bmiHeader.biWidth;
   }

   // show the fingerprint image
   hdc = GetDC( hShowWnd );
   DIBShow( hdc );
   ReleaseDC( hShowWnd, hdc );
}


 /******************************************************************************
  *
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
  *
  */
BOOL WriteBMPFile( void *lpBits, char *dbBMPFolder, char *fMask, int num )
{
   char              ImageFolder[_MAX_FNAME];
   char              FileName[_MAX_FNAME];
   HANDLE            hFind;
   WIN32_FIND_DATA   ff;
   FILE              *fptr;
   int   iCyc;
   char  *cptrData;
   char  *cptrDIBData;

   // create file name
   strcat( strcat( strcpy( ImageFolder, dbBMPFolder ), "\\" ), fMask );
   // check exist name in DB
   if( num == 0 )
   {
      hFind = FindFirstFile( ImageFolder, &ff );
      if( hFind != INVALID_HANDLE_VALUE )
      {
         char  msg[_MAX_FNAME];
         FindClose( hFind );
         strcpy( msg, fMask );
         strcat( msg, " already exist.\n\nOverwrite file(s)?" );
         if( MessageBox( hMainWnd, msg, "Atenntion!",
               MB_YESNO | MB_ICONQUESTION ) == IDNO )
            return FALSE;
      }
      else
         _mkdir( ImageFolder );
   }
   // continue file name creation
   sprintf( FileName, "%s\\%s%02d.bmp", ImageFolder, fMask, num );

   // copy fingerprint image
   cptrData = (char *)lpBits + (lpDIBHeader->bmiHeader.biHeight - 1) *
      lpDIBHeader->bmiHeader.biWidth;
   cptrDIBData = lbDIBData;
   for( iCyc = 0; iCyc < lpDIBHeader->bmiHeader.biHeight; iCyc++ )
   {
      memcpy( cptrDIBData, cptrData, lpDIBHeader->bmiHeader.biWidth );
      cptrData = cptrData - lpDIBHeader->bmiHeader.biWidth;
      cptrDIBData = cptrDIBData + lpDIBHeader->bmiHeader.biWidth;
   }

   // write file
   if( (fptr = fopen( FileName, "wb" )) == NULL )
      return FALSE;
   fwrite( (void *)&bmfHeader, sizeof( char ), sizeof( BITMAPFILEHEADER ), fptr );
   fwrite( (void *)lpDIBHeader, sizeof( char ),
      sizeof( BITMAPINFO ) + sizeof( RGBQUAD ) * 255, fptr );
   fwrite( (void *)lbDIBData, sizeof( char ),
      lpDIBHeader->bmiHeader.biWidth * lpDIBHeader->bmiHeader.biHeight, fptr );
   fclose( fptr );

   return TRUE;
}
