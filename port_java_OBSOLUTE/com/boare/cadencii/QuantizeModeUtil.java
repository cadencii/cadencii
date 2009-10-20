package com.boare.cadencii;

public class QuantizeModeUtil {
    public static String getString( QuantizeMode quantize_mode ) {
        switch ( quantize_mode ) {
            case off:
                return "Off";
            case p4:
                return "1/4";
            case p8:
                return "1/8";
            case p16:
                return "1/16";
            case p32:
                return "1/32";
            case p64:
                return "1/64";
            case p128:
                return "1/128";
            default:
                return "";
        }
    }

    /// <summary>
    /// クオンタイズ時の音符の最小単位を、クロック数に換算したものを取得します
    /// </summary>
    /// <param name="qm"></param>
    /// <param name="triplet"></param>
    /// <returns></returns>
    public static int getQuantizeClock( QuantizeMode qm, boolean triplet ) {
        int ret = 1;
        switch ( qm ) {
            case p4:
                ret = 480;
                break;
            case p8:
                ret = 240;
                break;
            case p16:
                ret = 120;
                break;
            case p32:
                ret = 60;
                break;
            case p64:
                ret = 30;
                break;
            case p128:
                ret = 15;
                break;
            default:
                return 1;
        }
        if ( triplet ) {
            ret = ret * 2 / 3;
        }
        return ret;
    }
}
