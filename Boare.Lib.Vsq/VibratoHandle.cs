using System;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class VibratoHandle : ICloneable {
        public int StartDepth;
        public VibratoBPList DepthBP;
        public int StartRate;
        public VibratoBPList RateBP;
        public int Index;
        public String IconID = "";
        public String IDS = "";
        public int Original;
        public String Caption = "";
        public int Length;

        public VibratoHandle() {
            StartRate = 64;
            StartDepth = 64;
            RateBP = new VibratoBPList();
            DepthBP = new VibratoBPList();
        }

        public String getDisplayString() {
            String s = IDS;
            if ( !Caption.Equals( "" ) ) {
                s += " (" + Caption + ")";
            }
            return s;
        }

        /*public static VibratoHandle[] fromVED( String ved_file, int original ){
        
        }*/

        public object Clone() {
            return clone();
        }

        public Object clone() {
            VibratoHandle result = new VibratoHandle();
            result.Index = Index;
            result.IconID = IconID;
            result.IDS = this.IDS;
            result.Original = this.Original;
            result.Caption = this.Caption;
            result.Length = this.Length;
            result.StartDepth = this.StartDepth;
            result.DepthBP = (VibratoBPList)DepthBP.Clone();
            result.StartRate = this.StartRate;
            result.RateBP = (VibratoBPList)RateBP.Clone();
            return result;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Vibrato;
            ret.Index = Index;
            ret.IconID = IconID;
            ret.IDS = IDS;
            ret.Original = Original;
            ret.Caption = Caption;
            ret.Length = Length;
            ret.StartDepth = StartDepth;
            ret.StartRate = StartRate;
            ret.DepthBP = (VibratoBPList)DepthBP.Clone();
            ret.RateBP = (VibratoBPList)RateBP.Clone();
            return ret;
        }
    }

}
