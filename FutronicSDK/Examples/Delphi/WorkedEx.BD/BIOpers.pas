
 {*****************************************************************************
  *
  * WorkedExBD    - worked example using Futronic SDK.
  *
  * BIOpers.pas   - concentrate all biometric operations.
  *
  *}

unit BIOpers;

interface

uses
   Windows, SysUtils, Dialogs, Forms,
   SDK_API, Errors, Includes, DataBase, ShowImage;

var
   boc:              BIOPERCONTEXT;
   bCancelOperation: Boolean;

function SetSDKDefaultSettings( ): Boolean;
procedure cbControl( Context: FTR_USER_CTX; StateMask: FTR_STATE;
   pResponse: FTR_RESPONSE_PTR; Signal: FTR_SIGNAL; pBitmap: FTR_BITMAP_PTR ); stdcall; far;
procedure Capture( );
procedure Enroll( );
procedure Verify( );
procedure Identify( );

implementation


 {*****************************************************************************
  *
  * SetSDKDefaultSettings - set SDK default settings.
  * Syntax:
  *   function SetSDKDefaultSettings( ): Boolean;
  * Return value:
  *   TRUE  - completed successfully;
  *   FALSE - error encountered.
  *
  *}
function SetSDKDefaultSettings( ): Boolean;
var
   rCode:      FTRAPI_RESULT;          // SDK return code
   value:      DWORD;
   bValue:     Boolean;
   rValCode:   Integer;

