package org.kbinani.cadencii;

import org.kbinani.windows.forms.BPanel;

public class WaveformZoomUiImpl
    extends BPanel
    implements WaveformZoomUi
{
    private WaveformZoomUiListener mListener;
    
    @Override
    public int getWidth()
    {
        return super.getWidth();
    }

    @Override
    public int getHeight()
    {
        return super.getHeight();
    }

    @Override
    public void setListener( WaveformZoomUiListener listener )
    {
        mListener = listener;
    }

    @Override
    public void repaint()
    {
        super.repaint();
    }

}
