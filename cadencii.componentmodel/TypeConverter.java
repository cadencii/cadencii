package com.github.cadencii.componentmodel;

import java.util.Vector;

/**
 * オブジェクトの実際の型と，画面表示に使用する型との相互変換機能を提供します
 * @author kbinani
 *
 * @param <T> オブジェクトの実際の型
 */
public class TypeConverter<T> {
    public String convertTo( Object obj ){
        return "" + obj;
    }
    
    public T convertFrom( String obj ){
        return null;
    }

    public boolean isStandardValuesSupported()
    {
        return false;
    }

    public Vector<T> getStandardValues()
    {
        return null;
    }
}
