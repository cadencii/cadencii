all: PlaySound.dll PlaySound64.dll

PlaySound.dll: PlaySound2.h PlaySound2.cpp
	g++ -shared -s PlaySound2.cpp -o PlaySound.dll -lwinmm

PlaySound64.dll: PlaySound2.h PlaySound2.cpp
	gcc -shared -m64 -s PlaySound2.cpp -o PlaySound64.dll -lwinmm

install: PlaySound.dll PlaySound64.dll
	copy PlaySound.dll ..\Cadencii\bin\x86\Debug\PlaySound.dll
	copy PlaySound.dll ..\Cadencii\bin\x86\Release\PlaySound.dll

clean:
	del PlaySound.dll PlaySound64.dll
