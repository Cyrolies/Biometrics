program WorkedExXE4;

uses
  Forms,
  Windows,
  TMainForm in 'TMainForm.pas' {Main},
  SDK_API in 'SDK_API.pas',
  Errors in 'Errors.pas',
  BIOpers in 'BIOpers.pas',
  Includes in 'Includes.pas',
  DataBase in 'DataBase.pas',
  ShowImage in 'ShowImage.pas';

{$R *.RES}

var
   hAppRun:    THandle;
   rCode:      FTRAPI_RESULT;          // SDK return code

begin

   // check uniqueness WorkedEx copy
   hAppRun := CreateMutex( nil, True, 'WorkedExMutex' );
   if GetLastError( ) = ERROR_ALREADY_EXISTS then
      begin
         MessageBox( 0, 'WorkedEx is already run', 'Warning', MB_OK or MB_ICONINFORMATION );
         Exit;
      end;

   // initialize SDK
   rCode := FTRInitialize( );
   if rCode <> FTR_RETCODE_OK then
   begin
      MessageBox( 0, PChar( weGetErrorText( rCode ) ), 'SDK Error', MB_OK or MB_ICONSTOP );
      CloseHandle( hAppRun );
      Exit;
   end;
   
   // initialize DB
   dbInit( );

   // run application
   Application.Initialize;
   Application.CreateForm(TMain, Main);
   Application.Run;

   // close SDK and global resourses
   FTRTerminate( );
   CloseHandle( hAppRun );
end.
