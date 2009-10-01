using System;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class NoteHeadHandle : ICloneable {
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        public String Caption = "";
        public int Length;
        public int Duration;
        public int Depth;

        public NoteHeadHandle() {
        }

        public String getDisplayString() {
            String s = IDS;
            if ( !Caption.Equals( "" ) ) {
                s += " (" + Caption + ")";
            }
            return s;
        }

        public object Clone() {
            return clone();
        }

        public Object clone() {
            NoteHeadHandle result = new NoteHeadHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.Caption = this.Caption;
            result.Length = this.Length;
            result.Duration = Duration;
            result.Depth = Depth;
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.NoteHeadHandle;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.Length = Length;
            ret.Duration = Duration;
            ret.Depth = Depth;
            return ret;
        }
    }

}
