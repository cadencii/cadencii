#!/bin/sh
#---------------------------------------------------------------------------
#
# Usage:
#     vocaloidrv.sh [WINEPREFIX] [wine_top] [exe] [dll] [midi_master] [midi_body] [total samples] {output wave file}
# Example:
#     vocaloidrv.sh ~/Library/Application\ Support/MikuInstaller/prefix/default /Applications/MikuInstaller.app/Contents/Resources/Wine.bundle/Contents/SharedSupport vocaloidrv.exe "C:\\Program Files\\Steinberg\\VSTplugins\\VOCALOID2\\VOCALOID2.dll" "midi_master.bin" "midi_body.bin" 441000
#
#---------------------------------------------------------------------------

WINEPREFIX="$1"
export WINEPREFIX

wine_top="$2"

lib_wine="$wine_top/lib/wine"
bin_wine="$wine_top/bin"
if [ -n "$DYLD_LIBRARY_PATH" ]
then
  DYLD_LIBRARY_PATH="$lib_wine:$DYLD_LIBRARY_PATH"
else
  DYLD_LIBRARY_PATH="$lib_wine"
fi

# #!/usr/bin/perlで始まっていたらwineではなくwineloaderにする(CrossOver対策)
#header15=`od -tx1 -N 15 $bin_wine/wine | cut -c 9-70 | sed -e "s/ //g"`
#if [ "$header15" = "23212f7573722f62696e2f7065726c" ]
#then
#  WINELOADER="$bin_wine/wineloader"
#else
  WINELOADER="$bin_wine/wine"
#fi
export WINELOADER

#echo "------------------------" >> args.txt
#echo "WINEPREFIX=$1" >> args.txt
#echo "WINETOP=$2" >> args.txt
#echo "WINELOADER=$WINELOADER" >> args.txt
#echo "3=$3" >> args.txt
#echo "4=$4" >> args.txt
#echo "5=$5" >> args.txt
#echo "6=$6" >> args.txt
#echo "7=$7" >> args.txt
#echo "8=$8" >> args.txt

#if [ "$5" = "-e" ]
#then
#echo "with cat" >> args.txt
#  cat - | exec "$WINELOADER" "$3" "$4" "$5" "$6" "$7" "$8"
#else
#echo "without cat" >> args.txt
  exec "$WINELOADER" "$3" "$4" "$5" "$6" "$7" "$8"
#fi
