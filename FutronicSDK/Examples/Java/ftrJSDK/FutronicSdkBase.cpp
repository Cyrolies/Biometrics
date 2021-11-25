#include "StdAfx.h"
#include "FutronicSdkBase.h"
#include "FTR_PROGRESS.h"

void FTR_CBAPI UnmanagedCBControl( FTR_USER_CTX Context,
                                   FTR_STATE StateMask,
                                   FTR_RESPONSE *pResponse,
                                   FTR_SIGNAL Signal,
                                   FTR_BITMAP_PTR pBitmap )
{
    CFutronicSdkBase    *pSdkBase = (CFutronicSdkBase*)Context;
    FTR_PROGRESS        *lpPrgData = (FTR_PROGRESS *)pResponse;
    unsigned int        Height = 0, Width = 0;
    void *              pBitmapData = NULL;

    if( pBitmap != NULL )
    {
        Height = pBitmap->Height;
        Width =  pBitmap->Width;
        pBitmapData = pBitmap->Bitmap.pData;
    }

    *pResponse = pSdkBase->cbControl( *lpPrgData,
                                      StateMask,
                                      Signal,
                                      Width,
                                      Height,
                                      (PBYTE)pBitmapData );
}

CFutronicSdkBase::CFutronicSdkBase(JNIEnv *env, jobject obj)
    : m_env( env )
    , m_obj( obj )
{
    _ASSERT( (env != NULL) && (obj != NULL) );
    m_class = m_env->GetObjectClass( obj );
    _ASSERT( m_class != NULL );
    m_bFakeDetectionID = NULL;
    m_bFFDControlID = NULL;
    m_FARNID = NULL;
    m_fcbControlID = NULL;
}

CFutronicSdkBase::~CFutronicSdkBase(void)
{
    m_bFakeDetectionID = NULL;
    m_bFFDControlID = NULL;
    m_FARNID = NULL;
    m_fcbControlID = NULL;
    m_obj = NULL;
    m_env = NULL;
}

FTRAPI_RESULT CFutronicSdkBase::Initialize()
{
    if( m_class == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_bFakeDetectionID = m_env->GetFieldID( m_class, "m_bFakeDetection", "Z" );
    if( m_bFakeDetectionID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_bFFDControlID = m_env->GetFieldID( m_class, "m_bFFDControl", "Z" );
    if( m_bFFDControlID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_FARNID = m_env->GetFieldID( m_class, "m_FARN", "I" );
    if( m_FARNID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_Version = m_env->GetFieldID( m_class, "m_InternalVersion", "I" );
    if( m_Version == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_fcbControlID = m_env->GetMethodID( m_class, "cbControl", "(Lcom/futronic/SDKHelper/FTR_PROGRESS;IIII[B)I" );
    if( m_fcbControlID == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    m_bFastMode = m_env->GetFieldID( m_class, "m_bFastMode", "Z" );
    if( m_bFastMode == NULL )
        return FTR_RETCODE_INTERNAL_ERROR;

    return FTR_RETCODE_OK;
}

int CFutronicSdkBase::cbControl(FTR_PROGRESS Progress, int StateMask, int Signal,
                                int BitmapWidth, int BitmapHeight, PBYTE pBitmap )
{
    int nRetCode = FTR_CANCEL;

    CFTR_PROGRESS JProgress( m_env );
    if( JProgress.Initialize() != FTR_RETCODE_OK )
        return nRetCode;

    jobject jFTR_PROGRESS = JProgress.NewObject( Progress.dwCount, Progress.bIsRepeated, Progress.dwTotal );
    if( jFTR_PROGRESS == NULL )
        return nRetCode;


    jbyteArray pBitmapData;

    pBitmapData = m_env->NewByteArray( (jsize)(BitmapWidth * BitmapHeight) );
    if( pBitmapData == NULL )
        return nRetCode;

    jbyte *pData = m_env->GetByteArrayElements( pBitmapData, NULL );
    memcpy( pData, pBitmap, BitmapWidth * BitmapHeight );
    m_env->ReleaseByteArrayElements( pBitmapData, pData, 0 );

    nRetCode = (int)m_env->CallIntMethod( m_obj, 
                                          m_fcbControlID,
                                          jFTR_PROGRESS,
                                          (jint)StateMask,
                                          (jint)Signal,
                                          (jint)BitmapWidth,
                                          (jint)BitmapHeight,
                                          pBitmapData );
    return nRetCode;
}
