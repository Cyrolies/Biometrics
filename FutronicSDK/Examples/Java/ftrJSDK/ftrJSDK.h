
#include "jni.h"

#ifdef __cplusplus
extern "C" {
#endif

JNIEXPORT jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicInitialize( JNIEnv *env, jobject obj );
JNIEXPORT void JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicTerminate( JNIEnv *env, jobject obj );
JNIEXPORT jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicEnroll( JNIEnv *env, jobject obj );
JNIEXPORT jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_VerificationProcess( JNIEnv *env, jobject obj );
JNIEXPORT jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_GetBaseTemplateProcess( JNIEnv *env, jobject obj );
JNIEXPORT jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_IdentifyProcess( JNIEnv *env, jobject obj, jobjectArray rgTemplates, jobject Result );
JNIEXPORT jboolean JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicIsTrial( JNIEnv *env, jobject obj );
JNIEXPORT jint JNICALL Java_com_futronic_SDKHelper_FutronicSdkBase_FutronicIdentificationsLeft( JNIEnv *env, jobject obj );

#ifdef __cplusplus
} /* extern "C" */
#endif /* __cplusplus */
