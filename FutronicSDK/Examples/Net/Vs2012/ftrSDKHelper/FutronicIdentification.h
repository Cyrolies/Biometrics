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
    public delegate void OnGetBaseTemplateCompleteHandler( bool bSuccess,
                                                           int nResult );

	///<summary>
	/// The "Identification operation" class
	///</summary>
    public ref class FutronicIdentification : public FutronicSdkBase
    {
    public:
        ///<summary>
		/// This event is signaled when the enrollment operation for
		/// the identification purpose is completed and base template is ready.
		/// If the operation is completed successfully, you may start
		/// the identification operation.
        ///</summary>
        event OnGetBaseTemplateCompleteHandler ^ OnGetBaseTemplateComplete;

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
        ~FutronicIdentification( void )
        {
            m_BaseTemplate = nullptr;
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
        ///     <description>The "Show the current fingerprint image" event.</description>
        /// </item>
        /// <item>
        ///     <term>OnFakeSourceHandler</term>
        ///     <description>The "Fake Finger Detected"  event. This event raises
		///     only if <c>FakeDetection</c> and <c>FFDControl</c> properties are
		///     <c>true</c>.</description>
        /// </item>
        /// <item>
        ///     <term>OnGetBaseTemplateComplete</term>
		///     <description>This event is signaled when the enrollment operation for
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
                throw gcnew InvalidOperationException();

            m_WorkedThread = gcnew Thread( gcnew ThreadStart( this, &Futronic::SDKHelper::FutronicIdentification::GetBaseTemplateThreadStartProc ) );
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
        int Identification( FtrIdentifyRecord Template, bool % bResult )
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;
            CheckDispose();

            if( m_State != EnrollmentState::ready_to_continue )
                throw gcnew InvalidOperationException();

            if( Template.KeyValue == nullptr || Template.Template == nullptr )
                throw gcnew ArgumentException();

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
                pin_ptr<byte> pBaseTemplateData = &m_BaseTemplate[0];
                params.pBaseTemplate = pBaseTemplateData;

                nResult = SetParameters4IdentifyProcess( &params );
                if( nResult != FTR_RETCODE_OK )
                {
                    throw gcnew FutronicException( nResult, SdkRetCode2Message( nResult ) );
                }

                // Set template value
                Templates.TotalNumber = 1;
                int nMemRequired = sizeof( FTR_IDENTIFY_RECORD ) + 
                                   sizeof( FTR_DATA );
                unsigned char *ptr = new unsigned char[nMemRequired];
                if( ptr == NULL )
                    throw gcnew FutronicException( FTR_RETCODE_NO_MEMORY, SdkRetCode2Message( FTR_RETCODE_NO_MEMORY ) );

                Templates.pMembers = (FTR_IDENTIFY_RECORD_PTR)ptr;
                ptr += sizeof( FTR_IDENTIFY_RECORD );
                Templates.pMembers->pData = (FTR_DATA_PTR)ptr;

                pin_ptr<byte> pPointer = &Template.KeyValue[0];
                memcpy( Templates.pMembers->KeyValue, pPointer, Template.KeyValue->Length );

                pPointer = &Template.Template[0];
                Templates.pMembers->pData->dwSize = Template.Template->Length;
                Templates.pMembers->pData->pData = pPointer;

                int nIndex;
                nResult = IdentifyProcess( &Templates, &nIndex );
                if( nResult != FTR_RETCODE_OK )
                {
                    throw gcnew FutronicException( nResult, SdkRetCode2Message( nResult ) );
                }
                bResult = (nIndex == 0);
            }
            catch( FutronicException ^ ex )
            {
                nResult = ex->ErrorCode;
            }
            finally
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
        int Identification( array< FtrIdentifyRecord > ^rgTemplates, int% nIndex )
        {
            int nResult = FTR_RETCODE_INTERNAL_ERROR;
            CheckDispose();

            if( m_State != EnrollmentState::ready_to_continue )
                throw gcnew InvalidOperationException();

            if( rgTemplates == nullptr )
                throw gcnew ArgumentNullException( "rgTemplates" );

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
                pin_ptr<byte> pBaseTemplateData = &m_BaseTemplate[0];
                params.pBaseTemplate = pBaseTemplateData;

                nResult = SetParameters4IdentifyProcess( &params );
                if( nResult != FTR_RETCODE_OK )
                {
                    throw gcnew FutronicException( nResult, SdkRetCode2Message( nResult ) );
                }

                Templates.TotalNumber = 1;
                int nMemRequired = sizeof( FTR_IDENTIFY_RECORD ) + 
                                   sizeof( FTR_DATA );
                unsigned char *ptr = new unsigned char[nMemRequired];
                if( ptr == NULL )
                    throw gcnew FutronicException( FTR_RETCODE_NO_MEMORY, SdkRetCode2Message( FTR_RETCODE_NO_MEMORY ) );
                Templates.pMembers = (FTR_IDENTIFY_RECORD_PTR)ptr;
                ptr += sizeof( FTR_IDENTIFY_RECORD );
                Templates.pMembers->pData = (FTR_DATA_PTR)ptr;

                int idxTemplate, nIdx;
                for( idxTemplate = 0; !m_bCancel && (idxTemplate < rgTemplates->Length); idxTemplate++ )
                {
                    pin_ptr<byte> pPointer = &(rgTemplates[idxTemplate].KeyValue[0]);
                    memcpy( Templates.pMembers->KeyValue, pPointer, rgTemplates[idxTemplate].KeyValue->Length );

                    pPointer = &(rgTemplates[idxTemplate].Template[0]);
                    Templates.pMembers->pData->pData = pPointer;
                    Templates.pMembers->pData->dwSize = rgTemplates[idxTemplate].Template->Length;

                    nResult = IdentifyProcess( &Templates, &nIdx );
                    if( nResult != FTR_RETCODE_OK )
                    {
                        throw gcnew FutronicException( nResult, SdkRetCode2Message( nResult ) );
                    }
                    if( nIdx == 0 )
                    {
                        nIndex = idxTemplate;
                        break;
                    }
                }
            }
            catch( FutronicException ^ ex )
            {
                nResult = ex->ErrorCode;
            }
            finally
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
		/// Get\Set the base template.
        ///</summary>
        ///<remarks>
		/// Returns the base template. If enrollment operation for the identification
		/// purpose is not completed, the return value is null (or Nothing for VB).
        ///</remarks>
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
        property array<Byte>^ BaseTemplate
        {
            array<Byte>^ get()
            {
                CheckDispose();
                if( m_State != EnrollmentState::ready_to_continue )
                    throw gcnew InvalidOperationException();
                if( m_BaseTemplate == nullptr )
                    return nullptr;
                return (array<Byte>^)m_BaseTemplate->Clone();
            }

            void set( array<Byte>^ BaseTemplate )
            {
                CheckDispose();
                if( (m_State != EnrollmentState::ready_to_process) && (m_State != EnrollmentState::ready_to_continue) )
                    throw gcnew InvalidOperationException();
			    if( BaseTemplate == nullptr )
				    throw gcnew ArgumentNullException( "BaseTemplate" );
                m_BaseTemplate = (array<Byte>^)BaseTemplate->Clone();
                m_State = EnrollmentState::ready_to_continue;
            }
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

            // This parametr protect our delegate from garbage collection
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

                MyCallBackHandler ^callback = gcnew MyCallBackHandler( this, &Futronic::SDKHelper::FutronicSdkBase::cbControl );
                gch = GCHandle::Alloc( callback );
                IntPtr ip = Marshal::GetFunctionPointerForDelegate( callback );
                params.CommonParam.fCallBack = static_cast<UnmanagedCallBack>(ip.ToPointer());

                m_BaseTemplate = nullptr;
                nResult = GetBaseTemplateProcess( &params );
                if( nResult == FTR_RETCODE_OK )
                {
                    m_BaseTemplate = gcnew array<byte>( params.nTemplateSize );
                    pin_ptr<byte> pointer = &m_BaseTemplate[0];
                    memcpy( pointer, params.pTemplate, params.nTemplateSize );
                }
            }
            catch( ThreadAbortException^ )
            {
                nResult = FTR_RETCODE_CANCELED_BY_USER;
            }
            finally
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
        array<Byte> ^m_BaseTemplate;
    };
}  // SDKHelper
}  // Futronic
