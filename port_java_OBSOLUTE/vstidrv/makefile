all:
	javac HelloCallBack.java
	g++ -g -I"C:\Program Files\Java\jdk1.6.0_03\include" -I"C:\Program Files\Java\jdk1.6.0_03\include\win32" -c HelloCallBack.cpp
	gcc -mdll -o junk.tmp -Wl,--base-file,base.tmp
	dlltool -l libHelloCallBack.a --dllname HelloCallBack.dll --base-file base.tmp --output-exp temp.exp --def HelloCallBack.def
	gcc -mdll -o HelloCallBack.dll HelloCallBack.o -Wl,temp.exp
