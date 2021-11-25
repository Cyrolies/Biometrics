// ftrSDKHelper.h

#pragma once

using namespace System;
using namespace System::Threading;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;

#include "ftrException.h"

namespace Futronic {
namespace SDKHelper {

    /// Data capture progress information.
    [StructLayout(LayoutKind::Sequential)]
    public __value struct MFTR_PROGRESS
    {
        /// The size of the structure in bytes.
        DWORD dwSize;
        /// Currently requested frame number.
        UInt32      dwCount;
        /// Flag indicating whether the frame is requested not the first time.
        BOOL  bIsRepeated;
        /// Total number of frames to be captured.
        UInt32      dwTotal;
    };

    ///<summary>
    /// Identification information record.
    ///</summary>
    public __value struct FtrIdentifyRecord
    {
        ///<summary>
        /// The current record unique ID.
        /// This record should be set from the main program.
        ///</summary>
        ///<remarks>
        /// The maximum unique ID length is 16 bytes.
        ///</remarks>
        Byte    KeyValue __gc [];

        /// The current template.
        Byte    Template __gc [];
    };

    ///////////////////////////////////////////////////////////////////
    // Base delegates for all Futronic .NET-wrapper classes.
    ///////////////////////////////////////////////////////////////////

    ///<summary>
    /// The "Put your finger on the scanner" __event.
    ///</summary>
    ///<param name="Progress">
    /// The current progress data structure.
    ///</param>
    public __delegate void OnPutOnHandler( MFTR_PROGRESS Progress );

    ///<summary>
    /// The "Take off your finger from the scanner" __event.
    ///</summary>
    ///<param name="Progress">
    /// The current progress data structure.
    ///</param>
    public __delegate void OnTakeOffHandler( MFTR_PROGRESS Progress );

    ///<summary>
    /// The "Show the current fingerprint image" __event.
    ///</summary>
    ///<param name="Bitmap">
    /// The instance of Bitmap class with fingerprint image.
    ///</param>
    public __delegate void UpdateScreenImageHandler( System::Drawing::Bitmap *Bitmap );

    ///<summary>
    /// The "Fake finger detected" __event.
    ///</summary>
    ///<param name="Progress">
    /// The current progress data structure.
    ///</param>
    ///<returns>
    /// Returns <c>true</c> if the current indetntification operation should be aborted, otherwise is <c>false</c>
    ///</returns>
    public __delegate bool OnFakeSourceHandler( MFTR_PROGRESS Progress );

    private __delegate UInt32 MyCallBackHandler( MFTR_PROGRESS Progress,
                                                 UInt32 StateMask, UInt32 Signal, 
                                                 UInt32 BitmapWidth, UInt32 BitmapHeight,
                                                 IntPtr pBitmap );
    ///<summary>
    /// Contains some predefined levels for FAR (False Accepting Ratio) 
    ///</summary>
    public __value enum FarnValues
    {
        farn_low            = 0,
        farn_below_normal   = 1,
        farn_normal         = 2,
        farn_above_normal   = 3,
        farn_high           = 4,
        farn_max            = 5,
        ///<summary>
        /// This value cannot be used as FARnLevel parameter.
		/// The farn_custom shows that a custom value is assigned for FAR.
        ///</summary>
        farn_custom         = 255
    };

    public __value enum EnrollmentState
    {
        ///<summary>
        /// The "ready to enrollment" state. class is ready to receive a base template
        /// and start the identification operation
        ///</summary>
        ready_to_process = 0,

        ///<summary>
        /// Class is receiving the base template or the enrollment operation is starting
        ///</summary>
        process_in_progress = 1,

        ///<summary>
        /// Class is ready to start the identification operation
        ///</summary>
        ready_to_continue = 2,

        ///<summary>
        /// The identification process is starting for this class
        ///</summary>
        continue_in_progress = 3
    };

    public __value enum VersionCompatible
    {
        ftr_version_previous = FTR_VERSION_PREVIOUS,
        ftr_version_current = FTR_VERSION_CURRENT,
        ftr_version_compatible = FTR_VERSION_COMPATIBLE
    };

