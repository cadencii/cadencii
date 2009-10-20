#include "HelloCallBack.h"

#define ARRAY_SIZE 4
char* szNums[ARRAY_SIZE] = { "zero", "one", "two", "three" };


JNIEXPORT void JNICALL Java_HelloCallBack_test
  (JNIEnv *env, jobject caller, jobject callback)
{
	// CallBackŒÄ‚Ño‚µ—p”z—ñ‚Ìì¬
	jclass jcString = env->FindClass("java/lang/String");
	jobjectArray jArray = 
		env->NewObjectArray( (jsize)ARRAY_SIZE, jcString, NULL );

	for ( int i=0; i<ARRAY_SIZE; i++ ) {
		jstring jstrNum = env->NewStringUTF(szNums[i]);
		env->SetObjectArrayElement( jArray, (jsize)i, jstrNum );
	}

	// CallBackæ‚ÌŽæ“¾
	jclass jcCallback = env->FindClass("HelloCallBackMe");
	if ( jcCallback == NULL ) {
		printf("HelloCallBackMe class could not find.\n");
		return;
	}
	jmethodID mid = env->GetMethodID(jcCallback, 
			"testMe", "([Ljava/lang/String;I)I");
	if ( mid == NULL ) {
		printf("HelloCallBackMe method could not find.\n");
		return;
	}

	jint jnResult = 
		env->CallIntMethod(callback, mid, jArray, (jint)ARRAY_SIZE);

	printf("Return: [%ld] \n", (long)jnResult);
	return;

}
