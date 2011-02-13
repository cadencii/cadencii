package org.kbinani.windows.forms;

import javax.swing.JSpinner;
import javax.swing.SpinnerNumberModel;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import org.kbinani.BEvent;
import org.kbinani.BEventArgs;
import org.kbinani.BEventHandler;

public class BNumericUpDown extends JSpinner
                            implements ChangeListener
{
    private static final long serialVersionUID = -1678016159355102909L;

    private JSpinner.NumberEditor mEditor = null;
    private SpinnerNumberModel mModel = null;
    private int mDecimalPlaces = 0;
    
    public BNumericUpDown(){
        super( new SpinnerNumberModel( 0.0, 0.0, 100.0, 1.0 ) );
        mModel =  new SpinnerNumberModel( 0.0, 0.0, 100.0, 1.0 );
        this.setModel( mModel );
        mEditor = new JSpinner.NumberEditor( this, "0" );
        setEditor( mEditor );
        addChangeListener( this );
    }

    public final BEvent<BEventHandler> valueChangedEvent = new BEvent<BEventHandler>();
    public void stateChanged( ChangeEvent e ){
        try{
            valueChangedEvent.raise( this, new BEventArgs() );
        }catch( Exception ex ){
            System.err.println( "BNumericUpDown#stateChanged; ex=" + ex );
        }
    }

    public Object getValue(){
        return getFloatValue();
    }
    
    public void setValue( Object value ){
        if( value == null ){
            return;
        }
        if( value instanceof Number ){
            Number n = (Number)value;
            setFloatValue( n.floatValue() );
        }
    }
    
    public float getFloatValue(){
        Double d = (Double)super.getValue();
        return d.floatValue();
    }
    
    public void setFloatValue( float value ){
        Double d = Double.valueOf( value );
        super.setValue( d );
    }
    
    public void setMaximum( float value ){
        Double d = Double.valueOf( (double)value );
        mModel.setMaximum( d );
    }
    
    public float getMaximum(){
        Double d = (Double)mModel.getMaximum();
        return d.floatValue();
    }
    
    public void setMinimum( float value ){
        Double d = Double.valueOf( value );
        mModel.setMinimum( d );
    }
    
    public float getMinimum(){
        Double d = (Double)mModel.getMinimum();
        return (float)d.floatValue();
    }
    
    public void setDecimalPlaces( int value ){
        if( value < 0 ){
            mDecimalPlaces = 0;
        }else{
            mDecimalPlaces = value;
        }
        String format = "0";
        if( mDecimalPlaces > 0 ){
            format += ".";
            for( int i = 0; i < mDecimalPlaces; i++ ){
                format += "0";
            }
        }
        mEditor = new JSpinner.NumberEditor( this, format );
        double stepsize = Math.pow( 10, -mDecimalPlaces );
        mModel.setStepSize( stepsize );
        setEditor( mEditor );
    }
    
    public int getDecimalPlaces(){
        return mDecimalPlaces;
    }
}
