#include "StdAfx.h"
#include "FutronicEnrollment.h"

CFutronicEnrollment::CFutronicEnrollment(JNIEnv *env, jobject obj)
    : CFutronicSdkBase( env, obj )
{
    m_bMIOTControlOffID = NULL;
    m_QualityID = NULL;
    m_MaxModelsID = NULL;
    m_TemplateID = NULL;
}

CFutronicEnrollment::~CFutronicEnrollment(void)
{
    m_bMIOTControlOffID = NULL;
    m_QualityID = NULL;
    m_MaxModelsID = NULL;
    m_TemplateID = NULL;
}

FTRAPI_RESULT CFutronicEnrollment::Initialize()
{
    FTRAPI_RESULT nResult = CFutronicSdkBase::Initialize();

    if( nResult == FTR_RETCODE_OK )
    {
        m_bMIOTControlOffID = m_env->GetFieldID( m_class, "m_bMIOTControlOff", "Z" );
        if( m_bMIOTControlOffID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;

        m_QualityID = m_env->GetFieldID( m_class, "m_Quality", "I" );
        if( m_QualityID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;

        m_MaxModelsID = m_env->GetFieldID( m_class, "m_MaxModels", "I" );
        if( m_MaxModelsID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;

        m_TemplateID = m_env->GetFieldID( m_class, "m_Template", "[B" );
        if( m_TemplateID == NULL )
            return FTR_RETCODE_INTERNAL_ERROR;
    }

    return nResult;
}

FTRAPI_RESULT CFutronicEnrollment::Enroll()
{
    FTRAPI_RESULT   nRetCode;
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
    nRetCode = FTRSetParam( FTR_PARAM_MIOT_CONTROL, reinterpret_cast<void*>(getMIOTControlOff()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. set max models
    nRetCode = FTRSetParam( FTR_PARAM_MAX_MODELS, reinterpret_cast<void*>(getMaxModels()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 8. VERSION compatibility
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<void*>(getVersionCompatible()) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 9. FastMode setting
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
    nRetCode = FTREnrollX( (FTR_USER_CTX)this, 
                            FTR_PURPOSE_ENROLL, 
                            &Template, 
                            &eData );
    if( nRetCode == FTR_RETCODE_OK )
    {
        setQuality( eData.dwQuality );
        setTemplate( (BYTE*)Template.pData, Template.dwSize );
    }
    delete pTemplate;

    return nRetCode;
}
