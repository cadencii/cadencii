#if ENABLE_SCRIPT
/*
 * ScriptInvoker.cs
 * Copyright © 2009-2010 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using org.kbinani.apputil;
using org.kbinani.vsq;
using org.kbinani.xml;

namespace org.kbinani.cadencii {

    using boolean = System.Boolean;

    public delegate boolean EditVsqScriptDelegate( VsqFile vsq );
    public delegate boolean EditVsqScriptDelegateEx( VsqFileEx vsq );
    public delegate ScriptReturnStatus EditVsqScriptDelegateWithStatus( VsqFile vsq );
    public delegate ScriptReturnStatus EditVsqScriptDelegateExWithStatus( VsqFileEx vsq );
    public delegate String ScriptDelegateGetDisplayName();

    /// <summary>
    /// スクリプトの起動とスクリプト設定の保存を行うためのオブジェクトの纏まり．
    /// FormMain.menuScript.DropDownItems[*].DropDownItems[0]のTagに代入される．
    /// </summary>
    public class ScriptInvoker {
        /// <summary>
        /// スクリプトを起動するためのデリゲート
        /// </summary>
        public Object scriptDelegate;
        /// <summary>
        /// スクリプト本体の型
        /// </summary>
        public Type ScriptType;
        /// <summary>
        /// スクリプトが記述されたファイルのパス
        /// </summary>
        public String ScriptFile;
        /// <summary>
        /// スクリプトをコンパイルしたときのエラーメッセージ
        /// </summary>
        public String ErrorMessage;
        /// <summary>
        /// スクリプト設定を保存し/読み込むためのXMLシリアライザ
        /// </summary>
#if JAVA
        public XmlSerializer Serializer;
#else
        public XmlStaticMemberSerializerEx Serializer;
#endif
        /// <summary>
        /// 最後にスクリプトをコンパイルしたときの，スクリプトが記述されたファイルのタイムスタンプ
        /// </summary>
        public double fileTimestamp;
        /// <summary>
        /// スクリプトの表示名を取得するためのデリゲート
        /// </summary>
        public ScriptDelegateGetDisplayName getDisplayNameDelegate;
    }

}
#endif
