
 {*****************************************************************************
  *
  * WorkedExBD    - worked example using Futronic SDK.
  *
  * Include.pas   - project common data.
  *
  *}

unit Includes;

interface

uses
   Windows, ComCtrls, StdCtrls, Dialogs, ExtCtrls, Forms, SDK_API;

type
// callback function contex
BIOPERCONTEXT  = packed record
   oType:      Integer;                // current biometric operation
   wPrgBar:    ^TProgressBar;          // progress bar window. Use with
                                       // enroll operation
   wTextLabel: ^TLabel;                // text window. Use with enroll operation
end;
LPBIOPERCONTEXT= ^BIOPERCONTEXT;

// Measure <--> probability array record
M2PREC         = packed record
   prob:       DWORD;                  // probability value
   meas:       PChar;                  // measure value
end;
LPM2PREC       = ^M2PREC;

// database initial settings
DBSET          = packed record
   dbFolder:   array[0..MAX_PATH-1] of Char;    // database folder root
   dbImages:   array[0..MAX_PATH-1] of Char;    // fingerprint images folder name
   dbTemplates:array[0..MAX_PATH-1] of Char;    // templates folder name
end;
LPDBSET        = ^DBSET;

// database record in memory
DBREC          = packed record
   key:        FTR_DATA_KEY;           // unique key within DB
   len:        DWORD;                  // size of data
   data:       Pointer;                // record's data
end;
LPDBREC        = ^DBREC;

const
// M2PArray records number
M2PNUM:        DWORD = 25;
// default array item
M2PDEFITEM:    DWORD = 10;

// Measure <--> probability array
// See commentary for according FAR value
M2PArray:      array[0..24] of M2PREC =
(
   ( prob:  738151462; meas:  '  1' ), // 0,343728560
   ( prob:  638070147; meas:  ' 16' ), // 0,297124566
   ( prob:  427995129; meas:  ' 31' ), // 0,199300763
   ( prob:  206984582; meas:  ' 49' ), // 0,096384707
   ( prob:  104396832; meas:  ' 63' ), // 0,048613563
   ( prob:   20854379; meas:  ' 95' ), // 0,009711077
   ( prob:   10620511; meas:  '107' ), // 0,004945561
   ( prob:    2066214; meas:  '130' ), // 0,000962156
   ( prob:    1002950; meas:  '136' ), // 0,000467035
   ( prob:     207859; meas:  '155' ), // 0,000096792
   ( prob:     103930; meas:  '166' ), // 0,000048396
   ( prob:      21002; meas:  '190' ), // 0,000009780
   ( prob:       9694; meas:  '199' ), // 0,000004514
   ( prob:       1885; meas:  '221' ), // 0,000000878
   ( prob:        807; meas:  '230' ), // 0,000000376
   ( prob:        256; meas:  '245' ), // 0,000000119209 (0/129)
   ( prob:        128; meas:  '265' ), // 0,000000059605 (0/153)
   ( prob:         64; meas:  '286' ), // 0,000000029802 (0/174)
   ( prob:         32; meas:  '305' ), // 0,000000014901 (0/205)
   ( prob:         16; meas:  '325' ), // 0,000000007451 (0/231)
   ( prob:          8; meas:  '345' ), // 0,000000003725 (0/294)
   ( prob:          4; meas:  '365' ), // 0,000000001863 (0/362)
   ( prob:          2; meas:  '385' ), // 0,000000000931 (0/439)
   ( prob:          1; meas:  '405' ), // 0,000000000466 (0/542)
   ( prob:          0; meas:  'Max' )  // Maximum possible measure value should
                                       // be placed here.
);

// biometric operation types
BO_CAPTURE:    Integer = 0;            // capture operation
BO_ENROLL:     Integer = 1;            // enroll --"--
BO_VERIFY:     Integer = 2;            // verify --"--
BO_IDENTIFY:   Integer = 3;            // identify --"--

var
   dbs:        DBSET;                  // DB settings
   RecName:    ^TSaveDialog;
   VrfName:    ^TOpenDialog;
   forImage:   ^TImage;
   bIsImgInit: Boolean;
   pMainForm:  ^TForm;

implementation

end.
