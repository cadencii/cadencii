namespace org.kbinani.cadencii {

    public abstract class WaveUnit {
        protected EditorConfig _config;

        public abstract void setConfig( string parameters );
        public abstract int getVersion();

        public void setGlobalConfig( EditorConfig config ) {
            _config = config;
        }
    }

}
