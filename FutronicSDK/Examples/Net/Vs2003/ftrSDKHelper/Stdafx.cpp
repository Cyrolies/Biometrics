// stdafx.cpp : source file that includes just the standard includes
// ftrSDKHelper.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"
#include "ftrSDKHelper.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Reflection;
using namespace Futronic::SDKHelper;

static void FTR_CBAPI UnmanagedCBControl( FTR_USER_CTX Context,
                                          FTR_STATE StateMask,
                                          FTR_RESPONSE *pResponse,
                                          FTR_SIGNAL Signal,
                                          FTR_BITMAP_PTR pBitmap )
{
    void *   ManagedCallBack = (void *)Context;
    FTR_PROGRESS        *lpPrgData = (FTR_PROGRESS *)pResponse;
    unsigned int        Height = 0, Width = 0;
    void *              pBitmapData = NULL;

    if( pBitmap != NULL )
    {
        Height = pBitmap->Height;
        Width =  pBitmap->Width;
        pBitmapData = pBitmap->Bitmap.pData;
    }

    GCHandle gch = GCHandle::op_Explicit( ManagedCallBack );
    Object * args __gc[];
    MFTR_PROGRESS Progress;
    Progress.dwSize = lpPrgData->dwSize;
    Progress.bIsRepeated = lpPrgData->bIsRepeated;
    Progress.dwCount = lpPrgData->dwCount;
    Progress.dwTotal = lpPrgData->dwTotal;

    args = __gc new Object* [ 6 ];
    args[0] = __box( Progress );
    args[1] = __box( StateMask );
    args[2] = __box( Signal );
    args[3] = __box( Width );
    args[4] = __box( Height );
    args[5] = __box( IntPtr( pBitmapData ) );

    Delegate * managed = static_cast<Delegate *>(gch.Target);
    Object * retCode = managed->get_Method()->Invoke( managed->get_Target(), args );
    *pResponse = *static_cast<__box UInt32*>(retCode);
}

#pragma unmanaged

