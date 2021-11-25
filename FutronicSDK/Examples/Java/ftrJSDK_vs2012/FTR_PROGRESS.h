#pragma once

/**
 * \brief This class represent Java class FTR_PROGRESS in Native code
 */
class CFTR_PROGRESS
{
public:
    /**
     * \brief Initialize instance of class CFTR_PROGRESS
     *
     * @param env the JNI interface pointer (must not be NULL).
     */
    CFTR_PROGRESS( JNIEnv *env );

    /**
     * \brief Deinitialize instance of class CFTR_PROGRESS
     */
    ~CFTR_PROGRESS(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FTR_PROGRESS.
     */
    FTRAPI_RESULT Initialize();

    /**
     * \brief Create new instance of java class FTR_PROGRESS and initialize it.
     *
     * @return pointer to object instance if function complete successfully, otherwize NULL.
     */
    jobject NewObject( int Count, BOOL bIsRepeated, int Total );

private:
    JNIEnv      *m_env;             /**< the JNI interface pointer */
    jobject     m_obj;              /**< a Java class object */
    jclass      m_clazz;            /**< the class of an object */
    jfieldID    m_CountID;          /**< ID of m_Count field. */
    jfieldID    m_bIsRepeatedID;    /**< ID of m_bIsRepeated field. */
    jfieldID    m_TotalID;          /**< ID of m_Total field. */
    jmethodID   m_fConstructorID;   /**< ID of constructor method. */
};
