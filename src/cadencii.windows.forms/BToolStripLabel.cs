#if !JAVA
/*
 * BToolStripLabel.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
namespace cadencii.windows.forms {

    /// <summary>
    /// 
    /// </summary>
    public class BToolStripLabel : System.Windows.Forms.ToolStripLabel {
        public void setText( string value ) {
            base.Text = value;
        }
    }

}
#endif
