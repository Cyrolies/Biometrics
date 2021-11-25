// ftrJSDK.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "FutronicEnrollment.h"
#include "FutronicVerification.h"
#include "FutronicIdentification.h"


#ifdef WIN32
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
    return TRUE;
}

jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicInitialize( JNIEnv *env, jobject obj )
{
    return (jint)FTRInitialize();
}

void JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicTerminate( JNIEnv *env, jobject obj )
{
    FTRTerminate();
}

jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicEnroll( JNIEnv *env, jobject obj )
{
    FTRAPI_RESULT   nResult;
    CFutronicEnrollment enroll( env, obj);

    nResult = enroll.Initialize();
    if( nResult == FTR_RETCODE_OK )
    {
        nResult = enroll.Enroll();
    }

    return (jint)nResult;
}

jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_VerificationProcess( JNIEnv *env, jobject obj )
{
    FTRAPI_RESULT   nResult;
    CFutronicVerification verification( env, obj);

    nResult = verification.Initialize();
    if( nResult == FTR_RETCODE_OK )
    {
        nResult = verification.Verification();
    }

    return (jint)nResult;
}

jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_GetBaseTemplateProcess( JNIEnv *env, jobject obj )
{
    FTRAPI_RESULT   nResult;
    CFutronicIdentification identify( env, obj);

    nResult = identify.Initialize();
    if( nResult == FTR_RETCODE_OK )
    {
        nResult = identify.GetBaseTemplateProcess();
    }

    return (jint)nResult;
}

jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_IdentifyProcess( JNIEnv *env, jobject obj, jobjectArray rgTemplates, jobject Result )
{
    FTRAPI_RESULT           nResult;
    CFutronicIdentification identify( env, obj);

    nResult = identify.Initialize();
    if( nResult == FTR_RETCODE_OK )
    {
        nResult = identify.IdentifyProcess( rgTemplates, Result );
    }

    return (jint)nResult;
}

jboolean JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicIsTrial( JNIEnv *env, jobject obj )
{
    DGT32 nIdentificationsLeft;
    if( FTRGetParam( FTR_PARAM_CHECK_TRIAL, reinterpret_cast<FTR_PARAM_VALUE*>(&nIdentificationsLeft) ) == FTR_RETCODE_OK )
    {
        if( nIdentificationsLeft >= 0 )
        {
            return JNI_TRUE;
        }
    }

    return JNI_FALSE;
}

jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicIdentificationsLeft( JNIEnv *env, jobject obj )
{
    DGT32 nIdentificationsLeft;
    if( FTRGetParam( FTR_PARAM_CHECK_TRIAL, reinterpret_cast<FTR_PARAM_VALUE*>(&nIdentificationsLeft) ) == FTR_RETCODE_OK )
    {
        if( nIdentificationsLeft >= 0 )
        {
            return nIdentificationsLeft;
        }
    }

    return (jint)0x7FFFFFFF;
}

#endif  // WIN32

