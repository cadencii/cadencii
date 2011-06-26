SRC=pp_cs2java/BOMSkipFileInputStream.java \
        pp_cs2java/CommentPosition.java \
        pp_cs2java/Evaluator.java \
        pp_cs2java/Preprocessor.java \
        pp_cs2java/ReplaceMode.java \
        pp_cs2java/SourceText.java \
        pp_cs2java/str.java \
        pp_cs2java/util.java

pp_cs2java.jar: $(SRC)
	javac $(SRC) -encoding UTF8
	jar cfm pp_cs2java.jar pp_cs2java.mf pp_cs2java/*.class

clean:
	rm pp_cs2java.jar
	rm pp_cs2java/*.class
