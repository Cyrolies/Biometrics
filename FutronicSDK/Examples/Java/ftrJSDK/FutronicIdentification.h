#pragma once
#include "futronicsdkbase.h"

/**
 * \brief This class represent FutronicIdentification Java class in Native code
 */
class CFutronicIdentification : public CFutronicSdkBase
{
public:
    /**
     * \brief Initialize instance of class CFutronicIdentification.
     *
     * @param env the JNI interface pointer (must not be NULL).
     * @param obj a Java object (must not be NULL).
     */
    CFutronicIdentification( JNIEnv *env, jobject obj );

    /**
     * \brief Deinitialize instance of class CFutronicIdentification.
     */
    ~CFutronicIdentification(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FutronicIdentification.
     */
    FTRAPI_RESULT Initialize();

    /**
     * \brief The native function does of the enrollment operation for the identification purpose.
     *
     * Function set parameters specific fro enrollment operation and does enrollment
     * for the identification purpose.
     */
    FTRAPI_RESULT GetBaseTemplateProcess();

    /**
     * \brief The native function does of the enrollment operation for the identification purpose.
     *
     * Function set parameters specific fro enrollment operation and does enrollment
     * for the identification purpose.
     *
     * @param rgTemplates the set of source templates. This is array of FtrIdentifyRecord objects
     *
     * @param Result if the function succeeds, field <code>m_Index</code> contains an 
     * index of the matched record (the first element has an index 0) or -1, if
     * no matching source templates are detected.
     *
     * @return the Futronic SDK return code.
     */
    FTRAPI_RESULT IdentifyProcess( jobjectArray rgTemplates, jobject Result );

private:
    jfieldID    m_BaseTemplateID;           /**< ID of m_BaseTemplate field. */
};
