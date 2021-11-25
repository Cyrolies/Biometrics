#pragma once

using namespace System::Runtime::InteropServices;

namespace Futronic {
namespace SDKHelper {

    ///////////////////////////////////////////////////////////////////
    // Declare delegates for the FutronicIdentification class
    ///////////////////////////////////////////////////////////////////

    ///<summary>
    /// The "Get base template operation complete" event.
    ///</summary>
    ///<param name="bSuccess">
    /// <c>true</c> if the operation succeeds, otherwise is <c>false</c>.
    ///</param>
    ///<param name="nResult">
    /// The Futronic SDK return code (see FTRAPI.h).
    ///</param>
    public __delegate void OnGetBaseTemplateCompleteHandler( bool bSuccess,
                                                           int nResult );

	///<summary>
	/// The "Identification operation" class
	///</summary>
    public __gc class FutronicIdentification : public FutronicSdkBase
    {
    public:
        ///<summary>
		/// This event is signaled when the enrollment operation for
		/// the identification purpose is completed and base template is ready.
		/// If the operation is completed successfully, you may start
		/// the identification operation.
        ///</summary>
        __event OnGetBaseTemplateCompleteHandler * OnGetBaseTemplateComplete;

        ///<summary>
        /// The FutronicIdentification class constructor.
        /// Initialize a new instance of the FutronicIdentification class.
        ///</summary>
        ///<exception cref="FutronicException">
        /// Error occurs during SDK initialization. To get error code, see 
        /// property ErrorCode of FutronicException class.
        ///</exception>
        FutronicIdentification(void)
            : FutronicSdkBase()
        {
            m_BaseTemplate = nullptr;
        }

        ///<summary>
        /// Releases the unmanaged resources used by the FutronicIdentification and optionally 
        /// releases the managed resources.
        ///</summary>
        virtual void Dispose( void )
        {
            if( m_bDispose )
                return;
            m_BaseTemplate = nullptr;
            FutronicSdkBase::Dispose();
        }

        ///<summary>
		/// This function starts the "get base template" operation for the
		/// identification purpose.
        ///</summary>
        ///<remarks>
		/// The "get base template" operation starts in its own thread. To interact
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
        ///     <term>OnGetBaseTemplateComplete</term>
		///     <description>This __event is signaled when the enrollment operation for
		///     the identification purpose is completed and base template is ready.
		///     If the operation is completed successfully, you may start
		///     the identification operation.</description>
        /// </item>
        /// </list>
		/// If the enrollment operation for the identification purpose is completed
		/// successfully, you may start any identification function.
		/// The next call of the enrollment operation will empty the last received results.
        ///</remarks>
        ///<exception cref="InvalidOperationException">
        /// The identification operation or the enrollment operation for the identification
		/// purpose is already started.
        ///</exception>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        void GetBaseTemplate()
        {
            CheckDispose();

            if( (m_State != EnrollmentState::ready_to_process) && (m_State != EnrollmentState::ready_to_continue ) )
                throw __gc new InvalidOperationException();

            m_WorkedThread = __gc new Thread( __gc new ThreadStart( this, &Futronic::SDKHelper::FutronicIdentification::GetBaseTemplateThreadStartProc ) );
            m_WorkedThread->IsBackground = true;
            m_WorkedThread->Start();
        }

        ///<summary>
        /// The function compares the base template against a single template.
        ///</summary>
        ///<param name="Template">
		/// The source templates.
        ///</param>
        ///<param name="bResult">
		/// If the function succeeds, <c>bResults</c> contains result of the
		/// identification.
		/// <c>true</c> if the template is matched, otherwise is <c>false</c>.
        ///</param>
        ///<returns>
		/// The Futronic SDK return code (see FTRAPI.h).
        ///</returns>
        ///<exception cref="InvalidOperationException">
		/// The enrollment operation for the identification purpose is
		/// not completed.
        ///</exception>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        int Identification( FtrIdentifyRecord Template, System::Boolean & bResult )
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;
            CheckDispose();

            if( m_State != EnrollmentState::ready_to_continue )
                throw __gc new InvalidOperationException();

            if( Template.KeyValue == nullptr || Template.Template == nullptr )
                throw __gc new ArgumentException();

            bResult = false;
            m_State = EnrollmentState::continue_in_progress;

            struct IdentifyParameters params;
            memset( &params, 0, sizeof( IdentifyParameters ) );
            FTR_IDENTIFY_ARRAY Templates;
            memset( &Templates, 0, sizeof( FTR_IDENTIFY_ARRAY ) );

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

				// Set base template value
                params.nBaseTemplateSize = m_BaseTemplate->Length;
                Byte __pin * pBaseTemplateData = &m_BaseTemplate[0];
                params.pBaseTemplate = pBaseTemplateData;

                nResult = SetParameters4IdentifyProcess( &params );
                if( nResult != FTR_RETCODE_OK )
                {
                    throw __gc new FutronicException( nResult, SdkRetCode2Message( nResult ) );
                }

                // Set template value
                Templates.TotalNumber = 1;
                int nMemRequired = sizeof( FTR_IDENTIFY_RECORD ) + 
                                   sizeof( FTR_DATA );
                unsigned char *ptr = new unsigned char[nMemRequired];
                if( ptr == NULL )
                    __gc new FutronicException( FTR_RETCODE_NO_MEMORY, SdkRetCode2Message( FTR_RETCODE_NO_MEMORY ) );

                Templates.pMembers = (FTR_IDENTIFY_RECORD_PTR)ptr;
                ptr += sizeof( FTR_IDENTIFY_RECORD );
                Templates.pMembers->pData = (FTR_DATA_PTR)ptr;

                Byte __pin * pPointer = &Template.KeyValue[0];
                memcpy( Templates.pMembers->KeyValue, pPointer, Template.KeyValue->Length );

                pPointer = &Template.Template[0];
                Templates.pMembers->pData->dwSize = Template.Template->Length;
                Templates.pMembers->pData->pData = pPointer;

                int nIndex;
                nResult = IdentifyProcess( &Templates, &nIndex );
                if( nResult != FTR_RETCODE_OK )
                {
                    throw __gc new FutronicException( nResult, SdkRetCode2Message( nResult ) );
                }
                bResult = (nIndex == 0);
            }
            catch( FutronicException * ex )
            {
                nResult = ex->ErrorCode;
            }
            __finally
            {
                if( Templates.pMembers != NULL )
                    delete ((unsigned char*)Templates.pMembers);

                m_State = EnrollmentState::ready_to_continue;

                Monitor::Exit( m_SyncRoot );
            }

