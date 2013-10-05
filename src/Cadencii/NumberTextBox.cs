/*
 * NumberTextBox.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#define COMPONENT_ENABLE_LOCATION
using System;
using cadencii;
using cadencii.java.awt;
using cadencii.windows.forms;



namespace cadencii
{

    public class NumberTextBox : System.Windows.Forms.TextBox
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
        private Color m_backcolor_invalid = new Color(240, 128, 128);

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
                setType(value);
            }
        }

        public ValueType getType()
        {
            return m_value_type;
        }

        public void setType(ValueType value)
        {
            m_value_type = value;
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            validateText();
        }

        private void validateText()
        {
            bool valid = false;
            string text = this.Text;
            if (m_value_type == ValueType.Double) {
                double dou;
                try {
                    dou = double.Parse(text);
                    valid = true;
                } catch (Exception ex) {
                    valid = false;
                }
            } else if (m_value_type == ValueType.Float) {
                float flo;
                try {
                    flo = (float)double.Parse(text);
                    valid = true;
                } catch (Exception ex) {
                    valid = false;
                }
            } else if (m_value_type == ValueType.Integer) {
                int inte;
                try {
                    inte = int.Parse(text);
                    valid = true;
                } catch (Exception ex) {
                    valid = false;
                }
            }
            if (valid) {
                ForeColor = m_textcolor_normal.color;
                BackColor = m_backcolor_normal.color;
            } else {
                ForeColor = m_textcolor_invalid.color;
                BackColor = m_backcolor_invalid.color;
            }
        }

    }

}
