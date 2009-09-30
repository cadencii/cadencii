all: main.exe vocaloiddevice2.dll

#ReWire.dll: ReWire.c ReWire.h
#	gcc -shared ReWire.c -o ReWire.dll
#main.exe: main.c ReWire.dll
#	gcc main.c -o main.exe

main.exe: main.c
#	gcc -shared main.c -o main.dll
	gcc main.c -o main.exe

vocaloiddevice2.dll: main.c
	gcc -shared main.c -o vocaloiddevice2.dll