            return nResult;
        }

        ///<summary>
        /// The function compares the base template against a set of
		/// source templates.
        ///</summary>
        ///<param name="rgTemplates">
        /// The set of source templates.
        ///</param>
        ///<param name="nIndex">
		/// If the function succeeds, <c>nIndex</c> contains an index of the
		/// matched record (the first element has an index 0) or -1, if
		/// no matching source templates are detected.
        ///</param>
        ///<remarks>
		/// The identification operation is stopped, when the first matched
		/// template is detected.
        ///</remarks>
        ///<returns>
        /// The Futronic SDK return code (see FTRAPI.h).
        ///</returns>
        ///<exception cref="InvalidOperationException">
		/// The enrollment operation for the identification purpose is
		/// not completed.
        ///</exception>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="ArgumentNullException">
        /// A null (Nothing in VB) reference parameter rgTemplates is passed to
        /// the function.
        ///</exception>
        int Identification( FtrIdentifyRecord rgTemplates[], System::Int32 & nIndex )
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;
            CheckDispose();

            if( m_State != EnrollmentState::ready_to_continue )
                throw __gc new InvalidOperationException();

            if( rgTemplates == nullptr )
                throw __gc new ArgumentNullException( "rgTemplates" );

            nIndex = -1;

            if( rgTemplates->Length == 0 )
                return FTR_RETCODE_OK;

            m_State = EnrollmentState::continue_in_progress;

            struct IdentifyParameters params;
            memset( &params, 0, sizeof( IdentifyParameters ) );
            FTR_IDENTIFY_ARRAY Templates;
            memset( &Templates, 0, sizeof( FTR_IDENTIFY_ARRAY ) );

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

				// Set base template value
                params.nBaseTemplateSize = m_BaseTemplate->Length;
                Byte __pin * pBaseTemplateData = &m_BaseTemplate[0];
                params.pBaseTemplate = pBaseTemplateData;

                nResult = SetParameters4IdentifyProcess( &params );
                if( nResult != FTR_RETCODE_OK )
                {
                    throw __gc new FutronicException( nResult, SdkRetCode2Message( nResult ) );
                }

                Templates.TotalNumber = 1;
                int nMemRequired = sizeof( FTR_IDENTIFY_RECORD ) + 
                                   sizeof( FTR_DATA );
                unsigned char *ptr = new unsigned char[nMemRequired];
                if( ptr == NULL )
                    throw __gc new FutronicException( FTR_RETCODE_NO_MEMORY, SdkRetCode2Message( FTR_RETCODE_NO_MEMORY ) );
                Templates.pMembers = (FTR_IDENTIFY_RECORD_PTR)ptr;
                ptr += sizeof( FTR_IDENTIFY_RECORD );
                Templates.pMembers->pData = (FTR_DATA_PTR)ptr;

