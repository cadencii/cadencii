/*
 * SoundDriver.h
 * Copyright (C) 2007-2011 kbinani
 *
 * This file is part of cadencii.media.helper.
 *
 * cadencii.media.helper is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.media.helper is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#include <windows.h>
#include <stdio.h>

#ifdef __cplusplus
#   define CADENCII_MEDIA_HELPER_EXTERN_C extern "C"
#else
#   define CADENCII_MEDIA_HELPER_EXTERN_C
#endif

#define CADENCII_MEDIA_HELPER_EXPORT CADENCII_MEDIA_HELPER_EXTERN_C __declspec(dllexport)

CADENCII_MEDIA_HELPER_EXPORT void SoundInit();
CADENCII_MEDIA_HELPER_EXPORT int SoundPrepare( int sample_rate );
CADENCII_MEDIA_HELPER_EXPORT void SoundAppend( double *left, double *right, int length );
CADENCII_MEDIA_HELPER_EXPORT void SoundExit();
CADENCII_MEDIA_HELPER_EXPORT double SoundGetPosition();
CADENCII_MEDIA_HELPER_EXPORT bool SoundIsBusy();
CADENCII_MEDIA_HELPER_EXPORT void SoundWaitForExit();
CADENCII_MEDIA_HELPER_EXPORT void SoundSetResolution( int resolution );
CADENCII_MEDIA_HELPER_EXPORT void SoundKill();
CADENCII_MEDIA_HELPER_EXPORT void SoundUnprepare();

CADENCII_MEDIA_HELPER_EXTERN_C void CALLBACK SoundCallback( HWAVEOUT hwo, unsigned int uMsg, unsigned long dwInstance, unsigned long dwParam1, unsigned long dwParam2 );

#define NUM_BUF 3
