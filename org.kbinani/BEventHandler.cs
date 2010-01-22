/*
 * BEventHandler.cs
 * Copyright (C) 2009-2010 kbinani
 *
 * This file is part of org.kbinani.
 *
 * org.kbinani is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\BEventHandler.java
#else
using System;
using System.Reflection;
using System.Collections.Generic;

namespace org.kbinani {
    public class BEventHandler {
        protected MethodInfo m_delegate = null;
        protected Object m_invoker = null;
#if DEBUG
        private static System.IO.StreamWriter m_log = null;
#endif

        private BEventHandler( Type invoker, Object bind, String method_name, Type return_type, Type arg1, Type arg2, bool staticOnly ) {
            try {
                List<MethodInfo> methods = staticOnly ? new List<MethodInfo>( invoker.GetMethods( BindingFlags.Static | BindingFlags.Public ) ) : new List<MethodInfo>( invoker.GetMethods() );
                //methods.AddRange( invoker.GetMethods( BindingFlags.NonPublic ) );
                foreach ( MethodInfo mi in methods ) {
                    if ( mi.Name != method_name ) {
                        continue;
                    }
                    if ( mi.ReturnType != return_type ) {
                        continue;
                    }
                    ParameterInfo[] param = mi.GetParameters();
                    if ( param.Length != 2 ) {
                        continue;
                    }
                    if ( param[0].ParameterType != arg1 || param[1].ParameterType != arg2 ) {
                        continue;
                    }
                    m_delegate = mi;
                    break;
                }
                m_invoker = bind;
                if ( m_delegate == null ) {
#if DEBUG
                    if ( m_log == null ) {
                        m_log = new System.IO.StreamWriter( System.IO.Path.Combine( System.Windows.Forms.Application.StartupPath, "error_event_handler.txt" ) );
                        m_log.AutoFlush = true;
                    }
                    m_log.WriteLine( method_name );
#endif
                    throw new Exception( "cannot create delegate; method_name=" + method_name );
                }
            } catch ( Exception ex ) {
                Console.WriteLine( "BEventHandler#.ctor; ex=" + ex );
            }
        }

        protected BEventHandler( Type invoker, String method_name, Type return_type, Type arg1, Type arg2 )
            : this( invoker, null, method_name, return_type, arg1, arg2, true ) {
        }

        protected BEventHandler( Object invoker, String method_name, Type return_type, Type arg1, Type arg2 )
            : this( invoker.GetType(), invoker, method_name, return_type, arg1, arg2, false ) {
        }

        public BEventHandler( Object invoker, String method_name )
            : this( invoker, method_name, typeof( void ), typeof( Object ), typeof( EventArgs ) ) {
        }

        public BEventHandler( Type invoker, String method_name )
            : this( invoker, method_name, typeof( void ), typeof( Object ), typeof( EventArgs ) ) {
        }

        public bool equals( Object item ) {
            if ( item == null ) {
                return false;
            }
            if ( !(item is BEventHandler) ) {
                return false;
            }
            BEventHandler casted = (BEventHandler)item;
            return m_delegate.Equals( casted.m_delegate );
        }

        public void invoke( params Object[] arguments ) {
            try {
                m_delegate.Invoke( m_invoker, arguments );
            } catch ( Exception ex ) {
                Console.WriteLine( "BEventHandler#invoke; ex=" + ex );
            }
        }

    }
}
#endif
