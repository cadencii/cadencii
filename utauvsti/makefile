CC=g++

utauvsti.dll: symboltable.cpp vstplugmain.cpp audioeffect.cpp audioeffectx.cpp utauvsti.cpp utauvsti.h
	$(CC) -s -shared symboltable.cpp vstplugmain.cpp audioeffect.cpp audioeffectx.cpp utauvsti.cpp -o utauvsti.dll -lkernel32 -lshlwapi

clean:
	rm utauvsti.dll
