#!/bin/sh
#---------------------------------------------------------------------------
#
# Usage:
#     vocaloidrv.sh [WINEPREFIX] [wine_top] {...arguments}
# Example:
#     vocaloidrv.sh ~/Library/Application\ Support/MikuInstaller/prefix/default /Applications/MikuInstaller.app/Contents/Resources/Wine.bundle/Contents/SharedSupport vocaloidrv.exe "C:\\Program Files\\Steinberg\\VSTplugins\\VOCALOID2\\VOCALOID2.dll" "midi_master.bin" "midi_body.bin" 441000
#
#---------------------------------------------------------------------------

WINEPREFIX="$1"
export WINEPREFIX
shift

wine_top="$1"
shift

lib_wine="$wine_top/lib/wine"
bin_wine="$wine_top/bin"
if [ -n "$DYLD_LIBRARY_PATH" ]
then
  DYLD_LIBRARY_PATH="$lib_wine:$DYLD_LIBRARY_PATH"
else
  DYLD_LIBRARY_PATH="$lib_wine"
fi

WINELOADER="$bin_wine/wine"
export WINELOADER

exec "$WINELOADER" "$@"
