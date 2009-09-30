all: PlaySound.dll

#test.exe: test.c
#	gcc test.c -o test.exe -lmingw32 -lSDLmain -lSDL

#test2.exe: test2.c
#	gcc test2.c -o test2.exe -lmingw32 -lSDLmain -lSDL

PlaySound.dll: PlaySound_SDL.c
	gcc -D_TEST PlaySound_SDL.c -shared -o PlaySound.dll -lmingw32 -lSDLmain -lSDL

libPlaySound.dylib: PlaySound_SDL.c
	gcc -I/Library/Frameworks/SDL.framework/Headers PlaySound_SDL.c SDLmain.m -shared -o libPlaySound.dylib -framework SDL -framework Cocoa
libPlaySound.so: PlaySound_SDL.c
	g++ -c -fPIC -g PlaySound_SDL.c
	g++ -shared -Wl,-soname,libPlaySound.so -o libPlaySound.so PlaySound_SDL.o -lSDL

