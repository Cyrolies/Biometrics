#pragma once
#include "futronicsdkbase.h"

/**
 * \brief This class represent FutronicEnrollment Java class in Native code
 */
class CFutronicEnrollment : public CFutronicSdkBase
{
public:
    /**
     * \brief Initialize instance of class CFutronicEnrollment
     *
     * @param env the JNI interface pointer (must not be NULL).
     * @param obj a Java object (must not be NULL).
     */
    CFutronicEnrollment(JNIEnv *env, jobject obj);

    /**
     * \brief Deinitialize instance of class CFutronicEnrollment
     */
    ~CFutronicEnrollment(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FutronicEnrollment.
     */
    FTRAPI_RESULT Initialize();

    /**
     * \brief Creates the fingerprint template for the desired purpose
     *
     * Function set parameters specific fro enrollment operation and does enrollment.
     *
     */
    FTRAPI_RESULT Enroll();

    /**
     * \brief get the MIOT mode setting
     */
    BOOL getMIOTControlOff()
    {
        jboolean value = m_env->GetBooleanField( m_obj, m_bMIOTControlOffID );
        return value == JNI_FALSE ? FALSE : TRUE;
    }

    /**
     * \brief get max number of models in one template.
     */
    long getMaxModels()
    {
        return (long)m_env->GetIntField( m_obj, m_MaxModelsID );
    }

    /**
     * \brief set quality of the template.
     */
    void setQuality( long nQuality )
    {
        m_env->SetIntField( m_obj, m_QualityID, (jint)nQuality );
    }

    /**
     * \brief set template.
     */
    void setTemplate( PBYTE pData, long nSize )
    {
        jbyteArray pTemplate;

        pTemplate = m_env->NewByteArray( (jsize)(nSize) );
        if( pTemplate == NULL )
        {
            _ASSERT( FALSE );
            return ;
        }

        jboolean isCopy;
        jbyte *pJData = m_env->GetByteArrayElements( pTemplate, &isCopy );
        memcpy( pJData, pData, nSize );
        m_env->ReleaseByteArrayElements( pTemplate, pJData, 0 );

        m_env->SetObjectField( m_obj, m_TemplateID, pTemplate );
    }

private:
    jfieldID    m_bMIOTControlOffID;       /**< ID of m_bMIOTControlOff field. */
    jfieldID    m_QualityID;            /**< ID of m_Quality field. */
    jfieldID    m_MaxModelsID;          /**< ID of m_MaxModels field. */
    jfieldID    m_TemplateID;           /**< ID of m_Template field. */
};
