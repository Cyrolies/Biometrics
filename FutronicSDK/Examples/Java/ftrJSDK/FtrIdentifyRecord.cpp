#include "StdAfx.h"
#include "FtrIdentifyRecord.h"

CFtrIdentifyRecord::CFtrIdentifyRecord( JNIEnv *env, jobject obj )
{
    _ASSERT( env != NULL );
    m_env = env;
    if( obj != NULL )
    {
        m_obj = obj;
        m_class = m_env->GetObjectClass( obj );
    } else {
        m_obj = NULL;
        m_class = m_env->FindClass( "com/futronic/SDKHelper/FtrIdentifyRecord" );
    }
    m_KeyValueID = NULL;
    m_TemplateID = NULL;
}

CFtrIdentifyRecord::~CFtrIdentifyRecord(void)
{
}

FTRAPI_RESULT CFtrIdentifyRecord::Initialize()
{
    if( m_class == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_KeyValueID = m_env->GetFieldID( m_class, "m_KeyValue", "[B" );
    if( m_KeyValueID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_TemplateID = m_env->GetFieldID( m_class, "m_Template", "[B" );
    if( m_TemplateID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    return FTR_RETCODE_OK;
}

BOOL CFtrIdentifyRecord::SetNewObject( jobject newObj )
{
    _ASSERT( newObj != NULL );
    if( newObj == NULL )
        return FALSE;

    if( m_env->IsInstanceOf( newObj, m_class ) == JNI_FALSE )
        return FALSE;

    m_obj = newObj;

    return TRUE;
}
