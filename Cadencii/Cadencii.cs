/*
 * Cadencii.cs
 * Copyright © 2009-2011 kbinani
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
#if JAVA
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.cadencii.*;
#else
using System;
using System.Threading;
using System.Windows.Forms;
using org.kbinani;
using org.kbinani.apputil;

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

    public class Cadencii
    {
#if !JAVA
        delegate void VoidDelegate();
        public static FormSplash splash = null;
        static Thread splashThread = null;
#endif
        private static String mPathVsq = "";
        private static String mPathResource = "";
        private static boolean mPrintVersion = false;

        /// <summary>
        /// 起動時に渡されたコマンドライン引数を評価します。
        /// 戻り値は、コマンドライン引数のうちVSQ,またはXVSQファイルとして指定された引数、または空文字です。
        /// </summary>
        /// <param name="arg"></param>
        private static void parseArguments( String[] arg )
        {
            String currentparse = "";

            for ( int i = 0; i < arg.Length; i++ ) {
                String argi = arg[i];
                if ( str.startsWith( argi, "-" ) ) {
                    currentparse = argi;
                    if ( str.compare( argi, "--version" ) ) {
                        mPrintVersion = true;
                        currentparse = "";
                    }
                } else {
                    if ( str.compare( currentparse, "" ) ) {
                        mPathVsq = argi;
                    } else if ( str.compare( currentparse, "-resources" ) ) {
                        mPathResource = argi;
                    }
                    currentparse = "";
                }
            }
        }

#if JAVA
        public static void main( String[] args ){
            // 引数を解釈
            parseArguments( args );
            if( mPrintVersion ){
                System.out.print( BAssemblyInfo.fileVersion );
                return;
            }
            String file = mPathVsq;
            if ( !str.compare( mPathResource, "" ) ) {
                Resources.setBasePath( mPathResource );
            }
            try{
            	Messaging.loadMessages();
            }catch( Exception ex ){
                Logger.write( Cadencii.class + ".main; ex=" + ex + "\n" );
                serr.println( "Cadencii.main; ex=" + ex );
            }
            AppManager.init();
            AppManager.mMainWindow = new FormMain( file );
            AppManager.mMainWindow.setVisible( true );
        }
#else

        [STAThread]
        public static void Main( String[] args )
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            // 引数を解釈
            parseArguments( args );
            if ( mPrintVersion ) {
                Console.Write( BAssemblyInfo.fileVersion );
                return;
            }
            String file = mPathVsq;
            if ( !str.compare( mPathResource, "" ) ) {
                Resources.setBasePath( mPathResource );
            }

            Logger.setEnabled( false );
            String logfile = PortUtil.createTempFile() + ".txt";

            Logger.setPath( logfile );
#if DEBUG
            Logger.setEnabled( true );
#endif

#if !DEBUG
            try {
#endif

            // 言語設定を読み込み
            try {
                Messaging.loadMessages();
                // システムのデフォルトの言語を調べる．
                // EditorConfigのコンストラクタは，この判定を自動でやるのでそれを利用
                EditorConfig ec = new EditorConfig();
                Messaging.setLanguage( ec.Language );
            } catch ( Exception ex ) {
                Logger.write( typeof( FormMain ) + ".ctor; ex=" + ex + "\n" );
                serr.println( "FormMain#.ctor; ex=" + ex );
            }

            // 開発版の場合の警告ダイアログ
            String str_minor = BAssemblyInfo.fileVersionMinor;
            int minor = 0;
            try {
                minor = str.toi( str_minor );
            } catch ( Exception ex ) {
            }
            if ( (minor % 2) != 0 ) {
                AppManager.showMessageBox(
                    PortUtil.formatMessage(
                        _( "Info: This is test version of Cadencii version {0}" ),
                        BAssemblyInfo.fileVersionMeasure + "." + (minor + 1) ),
                    "Cadencii",
                    org.kbinani.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    org.kbinani.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE );
            }

            // スプラッシュを表示するスレッドを開始
#if !MONO
            splashThread = new Thread( new ThreadStart( showSplash ) );
            splashThread.TrySetApartmentState( ApartmentState.STA );
            splashThread.Start();
#endif

            // AppManagerの初期化
            AppManager.init();

#if ENABLE_SCRIPT
            try {
                ScriptServer.reload();
                PaletteToolServer.init();
            } catch ( Exception ex ) {
                serr.println( "Cadencii::Main; ex=" + ex );
                Logger.write( typeof( Cadencii ) + ".Main; ex=" + ex + "\n" );
            }
#endif
            AppManager.mMainWindow = new FormMain( file );
#if !MONO
            AppManager.mMainWindow.Load += mainWindow_Load;
#endif
            Application.Run( AppManager.mMainWindow );
#if !DEBUG
            } catch ( Exception ex ) {
                String str_ex = getExceptionText( ex, 0 );
                FormCompileResult dialog = new FormCompileResult(
                    _( "Failed to launch Cadencii. Please send the exception report to developer" ),
                    str_ex );
                dialog.setTitle( _( "Error" ) );
                dialog.showDialog();
                if ( splash != null ) {
                    VoidDelegate splash_close = new VoidDelegate( splash.close );
                    if ( splash != null ) {
                        splash.Invoke( splash_close );
                    }
                }
                Logger.write( typeof( Cadencii ) + ".Main; ex=" + ex + "\n" );
            }
#endif
        }

        /// <summary>
        /// 内部例外を含めた例外テキストを再帰的に取得します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="depth_count"></param>
        /// <returns></returns>
        private static String getExceptionText( Exception ex, int depth_count )
        {
            String ret = ex.ToString();
            if ( ex.InnerException != null ) {
                ret += "\n" +
                       "-- InnerException; Depth Level " + depth_count + " -----------------------" +
                       getExceptionText( ex.InnerException, depth_count + 1 );
            }
            return ret;
        }

        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        static void showSplash()
        {
            splash = new FormSplash();
            splash.showDialog( null );
        }

        static void closeSplash()
        {
            splash.close();
        }

        public static void mainWindow_Load( Object sender, EventArgs e )
        {
            if ( splash != null ) {
                VoidDelegate deleg = new VoidDelegate( closeSplash );
                if ( deleg != null ) {
                    splash.Invoke( deleg );
                }
            }
            splash = null;
            AppManager.mMainWindow.Load -= mainWindow_Load;
        }
#endif
    }

#if !JAVA
}
#endif
