#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BStatusLabel.java
#else
namespace bocoree.windows.forms {

    public class BStatusLabel : System.Windows.Forms.ToolStripStatusLabel {
        public void setText( string value ) {
            base.Text = value;
        }

        public string getText() {
            return base.Text;
        }
    }

}
#endif
