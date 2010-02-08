all: wordsFromDBFiles.exe makeMids.exe

wordsFromDBFiles.exe: wordsFromDBFiles.cs
	gmcs wordsFromDBFiles.cs

makeMids.exe: makeMids.cs
	gmcs makeMids.cs -r:org.kbinani.dll,org.kbinani.vsq.dll

extractDictionary.exe: extractDictionary.cs
	gmcs extractDictionary.cs -r:org.kbinani.dll,org.kbinani.vsq.dll
