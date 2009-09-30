SHARED_OPT=-shared
CC=g++
INCL=

all: vstidrv.dll install

vstidrv.dll: stdafx.cpp waveplay.cpp waveplay.h vstidrv3.cpp vstidrv3.h
	$(CC) $(SHARED_OPT) stdafx.cpp winmmhelp.cpp wavereader.cpp waveplay.cpp vstidrv3.cpp -o vstidrv.dll -lwinmm 

install: vstidrv.dll
	cp vstidrv.dll ../Cadencii/bin/x86/Debug/vstidrv.dll
	cp vstidrv.dll ../Cadencii/bin/x86/Release/vstidrv.dll
	cp vstidrv.dll ../Cadencii/vstidrv.dll

clean:
	rm vstidrv.dll
