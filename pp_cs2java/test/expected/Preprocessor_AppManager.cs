        public static void setClipboard( ClipboardEntry item )
        {
            String clip = "";
            try {
                clip = getSerializedText( item );
            } catch ( Exception ex ) {
                serr.println( "AppManager#setClipboard; ex=" + ex );
                Logger.write( AppManager.class + ".setClipboard; ex=" + ex + "\n" );
                return;
            }
            PortUtil.setClipboardText( clip );
        }
