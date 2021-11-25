#pragma once
#include "futronicsdkbase.h"

/**
 * \brief This class represent FutronicVerification Java class in Native code
 */
class CFutronicVerification : public CFutronicSdkBase
{
public:
    /**
     * \brief Initialize instance of class CFutronicVerification.
     *
     * @param env the JNI interface pointer (must not be NULL).
     * @param obj a Java object (must not be NULL).
     */
    CFutronicVerification( JNIEnv *env, jobject obj );

    /**
     * \brief Deinitialize instance of class CFutronicVerification.
     */
    ~CFutronicVerification(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FutronicVerification.
     */
    FTRAPI_RESULT Initialize();

    /**
     * \brief builds the corresponding template and compares it with the source template.
     *
     * Function set parameters specific for verification operation and does verification.
     */
    FTRAPI_RESULT Verification();



private:
    jfieldID    m_TemplateID;           /**< ID of m_Template field. */
    jfieldID    m_bResultID;            /**< ID of m_bResult field. */
    jfieldID    m_FARNValueID;          /**< ID of m_FARNValue field. */

};
