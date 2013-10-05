/*
 * Cadencii.cs
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
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using cadencii;
using cadencii.apputil;

namespace cadencii
{
    public class Cadencii
    {
        public static FormSplash splash = null;
        static Thread splashThread = null;
        private static string mPathVsq = "";
        private static bool mPrintVersion = false;

        /// <summary>
        /// 起動時に渡されたコマンドライン引数を評価します。
        /// 戻り値は、コマンドライン引数のうちVSQ,またはXVSQファイルとして指定された引数、または空文字です。
        /// </summary>
        /// <param name="arg"></param>
        private static void parseArguments(string[] arg)
        {
            string currentparse = "";

            for (int i = 0; i < arg.Length; i++) {
                string argi = arg[i];
                if (argi.StartsWith("-")) {
                    currentparse = argi;
                    if (argi == "--version") {
                        mPrintVersion = true;
                        currentparse = "";
                    }
                } else {
                    if (currentparse == "") {
                        mPathVsq = argi;
                    }
                    currentparse = "";
                }
            }
        }

        private static void handleUnhandledException(Exception ex)
        {
            ExceptionNotifyFormController controller = new ExceptionNotifyFormController();
            controller.setReportTarget(ex);
            controller.getUi().showDialog(null);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(Cadencii_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 引数を解釈
            parseArguments(args);
            if (mPrintVersion) {
                Console.Write(BAssemblyInfo.fileVersion);
                return;
            }
            string file = mPathVsq;

            Logger.setEnabled(false);
            string logfile = PortUtil.createTempFile() + ".txt";

            Logger.setPath(logfile);
#if DEBUG
            Logger.setEnabled(true);
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
                Messaging.setLanguage(ec.Language);
            } catch (Exception ex) {
                Logger.write(typeof(FormMain) + ".ctor; ex=" + ex + "\n");
                serr.println("FormMain#.ctor; ex=" + ex);
            }

            // 開発版の場合の警告ダイアログ
            string str_minor = BAssemblyInfo.fileVersionMinor;
            int minor = 0;
            try {
                minor = int.Parse(str_minor);
            } catch (Exception ex) {
            }
            if ((minor % 2) != 0) {
                AppManager.showMessageBox(
                    PortUtil.formatMessage(
                        _("Info: This is test version of Cadencii version {0}"),
                        BAssemblyInfo.fileVersionMeasure + "." + (minor + 1)),
                    "Cadencii",
                    cadencii.windows.forms.Utility.MSGBOX_DEFAULT_OPTION,
                    cadencii.windows.forms.Utility.MSGBOX_INFORMATION_MESSAGE);
            }

            // スプラッシュを表示するスレッドを開始
#if !MONO
            splashThread = new Thread(new ThreadStart(showSplash));
            splashThread.TrySetApartmentState(ApartmentState.STA);
            splashThread.Start();
#endif

            // AppManagerの初期化
            AppManager.init();

#if ENABLE_SCRIPT
            try {
                ScriptServer.reload();
                PaletteToolServer.init();
            } catch (Exception ex) {
                serr.println("Cadencii::Main; ex=" + ex);
                Logger.write(typeof(Cadencii) + ".Main; ex=" + ex + "\n");
            }
#endif
            AppManager.mMainWindowController = new FormMainController();
            AppManager.mMainWindow = new FormMain(AppManager.mMainWindowController, file);
#if !MONO
            AppManager.mMainWindow.Load += mainWindow_Load;
#endif
            Application.Run(AppManager.mMainWindow);
#if !DEBUG
            } catch ( Exception ex ) {
                String str_ex = getExceptionText( ex, 0 );
                FormCompileResult dialog = new FormCompileResult(
                    _( "Failed to launch Cadencii. Please send the exception report to developer" ),
                    str_ex );
                dialog.Text = _( "Error" );
                dialog.ShowDialog();
                if ( splash != null ) {
                    splash.Invoke(new Action(splash.Close));
                }
                Logger.write( typeof( Cadencii ) + ".Main; ex=" + ex + "\n" );
            }
#endif
        }

        private static void Cadencii_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = new Exception("unknown exception handled at 'Cadencii::Cadencii_UnhandledException");
            if (e.ExceptionObject != null && e.ExceptionObject is Exception) {
                ex = (Exception)e.ExceptionObject;
            }
            handleUnhandledException(ex);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            handleUnhandledException(e.Exception);
        }

        /// <summary>
        /// 内部例外を含めた例外テキストを再帰的に取得します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="depth_count"></param>
        /// <returns></returns>
        private static string getExceptionText(Exception ex, int depth_count)
        {
            string ret = ex.ToString();
            if (ex.InnerException != null) {
                ret += "\n" +
                       "-- InnerException; Depth Level " + depth_count + " -----------------------" +
                       getExceptionText(ex.InnerException, depth_count + 1);
            }
            return ret;
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        static void showSplash()
        {
            splash = new FormSplash();
            splash.ShowDialog(null);
        }

        static void closeSplash()
        {
            splash.Close();
        }

        public static void mainWindow_Load(Object sender, EventArgs e)
        {
            if (splash != null) {
                splash.Invoke(new Action(closeSplash));
            }
            splash = null;

            // AquesTone2 は UI のインスタンスを生成してからでないと、合成時にクラッシュする。
            // これを回避するため、UI インスタンスの初回生成をココで行う。
            // AquesTone2 DLL のリロード時にも同様の処理が必要だが、これは VSTiDllManager.getAquesTone2Driver にて行う。
            var driver = VSTiDllManager.getAquesTone2Driver();
            if (driver != null) {
                driver.getUi(AppManager.mMainWindow);
            }

            AppManager.mMainWindow.Load -= mainWindow_Load;
        }
    }

}
