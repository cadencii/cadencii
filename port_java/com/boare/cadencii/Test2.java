package com.boare.cadencii;

public class Test2{
    public float v = 1.0f;
    private boolean m_b = true;
    private int m_i = 2;

    public String toString(){
        return "{v=" + v + ",Hoge=" + isHoge() + ";Integer=" + getInteger() + "}";
    }

    public boolean isHoge(){
        return m_b;
    }

    public void setHoge( boolean value ){
        m_b = value;
    }

    public int getInteger(){
        return m_i;
    }

    public void setInteger( int value ){
        m_i = value;
    }
}
