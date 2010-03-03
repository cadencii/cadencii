PlaySound.dll: PlaySound2.h PlaySound2.cpp
	g++ -shared -s PlaySound2.cpp -o PlaySound.dll -lwinmm

install: PlaySound.dll
	copy PlaySound.dll ..\Cadencii\bin\x86\Debug\PlaySound.dll
	copy PlaySound.dll ..\Cadencii\bin\x86\Release\PlaySound.dll
