#if JAVA
//INCLUDE ..\BuildJavaUI\src\org\kbinani\windows\forms\BTextArea.java
#else
namespace bocoree.windows.forms {

    public class BTextArea : System.Windows.Forms.TextBox {
        public BTextArea() {
            base.Multiline = true;
            base.AcceptsReturn = true;
            base.AcceptsTab = true;
            base.WordWrap = false;
            base.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        }
    }

}
#endif
