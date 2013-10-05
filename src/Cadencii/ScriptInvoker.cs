#if ENABLE_SCRIPT
/*
 * ScriptInvoker.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using cadencii.apputil;
using cadencii.vsq;
using cadencii.xml;



namespace cadencii
{
    public delegate bool EditVsqScriptDelegate(VsqFile vsq);
    public delegate bool EditVsqScriptDelegateEx(VsqFileEx vsq);
    public delegate ScriptReturnStatus EditVsqScriptDelegateWithStatus(VsqFile vsq);
    public delegate ScriptReturnStatus EditVsqScriptDelegateExWithStatus(VsqFileEx vsq);
    public delegate string ScriptDelegateGetDisplayName();

    /// <summary>
    /// スクリプトの起動とスクリプト設定の保存を行うためのオブジェクトの纏まり．
    /// FormMain.menuScript.DropDownItems[*].DropDownItems[0]のTagに代入される．
    /// </summary>
    public class ScriptInvoker
    {
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
        public string ScriptFile;
        /// <summary>
        /// スクリプトをコンパイルしたときのエラーメッセージ
        /// </summary>
        public string ErrorMessage;
        /// <summary>
        /// スクリプト設定を保存し/読み込むためのXMLシリアライザ
        /// </summary>
        public XmlStaticMemberSerializerEx Serializer;
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