                int idxTemplate, nIdx;
                for( idxTemplate = 0; !m_bCancel && (idxTemplate < rgTemplates->Length); idxTemplate++ )
                {
                    Byte __pin * pPointer = &(rgTemplates[idxTemplate].KeyValue[0]);
                    memcpy( Templates.pMembers->KeyValue, pPointer, rgTemplates[idxTemplate].KeyValue->Length );

                    pPointer = &(rgTemplates[idxTemplate].Template[0]);
                    Templates.pMembers->pData->pData = pPointer;
                    Templates.pMembers->pData->dwSize = rgTemplates[idxTemplate].Template->Length;

                    nResult = IdentifyProcess( &Templates, &nIdx );
                    if( nResult != FTR_RETCODE_OK )
                    {
                        throw __gc new FutronicException( nResult, SdkRetCode2Message( nResult ) );
                    }
                    if( nIdx == 0 )
                    {
                        nIndex = idxTemplate;
                        break;
                    }
                }
            }
            catch( FutronicException * ex )
            {
                nResult = ex->ErrorCode;
            }
            __finally
            {
                if( Templates.pMembers != NULL )
                    delete ((unsigned char*)Templates.pMembers);

                m_State = EnrollmentState::ready_to_continue;

                m_bCancel = false;

                Monitor::Exit( m_SyncRoot );
            }

            return nResult;
        }

        ///<summary>
		/// Get the base template.
        ///</summary>
        ///<remarks>
		/// Returns the base template. If enrollment operation for the identification
		/// purpose is not completed, the return value is 0 (or Nothing for VB).
        ///</remarks>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The identification operation or the enrollment operation for the identification
		/// purpose is already started.
        ///</exception>
        __property Byte get_BaseTemplate() []
        {
            CheckDispose();
            if( m_State != EnrollmentState::ready_to_continue )
                throw __gc new InvalidOperationException();
            if( m_BaseTemplate == nullptr )
                return nullptr;
            return (Byte[])m_BaseTemplate->Clone();
        }

        ///<summary>
		/// Set the base template.
        ///</summary>
        ///<exception cref="ObjectDisposedException">
        /// The class instance is disposed. Any calls are prohibited.
        ///</exception>
        ///<exception cref="InvalidOperationException">
		/// The identification operation or the enrollment operation for the identification
		/// purpose is already started.
        ///</exception>
        ///<exception cref="ArgumentNullException">
		/// A null reference (Nothing in Visual Basic) is passed to a method.
        ///</exception>
        __property void set_BaseTemplate( Byte BaseTemplate[] )
        {
            CheckDispose();
            if( (m_State != EnrollmentState::ready_to_process) && (m_State != EnrollmentState::ready_to_continue) )
                throw __gc new InvalidOperationException();
			if( BaseTemplate == nullptr )
				throw __gc new ArgumentNullException( "BaseTemplate" );
            m_BaseTemplate = (Byte[])BaseTemplate->Clone();
            m_State = EnrollmentState::ready_to_continue;
        }

    private:
		///<summary>
		/// The main thread of the enrollment operation for the
		/// identification purpose.
		///</summary>
		///<remarks>
		/// Function prepares all necessary parameters for the enrollment
		/// operation and calls the function from unmanaged
		/// code. This unmanaged function sets all parameters for SDK and
		/// starts the enrollment operation. It helps to reduce number of
		/// switching between managed and unmanaged codes and increases
		/// speed.
		///</remarks>
        void GetBaseTemplateThreadStartProc()
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;

            // This parametr protect our __delegate from garbage collection
            GCHandle gch;

            struct GetBaseTemplateParameters params;
            memset( &params, 0, sizeof( GetBaseTemplateParameters ) );

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

                m_BaseTemplate = nullptr;
                nResult = GetBaseTemplateProcess( &params );
                if( nResult == FTR_RETCODE_OK )
                {
                    m_BaseTemplate = __gc new Byte[ params.nTemplateSize ];
                    Byte __pin * pointer = &m_BaseTemplate[0];
                    memcpy( pointer, params.pTemplate, params.nTemplateSize );
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

                if( m_BaseTemplate != nullptr )
                {
                    m_State = EnrollmentState::ready_to_continue;
                } else {
                    m_State = EnrollmentState::ready_to_process;
                }
                Monitor::Exit( m_SyncRoot );

                OnGetBaseTemplateComplete( nResult == FTR_RETCODE_OK, nResult );
            }
        }

        ///<summary>
		/// The base template.
		/// You cannot access to this variable directly. Use the BaseTemplate property.
        ///</summary>
        Byte m_BaseTemplate[];
    };
}  // SDKHelper
}  // Futronic
