<?xml version="1.0"?><doc>
<members>
<member name="T:Futronic.SDKHelper.FutronicException" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrException.h" line="10">
<summary>
Represent errors that occur during SDK API functions execution.
</summary>
</member>
<member name="M:Futronic.SDKHelper.FutronicException.#ctor(System.Int32)" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrException.h" line="17">
<summary>
Initialize a new instance of the FutronicException class
with specified error code.
</summary>
<param name="nErrorCode">
Error code
</param>
</member>
<member name="M:Futronic.SDKHelper.FutronicException.#ctor(System.Int32,System.String)" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrException.h" line="30">
<summary>
Initialize a new instance of the FutronicException class
with specified error code and error message.
</summary>
<param name="nErrorCode">
Error code
</param>
<param name="message">
Error message
</param>
</member>
<member name="P:Futronic.SDKHelper.FutronicException.ErrorCode" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrException.h" line="45">
<summary>
Gets a error code.
</summary>
</member>
<member name="T:Futronic.SDKHelper.FTR_PROGRESS" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="14">
Data capture progress information.
</member>
<member name="F:Futronic.SDKHelper.FTR_PROGRESS.dwSize" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="17">
The size of the structure in bytes.
</member>
<member name="F:Futronic.SDKHelper.FTR_PROGRESS.dwCount" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="19">
Currently requested frame number.
</member>
<member name="F:Futronic.SDKHelper.FTR_PROGRESS.bIsRepeated" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="21">
Flag indicating whether the frame is requested not the first time.
</member>
<member name="F:Futronic.SDKHelper.FTR_PROGRESS.dwTotal" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="23">
Total number of frames to be captured.
</member>
<member name="T:Futronic.SDKHelper.FtrIdentifyRecord" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="27">
<summary>
Identification information record.
</summary>
</member>
<member name="F:Futronic.SDKHelper.FtrIdentifyRecord.KeyValue" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="32">
<summary>
The current record unique ID.
This record should be set from the main program.
</summary>
<remarks>
The maximum unique ID length is 16 bytes.
</remarks>
</member>
<member name="F:Futronic.SDKHelper.FtrIdentifyRecord.Template" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="41">
The current template.
</member>
<member name="T:Futronic.SDKHelper.OnPutOnHandler" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="49">
<summary>
The "Put your finger on the scanner" event.
</summary>
<param name="Progress">
The current progress data structure.
</param>
</member>
<member name="T:Futronic.SDKHelper.OnTakeOffHandler" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="57">
<summary>
The "Take off your finger from the scanner" event.
</summary>
<param name="Progress">
The current progress data structure.
</param>
</member>
<member name="T:Futronic.SDKHelper.UpdateScreenImageHandler" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="65">
<summary>
The "Show the current fingerprint image" event.
</summary>
<param name="Bitmap">
The instance of Bitmap class with fingerprint image.
</param>
</member>
<member name="T:Futronic.SDKHelper.OnFakeSourceHandler" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="73">
<summary>
The "Fake finger detected" event.
</summary>
<param name="Progress">
The current progress data structure.
</param>
<returns>
Returns <c>true</c> if the current indetntification operation should be aborted, otherwise is <c>false</c>
</returns>
</member>
<member name="F:Futronic.SDKHelper.FarnValues.farn_custom" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="99">
<summary>
This value cannot be used as FARnLevel parameter.
The farn_custom shows that a custom value is assigned for FAR.
</summary>
</member>
<member name="T:Futronic.SDKHelper.FarnValues" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="88">
<summary>
Contains some predefined levels for FAR (False Accepting Ratio) 
</summary>
</member>
<member name="F:Futronic.SDKHelper.EnrollmentState.ready_to_process" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="108">
<summary>
The "ready to enrollment" state. class is ready to receive a base template
and start the identification operation
</summary>
</member>
<member name="F:Futronic.SDKHelper.EnrollmentState.process_in_progress" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="114">
<summary>
Class is receiving the base template or the enrollment operation is starting
</summary>
</member>
<member name="F:Futronic.SDKHelper.EnrollmentState.ready_to_continue" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="119">
<summary>
Class is ready to start the identification operation
</summary>
</member>
<member name="F:Futronic.SDKHelper.EnrollmentState.continue_in_progress" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="124">
<summary>
The identification process is starting for this class
</summary>
</member>
<member name="T:Futronic.SDKHelper.FutronicSdkBase" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="137">
<summary>
Base class for any .NET-wrapper. It initialize and terminate the FTRAPI.dll library.
</summary>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.rgFARN" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="167">
<summary>
Contains predefined FAR values. This array must have the same size as FarnValues
without farn_custom (currently only 6 elements). You may use FarnValues values as
index of this array.
</summary>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.SdkRetCode2Message(System.Int32)" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="181">
<summary>
Gets an error description by a Futronic SDK error code.
</summary>
<param name="nRetCode">
Futronic SDK error code.
</param>
<returns>
Error description.
</returns>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_RefCount" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="284">
<summary>
Number of the FTRAPI library references.
</summary>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_InitLock" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="289">
<summary>
This object prevents more than one thread from using nRefCount simultaneously.
It also synchronize the FTRAPI library initialization/deinitialization.
</summary>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_bDispose" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="296">
<summary>
If the class is deleted by calling <c>Dispose</c>, m_bDispose is true.
</summary>
<remarks>
After of calling <c>Dispose</c>, the class cannot be used. 
The class raises the <c>ObjectDisposedException</c> exception in
the event of an invalid usage condition. 
</remarks>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_SyncRoot" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="307">
<summary>
This object synchronizes the FTRAPI.dll usage from any .NET-wrapper class.
</summary>
</member>
<member name="E:Futronic.SDKHelper.FutronicSdkBase.OnPutOn" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="313">
<summary>
The "Put your finger on the scanner" event handler.
This event should be used to interact with a user during
enrollment, identification and verification operations.
</summary>
</member>
<member name="E:Futronic.SDKHelper.FutronicSdkBase.OnTakeOff" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="320">
<summary>
The "Take off your finger from the scanner" event handler.
This event should be used to interact with a user during
enrollment, identification and verification processes.
</summary>
</member>
<member name="E:Futronic.SDKHelper.FutronicSdkBase.UpdateScreenImage" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="327">
<summary>
This event handler allows to show Bitmap with users
fingerprint.
This event should be used to interact with a user during
enrollment, identification and verification processes.
</summary>
</member>
<member name="E:Futronic.SDKHelper.FutronicSdkBase.OnFakeSource" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="335">
<summary>
The "Fake Finger Detected"  event handler. This event raises
only if <c>FakeDetection</c> and <c>FFDControl</c> properties are
<c>true</c>.
This event should be used to interact with a user during
enrollment, identification and verification processes.
</summary>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.#ctor" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="344">
<summary>
The FutronicSdkBase class constructor.
Initialize a new instance of the FutronicSdkBase class.
</summary>
<exception cref="T:Futronic.SDKHelper.FutronicException">
Error occur during SDK initialization. To get error code, see 
property ErrorCode of FutronicException class.
</exception>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.Dispose" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="388">
<summary>
Releases the unmanaged resources used by the FutronicSdkBase and optionally 
releases the managed resources.
</summary>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.OnCalcel" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="403">
<summary>
This function should be called to abort current process
(enrollment, identification etc.).
</summary>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.FakeDetection" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="412">
<summary>
The "Fake Detection" property (Read/Write).
</summary>
<remarks>
Set this property to <c>true</c>, if you want to activate Live Finger
Detection (LFD) feature during the capture process.
The capture time is increasing, when you activate the LFD feature.
The default value is <c>false</c>.
</remarks>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
<exception cref="T:System.InvalidOperationException">
You cannot change this property in the current moment.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.Version" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="444">
<summary>
ADD DESCRIPTION
</summary>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
<exception cref="T:System.InvalidOperationException">
You cannot change this property in the current moment.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.FFDControl" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="470">
<summary>
The "Fake Detection Event Handler" property (Read/Write).
</summary>
<remarks>
Set this property to <c>true</c>, if you want to receive the
"Fake Detect" event. You should also set the <c>FakeDetection</c>
property to receive this event.
The default value is <c>true</c>.
</remarks>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
<exception cref="T:System.InvalidOperationException">
You cannot change this property in the current moment.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.FARnLevel" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="502">
<summary>
The "False Accepting Ratio" property (Read/Write).
</summary>
<remarks>
You cannot use the farn_custom value to set this property. The
farn_custom value shows that a custom value is assigned.
The default value is <c>FarnValues.farn_normal</c>.
</remarks>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
<exception cref="T:System.InvalidOperationException">
You cannot change this property in the current moment.
</exception>
<exception cref="T:System.ArgumentException">
Invalid FarnLevel value. For example, you are trying to set this
property to farn_custom.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.FARN" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="540">
<summary>
The "False Accepting Ratio" property (Read/Write).
</summary>
<remarks>
You can set any valid False Accepting Ratio (FAR). 
The value must be between 1 and 1000. The larger value implies
the "softer" result.
If you set one from FarnValues values, FARnLevel sets to the
appropriate level.
</remarks>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
<exception cref="T:System.InvalidOperationException">
You cannot change this property in the current moment.
</exception>
<exception cref="T:System.ArgumentOutOfRangeException">
Invalid FARN value. The FARN value must be between 1 and 1000.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.IsTrial" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="587">
<summary>
Gets a value that indicates whether a library is trial version.
</summary>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.IdentificationsLeft" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="605">
<summary>
Gets a value that specify identification limit value. If property contains -1 that is "no limits"
</summary>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
</member>
<member name="P:Futronic.SDKHelper.FutronicSdkBase.FastMode" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="627">
<summary>
The "Fast mode" property (Read/Write).
</summary>
<remarks>
Set this property to <c>true</c>, if you want to use fast mode.
The default value is <c>false</c>.
</remarks>
<exception cref="T:System.ObjectDisposedException">
The class instance is disposed. Any calls are prohibited.
</exception>
<exception cref="T:System.InvalidOperationException">
You cannot change this property in the current moment.
</exception>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.cbControl(Futronic.SDKHelper.FTR_PROGRESS,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.IntPtr)" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="657">
<summary>
State callback function. It's called from unmanaged code.
</summary>
<param name="StateMask">
a bit mask indicating what arguments are provided
</param>
<param name="Signal">
this signal should be used to interact with a user
</param>
<param name="BitmapWidth">
This parameter contain a width of the bitmap to be displayed.
</param>
<param name="BitmapHeight">
This parameter contain a height of the bitmap to be displayed.
</param>
<param name="pBitmap">
This parameter contain a pointer to the bitmap to be displayed
</param>
<returns>
API function execution control code
</returns>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.Finalize" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="756">
<summary>
The Finalize method.
</summary>
<remarks>
Decrements the reference count for the library.
If the reference count on the library falls to 0, the SDK library
is uninitialized.
</remarks>
</member>
<member name="M:Futronic.SDKHelper.FutronicSdkBase.CheckDispose" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="805">
<summary>
If the class is disposed, this function raises an exception.
</summary>
<remarks>
This function must be called before any operation in all functions.
</remarks>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_bFakeDetection" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="819">
<summary>
<c>true</c> if the library should activate Live Finger
Detection (LFD) feature. You cannot modify this variable
directly. Use the FakeDetection property.
The default value is <c>false</c>.
</summary>
<seealso cref="P:Futronic.SDKHelper.FutronicSdkBase.FakeDetection"/>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_bFFDControl" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="828">
<summary>
<c>true</c> if the library should raise the "Fake Detection Event Handler".
You cannot modify this variable directly. Use the FFDControl property.
The default value is <c>true</c>.
</summary>
<seealso cref="P:Futronic.SDKHelper.FutronicSdkBase.FFDControl"/>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_bCancel" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="836">
<summary>
<c>true</c> if the library should abort current process.
You cannot modify this variable directly. Use the OnCancel property.
</summary>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_FarnLevel" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="842">
<summary>
Current False Accepting Ratio value. Contains only one of
predefined values.
The default value is <c>FarnValues.farn_normal</c>.
</summary>
<seealso cref="P:Futronic.SDKHelper.FutronicSdkBase.FARnLevel"/>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_FARN" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="850">
<summary>
Current False Accepting Ratio value. It may contains any valid
value.
</summary>
<seealso cref="P:Futronic.SDKHelper.FutronicSdkBase.FARN"/>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_bFastMode" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="857">
<summary>
Fast mode property
</summary>
<seealso cref="P:Futronic.SDKHelper.FutronicSdkBase.FastMode"/>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_WorkedThread" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="863">
<summary>
Pointer to the operation thread: capture, enrollment etc.
</summary>
</member>
<member name="F:Futronic.SDKHelper.FutronicSdkBase.m_State" decl="false" source="C:\Projects\CyroTech\Biometrics\CyroTechBiometric\App\Net\ScannerApp\ftrSDKHelper\ftrSDKHelper.h" line="868">
<summary>
Current state for the class.
</summary>
</member>
</members>
</doc>