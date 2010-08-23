#if JAVA
package org.kbinani.cadencii;

#else
using System;

namespace org.kbinani.cadencii {
#endif

    /// <summary>
    /// インターフェースWaveReceiver, WaveSender, WaveGeneratorを持つクラスの基底クラス．
    /// </summary>
    public abstract class WaveUnit {
        protected EditorConfig _config;

        /// <summary>
        /// バージョンを表す整数を返す．
        /// 実装上，setConfigに渡す文字列の書式が変わったとき，バージョンを増やすようにする．
        /// </summary>
        /// <returns></returns>
        public abstract int getVersion();

        /// <summary>
        /// 設定を行う．parameterの1字目は，parameterを分割するのに利用する文字を付ける．
        /// 例えば，3個の整数を受け取る実装の場合，次の2つは同じ意味になる(そのように実装する)．
        ///     1)    parameter = "\n1\n2\n3"
        ///     2)    parameter = "\t1\t2\t3"
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void setConfig( String parameter );

        /// <summary>
        /// スコアエディタ全体の設定値を設定する．
        /// </summary>
        /// <param name="config"></param>
        public void setGlobalConfig( EditorConfig config ) {
            _config = config;
        }
    }

#if !JAVA
}
#endif
