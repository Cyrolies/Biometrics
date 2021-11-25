#pragma once

/**
 * \brief This class represent Java class FtrIdentifyRecord in Native code
 */
class CFtrIdentifyRecord
{
public:
    /**
     * \brief Initialize instance of class CFtrIdentifyRecord
     *
     * @param env the JNI interface pointer (must not be NULL).
     * @param obj a Java object (can be NULL).
     */
    CFtrIdentifyRecord( JNIEnv *env, jobject obj );

    /**
     * \brief Destructor of class CFtrIdentifyRecord.
     */
    ~CFtrIdentifyRecord(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FtrIdentifyRecord.
     */
    FTRAPI_RESULT Initialize();

    /**
     * \brief Function sets new Java object.
     *
     * @param newObj pointer to the new Java object, can not bew NULL
     *
     * @return TRUE if function complete successfully otherwose FALSE.
     */
    BOOL SetNewObject( jobject newObj );

    /**
     * \brief Get template.
     */
    jbyteArray GetTemplate()
    {
        _ASSERT( m_obj != NULL );
        return (jbyteArray)m_env->GetObjectField( m_obj, m_TemplateID );
    }

    /**
     * \brief Get key.
     */
    jbyteArray GetKey()
    {
        _ASSERT( m_obj != NULL );
        return (jbyteArray)m_env->GetObjectField( m_obj, m_KeyValueID );
    }

private:
    JNIEnv      *m_env;             /**< the JNI interface pointer */
    jobject     m_obj;              /**< a Java class object */
    jclass      m_class;            /**< the class of an object */
    jfieldID    m_KeyValueID;       /**< ID of m_KeyValue field. */
    jfieldID    m_TemplateID;       /**< ID of m_Template field. */
};
