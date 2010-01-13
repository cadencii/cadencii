#if JAVA
package org.kbinani.cadencii;

#else
using System;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class EditedZone : ICloneable {
        private Vector<EditedZoneUnit> series = new Vector<EditedZoneUnit>();

        public EditedZone(){
        }

        public Object clone() {
            EditedZone ret = new EditedZone();
            int count = series.size();
            for ( int i = 0; i < count; i++ ) {
                EditedZoneUnit p = series.get( i );
                ret.series.add( (EditedZoneUnit)p.clone() );
            }
            return ret;
        }

        public void add( int start, int end ) {
            if ( start <= end ) {
                series.add( new EditedZoneUnit( start, end ) );
                normalize();
            }
        }

        /// <summary>
        /// 重複している部分を統合する
        /// </summary>
        private void normalize() {
            boolean changed = true;
            while ( changed ) {
                changed = false;
                int count = series.size();
                for ( int i = 0; i < count - 1; i++ ) {
                    EditedZoneUnit itemi = series.get( i );
                    for ( int j = i + 1; j < count; j++ ) {
                        EditedZoneUnit itemj = series.get( j );
                        if ( itemi.start <= itemj.end && itemj.end < itemi.end ) {
                            itemj.end = itemi.end;
                            series.removeElementAt( i );
                            changed = true;
                            break;
                        } else if ( itemi.start <= itemj.start && itemj.start < itemi.end ) {
                            itemi.end = itemj.end;
                            series.removeElementAt( j );
                            changed = true;
                            break;
                        } else if ( itemj.start <= itemi.start && itemi.end <= itemi.end ) {
                            series.removeElementAt( i );
                            changed = true;
                            break;
                        }
                    }
                    if ( changed ) {
                        break;
                    }
                }
            }
            Collections.sort( series );
        }

        public void clear() {
            series.clear();
        }

#if !JAVA
        public object Clone(){
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
