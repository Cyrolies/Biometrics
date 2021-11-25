#pragma once

using namespace System::Runtime::InteropServices;

namespace Futronic {
namespace SDKHelper {

    ///////////////////////////////////////////////////////////////////
    // Declare delegates for the FutronicEnrollment class
    ///////////////////////////////////////////////////////////////////

    ///<summary>
    /// The "Enrollment operation complete" __event.
    ///</summary>
    ///<param name="bSuccess">
    /// <c>true</c> if the operation succeeds, otherwise is <c>false</c>.
    ///</param>
    ///<param name="nResult">
    /// The Futronic SDK return code (see FTRAPI.h).
    ///</param>
    public __delegate void OnEnrollmentCompleteHandler( bool bSuccess, int nResult );

    ///<summary>
    /// The "Enrollment operation" class
    ///</summary>
    public __gc class FutronicEnrollment : public FutronicSdkBase
    {
    protected:
        static int MinModelsValue = 1;
        static int MaxModelsValue = 10;
        static int DefaultModelsValue = 5;
    public:

        ///<summary>
		/// This __event is signaled when the enrollment operation is completed.
		/// If the operation is completed successfully, you may get a template.
        ///</summary>
        __event OnEnrollmentCompleteHandler * OnEnrollmentComplete;

        ///<summary>
        /// The FutronicEnrollment class constructor.
        /// Initialize a new instance of the FutronicEnrollment class.
        ///</summary>
        ///<exception cref="FutronicException">
        /// Error occurs during SDK initialization. To get error code, see 
        /// property ErrorCode of FutronicException class.
        ///</exception>
        FutronicEnrollment(void)
            : FutronicSdkBase()
        {
            m_bMIOTControlOff = false;
            m_Template = nullptr;
            m_Quality = 0;
            m_MaxModels = DefaultModelsValue;
        }

        ///<summary>
        /// Releases the unmanaged resources used by the FutronicEnrollment and optionally 
        /// releases the managed resources.
        ///</summary>
        virtual void Dispose( void )
        {
            if( m_bDispose )
                return;
            m_Template = nullptr;
            FutronicSdkBase::Dispose();
        }

        ///<summary>
        /// This function starts the enrollment operation.
        ///</summary>
        ///<remarks>
		/// The enrollment operation starts in its own thread. To interact
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
        ///     <description>The "Show the current fingerprint image" __event.</description>
        /// </item>
        /// <item>
        ///     <term>OnFakeSourceHandler</term>
        ///     <description>The "Fake Finger Detected"  __event. This __event raises
		///     only if <c>FakeDetection</c> and <c>FFDControl</c> properties are
		///     <c>true</c>.</description>
        /// </item>
        /// <item>
        ///     <term>OnEnrollmentComplete</term>
		///     <description>This __event is signaled when the enrollment operation is completed.
		///     If the operation is completed successfully, you may get a template.</description>
        /// </item>
        /// </list>
        /// If the enrollment operation is completed successfully, you may get a template.
        /// The next call of the enrollment operation removes the last created template.
        ///</remarks>
        ///<exception cref="InvalidOperationException">
        /// The enrollment operation is already started.
        ///</exception>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        void Enrollment()
        {
            CheckDispose();

            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();

            m_State = EnrollmentState::process_in_progress;

            m_WorkedThread = __gc new Thread( __gc new ThreadStart( this, &Futronic::SDKHelper::FutronicEnrollment::EnrollmentThreadStartProc ) );
            m_WorkedThread->IsBackground = true;
            m_WorkedThread->Start();
        }

        ///<summary>
        /// Get the MIOT mode setting property.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        __property bool get_MIOTControlOff()
        {
            CheckDispose();
            return m_bMIOTControlOff;
        }

        ///<summary>
        /// Set the MIOT mode setting property.
        ///</summary>
        ///<remarks>
        /// Set this property to <c>true</c>, if you want to enable
		/// the MIOT mode.
		/// The default value is <c>false</c>.
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The enrollment operation is started. You cannot change
		/// this property in the current moment.
        ///</exception>
        __property void set_MIOTControlOff( bool bMIOTControlOff )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            m_bMIOTControlOff = bMIOTControlOff;
        }

        ///<summary>
        /// Get max number of models in one template.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        __property int get_MaxModels()
        {
            CheckDispose();
            return m_MaxModels;
        }