    ///<summary>
    /// Base class for any .NET-wrapper. It initialize and terminate the FTRAPI.dll library.
    ///</summary>
    public __abstract __gc class FutronicSdkBase : public IDisposable
    {
    public:
        static const int RETCODE_OK = FTR_RETCODE_OK;
        static const int RETCODE_NO_MEMORY = FTR_RETCODE_NO_MEMORY;
        static const int RETCODE_INVALID_ARG = FTR_RETCODE_INVALID_ARG;
        static const int RETCODE_ALREADY_IN_USE = FTR_RETCODE_ALREADY_IN_USE;
        static const int RETCODE_INVALID_PURPOSE = FTR_RETCODE_INVALID_PURPOSE;
        static const int RETCODE_INTERNAL_ERROR = FTR_RETCODE_INTERNAL_ERROR;

        static const int RETCODE_UNABLE_TO_CAPTURE = FTR_RETCODE_UNABLE_TO_CAPTURE;
        static const int RETCODE_CANCELED_BY_USER = FTR_RETCODE_CANCELED_BY_USER;
        static const int RETCODE_NO_MORE_RETRIES = FTR_RETCODE_NO_MORE_RETRIES;
        static const int RETCODE_INCONSISTENT_SAMPLING = FTR_RETCODE_INCONSISTENT_SAMPLING;

        static const int RETCODE_FRAME_SOURCE_NOT_SET = FTR_RETCODE_FRAME_SOURCE_NOT_SET;
        static const int RETCODE_DEVICE_NOT_CONNECTED = FTR_RETCODE_DEVICE_NOT_CONNECTED;
        static const int RETCODE_DEVICE_FAILURE = FTR_RETCODE_DEVICE_FAILURE;
        static const int RETCODE_EMPTY_FRAME = FTR_RETCODE_EMPTY_FRAME;
        static const int RETCODE_FAKE_SOURCE = FTR_RETCODE_FAKE_SOURCE;
        static const int RETCODE_INCOMPATIBLE_HARDWARE = FTR_RETCODE_INCOMPATIBLE_HARDWARE;
        static const int RETCODE_INCOMPATIBLE_FIRMWARE = FTR_RETCODE_INCOMPATIBLE_FIRMWARE;
        static const int RETCODE_TRIAL_EXPIRED = FTR_RETCODE_TRIAL_EXPIRED;
        static const int RETCODE_FRAME_SOURCE_CHANGED = FTR_RETCODE_FRAME_SOURCE_CHANGED;
        static const int RETCODE_INCOMPATIBLE_SOFTWARE = FTR_RETCODE_INCOMPATIBLE_SOFTWARE;


        ///<summary>
        /// Contains predefined FAR values. This array must have the same size as FarnValues
		/// without farn_custom (currently only 6 elements). You may use FarnValues values as
		/// index of this array.
        ///</summary>
        static Int32 rgFARN[] = 
        {
            1,          // 738151462: 0,343728560
            95,         //  20854379: 0,009711077
            166,        //    103930: 0,000048396
            245,        //       256: 0,000000119209
            345,        //         8: 0,000000003725
            405         //         1: 0,000000000466
        };

        ///<summary>
        /// Gets an error description by a Futronic SDK error code.
        ///</summary>
        ///<param name="nRetCode">
        /// Futronic SDK error code.
        ///</param>
        ///<returns>
        /// Error description.
        ///</returns>
        static String* SdkRetCode2Message(int nRetCode)
        {
            String* szMessage;
            switch (nRetCode)
            {
            case FTR_RETCODE_OK:
                szMessage = __gc new String( S"The function is completed successfully." );
                break;

            case FTR_RETCODE_NO_MEMORY:
                szMessage = __gc new String( S"There is not enough memory to continue the execution of a program." );
                break;

            case FTR_RETCODE_INVALID_ARG:
                szMessage = __gc new String( S"Some parameters were not specified or had invalid values.");
                break;

            case FTR_RETCODE_ALREADY_IN_USE:
                szMessage = __gc new String( S"The current operation has already initialized the API." );
                break;

            case FTR_RETCODE_INVALID_PURPOSE:
                szMessage = __gc new String( S"Base template is not correspond purpose.");
                break;

            case FTR_RETCODE_INTERNAL_ERROR:
                szMessage = __gc new String( S"Internal SDK or Win32 API system error.");
                break;

            case FTR_RETCODE_UNABLE_TO_CAPTURE:
                szMessage = __gc new String( S"Unable to capture." );
                break;

            case FTR_RETCODE_CANCELED_BY_USER:
                szMessage = __gc new String( S"User canceled operation." );
                break;

            case FTR_RETCODE_NO_MORE_RETRIES:
                szMessage = __gc new String( S"Number of retries is overflow." );
                break;

            case FTR_RETCODE_INCONSISTENT_SAMPLING:
                szMessage = __gc new String( S"Source sampling is inconsistent." );
                break;

            case FTR_RETCODE_FRAME_SOURCE_NOT_SET:
                szMessage = __gc new String( S"Frame source not set." );
                break;

            case FTR_RETCODE_DEVICE_NOT_CONNECTED:
                szMessage = __gc new String( S"The frame source device is not connected." );
                break;

            case FTR_RETCODE_DEVICE_FAILURE:
                szMessage = __gc new String( S"Device failure." );
                break;

            case FTR_RETCODE_EMPTY_FRAME:
                szMessage = __gc new String( S"Empty frame." );
                break;

            case FTR_RETCODE_FAKE_SOURCE:
                szMessage = __gc new String( S"Fake source." );
                break;

            case FTR_RETCODE_INCOMPATIBLE_HARDWARE:
                szMessage = __gc new String( S"Incompatible hardware." );
                break;

            case FTR_RETCODE_INCOMPATIBLE_FIRMWARE:
                szMessage = __gc new String( S"Incompatible firmware." );
                break;

            case FTR_RETCODE_TRIAL_EXPIRED:
                szMessage = __gc new String( S"Trial limitation - only 1000 templates may be verified/identified." );
                break;

            case FTR_RETCODE_FRAME_SOURCE_CHANGED:
                szMessage = __gc new String( S"Frame source has been changed." );
                break;

            case FTR_RETCODE_INCOMPATIBLE_SOFTWARE:
                szMessage = __gc new String( S"FTR_RETCODE_INCOMPATIBLE_SOFTWARE." );
                break;

            default:
                szMessage = String::Format( S"Unknown error code {0}.", __box( nRetCode ) );
                break;
            }

            return szMessage;
        }

    private:
        ///<summary>
        /// Number of the FTRAPI library references.
        ///</summary>
        static int m_RefCount = 0;

        ///<summary>
        /// This object prevents more than one thread from using nRefCount simultaneously.
        /// It also synchronize the FTRAPI library initialization/deinitialization.
        ///</summary>
        static Object *m_InitLock = __gc new Object();

    protected:
        ///<summary>
        /// If the class is deleted by calling <c>Dispose</c>, m_bDispose is true.
        ///</summary>
        ///<remarks>
        /// After of calling <c>Dispose</c>, the class cannot be used. 
        /// The class raises the <c>ObjectDisposedException</c> exception in
		/// the __event of an invalid usage condition. 
        ///</remarks>
        bool m_bDispose;

    public:
        ///<summary>
        /// This object synchronizes the FTRAPI.dll usage from any .NET-wrapper class.
        ///</summary>
        static Object *m_SyncRoot = __gc new Object();

    public:
        ///<summary>
        /// The "Put your finger on the scanner" __event handler.
		/// This __event should be used to interact with a user during
		/// enrollment, identification and verification operations.
        ///</summary>
        __event OnPutOnHandler * OnPutOn;

        ///<summary>
		/// The "Take off your finger from the scanner" __event handler.
		/// This __event should be used to interact with a user during
		/// enrollment, identification and verification processes.
        ///</summary>
        __event OnTakeOffHandler * OnTakeOff;

        ///<summary>
        /// This __event handler allows to show Bitmap with users
		/// fingerprint.
		/// This __event should be used to interact with a user during
		/// enrollment, identification and verification processes.
        ///</summary>
        __event UpdateScreenImageHandler * UpdateScreenImage;

        ///<summary>
        /// The "Fake Finger Detected"  __event handler. This __event raises
		/// only if <c>FakeDetection</c> and <c>FFDControl</c> properties are
		/// <c>true</c>.
		/// This __event should be used to interact with a user during
		/// enrollment, identification and verification processes.
        ///</summary>
        __event OnFakeSourceHandler * OnFakeSource;

        ///<summary>
        /// The FutronicSdkBase class constructor.
        /// Initialize a new instance of the FutronicSdkBase class.
        ///</summary>
        ///<exception cref="FutronicException">
        /// Error occur during SDK initialization. To get error code, see 
        /// property ErrorCode of FutronicException class.
        ///</exception>
        FutronicSdkBase()
        {
            m_bDispose = false;

            Monitor::Enter( m_InitLock );

            try
            {
                if( m_RefCount == 0)
                {
                    FTRAPI_RESULT   nResult;
                    nResult = FTRInitialize();
                    if( nResult != FTR_RETCODE_OK )
                    {
                        Trace::Assert( nResult != FTR_RETCODE_ALREADY_IN_USE );
                        __gc new FutronicException( nResult, SdkRetCode2Message( nResult ) );
                    }
                }

                m_RefCount++;
            }
            __finally
            {
                Monitor::Exit( m_InitLock );
            }

            m_bFakeDetection = false;
            m_bFFDControl = true;
            m_FarnLevel = FarnValues::farn_normal;
            m_WorkedThread = nullptr;
            m_State = EnrollmentState::ready_to_process;
            m_FARN = rgFARN[ (int)m_FarnLevel ];
			m_Version = VersionCompatible::ftr_version_current;
            m_bFastMode = false;
        }

        ///<summary>
        /// Releases the unmanaged resources used by the FutronicSdkBase and optionally 
        /// releases the managed resources.
        ///</summary>
		virtual void Dispose ()
        {
            if( m_bDispose )
                return;

            this->~FutronicSdkBase();
            m_bDispose = true;

            GC::SuppressFinalize( this );
        }

        ///<summary>
        /// This function should be called to abort current process
		/// (enrollment, identification etc.).
        ///</summary>
        void OnCalcel()
        {
            m_bCancel = true;
        }

        ///<summary>
        /// Get the "Fake Detection" property.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        __property bool get_FakeDetection()
        {
            CheckDispose();
            return m_bFakeDetection;
        }

        ///<summary>
        /// Set the "Fake Detection" property.
        ///</summary>
        ///<remarks>
		/// Set this property to <c>true</c>, if you want to activate Live Finger
		/// Detection (LFD) feature during the capture process.
		/// The capture time is increasing, when you activate the LFD feature.
        /// The default value is <c>false</c>.
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
        /// You cannot change this property in the current moment.
        ///</exception>
        __property void set_FakeDetection( bool bFakeDetection )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            m_bFakeDetection = bFakeDetection;
        }

