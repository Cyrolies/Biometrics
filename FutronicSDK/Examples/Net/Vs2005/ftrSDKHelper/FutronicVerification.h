#pragma once

using namespace System::Runtime::InteropServices;

namespace Futronic {
namespace SDKHelper {

    ///////////////////////////////////////////////////////////////////
    // Declare delegates for the FutronicVerification class
    ///////////////////////////////////////////////////////////////////

    ///<summary>
	/// The "Verification operation complete" event.
    ///</summary>
    ///<param name="bSuccess">
    /// <c>true</c> if the operation succeeds, otherwise is <c>false</c>.
    ///</param>
    ///<param name="nResult">
    /// The Futronic SDK return code (see FTRAPI.h).
    ///</param>
    ///<param name="bVerificationSuccess">
	/// If the operation succeeds (bSuccess is <c>true</c>), this parameters
	/// shows the verification operation result. <c>true</c> if the captured
	/// from the attached scanner template is matched, otherwise is <c>false</c>.
    ///</param>
    public delegate 
        void OnVerificationCompleteHandler( bool bSuccess,
                                            int nResult,
                                            bool bVerificationSuccess );

    ///<summary>
	/// The FutronicVerification class captures an image from the attached
	// scanner, builds the corresponding template and compares it with the source
	// template.
    ///</summary>
    public ref class FutronicVerification : public FutronicSdkBase
    {
    public:

        ///<summary>
		/// This event is signaled when the verification operation is completed.
        ///</summary>
        event OnVerificationCompleteHandler ^ OnVerificationComplete;

        ///<summary>
        /// The FutronicVerification class constructor.
        /// Initialize a new instance of the FutronicVerification class.
        ///</summary>
        ///<param name="Template">
        /// A source template for verification.
        ///</param>
        ///<exception cref="FutronicException">
        /// Error occurs during SDK initialization. To get error code, see 
        /// property ErrorCode of the FutronicException class.
        ///</exception>
        ///<exception cref="ArgumentNullException">
        /// A null (Nothing in VB) reference parameter Template is passed to
        /// the constructor.
        ///</exception>
        FutronicVerification( array<byte> ^ Template )
            : FutronicSdkBase()
        {
            if( Template == nullptr )
                throw gcnew ArgumentNullException( "Template" );
            m_Template = (array<byte>^)Template->Clone();
            m_FARNValue = 1;
            m_bResult = false;
        }

        ///<summary>
        /// Releases the unmanaged resources used by the FutronicVerification and optionally 
        /// releases the managed resources.
        ///</summary>
        ~FutronicVerification( void )
        {
            m_Template = nullptr;
        }

        ///<summary>
		/// This function starts the verification operation.
		///</summary>
		///<remarks>
		/// The verification operation starts in its own thread. To interact
		/// with the enrollment operation you should specify one or more
		/// of the following events:
        /// <list type="table">
        /// <listheader>
        ///     <term>Event</term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term>OnPutOn</term>
        ///     <description>Invitation for touching the fingerprint scanner surface.
        ///     </description>
        /// </item>
        /// <item>
        ///     <term>OnTakeOff</term>
		///     <description>Proposal to take off a finger from the scanner surface.
        ///     </description>
        /// </item>
        /// <item>
        ///     <term>UpdateScreenImage</term>
        ///     <description>The "Show the current fingerprint image" event.</description>
        /// </item>
        /// <item>
        ///     <term>OnFakeSourceHandler</term>
        ///     <description>The "Fake Finger Detected"  event. This event raises
		///     only if <c>FakeDetection</c> and <c>FFDControl</c> properties are
		///     <c>true</c>.</description>
        /// </item>
        /// <item>
        ///     <term>OnVerificationComplete</term>
		///     <description>This event is signaled when the verification operation
		///     is completed.</description>
        /// </item>
        /// </list>
        ///</remarks>
        ///<exception cref="InvalidOperationException">
        /// The verification operation is already started.
        ///</exception>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        void Verification()
        {
            CheckDispose();

            if( m_State != EnrollmentState::ready_to_process )
                throw gcnew InvalidOperationException();

            m_State = EnrollmentState::process_in_progress;
            m_WorkedThread = gcnew Thread( gcnew ThreadStart( this, &Futronic::SDKHelper::FutronicVerification::VerificationThreadStartProc ) );
            m_WorkedThread->IsBackground = true;
            m_WorkedThread->Start();
        }

        ///<summary>
		/// The last verification result (Read only).
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The verification operation is not finished.
        ///</exception>
        property bool Result
        {
            bool get()
            {
                CheckDispose();
                if( m_State != EnrollmentState::ready_to_process )
                    throw gcnew InvalidOperationException();
                return m_bResult;
            }
        }

        ///<summary>
		/// The FARN value returned during the last verification
		/// operation (Read only).
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The verification operation is not finished.
        ///</exception>
        property int FARNValue
        {
            int get()
            {
                CheckDispose();
                if( m_State != EnrollmentState::ready_to_process )
                    throw gcnew InvalidOperationException();
                return m_FARNValue;
            }
        }
    private:

		///<summary>
		/// The main thread of the verification operation.
		///</summary>
		///<remarks>
		/// Function prepares all necessary parameters for the verification
		/// operation and calls the function from unmanaged
		/// code. This unmanaged function sets all parameters for SDK and
		/// starts the verification operation. It helps to reduce number of
		/// switching between managed and unmanaged codes and increases
		/// speed.
		///</remarks>
        void VerificationThreadStartProc()
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;

            // This parameter protects our delegate from garbage collection
            GCHandle gch;

            struct VerificationParameters params;
            memset( &params, 0, sizeof( VerificationParameters ) );

            Monitor::Enter( m_SyncRoot );

            m_bResult = FALSE;

            try
            {
                params.CommonParam.FrameSource = FSD_FUTRONIC_USB;

                if( m_bFakeDetection ) params.CommonParam.bFakeDetection = TRUE;
                else params.CommonParam.bFakeDetection = FALSE;

                if( m_bFFDControl ) params.CommonParam.bFFDControl = TRUE;
                else params.CommonParam.bFFDControl = FALSE;

                params.CommonParam.FARNLevel = m_FARN;
                params.CommonParam.Version = (FTR_VERSION)m_Version;

                if( m_bFastMode ) params.CommonParam.bFastMode = TRUE;
                else params.CommonParam.bFastMode = FALSE;

                MyCallBackHandler ^callback = gcnew MyCallBackHandler( this, &Futronic::SDKHelper::FutronicSdkBase::cbControl );
                gch = GCHandle::Alloc( callback );
                IntPtr ip = Marshal::GetFunctionPointerForDelegate( callback );
                params.CommonParam.fCallBack = static_cast<UnmanagedCallBack>(ip.ToPointer());

                params.nTemplateSize = m_Template->Length;
                pin_ptr<byte> pointer = &m_Template[0];
                params.pTemplate = pointer;
                nResult = VerificationProcess( &params );
                m_bResult = (params.bResult == TRUE);
                m_FARNValue = params.FARNLevel;
            }
            catch( ThreadAbortException^ )
            {
                nResult = FTR_RETCODE_CANCELED_BY_USER;
            }
            finally
            {
                gch.Free();

                m_State = EnrollmentState::ready_to_process;

                Monitor::Exit( m_SyncRoot );

                OnVerificationComplete( nResult == FTR_RETCODE_OK, nResult, m_bResult );
            }
        }

        ///<summary>
        /// This is a copy of the source template.
        ///</summary>
        array<byte>     ^m_Template;

        ///<summary>
		/// The last verification result.
		/// You cannot access to this variable directly. Use the Result property.
        ///</summary>
        bool            m_bResult;

        ///<summary>
		/// The FARN value returned during the last verification operation.
		/// You cannot access to this variable directly. Use the FARNValue property.
        ///</summary>
        int             m_FARNValue;
    };
}  // SDKHelper
}  // Futronic
