SRC=pp_cs2java/BOMSkipFileInputStream.java \
        pp_cs2java/DirectiveUnit.java \
        pp_cs2java/Directives.java \
        pp_cs2java/ReplaceMode.java \
        pp_cs2java/WordReplaceContext.java \
        pp_cs2java/pp_cs2java.java \
        pp_cs2java/str.java \
        pp_cs2java/util.java \
        pp_cs2java/LocalDefines.java \
        pp_cs2java/LocalDefineUnit.java

pp_cs2java.jar: $(SRC)
	javac $(SRC) -encoding UTF8
	jar cfm pp_cs2java.jar pp_cs2java.mf pp_cs2java/*.class

clean:
	rm pp_cs2java.jar
