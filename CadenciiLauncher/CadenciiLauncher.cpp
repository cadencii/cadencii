/*
 * CadenciiLauncher.cpp
 * Copyright (C) 2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <tchar.h>
#include <vst2.x/audioeffectx.h>
using namespace System;
using namespace System::Threading;
using namespace System::Windows::Forms;
using namespace org::kbinani;
using namespace org::kbinani::cadencii;

void Start( Object^ arg ){
    array<String^>^ args = (array<String^>^)arg;
    Program::Main( args );
}

void RunCadencii( Object^ args, bool wait_for_exit ){
    Thread^ t = gcnew Thread( gcnew ParameterizedThreadStart( Start ) );
    t->SetApartmentState( ApartmentState::STA );
    t->Start( args );
    if( wait_for_exit ){
        while( t->IsAlive ){
            Thread::Sleep( 1000 );
        }
    }
}

int APIENTRY _tWinMain( HINSTANCE hInstance,
                        HINSTANCE hPrevInstance,
                        LPTSTR    lpCmdLine,
                        int       nCmdShow ){
    // command line
    RunCadencii( System::Environment::GetCommandLineArgs(), true );
    return 0;
}

class ClassSlave : public AudioEffectX{
public:
    ClassSlave( audioMasterCallback audioMaster );
    ~ClassSlave();
	// Processing
	virtual void processReplacing (float** inputs, float** outputs, VstInt32 sampleFrames);
	virtual void processDoubleReplacing (double** inputs, double** outputs, VstInt32 sampleFrames);

	// Program
	virtual void setProgramName (char* name);
	virtual void getProgramName (char* name);

	// Parameters
	virtual void setParameter (VstInt32 index, float value);
	virtual float getParameter (VstInt32 index);
	virtual void getParameterLabel (VstInt32 index, char* label);
	virtual void getParameterDisplay (VstInt32 index, char* text);
	virtual void getParameterName (VstInt32 index, char* text);

	virtual bool getEffectName (char* name);
	virtual bool getVendorString (char* text);
	virtual bool getProductString (char* text);
	virtual VstInt32 getVendorVersion ();
};

ClassSlave::ClassSlave( audioMasterCallback audioMaster )
: AudioEffectX( audioMaster, 1, 0 ) { // 1 program, 0 parameter.
    setNumInputs( 0 );
    setNumOutputs( 2 );
    setUniqueID( 'Cdnc' );
    canProcessReplacing();
}

void ClassSlave::processReplacing( float **in, float **out, VstInt32 samples ){
}

void ClassSlave::processDoubleReplacing(double **inputs, double **outputs, VstInt32 sampleFrames){
}

void ClassSlave::setProgramName( char *name ){
}

void ClassSlave::getProgramName( char *name ){
}

void ClassSlave::setParameter( VstInt32 index, float value ){
}

float ClassSlave::getParameter(VstInt32 index){
    return 0.0f;
}

void ClassSlave::getParameterLabel(VstInt32 index, char *label){
}

void ClassSlave::getParameterDisplay( VstInt32 index, char* text ){
}

void ClassSlave::getParameterName(VstInt32 index, char *text){
}

bool ClassSlave::getEffectName(char *name){
    vst_strncpy( name, "Cadencii VSTi", kVstMaxEffectNameLen );
    return true;
}

bool ClassSlave::getVendorString( char* text ){
    vst_strncpy( text, "kbinani", kVstMaxVendorStrLen );
    return true;
}

bool ClassSlave::getProductString(char *text){
    vst_strncpy( text, "Cadencii VSTi", kVstMaxProductStrLen );
    return true;
}

VstInt32 ClassSlave::getVendorVersion(){
    return 3100;
}

ClassSlave::~ClassSlave(){
}

AudioEffect* createEffectInstance( audioMasterCallback audioMaster ){
    RunCadencii( gcnew array<String ^>( 0 ), false );
    return new ClassSlave( audioMaster );
}
