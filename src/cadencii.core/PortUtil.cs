/*
 * PortUtil.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Text;
using cadencii.java.awt;
using cadencii.java.util;

namespace cadencii
{

    public class PortUtil
    {
        public static Color AliceBlue = new Color(240, 248, 255);
        public static Color AntiqueWhite = new Color(250, 235, 215);
        public static Color Aqua = new Color(0, 255, 255);
        public static Color Aquamarine = new Color(127, 255, 212);
        public static Color Azure = new Color(240, 255, 255);
        public static Color Beige = new Color(245, 245, 220);
        public static Color Bisque = new Color(255, 228, 196);
        public static Color Black = new Color(0, 0, 0);
        public static Color BlanchedAlmond = new Color(255, 235, 205);
        public static Color Blue = new Color(0, 0, 255);
        public static Color BlueViolet = new Color(138, 43, 226);
        public static Color Brown = new Color(165, 42, 42);
        public static Color BurlyWood = new Color(222, 184, 135);
        public static Color CadetBlue = new Color(95, 158, 160);
        public static Color Chartreuse = new Color(127, 255, 0);
        public static Color Chocolate = new Color(210, 105, 30);
        public static Color Coral = new Color(255, 127, 80);
        public static Color CornflowerBlue = new Color(100, 149, 237);
        public static Color Cornsilk = new Color(255, 248, 220);
        public static Color Crimson = new Color(220, 20, 60);
        public static Color Cyan = new Color(0, 255, 255);
        public static Color DarkBlue = new Color(0, 0, 139);
        public static Color DarkCyan = new Color(0, 139, 139);
        public static Color DarkGoldenrod = new Color(184, 134, 11);
        public static Color DarkGray = new Color(169, 169, 169);
        public static Color DarkGreen = new Color(0, 100, 0);
        public static Color DarkKhaki = new Color(189, 183, 107);
        public static Color DarkMagenta = new Color(139, 0, 139);
        public static Color DarkOliveGreen = new Color(85, 107, 47);
        public static Color DarkOrange = new Color(255, 140, 0);
        public static Color DarkOrchid = new Color(153, 50, 204);
        public static Color DarkRed = new Color(139, 0, 0);
        public static Color DarkSalmon = new Color(233, 150, 122);
        public static Color DarkSeaGreen = new Color(143, 188, 139);
        public static Color DarkSlateBlue = new Color(72, 61, 139);
        public static Color DarkSlateGray = new Color(47, 79, 79);
        public static Color DarkTurquoise = new Color(0, 206, 209);
        public static Color DarkViolet = new Color(148, 0, 211);
        public static Color DeepPink = new Color(255, 20, 147);
        public static Color DeepSkyBlue = new Color(0, 191, 255);
        public static Color DimGray = new Color(105, 105, 105);
        public static Color DodgerBlue = new Color(30, 144, 255);
        public static Color Firebrick = new Color(178, 34, 34);
        public static Color FloralWhite = new Color(255, 250, 240);
        public static Color ForestGreen = new Color(34, 139, 34);
        public static Color Fuchsia = new Color(255, 0, 255);
        public static Color Gainsboro = new Color(220, 220, 220);
        public static Color GhostWhite = new Color(248, 248, 255);
        public static Color Gold = new Color(255, 215, 0);
        public static Color Goldenrod = new Color(218, 165, 32);
        public static Color Gray = new Color(128, 128, 128);
        public static Color Green = new Color(0, 128, 0);
        public static Color GreenYellow = new Color(173, 255, 47);
        public static Color Honeydew = new Color(240, 255, 240);
        public static Color HotPink = new Color(255, 105, 180);
        public static Color IndianRed = new Color(205, 92, 92);
        public static Color Indigo = new Color(75, 0, 130);
        public static Color Ivory = new Color(255, 255, 240);
        public static Color Khaki = new Color(240, 230, 140);
        public static Color Lavender = new Color(230, 230, 250);
        public static Color LavenderBlush = new Color(255, 240, 245);
        public static Color LawnGreen = new Color(124, 252, 0);
        public static Color LemonChiffon = new Color(255, 250, 205);
        public static Color LightBlue = new Color(173, 216, 230);
        public static Color LightCoral = new Color(240, 128, 128);
        public static Color LightCyan = new Color(224, 255, 255);
        public static Color LightGoldenrodYellow = new Color(250, 250, 210);
        public static Color LightGreen = new Color(144, 238, 144);
        public static Color LightGray = new Color(211, 211, 211);
        public static Color LightPink = new Color(255, 182, 193);
        public static Color LightSalmon = new Color(255, 160, 122);
        public static Color LightSeaGreen = new Color(32, 178, 170);
        public static Color LightSkyBlue = new Color(135, 206, 250);
        public static Color LightSlateGray = new Color(119, 136, 153);
        public static Color LightSteelBlue = new Color(176, 196, 222);
        public static Color LightYellow = new Color(255, 255, 224);
        public static Color Lime = new Color(0, 255, 0);
        public static Color LimeGreen = new Color(50, 205, 50);
        public static Color Linen = new Color(250, 240, 230);
        public static Color Magenta = new Color(255, 0, 255);
        public static Color Maroon = new Color(128, 0, 0);
        public static Color MediumAquamarine = new Color(102, 205, 170);
        public static Color MediumBlue = new Color(0, 0, 205);
        public static Color MediumOrchid = new Color(186, 85, 211);
        public static Color MediumPurple = new Color(147, 112, 219);
        public static Color MediumSeaGreen = new Color(60, 179, 113);
        public static Color MediumSlateBlue = new Color(123, 104, 238);
        public static Color MediumSpringGreen = new Color(0, 250, 154);
        public static Color MediumTurquoise = new Color(72, 209, 204);
        public static Color MediumVioletRed = new Color(199, 21, 133);
        public static Color MidnightBlue = new Color(25, 25, 112);
        public static Color MintCream = new Color(245, 255, 250);
        public static Color MistyRose = new Color(255, 228, 225);
        public static Color Moccasin = new Color(255, 228, 181);
        public static Color NavajoWhite = new Color(255, 222, 173);
        public static Color Navy = new Color(0, 0, 128);
        public static Color OldLace = new Color(253, 245, 230);
        public static Color Olive = new Color(128, 128, 0);
        public static Color OliveDrab = new Color(107, 142, 35);
        public static Color Orange = new Color(255, 165, 0);
        public static Color OrangeRed = new Color(255, 69, 0);
        public static Color Orchid = new Color(218, 112, 214);
        public static Color PaleGoldenrod = new Color(238, 232, 170);
        public static Color PaleGreen = new Color(152, 251, 152);
        public static Color PaleTurquoise = new Color(175, 238, 238);
        public static Color PaleVioletRed = new Color(219, 112, 147);
        public static Color PapayaWhip = new Color(255, 239, 213);
        public static Color PeachPuff = new Color(255, 218, 185);
        public static Color Peru = new Color(205, 133, 63);
        public static Color Pink = new Color(255, 192, 203);
        public static Color Plum = new Color(221, 160, 221);
        public static Color PowderBlue = new Color(176, 224, 230);
        public static Color Purple = new Color(128, 0, 128);
        public static Color Red = new Color(255, 0, 0);
        public static Color RosyBrown = new Color(188, 143, 143);
        public static Color RoyalBlue = new Color(65, 105, 225);
        public static Color SaddleBrown = new Color(139, 69, 19);
        public static Color Salmon = new Color(250, 128, 114);
        public static Color SandyBrown = new Color(244, 164, 96);
        public static Color SeaGreen = new Color(46, 139, 87);
        public static Color SeaShell = new Color(255, 245, 238);
        public static Color Sienna = new Color(160, 82, 45);
        public static Color Silver = new Color(192, 192, 192);
        public static Color SkyBlue = new Color(135, 206, 235);
        public static Color SlateBlue = new Color(106, 90, 205);
        public static Color SlateGray = new Color(112, 128, 144);
        public static Color Snow = new Color(255, 250, 250);
        public static Color SpringGreen = new Color(0, 255, 127);
        public static Color SteelBlue = new Color(70, 130, 180);
        public static Color Tan = new Color(210, 180, 140);
        public static Color Teal = new Color(0, 128, 128);
        public static Color Thistle = new Color(216, 191, 216);
        public static Color Tomato = new Color(255, 99, 71);
        public static Color Turquoise = new Color(64, 224, 208);
        public static Color Violet = new Color(238, 130, 238);
        public static Color Wheat = new Color(245, 222, 179);
        public static Color White = new Color(255, 255, 255);
        public static Color WhiteSmoke = new Color(245, 245, 245);
        public static Color Yellow = new Color(255, 255, 0);
        public static Color YellowGreen = new Color(154, 205, 50);

        public const int YES_OPTION = 0;
        public const int NO_OPTION = 1;
        public const int CANCEL_OPTION = 2;
        public const int OK_OPTION = 0;
        public const int CLOSED_OPTION = -1;

        static PortUtil()
        {
        }

        private PortUtil()
        {
        }

        /// <summary>
        /// java:コンポーネントのnameプロパティを返します。C#:コントロールのNameプロパティを返します。
        /// objがnullだったり、型がComponent/Controlでない場合は空文字を返します。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string getComponentName(Object obj)
        {
            if (obj == null) {
                return "";
            }
            if (obj is System.Windows.Forms.Control) {
                return ((System.Windows.Forms.Control)obj).Name;
            } else {
                return "";
            }
        }

        public static string formatMessage(string patern, params Object[] args)
        {
            return string.Format(patern, args);
        }

        /// <summary>
        /// 単位は秒
        /// </summary>
        /// <returns></returns>
        public static double getCurrentTime()
        {
            return DateTime.Now.Ticks * 100.0 / 1e9;
        }

        public static Rectangle getScreenBounds(System.Windows.Forms.Control w)
        {
            System.Drawing.Rectangle rc = System.Windows.Forms.Screen.GetWorkingArea(w);
            return new Rectangle(rc.X, rc.Y, rc.Width, rc.Height);
        }

        #region Clipboard
        public static void setClipboardText(string value)
        {
            System.Windows.Forms.Clipboard.SetText(value);
        }

        public static string getClipboardText()
        {
            return System.Windows.Forms.Clipboard.GetText();
        }
        #endregion

        #region BitConverter

        public static byte[] getbytes_int64_le(long data)
        {
            byte[] dat = new byte[8];
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[7] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_uint32_le(long data)
        {
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[0] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_int32_le(int data)
        {
            long v = data;
            if (v < 0) {
                v += 4294967296L;
            }
            return getbytes_uint32_le(v);
        }

        public static byte[] getbytes_int32_be(int data)
        {
            long v = data;
            if (v < 0) {
                v += 4294967296L;
            }
            return getbytes_uint32_be(v);
        }

        public static byte[] getbytes_int64_be(long data)
        {
            byte[] dat = new byte[8];
            dat[7] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[6] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[5] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[4] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[3] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[2] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            data = (data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_uint32_be(long data)
        {
            byte[] dat = new byte[4];
            data = 0xffffffff & data;
            dat[3] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[2] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[1] = (byte)(data & (byte)0xff);
            data = data >> 8;
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_int16_le(short data)
        {
            int i = data;
            if (i < 0) {
                i += 65536;
            }
            return getbytes_uint16_le(i);
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static byte[] getbytes_uint16_le(int data)
        {
            byte[] dat = new byte[2];
            dat[0] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[1] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static byte[] getbytes_uint16_be(int data)
        {
            byte[] dat = new byte[2];
            dat[1] = (byte)(data & (byte)0xff);
            data = (byte)(data >> 8);
            dat[0] = (byte)(data & (byte)0xff);
            return dat;
        }

        public static long make_int64_le(byte[] buf)
        {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[7]) << 8) | (0xff & buf[6])) << 8) | (0xff & buf[5])) << 8) | (0xff & buf[4])) << 8) | (0xff & buf[3])) << 8 | (0xff & buf[2])) << 8) | (0xff & buf[1])) << 8 | (0xff & buf[0]);
        }

        public static long make_int64_be(byte[] buf)
        {
            return (long)((long)((long)((long)((long)((long)((long)((long)((long)((((0xff & buf[0]) << 8) | (0xff & buf[1])) << 8) | (0xff & buf[2])) << 8) | (0xff & buf[3])) << 8) | (0xff & buf[4])) << 8 | (0xff & buf[5])) << 8) | (0xff & buf[6])) << 8 | (0xff & buf[7]);
        }

        public static long make_uint32_le(byte[] buf, int index)
        {
            return (long)((long)((long)((long)(((0xff & buf[index + 3]) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index]);
        }

        public static long make_uint32_le(byte[] buf)
        {
            return make_uint32_le(buf, 0);
        }

        public static long make_uint32_be(byte[] buf, int index)
        {
            return (long)((long)((long)((long)(((0xff & buf[index]) << 8) | (0xff & buf[index + 1])) << 8) | (0xff & buf[index + 2])) << 8) | (0xff & buf[index + 3]);
        }

        public static long make_uint32_be(byte[] buf)
        {
            return make_uint32_be(buf, 0);
        }

        public static int make_int32_le(byte[] buf)
        {
            long v = make_uint32_le(buf);
            if (v >= 2147483647L) {
                v -= 4294967296L;
            }
            return (int)v;
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static int make_uint16_le(byte[] buf, int index)
        {
            return (int)((int)((0xff & buf[index + 1]) << 8) | (0xff & buf[index]));
        }

        public static int make_uint16_le(byte[] buf)
        {
            return make_uint16_le(buf, 0);
        }

        public static int make_uint16_be(byte[] buf, int index)
        {
            return (int)((int)((0xff & buf[index]) << 8) | (0xff & buf[index + 1]));
        }

        public static int make_uint16_be(byte[] buf)
        {
            return make_uint16_be(buf, 0);
        }

        /// <summary>
        /// compatible to BitConverter
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static short make_int16_le(byte[] buf, int index)
        {
            int i = make_uint16_le(buf, index);
            if (i >= 32768) {
                i = i - 65536;
            }
            return (short)i;
        }

        public static short make_int16_le(byte[] buf)
        {
            return make_int16_le(buf, 0);
        }

        public static double make_double_le(byte[] buf)
        {
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = buf[i];
                    buf[i] = buf[7 - i];
                    buf[7 - i] = d;
                }
            }
            return BitConverter.ToDouble(buf, 0);
        }

        public static double make_double_be(byte[] buf)
        {
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = buf[i];
                    buf[i] = buf[7 - i];
                    buf[7 - i] = d;
                }
            }
            return BitConverter.ToDouble(buf, 0);
        }

        public static float make_float_le(byte[] buf)
        {
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = buf[i];
                    buf[i] = buf[3 - i];
                    buf[3 - i] = d;
                }
            }
            return BitConverter.ToSingle(buf, 0);
        }

        public static float make_float_be(byte[] buf)
        {
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = buf[i];
                    buf[i] = buf[3 - i];
                    buf[3 - i] = d;
                }
            }
            return BitConverter.ToSingle(buf, 0);
        }

        public static byte[] getbytes_double_le(double value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = ret[i];
                    ret[i] = ret[7 - i];
                    ret[7 - i] = d;
                }
            }
            return ret;
        }

        public static byte[] getbytes_double_be(double value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 4; i++) {
                    byte d = ret[i];
                    ret[i] = ret[7 - i];
                    ret[7 - i] = d;
                }
            }
            return ret;
        }

        public static byte[] getbytes_float_le(float value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = ret[i];
                    ret[i] = ret[3 - i];
                    ret[3 - i] = d;
                }
            }
            return ret;
        }

        public static byte[] getbytes_float_be(float value)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) {
                for (int i = 0; i < 2; i++) {
                    byte d = ret[i];
                    ret[i] = ret[3 - i];
                    ret[3 - i] = d;
                }
            }
            return ret;
        }
        #endregion

        #region Graphics extension
        public static void drawBezier(Graphics2D g, float x1, float y1,
                           float ctrlx1, float ctrly1,
                           float ctrlx2, float ctrly2,
                           float x2, float y2)
        {
            Stroke stroke = g.getStroke();
            System.Drawing.Pen pen = null;
            if (stroke is BasicStroke) {
                pen = ((BasicStroke)stroke).pen;
            } else {
                pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            }
            g.nativeGraphics.DrawBezier(pen, new System.Drawing.PointF(x1, y1),
                                              new System.Drawing.PointF(ctrlx1, ctrly1),
                                              new System.Drawing.PointF(ctrlx2, ctrly2),
                                              new System.Drawing.PointF(x2, y2));
        }

        public const int STRING_ALIGN_FAR = 1;
        public const int STRING_ALIGN_NEAR = -1;
        public const int STRING_ALIGN_CENTER = 0;
        private static System.Drawing.StringFormat mStringFormat = new System.Drawing.StringFormat();
        public static void drawStringEx(Graphics g1, string s, Font font, Rectangle rect, int align, int valign)
        {
            if (align > 0) {
                mStringFormat.Alignment = System.Drawing.StringAlignment.Far;
            } else if (align < 0) {
                mStringFormat.Alignment = System.Drawing.StringAlignment.Near;
            } else {
                mStringFormat.Alignment = System.Drawing.StringAlignment.Center;
            }
            if (valign > 0) {
                mStringFormat.LineAlignment = System.Drawing.StringAlignment.Far;
            } else if (valign < 0) {
                mStringFormat.LineAlignment = System.Drawing.StringAlignment.Near;
            } else {
                mStringFormat.LineAlignment = System.Drawing.StringAlignment.Center;
            }
            g1.nativeGraphics.DrawString(s, font.font, g1.brush, new System.Drawing.RectangleF(rect.x, rect.y, rect.width, rect.height), mStringFormat);
        }
        #endregion

        #region System.IO
        public static double getFileLastModified(string path)
        {
            if (File.Exists(path)) {
                return new FileInfo(path).LastWriteTimeUtc.Ticks * 100.0 / 1e9;
            }
            return 0;
        }

        public static long getFileLength(string fpath)
        {
            return new FileInfo(fpath).Length;
        }

        public static string getExtension(string fpath)
        {
            return Path.GetExtension(fpath);
        }

        public static string getFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static string getDirectoryName(string path)
        {
            if (path == null) {
                return "";
            }
            if (path.Length == 0) {
                return "";
            }
            return System.IO.Path.GetDirectoryName(path);
        }

        public static string getFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public static string createTempFile()
        {
            return System.IO.Path.GetTempFileName();
        }

        public static string[] listDirectories(string directory)
        {
            return System.IO.Directory.GetDirectories(directory);
        }

        public static string[] listFiles(string directory, string extension)
        {
            return System.IO.Directory.GetFiles(directory, "*" + extension);
        }

        public static void deleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public static void moveFile(string pathBefore, string pathAfter)
        {
            System.IO.File.Move(pathBefore, pathAfter);
        }

        [Obsolete]
        public static bool isDirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        [Obsolete]
        public static bool isFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static string getTempPath()
        {
            return Path.GetTempPath();
        }

        public static void createDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void deleteDirectory(string path, bool recurse)
        {
            Directory.Delete(path, recurse);
        }

        public static void deleteDirectory(string path)
        {
            Directory.Delete(path);
        }

        public static void copyFile(string file1, string file2)
        {
            File.Copy(file1, file2);
        }
        #endregion

        #region Number Formatting
        public static bool tryParseInt(string s, ByRef<int> value)
        {
            try {
                value.value = int.Parse(s);
            } catch (Exception ex) {
                return false;
            }
            return true;
        }

        public static bool tryParseFloat(string s, ByRef<float> value)
        {
            try {
                value.value = (float)double.Parse(s);
            } catch (Exception ex) {
                return false;
            }
            return true;
        }

        public static string formatDecimal(string format, double value)
        {
            return value.ToString(format);
        }

        public static string formatDecimal(string format, long value)
        {
            return value.ToString(format);
        }

        public static string toHexString(long value, int digits)
        {
            string ret = toHexString(value);
            int add = digits - getStringLength(ret);
            for (int i = 0; i < add; i++) {
                ret = "0" + ret;
            }
            return ret;
        }

        public static string toHexString(long value)
        {
            return Convert.ToString(value, 16);
        }

        public static long fromHexString(string s)
        {
            return Convert.ToInt64(s, 16);
        }
        #endregion

        #region String Utility
        /// <summary>
        /// 文字列の指定した位置の文字を取得します
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="index">位置．先頭が0</param>
        /// <returns></returns>
        public static char charAt(string s, int index)
        {
            return s[index];
        }

        public static string[] splitString(string s, params char[] separator)
        {
            return splitStringCorB(s, separator, int.MaxValue, false);
        }

        public static string[] splitString(string s, char[] separator, int count)
        {
            return splitStringCorB(s, separator, count, false);
        }

        public static string[] splitString(string s, char[] separator, bool ignore_empty_entries)
        {
            return splitStringCorB(s, separator, int.MaxValue, ignore_empty_entries);
        }

        public static string[] splitString(string s, string[] separator, bool ignore_empty_entries)
        {
            return splitStringCorA(s, separator, int.MaxValue, ignore_empty_entries);
        }

        public static string[] splitString(string s, char[] separator, int count, bool ignore_empty_entries)
        {
            return splitStringCorB(s, separator, count, ignore_empty_entries);
        }

        public static string[] splitString(string s, string[] separator, int count, bool ignore_empty_entries)
        {
            return splitStringCorA(s, separator, count, ignore_empty_entries);
        }

        private static string[] splitStringCorB(string s, char[] separator, int count, bool ignore_empty_entries)
        {
            int length = separator.Length;
            string[] spl = new string[length];
            for (int i = 0; i < length; i++) {
                spl[i] = separator[i] + "";
            }
            return splitStringCorA(s, spl, count, false);
        }

        private static string[] splitStringCorA(string s, string[] separator, int count, bool ignore_empty_entries)
        {
            return s.Split(separator, count, (ignore_empty_entries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None));
        }

        public static int getStringLength(string s)
        {
            if (s == null) {
                return 0;
            } else {
                return s.Length;
            }
        }

        public static int getEncodedByteCount(string encoding, string str)
        {
            byte[] buf = getEncodedByte(encoding, str);
            return buf.Length;
        }

        public static byte[] getEncodedByte(string encoding, string str)
        {
            Encoding enc = Encoding.GetEncoding(encoding);
            return enc.GetBytes(str);
        }

        public static string getDecodedString(string encoding, int[] data, int offset, int length)
        {
            Encoding enc = Encoding.GetEncoding(encoding);
            byte[] d = new byte[data.Length];
            for (int i = 0; i < data.Length; i++) {
                d[i] = (byte)data[i];
            }
            return enc.GetString(d, offset, length);
        }

        public static string getDecodedString(string encoding, int[] data)
        {
            return getDecodedString(encoding, data, 0, data.Length);
        }

        #endregion

        public static void setMousePosition(Point p)
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(p.x, p.y);
        }

        public static Point getMousePosition()
        {
            System.Drawing.Point p = System.Windows.Forms.Control.MousePosition;
            return new Point(p.X, p.Y);
        }

        /// <summary>
        /// 指定した点が，コンピュータの画面のいずれかに含まれているかどうかを調べます
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool isPointInScreens(Point p)
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens) {
                System.Drawing.Rectangle rc = screen.WorkingArea;
                if (rc.X <= p.x && p.x <= rc.X + rc.Width) {
                    if (rc.Y <= p.y && p.y <= rc.Y + rc.Height) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Rectangle getWorkingArea(System.Windows.Forms.Form w)
        {
            System.Drawing.Rectangle r = System.Windows.Forms.Screen.GetWorkingArea(w);
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public static string getMD5FromString(string str)
        {
            return Misc.getmd5(str);
        }

        public static string getMD5(string file)
        {
            string ret = "";
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                ret = Misc.getmd5(fs);
            }
            return ret;
        }

        #region Array conversion
        public static int[] convertIntArray(int[] arr)
        {
            return arr;
        }

        public static long[] convertLongArray(long[] arr)
        {
            return arr;
        }

        public static byte[] convertByteArray(byte[] arr)
        {
            return arr;
        }

        public static float[] convertFloatArray(float[] arr)
        {
            return arr;
        }

        public static char[] convertCharArray(char[] arr)
        {
            return arr;
        }

        #endregion

        public static string getApplicationStartupPath()
        {
            return System.Windows.Forms.Application.StartupPath;
        }
    }

}
