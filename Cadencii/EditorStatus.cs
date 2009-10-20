namespace Boare.Cadencii {
    using boolean = System.Boolean;

    public class EditorStatus {
        /// <summary>
        /// トラックのレンダリングが必要かどうかを表すフラグ
        /// </summary>
        public boolean[] renderRequired = new boolean[16];

        public EditorStatus() {
            for ( int i = 0; i < renderRequired.Length; i++ ) {
                renderRequired[i] = false;
            }
        }

        public EditorStatus clone() {
            EditorStatus ret = new EditorStatus();
            for ( int i = 0; i < renderRequired.Length; i++ ) {
                ret.renderRequired[i] = renderRequired[i];
            }
            return ret;
        }
    }

}
