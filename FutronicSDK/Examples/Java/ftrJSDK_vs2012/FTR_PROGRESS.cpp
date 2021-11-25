#include "StdAfx.h"
#include "FTR_PROGRESS.h"

CFTR_PROGRESS::CFTR_PROGRESS(JNIEnv *env)
{
    m_env = env;
    m_clazz = NULL;
    m_CountID = NULL;
    m_bIsRepeatedID = NULL;
    m_TotalID = NULL;
    m_fConstructorID = NULL;
    m_obj = NULL;
}

CFTR_PROGRESS::~CFTR_PROGRESS(void)
{
    m_env = NULL;
    m_clazz = NULL;
    m_CountID = NULL;
    m_bIsRepeatedID = NULL;
    m_TotalID = NULL;
    m_fConstructorID = NULL;
    m_obj = NULL;
}

FTRAPI_RESULT CFTR_PROGRESS::Initialize()
{
    m_clazz = m_env->FindClass( "com/futronic/SDKHelper/FTR_PROGRESS" );
    if( m_clazz == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_CountID = m_env->GetFieldID( m_clazz, "m_Count", "I" );
    if( m_CountID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_bIsRepeatedID = m_env->GetFieldID( m_clazz, "m_bIsRepeated", "Z" );
    if( m_bIsRepeatedID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_TotalID = m_env->GetFieldID( m_clazz, "m_Total", "I" );
    if( m_TotalID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_fConstructorID = m_env->GetMethodID( m_clazz, "<init>", "()V" );
    if( m_fConstructorID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    return FTR_RETCODE_OK;
}

jobject CFTR_PROGRESS::NewObject( int Count, BOOL bIsRepeated, int Total )
{
    m_obj = m_env->NewObject( m_clazz, m_fConstructorID );
    if( m_obj == NULL )
        return m_obj;
    m_env->SetIntField( m_obj, m_CountID, (jint)Count );
    m_env->SetBooleanField( m_obj, m_bIsRepeatedID, bIsRepeated ? JNI_TRUE : JNI_FALSE );
    m_env->SetIntField( m_obj, m_TotalID, (jint)Total );

    return m_obj;
}
