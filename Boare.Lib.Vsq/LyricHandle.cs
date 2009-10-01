using System;

namespace Boare.Lib.Vsq {

    [Serializable]
    public class LyricHandle : ICloneable {
        public Lyric L0;
        public int Index;

        public LyricHandle() {
        }

        /// <summary>
        /// type = Lyric用のhandleのコンストラクタ
        /// </summary>
        /// <param name="phrase">歌詞</param>
        /// <param name="phonetic_symbol">発音記号</param>
        public LyricHandle( String phrase, String phonetic_symbol ) {
            L0 = new Lyric( phrase, phonetic_symbol );
        }

        public object Clone() {
            LyricHandle ret = new LyricHandle();
            ret.Index = Index;
            ret.L0 = (Lyric)L0.Clone();
            return ret;
        }

        public VsqHandle castToVsqHandle() {
            VsqHandle ret = new VsqHandle();
            ret.m_type = VsqHandleType.Lyric;
            ret.L0 = (Lyric)L0.Clone();
            ret.Index = Index;
            return ret;
        }
    }

}
