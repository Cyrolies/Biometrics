#pragma once

/**
 * \brief This is a base class for representing Java classes in Native code
 */
class CFutronicSdkBase
{
public:

    /**
     * \brief Initialize instance of class CFutronicSdkBase
     *
     * @param env the JNI interface pointer (must not be NULL).
     * @param obj a Java object (must not be NULL).
     */
    CFutronicSdkBase( JNIEnv *env, jobject obj );

    /**
     * \brief Deinitialize instance of class CFutronicSdkBase
     */
    virtual ~CFutronicSdkBase(void);

    /**
     * \brief Prepare class to work
     *
     * The function gets methods' identifiers and fields' identifiers for java class 
     * FutronicSdkBase.
     */
    virtual FTRAPI_RESULT Initialize();

    /**
     * \brief get the "Frame source" value
     */
    unsigned long getFrameSource()
    {
        return FSD_FUTRONIC_USB;
    }

    /**
     * \brief get the "Fake Detection" value
     */
    BOOL getFakeDetection()
    {
        jboolean value = m_env->GetBooleanField( m_obj, m_bFakeDetectionID );
        return value == JNI_FALSE ? FALSE : TRUE;
    }

    /**
     * \brief get the "Fake Detection Event Handler" property value
     */
    BOOL getFFDControl()
    {
        jboolean value = m_env->GetBooleanField( m_obj, m_bFFDControlID );
        return value == JNI_FALSE ? FALSE : TRUE;
    }

    /**
     * \brief get the "False Accepting Ratio" property value
     */
    long getFARN()
    {
        return (long)m_env->GetIntField( m_obj, m_FARNID );
    }

    /**
     * \brief get the "Version compatible" property value
     */
    long getVersionCompatible()
    {
        return (long)m_env->GetIntField( m_obj, m_Version );
    }

    /**
     * \brief get the "Fast mode" property value
     */
    BOOL getFastMode()
    {
        jboolean value = m_env->GetBooleanField( m_obj, m_bFastMode );
        return value == JNI_FALSE ? FALSE : TRUE;
    }

    /**
     * \brief State callback function.
     *
     * @param Progress data capture progress information.
     * @param StateMask a bit mask indicating what arguments are provided.
     * @param Signal this signal should be used to interact with a user.
     * @param BitmapWidth contain a width of the bitmap to be displayed.
     * @param BitmapHeight contain a height of the bitmap to be displayed.
     * @param pBitmap contain a bitmap data.
     *
     * @return user response value
     */
    int cbControl( FTR_PROGRESS Progress, int StateMask, int Signal,
                   int BitmapWidth, int BitmapHeight, PBYTE pBitmap );

protected:
    JNIEnv      *m_env;                 /**< the JNI interface pointer */
    jobject     m_obj;                  /**< a Java class object */
    jclass      m_class;                /**< the class of an object */
    jfieldID    m_bFakeDetectionID;     /**< ID of m_bFakeDetection field. */
    jfieldID    m_bFFDControlID;        /**< ID of m_bFFDControl field. */
    jfieldID    m_FARNID;               /**< ID of m_FARN field. */
    jfieldID    m_Version;              /**< Version compatible */
    jmethodID   m_fcbControlID;         /**< ID of cbControl method. */
    jfieldID    m_bFastMode;            /**< TODO */
};
