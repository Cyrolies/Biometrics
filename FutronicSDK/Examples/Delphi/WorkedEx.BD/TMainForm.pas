
 {*****************************************************************************
  *
  * WorkedExBD    - worked example using Futronic SDK.
  *
  * TMainForm.pas - application main form.
  *
  *}

unit TMainForm;

interface

uses
   Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
   ExtCtrls, StdCtrls, ComCtrls,
   SDK_API, Includes, BIOpers, Errors, DataBase;

type
   TMain = class(TForm)
   MainBackground: TPanel;
   OperationsGBox: TGroupBox;
   ButCapture: TButton;
   ButVerify: TButton;
   ButEnroll: TButton;
   ButIdentify: TButton;
   ButStop: TButton;
   ButExit: TButton;
   SettingsGBox: TGroupBox;
   CaptureToFileRB: TRadioButton;
   CaptureToScreenGB: TRadioButton;
   LabelCaptureTo: TLabel;
   GroupBox1: TGroupBox;
   DetectFFCB: TCheckBox;
   LabelSetMaxFrames: TLabel;
   MaxFramesCB: TComboBox;
   DisableMIOTCB: TCheckBox;
   LabelSetMeasure: TLabel;
   MeasureValueCB: TComboBox;
   LabelTextMessage: TLabel;
   GroupBox2: TGroupBox;
   LabelTextValue: TLabel;
   EnrollPB: TProgressBar;
   LabelDLLVersion: TLabel;
   ShowBackground: TPanel;
   ShowWnd: TImage;
   LabelProgressText: TLabel;
   SelectRecName: TSaveDialog;
   OpenRecName: TOpenDialog;
   LabelVersionCompatibility: TLabel;
   VersionCompatibilityCB: TComboBox;
   procedure ButExitClick(Sender: TObject);
   procedure FormCreate(Sender: TObject);
   procedure ButCaptureClick(Sender: TObject);
   procedure ButEnrollClick(Sender: TObject);
   procedure ButVerifyClick(Sender: TObject);
   procedure ButIdentifyClick(Sender: TObject);
   procedure DetectFFCBClick(Sender: TObject);
   procedure DisableMIOTCBClick(Sender: TObject);
   procedure MaxFramesCBChange(Sender: TObject);
   procedure MeasureValueCBChange(Sender: TObject);
   procedure VersionCompatibilityCBChange(Sender: TObject);
   procedure ControlsDisable( );
   procedure ControlsEnable( );
private
   { Private declarations }
public
   { Public declarations }
   end;

var
   Main: TMain;

procedure GetFTRSDKVersion( verinfo: PChar; buflen: Integer );

implementation

{$R *.DFM}

procedure TMain.ButExitClick(Sender: TObject);
begin
   Close;
end;


//--------------------------------------
procedure TMain.FormCreate(Sender: TObject);
var
   bRet:    Boolean;
   verinfo: array[0..31] of Char;
   vertext: array[0..255] of Char;
   iCyc:    Integer;
   rCode:   FTRAPI_RESULT;
   Version: FTR_VERSION;

