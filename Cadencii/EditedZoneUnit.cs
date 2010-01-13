#if JAVA
pacakge org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
#endif

    public class EditedZoneUnit : ICloneable, IComparable<EditedZoneUnit> {
        public int start;
        public int end;

        private EditedZoneUnit(){
        }

        public EditedZoneUnit( int start, int end ){
            this.start = start;
            this.end = end;
        }

        public int compareTo( EditedZoneUnit item ) {
            return this.start - item.start;
        }

#if !JAVA
        public int CompareTo( EditedZoneUnit item ) {
            return compareTo( item );
        }
#endif

        public Object clone() {
            return new EditedZoneUnit( start, end );
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
