package org.kbinani.componentmodel;

import java.util.Vector;

/**
 * オブジェクトの実際の型と，画面表示に使用する型との相互変換機能を提供します
 * @author kbinani
 *
 * @param <Act> オブジェクトの実際の型
 * @param <Disp> オブジェクトを画面表示する際に使用する型
 */
public class TypeConverter<Act, Disp> {
    public Disp convertTo( Act obj ){
        return null;
    }
    
    public Act convertFrom( Disp obj ){
        return null;
    }

    public boolean isStandardValuesSupported(){
        return false;
    }
    
    public Vector<Act> getStandardValues(){
        return null;
    }
}
