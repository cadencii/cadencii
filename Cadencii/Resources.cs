#if JAVA
package org.kbinani.cadencii;

import java.awt.*;
import java.io.*;
import javax.imageio.*;
import javax.swing.*;
import org.kbinani.*;
#else
using System;
using System.IO;
using org.kbinani;
using org.kbinani.java.awt;

namespace org.kbinani.cadencii{
#endif

    public class Resources{
        private static String basePath = null;
        private static String getBasePath(){
            if( basePath == null ){
                basePath = PortUtil.combinePath( PortUtil.getApplicationStartupPath(), "resources" );
            }
            return basePath;
        }

        private static Image s_alarm_clock = null;
        public static Image get_alarm_clock(){
            if( s_alarm_clock == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "alarm-clock.png" );
#if JAVA
                    s_alarm_clock = ImageIO.read( new File( res_path ) );
#else
                    s_alarm_clock = new Image();
                    s_alarm_clock.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_alarm_clock; ex=" + ex );
                }
            }
            return s_alarm_clock;
        }

        private static Image s_arrow_090 = null;
        public static Image get_arrow_090(){
            if( s_arrow_090 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow-090.png" );
#if JAVA
                    s_arrow_090 = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_090 = new Image();
                    s_arrow_090.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_090; ex=" + ex );
                }
            }
            return s_arrow_090;
        }

        private static Image s_arrow_180 = null;
        public static Image get_arrow_180(){
            if( s_arrow_180 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow-180.png" );
#if JAVA
                    s_arrow_180 = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_180 = new Image();
                    s_arrow_180.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_180; ex=" + ex );
                }
            }
            return s_arrow_180;
        }

        private static Image s_arrow_270 = null;
        public static Image get_arrow_270(){
            if( s_arrow_270 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow-270.png" );
#if JAVA
                    s_arrow_270 = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_270 = new Image();
                    s_arrow_270.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_270; ex=" + ex );
                }
            }
            return s_arrow_270;
        }

        private static Image s_arrow_skip_090 = null;
        public static Image get_arrow_skip_090(){
            if( s_arrow_skip_090 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow-skip-090.png" );
#if JAVA
                    s_arrow_skip_090 = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_skip_090 = new Image();
                    s_arrow_skip_090.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_skip_090; ex=" + ex );
                }
            }
            return s_arrow_skip_090;
        }

        private static Image s_arrow_135 = null;
        public static Image get_arrow_135(){
            if( s_arrow_135 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow_135.png" );
#if JAVA
                    s_arrow_135 = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_135 = new Image();
                    s_arrow_135.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_135; ex=" + ex );
                }
            }
            return s_arrow_135;
        }

        private static Image s_arrow_circle_double = null;
        public static Image get_arrow_circle_double(){
            if( s_arrow_circle_double == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow_circle_double.png" );
#if JAVA
                    s_arrow_circle_double = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_circle_double = new Image();
                    s_arrow_circle_double.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_circle_double; ex=" + ex );
                }
            }
            return s_arrow_circle_double;
        }

        private static Image s_arrow_return = null;
        public static Image get_arrow_return(){
            if( s_arrow_return == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow_return.png" );
#if JAVA
                    s_arrow_return = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_return = new Image();
                    s_arrow_return.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_return; ex=" + ex );
                }
            }
            return s_arrow_return;
        }

        private static Image s_arrow_skip = null;
        public static Image get_arrow_skip(){
            if( s_arrow_skip == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow_skip.png" );
#if JAVA
                    s_arrow_skip = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_skip = new Image();
                    s_arrow_skip.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_skip; ex=" + ex );
                }
            }
            return s_arrow_skip;
        }

        private static Image s_arrow_skip_180 = null;
        public static Image get_arrow_skip_180(){
            if( s_arrow_skip_180 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "arrow_skip_180.png" );
#if JAVA
                    s_arrow_skip_180 = ImageIO.read( new File( res_path ) );
#else
                    s_arrow_skip_180 = new Image();
                    s_arrow_skip_180.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_arrow_skip_180; ex=" + ex );
                }
            }
            return s_arrow_skip_180;
        }

        private static Image s_btn1 = null;
        public static Image get_btn1(){
            if( s_btn1 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "btn1.png" );
#if JAVA
                    s_btn1 = ImageIO.read( new File( res_path ) );
#else
                    s_btn1 = new Image();
                    s_btn1.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_btn1; ex=" + ex );
                }
            }
            return s_btn1;
        }

        private static Image s_btn2 = null;
        public static Image get_btn2(){
            if( s_btn2 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "btn2.png" );
#if JAVA
                    s_btn2 = ImageIO.read( new File( res_path ) );
#else
                    s_btn2 = new Image();
                    s_btn2.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_btn2; ex=" + ex );
                }
            }
            return s_btn2;
        }

        private static Image s_btn3 = null;
        public static Image get_btn3(){
            if( s_btn3 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "btn3.png" );
#if JAVA
                    s_btn3 = ImageIO.read( new File( res_path ) );
#else
                    s_btn3 = new Image();
                    s_btn3.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_btn3; ex=" + ex );
                }
            }
            return s_btn3;
        }

        private static Image s_chevron_small_collapse = null;
        public static Image get_chevron_small_collapse(){
            if( s_chevron_small_collapse == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "chevron-small-collapse.png" );
#if JAVA
                    s_chevron_small_collapse = ImageIO.read( new File( res_path ) );
#else
                    s_chevron_small_collapse = new Image();
                    s_chevron_small_collapse.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_chevron_small_collapse; ex=" + ex );
                }
            }
            return s_chevron_small_collapse;
        }

        private static Image s_clipboard_paste = null;
        public static Image get_clipboard_paste(){
            if( s_clipboard_paste == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "clipboard_paste.png" );
#if JAVA
                    s_clipboard_paste = ImageIO.read( new File( res_path ) );
#else
                    s_clipboard_paste = new Image();
                    s_clipboard_paste.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_clipboard_paste; ex=" + ex );
                }
            }
            return s_clipboard_paste;
        }

        private static Image s_control = null;
        public static Image get_control(){
            if( s_control == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control.png" );
#if JAVA
                    s_control = ImageIO.read( new File( res_path ) );
#else
                    s_control = new Image();
                    s_control.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control; ex=" + ex );
                }
            }
            return s_control;
        }

        private static Image s_control_double = null;
        public static Image get_control_double(){
            if( s_control_double == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control_double.png" );
#if JAVA
                    s_control_double = ImageIO.read( new File( res_path ) );
#else
                    s_control_double = new Image();
                    s_control_double.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control_double; ex=" + ex );
                }
            }
            return s_control_double;
        }

        private static Image s_control_double_180 = null;
        public static Image get_control_double_180(){
            if( s_control_double_180 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control_double_180.png" );
#if JAVA
                    s_control_double_180 = ImageIO.read( new File( res_path ) );
#else
                    s_control_double_180 = new Image();
                    s_control_double_180.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control_double_180; ex=" + ex );
                }
            }
            return s_control_double_180;
        }

        private static Image s_control_pause = null;
        public static Image get_control_pause(){
            if( s_control_pause == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control_pause.png" );
#if JAVA
                    s_control_pause = ImageIO.read( new File( res_path ) );
#else
                    s_control_pause = new Image();
                    s_control_pause.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control_pause; ex=" + ex );
                }
            }
            return s_control_pause;
        }

        private static Image s_control_skip = null;
        public static Image get_control_skip(){
            if( s_control_skip == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control_skip.png" );
#if JAVA
                    s_control_skip = ImageIO.read( new File( res_path ) );
#else
                    s_control_skip = new Image();
                    s_control_skip.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control_skip; ex=" + ex );
                }
            }
            return s_control_skip;
        }

        private static Image s_control_stop = null;
        public static Image get_control_stop(){
            if( s_control_stop == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control_stop.png" );
#if JAVA
                    s_control_stop = ImageIO.read( new File( res_path ) );
#else
                    s_control_stop = new Image();
                    s_control_stop.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control_stop; ex=" + ex );
                }
            }
            return s_control_stop;
        }

        private static Image s_control_stop_180 = null;
        public static Image get_control_stop_180(){
            if( s_control_stop_180 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "control_stop_180.png" );
#if JAVA
                    s_control_stop_180 = ImageIO.read( new File( res_path ) );
#else
                    s_control_stop_180 = new Image();
                    s_control_stop_180.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_control_stop_180; ex=" + ex );
                }
            }
            return s_control_stop_180;
        }

        private static Image s_cross_small = null;
        public static Image get_cross_small(){
            if( s_cross_small == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "cross-small.png" );
#if JAVA
                    s_cross_small = ImageIO.read( new File( res_path ) );
#else
                    s_cross_small = new Image();
                    s_cross_small.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_cross_small; ex=" + ex );
                }
            }
            return s_cross_small;
        }

        private static Image s_disk = null;
        public static Image get_disk(){
            if( s_disk == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "disk.png" );
#if JAVA
                    s_disk = ImageIO.read( new File( res_path ) );
#else
                    s_disk = new Image();
                    s_disk.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_disk; ex=" + ex );
                }
            }
            return s_disk;
        }

        private static Image s_disk__plus = null;
        public static Image get_disk__plus(){
            if( s_disk__plus == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "disk__plus.png" );
#if JAVA
                    s_disk__plus = ImageIO.read( new File( res_path ) );
#else
                    s_disk__plus = new Image();
                    s_disk__plus.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_disk__plus; ex=" + ex );
                }
            }
            return s_disk__plus;
        }

        private static Image s_documents = null;
        public static Image get_documents(){
            if( s_documents == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "documents.png" );
#if JAVA
                    s_documents = ImageIO.read( new File( res_path ) );
#else
                    s_documents = new Image();
                    s_documents.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_documents; ex=" + ex );
                }
            }
            return s_documents;
        }

        private static Image s_drive = null;
        public static Image get_drive(){
            if( s_drive == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "drive.png" );
#if JAVA
                    s_drive = ImageIO.read( new File( res_path ) );
#else
                    s_drive = new Image();
                    s_drive.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_drive; ex=" + ex );
                }
            }
            return s_drive;
        }

        private static Image s_edit_list_order = null;
        public static Image get_edit_list_order(){
            if( s_edit_list_order == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "edit-list-order.png" );
#if JAVA
                    s_edit_list_order = ImageIO.read( new File( res_path ) );
#else
                    s_edit_list_order = new Image();
                    s_edit_list_order.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_edit_list_order; ex=" + ex );
                }
            }
            return s_edit_list_order;
        }

        private static Image s_end_marker = null;
        public static Image get_end_marker(){
            if( s_end_marker == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "end_marker.png" );
#if JAVA
                    s_end_marker = ImageIO.read( new File( res_path ) );
#else
                    s_end_marker = new Image();
                    s_end_marker.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_end_marker; ex=" + ex );
                }
            }
            return s_end_marker;
        }

        private static Image s_eraser = null;
        public static Image get_eraser(){
            if( s_eraser == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "eraser.png" );
#if JAVA
                    s_eraser = ImageIO.read( new File( res_path ) );
#else
                    s_eraser = new Image();
                    s_eraser.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_eraser; ex=" + ex );
                }
            }
            return s_eraser;
        }

        private static Image s_folder__plus = null;
        public static Image get_folder__plus(){
            if( s_folder__plus == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "folder--plus.png" );
#if JAVA
                    s_folder__plus = ImageIO.read( new File( res_path ) );
#else
                    s_folder__plus = new Image();
                    s_folder__plus.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_folder__plus; ex=" + ex );
                }
            }
            return s_folder__plus;
        }

        private static Image s_folder = null;
        public static Image get_folder(){
            if( s_folder == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "folder.png" );
#if JAVA
                    s_folder = ImageIO.read( new File( res_path ) );
#else
                    s_folder = new Image();
                    s_folder.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_folder; ex=" + ex );
                }
            }
            return s_folder;
        }

        private static Image s_folder_horizontal_open = null;
        public static Image get_folder_horizontal_open(){
            if( s_folder_horizontal_open == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "folder_horizontal_open.png" );
#if JAVA
                    s_folder_horizontal_open = ImageIO.read( new File( res_path ) );
#else
                    s_folder_horizontal_open = new Image();
                    s_folder_horizontal_open.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_folder_horizontal_open; ex=" + ex );
                }
            }
            return s_folder_horizontal_open;
        }

        private static Image s_layer_shape_curve = null;
        public static Image get_layer_shape_curve(){
            if( s_layer_shape_curve == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "layer_shape_curve.png" );
#if JAVA
                    s_layer_shape_curve = ImageIO.read( new File( res_path ) );
#else
                    s_layer_shape_curve = new Image();
                    s_layer_shape_curve.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_layer_shape_curve; ex=" + ex );
                }
            }
            return s_layer_shape_curve;
        }

        private static Image s_layer_shape_line = null;
        public static Image get_layer_shape_line(){
            if( s_layer_shape_line == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "layer_shape_line.png" );
#if JAVA
                    s_layer_shape_line = ImageIO.read( new File( res_path ) );
#else
                    s_layer_shape_line = new Image();
                    s_layer_shape_line.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_layer_shape_line; ex=" + ex );
                }
            }
            return s_layer_shape_line;
        }

        private static Image s_pencil = null;
        public static Image get_pencil(){
            if( s_pencil == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "pencil.png" );
#if JAVA
                    s_pencil = ImageIO.read( new File( res_path ) );
#else
                    s_pencil = new Image();
                    s_pencil.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_pencil; ex=" + ex );
                }
            }
            return s_pencil;
        }

        private static Image s_piano = null;
        public static Image get_piano(){
            if( s_piano == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "piano.png" );
#if JAVA
                    s_piano = ImageIO.read( new File( res_path ) );
#else
                    s_piano = new Image();
                    s_piano.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_piano; ex=" + ex );
                }
            }
            return s_piano;
        }

        private static Image s_pin__arrow = null;
        public static Image get_pin__arrow(){
            if( s_pin__arrow == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "pin__arrow.png" );
#if JAVA
                    s_pin__arrow = ImageIO.read( new File( res_path ) );
#else
                    s_pin__arrow = new Image();
                    s_pin__arrow.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_pin__arrow; ex=" + ex );
                }
            }
            return s_pin__arrow;
        }

        private static Image s_pin__arrow_inv = null;
        public static Image get_pin__arrow_inv(){
            if( s_pin__arrow_inv == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "pin__arrow_inv.png" );
#if JAVA
                    s_pin__arrow_inv = ImageIO.read( new File( res_path ) );
#else
                    s_pin__arrow_inv = new Image();
                    s_pin__arrow_inv.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_pin__arrow_inv; ex=" + ex );
                }
            }
            return s_pin__arrow_inv;
        }

        private static Image s_ruler_crop = null;
        public static Image get_ruler_crop(){
            if( s_ruler_crop == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "ruler_crop.png" );
#if JAVA
                    s_ruler_crop = ImageIO.read( new File( res_path ) );
#else
                    s_ruler_crop = new Image();
                    s_ruler_crop.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_ruler_crop; ex=" + ex );
                }
            }
            return s_ruler_crop;
        }

        private static Image s_scissors = null;
        public static Image get_scissors(){
            if( s_scissors == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "scissors.png" );
#if JAVA
                    s_scissors = ImageIO.read( new File( res_path ) );
#else
                    s_scissors = new Image();
                    s_scissors.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_scissors; ex=" + ex );
                }
            }
            return s_scissors;
        }

        private static Image s_slash = null;
        public static Image get_slash(){
            if( s_slash == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "slash.png" );
#if JAVA
                    s_slash = ImageIO.read( new File( res_path ) );
#else
                    s_slash = new Image();
                    s_slash.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_slash; ex=" + ex );
                }
            }
            return s_slash;
        }

        private static Image s_start_marker = null;
        public static Image get_start_marker(){
            if( s_start_marker == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "start_marker.png" );
#if JAVA
                    s_start_marker = ImageIO.read( new File( res_path ) );
#else
                    s_start_marker = new Image();
                    s_start_marker.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_start_marker; ex=" + ex );
                }
            }
            return s_start_marker;
        }

        private static Image s_target__pencil = null;
        public static Image get_target__pencil(){
            if( s_target__pencil == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "target--pencil.png" );
#if JAVA
                    s_target__pencil = ImageIO.read( new File( res_path ) );
#else
                    s_target__pencil = new Image();
                    s_target__pencil.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_target__pencil; ex=" + ex );
                }
            }
            return s_target__pencil;
        }

        private static Image s_VSTonWht = null;
        public static Image get_VSTonWht(){
            if( s_VSTonWht == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "VSTonWht.png" );
#if JAVA
                    s_VSTonWht = ImageIO.read( new File( res_path ) );
#else
                    s_VSTonWht = new Image();
                    s_VSTonWht.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_VSTonWht; ex=" + ex );
                }
            }
            return s_VSTonWht;
        }