        ///<summary>
		/// Get the "Fake Detection Event Handler" property.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property bool get_FFDControl()
        {
            CheckDispose();
            return m_bFFDControl;
        }

        ///<summary>
        /// ADD DESCRIPTION
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
        /// You cannot change this property in the current moment.
        ///</exception>
        __property void set_Version( VersionCompatible Version )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
			m_Version = Version;
        }

        ///<summary>
        /// ADD DESCRIPTION
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property VersionCompatible get_Version()
        {
            CheckDispose();
            return m_Version;
        }

        ///<summary>
		/// Set the "Fake Detection Event Handler" property.
        ///</summary>
        ///<remarks>
		/// Set this property to <c>true</c>, if you want to receive the
		/// "Fake Detect" __event. You should also set the <c>FakeDetection</c>
		/// property to receive this __event.
		/// The default value is <c>true</c>.
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
		///<exception cref="InvalidOperationException">
		/// You cannot change this property in the current moment.
        ///</exception>
        __property void set_FFDControl( bool bFFDControl )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            m_bFFDControl = bFFDControl;
        }

        ///<summary>
		/// Get the "False Accepting Ratio" property.
        ///</summary>
		///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property FarnValues get_FARnLevel()
        {
            CheckDispose();
            return m_FarnLevel;
        }

        ///<summary>
		/// Set the "False Accepting Ratio" property.
        ///</summary>
        ///<remarks>
		/// You cannot use the farn_custom value to set this property. The
		/// farn_custom value shows that a custom value is assigned.
		/// The default value is <c>FarnValues.farn_normal</c>.
        ///</remarks>
		///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
		///<exception cref="InvalidOperationException">
		/// You cannot change this property in the current moment.
		///</exception>
        ///<exception cref="ArgumentException">
        /// Invalid FarnLevel value. For example, you are trying to set this
		/// property to farn_custom.
        ///</exception>
        __property void set_FARnLevel( FarnValues FarnLevel )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            if( (int)FarnLevel > rgFARN->Length )
                throw __gc new ArgumentException();
            m_FarnLevel = FarnLevel;
            m_FARN = rgFARN[ (int)m_FarnLevel ];
        }

		///<summary>
		/// Get the "False Accepting Ratio" property.
        ///</summary>
		///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property int get_FARN()
        {
            CheckDispose();
            return m_FARN;
        }

		///<summary>
		/// Set the "False Accepting Ratio" property.
        ///</summary>
        ///<remarks>
		/// You can set any valid False Accepting Ratio (FAR). 
		/// The value must be between 1 and 1000. The larger value implies
		/// the "softer" result.
		/// If you set one from FarnValues values, FARnLevel sets to the
		/// appropriate level.
        ///</remarks>
		///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
		///<exception cref="InvalidOperationException">
		/// You cannot change this property in the current moment.
		///</exception>
        ///<exception cref="ArgumentOutOfRangeException">
		/// Invalid FARN value. The FARN value must be between 1 and 1000.
        ///</exception>
        __property void set_FARN( int value )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            if( value < 1 || value > 1000 )
                throw __gc new ArgumentOutOfRangeException();
            m_FarnLevel = FarnValues::farn_custom;
            for( int i = 0; i < rgFARN->Length; i++)
            {
                if( rgFARN[i] == value )
                {
                    m_FarnLevel = (FarnValues)i;
                    break;
                }
            }
            m_FARN = value;
        }

        ///<summary>
		/// Get the "Fast mode" property.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property bool get_FastMode()
        {
            CheckDispose();
            return m_bFastMode;
        }

        ///<summary>
		/// Set the "Fast mode" property.
        ///</summary>
        ///<remarks>
		/// Set this property to <c>true</c>, if you want to use fast mode.
		/// The default value is <c>false</c>.
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
		///<exception cref="InvalidOperationException">
		/// You cannot change this property in the current moment.
        ///</exception>
        __property void set_FastMode( bool bFastMode )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            m_bFastMode = bFastMode;
        }

        ///<summary>
		/// Gets a value that indicates whether a library is trial version.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property bool get_IsTrial()
        {
            CheckDispose();
            bool bResult;
            DGT32 nIdentificationsLeft;
            bResult = ( FTRGetParam( FTR_PARAM_CHECK_TRIAL, reinterpret_cast<FTR_PARAM_VALUE*>(&nIdentificationsLeft) ) == FTR_RETCODE_OK && nIdentificationsLeft >= 0 );
            return bResult;
        }

        ///<summary>
		/// Gets a value that specify identification limit value. If property contains -1 that is "no limits"
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
		///</exception>
        __property int get_IdentificationsLeft()
        {
            CheckDispose();
            bool bResult;
            DGT32 nIdentificationsLeft;
            bResult = ( FTRGetParam( FTR_PARAM_CHECK_TRIAL, reinterpret_cast<FTR_PARAM_VALUE*>(&nIdentificationsLeft) ) == FTR_RETCODE_OK && nIdentificationsLeft >= 0 );
            if( bResult )
            {
                return nIdentificationsLeft;
            }
            return Int32::MaxValue;
        }

        ///<summary>
		/// State callback function. It's called from unmanaged code.
        ///</summary>
        ///<param name="StateMask">
        /// a bit mask indicating what arguments are provided
        ///</param>
        ///<param name="Signal">
        /// this signal should be used to interact with a user
        ///</param>
        ///<param name="BitmapWidth">
        /// This parameter contain a width of the bitmap to be displayed.
        ///</param>
        ///<param name="BitmapHeight">
        /// This parameter contain a height of the bitmap to be displayed.
        ///</param>
        ///<param name="pBitmap">
        /// This parameter contain a pointer to the bitmap to be displayed
        ///</param>
        ///<returns>
        /// API function execution control code
        ///</returns>
        UInt32 cbControl( MFTR_PROGRESS Progress, UInt32 StateMask, UInt32 Signal,
                          UInt32 BitmapWidth, UInt32 BitmapHeight,
                          IntPtr pBitmap )
        {
            unsigned int    nRetCode = FTR_CONTINUE;

	        if( (StateMask & FTR_STATE_SIGNAL_PROVIDED) != 0 )
	        {
		        switch( Signal )
		        {
		        case FTR_SIGNAL_TOUCH_SENSOR:
                    OnPutOn( Progress );
			        break;

		        case FTR_SIGNAL_TAKE_OFF:
                    OnTakeOff( Progress );
			        break;

		        case FTR_SIGNAL_FAKE_SOURCE:
                    if( OnFakeSource( Progress ) )
                        nRetCode = FTR_CANCEL;
			        break;

		        case FTR_SIGNAL_UNDEFINED:
                    Trace::Assert( false );
			        break;
		        }
	        }

	        if( (StateMask & FTR_STATE_FRAME_PROVIDED) != 0 )
	        {
                int length = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) +
                             sizeof(RGBQUAD)*255 + BitmapWidth * BitmapHeight;
                Byte BmpData[] = __gc new Byte [ length ];
                Byte __pin *pBmpData = &BmpData[0];
                BITMAPFILEHEADER *pFileHeader = (BITMAPFILEHEADER *)pBmpData;
                BITMAPINFO *pBmpInfoHeader = (BITMAPINFO*)(pBmpData + sizeof( BITMAPFILEHEADER ) );
                RGBQUAD *pColorTable = pBmpInfoHeader->bmiColors;

                pFileHeader->bfType = MAKEWORD( 'B', 'M' );
                pFileHeader->bfSize = length;
                pFileHeader->bfOffBits = sizeof( BITMAPFILEHEADER ) + sizeof( BITMAPINFO ) + sizeof(RGBQUAD)*255;

                pBmpInfoHeader->bmiHeader.biSize          = sizeof( BITMAPINFOHEADER );
                pBmpInfoHeader->bmiHeader.biWidth         = BitmapWidth;
                pBmpInfoHeader->bmiHeader.biHeight        = BitmapHeight;
                pBmpInfoHeader->bmiHeader.biPlanes        = 1;
                pBmpInfoHeader->bmiHeader.biBitCount      = 8;
                pBmpInfoHeader->bmiHeader.biCompression   = BI_RGB;

                for( int iCyc = 0; iCyc < 256; iCyc++ )
                {
                    pColorTable[iCyc].rgbBlue = pColorTable[iCyc].rgbGreen =
                    pColorTable[iCyc].rgbRed = (BYTE)iCyc;
                }

                memcpy( (void*)(pBmpData+pFileHeader->bfOffBits), (void*)pBitmap.ToInt32(), BitmapWidth * BitmapHeight );

                System::IO::MemoryStream *BmpStream = __gc new System::IO::MemoryStream( BmpData );

                System::Drawing::Bitmap *hBmp = __gc new System::Drawing::Bitmap( BmpStream ); 
                hBmp->RotateFlip( System::Drawing::RotateFlipType::Rotate180FlipX);

                UpdateScreenImage( hBmp );
	        }

            if( m_bCancel )
            {
                nRetCode = FTR_CANCEL;
                m_bCancel = false;
            }

            return nRetCode;
        }

        ///<summary>
        /// The Finalize method.
        ///</summary>
        ///<remarks>
		/// Decrements the reference count for the library.
		/// If the reference count on the library falls to 0, the SDK library
        /// is uninitialized.
        ///</remarks>
        virtual ~FutronicSdkBase()
        {
            if( m_bDispose )
                return;
            if( AppDomain::CurrentDomain->IsFinalizingForUnload() )
            {
                m_RefCount--;
                if( m_RefCount == 0 )
                    FTRTerminate();
            } else {
                try
                {
                    if( (m_WorkedThread != nullptr) && m_WorkedThread->get_IsAlive() )
                    {
                        m_bCancel = true;
                        if( !m_WorkedThread->Join( 3000 ) )
                            m_WorkedThread->Abort();
                        m_WorkedThread = nullptr;
                    }
                }
                catch( ThreadStateException *ex )
                {
                    System::Diagnostics::Debug::WriteLine( ex->get_Message() );
                    m_WorkedThread = nullptr;
                }

                Monitor::Enter( m_InitLock );
                Trace::Assert( m_RefCount > 0 );
                m_RefCount--;

                if( m_RefCount == 0 )
                    FTRTerminate();

                Monitor::Exit( m_InitLock );
            }
        }

    protected:

        ///<summary>
		/// If the class is disposed, this function raises an exception.
        ///</summary>
        ///<remarks>
		/// This function must be called before any operation in all functions.
        ///</remarks>
        void CheckDispose()
        {
            if( m_bDispose )
            {
                throw __gc new ObjectDisposedException( this->GetType()->FullName );
            }
        }

        ///<summary>
		/// <c>true</c> if the library should activate Live Finger
		/// Detection (LFD) feature. You cannot modify this variable
		/// directly. Use the FakeDetection property.
		/// The default value is <c>false</c>.
        ///</summary>
        ///<seealso cref="FakeDetection"/>
        bool m_bFakeDetection;

        ///<summary>
		/// <c>true</c> if the library should raise the "Fake Detection Event Handler".
		/// You cannot modify this variable directly. Use the FFDControl property.
		/// The default value is <c>true</c>.
        ///</summary>
        ///<seealso cref="FFDControl"/>
        bool m_bFFDControl;

        ///<summary>
		/// <c>true</c> if the library should abort current process.
		/// You cannot modify this variable directly. Use the OnCancel property.
        ///</summary>
        bool m_bCancel;

        ///<summary>
		/// Current False Accepting Ratio value. Contains only one of
		/// predefined values.
		/// The default value is <c>FarnValues.farn_normal</c>.
        ///</summary>
        ///<seealso cref="FARnLevel"/>
        FarnValues  m_FarnLevel;

        ///<summary>
		/// Current False Accepting Ratio value. It may contains any valid
		/// value.
        ///</summary>
        ///<seealso cref="FARN"/>
        Int32       m_FARN;

        ///<summary>
		/// Fast mode property
        ///</summary>
        ///<seealso cref="FastMode"/>
        bool m_bFastMode;

		///<summary>
		/// Pointer to the operation thread: capture, enrollment etc.
        ///</summary>
        Thread      *m_WorkedThread;

        ///<summary>
		/// Current state for the class.
        ///</summary>
        EnrollmentState m_State;

        VersionCompatible   m_Version;
	};
}  // SDKHelper
}  // Futronic

