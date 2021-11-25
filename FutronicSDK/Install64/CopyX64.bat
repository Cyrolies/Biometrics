rem Copy binary libraries to system directory
copy /Y "%~dp0\..\Redist\x64\FTRAPI.dll" "%SYSTEMROOT%\System32"
copy /Y "%~dp0\..\Redist\x64\ftrScanAPI.dll" "%SYSTEMROOT%\System32"
