#if ENABLE_AQUESTONE
using System;
using System.Text;
using bocoree;
using bocoree.java.io;
using VstSdk;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public class AquesToneDriver : vstidrv {
        public System.Windows.Forms.Form pluginUi = null;

        private static String[] phones = new String[] { 
            /*アカサタナハマヤラワンガザダバパ
イキシチニヒミリギジビピ
ウクスツヌフムユルグズブプ
エケセテネヘメイェレゲゼデベペ
オコソトノホモヨロヲゴゾドボポ
キャシャチャニャヒャミャリャギャジャツァファビャピャスィトゥ
キュシュチュニュヒュミュリュギュジュウィツィフィビュピュティドゥ
キェシェチェニェヒェミェリェギェジェウェツェフェビェピェズィデュ
キョショチョニョヒョミョリョギョジョウォツォフォビョピョディテュ*/
            "あ",
            "い",
            "う",
            "え",
            "お",
        };

        public override boolean open( string dll_path, int block_size, int sample_rate ) {
            int strlen = 260;
            StringBuilder sb = new StringBuilder( strlen );
            win32.GetProfileString( "AquesTone", "FileKoe_00", "", sb, (uint)strlen );
            String koe_old = sb.ToString();

            String required = getKoeFilePath();
            boolean refresh_winini = false;
            if ( !required.Equals( koe_old ) && !koe_old.Equals( "" ) ) {
                refresh_winini = true;
                win32.WriteProfileString( "AquesTone", "FileKoe_00", required );
            }
            boolean ret = false;
            try {
                ret = base.open( dll_path, block_size, sample_rate );
            } catch ( Exception ex ) {
                ret = false;
#if DEBUG
                PortUtil.stderr.println( "AquesToneDriver#open; ex=" + ex );
#endif
            }
#if DEBUG
            PortUtil.println( "AquesToneDriver#open; try opening UI Window..." );
            try {
                pluginUi = new System.Windows.Forms.Form();
                pluginUi.Text = "AquesToneWindow";
                unsafe {
                    ERect rect = new ERect();
                    aEffect.Dispatch( ref aEffect, AEffectOpcodes.effEditOpen, 0, 0, pluginUi.Handle.ToPointer(), 0.0f );
                }
                pluginUi.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                pluginUi.ClientSize = new System.Drawing.Size( 373, 158 );
                pluginUi.Location = new System.Drawing.Point( int.MinValue, int.MinValue );
                pluginUi.Show();
                pluginUi.Refresh();
                pluginUi.Hide();
            } catch ( Exception ex ) {
                PortUtil.stderr.println( "AquesToneDriver#open; ex=" + ex );
            }
            PortUtil.println( "AquesToneDriver#open; ...done (try opening UI Window)" );
#endif
            if ( refresh_winini ) {
#if DEBUG
                PortUtil.println( "AquesToneDriver#open; refresh_winini; koe_old=" + koe_old );
#endif
                win32.WriteProfileString( "AquesTone", "FileKoe_00", koe_old );
            }
            return ret;
        }

        public override void close() {
            if ( pluginUi != null ) {
                pluginUi.Close();
            }
            base.close();
        }

        private static String getKoeFilePath() {
            String ret = PortUtil.combinePath( AppManager.getCadenciiTempDir(), "jphonefifty.txt" );
            if ( !PortUtil.isFileExists( ret ) ) {
                BufferedWriter bw = null;
                try {
                    bw = new BufferedWriter( new OutputStreamWriter( new FileOutputStream( ret ), "Shift_JIS" ) );
                    foreach ( String s in phones ) {
                        bw.write( s ); bw.newLine();
                    }
                } catch ( Exception ex ) {
                    PortUtil.stderr.println( "AquesToneDriver#getKoeFilePath; ex=" + ex );
                } finally {
                    if ( bw != null ) {
                        try {
                            bw.close();
                        } catch ( Exception ex2 ) {
                        }
                    }
                }
            }
            return ret;
        }
    }

}
#endif
