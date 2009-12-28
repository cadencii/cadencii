#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BGroupBox.java
#else
namespace org.kbinani.windows.forms{
    public class BGroupBox : System.Windows.Forms.GroupBox {
        public string getTitle() {
            return base.Text;
        }

        public void setTitle( string value ) {
            base.Text = value;
        }
    }
}
#endif