		///<summary>
        /// Set max number of models in one template (Read/Write).
        ///</summary>
        ///<remarks>
        /// This value must be between 3 and 10. The default value is 7.
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The enrollment operation is started. You cannot change
		/// this property in the current moment.
        ///</exception>
        ///<exception cref="ArgumentOutOfRangeException">
        /// You are trying to set invalid number of models.
        ///</exception>
        __property void set_MaxModels( int MaxModels )
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();

            if( MaxModels < 3 || MaxModels > 10 )
                throw __gc new ArgumentOutOfRangeException( "MaxModels" );

            m_MaxModels = MaxModels;
        }

        ///<summary>
        /// Returns the template of the last enrollment operation (Read only).
        ///</summary>
        ///<remarks>
		/// Returns a copy of template. If the last enrollment operation is
		/// unsuccessful, the return code is null (or Nothing for VB).
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The enrollment operation is started. You cannot get
		/// this property in the current moment.
        ///</exception>
        __property Byte get_Template() []
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            if( m_Template == nullptr )
                return nullptr;
            return (Byte[])m_Template->Clone();
        }

        ///<summary>
		/// Return the quality of the template (Read only).
        ///</summary>
        ///<remarks>
		/// Return value may be one of the following: 1 (the lowest quality)
		/// to  10 (best quality).
		/// If the enrollment operation is unsuccessful or was not started, the
		/// return value is 0.
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The enrollment operation is started. You cannot get
		/// this property in the current moment.
        ///</exception>
        __property unsigned int get_Quality()
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_process )
                throw __gc new InvalidOperationException();
            return m_Quality;
        }

    private:

        ///<summary>
        /// The main thread of the enrollment operation.
        ///</summary>
        ///<remarks>
		/// Function prepares all necessary parameters for enrollment
		/// operation and calls the function from unmanaged
		/// code. This unmanaged function sets all parameters for SDK and
		/// starts the enrollment operation. It helps to reduce number of
		/// switching between managed and unmanaged codes and increases
		/// speed.
        ///</remarks>
        void EnrollmentThreadStartProc()
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;

            // This parameter protects our __delegate from garbage collection
            GCHandle gch;

            struct EnrollmentParameters params;
            memset( &params, 0, sizeof( EnrollmentParameters ) );

            Monitor::Enter( m_SyncRoot );

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

				MyCallBackHandler *callback = __gc new MyCallBackHandler( this, &Futronic::SDKHelper::FutronicSdkBase::cbControl );
                gch = GCHandle::Alloc( callback );
                params.CommonParam.fCallBack = GCHandle::op_Explicit( gch ).ToPointer();

                params.nMaxModels = m_MaxModels;

                if( m_bMIOTControlOff ) params.bMIOTControlOff = TRUE;
                else params.bMIOTControlOff = FALSE;

                m_Template = nullptr;
                m_Quality = 0;
                nResult = EnrollmentProcess( &params );
                if( nResult == FTR_RETCODE_OK )
                {
                    m_Template = __gc new Byte[ params.nTemplateSize ];
                    Byte __pin * pointer = &m_Template[0];
                    memcpy( pointer, params.pTemplate, params.nTemplateSize );
                    m_Quality = params.Quality;
                }
            }
            catch( ThreadAbortException* )
            {
                nResult = FTR_RETCODE_CANCELED_BY_USER;
            }
            __finally
            {
                if( params.pTemplate != NULL )
                    delete params.pTemplate;

                gch.Free();

                m_State = EnrollmentState::ready_to_process;

                Monitor::Exit( m_SyncRoot );

                OnEnrollmentComplete( nResult == FTR_RETCODE_OK, nResult );
            }
        }

        ///<summary>
		/// The MIOT mode setting.
		/// You cannot modify this variable directly. Use the MIOTControlOff property.
		/// The default value is <c>false</c>.
        ///</summary>
        ///<seealso cref="MIOTControlOff"/>
        bool            m_bMIOTControlOff;

        ///<summary>
		/// The template of the last enrollment operation.
		/// You cannot modify this variable directly. Use the Template property.
        ///</summary>
        Byte            m_Template[];

        ///<summary>
        /// Estimation of a template quality in terms of recognition:
        /// 1 corresponds to the worst quality, 10 denotes the best.
        ///</summary>
        unsigned int    m_Quality;

        ///<summary>
        /// Max number of models in one template. This value must
		/// be between 3 and 10.
        ///</summary>
        int             m_MaxModels;
    };
}  // SDKHelper
}  // Futronic
