using System;
using System.Drawing;
using System.Windows.Forms;

using bocoree;

namespace Boare.Cadencii {

    public class NumberTextBox : TextBox {
        public enum ValueType {
            Double,
            Float,
            Integer,
        }

        private ValueType m_value_type = ValueType.Double;
        private Color m_textcolor_normal = SystemColors.WindowText;
        private Color m_textcolor_invalid = Color.White;
        private Color m_backcolor_normal = SystemColors.Window;
        private Color m_backcolor_invalid = Color.LightCoral;

        public ValueType Type {
            get {
                return m_value_type;
            }
            set {
                m_value_type = value;
            }
        }

        protected override void OnTextChanged( EventArgs e ) {
            base.OnTextChanged( e );
            bool valid = false;
            switch ( m_value_type ) {
                case ValueType.Double:
                    double dou;
                    try {
                        dou = PortUtil.parseDouble( base.Text );
                        valid = true;
                    } catch ( Exception ex ) {
                        valid = false;
                    }
                    break;
                case ValueType.Float:
                    float flo;
                    try {
                        flo = PortUtil.parseFloat( base.Text );
                        valid = true;
                    } catch ( Exception ex ) {
                        valid = false;
                    }
                    break;
                case ValueType.Integer:
                    int inte;
                    try {
                        inte = PortUtil.parseInt( base.Text );
                        valid = true;
                    } catch ( Exception ex ) {
                        valid = false;
                    }
                    break;
            }
            if ( valid ) {
                this.ForeColor = m_textcolor_normal;
                this.BackColor = m_backcolor_normal;
            } else {
                this.ForeColor = m_textcolor_invalid;
                this.BackColor = m_backcolor_invalid;
            }
        }
    }

}
