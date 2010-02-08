all: wordsFromDBFiles.exe makeMids.exe extractDictionary.exe diffWordsList.exe mergeWordsList.exe mergeAll.exe

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
