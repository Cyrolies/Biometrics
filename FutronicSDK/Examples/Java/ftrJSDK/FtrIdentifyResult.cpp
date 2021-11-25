#include "StdAfx.h"
#include "FtrIdentifyResult.h"

CFtrIdentifyResult::CFtrIdentifyResult( JNIEnv *env, jobject obj )
{
    _ASSERT( env != NULL && obj != NULL );
    m_env = env;
    m_obj = obj;
    m_class = m_env->GetObjectClass( obj );
    m_IndexID = NULL;
}

CFtrIdentifyResult::~CFtrIdentifyResult(void)
{
}

FTRAPI_RESULT CFtrIdentifyResult::Initialize()
{
    if( m_class == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_IndexID = m_env->GetFieldID( m_class, "m_Index", "I" );
    if( m_IndexID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    return FTR_RETCODE_OK;
}
