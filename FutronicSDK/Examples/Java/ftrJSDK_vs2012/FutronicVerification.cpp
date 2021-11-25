#include "StdAfx.h"
#include "FutronicVerification.h"

CFutronicVerification::CFutronicVerification(JNIEnv *env, jobject obj)
    : CFutronicSdkBase( env, obj )
{
    m_TemplateID = NULL;
    m_bResultID = NULL;
    m_FARNValueID = NULL;
}

CFutronicVerification::~CFutronicVerification(void)
{
    m_TemplateID = NULL;
    m_bResultID = NULL;
    m_FARNValueID = NULL;
}

FTRAPI_RESULT CFutronicVerification::Initialize()
{
    FTRAPI_RESULT nResult = CFutronicSdkBase::Initialize();

    if( nResult == FTR_RETCODE_OK )
    {
        m_TemplateID = m_env->GetFieldID( m_class, "m_Template", "[B" );
        if( m_TemplateID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;

        m_bResultID = m_env->GetFieldID( m_class, "m_bResult", "Z" );
        if( m_bResultID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;

        m_FARNValueID = m_env->GetFieldID( m_class, "m_FARNValue", "I" );
        if( m_FARNValueID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;
    }

    return nResult;
}

FTRAPI_RESULT CFutronicVerification::Verification()
{
    FTRAPI_RESULT   nRetCode;
    FTR_DATA        Template;

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

    // prepare arguments
    DGTBOOL bResult;
    FTR_FARN  FARNLevel;
    jboolean isCopy;

    jobject JTempate = m_env->GetObjectField( m_obj, m_TemplateID );


    Template.dwSize = m_env->GetArrayLength( (jarray)JTempate );
    Template.pData = m_env->GetByteArrayElements( (jbyteArray)JTempate, &isCopy );

    // verify operation
    nRetCode = FTRVerifyN( (FTR_USER_CTX)this, 
                            &Template, 
                            &bResult,
                            &FARNLevel );

    m_env->ReleaseByteArrayElements( (jbyteArray)JTempate, (jbyte *)Template.pData, JNI_ABORT );

    if( nRetCode == FTR_RETCODE_OK )
    {
        m_env->SetBooleanField( m_obj, m_bResultID, bResult ? JNI_TRUE : JNI_FALSE );
        m_env->SetIntField( m_obj, m_FARNValueID, (jint)FARNLevel );
    }

    return nRetCode;
}
