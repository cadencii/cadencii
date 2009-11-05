using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.PitchEdit {

    public partial class FormMain : Form {
        const int _DOT_WID = 2;
        const int _SCALEX_INT_MAX = 10;
        const int _SCALEX_INT_MIN = 0;
        const int _SCALEY_INT_MAX = 10;
        const int _SCALEY_INT_MIN = 0;

        private UstFile m_ust;
        private UstTrack m_track;
        private String m_temp;
        public static PitchEditConfig Config = new PitchEditConfig();
        private XmlSerializer m_serializer = null;
        /// <summary>
        /// 画面右端での時間(msec)
        /// </summary>
        private int m_start_to_drawx = 0;
        /// <summary>
        /// x軸方向の描画スケール(pixel/msec)
        /// </summary>
        private float m_scalex = 0.1f;
        private int m_scalex_int = -1;
        /// <summary>
        /// Y軸方向の描画スケール(pixel/cent)
        /// </summary>
        private float m_scaley = 1.0f;
        private int m_scaley_int = 1;

        public FormMain( String temp ) {
            InitializeComponent();
            updateScaleX();
            updateScaleY();
            m_temp = temp;
            Encoding shift_jis = Encoding.GetEncoding( "Shift_JIS" );
            using ( StreamReader sr = new StreamReader( temp, shift_jis ) )
            using ( StreamWriter sw = new StreamWriter( "regen.txt" ) ) {
                string line = "";
                while ( (line = sr.ReadLine()) != null ) {
                    sw.WriteLine( line );
                }
            }
            try {
                m_ust = new UstFile( m_temp );
                m_track = m_ust.getTrack( 0 );
                String config_path = Path.Combine( Application.StartupPath, "config.xml" );
                if ( File.Exists( config_path ) ){
                    m_serializer = new XmlSerializer( typeof( PitchEditConfig ) );
                    PitchEditConfig obj = null;
                    using ( FileStream fs = new FileStream( config_path, FileMode.Open, FileAccess.Read ) ) {
                        obj = (PitchEditConfig)m_serializer.Deserialize( fs );
                    }
                    if ( obj != null ) {
                        Config = obj;
                    }
                }
            } catch ( Exception ex ) {
                bocoree.debug.push_log( "FormMain#.ctor; ex=" + ex );
            }
        }

        [STAThread]
        public static void Main( string[] args ) {
            if ( args.Length <= 0 ) {
                return;
            }
            if ( !File.Exists( args[0] ) ) {
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new FormMain( args[0] ) );
        }

        private void FormMain_FormClosed( object sender, FormClosedEventArgs e ) {
            try {
                if ( m_ust != null ) {
                    m_ust.write( m_temp );
                }
                if ( m_serializer == null ) {
                    m_serializer = new XmlSerializer( typeof( PitchEditConfig ) );
                }
                String config_path = Path.Combine( Application.StartupPath, "config.xml" );
                using ( FileStream fs = new FileStream( config_path, FileMode.Create, FileAccess.Write ) ) {
                    m_serializer.Serialize( fs, Config );
                }
            } catch ( Exception ex ) {
                bocoree.debug.push_log( "FormMain#FormMain_FormClosed; ex=" + ex );
            }
        }

        private void pictMain_Paint( object sender, PaintEventArgs e ) {
            Graphics g = e.Graphics;

            // 100centごとの横線
            for ( int note100 = 0; note100 <= 12700; note100 += 100 ) {
                int y = yCoordFromNote( note100 );
                if ( y > pictMain.Height ) {
                    continue;
                }
                if ( y < 0 ) {
                    break;
                }
                g.DrawLine( Pens.Black, new Point( 0, y ), new Point( pictMain.Width, y ) );
            }
            
            // ピッチを描画
            int clock = 0;
            int count = m_track.getEventCount();
            for ( int index = 0; index < count; index++ ) {
                UstEvent item = m_track.getEvent( index );
                if ( item.Index == int.MinValue ) {
                    clock = 0;
                    continue;
                }
                if ( item.Index == int.MaxValue ) {
                    break;
                }
                int msec = (int)(m_ust.getSecFromClock( clock ) * 1000);
                int sclock = clock - item.PBType;
                if ( item.Pitches == null ) {
                    for ( ; sclock < clock + item.Length; sclock += 5 ) {
                        int t_msec = (int)(m_ust.getSecFromClock( sclock ) * 1000);
                        float note = item.Note * 100.0f;
                        int x = xCoordFromMillisec( t_msec );
                        int y = yCoordFromNote( note );
                        Rectangle rc = new Rectangle( x - _DOT_WID, y - _DOT_WID, _DOT_WID * 2 + 1, _DOT_WID * 2 + 1 );
                        g.FillEllipse( Brushes.Black, rc );
                    }
                } else {
                    for ( int i = 0; i < item.Pitches.Length; i++ ) {
                        sclock += item.PBType;
                        int t_msec = (int)(m_ust.getSecFromClock( sclock ) * 1000);
                        float note = item.Note * 100.0f + item.Pitches[i];
                        int x = xCoordFromMillisec( t_msec );
                        int y = yCoordFromNote( note );
                        Rectangle rc = new Rectangle( x - _DOT_WID, y - _DOT_WID, _DOT_WID * 2 + 1, _DOT_WID * 2 + 1 );
                        g.FillEllipse( Brushes.Black, rc );
                    }
                }
                clock += item.Length;
            }
        }

        private int xCoordFromMillisec( int msec ) {
            return (int)((msec - m_start_to_drawx) * m_scalex);
        }

        private int yCoordFromNote( float note100 ) {
            return pictMain.Height / 2 - (int)((note100 - (12700 -  vScroll.Value)) * m_scaley);
        }

        private void refreshScreen() {
            pictMain.Invalidate();
        }

        private void pictMain_SizeChanged( object sender, EventArgs e ) {
            refreshScreen();
        }

        private void pictMain_MouseMove( object sender, MouseEventArgs e ) {
            refreshScreen();
        }

        private void updateScaleX() {
            // 10のとき10
            //  0のとき0.1
            m_scalex = (float)Math.Pow( 10.0, m_scalex_int / 5.0 - 1.0 );
        }

        private void updateScaleY() {
            // 10のとき2
            //  0のとき0.05
            m_scaley = (float)Math.Pow( 10.0, 0.1903 * m_scaley_int - 1.301 );
            int barwid = (int)(pictMain.Height / m_scaley);
            vScroll.LargeChange = barwid;
            vScroll.Maximum = 12700 + barwid - 1;
        }

        private void btnMoozX_Click( object sender, EventArgs e ) {
            int draft = m_scalex_int - 1;
            if ( draft < _SCALEX_INT_MIN ) {
                draft = _SCALEX_INT_MIN;
            }
            m_scalex_int = draft;
            updateScaleX();
            refreshScreen();
        }

        private void btnZoomX_Click( object sender, EventArgs e ) {
            int draft = m_scalex_int + 1;
            if ( _SCALEX_INT_MAX < draft ) {
                draft = _SCALEX_INT_MAX;
            }
            m_scalex_int = draft;
            updateScaleX();
            refreshScreen();
        }

        private void btnZoomY_Click( object sender, EventArgs e ) {
            int draft = m_scaley_int + 1;
            if ( _SCALEY_INT_MAX < draft ) {
                draft = _SCALEY_INT_MAX;
            }
            m_scaley_int = draft;
            updateScaleY();
            refreshScreen();
        }

        private void btnMoozY_Click( object sender, EventArgs e ) {
            int draft = m_scaley_int - 1;
            if ( draft < _SCALEY_INT_MIN ) {
                draft = _SCALEY_INT_MIN;
            }
            m_scaley_int = draft;
            updateScaleY();
            refreshScreen();
        }

        private void vScroll_ValueChanged( object sender, EventArgs e ) {
            refreshScreen();
        }
    }

}
