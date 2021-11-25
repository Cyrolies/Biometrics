
 {******************************************************************************
  *
  * WorkedExXE4    - worked example using Futronic SDK.
  *
  * DataBase.pas   - simple DB for work with templates and images.
  *
  *}
unit DataBase;

interface

uses
   Windows, SysUtils, Includes, SDK_API, SHFolder;

procedure dbInit( );
function EmptyDB( ): Boolean;
function AddRecord( key: PChar; data: PChar; dLen: DWORD ): Boolean;
function GetRecord( fName: String ): LPDBREC;
procedure FreeRecordMem( lpRec: LPDBREC );

implementation


 {******************************************************************************
  *
  * dbInit  - load DB initial settings.
  * Syntax:
  *   procedure dbInit( );
  *
  *}
procedure dbInit( );
var
   fName:   array[0..MAX_PATH-1] of Char;
   hFind:   THandle;
   ff:      WIN32_FIND_DATA;
begin

   ZeroMemory( @dbs.dbFolder, sizeof( dbs.dbFolder ) );
   ZeroMemory( @dbs.dbImages, sizeof( dbs.dbImages ) );
   ZeroMemory( @dbs.dbTemplates, sizeof( dbs.dbTemplates ) );

   ZeroMemory( @fName, sizeof( fName ) );
   SHGetFolderPath( 0, CSIDL_PERSONAL, 0, 0 (* SHGFP_TYPE_CURRENT *), fName );
   StrCat( fName, '\Futronic' );

   StrCopy( dbs.dbFolder, fName );
   hFind := FindFirstFile( dbs.dbFolder, ff );
   if hFind = INVALID_HANDLE_VALUE then
      MkDir( dbs.dbFolder )
   else
      Windows.FindClose( hFind );

   StrCat( fName, '\SDK 4.0' );

   StrCopy( dbs.dbFolder, fName );
   hFind := FindFirstFile( dbs.dbFolder, ff );
   if hFind = INVALID_HANDLE_VALUE then
      MkDir( dbs.dbFolder )
   else
      Windows.FindClose( hFind );

   // set & create DB root folder
   StrCat( fName, '\DataBase' );
   StrCopy( dbs.dbFolder, fName );
   hFind := FindFirstFile( dbs.dbFolder, ff );
   if hFind = INVALID_HANDLE_VALUE then
      MkDir( dbs.dbFolder )
   else
      Windows.FindClose( hFind );

   // set & create DB images folder
   StrCopy( dbs.dbImages, fName );
   StrCat( dbs.dbImages, '\Bmp' );
   hFind := FindFirstFile( dbs.dbImages, ff );
   if hFind = INVALID_HANDLE_VALUE then
      MkDir( dbs.dbImages )
   else
      Windows.FindClose( hFind );

   // set & create DB templates folder
   StrCopy( dbs.dbTemplates, fName );
   hFind := FindFirstFile( dbs.dbTemplates, ff );
   if hFind = INVALID_HANDLE_VALUE then
      MkDir( dbs.dbTemplates )
   else
      Windows.FindClose( hFind );
end;


 {******************************************************************************
  *
  * EmptyDB - check DB is empty.
  * Syntax:
  *   function EmptyDB( ): Boolean;
  * Return value:
  *   TRUE  - DB is empty;
  *   FALSE - DB is't empty.
  *
  *}
function EmptyDB( ): Boolean;
var
   ff:      WIN32_FIND_DATA;
   hFind:   THandle;
   fMask:   array[0..MAX_PATH-1] of Char;
begin

   StrCopy( fMask, dbs.dbTemplates );
   StrCat( fMask, '\*.tml' );
   hFind := FindFirstFile( fMask, ff );
   if hFind = INVALID_HANDLE_VALUE then
   begin
      EmptyDB := TRUE;
      Exit;
   end;
   Windows.FindClose( hFind );
   EmptyDB := FALSE;