begin
   // default SDK settings:
   // 1. frame source
   value := FSD_FUTRONIC_USB;
   rCode := FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, FTR_PARAM_VALUE( value ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;

   // 2. user's callbacks
   rCode := FTRSetParam( FTR_PARAM_CB_CONTROL, FTR_PARAM_VALUE( @cbControl ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;

   // You may use FAR or FARN options on your own choice
   // 3. FAR setting
   {
   value := M2PArray[M2PDEFITEM].prob;
   rCode := FTRSetParam( FTR_PARAM_MAX_FAR_REQUESTED, FTR_PARAM_VALUE( value ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;
   }
   // 3. FARN setting
   val( M2PArray[M2PDEFITEM].meas, value, rValCode );
   rCode := FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, FTR_PARAM_VALUE( value ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;

   // 4. fake mode setting
   bValue := FALSE;
   rCode := FTRSetParam( FTR_PARAM_FAKE_DETECT, FTR_PARAM_VALUE( bValue ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;

   // 5. application takes a control over the Fake Finger Detection (FFD) event
   bValue := TRUE;
   rCode := FTRSetParam( FTR_PARAM_FFD_CONTROL, FTR_PARAM_VALUE( bValue ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;

   // 6. MIOT mode setting
   bValue := FALSE;
   rCode := FTRSetParam( FTR_PARAM_MIOT_CONTROL, FTR_PARAM_VALUE( bValue ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      SetSDKDefaultSettings := FALSE;
      Exit;
   end;

   bCancelOperation := False;
   SetSDKDefaultSettings := TRUE;

end;


 {******************************************************************************
  *
  * cbControl  - user's callback function for control the enrollment or
  *              verification execution flow.
  * Syntax:
  *   procedure cbControl( Context: FTR_USER_CTX; StateMask: FTR_STATE;
  *      FTR_RESPONSE *pResponse, FTR_SIGNAL Signal, FTR_BITMAP_PTR pBitmap );
  * Argument list:
  *   Context (input)      - user-defined context information;
  *   StateMask (input)    - a bit mask indicating what arguments are provided;
  *   pResponse (output)   - API function execution control is achieved through
  *                          this value;
  *   Signal (input)       - this signal should be used to interact with a user;
  *   pBitmap (input)      - a pointer to the bitmap to be displayed.
  *
  *}
procedure cbControl( Context: FTR_USER_CTX; StateMask: FTR_STATE;
   pResponse: FTR_RESPONSE_PTR; Signal: FTR_SIGNAL; pBitmap: FTR_BITMAP_PTR ); stdcall; far;
var
   lpboc:      LPBIOPERCONTEXT;        // biometric operation context
   lpPrgData:  FTR_PROGRESS_PTR;       // current progress data
   prgTitle:   array[0..63] of Char;   // progress window text
   iRet:       Integer;

begin
   lpboc := LPBIOPERCONTEXT( Context );
   lpPrgData := FTR_PROGRESS_PTR( pResponse );

   // frame show
   if (StateMask and FTR_STATE_FRAME_PROVIDED) <> 0 then
   begin
      UpdateImage( pBitmap );
   end;

   // message print
   if (StateMask and FTR_STATE_SIGNAL_PROVIDED) <> 0 then
   begin

      if Signal = FTR_SIGNAL_TOUCH_SENSOR then
      begin
         if (lpboc.oType = BO_ENROLL) and (lpPrgData.dwCount = 1) and
            (lpPrgData.bIsRepeated = False) then
         begin
            // setup progress bar
            lpboc.wPrgBar.Min := 0;
            lpboc.wPrgBar.Max := lpPrgData.dwTotal;
            lpboc.wPrgBar.Position := 0;
            lpboc.wPrgBar.Step := 1;
            lpboc.wTextLabel.Caption := '';
            // show progress bar
            lpboc.wPrgBar.Visible := True;
            lpboc.wTextLabel.Visible := True;
         end;
         PrintTextMsg( 'Put your finger on the scaner' );
      end;  // if Signal = FTR_SIGNAL_TOUCH_SENSOR then

      if Signal = FTR_SIGNAL_TAKE_OFF then
      begin
         if boc.oType = BO_ENROLL then
         begin
            // update progress
            lpboc.wPrgBar.Position := lpPrgData.dwCount;
            StrCopy( prgTitle, PChar( IntToStr( lpPrgData.dwCount ) ) );
            StrCat( prgTitle, ' of ' );
            StrCat( prgTitle, PChar( IntToStr( lpPrgData.dwTotal ) ) );
            lpboc.wTextLabel.Caption := prgTitle;
            lpboc.wTextLabel.Repaint( );
         end;
         PrintTextMsg( 'Take off your finger from the scaner' );
      end;  // if Signal = FTR_SIGNAL_TAKE_OFF then

      if Signal = FTR_SIGNAL_FAKE_SOURCE then
      begin
         iRet := MessageBox( 0, 'Fake finger detected. Continue process?',
                             '!!! Attention !!!', MB_YESNO + MB_ICONQUESTION );
         if iRet = IDYES then
            pResponse^ := FTR_CONTINUE
         else
            pResponse^ := FTR_CANCEL;
         Exit;
      end;  // if Signal = FTR_SIGNAL_FAKE_SOURCE then

      if Signal = FTR_SIGNAL_UNDEFINED then
         PrintTextMsg( 'Baida signal value' );

   end;  // if (StateMask and FTR_STATE_SIGNAL_PROVIDED) = 1 then

   if bCancelOperation = False then
      pResponse^ := FTR_CONTINUE
   else
      pResponse^ := FTR_CANCEL;

end;   


 {******************************************************************************
  *
  * Capture - capture operation.
  *
  *}
procedure Capture( );
var
   rCode:            FTRAPI_RESULT;
   ImageSize:        Integer;
   lpImageBytes:     Pointer;

begin
   // select memory
   rCode := FTRGetParam( FTR_PARAM_IMAGE_SIZE, FTR_PARAM_VALUE_PTR( @ImageSize ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      Exit;
   end;
   GetMem( lpImageBytes, ImageSize );

   // prepare callback context
   boc.oType   := BO_CAPTURE;

   // capture frame
   rCode := FTRCaptureFrame( FTR_USER_CTX( @boc ), lpImageBytes );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      FreeMem( lpImageBytes );
   end;
   PrintTextMsg( 'Take off your finger from the scaner' );
   
end;


 {******************************************************************************
  *
  * Enroll - enroll operation.
  *
  *}
procedure Enroll( );
var
   rCode:            FTRAPI_RESULT;
   TemplateSize:     Integer;
   lpTemplateBytes:  Pointer;
   Template:         FTR_DATA;
   eData:            FTR_ENROLL_DATA;
   bRet:             Boolean;
   ShortName:        array[0..15] of char;
   key:              array[0..15] of char;
   FullName:         array[0..MAX_PATH-1] of char;
   TextMsg:          array[0..127] of char;
begin

   // select memory
   rCode := FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE,
                         FTR_PARAM_VALUE_PTR( @TemplateSize ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      Exit;
   end;
   GetMem( lpTemplateBytes, TemplateSize );

   // prepare arguments
   Template.dwSize := TemplateSize;
   Template.pData  := lpTemplateBytes;
   eData.dwSize    := sizeof( FTR_ENROLL_DATA );

   // prepare callback context
   boc.oType := BO_ENROLL;

   // enroll operation
   rCode := FTREnrollX( FTR_USER_CTX( @boc ), FTR_PURPOSE_ENROLL,
                        @Template, @eData );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      FreeMem( lpTemplateBytes );
      Exit;
   end;

   // write template
   RecName.InitialDir := dbs.dbTemplates;
   bRet := RecName.Execute( );
   pMainForm.Refresh( );
   if bRet <> True then
   begin
      PrintTextMsg( 'Template not stored' );
      FreeMem( lpTemplateBytes );
      boc.wPrgBar.Visible := False;
      boc.wTextLabel.Visible := False;
      Exit;
   end;
   StrCopy( FullName, PChar( RecName.FileName ) );
   StrCopy( ShortName, @FullName[StrRScan( FullName, '\' ) - FullName + 1] );
   ShortName[StrRScan( ShortName, '.' ) - ShortName] := Char( 0 );
   ZeroMemory( @key, sizeof( key ) );
   StrCopy( key, ShortName );
   bRet := AddRecord( key, Template.pData, Template.dwSize );
   if bRet <> True then
      PrintTextMsg( 'Error store template' )
   else
   begin
      StrCopy( TextMsg, key );
      StrCat( TextMsg, ' successfully stored. Quality is ' );
      StrCat( TextMsg, PChar( IntToStr( eData.dwQuality ) ) );
      StrCat( TextMsg, ' of 10' );
      PrintTextMsg( TextMsg );
   end;

   // finish operation
   FreeMem( lpTemplateBytes );
   boc.wPrgBar.Visible := False;
   boc.wTextLabel.Visible := False;

end;


 {******************************************************************************
  *
  * CalcMfromP - calculate mesure value for probability.
  * Syntax:
  *   function CalcMfromP( prob: DWORD ): PChar;
  * Argument list:
  *   prob  - source probability value.
  * Return value:
  *   NULL  -  bad prob parameter;
  *         -  mesure value from M2PArray.
  *
  *}
function CalcMfromP( prob: DWORD ): PChar;
var
   iCyc:    Integer;
begin
   for iCyc := 0 to M2PNUM-1 do
      if M2PArray[iCyc].prob <= prob then
      begin
         CalcMfromP := M2PArray[iCyc].meas;
         Exit;
      end;
   CalcMfromP := nil;
end;


 {******************************************************************************
  *
  * Verify - verify operation.
  *
  *}
procedure Verify( );
var
   bRet:       Boolean;
   lpRec:      LPDBREC;
   rCode:      FTRAPI_RESULT;
   // FAR or FARN choise
   //vFAR:       FTR_FAR;
   vFARN:      FTR_FARN;
   sFARN:      string[10];
   cFARN:      array[0..10] of char;
   vResult:    LongBool;
   forVerify:  FTR_DATA;
   TextResult: array[0..63] of char;

begin
   // get template name from DB
   VrfName.InitialDir := dbs.dbTemplates;
   bRet := VrfName.Execute( );
   pMainForm.Refresh( );
   if bRet <> True then
   begin
      PrintTextMsg( 'Canseled by user' );
      Exit;
   end;

   // read template from DB
   lpRec := GetRecord( PChar( VrfName.FileName ) );
   if lpRec = nil then
   begin
      PrintTextMsg( 'DB reading error' );
      Exit;
   end;

   // prepare callback context
   boc.oType := BO_VERIFY;

   // prepare arguments
   forVerify.dwSize := lpRec.len;
   forVerify.pData := lpRec.data;

   // call verify operation
   // FAR or FARN choise
   //rCode := FTRVerify( FTR_USER_CTX( @boc ), @forVerify, @vResult, @vFAR );
   rCode := FTRVerifyN( FTR_USER_CTX( @boc ), @forVerify, @vResult, @vFARN );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      FreeRecordMem( lpRec );
      Exit;
   end;

   // print result
   if vResult = True then
   begin
      StrCopy( TextResult, 'Verification is successful (' );
      // FAR or FARN choise
      //StrCat( TextResult, CalcMfromP( vFar ) );
      Str( vFARN, sFARN );
      StrPCopy( cFARN, sFARN );
      StrCat( TextResult, cFARN );
      StrCat( TextResult, ')' );
      PrintTextMsg( TextResult );
   end
   else
      PrintTextMsg( 'Verification is wrong' );

   FreeRecordMem( lpRec );
end;


 {******************************************************************************
  *
  * Identify - identify operation.
  *
  *}
procedure Identify( );
var
   rCode:      FTRAPI_RESULT;
   Sample:     FTR_DATA;               // template for identify
   fromDB:     FTR_IDENTIFY_ARRAY;
   idRec:      FTR_IDENTIFY_RECORD;
   // FAR or FARN choise
   //resArr:     FTR_MATCHED_ARRAY;
   //resRec:     FTR_MATCHED_RECORD;
   resArrX:    FTR_MATCHED_X_ARRAY;
   resRecX:    FTR_MATCHED_X_RECORD;
   fMask:      array[0..MAX_PATH-1] of char;
   fullFName:  array[0..MAX_PATH-1] of char;
   hFind:      THandle;
   ff:         WIN32_FIND_DATA;
   bRet:       Boolean;
   lpDBR:      LPDBREC;
   msg:        array[0..63] of char;
   fromDBRec:  FTR_DATA;
   resNum:     DWORD;
   sFARN:      string[10];
   cFARN:      array[0..10] of char;

begin
   // prepare callback context
   boc.oType := BO_IDENTIFY;

   // create template for identify
   // 1. allocate memory
   rCode := FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE,
                         FTR_PARAM_VALUE_PTR( @Sample.dwSize ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      Exit;
   end;
   GetMem( Sample.pData, Sample.dwSize );
   // 2. enroll operation
   rCode := FTREnroll( FTR_USER_CTX( @boc ), FTR_PURPOSE_IDENTIFY, @Sample );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      FreeMem( Sample.pData );
      Exit;
   end;

   // prepare arguments for identify
   // 1. set base template
   rCode := FTRSetBaseTemplate( @Sample );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      FreeMem( Sample.pData );
      Exit;
   end;
   // 2. initialize array of identify records
   fromDB.TotalNumber   := 1;
   fromDB.pMembers      := @idRec;
   // 3. initialize result array
   // FAR or FARN choise
   //resArr.TotalNumber   := 1;
   //resArr.pMembers      := @resRec;
   resArrX.TotalNumber  := 1;
   resArrX.pMembers     := @resRecX;

   // prepare exhaustive search in DB
   StrCopy( fMask, dbs.dbTemplates );
   StrCat( fMask, '\*.tml' );
   hFind := FindFirstFile( fMask, ff );
   if hFind = INVALID_HANDLE_VALUE then
   begin
      PrintTextMsg( 'Database is empty' );
      FreeMem( Sample.pData );
      Exit;
   end;

   // identify cycle
   bRet := True;
   while bRet = True do 
   begin
      // read record from DB
      StrCopy( fullFName, dbs.dbTemplates );
      StrCat( fullFName, '\' );
      StrCat( fullFName, ff.cFileName );
      lpDBR := GetRecord( fullFName );
      if lpDBR = nil then
      begin
         PrintTextMsg( 'Error reading DB' );
         Windows.FindClose( hFind );
         FreeMem( Sample.pData );
         Exit;
      end;

      // translate DB record to identify record
      StrCopy( idRec.KeyValue, lpDBR.key );
      fromDBRec.dwSize := lpDBR.len;
      fromDBRec.pData := lpDBR.data;
      idRec.pData := @fromDBRec;

      // call identify
      resNum := 0;
      // FAR or FARN choise
      //rCode := FTRIdentify( @fromDB, @resNum, @resArr );
      rCode := FTRIdentifyN( @fromDB, @resNum, @resArrX );
      if rCode <> FTR_RETCODE_OK then
      begin
         wePrintError( rCode, 1 );
         FreeRecordMem( lpDBR );
         Windows.FindClose( hFind );
         FreeMem( Sample.pData );
         Exit;
      end;

      // search finish
      if resNum > 0 then
      begin
         StrCopy( msg, 'You are ' );
         // FAR or FARN choise
         //StrCat( msg, resArr.pMembers.KeyValue );
         StrCat( msg, resArrX.pMembers.KeyValue );
         StrCat( msg, ' (' );
         // FAR or FARN choise
         //StrCat( msg, CalcMfromP( resArr.pMembers.FarAttained ) );
         Str( resArrX.pMembers.FarAttained, sFARN );
         StrPCopy( cFARN, sFARN );
         StrCat( msg, cFARN );
         StrCat( msg, ')' );
         PrintTextMsg( msg );
         FreeRecordMem( lpDBR );
         Windows.FindClose( hFind );
         FreeMem( Sample.pData );
         Exit;
      end;

      // search continue
      FreeRecordMem( lpDBR );
      bRet := FindNextFile( hFind, ff );

   end;  // while bRet = True do

   // finaly
   Windows.FindClose( hFind );
   PrintTextMsg( 'You are not found' );
   FreeMem( Sample.pData );

end;

end.
