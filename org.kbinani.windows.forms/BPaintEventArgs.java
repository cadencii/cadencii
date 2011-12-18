package com.github.cadencii.windows.forms;

import java.awt.Graphics;
import com.github.cadencii.BEventArgs;

public class BPaintEventArgs extends BEventArgs{
    public Graphics Graphics;

    public BPaintEventArgs(Graphics g1) {
        Graphics = g1;
    }

}
