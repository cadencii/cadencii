CP=cp
RM=rm

bocoree.dll: *.cs
	gmcs -recurse:*.cs -unsafe+ -target:library -out:bocoree.dll -r:System,System.Drawing,System.Windows.Forms

clean:
	$(RM) bocoree.dll
