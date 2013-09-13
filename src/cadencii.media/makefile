CP=cp
RM=rm
SRC=AviReader.cs AviWriter.cs RawAvi2Writer.cs AviWriterEx.cs VCM.cs Util.cs VFW.cs MediaPlayer.cs Wave.cs
OPT=

Boare.Lib.Media.dll: *.cs bocoree.dll
	gmcs $(OPT) -recurse:*.cs -unsafe+ -target:library -out:Boare.Lib.Media.dll -r:bocoree.dll,System,System.Drawing,System.Windows.Forms -define:MONO

bocoree.dll: ../bocoree/bocoree.dll
	cp ../bocoree/bocoree.dll bocoree.dll

../bocoree/bocoree.dll:
	cd ../bocoree/ && $(MAKE) OPT=$(OPT)

clean:
	$(RM) Boare.Lib.Media.dll
	$(RM) bocoree.dll
