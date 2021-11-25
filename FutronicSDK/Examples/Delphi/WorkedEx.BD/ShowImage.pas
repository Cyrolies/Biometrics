
 {******************************************************************************
  *
  * WorkedExBD    - worked example using Futronic SDK.
  *
  * ShowImage.pas - display fingerprint image.
  *
  *}

unit ShowImage;

interface

uses
   Windows, ExtCtrls, SysUtils, Graphics,
   SDK_API, Errors, Includes;

procedure UpdateImage( pBitmap: FTR_BITMAP_PTR );

implementation

procedure UpdateImage( pBitmap: FTR_BITMAP_PTR );
type
  TRGBQuadArray = ARRAY[Word] of TRGBQuad;
  pTRGBQuadArray = ^TRGBQuadArray;
var
   imgPixels: PByteArray;
   x,y, i : Integer;
   P : pTRGBQuadArray;
begin
   imgPixels := pBitmap.Bitmap.pData;

   if bIsImgInit = False then
   begin
      bIsImgInit := true;
      forImage.Picture.Bitmap.Width := pBitmap.Width;
      forImage.Picture.Bitmap.Height := pBitmap.Height;
      forImage.Picture.Bitmap.PixelFormat := pf32bit;
   end;

   i := 0;
   for y := 0 to forImage.Picture.Bitmap.Height - 1 do
   begin
       P := forImage.Picture.Bitmap.ScanLine[y];
       for x := 0 to forImage.Picture.Bitmap.Width-1 do
       begin
         P[x].rgbRed := Byte(imgPixels[i]);
         P[x].rgbGreen := Byte(imgPixels[i]);
         P[x].rgbBlue := Byte(imgPixels[i]);
         Inc( i );
       end;
   end;

   forImage.Refresh( );

end;

end.
