// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#pragma unmanaged

#include "windows.h"
#include "FTRAPI.H"

typedef unsigned int (CALLBACK *UnmanagedCallBack)( FTR_PROGRESS Progress, 
                                                    unsigned int StateMask,
                                                    unsigned int Signal, 
                                                    unsigned int BitmapWidth,
                                                    unsigned int BitmapHeight,
                                                    void* pBitmap );

#pragma pack( push, 1 )

struct CommonParameters
{
    unsigned int        FrameSource;
    DGTBOOL             bFakeDetection;
    DGTBOOL             bFFDControl;
    int                 FARNLevel;
    UnmanagedCallBack   fCallBack;
    FTR_VERSION         Version;
    DGTBOOL             bFastMode;            
};

struct EnrollmentParameters
{
    CommonParameters    CommonParam;
    int                 nMaxModels;
    DGTBOOL             bMIOTControlOff;
    unsigned int        Quality;
    int                 nTemplateSize;
    BYTE*               pTemplate;
};

struct VerificationParameters
{
    CommonParameters    CommonParam;
    int                 nTemplateSize;
    BYTE*               pTemplate;
    DGTBOOL             bResult;
    FTR_FARN            FARNLevel;
};

struct GetBaseTemplateParameters
{
    CommonParameters    CommonParam;
    int                 nTemplateSize;
    BYTE*               pTemplate;
};

struct IdentifyParameters
{
    CommonParameters    CommonParam;
    int                 nBaseTemplateSize;
    BYTE*               pBaseTemplate;
};
#pragma pack( pop )

FTRAPI_RESULT EnrollmentProcess( struct EnrollmentParameters *pParams);
FTRAPI_RESULT VerificationProcess( struct VerificationParameters *pParams);
FTRAPI_RESULT GetBaseTemplateProcess( struct GetBaseTemplateParameters *pParams);
FTRAPI_RESULT SetParameters4IdentifyProcess( struct IdentifyParameters *pParams );
FTRAPI_RESULT IdentifyProcess( FTR_IDENTIFY_ARRAY_PTR  pTemplates, int *pnIndex );

#pragma managed
