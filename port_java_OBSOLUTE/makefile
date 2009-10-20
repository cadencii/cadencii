Cadencii.jar: clean com/boare/cadencii/*.java com/boare/util/*.java com/boare/corlib/*.java com/boare/vsq/*.java com/boare/windows/forms/*.java com/boare/*.java
	javac com/boare/cadencii/Program.java -encoding UTF8 -target 5 -source 5
	jar cfm Cadencii.jar Cadencii.mf com/boare/cadencii/*.class com/boare/util/*.class com/boare/corlib/*.class com/boare/vsq/*.class com/boare/media/*.class com/boare/*.class com/boare/windows/forms/*.class

VocaloSysUtil.exe: VocaloSysUtil.c file2binArraySrc.class
	gcc VocaloSysUtil.c -o VocaloSysUtil.exe -ladvapi32
	java file2binArraySrc VocaloSysUtil.exe src.txt

file2binArraySrc.class: file2binArraySrc.java
	javac file2binArraySrc.java -encoding UTF8

clean:
	del com\boare\corlib\*.class
	del com\boare\cadencii\*.class
	del com\boare\util\*.class
	del com\boare\vsq\*.class
	del com\boare\media\*.class
	del com\boare\*.class
	del com\boare\windows\forms\*.class
	del Cadencii.jar
	del file2binArraySrc.class
	del VocaloSysUtil.exe

doc:
	javadoc com.boare.vsq com.boare.corlib com.boare.util com.boare.media -encoding UTF8 -use -public

vjs:
	set PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v1.1.4322
	vjc /codepage:65001 /r:vjssupuilib.dll /recurse:com/boare/corlib/*.java

formtest:
	javac com/boare/windows/forms/FormMain.java -encoding UTF8