FTRAPI_RESULT EnrollmentProcess( struct EnrollmentParameters *pParams )
{
    FTRAPI_RESULT   nRetCode;
    DWORD           value;
    BOOL            bValue;
    int             TemplateSize;
    BYTE*           pTemplate;
    FTR_DATA        Template;
    FTR_ENROLL_DATA eData;
    FTR_VERSION     ver;

    // 1. set frame source
    value = pParams->CommonParam.FrameSource;
    nRetCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 2. user's calback
    nRetCode = FTRSetParam( FTR_PARAM_CB_CONTROL, reinterpret_cast<void*>(UnmanagedCBControl) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 3. FARN setting
    value = pParams->CommonParam.FARNLevel;
    nRetCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 4. fake mode setting
    bValue = pParams->CommonParam.bFakeDetection;
    nRetCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 5. application takes a control over the Fake Finger Detection (FFD) __event
    bValue = pParams->CommonParam.bFFDControl;
    nRetCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 6. MIOT mode setting
    bValue = pParams->bMIOTControlOff;
    nRetCode = FTRSetParam( FTR_PARAM_MIOT_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. set max models
    value = pParams->nMaxModels;
    nRetCode = FTRSetParam( FTR_PARAM_MAX_MODELS, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 8. VERSION compatibility
    ver = pParams->CommonParam.Version;
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<FTR_PARAM_VALUE>(ver) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 9. Fast mode setting
    bValue = pParams->CommonParam.bFastMode;
    nRetCode = FTRSetParam( FTR_PARAM_FAST_MODE, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // select memory
    nRetCode = FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE, reinterpret_cast<void**>(&TemplateSize) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }
    pTemplate = new BYTE[TemplateSize];
    if( pTemplate == NULL )
    {
        return FTR_RETCODE_NO_MEMORY;
    }

    // prepare arguments
    Template.dwSize = TemplateSize;
    Template.pData = pTemplate;
    eData.dwSize   = sizeof( FTR_ENROLL_DATA );

    // enroll operation
    nRetCode = FTREnrollX( (FTR_USER_CTX)pParams->CommonParam.fCallBack, 
                            FTR_PURPOSE_ENROLL, 
                            &Template, 
                            &eData );
    if( nRetCode == FTR_RETCODE_OK )
    {
        pParams->Quality = eData.dwQuality;
        pParams->nTemplateSize = Template.dwSize;
        pParams->pTemplate = (BYTE*)Template.pData;
    } else {
        delete pTemplate;
    }

    return nRetCode;
}

FTRAPI_RESULT VerificationProcess( struct VerificationParameters *pParams )
{
    FTRAPI_RESULT   nRetCode;
    DWORD           value;
    BOOL            bValue;
    FTR_DATA        Template;
    FTR_VERSION     ver;

    // 1. set frame source
    value = pParams->CommonParam.FrameSource;
    nRetCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 2. user's calback
    nRetCode = FTRSetParam( FTR_PARAM_CB_CONTROL, reinterpret_cast<void*>(UnmanagedCBControl) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 3. FARN setting
    value = pParams->CommonParam.FARNLevel;
    nRetCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 4. fake mode setting
    bValue = pParams->CommonParam.bFakeDetection;
    nRetCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 5. application takes a control over the Fake Finger Detection (FFD) __event
    bValue = pParams->CommonParam.bFFDControl;
    nRetCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 6. VERSION compatibility
    ver = pParams->CommonParam.Version;
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<FTR_PARAM_VALUE>(ver) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. Fast mode setting
    bValue = pParams->CommonParam.bFastMode;
    nRetCode = FTRSetParam( FTR_PARAM_FAST_MODE, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // prepare arguments
    Template.dwSize = pParams->nTemplateSize;
    Template.pData = pParams->pTemplate;

    // verify operation
    nRetCode = FTRVerifyN( (FTR_USER_CTX)pParams->CommonParam.fCallBack, 
                            &Template, 
                            &(pParams->bResult),
                            &(pParams->FARNLevel) );

    return nRetCode;
}

FTRAPI_RESULT GetBaseTemplateProcess( struct GetBaseTemplateParameters *pParams )
{
    FTRAPI_RESULT   nRetCode;
    DWORD           value;
    BOOL            bValue;
    int             TemplateSize;
    BYTE*           pTemplate;
    FTR_DATA        Template;
    FTR_ENROLL_DATA eData;
    FTR_VERSION     ver;

    // 1. set frame source
    value = pParams->CommonParam.FrameSource;
    nRetCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 2. user's calback
    nRetCode = FTRSetParam( FTR_PARAM_CB_CONTROL, reinterpret_cast<void*>(UnmanagedCBControl) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 3. FARN setting
    value = pParams->CommonParam.FARNLevel;
    nRetCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 4. fake mode setting
    bValue = pParams->CommonParam.bFakeDetection;
    nRetCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 5. application takes a control over the Fake Finger Detection (FFD) __event
    bValue = pParams->CommonParam.bFFDControl;
    nRetCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 6. MIOT mode setting
    bValue = FALSE;
    nRetCode = FTRSetParam( FTR_PARAM_MIOT_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. VERSION compatibility
    ver = pParams->CommonParam.Version;
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<FTR_PARAM_VALUE>(ver) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 8. Fast mode setting
    bValue = pParams->CommonParam.bFastMode;
    nRetCode = FTRSetParam( FTR_PARAM_FAST_MODE, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // select memory
    nRetCode = FTRGetParam( FTR_PARAM_MAX_TEMPLATE_SIZE, reinterpret_cast<void**>(&TemplateSize) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }
    pTemplate = new BYTE[TemplateSize];
    if( pTemplate == NULL )
    {
        return FTR_RETCODE_NO_MEMORY;
    }

    // prepare arguments
    Template.dwSize = TemplateSize;
    Template.pData = pTemplate;
    eData.dwSize   = sizeof( FTR_ENROLL_DATA );

    // enroll operation
    nRetCode = FTREnroll( (FTR_USER_CTX)pParams->CommonParam.fCallBack, 
                           FTR_PURPOSE_IDENTIFY, 
                            &Template );
    if( nRetCode == FTR_RETCODE_OK )
    {
        pParams->nTemplateSize = Template.dwSize;
        pParams->pTemplate = (BYTE*)Template.pData;
    } else {
        delete pTemplate;
    }

    return nRetCode;
}

FTRAPI_RESULT SetParameters4IdentifyProcess( struct IdentifyParameters *pParams )
{
    FTRAPI_RESULT   nRetCode;
    DWORD           value;
    BOOL            bValue;
    FTR_DATA        BaseTemplate;
    FTR_VERSION     ver;

    // 1. set frame source
    value = pParams->CommonParam.FrameSource;
    nRetCode = FTRSetParam( FTR_PARAM_CB_FRAME_SOURCE, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 3. FARN setting
    value = pParams->CommonParam.FARNLevel;
    nRetCode = FTRSetParam( FTR_PARAM_MAX_FARN_REQUESTED, reinterpret_cast<void*>(value) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 4. fake mode setting
    bValue = pParams->CommonParam.bFakeDetection;
    nRetCode = FTRSetParam( FTR_PARAM_FAKE_DETECT, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 5. application takes a control over the Fake Finger Detection (FFD) event
    bValue = pParams->CommonParam.bFFDControl;
    nRetCode = FTRSetParam( FTR_PARAM_FFD_CONTROL, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 6. VERSION compatibility
    ver = pParams->CommonParam.Version;
    nRetCode = FTRSetParam( FTR_PARAM_VERSION, reinterpret_cast<FTR_PARAM_VALUE>(ver) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // 7. Fast mode setting
    bValue = pParams->CommonParam.bFastMode;
    nRetCode = FTRSetParam( FTR_PARAM_FAST_MODE, reinterpret_cast<void*>(bValue) );
    if( nRetCode != FTR_RETCODE_OK )
    {
        return nRetCode;
    }

    // set base template
    BaseTemplate.dwSize = pParams->nBaseTemplateSize;
    BaseTemplate.pData = pParams->pBaseTemplate;

    nRetCode = FTRSetBaseTemplate( &BaseTemplate );

    return nRetCode;
}

FTRAPI_RESULT IdentifyProcess( FTR_IDENTIFY_ARRAY_PTR pTemplates, int *pnIndex )
{
    FTRAPI_RESULT           nRetCode = FTR_RETCODE_OK;
    DWORD                   resNum = 0;
    FTR_DATA                Template;
    FTR_IDENTIFY_RECORD     inputRec;
    FTR_IDENTIFY_ARRAY      input;
    FTR_MATCHED_X_RECORD    resultRec;
    FTR_MATCHED_X_ARRAY     result;

    *pnIndex = -1;

    input.TotalNumber = 1;
    input.pMembers = &inputRec;

    inputRec.pData = &Template;

    memset( &resultRec, 0, sizeof( FTR_MATCHED_X_RECORD ) );
    result.TotalNumber = 1;
    result.pMembers = &resultRec;

    inputRec.pData = &Template;

    int nIndex;

    for( nIndex = 0; nIndex < (int)pTemplates->TotalNumber; nIndex++ )
    {
        memcpy( inputRec.KeyValue, pTemplates->pMembers[nIndex].KeyValue, sizeof(FTR_DATA_KEY) );
        Template.dwSize = pTemplates->pMembers[nIndex].pData->dwSize;
        Template.pData = pTemplates->pMembers[nIndex].pData->pData;

        nRetCode = FTRIdentifyN( &input, &resNum, &result );
        if( nRetCode != FTR_RETCODE_OK )
            break;
        if( resNum != 0 )
        {
            *pnIndex = nIndex;
            break;
        }
    }

    return nRetCode;
}

#pragma managed