end;


 {******************************************************************************
  *
  * AddRecord  - add new record to database.
  * Syntax:
  *   function AddRecord( key: PChar; data: Pointer, dLen: DWORD ): Boolean;
  * Argument list:
  *   key   - identifier of record (in this case it is the file name);
  *   data  - data for store;
  *   dLen  - size of data.
  * Return value:
  *   TRUE  - record was writen;
  *   FALSE - record was't writen.
  *
  *}
function AddRecord( key: PChar; data: PChar; dLen: DWORD ): Boolean;
var
   fName:      array[0..MAX_PATH] of char;
   fPtr:       file;
   wrTotal:    Integer;
   
begin
   // create file name
   StrCopy( fName, dbs.dbTemplates );
   StrCat( fName, '\' );
   StrCat( fName, key );
   StrCat( fName, '.tml' );

   // write record
   AssignFile( fPtr, fName );
   Rewrite( fPtr, 1 );
   BlockWrite( fPtr, dLen, sizeof( DWORD ), wrTotal );
   if wrTotal <> sizeof( DWORD ) then
   begin
      AddRecord := False;
      CloseFile( fPtr );
      Exit;
   end;
   BlockWrite( fPtr, key^, 16, wrTotal );
   if wrTotal <> 16 then
   begin
      AddRecord := False;
      CloseFile( fPtr );
      Exit;
   end;
   BlockWrite( fPtr, data^, dLen, wrTotal );
   if DWORD( wrTotal ) <> dLen then
   begin
      AddRecord := False;
      CloseFile( fPtr );
      Exit;
   end;


   // finish operation
   AddRecord := True;
   CloseFile( fPtr );

end;


 {******************************************************************************
  *
  * AllocRecordMem   - allocate memory for store DB record.
  * Syntax:
  *   function AllocRecordMem( dwSize: DWORD ): LPDBREC;
  * Argument list:
  *   dwSize   - size of data member in record.
  * Return value:
  *   NULL  - no memory.
  *
  *}
function AllocRecordMem( dwSize: DWORD ): LPDBREC;
var
   lpRec:      LPDBREC;

begin
   GetMem( lpRec, sizeof( DBREC ) );
   lpRec.len := dwSize;
   GetMem( lpRec.data, dwSize );
   AllocRecordMem := lpRec;
end;  


 {******************************************************************************
  *
  * FreeRecordMem - free memory for DB record.
  * Syntax:
  *   procedure FreeRecordMem( lpRec: LPDBREC );
  * Argument list:
  *   lpRec - pointer on previously allocated DB record.
  *
  *}
procedure FreeRecordMem( lpRec: LPDBREC );
begin
   FreeMem( lpRec.data );
   FreeMem( lpRec );
end;


 {******************************************************************************
  *
  * GetRecord  - read record from DB.
  * Syntax:
  *   function GetRecord( fName: String ): LPDBREC;
  * Argument list:
  *   fName   - file name.
  * Return value:
  *   NULL  - error.
  *
  *}
function GetRecord( fName: String ): LPDBREC;
var
   fPtr:       file;
   lpRec:      LPDBREC;
   dSize:      DWORD;
   rdTotal:    Integer;

begin
   // get record's data length
   AssignFile( fPtr, fName );
   Reset( fPtr, 1 );
   BlockRead( fPtr, dSize, sizeof( DWORD ), rdTotal );
   if rdTotal <> sizeof( DWORD ) then
   begin
      CloseFile( fPtr );
      GetRecord := nil;
      Exit;
   end;

   // allocate memory for record
   lpRec := AllocRecordMem( dSize );

   // read key information
   BlockRead( fPtr, lpRec.key, sizeof( FTR_DATA_KEY ), rdTotal );
   if rdTotal <> sizeof( FTR_DATA_KEY ) then
   begin
      CloseFile( fPtr );
      FreeRecordMem( lpRec );
      GetRecord := nil;
      Exit;
   end;

   // read biometric data
   BlockRead( fPtr, PChar( lpRec.data )^, dSize, rdTotal );
   if DWORD( rdTotal ) <> dSize then
   begin
      CloseFile( fPtr );
      FreeRecordMem( lpRec );
      GetRecord := nil;
      Exit;
   end;

   // success
   CloseFile( fPtr );
   GetRecord := lpRec;
end;

end.
