
 /******************************************************************************
  *
  * WorkedEx   - worked example using Futronic SDK.
  *
  * DataBase.c - simple DB for work with templates and images.
  *
  */

#include "WorkedEx.h"
#include <direct.h>
#include <stdio.h>
#include <io.h>
#include <shlobj.h>

 /******************************************************************************
  *
  * Data types, constants and macros.
  *
  */


 /******************************************************************************
  *
  * Global variables.
  *
  */
DBSET    dbs;                          // DB settings


 /******************************************************************************
  *
  * Functions prototypes.
  *
  */

 /*
  * AllocRecordMem   - allocate memory for store DB record.
  * Syntax:
  *   LPDBREC AllocRecordMem( DWORD dwSize );
  * Argument list:
  *   dwSize   - size of data member in record.
  * Return value:
  *   NULL  - no memory.
  */
LPDBREC AllocRecordMem( DWORD dwSize );

char const szCompanyName[] = "Futronic";
char const szProductName[] = "SDK 4.0";

 /******************************************************************************
  *
  * GetDatabaseParentFolder - get parent path for database folder for current user.
  * Syntax:
  *   BOOL GetDatabaseParentFolder( LPSTR pDatabasePath, int length );
  * Argument list:
  *   pDatabasePath  - pointer to memory where folder name will be saved;
  *   dLength - size of memory in symbols.
  * Return value:
  *   TRUE  - folder was gotten successfully;
  *   FALSE - error occures during getting folder information.
  *
  */
BOOL GetDatabaseParentFolder( LPSTR pDatabasePath, DWORD dLength )
{
    BOOL bRetCode = FALSE;
    HRESULT hr = CoInitialize( NULL );
    if( S_OK != hr )
    {
        return FALSE;
    }

    do
    {
        char szPath[MAX_PATH];
        hr = SHGetFolderPath( NULL, CSIDL_PERSONAL|CSIDL_FLAG_CREATE, NULL, SHGFP_TYPE_CURRENT, szPath );
        if( S_OK != hr )
        {
            break;
        }
        strcat_s( szPath, MAX_PATH, "\\" );
        strcat_s( szPath, MAX_PATH, szCompanyName );
        if( GetFileAttributes( szPath ) == INVALID_FILE_ATTRIBUTES )
        {
            if( !CreateDirectory( szPath, NULL ) )
            {
                break;
            }
        }

        strcat_s( szPath, MAX_PATH, "\\" );
        strcat_s( szPath, MAX_PATH, szProductName );
        if( GetFileAttributes( szPath ) == INVALID_FILE_ATTRIBUTES )
        {
            if( !CreateDirectory( szPath, NULL ) )
            {
                break;
            }
        }
        strcat_s( szPath, MAX_PATH, "\\" );

        if( strlen( szPath ) >= dLength )
        {
            break;
        }
        strcpy( pDatabasePath, szPath );

        bRetCode = TRUE;
    } while( FALSE );
    CoUninitialize();

    return bRetCode;
}

 /******************************************************************************
  *
  * dbInit  - load DB initial settings.
  * Syntax:
  *   void dbInit( void );
  *
  */
