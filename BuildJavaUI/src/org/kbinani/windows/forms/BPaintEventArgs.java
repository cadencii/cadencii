package org.kbinani.windows.forms;

import java.awt.Graphics;
import org.kbinani.BEventArgs;

public class BPaintEventArgs extends BEventArgs{
    public Graphics Graphics;

    public BPaintEventArgs(Graphics g1) {
        Graphics = g1;
    }

}
