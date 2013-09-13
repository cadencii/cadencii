OPT=
CP=cp
RM=rm

Boare.Lib.AppUtil.dll: *.cs bocoree.dll
	gmcs $(OPT) -recurse:*.cs -unsafe+ -target:library -out:Boare.Lib.AppUtil.dll \
	     -r:bocoree.dll,System.Drawing,System.Windows.Forms

clean:
	$(RM) Boare.Lib.AppUtil.dll
	$(RM) bocoree.dll
