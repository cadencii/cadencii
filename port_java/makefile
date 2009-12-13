RM=del

all: Cadencii.jar

apputil:
	javac org_kbinani_apputil.java -encoding UTF-8

corlib:
	javac org_kbinani.java -encoding UTF-8

media:
	javac org_kbinani_media.java -encoding UTF-8

vsq:
	javac org_kbinani_vsq.java -encoding UTF-8

Cadencii.jar: clean org/kbinani/cadencii/*.java org/kbinani/apputil/*.java org/kbinani/vsq/*.java org/kbinani/media/*.java org/kbinani/windows/forms/*.java org/kbinani/componentmodel/*.java org/kbinani/*.java org/kbinani/xml/*.java
	javac org/kbinani/cadencii/Program.java -encoding UTF8 -target 5 -source 5
	jar cfm Cadencii.jar Cadencii.mf org/kbinani/cadencii/*.class org/kbinani/apputil/*.class org/kbinani/vsq/*.class org/kbinani/media/*.class org/kbinani/windows/forms/*.class org/kbinani/componentmodel/*.class org/kbinani/*.class org/kbinani/xml/*.class

clean:
	$(RM) org\kbinani\cadencii\*.class
	$(RM) org\kbinani\apputil\*.class
	$(RM) org\kbinani\vsq\*.class
	$(RM) org\kbinani\media\*.class
	$(RM) org\kbinani\windows\forms\*.class
	$(RM) org\kbinani\componentmodel\*.class
	$(RM) org\kbinani\*.class
	$(RM) org\kbinani\xml\*.class
	$(RM) Cadencii.jar
