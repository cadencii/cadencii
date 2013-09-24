/*
 * PluginLoader.cs
 * Copyright © 2013 kbinani
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
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Linq;
using cadencii;
using cadencii.apputil;
using cadencii.vsq;

namespace cadencii
{
    /// <summary>
    /// プラグインファイルを読み込み、コンパイルするクラス
    /// </summary>
    public class PluginLoader
    {
        /// <summary>
        /// 使用中のアセンブリ・キャッシュのフルパス
        /// </summary>
        private static List<string> usedAssemblyChache = new List<string>();

#if ENABLE_SCRIPT
        public Assembly compileScript( string code, List<string> errors )
        {
            Assembly ret = null;

            String md5 = PortUtil.getMD5FromString( code ).Replace( "_", "" );
            String cached_asm_file = Path.Combine( Utility.getCachedAssemblyPath(), md5 + ".dll" );
            bool compiled = false;

            if ( File.Exists( cached_asm_file ) ) {
                ret = Assembly.LoadFile( cached_asm_file );
                if ( !isValidCachedAssembly( ret ) ){
                    ret = null;
                    usedAssemblyChache.Remove( cached_asm_file );
                }
                if ( ret != null ) {
                    if ( !usedAssemblyChache.Contains( cached_asm_file ) ) {
                        usedAssemblyChache.Add( cached_asm_file );
                    }
                }
            }

            CompilerResults cr = null;
            if ( ret == null ) {
                CSharpCodeProvider provider = new CSharpCodeProvider();
                String path = System.Windows.Forms.Application.StartupPath;

                if ( System.IO.Path.GetFileName( System.Windows.Forms.Application.ExecutablePath ).ToLower().StartsWith( "nunit" ) ) {
                    // nunit の場合は、 StartupPath が nunit のものになってしまうため、
                    // CadenciiTest.dll がデプロイされているディレクトリを、アセンブリのロード起点とする。
                    foreach ( var asm in AppDomain.CurrentDomain.GetAssemblies() ) {
                        if ( System.IO.Path.GetFileName( asm.Location ).ToLower() == "cadenciitest.dll" ) {
                            path = System.IO.Path.GetDirectoryName( asm.Location );
                            break;
                        }
                    }
                }

                CompilerParameters parameters = new CompilerParameters( new String[] {
                    Path.Combine( path, "cadencii.vsq.dll" ),
                    Path.Combine( path, "Cadencii.exe" ),
                    Path.Combine( path, "cadencii.media.dll" ),
                    Path.Combine( path, "cadencii.apputil.dll" ),
                    Path.Combine( path, "cadencii.windows.forms.dll" ),
                    Path.Combine( path, "cadencii.core.dll" )
                } );
                parameters.ReferencedAssemblies.Add( "System.Windows.Forms.dll" );
                parameters.ReferencedAssemblies.Add( "System.dll" );
                parameters.ReferencedAssemblies.Add( "System.Drawing.dll" );
                parameters.ReferencedAssemblies.Add( "System.Xml.dll" );
                parameters.GenerateInMemory = false;
                parameters.GenerateExecutable = false;
                parameters.IncludeDebugInformation = true;
                try {
                    cr = provider.CompileAssemblyFromSource( parameters, code );
                    ret = cr.CompiledAssembly;
                    compiled = true;
                } catch ( Exception ex ) {
                    serr.println( "Utility#compileScript; ex=" + ex );
                    Logger.write( typeof( Utility ) + ".compileScript; ex=" + ex + "\n" );
                }
                if ( !compiled ) {
                    int c = cr.Errors.Count;
                    for ( int i = 0; i < c; i++ ) {
                        errors.Add( _( "line" ) + ":" + cr.Errors[i].Line + " " + cr.Errors[i].ErrorText );
                    }
                }
            }

            if ( compiled ) {
                if ( !usedAssemblyChache.Contains( cached_asm_file ) ) {
                    usedAssemblyChache.Add( cached_asm_file );
                }
                if ( File.Exists( cached_asm_file ) ) {
                    try {
                        PortUtil.deleteFile( cached_asm_file );
                    } catch ( Exception ex ) {
                        serr.println( "Utility#compileScript; ex=" + ex );
                        Logger.write( typeof( Utility ) + ".compileScript; ex=" + ex + "\n" );
                    }
                }
                try {
                    PortUtil.copyFile( cr.PathToAssembly, cached_asm_file );
                } catch ( Exception ex ) {
                    serr.println( "Utility#compileScript; ex=" + ex );
                    Logger.write( typeof( Utility ) + ".compileScript; ex=" + ex + "\n" );
                }
            }

            return ret;
        }
#endif // ENABLE_SCRIPT

        /// <summary>
        /// キャシュから読み込んだアセンブリが、このバージョンの Cadencii で使えるかどうかを調べる
        /// </summary>
        /// <param name="assembly">対象のアセンブリ</param>
        /// <returns>使えるようであれば true</returns>
        private static bool isValidCachedAssembly( Assembly assembly )
        {
            return assembly.GetReferencedAssemblies().All( ( asm ) => !asm.Name.Contains( "org.kbinani" ) );
        }

        /// <summary>
        /// 使用されていないアセンブリのキャッシュを削除します
        /// </summary>
        public static void cleanupUnusedAssemblyCache()
        {
            String dir = Utility.getCachedAssemblyPath();
            String[] files = PortUtil.listFiles( dir, ".dll" );
            foreach ( String file in files ) {
                String name = PortUtil.getFileName( file );
                String full = Path.Combine( dir, name );
                if ( !usedAssemblyChache.Contains( full ) ) {
                    try {
                        PortUtil.deleteFile( full );
                    } catch ( Exception ex ) {
                        serr.println( "Utility#cleanupUnusedAssemblyCache; ex=" + ex );
                        Logger.write( typeof( Utility ) + ".cleanupUnusedAssemblyCache; ex=" + ex + "\n" );
                    }
                }
            }
        }

        private string _( string id )
        {
            return Messaging.getMessage( id );
        }

        /// <summary>
        /// 指定されたファイルを読み込んでスクリプトをコンパイルします．
        /// </summary>
        /// <param name="file">スクリプトを発動するのに使用するコンテナを返します．</param>
        /// <returns></returns>
#if ENABLE_SCRIPT
        public ScriptInvoker loadScript( String file )
        {
            ScriptInvoker ret = new ScriptInvoker();
            ret.ScriptFile = file;
            ret.fileTimestamp = PortUtil.getFileLastModified( file );
            // スクリプトの記述のうち、以下のリストに当てはまる部分は空文字に置換される
            string config_file = ScriptServer.configFileNameFromScriptFileName( file );
            string script = "";
            using ( StreamReader sr = new StreamReader( file ) ) {
                script += sr.ReadToEnd();
            }

            var code = createPluginCode( script );
            ret.ErrorMessage = "";

            List<string> errors = new List<string>();
            Assembly testAssembly = compileScript( code, errors );
            if ( testAssembly == null ) {
                ret.scriptDelegate = null;
                if ( errors.Count == 0 ) {
                    ret.ErrorMessage = "failed compiling";
                } else {
                    for ( int i = 0; i < errors.Count; i++ ) {
                        ret.ErrorMessage += errors[i] + "\r\n";
                    }
                }
                return ret;
            } else {
                foreach ( Type implemented in testAssembly.GetTypes() ) {
                    Object scriptDelegate = null;
                    ScriptDelegateGetDisplayName getDisplayNameDelegate = null;

                    MethodInfo get_displayname_delegate = implemented.GetMethod( "GetDisplayName", new Type[] { } );
                    if ( get_displayname_delegate != null && get_displayname_delegate.IsStatic && get_displayname_delegate.IsPublic ) {
                        if ( get_displayname_delegate.ReturnType.Equals( typeof( String ) ) ) {
                            getDisplayNameDelegate = (ScriptDelegateGetDisplayName)Delegate.CreateDelegate( typeof( ScriptDelegateGetDisplayName ), get_displayname_delegate );
                        }
                    }

                    MethodInfo tmi = implemented.GetMethod( "Edit", new Type[] { typeof( VsqFile ) } );
                    if ( tmi != null && tmi.IsStatic && tmi.IsPublic ) {
                        if ( tmi.ReturnType.Equals( typeof( bool ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegate)Delegate.CreateDelegate( typeof( EditVsqScriptDelegate ), tmi );
                        } else if ( tmi.ReturnType.Equals( typeof( ScriptReturnStatus ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegateWithStatus)Delegate.CreateDelegate( typeof( EditVsqScriptDelegateWithStatus ), tmi );
                        }
                    }
                    tmi = implemented.GetMethod( "Edit", new Type[] { typeof( VsqFileEx ) } );
                    if ( tmi != null && tmi.IsStatic && tmi.IsPublic ) {
                        if ( tmi.ReturnType.Equals( typeof( bool ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegateEx)Delegate.CreateDelegate( typeof( EditVsqScriptDelegateEx ), tmi );
                        } else if ( tmi.ReturnType.Equals( typeof( ScriptReturnStatus ) ) ) {
                            scriptDelegate = (EditVsqScriptDelegateExWithStatus)Delegate.CreateDelegate( typeof( EditVsqScriptDelegateExWithStatus ), tmi );
                        }
                    }
                    if ( scriptDelegate != null ) {
                        ret.ScriptType = implemented;
                        ret.scriptDelegate = scriptDelegate;
                        ret.Serializer = new XmlStaticMemberSerializerEx( implemented );
                        ret.getDisplayNameDelegate = getDisplayNameDelegate;

                        if ( !File.Exists( config_file ) ) {
                            continue;
                        }

                        // 設定ファイルからDeserialize
                        System.IO.FileStream fs = null;
                        bool delete_when_exit = false;
                        try {
                            fs = new System.IO.FileStream( config_file, System.IO.FileMode.Open, System.IO.FileAccess.Read );
                            ret.Serializer.deserialize( fs );
                        } catch ( Exception ex ) {
                            serr.println( "Utility#loadScript; ex=" + ex );
                            Logger.write( typeof( Utility ) + ".loadScript; ex=" + ex + "\n" );
                            delete_when_exit = true;
                        } finally {
                            if ( fs != null ) {
                                try {
                                    fs.Close();
                                    if ( delete_when_exit ) {
                                        System.IO.File.Delete( config_file );
                                    }
                                } catch ( Exception ex2 ) {
                                    serr.println( "Utility#loadScript; ex2=" + ex2 );
                                    Logger.write( typeof( Utility ) + ".loadScritp; ex=" + ex2 + "\n" );
                                }
                            }
                        }
                    } else {
                        ret.ErrorMessage = _( "'Edit' Method not implemented" );
                    }
                }
            }
            return ret;
        }
#endif // ENABLE_SCRIPT

        /// <summary>
        /// プラグインのソースコード文面から、プラグインのバージョンを推定する
        /// </summary>
        /// <param name="code">プラグインのソースコード</param>
        /// <returns>推定されたプラグインのバージョン</returns>
        private PluginVersion estimateVersionByCode( string code )
        {
            if ( code.Contains( "Boare." ) ) {
                return PluginVersion.Version1;
            } else if ( code.Contains( "org.kbinani." ) ) {
                return PluginVersion.Version2;
            } else {
                return PluginVersion.Latest;
            }
        }

        /// <summary>
        /// ファイルから読み込んだプラグインのソースコードに適切な prefix, suffix コードを挿入したソースコードを作成する
        /// </summary>
        /// <param name="code">ファイルから読み込んだプラグインのソースコード</param>
        /// <returns>加工済みのソースコード</returns>
        private string createPluginCode( string code )
        {
            ScriptProcessor processor = null;
            switch ( estimateVersionByCode( code ) ) {
                case PluginVersion.Version1: {
                    processor = new ScriptProcessorVersion1();
                    break;
                }
                case PluginVersion.Version2: {
                    processor = new ScriptProcessorVersion2();
                    break;
                }
                case PluginVersion.Latest: {
                    processor = new ScriptProcessorVersion3();
                    break;
                }
            }
            return processor.process( code );
        }
    }
}
