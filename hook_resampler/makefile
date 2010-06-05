all: resampler10.dll resampler.exe

resampler10.dll: resampler10.c resampler10.def
	gcc -shared -s -o resampler10.dll resampler10.c

resampler.exe: resampler.cs
	gmcs resampler.cs -r:System.Windows.Forms

install: resampler10.dll
	copy resampler10.dll "C:\Users\kbinani\Documents\Installer\UTAU\utau0260\utau0260\resampler10.dll"

clean:
	del resampler.exe resampler10.dll
