        public static void setClipboard( ClipboardEntry item )
        {
#if CLIPBOARD_AS_TEXT
            String clip = "";
            try {
                clip = getSerializedText( item );
#if DEBUG
                sout.println( "AppManager#setClipboard; clip=" + clip );
#endif
            } catch ( Exception ex ) {
                serr.println( "AppManager#setClipboard; ex=" + ex );
                Logger.write( typeof( AppManager ) + ".setClipboard; ex=" + ex + "\n" );
                return;
            }
            PortUtil.setClipboardText( clip );
#else
            Clipboard.SetDataObject( item, false );
#endif
        }
