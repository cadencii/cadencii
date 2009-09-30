first:
	perl first.pl

win: first win/PlaySound.dll

linux: first linux/libPlaySound.so

macosx: first macosx/PlaySound.dll

win/PlaySound.dll: PlaySound.cpp PlaySound.h
	g++ -shared -s -o win/PlaySound.dll PlaySound.cpp -lwinmm

linux/libPlaySound.so: PlaySound_linux.c
	g++ -c -fPIC -g -Wall PlaySound_linux.c
	g++ -shared -Wl,-soname,libPlaySound.so -o linux/libPlaySound.so PlaySound_linux.o -lm

macosx/PlaySound.dll:
#TODO

clean: clean_win clean_linux

clean_win:
	del win\PlaySound.dll

clean_linux:
	rm linux/libPlaySound.so

clean_macosx:
	rm macosx/PlaySound.dll

install:
	copy .\win\PlaySound.dll ..\Cadencii\bin\x86\Debug\PlaySound.dll
	copy .\win\PlaySound.dll ..\Cadencii\bin\x86\Release\PlaySound.dll