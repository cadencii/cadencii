all: bocoree vsq apputil media cadencii editotoini

bocoree:
	pp_cs2java -DJAVA -DUSE_DOBJ -DRELEASE -b "..\port_java" -encoding "UTF-8" -s -4 -c -t "..\bocoree" -m "..\port_java\org_kbinani.java"

vsq:
	pp_cs2java -DJAVA -DUSE_DOBJ -DRELEASE -b "..\port_java" -encoding "UTF-8" -s -4 -c -t "..\Boare.Lib.Vsq" -m "..\port_java\org_kbinani_vsq.java"

apputil:
	pp_cs2java -DJAVA -DUSE_DOBJ -DRELEASE -b "..\port_java" -encoding "UTF-8" -s -4 -c -t "..\Boare.Lib.AppUtil" -m "..\port_java\org_kbinani_apputil.java"

media:
	pp_cs2java -DJAVA -DUSE_DOBJ -DRELEASE -b "..\port_java" -encoding "UTF-8" -s -4 -c -t "..\Boare.Lib.Media" -m "..\port_java\org_kbinani_media.java"

cadencii:
	pp_cs2java -DJAVA -DUSE_DOBJ -DRELEASE -b "..\port_java" -encoding "UTF-8" -s -4 -c -t "..\Cadencii" -m "..\port_java\org_kbinani_Cadencii.java"

editotoini:
	pp_cs2java -DJAVA -DUSE_DOBJ -DRELEASE -b "..\port_java" -encoding "UTF-8" -s -4 -c -t "..\EditOtoIni" -m "..\port_java\org_kbinani_EditOtoIni.java"

#compile example: $javac .\org_kbinani_Cadencii.java -encoding "UTF-8"
#                 $javac .\org\kbinani\Cadencii\Program.java -encoding "UTF-8"
#                 $javac .\org\kbinani\EditOtoIni\FormUtauVoiceConfig.java -encoding "UTF-8"
#execute: $java org.kbinani.Cadencii.Program
