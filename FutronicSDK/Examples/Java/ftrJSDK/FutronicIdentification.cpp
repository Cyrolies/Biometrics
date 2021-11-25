#include "StdAfx.h"
#include "FutronicIdentification.h"
#include "FtrIdentifyRecord.h"
#include "FtrIdentifyResult.h"

CFutronicIdentification::CFutronicIdentification( JNIEnv *env, jobject obj )
    : CFutronicSdkBase( env, obj )
{
    m_BaseTemplateID = NULL;
}

CFutronicIdentification::~CFutronicIdentification(void)
{
    m_BaseTemplateID = NULL;
}

FTRAPI_RESULT CFutronicIdentification::Initialize()
{
    FTRAPI_RESULT nResult = CFutronicSdkBase::Initialize();

    if( nResult == FTR_RETCODE_OK )
    {
        m_BaseTemplateID = m_env->GetFieldID( m_class, "m_BaseTemplate", "[B" );
        if( m_BaseTemplateID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;
    }

    return nResult;
}

FTRAPI_RESULT CFutronicIdentification::GetBaseTemplateProcess()
{
    FTRAPI_RESULT   nRetCode;
    BOOL            bValue;
    int             TemplateSize;
    BYTE*           pTemplate;
    FTR_DATA        Template;
    FTR_ENROLL_DATA eData;

    // 1. set frame source
    nRetCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, reinterpret_cast<void*>(getFrameSource()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 2. user's calback
    nRetCode = FTRSetParam( FTR_PARAM_CB_CONTROL, reinterpret_cast<void*>(UnmanagedCBControl) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 3. FARN setting
    nRetCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, reinterpret_cast<void*>(getFARN()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 4. fake mode setting
    nRetCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, reinterpret_cast<void*>(getFakeDetection()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 5. application takes a control over the Fake Finger Detection (FFD) event
    nRetCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, reinterpret_cast<void*>(getFFDControl()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 6. MIOT mode setting
    bValue = FALSE;
    nRetCode = FTRSetParam( FTR_PARAM_MIOT_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. VERSION compatibility
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<FTR_PARAM_VALUE>(getVersionCompatible()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 8. FastMode setting
    nRetCode = FTRSetParam( FTR_PARAM_FAST_MODE, reinterpret_cast<void*>(getFastMode()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // select memory
    nRetCode = FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE, reinterpret_cast<void**>(&TemplateSize) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }
    pTemplate = new BYTE[TemplateSize];
    if( pTemplate == NULL )
    {
        return FTR_RETCODE_NO_MEMORY;
    }

    // prepare arguments
    Template.dwSize = TemplateSize;
    Template.pData = pTemplate;
    eData.dwSize   = sizeof( FTR_ENROLL_DATA );

    // enroll operation
    nRetCode = FTREnroll( (FTR_USER_CTX)this, FTR_PURPOSE_IDENTIFY, &Template );
    if( nRetCode == FTR_RETCODE_OK )
    {
        jbyteArray pJTemplate;
        pJTemplate = m_env->NewByteArray( (jsize)(Template.dwSize) );
        if( pJTemplate != NULL )
        {
            jboolean isCopy;
            jbyte *pJData = m_env->GetByteArrayElements( pJTemplate, &isCopy );
            memcpy( pJData, Template.pData, Template.dwSize );
            m_env->ReleaseByteArrayElements( pJTemplate, pJData, 0 );
            m_env->SetObjectField( m_obj, m_BaseTemplateID, pJTemplate );
        } else {
            nRetCode = FTR_RETCODE_NO_MEMORY;
        }
    }
    delete pTemplate;

    return nRetCode;
}

FTRAPI_RESULT CFutronicIdentification::IdentifyProcess( jobjectArray rgTemplates, jobject Result )
{
    FTRAPI_RESULT   nRetCode;
    FTR_DATA        BaseTemplate;

    // 1. set frame source
    nRetCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, reinterpret_cast<void*>(getFrameSource()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 3. FARN setting
    nRetCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, reinterpret_cast<void*>(getFARN()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 4. fake mode setting
    nRetCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, reinterpret_cast<void*>(getFakeDetection()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 5. application takes a control over the Fake Finger Detection (FFD) event
    nRetCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, reinterpret_cast<void*>(getFFDControl()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 6. VERSION compatibility
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<FTR_PARAM_VALUE>(getVersionCompatible()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. FastMode setting
    nRetCode = FTRSetParam( FTR_PARAM_FAST_MODE, reinterpret_cast<void*>(getFastMode()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // set base template
    jbyteArray pJTemplate;
    pJTemplate = (jbyteArray)m_env->GetObjectField( m_obj, m_BaseTemplateID );
    if( pJTemplate != NULL )
    {
        BaseTemplate.dwSize = m_env->GetArrayLength( pJTemplate );
        BaseTemplate.pData = m_env->GetByteArrayElements( pJTemplate, NULL );
        nRetCode = FTRSetBaseTemplate( &BaseTemplate );
        m_env->ReleaseByteArrayElements( pJTemplate, (jbyte*)BaseTemplate.pData, JNI_ABORT );
    } else {
        nRetCode = FTR_RETCODE_INTERNAL_ERROR;
    }
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // walk throw sample array
    DWORD                   resNum = 0;
    FTR_DATA                Template;
    FTR_IDENTIFY_RECORD     inputRec;
    FTR_IDENTIFY_ARRAY      input;
    FTR_MATCHED_X_RECORD    resultRec;
    FTR_MATCHED_X_ARRAY     result;

    input.TotalNumber = 1;
    input.pMembers = &inputRec;

    inputRec.pData = &Template;

    memset( &resultRec, 0, sizeof( FTR_MATCHED_X_RECORD ) );
    result.TotalNumber = 1;
    result.pMembers = &resultRec;

    inputRec.pData = &Template;

    int nIndex;
    int TotalNumber = m_env->GetArrayLength( rgTemplates );
    jobject JIdentifyRecord;
    CFtrIdentifyRecord IdentifyRecord( m_env, NULL );
    CFtrIdentifyResult IdentifyResult( m_env, Result );

    nRetCode = IdentifyRecord.Initialize();
    if( nRetCode != FTR_RETCODE_OK )
        return nRetCode;

    nRetCode = IdentifyResult.Initialize();
    if( nRetCode != FTR_RETCODE_OK )
        return nRetCode;

    for( nIndex = 0; nIndex < TotalNumber; nIndex++ )
    {
        // get next input sample from array
        JIdentifyRecord = m_env->GetObjectArrayElement( rgTemplates, nIndex );
        if( !IdentifyRecord.SetNewObject( JIdentifyRecord ) )
        {
            nRetCode = FTR_RETCODE_INTERNAL_ERROR;
            break;
        }

        // set key value
        pJTemplate = IdentifyRecord.GetKey();
        if( pJTemplate == NULL )
        {
            nRetCode = FTR_RETCODE_INVALID_ARG;
            break;
        }
        BaseTemplate.dwSize = m_env->GetArrayLength( pJTemplate );
        _ASSERT( BaseTemplate.dwSize <= sizeof(FTR_DATA_KEY) );
        memset( inputRec.KeyValue, 0, sizeof(FTR_DATA_KEY) );
        BaseTemplate.pData = m_env->GetByteArrayElements( pJTemplate, NULL );
        memcpy( inputRec.KeyValue, BaseTemplate.pData, BaseTemplate.dwSize );
        m_env->ReleaseByteArrayElements( pJTemplate, (jbyte*)BaseTemplate.pData, JNI_ABORT );

        // set template value
        pJTemplate = IdentifyRecord.GetTemplate();
        if( pJTemplate == NULL )
        {
            nRetCode = FTR_RETCODE_INVALID_ARG;
            break;
        }
        Template.dwSize = m_env->GetArrayLength( pJTemplate );
        Template.pData = m_env->GetByteArrayElements( pJTemplate, NULL );

        nRetCode = FTRIdentifyN( &input, &resNum, &result );
        m_env->ReleaseByteArrayElements( pJTemplate, (jbyte*)Template.pData, JNI_ABORT );

        if( nRetCode != FTR_RETCODE_OK )
            break;

        if( resNum != 0 )
        {
            IdentifyResult.SetIndexValue( nIndex );
            break;
        }
    }

    return nRetCode;
}
