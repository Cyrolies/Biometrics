// ftrSDKHelper.h

#pragma once

using namespace System;

namespace Futronic {
namespace SDKHelper {

    ///<summary>
    /// Represent errors that occur during SDK API functions execution.
    ///</summary>
    public __gc class FutronicException : public Exception
    {
    public:

        ///<summary>
        /// Initialize a new instance of the FutronicException class
        /// with specified error code.
        ///</summary>
        ///<param name="nErrorCode">
        /// Error code
        ///</param>
        FutronicException( int nErrorCode )
            : Exception()
            , m_ErrorCode( nErrorCode )
        {
        }

        ///<summary>
        /// Initialize a new instance of the FutronicException class
        /// with specified error code and error message.
        ///</summary>
        ///<param name="nErrorCode">
        /// Error code
        ///</param>
        ///<param name="message">
        /// Error message
        ///</param>
        FutronicException( int nErrorCode, String *message )
            : Exception( message )
            , m_ErrorCode( nErrorCode )
        {
        }

		///<summary>
        /// Gets a error code.
        ///</summary>
        __property int get_ErrorCode()
        {
            return m_ErrorCode;
        }
    private:
        int     m_ErrorCode;
    };
}  // SDKHelper
}  // Futronic