#if JAVA
        private static Image s_icon = null;
        public static Image get_icon(){
#else
        private static System.Drawing.Icon s_icon = null;
        public static System.Drawing.Icon get_icon(){
#endif
            if( s_icon == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "icon.ico" );
#if JAVA
                    s_icon = ImageIO.read( new File( res_path ) );
#else
                    s_icon = new System.Drawing.Icon( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_icon; ex=" + ex );
                }
            }
            return s_icon;
        }

#if JAVA
        private static Image s_switch = null;
        public static Image get_switch(){
#else
        private static System.Drawing.Icon s_switch = null;
        public static System.Drawing.Icon get_switch(){
#endif
            if( s_switch == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "switch.ico" );
#if JAVA
                    s_switch = ImageIO.read( new File( res_path ) );
#else
                    s_switch = new System.Drawing.Icon( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_switch; ex=" + ex );
                }
            }
            return s_switch;
        }

        private static Image s_cresc1 = null;
        public static Image get_cresc1(){
            if( s_cresc1 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "cresc1.png" );
#if JAVA
                    s_cresc1 = ImageIO.read( new File( res_path ) );
#else
                    s_cresc1 = new Image();
                    s_cresc1.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_cresc1; ex=" + ex );
                }
            }
            return s_cresc1;
        }

        private static Image s_cresc2 = null;
        public static Image get_cresc2(){
            if( s_cresc2 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "cresc2.png" );
#if JAVA
                    s_cresc2 = ImageIO.read( new File( res_path ) );
#else
                    s_cresc2 = new Image();
                    s_cresc2.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_cresc2; ex=" + ex );
                }
            }
            return s_cresc2;
        }

        private static Image s_cresc3 = null;
        public static Image get_cresc3(){
            if( s_cresc3 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "cresc3.png" );
#if JAVA
                    s_cresc3 = ImageIO.read( new File( res_path ) );
#else
                    s_cresc3 = new Image();
                    s_cresc3.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_cresc3; ex=" + ex );
                }
            }
            return s_cresc3;
        }

        private static Image s_cresc4 = null;
        public static Image get_cresc4(){
            if( s_cresc4 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "cresc4.png" );
#if JAVA
                    s_cresc4 = ImageIO.read( new File( res_path ) );
#else
                    s_cresc4 = new Image();
                    s_cresc4.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_cresc4; ex=" + ex );
                }
            }
            return s_cresc4;
        }

        private static Image s_cresc5 = null;
        public static Image get_cresc5(){
            if( s_cresc5 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "cresc5.png" );
#if JAVA
                    s_cresc5 = ImageIO.read( new File( res_path ) );
#else
                    s_cresc5 = new Image();
                    s_cresc5.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_cresc5; ex=" + ex );
                }
            }
            return s_cresc5;
        }

        private static Image s_dim1 = null;
        public static Image get_dim1(){
            if( s_dim1 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "dim1.png" );
#if JAVA
                    s_dim1 = ImageIO.read( new File( res_path ) );
#else
                    s_dim1 = new Image();
                    s_dim1.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_dim1; ex=" + ex );
                }
            }
            return s_dim1;
        }

        private static Image s_dim2 = null;
        public static Image get_dim2(){
            if( s_dim2 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "dim2.png" );
#if JAVA
                    s_dim2 = ImageIO.read( new File( res_path ) );
#else
                    s_dim2 = new Image();
                    s_dim2.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_dim2; ex=" + ex );
                }
            }
            return s_dim2;
        }

        private static Image s_dim3 = null;
        public static Image get_dim3(){
            if( s_dim3 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "dim3.png" );
#if JAVA
                    s_dim3 = ImageIO.read( new File( res_path ) );
#else
                    s_dim3 = new Image();
                    s_dim3.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_dim3; ex=" + ex );
                }
            }
            return s_dim3;
        }

        private static Image s_dim4 = null;
        public static Image get_dim4(){
            if( s_dim4 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "dim4.png" );
#if JAVA
                    s_dim4 = ImageIO.read( new File( res_path ) );
#else
                    s_dim4 = new Image();
                    s_dim4.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_dim4; ex=" + ex );
                }
            }
            return s_dim4;
        }

        private static Image s_dim5 = null;
        public static Image get_dim5(){
            if( s_dim5 == null ){
                try{
                    String res_path = PortUtil.combinePath( getBasePath(), "dim5.png" );
#if JAVA
                    s_dim5 = ImageIO.read( new File( res_path ) );
#else
                    s_dim5 = new Image();
                    s_dim5.image = new System.Drawing.Bitmap( res_path );
#endif
                }catch( Exception ex ){
                    PortUtil.stderr.println( "Resources#get_dim5; ex=" + ex );
                }
            }
            return s_dim5;
        }

    }
#if !JAVA
}
#endif
