using System;
using System.Drawing;
using System.Windows.Forms;

namespace Boare.Cadencii {

    public class NumberTextBox : TextBox {
        public enum ValueType {
            Double,
            Float,
            Integer,
            Decimal,
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
                case ValueType.Decimal:
                    decimal dec;
                    valid = decimal.TryParse( base.Text, out dec );
                    break;
                case ValueType.Double:
                    double dou;
                    valid = double.TryParse( base.Text, out dou );
                    break;
                case ValueType.Float:
                    float flo;
                    valid = float.TryParse( base.Text, out flo );
                    break;
                case ValueType.Integer:
                    int inte;
                    valid = int.TryParse( base.Text, out inte );
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
