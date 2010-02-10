RM=del

all: vstidrv4.exe GenerateFileMapping.exe

vstidrv4.exe: vstidrv4.cpp
	g++ -D_DEBUG vstidrv4.cpp -o vstidrv4.exe

GenerateFileMapping.exe: GenerateFileMapping.cpp
	g++ -D_DEBUG GenerateFileMapping.cpp -o GenerateFileMapping.exe

clean:
	$(RM) vstidrv4.exe GenerateFileMapping.exe
