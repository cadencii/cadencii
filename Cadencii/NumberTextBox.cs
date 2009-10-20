/*
 * NumberTextBox.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA
package org.kbinani.Cadencii;
#else
#define COMPONENT_ENABLE_LOCATION
using System;
using System.Windows.Forms;
using bocoree;
using bocoree.windows.forms;
using bocoree.awt;

namespace Boare.Cadencii
{
#endif

    public class NumberTextBox : BTextBox
    {
        public enum ValueType
        {
            Double,
            Float,
            Integer,
        }

        private ValueType m_value_type = ValueType.Double;
        private Color m_textcolor_normal = Color.black;
        private Color m_textcolor_invalid = Color.white;
        private Color m_backcolor_normal = Color.white;
        private Color m_backcolor_invalid = PortUtil.LightCoral;

#if !JAVA
        /// <summary>
        /// IDEでのデザイン用
        /// </summary>
        public ValueType Type
        {
            get
            {
                return getType();
            }
            set
            {
                setType( value );
            }
        }
#endif

        public ValueType getType()
        {
            return m_value_type;
        }

        public void setType( ValueType value )
        {
            m_value_type = value;
        }

        protected override void OnTextChanged( EventArgs e )
        {
            base.OnTextChanged( e );
            bool valid = false;
            if ( m_value_type == ValueType.Double )
            {
                double dou;
                try
                {
                    dou = PortUtil.parseDouble( base.Text );
                    valid = true;
                }
                catch ( Exception ex )
                {
                    valid = false;
                }
            }
            else if ( m_value_type == ValueType.Float )
            {
                float flo;
                try
                {
                    flo = PortUtil.parseFloat( base.Text );
                    valid = true;
                }
                catch ( Exception ex )
                {
                    valid = false;
                }
            }
            else if ( m_value_type == ValueType.Integer )
            {
                int inte;
                try
                {
                    inte = PortUtil.parseInt( base.Text );
                    valid = true;
                }
                catch ( Exception ex )
                {
                    valid = false;
                }
            }
            if ( valid )
            {
                this.setForeground( m_textcolor_normal );
                this.setBackground( m_backcolor_normal );
            }
            else
            {
                this.setForeground( m_textcolor_invalid );
                this.setBackground( m_backcolor_invalid );
            }
        }
    }

#if !JAVA
}
#endif