begin
   // set global variables
   MessageLabel := @LabelTextValue;
   boc.wTextLabel := @LabelProgressText;
   boc.wPrgBar := @EnrollPB;
   RecName := @SelectRecName;
   VrfName := @OpenRecName;
   forImage := @ShowWnd;
   bIsImgInit := False;
   pMainForm := @Main;

   // set default SDK example settings
   bRet := SetSDKDefaultSettings( );
   if bRet <> TRUE then
   begin
      ButCapture.Enabled := False;
      ButVerify.Enabled := False;
      ButEnroll.Enabled := False;
      ButIdentify.Enabled := False;
      CaptureToFileRB.Enabled := False;
      CaptureToScreenGB.Enabled := False;
      DetectFFCB.Enabled := False;
      MaxFramesCB.Enabled := False;
      VersionCompatibilityCB.Enabled := False;
      DisableMIOTCB.Enabled := False;
      MeasureValueCB.Enabled := False;
      Exit;
   end;

   // set dialog controls
   if EmptyDB( ) = TRUE then
   begin
      ButVerify.Enabled := FALSE;
      ButIdentify.Enabled := FALSE;
   end;

   // set VersionCompatibilityCB control
   rCode := FTRGetParam( FTR_PARAM_VERSION, FTR_PARAM_VALUE_PTR( @Version ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
      Exit;
   end;

   if Version = FTR_VERSION_PREVIOUS then
      VersionCompatibilityCB.ItemIndex := 1
   else if Version = FTR_VERSION_CURRENT then
      VersionCompatibilityCB.ItemIndex := 2
   else VersionCompatibilityCB.ItemIndex := 3; // FTR_VERSION_CURRENT

   // set MaxFramesCB control
   MaxFramesCB.ItemIndex := 4;

   // set MeasureValueCB control
   for iCyc := 0 to M2PNUM-1 do
      MeasureValueCB.Items.Add( M2PArray[iCyc].meas );
   MeasureValueCB.ItemIndex := M2PDEFITEM;

   // get SDK version information
   ZeroMemory( @verinfo, SizeOf( verinfo ) );
   GetFTRSDKVersion( verinfo, 31 );
   StrCopy( vertext, 'FTRAPI.dll version ' );
   StrCat(  vertext, verinfo );
   LabelDLLVersion.Caption := vertext;

end;


 {******************************************************************************
  *
  * GetFTRSDKVersion - get Futronic SDK version. This is FTRAPI.dll version.
  * Syntax:
  *   procedure GetFTRSDKVersion( verinfo: PChar; buflen: Integer );
  * Argument list:
  *   verinfo (input/output)  - text buffer;
  *   buflen (input)          - text buffer size.
  *
  *}
procedure GetFTRSDKVersion( verinfo: PChar; buflen: Integer );
var
   dwZero:        DWORD;
   viLen:         Integer;
   verBuf:        Pointer;
   bRet:          Boolean;
   FI:            PVSFixedFileInfo;
   cbTranslate:   DWORD;
   HVMS, LVMS:    WORD;
   HVLS, LVLS:    WORD;

begin
   viLen := GetFileVersionInfoSize( 'FTRAPI.dll', dwZero );
   if viLen = 0 then
   begin
      StrCopy( verinfo, 'unknown version' );
      Exit;
   end;
   GetMem( verBuf, viLen );
   
   bRet := GetFileVersionInfo( 'FTRAPI.dll', dwZero, viLen, verBuf );
   if bRet = FALSE then
   begin
      StrCopy( verinfo, 'unknown version' );
      FreeMem( verBuf );
      Exit;
   end;

   bRet := VerQueryValue( verBuf, '\', Pointer( FI ), cbTranslate );
   if bRet = FALSE then
   begin
      StrCopy( verinfo, 'unknown version' );
      FreeMem( verBuf );
      Exit;
   end;

   HVMS := HIWORD( FI.dwFileVersionMS );
   LVMS := LOWORD( FI.dwFileVersionMS );
   HVLS := HIWORD( FI.dwFileVersionLS );
   LVLS := LOWORD( FI.dwFileVersionLS );
   StrCopy( verinfo, PChar( IntToStr( HVMS ) ) );
   StrCat( verinfo, '.' );
   StrCat( verinfo, PChar( IntToStr( LVMS ) ) );
   StrCat( verinfo, '.' );
   StrCat( verinfo, PChar( IntToStr( HVLS ) ) );
   StrCat( verinfo, '.' );
   StrCat( verinfo, PChar( IntToStr( LVLS ) ) );

   FreeMem( verBuf );

end;


//--------------------------------------
//
// start capture operation.
//
procedure TMain.ButCaptureClick(Sender: TObject);
begin
   ControlsDisable( );
   Capture( );
   ControlsEnable( );
end;


//--------------------------------------
//
// start enroll operation.
//
procedure TMain.ButEnrollClick(Sender: TObject);
begin
   ControlsDisable( );
   Enroll( );
   ControlsEnable( );
end;


//--------------------------------------
//
// start verify operation.
//
procedure TMain.ButVerifyClick(Sender: TObject);
begin
   ControlsDisable( );
   Verify( );
   ControlsEnable( );
end;


//--------------------------------------
//
// start identify operation.
//
procedure TMain.ButIdentifyClick(Sender: TObject);
begin
   ControlsDisable( );
   Identify( );
   ControlsEnable( );
end;


//--------------------------------------
//
// "Detect fake finger" mode was changed
//
procedure TMain.DetectFFCBClick(Sender: TObject);
var
   rCode:      FTRAPI_RESULT;
   bValue:     LongBool;
begin
   bValue := DetectFFCB.Checked;
   rCode := FTRSetParam( FTR_PARAM_FAKE_DETECT, FTR_PARAM_VALUE( bValue ) );
   if rCode <> FTR_RETCODE_OK then
      wePrintError( rCode, 1 );
end;


//--------------------------------------
//
// MIOT mode was changed
//
procedure TMain.DisableMIOTCBClick(Sender: TObject);
var
   rCode:      FTRAPI_RESULT;
   bValue:     LongBool;
begin
   bValue := DisableMIOTCB.Checked;
   rCode := FTRSetParam( FTR_PARAM_MIOT_CONTROL, FTR_PARAM_VALUE( bValue ) );
   if rCode <> FTR_RETCODE_OK then
      wePrintError( rCode, 1 );
end;


//--------------------------------------
//
// Max frames in template was changed
//
procedure TMain.MaxFramesCBChange(Sender: TObject);
var
   nFrames:    Integer;
   rCode:      FTRAPI_RESULT;
begin
   nFrames := StrToInt( MaxFramesCB.Items[MaxFramesCB.ItemIndex] );
   rCode := FTRSetParam( FTR_PARAM_MAX_MODELS, FTR_PARAM_VALUE( nFrames ) );
   if rCode <> FTR_RETCODE_OK then
      wePrintError( rCode, 1 );
end;


//--------------------------------------
//
// Version compatibility was changed
//
procedure TMain.VersionCompatibilityCBChange(Sender: TObject);
var
   Version:    FTR_VERSION;
   rCode:      FTRAPI_RESULT;
begin
   if VersionCompatibilityCB.ItemIndex = 1 then
      Version := FTR_VERSION_PREVIOUS
   else if VersionCompatibilityCB.ItemIndex = 2 then
      Version := FTR_VERSION_CURRENT
   else Version := FTR_VERSION_COMPATIBLE;

   rCode := FTRSetParam( FTR_PARAM_VERSION, FTR_PARAM_VALUE( Version ) );
   if rCode <> FTR_RETCODE_OK then
      wePrintError( rCode, 1 );
end;


//--------------------------------------
//
// Measure was changed
//
procedure TMain.MeasureValueCBChange(Sender: TObject);
var
   rCode:      FTRAPI_RESULT;
   value:      DWORD;
   rValCode:   Integer;
begin
   // Use FAR option
   {
   rCode := FTRSetParam( FTR_PARAM_MAX_FAR_REQUESTED,
               FTR_PARAM_VALUE( M2PArray[MeasureValueCB.ItemIndex].prob ) );
   if rCode <> FTR_RETCODE_OK then
      wePrintError( rCode, 1 );
   }
   // Use FARN option
   if M2PArray[MeasureValueCB.ItemIndex].meas = 'Max' then
      value := 1000
   else
      val( M2PArray[MeasureValueCB.ItemIndex].meas, value, rValCode );
   rCode := FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, FTR_PARAM_VALUE( value ) );
   if rCode <> FTR_RETCODE_OK then
   begin
      wePrintError( rCode, 1 );
   end;
end;

procedure TMain.ControlsDisable( );
begin
   ButCapture.Enabled := False;
   ButEnroll.Enabled := False;
   ButVerify.Enabled := False;
   ButIdentify.Enabled := False;
   DetectFFCB.Enabled := False;
   MaxFramesCB.Enabled := False;
   MaxFramesCB.Color := clMenu;
   MaxFramesCB.Refresh( );
   VersionCompatibilityCB.Enabled := False;
   VersionCompatibilityCB.Color := clMenu;
   VersionCompatibilityCB.Refresh( );
   DisableMIOTCB.Enabled := False;
   MeasureValueCB.Enabled := False;
   MeasureValueCB.Color := clMenu;
   MeasureValueCB.Refresh( );
   ButExit.Enabled := False;
end;

procedure TMain.ControlsEnable( );
begin
   ButCapture.Enabled := True;
   ButEnroll.Enabled := True;
   if EmptyDB( ) = False then
   begin
      ButVerify.Enabled := True;
      ButIdentify.Enabled := True;
   end
   else
   begin
      ButVerify.Enabled := False;
      ButIdentify.Enabled := False;
   end;
   DetectFFCB.Enabled := True;
   MaxFramesCB.Enabled := True;
   MaxFramesCB.Color := clWindow;
   VersionCompatibilityCB.Enabled := True;
   VersionCompatibilityCB.Color := clWindow;
   DisableMIOTCB.Enabled := True;
   MeasureValueCB.Enabled := True;
   MeasureValueCB.Color := clWindow;
   ButExit.Enabled := True;
end;

end.
