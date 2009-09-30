CP=cp
RM=rm

all: first win linux macosx

first:
	perl first.pl

win: first build/win/Cadencii.exe

linux: first build/linux/Cadencii.exe

macosx: first build/macosx/Cadencii.exe

build/win/Cadencii.exe: bocoree/bocoree.dll Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll Boare.Lib.Media/Boare.Lib.Media.dll Boare.Lib.Vsq/Boare.Lib.Vsq.dll Cadencii/Cadencii.exe PlaySound/win/PlaySound.dll
	$(CP) bocoree/bocoree.dll build/win/bocoree.dll
	$(CP) Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll build/win/Boare.Lib.AppUtil.dll
	$(CP) Boare.Lib.Media/Boare.Lib.Media.dll build/win/Boare.Lib.Media.dll
	$(CP) Boare.Lib.Vsq/Boare.Lib.Vsq.dll build/win/Boare.Lib.Vsq.dll
	$(CP) PlaySound/win/PlaySound.dll build/win/PlaySound.dll
	$(CP) Cadencii/Cadencii.exe build/win/Cadencii.exe

build/linux/Cadencii.exe: bocoree/bocoree.dll Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll Boare.Lib.Media/Boare.Lib.Media.dll Boare.Lib.Vsq/Boare.Lib.Vsq.dll Cadencii/Cadencii.exe PlaySound/linux/libPlaySound.so
	$(CP) bocoree/bocoree.dll build/linux/bocoree.dll
	$(CP) Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll build/linux/Boare.Lib.AppUtil.dll
	$(CP) Boare.Lib.Media/Boare.Lib.Media.dll build/linux/Boare.Lib.Media.dll
	$(CP) Boare.Lib.Vsq/Boare.Lib.Vsq.dll build/linux/Boare.Lib.Vsq.dll
	$(CP) PlaySound/linux/libPlaySound.so build/linux/libPlaySound.so
	$(CP) Cadencii/Cadencii.exe build/linux/Cadencii.exe

build/macosx/Cadencii.exe: bocoree/bocoree.dll Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll Boare.Lib.Media/Boare.Lib.Media.dll Boare.Lib.Vsq/Boare.Lib.Vsq.dll Cadencii/Cadencii.exe PlaySound/linux/PlaySound.dll
	$(CP) bocoree/bocoree.dll build/macosx/bocoree.dll
	$(CP) Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll build/macosx/Boare.Lib.AppUtil.dll
	$(CP) Boare.Lib.Media/Boare.Lib.Media.dll build/macosx/Boare.Lib.Media.dll
	$(CP) Boare.Lib.Vsq/Boare.Lib.Vsq.dll build/macosx/Boare.Lib.Vsq.dll
	$(CP) PlaySound/linux/PlaySound.dll build/macosx/PlaySound.dll
	$(CP) Cadencii/Cadencii.exe build/macosx/Cadencii.exe

bocoree/bocoree.dll: bocoree/*.cs
	cd bocoree && $(MAKE) CP=$(CP) RM=$(RM)

Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll: bocoree/bocoree.dll Boare.Lib.AppUtil/*.cs
	$(CP) bocoree/bocoree.dll Boare.Lib.AppUtil/bocoree.dll
	cd Boare.Lib.AppUtil && $(MAKE) CP=$(CP) RM=$(RM)

Boare.Lib.Media/Boare.Lib.Media.dll: bocoree/bocoree.dll Boare.Lib.Media/*.cs
	$(CP) bocoree/bocoree.dll Boare.Lib.Media/bocoree.dll
	cd Boare.Lib.Media && $(MAKE) CP=$(CP) RM=$(RM)

Boare.Lib.Vsq/Boare.Lib.Vsq.dll: bocoree/bocoree.dll Boare.Lib.Vsq/*.cs
	$(CP) bocoree/bocoree.dll Boare.Lib.Vsq/bocoree.dll
	cd Boare.Lib.Vsq && $(MAKE) CP=$(CP) RM=$(RM)

Cadencii/Cadencii.exe: Cadencii/*.cs bocoree/bocoree.dll Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll Boare.Lib.Media/Boare.Lib.Media.dll Boare.Lib.Vsq/Boare.Lib.Vsq.dll
	$(CP) bocoree/bocoree.dll Cadencii/bocoree.dll
	$(CP) Boare.Lib.AppUtil/Boare.Lib.AppUtil.dll Cadencii/Boare.Lib.AppUtil.dll
	$(CP) Boare.Lib.Media/Boare.Lib.Media.dll Cadencii/Boare.Lib.Media.dll
	$(CP) Boare.Lib.Vsq/Boare.Lib.Vsq.dll Cadencii/Boare.Lib.Vsq.dll
	cd Cadencii && $(MAKE) CP=$(CP) RM=$(RM)

PlaySound/win/PlaySound.dll: PlaySound/PlaySound.cpp PlaySound/PlaySound.h
	cd PlaySound && $(MAKE) win

PlaySound/linux/libPlaySound.so: PlaySound/PlaySound_linux.c
	cd PlaySound && $(MAKE) linux

clean:
	$(RM) build/win/bocoree.dll build/win/Boare.Lib.AppUtil.dll build/win/Boare.Lib.Media.dll build/win/Boare.Lib.Vsq.dll build/win/Cadencii.exe build/win/PlaySound.dll
	$(RM) build/linux/bocoree.dll build/linux/Boare.Lib.AppUtil.dll build/linux/Boare.Lib.Media.dll build/linux/Boare.Lib.Vsq.dll build/linux/Cadencii.exe build/linux/libPlaySound.so
	$(RM) build/macosx/bocoree.dll build/macosx/Boare.Lib.AppUtil.dll build/macosx/Boare.Lib.Media.dll build/macosx/Boare.Lib.Vsq.dll build/macosx/Cadencii.exe build/macosx/PlaySound.dll
	cd bocoree && $(MAKE) clean RM=$(RM) CP=$(CP)
	cd Boare.Lib.AppUtil && $(MAKE) clean RM=$(RM) CP=$(CP)
	cd Boare.Lib.Media && $(MAKE) clean RM=$(RM) CP=$(CP)
	cd Boare.Lib.Vsq && $(MAKE) clean RM=$(RM) CP=$(CP)
	cd Cadencii && $(MAKE) clean RM=$(RM) CP=$(CP)
	cd PlaySound && $(MAKE) clean RM=$(RM) CP=$(CP)
