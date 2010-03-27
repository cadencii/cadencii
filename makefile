CP=cp
RM=rm
TARGET=./build/win
MCS_OPT=-warn:0
JROOT=./build/java/org/kbinani

all: first $(TARGET)/Cadencii.exe

first: ./first.pl
	perl ./first.pl

jcadencii_win: jcadencii
	exewrap -g -t 1.6 -o .\build\java\jCadencii.exe -i .\Cadencii\resources\icon.ico .\build\java\Cadencii.jar

jcadencii: pp_cs2java.exe jcorlib japputil jmedia jvsq ./Cadencii/Resources.cs
	mono ./pp_cs2java.exe -DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -b ./build/java -encoding "UTF-8" -s -4 -c -t ./Cadencii
	$(CP) ./build/java/org/kbinani/cadencii/NumberTextBox.java ./BuildJavaUI/src/org/kbinani/cadencii/NumberTextBox.java
	$(CP) ./build/java/org/kbinani/cadencii/NumericUpDownEx.java ./BuildJavaUI/src/org/kbinani/cadencii/NumericUpDownEx.java
	javac ./build/java/Cadencii.java ./build/java/org/kbinani/cadencii/*.java ./build/java/org/kbinani/*.java ./build/java/org/kbinani/apputil/*.java ./build/java/org/kbinani/media/*.java ./build/java/org/kbinani/vsq/*.java ./build/java/org/kbinani/cadencii/*.java ./build/java/org/kbinani/componentmodel/*.java ./build/java/org/kbinani/windows/forms/*.java ./build/java/org/kbinani/xml/*.java -encoding UTF8 -target 1.5 -source 1.5
	$(CP) ./Cadencii/Cadencii.mf ./build/java/Cadencii.mf
	cd ./build/java && jar cfm Cadencii.jar Cadencii.mf Cadencii.class org/kbinani/cadencii/*.class org/kbinani/*.class org/kbinani/apputil/*.class org/kbinani/media/*.class org/kbinani/vsq/*.class org/kbinani/cadencii/*.class org/kbinani/componentmodel/*.class org/kbinani/windows/forms/*.class org/kbinani/xml/*.class

jeditotoini: pp_cs2java.exe jcorlib japputil jmedia jvsq
	mono ./pp_cs2java.exe -DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -b ./build/java -encoding "UTF-8" -s -4 -c -t ./EditOtoIni
	javac $(JROOT)/editotoini/*.java $(JROOT)/*.java $(JROOT)/windows/forms/*.java $(JROOT)/componentmodel/*.java $(JROOT)/vsq/*.java $(JROOT)/xml/*.java $(JROOT)/apputil/*.java $(JROOT)/media/*.java -encoding UTF8

jcorlib: pp_cs2java.exe ./org.kbinani/*.cs
	mono ./pp_cs2java.exe -DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -b ./build/java/ -encoding "UTF-8" -s -4 -c -t ./org.kbinani/
	$(CP) ./build/java/org/kbinani/math.java ./BuildJavaUI/src/org/kbinani/math.java
	$(CP) ./build/java/org/kbinani/PortUtil.java ./BuildJavaUI/src/org/kbinani/PortUtil.java
	$(CP) ./build/java/org/kbinani/InternalStdErr.java ./BuildJavaUI/src/org/kbinani/InternalStdErr.java
	$(CP) ./build/java/org/kbinani/InternalStdOut.java ./BuildJavaUI/src/org/kbinani/InternalStdOut.java
	$(CP) ./build/java/org/kbinani/ByRef.java ./BuildJavaUI/src/org/kbinani/ByRef.java
	$(CP) ./build/java/org/kbinani/xml/XmlSerializable.java ./BuildJavaUI/src/org/kbinani/xml/XmlSerializable.java

japputil: pp_cs2java.exe ./org.kbinani.apputil/*.cs
	mono ./pp_cs2java.exe -DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -b ./build/java -encoding "UTF-8" -s -4 -c -t ./org.kbinani.apputil

jmedia: pp_cs2java.exe ./org.kbinani.media/*.cs
	mono ./pp_cs2java.exe -DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -b ./build/java -encoding "UTF-8" -s -4 -c -t ./org.kbinani.media

jvsq: pp_cs2java.exe ./org.kbinani.vsq/*.cs
	mono ./pp_cs2java.exe -DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -b ./build/java -encoding "UTF-8" -s -4 -c -t ./org.kbinani.vsq

pp_cs2java.exe: first $(TARGET)/org.kbinani.dll ./pp_cs2java/Program.cs
	gmcs ./pp_cs2java/Program.cs -out:./pp_cs2java.exe -r:$(TARGET)/org.kbinani.dll,System.Drawing $(MCS_OPT) -define:DEBUG
	$(CP) $(TARGET)/org.kbinani.dll ./org.kbinani.dll

$(TARGET)/Cadencii.exe: ./Cadencii/Resources.cs ./Cadencii/*.cs $(TARGET)/org.kbinani.dll $(TARGET)/org.kbinani.apputil.dll $(TARGET)/org.kbinani.media.dll $(TARGET)/org.kbinani.vsq.dll $(TARGET)/PlaySound.dll
	gmcs -recurse:./Cadencii/*.cs -define:MONO,ENABLE_VOCALOID,ENABLE_AQUESTONE,ENABLE_MIDI,ENABLE_PROPERTY -out:$(TARGET)/Cadencii.exe -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll,$(TARGET)/org.kbinani.apputil.dll,$(TARGET)/org.kbinani.media.dll,$(TARGET)/org.kbinani.vsq.dll -unsafe+ -codepage:utf8 $(MCS_OPT)

./Cadencii/REsources.cs: ./Cadencii/Resources.list makeRes.exe
	mono ./makeRes.exe -i ./Cadencii/Resources.list -o ./Cadencii/Resources.cs -p org.kbinani.cadencii -n org.kbinani.cadencii

makeRes.exe: ./makeRes.cs
	gmcs makeRes.cs $(MCS_OPT)

$(TARGET)/PlaySound.dll: ./PlaySound2/PlaySound2.cpp ./PlaySound2/PlaySound2.h
	g++ -shared -s -o $(TARGET)/PlaySound.dll ./PlaySound2/PlaySound2.cpp -lwinmm

$(TARGET)/org.kbinani.dll: ./org.kbinani/*.cs
	gmcs -recurse:./org.kbinani/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.dll -r:System.Windows.Forms,System.Drawing -unsafe+ -codepage:utf8 $(MCS_OPT)

$(TARGET)/org.kbinani.apputil.dll: $(TARGET)/org.kbinani.dll ./org.kbinani.apputil/*.cs
	gmcs -recurse:./org.kbinani.apputil/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.apputil.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll -unsafe+ -codepage:utf8 $(MCS_OPT)

$(TARGET)/org.kbinani.media.dll: $(TARGET)/org.kbinani.dll ./org.kbinani.media/*.cs
	gmcs -recurse:./org.kbinani.media/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.media.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll -unsafe+ -codepage:utf8 $(MCS_OPT)

$(TARGET)/org.kbinani.vsq.dll: $(TARGET)/org.kbinani.dll ./org.kbinani.vsq/*.cs
	gmcs -recurse:./org.kbinani.vsq/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.vsq.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll  -codepage:utf8 $(MCS_OPT)

doc: jcadencii
	javadoc -sourcepath ./build/java org.kbinani.vsq org.kbinani org.kbinani.apputil org.kbinani.media org.kbinani.cadencii -encoding UTF8 -use -public

clean:
	$(RM) $(TARGET)/org.kbinani.dll $(TARGET)/org.kbinani.apputil.dll $(TARGET)/org.kbinani.media.dll $(TARGET)/org.kbinani.vsq.dll $(TARGET)/Cadencii.exe $(TARGET)/PlaySound.dll pp_cs2java.exe org.kbinani.dll
