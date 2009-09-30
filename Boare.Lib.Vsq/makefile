CP=cp
RM=rm

all: Boare.Lib.Vsq.dll

Boare.Lib.Vsq.dll: *.cs bocoree.dll
	gmcs -recurse:*.cs -target:library -define:MONO -out:Boare.Lib.Vsq.dll -r:bocoree.dll,System,System.Windows.Forms,System.Drawing

#libvsq.exe: common.cpp TextMemoryStream.cpp VsqCommon.cpp main.cpp
#	g++ common.cpp TextMemoryStream.cpp VsqCommon.cpp main.cpp -o libvsq.exe

clean:
	$(RM) Boare.Lib.Vsq.dll
	$(RM) bocoree.dll
