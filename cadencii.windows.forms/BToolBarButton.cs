/*
 * BToolBarButton.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.windows.forms.
 *
 * org.kbinani.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
//INCLUDE ./BToolBarButton.java
#else
namespace cadencii.windows.forms
{

    public class BToolBarButton : System.Windows.Forms.ToolBarButton
    {
        public bool isCheckOnClick()
        {
            return (base.Style == System.Windows.Forms.ToolBarButtonStyle.ToggleButton);
        }

        public void setCheckOnClick( bool value )
        {
            base.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
        }

        public bool isSelected()
        {
            return base.Pushed;
        }

        public void setSelected( bool value )
        {
            base.Pushed = value;
        }
    }

}
#endif
