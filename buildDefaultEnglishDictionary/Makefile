all: wordsFromDBFiles.exe makeMids.exe extractDictionary.exe diffWordsList.exe mergeWordsList.exe mergeAll.exe AutoLyricInsert.exe AutoLyricInsertPostProcess.exe

AutoLyricInsertPostProcess.exe: AutoLyricInsertPostProcess.cs
	gmcs AutoLyricInsertPostProcess.cs /r:org.kbinani.dll,org.kbinani.vsq.dll

wordsFromDBFiles.exe: wordsFromDBFiles.cs
	gmcs wordsFromDBFiles.cs

makeMids.exe: makeMids.cs
	gmcs makeMids.cs -r:org.kbinani.dll,org.kbinani.vsq.dll

extractDictionary.exe: extractDictionary.cs
	gmcs extractDictionary.cs -r:org.kbinani.dll,org.kbinani.vsq.dll

diffWordsList.exe: diffWordsList.cs
	gmcs diffWordsList.cs

mergeWordsList.exe: mergeWordsList.cs
	gmcs mergeWordsList.cs

mergeAll.exe: mergeAll.cs
	gmcs mergeAll.cs

AutoLyricInsert.exe: AutoLyricInsert.c
	gcc AutoLyricInsert.c -o AutoLyricInsert.exe

install: AutoLyricInsertPostProcess.exe
	copy AutoLyricInsertPostProcess.exe AutoLyricInsert\received\AutoLyricInsertPostProcess.exe
	copy org.kbinani.dll AutoLyricInsert\received\org.kbinani.dll
	copy org.kbinani.vsq.dll AutoLyricInsert\received\org.kbinani.vsq.dll
