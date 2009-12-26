OPT=
RM=rm
CP=cp

#Cadencii.exe: bocoree.dll Boare.Lib.Media.dll vstildr.dll vstidrv.dll Boare.Lib.AppUtil.dll Boare.Lib.Vsq.dll

Cadencii.exe: bocoree.dll Boare.Lib.Media.dll Boare.Lib.Vsq.dll Boare.Lib.AppUtil.dll
	gmcs $(OPT) -unsafe+ -recurse:*.cs -target:exe -out:Cadencii.exe -warn:0 -define:MONO -define:USE_DOBJ -codepage:utf8 \
	     -define:ENABLE_VOCALOID;ENABLE_AQUESTONE;ENABLE_MIDI \
	     -r:System.Windows.Forms,System.Drawing \
	     -r:Boare.Lib.Media.dll,Boare.Lib.Vsq.dll,Boare.Lib.AppUtil.dll,bocoree.dll

FormMain.resources: FormMain.resx EditResx.pl
	perl EditResx.pl FormMain.resx
	resgen FormMain_.resx FormMain.resources

Properties/Resources.resources: Properties/Resources.resx EditResx.pl
	perl EditResx.pl Properties/Resources.resx
	cd Properties && resgen Resources_.resx Resources.resources

clean:
	$(RM) Cadencii.exe Boare.Lib.Media.dll Boare.Lib.Vsq.dll Boare.Lib.AppUtil.dll bocoree.dll
