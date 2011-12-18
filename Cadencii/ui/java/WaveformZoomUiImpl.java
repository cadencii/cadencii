package com.github.cadencii.ui;

import com.github.cadencii.windows.forms.BPanel;
import com.github.cadencii.*;

public class WaveformZoomUiImpl
    extends BPanel
    implements WaveformZoomUi
{
    public WaveformZoomUiImpl() {
    }
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
