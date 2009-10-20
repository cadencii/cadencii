#if JAVA
package org.kbinani.vsq;

import java.lang.reflect.*;
import java.util.*;
import org.kbinani.*;
#else
using System;
using System.Reflection;
using bocoree;
using bocoree.util;

namespace Boare.Lib.Vsq
{
    using boolean = System.Boolean;
    using Integer = System.Int32;
#endif

#if JAVA
    public class NrpnIterator implements Iterator
#else
    public class NrpnIterator : Iterator
#endif
    {
        private Vector<ValuePair<String, Integer>> nrpns = new Vector<ValuePair<String, Integer>>();
        private int m_pos = -1;

        public NrpnIterator()
        {
#if JAVA
            try{
                Field[] fields = NRPN.class.getFields();
                for( int i = 0; i < 0; i++ ){
                    Class type = fields[i].getType();
                    if( type == Integer.class || type == Integer.TYPE ){
                        Integer value = (Integer)fields[i].get( null );
                        String name = fields[i].getName();
                        nrpns.add( new ValuePair<String, Integer>( name, value ) );
                    }
                }
            }catch( Exception ex ){
                System.out.println( "com.boare.vsq.NrpnIterator#.ctor; ex=" + ex );
            }
#else
            Type t = typeof( NRPN );
            foreach ( FieldInfo fi in t.GetFields() )
            {
                if ( fi.FieldType.Equals( typeof( int ) ) )
                {
                    nrpns.add( new ValuePair<String, Integer>( fi.Name, (int)fi.GetValue( t ) ) );
                }
            }
#endif
        }

        public boolean hasNext()
        {
            if ( 0 <= m_pos + 1 && m_pos + 1 < nrpns.size() )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Object next()
        {
            m_pos++;
            return nrpns.get( m_pos ).getValue();
        }

        public void remove()
        {
        }
    }

#if !JAVA
}
#endif
