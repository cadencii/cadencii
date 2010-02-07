#if JAVA
package org.kbinani.editotoini;

#else
using System;

namespace org.kbinani.editotoini {
#endif

    /// <summary>
    /// cadencii本体の設定値のうち，EditOtoIniで必要な設定値のみを取り出したクラス．
    /// </summary>
    public class EditorConfig {
        public String BaseFontName = "MS UI Gothic";
        public float BaseFontSize = 9.0f;
        public String PathResampler = "";
        public String Language = "";
    }

#if !JAVA
}
#endif
