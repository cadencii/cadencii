using System;

namespace Boare.Lib.Vsq {

    using boolean = System.Boolean;

    [Serializable]
    public class IconHandle : ICloneable {
        public String Caption = "";
        public String IconID = "";
        public String IDS = "";
        public int Index;
        public int Length;
        public int Original;
        public int Program;
        public int Language;

        public IconHandle() {
        }

        public boolean equals( IconHandle item ) {
            if ( item == null ) {
                return false;
            } else {
                return IconID.Equals( item.IconID );
            }
        }

        public Object clone() {
            IconHandle ret = new IconHandle();
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.Length = Length;
            ret.Original = Original;
            ret.Program = Program;
            return ret;
        }

#if !JAVA
        public object Clone() {
            return clone();
        }
#endif

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Singer;
            ret.Caption = Caption;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Index = Index;
            ret.Language = Language;
            ret.Length = Length;
            ret.Program = Program;
            return ret;
        }
    }

}
