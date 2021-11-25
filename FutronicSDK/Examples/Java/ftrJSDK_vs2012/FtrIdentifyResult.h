#pragma once

/**
 * \brief This class represent Java class FtrIdentifyResult in Native code
 */
class CFtrIdentifyResult
{
public:
    /**
     * \brief Initialize instance of class CFtrIdentifyRecord
     *
     * @param env the JNI interface pointer (must not be NULL).
     * @param obj a Java object (must not be NULL).
     */
    CFtrIdentifyResult( JNIEnv *env, jobject obj );

    /**
     * \brief Destructor of class CFtrIdentifyResult.
     */
    ~CFtrIdentifyResult(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FtrIdentifyResult.
     */
    FTRAPI_RESULT Initialize();

    /**
     * \brief Set an index of the matched record.
     *
     * @param nIndex index of the matched record.
     */
    void SetIndexValue( int nIndex )
    {
        _ASSERT( m_env != NULL && m_obj != NULL );
        m_env->SetIntField( m_obj, m_IndexID, (jint)nIndex );
    }

private:
    JNIEnv      *m_env;             /**< the JNI interface pointer */
    jobject     m_obj;              /**< a Java class object */
    jclass      m_class;            /**< the class of an object */
    jfieldID    m_IndexID;          /**< ID of m_Index field. */
};