void dbInit( void )
{
   char     fName[_MAX_FNAME];
   HANDLE   hFind;
   WIN32_FIND_DATA   ff;

   if( !GetDatabaseParentFolder( fName, sizeof(fName) ) )
   {
       MessageBox( hMainWnd, "Can't create DB folder", "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
       return;
   }

   // set & create DB root folder
   strcat( fName, "DataBase" );
   strcpy( dbs.dbFolder, fName );
   hFind = FindFirstFile( dbs.dbFolder, &ff );
   if( hFind == INVALID_HANDLE_VALUE )
   {
      if( _mkdir( dbs.dbFolder ) == -1 )
         MessageBox( hMainWnd, "Can't create DB folder",
            "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
   }
   else
      FindClose( hFind );

   // set & create DB images folder
   strcat( strcpy( dbs.dbImages, fName ), "\\Bmp" );
   hFind = FindFirstFile( dbs.dbImages, &ff );
   if( hFind == INVALID_HANDLE_VALUE )
   {
      if( _mkdir( dbs.dbImages ) == -1 )
         MessageBox( hMainWnd, "Can't create DB folder",
            "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
   }
   else
      FindClose( hFind );

   // set & create DB templates folder
   strcpy( dbs.dbTemplates, fName );
   hFind = FindFirstFile( dbs.dbTemplates, &ff );
   if( hFind == INVALID_HANDLE_VALUE )
   {
      if( _mkdir( dbs.dbTemplates ) == -1 )
         MessageBox( hMainWnd, "Can't create DB folder",
            "WorkedEx fatal error", MB_OK | MB_ICONSTOP );
   }
   else
      FindClose( hFind );
}


 /******************************************************************************
  *
  * EmptyDB - check DB is empty.
  * Syntax:
  *   BOOL EmptyDB( void );
  * Return value:
  *   TRUE  - DB is empty;
  *   FALSE - DB is't empty.
  *
  */
BOOL EmptyDB( void )
{
   WIN32_FIND_DATA   ff;
   HANDLE            hFind;
   char              fMask[_MAX_FNAME];

   strcat( strcpy( fMask, dbs.dbTemplates ), "\\*.tml" );
   hFind = FindFirstFile( fMask, &ff );
   if( hFind == INVALID_HANDLE_VALUE )
      return TRUE;

   FindClose( hFind );
   return FALSE;
}


 /******************************************************************************
  *
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
  *
  */
BOOL AddRecord( char *name, void *data, DWORD dLen )
{
   char              fName[_MAX_FNAME];
   char              rKey[16];
   WIN32_FIND_DATA   ff;
   HANDLE            hFind;
   FILE              *fptr;

   // create file name
   strcat( strcat( strcat( strcpy( fName, dbs.dbTemplates ), "\\" ), name ), ".tml" );
   hFind = FindFirstFile( fName, &ff );
   if( hFind != INVALID_HANDLE_VALUE )
   {
      char  msg[128];
      FindClose( hFind );
      strcpy( msg, name );
      strcat( msg, " already exist.\n\nOverwrite record?" );
      if( MessageBox( hMainWnd, msg, "Warning:",
            MB_YESNO | MB_ICONQUESTION ) == IDNO )
         return FALSE;
   }

   // prepare data for writing
   ZeroMemory( (LPVOID)rKey, 16 );
   strcpy( rKey, name );

   // write record
   if( (fptr = fopen( fName, "wb" )) == NULL )
      return FALSE;
   fwrite( (void *)&dLen, sizeof( DWORD ), 1, fptr );
   fwrite( (void *)rKey, sizeof( char ), 16, fptr );
   fwrite( data, sizeof( char ), dLen, fptr );
   fclose( fptr );

   return TRUE;
}


 /******************************************************************************
  *
  * AllocRecordMem   - allocate memory for store DB record.
  * Syntax:
  *   LPDBREC AllocRecordMem( DWORD dwSize );
  * Argument list:
  *   dwSize   - size of data member in record.
  * Return value:
  *   NULL  - no memory.
  *
  */
LPDBREC AllocRecordMem( DWORD dwSize )
{
   LPDBREC  lpRec;

   if( (lpRec = (LPDBREC)malloc( sizeof( DBREC ) )) == NULL )
      return NULL;
   lpRec->len = dwSize;
   if( (lpRec->data = malloc( dwSize )) == NULL )
   {
      free( (void *)lpRec );
      return NULL;
   }
   return lpRec;
}


 /******************************************************************************
  *
  * FreeRecordMem - free memory for DB record.
  * Syntax:
  *   void FreeRecordMem( LPDBREC lpRec );
  * Argument list:
  *   lpRec - pointer on previously allocated DB record.
  *
  */
void FreeRecordMem( LPDBREC lpRec )
{
   free( lpRec->data );
   free( (void *)lpRec );
}


 /******************************************************************************
  *
  * GetRecord  - read record from DB.
  * Syntax:
  *   LPDBREC GetRecord( char *key );
  * Argument list:
  *   key   - record identificator (file name without extension).
  * Return value:
  *   NULL  - error.
  *
  */
LPDBREC GetRecord( char *key )
{
   char     fName[_MAX_FNAME];
   FILE     *fptr;
   DWORD    dSize, l;
   LPDBREC  lpRec;
   WORD     *s;

   // create file name for key
   strcat( strcat( strcat( strcpy( fName, dbs.dbTemplates ), "\\" ), key ), ".tml" );

   // open file
   if( (fptr = fopen( fName, "rb" )) == NULL )
      return NULL;
   fread( (void *)&dSize, sizeof( DWORD ), 1, fptr );
   l = _filelength( _fileno( fptr ) );
   if( l != dSize + 20 ) return NULL;
   if( (lpRec = AllocRecordMem( dSize )) == NULL )
      return NULL;

   // read record
   fread( (void *)lpRec->key, sizeof( char ), 16, fptr );
   if( strlen( lpRec->key ) == 0 ) {FreeRecordMem( lpRec ); return NULL;}
   fread( (void *)lpRec->data, sizeof( char ), dSize, fptr );
   fclose( fptr );
   s = (WORD *)lpRec->data;
   if( *s != dSize ) {FreeRecordMem( lpRec ); return NULL;}

   return lpRec;
}
