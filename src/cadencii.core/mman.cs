/*
 * mman.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii
{

    public class mman
    {
#if __cplusplus
    public:
#endif

        public mman()
        {
        }

        ~mman()
        {
#if __cplusplus
            int size = mMan.size();
            for( int i = 0; i < size; i++ ){
                del( mMan.get( i ) );
            }
            mMan.clear();
            size = mManArray.size();
            for( int i = 0; i < size; i++ ){
                delArr( mManArray.get( i ) );
            }
            mManArray.clear();
#endif
        }

        public void add( object obj )
        {
#if __cplusplus
            mMan.push_back( obj );
#endif
        }

        public void addArr( object obj )
        {
#if __cplusplus
            mManArr.push_back( obj );
#endif
        }

        public static void del( object obj )
        {
#if __cplusplus
            if( obj ){
                delete obj;
            }
#endif
        }

        public static void delArr( object obj )
        {
#if __cplusplus
            if( obj ){
                delete [] obj;
            }
#endif
        }

#if __cplusplus
    private:
#endif

#if __cplusplus
        vector<void *> mMem;
        vector<void *> mMemArray;
#endif

    }

}
