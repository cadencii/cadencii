CP=cp
RM=rm
TARGET=.\build\win

all: first $(TARGET)\Cadencii.exe

first: .\first.pl
	perl .\first.pl

$(TARGET)\Cadencii.exe: resources .\Cadencii\*.cs $(TARGET)\org.kbinani.dll $(TARGET)\org.kbinani.apputil.dll $(TARGET)\org.kbinani.media.dll $(TARGET)\org.kbinani.vsq.dll
	gmcs -recurse:.\Cadencii\*.cs -define:MONO,USE_DOBJ,ENABLE_VOCALOID;ENABLE_AQUESTONE;ENABLE_MIDI -out:$(TARGET)\Cadencii.exe -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll,$(TARGET)\org.kbinani.apputil.dll,$(TARGET)\org.kbinani.media.dll,$(TARGET)\org.kbinani.vsq.dll -unsafe+ -codepage:utf8

resources: .\Cadencii\Resources.list makeRes
	makeRes -i ".\Cadencii\Resources.list" -o ".\Cadencii\Resources.cs" -p "org.kbinani.cadencii" -n "org.kbinani.cadencii"

makeRes: .\makeRes.cs
	gmcs makeRes.cs

$(TARGET)\org.kbinani.dll: ./org.kbinani/*.cs
	gmcs -recurse:.\org.kbinani\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.dll -r:System.Windows.Forms,System.Drawing -unsafe+ -codepage:utf8

$(TARGET)\org.kbinani.apputil.dll: $(TARGET)\org.kbinani.dll ./org.kbinani.apputil/*.cs
	gmcs -recurse:.\org.kbinani.apputil\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.apputil.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll -unsafe+ -codepage:utf8

$(TARGET)\org.kbinani.media.dll: $(TARGET)\org.kbinani.dll ./org.kbinani.media/*.cs
	gmcs -recurse:.\org.kbinani.media\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.media.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll -unsafe+ -codepage:utf8

$(TARGET)\org.kbinani.vsq.dll: $(TARGET)\org.kbinani.dll ./org.kbinani.vsq/*.cs
	gmcs -recurse:.\org.kbinani.vsq\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.vsq.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll  -codepage:utf8

clean:
	$(RM) $(TARGET)\org.kbinani.dll
	$(RM) $(TARGET)\org.kbinani.apputil.dll
	$(RM) $(TARGET)\org.kbinani.media.dll
	$(RM) $(TARGET)\org.kbinani.vsq.dll
	$(RM) $(TARGET)\Cadencii.exe
