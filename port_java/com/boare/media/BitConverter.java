package com.boare.media;

public class BitConverter{
    public static byte[] getBytes( int value ){
        byte[] ret = new byte[4];
        ret[0] = (byte)(0xff & value);
        ret[1] = (byte)(0xff & (value >>> 8));
        ret[2] = (byte)(0xff & (value >>> 16));
        ret[3] = (byte)(0xff & (value >>> 24));
        return ret;
    }

    public static byte[] getBytes( short value ){
        byte[] ret = new byte[2];
        ret[0] = (byte)(0xff & value);
        ret[1] = (byte)(0xff & (value >>> 8));
        return ret;
    }

    public static int toInt32( byte[] value, int offset ){
        int ret = 0;
        ret = ret | (0xff & value[offset]);
        ret = ret | ((0xff & value[offset + 1]) << 8);
        ret = ret | ((0xff & value[offset + 2]) << 16);
        ret = ret | ((0xff & value[offset + 3]) << 24);
        return ret;
    }
}
