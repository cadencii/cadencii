CP=cp
RM=rm
TARGET=.\build\win

all: first $(TARGET)\Cadencii.exe

first: .\first.pl
	perl .\first.pl

jcadencii: pp_cs2java.exe jcorlib japputil jmedia jvsq
	pp_cs2java.exe -DJAVA -DUSE_DOBJ -DRELEASE -b ".\build\java" -encoding "UTF-8" -s -4 -c -t ".\Cadencii"
	javac .\build\java\org\kbinani\cadencii\*.java .\build\java\org\kbinani\*.java .\build\java\org\kbinani\apputil\*.java .\build\java\org\kbinani\media\*.java .\build\java\org\kbinani\vsq\*.java .\build\java\org\kbinani\cadencii\*.java -encoding UTF8

jcorlib: pp_cs2java.exe ./org.kbinani/*.cs
	pp_cs2java.exe -DJAVA -DUSE_DOBJ -DRELEASE -b ".\build\java" -encoding "UTF-8" -s -4 -c -t ".\org.kbinani"

japputil: pp_cs2java.exe ./org.kbinani.apputil/*.cs
	pp_cs2java.exe -DJAVA -DUSE_DOBJ -DRELEASE -b ".\build\java" -encoding "UTF-8" -s -4 -c -t ".\org.kbinani.apputil"

jmedia: pp_cs2java.exe ./org.kbinani.media/*.cs
	pp_cs2java.exe -DJAVA -DUSE_DOBJ -DRELEASE -b ".\build\java" -encoding "UTF-8" -s -4 -c -t ".\org.kbinani.media"

jvsq: pp_cs2java.exe ./org.kbinani.vsq/*.cs
	pp_cs2java.exe -DJAVA -DUSE_DOBJ -DRELEASE -b ".\build\java" -encoding "UTF-8" -s -4 -c -t ".\org.kbinani.vsq"

pp_cs2java.exe: $(TARGET)\org.kbinani.dll .\pp_cs2java\Program.cs
	gmcs .\pp_cs2java\Program.cs -out:.\pp_cs2java.exe -r:$(TARGET)\org.kbinani.dll,System.Drawing
	$(CP) $(TARGET)\org.kbinani.dll .\org.kbinani.dll

$(TARGET)\Cadencii.exe: resources .\Cadencii\*.cs $(TARGET)\org.kbinani.dll $(TARGET)\org.kbinani.apputil.dll $(TARGET)\org.kbinani.media.dll $(TARGET)\org.kbinani.vsq.dll $(TARGET)\PlaySound.dll
	gmcs -recurse:.\Cadencii\*.cs -define:MONO,USE_DOBJ,ENABLE_VOCALOID;ENABLE_AQUESTONE;ENABLE_MIDI -out:$(TARGET)\Cadencii.exe -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll,$(TARGET)\org.kbinani.apputil.dll,$(TARGET)\org.kbinani.media.dll,$(TARGET)\org.kbinani.vsq.dll -unsafe+ -codepage:utf8

resources: .\Cadencii\Resources.list makeRes.exe
	makeRes.exe -i ".\Cadencii\Resources.list" -o ".\Cadencii\Resources.cs" -p "org.kbinani.cadencii" -n "org.kbinani.cadencii"

makeRes.exe: .\makeRes.cs
	gmcs makeRes.cs

$(TARGET)\PlaySound.dll: ./PlaySound/PlaySound.cpp ./PlaySound/PlaySound.h
	g++ -shared -s -o $(TARGET)\PlaySound.dll ./PlaySound/PlaySound.cpp -lwinmm

$(TARGET)\org.kbinani.dll: ./org.kbinani/*.cs
	gmcs -recurse:.\org.kbinani\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.dll -r:System.Windows.Forms,System.Drawing -unsafe+ -codepage:utf8

$(TARGET)\org.kbinani.apputil.dll: $(TARGET)\org.kbinani.dll ./org.kbinani.apputil/*.cs
	gmcs -recurse:.\org.kbinani.apputil\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.apputil.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll -unsafe+ -codepage:utf8

$(TARGET)\org.kbinani.media.dll: $(TARGET)\org.kbinani.dll ./org.kbinani.media/*.cs
	gmcs -recurse:.\org.kbinani.media\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.media.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll -unsafe+ -codepage:utf8

$(TARGET)\org.kbinani.vsq.dll: $(TARGET)\org.kbinani.dll ./org.kbinani.vsq/*.cs
	gmcs -recurse:.\org.kbinani.vsq\*.cs -target:library -define:MONO -out:$(TARGET)\org.kbinani.vsq.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)\org.kbinani.dll  -codepage:utf8

doc: jcadencii
	javadoc -sourcepath ".\build\java" org.kbinani.vsq org.kbinani org.kbinani.apputil org.kbinani.media org.kbinani.cadencii -encoding UTF8 -use -public


clean:
	$(RM) $(TARGET)\org.kbinani.dll $(TARGET)\org.kbinani.apputil.dll $(TARGET)\org.kbinani.media.dll $(TARGET)\org.kbinani.vsq.dll $(TARGET)\Cadencii.exe $(TARGET)\PlaySound.dll pp_cs2java.dll org.kbinani.dll
