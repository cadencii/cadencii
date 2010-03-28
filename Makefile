CP=cp
RM=rm
TARGET=./build/win
MCS_OPT=-warn:0
JROOT=./build/java/org/kbinani
PPCS2JAVA_OPT=-DJAVA -DRELEASE -DCLIPBOARD_AS_TEXT -encoding "UTF-8" -s -4 -c

SRC_JAPPUTIL=./build/java/org/kbinani/apputil/AuthorListEntry.java ./build/java/org/kbinani/apputil/InputBox.java \
            ./build/java/org/kbinani/apputil/MessageBody.java ./build/java/org/kbinani/apputil/MessageBodyEntry.java \
            ./build/java/org/kbinani/apputil/PolylineDrawer.java ./build/java/org/kbinani/apputil/Util.java \
            ./build/java/org/kbinani/apputil/Messaging.java

SRC_JCORLIB=./build/java/org/kbinani/Base64.java ./build/java/org/kbinani/BDelegate.java \
            ./build/java/org/kbinani/BEvent.java ./build/java/org/kbinani/BEventArgs.java \
            ./build/java/org/kbinani/BEventHandler.java ./build/java/org/kbinani/ByRef.java \
            ./build/java/org/kbinani/InternalStdErr.java ./build/java/org/kbinani/InternalStdOut.java \
            ./build/java/org/kbinani/math.java ./build/java/org/kbinani/PortUtil.java \
            ./build/java/org/kbinani/ValuePair.java ./build/java/org/kbinani/XmlPoint.java \
            ./build/java/org/kbinani/XmlRectangle.java \
            ./build/java/org/kbinani/componentmodel/BBackgroundWorker.java ./build/java/org/kbinani/componentmodel/BCancelEventArgs.java \
            ./build/java/org/kbinani/componentmodel/BCancelEventHandler.java ./build/java/org/kbinani/componentmodel/BDoWorkEventArgs.java \
            ./build/java/org/kbinani/componentmodel/BDoWorkEventHandler.java ./build/java/org/kbinani/componentmodel/BProgressChangedEventArgs.java \
            ./build/java/org/kbinani/componentmodel/BProgressChangedEventHandler.java ./build/java/org/kbinani/componentmodel/BRunWorkerCompletedEventArgs.java \
            ./build/java/org/kbinani/componentmodel/BRunWorkerCompletedEventHandler.java \
            ./build/java/org/kbinani/windows/forms/BButton.java ./build/java/org/kbinani/windows/forms/BCheckBox.java \
            ./build/java/org/kbinani/windows/forms/BComboBox.java ./build/java/org/kbinani/windows/forms/BDialog.java \
            ./build/java/org/kbinani/windows/forms/BDialogResult.java ./build/java/org/kbinani/windows/forms/BFileChooser.java \
            ./build/java/org/kbinani/windows/forms/BFolderBrowser.java ./build/java/org/kbinani/windows/forms/BFontChooser.java \
            ./build/java/org/kbinani/windows/forms/BForm.java ./build/java/org/kbinani/windows/forms/BFormClosedEventArgs.java \
            ./build/java/org/kbinani/windows/forms/BFormClosedEventHandler.java ./build/java/org/kbinani/windows/forms/BFormClosingEventArgs.java \
            ./build/java/org/kbinani/windows/forms/BFormClosingEventHandler.java ./build/java/org/kbinani/windows/forms/BFormWindowState.java \
            ./build/java/org/kbinani/windows/forms/BGroupBox.java ./build/java/org/kbinani/windows/forms/BHScrollBar.java \
            ./build/java/org/kbinani/windows/forms/BKeyEventArgs.java ./build/java/org/kbinani/windows/forms/BKeyEventHandler.java \
            ./build/java/org/kbinani/windows/forms/BKeyPressEventArgs.java ./build/java/org/kbinani/windows/forms/BKeyPressEventHandler.java \
            ./build/java/org/kbinani/windows/forms/BKeys.java ./build/java/org/kbinani/windows/forms/BLabel.java \
            ./build/java/org/kbinani/windows/forms/BListView.java ./build/java/org/kbinani/windows/forms/BListViewItem.java \
            ./build/java/org/kbinani/windows/forms/BMenu.java ./build/java/org/kbinani/windows/forms/BMenuBar.java \
            ./build/java/org/kbinani/windows/forms/BMenuItem.java ./build/java/org/kbinani/windows/forms/BMenuSeparator.java \
            ./build/java/org/kbinani/windows/forms/BMouseButtons.java ./build/java/org/kbinani/windows/forms/BMouseEventArgs.java \
            ./build/java/org/kbinani/windows/forms/BMouseEventHandler.java ./build/java/org/kbinani/windows/forms/BNumericUpDown.java \
            ./build/java/org/kbinani/windows/forms/BPaintEventArgs.java ./build/java/org/kbinani/windows/forms/BPaintEventHandler.java \
            ./build/java/org/kbinani/windows/forms/BPanel.java ./build/java/org/kbinani/windows/forms/BPictureBox.java \
            ./build/java/org/kbinani/windows/forms/BPopupMenu.java ./build/java/org/kbinani/windows/forms/BPreviewKeyDownEventArgs.java \
            ./build/java/org/kbinani/windows/forms/BPreviewKeyDownEventHandler.java ./build/java/org/kbinani/windows/forms/BProgressBar.java \
            ./build/java/org/kbinani/windows/forms/BRadioButton.java ./build/java/org/kbinani/windows/forms/BScrollBar.java \
            ./build/java/org/kbinani/windows/forms/BSlider.java ./build/java/org/kbinani/windows/forms/BSplitPane.java \
            ./build/java/org/kbinani/windows/forms/BStatusLabel.java ./build/java/org/kbinani/windows/forms/BTextArea.java \
            ./build/java/org/kbinani/windows/forms/BTextBox.java ./build/java/org/kbinani/windows/forms/BTimer.java \
            ./build/java/org/kbinani/windows/forms/BToggleButton.java ./build/java/org/kbinani/windows/forms/BToolBar.java \
            ./build/java/org/kbinani/windows/forms/BToolStripButton.java ./build/java/org/kbinani/windows/forms/BVScrollBar.java \
            ./build/java/org/kbinani/xml/XmlMember.java ./build/java/org/kbinani/xml/XmlSerializable.java \
            ./build/java/org/kbinani/xml/XmlSerializer.java

SRC_JMEDIA=./build/java/org/kbinani/media/BSoundPlayer.java ./build/java/org/kbinani/media/IWaveReceiver.java \
           ./build/java/org/kbinani/media/MidiInDevice.java ./build/java/org/kbinani/media/Wave.java \
           ./build/java/org/kbinani/media/WaveRateConvertAdapter.java ./build/java/org/kbinani/media/WaveRateConverter.java \
           ./build/java/org/kbinani/media/WaveReader.java ./build/java/org/kbinani/media/WaveWriter.java

SRC_JVSQ=./build/java/org/kbinani/vsq/BPPair.java ./build/java/org/kbinani/vsq/DynamicsMode.java \
           ./build/java/org/kbinani/vsq/ExpressionConfigSys.java ./build/java/org/kbinani/vsq/IconDynamicsHandle.java \
           ./build/java/org/kbinani/vsq/IconHandle.java ./build/java/org/kbinani/vsq/IconParameter.java \
           ./build/java/org/kbinani/vsq/IndexIteratorKind.java ./build/java/org/kbinani/vsq/ITextWriter.java \
           ./build/java/org/kbinani/vsq/Lyric.java ./build/java/org/kbinani/vsq/LyricHandle.java \
           ./build/java/org/kbinani/vsq/MidiEvent.java ./build/java/org/kbinani/vsq/MidiFile.java \
           ./build/java/org/kbinani/vsq/NoteHeadHandle.java ./build/java/org/kbinani/vsq/NRPN.java \
           ./build/java/org/kbinani/vsq/NrpnData.java ./build/java/org/kbinani/vsq/NrpnIterator.java \
           ./build/java/org/kbinani/vsq/PlayMode.java ./build/java/org/kbinani/vsq/SingerConfig.java \
           ./build/java/org/kbinani/vsq/SingerConfigSys.java ./build/java/org/kbinani/vsq/SymbolTable.java \
           ./build/java/org/kbinani/vsq/SynthesizerType.java ./build/java/org/kbinani/vsq/TempoTableEntry.java \
           ./build/java/org/kbinani/vsq/TempoVector.java ./build/java/org/kbinani/vsq/TextStream.java \
           ./build/java/org/kbinani/vsq/Timesig.java ./build/java/org/kbinani/vsq/TimeSigTableEntry.java \
           ./build/java/org/kbinani/vsq/TransCodeUtil.java ./build/java/org/kbinani/vsq/UstEnvelope.java \
           ./build/java/org/kbinani/vsq/UstEvent.java ./build/java/org/kbinani/vsq/UstFile.java \
           ./build/java/org/kbinani/vsq/UstPortamento.java ./build/java/org/kbinani/vsq/UstPortamentoPoint.java \
           ./build/java/org/kbinani/vsq/UstPortamentoType.java ./build/java/org/kbinani/vsq/UstTrack.java \
           ./build/java/org/kbinani/vsq/UstVibrato.java ./build/java/org/kbinani/vsq/VibratoBPList.java \
           ./build/java/org/kbinani/vsq/VibratoBPPair.java ./build/java/org/kbinani/vsq/VibratoHandle.java \
           ./build/java/org/kbinani/vsq/VocaloSysUtil.java ./build/java/org/kbinani/vsq/VsqBarLineType.java \
           ./build/java/org/kbinani/vsq/VsqBPList.java ./build/java/org/kbinani/vsq/VsqBPPair.java \
           ./build/java/org/kbinani/vsq/VsqBPPairSearchContext.java ./build/java/org/kbinani/vsq/VsqCommand.java \
           ./build/java/org/kbinani/vsq/VsqCommandType.java ./build/java/org/kbinani/vsq/VsqCommon.java \
           ./build/java/org/kbinani/vsq/VsqEvent.java ./build/java/org/kbinani/vsq/VsqEventList.java \
           ./build/java/org/kbinani/vsq/VsqFile.java ./build/java/org/kbinani/vsq/VsqHandle.java \
           ./build/java/org/kbinani/vsq/VsqHandleType.java ./build/java/org/kbinani/vsq/VsqID.java \
           ./build/java/org/kbinani/vsq/VsqIDType.java ./build/java/org/kbinani/vsq/VsqMaster.java \
           ./build/java/org/kbinani/vsq/VsqMetaText.java ./build/java/org/kbinani/vsq/VsqMixer.java \
           ./build/java/org/kbinani/vsq/VsqMixerEntry.java ./build/java/org/kbinani/vsq/VsqNote.java \
           ./build/java/org/kbinani/vsq/VsqNrpn.java ./build/java/org/kbinani/vsq/VsqPhoneticSymbol.java \
           ./build/java/org/kbinani/vsq/VsqTrack.java ./build/java/org/kbinani/vsq/VsqVoiceLanguage.java \
           ./build/java/org/kbinani/vsq/WrappedStreamWriter.java

SRC_JCADENCII=./build/java/org/kbinani/cadencii/AmplifyCoefficient.java ./build/java/org/kbinani/cadencii/AppManager.java \
              ./build/java/org/kbinani/cadencii/AttachedCurve.java ./build/java/org/kbinani/cadencii/AttackVariation.java \
              ./build/java/org/kbinani/cadencii/AutoVibratoMinLengthEnum.java \
              ./build/java/org/kbinani/cadencii/AutoVibratoMinLengthUtil.java ./build/java/org/kbinani/cadencii/BAssemblyInfo.java \
              ./build/java/org/kbinani/cadencii/BezierChain.java ./build/java/org/kbinani/cadencii/BezierControlType.java \
              ./build/java/org/kbinani/cadencii/BezierCurves.java ./build/java/org/kbinani/cadencii/BezierPickedSide.java \
              ./build/java/org/kbinani/cadencii/BezierPoint.java ./build/java/org/kbinani/cadencii/BgmFile.java \
              ./build/java/org/kbinani/cadencii/BooleanEnum.java ./build/java/Cadencii.java \
              ./build/java/org/kbinani/cadencii/CadenciiCommand.java ./build/java/org/kbinani/cadencii/CadenciiCommandType.java \
              ./build/java/org/kbinani/cadencii/ClipboardEntry.java ./build/java/org/kbinani/cadencii/ClockResolution.java \
              ./build/java/org/kbinani/cadencii/ClockResolutionUtility.java \
              ./build/java/org/kbinani/cadencii/CurveType.java \
              ./build/java/org/kbinani/cadencii/DefaultVibratoLengthEnum.java ./build/java/org/kbinani/cadencii/DefaultVibratoLengthUtil.java \
              ./build/java/org/kbinani/cadencii/DrawObject.java ./build/java/org/kbinani/cadencii/DrawObjectType.java \
              ./build/java/org/kbinani/cadencii/DynaffComparisonContext.java ./build/java/org/kbinani/cadencii/EditedZone.java \
              ./build/java/org/kbinani/cadencii/EditedZoneCommand.java ./build/java/org/kbinani/cadencii/EditedZoneUnit.java \
              ./build/java/org/kbinani/cadencii/EditMode.java ./build/java/org/kbinani/cadencii/EditorConfig.java \
              ./build/java/org/kbinani/cadencii/EditorStatus.java ./build/java/org/kbinani/cadencii/EditTool.java \
              ./build/java/org/kbinani/cadencii/EmptyRenderingRunner.java ./build/java/org/kbinani/cadencii/FederChangedEventHandler.java \
              ./build/java/org/kbinani/cadencii/FormAskKeySoundGeneration.java ./build/java/org/kbinani/cadencii/FormBeatConfig.java \
              ./build/java/org/kbinani/cadencii/FormBezierPointEdit.java ./build/java/org/kbinani/cadencii/FormCompileResult.java \
              ./build/java/org/kbinani/cadencii/FormCurvePointEdit.java ./build/java/org/kbinani/cadencii/FormDeleteBar.java \
              ./build/java/org/kbinani/cadencii/FormExportMusicXml.java ./build/java/org/kbinani/cadencii/FormGameControlerConfig.java \
              ./build/java/org/kbinani/cadencii/FormGenerateKeySound.java ./build/java/org/kbinani/cadencii/FormIconPalette.java \
              ./build/java/org/kbinani/cadencii/FormImportLyric.java ./build/java/org/kbinani/cadencii/FormInsertBar.java \
              ./build/java/org/kbinani/cadencii/FormMain.java \
              ./build/java/org/kbinani/cadencii/FormMidiImExport.java ./build/java/org/kbinani/cadencii/FormMidiImExportConfig.java \
              ./build/java/org/kbinani/cadencii/FormMixer.java ./build/java/org/kbinani/cadencii/FormNoteExpressionConfig.java \
              ./build/java/org/kbinani/cadencii/FormNoteProperty.java ./build/java/org/kbinani/cadencii/FormPluginUi.java \
              ./build/java/org/kbinani/cadencii/FormRandomize.java ./build/java/org/kbinani/cadencii/FormRealtimeConfig.java \
              ./build/java/org/kbinani/cadencii/FormShortcutKeys.java ./build/java/org/kbinani/cadencii/FormSingerStyleConfig.java \
              ./build/java/org/kbinani/cadencii/FormSplash.java ./build/java/org/kbinani/cadencii/FormSynthesize.java \
              ./build/java/org/kbinani/cadencii/FormTempoConfig.java \
              ./build/java/org/kbinani/cadencii/FormVibratoConfig.java ./build/java/org/kbinani/cadencii/FormWordDictionary.java \
              ./build/java/org/kbinani/cadencii/HScroll.java ./build/java/org/kbinani/cadencii/ICommand.java \
              ./build/java/org/kbinani/cadencii/ICommandRunnable.java ./build/java/org/kbinani/cadencii/IComparisonContext.java \
              ./build/java/org/kbinani/cadencii/KanaDeRomanization.java \
              ./build/java/org/kbinani/cadencii/KeySoundPlayer.java \
              ./build/java/org/kbinani/cadencii/MidiPortConfig.java \
              ./build/java/org/kbinani/cadencii/MuteChangedEventHandler.java ./build/java/org/kbinani/cadencii/NoteNumberExpressionType.java \
              ./build/java/org/kbinani/cadencii/NumberTextBox.java ./build/java/org/kbinani/cadencii/NumericUpDownEx.java \
              ./build/java/org/kbinani/cadencii/OtoArgs.java \
              ./build/java/org/kbinani/cadencii/PanelState.java \
              ./build/java/org/kbinani/cadencii/PanpotChangedEventHandler.java ./build/java/org/kbinani/cadencii/PencilMode.java \
              ./build/java/org/kbinani/cadencii/PencilModeEnum.java ./build/java/org/kbinani/cadencii/PictPianoRoll.java \
              ./build/java/org/kbinani/cadencii/PlatformEnum.java ./build/java/org/kbinani/cadencii/PlayPositionSpecifier.java \
              ./build/java/org/kbinani/cadencii/PlaySound.java ./build/java/org/kbinani/cadencii/PointD.java \
              ./build/java/org/kbinani/cadencii/Preference.java ./build/java/org/kbinani/cadencii/PropertyPanel.java \
              ./build/java/org/kbinani/cadencii/PropertyPanelState.java \
              ./build/java/org/kbinani/cadencii/QuantizeMode.java ./build/java/org/kbinani/cadencii/QuantizeModeUtil.java \
              ./build/java/org/kbinani/cadencii/RenderedStatus.java \
              ./build/java/org/kbinani/cadencii/RendererKind.java ./build/java/org/kbinani/cadencii/RenderingRunner.java \
              ./build/java/org/kbinani/cadencii/RenderQueue.java ./build/java/org/kbinani/cadencii/RenderRequiredEventHandler.java \
              ./build/java/org/kbinani/cadencii/Resources.java ./build/java/org/kbinani/cadencii/RgbColor.java \
              ./build/java/org/kbinani/cadencii/SandBox.java ./build/java/org/kbinani/cadencii/ScreenStatus.java \
              ./build/java/org/kbinani/cadencii/SelectedBezierPoint.java \
              ./build/java/org/kbinani/cadencii/SelectedCurveChangedEventHandler.java ./build/java/org/kbinani/cadencii/SelectedEventChangedEventHandler.java \
              ./build/java/org/kbinani/cadencii/SelectedEventEntry.java \
              ./build/java/org/kbinani/cadencii/SelectedRegion.java \
              ./build/java/org/kbinani/cadencii/SelectedTempoEntry.java ./build/java/org/kbinani/cadencii/SelectedTimesigEntry.java \
              ./build/java/org/kbinani/cadencii/SelectedTrackChangedEventHandler.java ./build/java/org/kbinani/cadencii/SingerEventComparisonContext.java \
              ./build/java/org/kbinani/cadencii/SoloChangedEventHandler.java ./build/java/org/kbinani/cadencii/StateChangeRequiredEventHandler.java \
              ./build/java/org/kbinani/cadencii/StraightRenderingQueue.java ./build/java/org/kbinani/cadencii/StraightRenderingRunner.java \
              ./build/java/org/kbinani/cadencii/TagLyricTextBox.java \
              ./build/java/org/kbinani/cadencii/TextBoxEx.java ./build/java/org/kbinani/cadencii/ToolStripLocation.java \
              ./build/java/org/kbinani/cadencii/TopMostChangedEventHandler.java ./build/java/org/kbinani/cadencii/TrackSelector.java \
              ./build/java/org/kbinani/cadencii/UtauFreq.java \
              ./build/java/org/kbinani/cadencii/UtauRenderingRunner.java ./build/java/org/kbinani/cadencii/UtauVoiceDB.java \
              ./build/java/org/kbinani/cadencii/Utility.java ./build/java/org/kbinani/cadencii/ValuePairOfStringArrayOfKeys.java \
              ./build/java/org/kbinani/cadencii/VersionInfo.java ./build/java/org/kbinani/cadencii/VibratoLengthEditingRule.java \
              ./build/java/org/kbinani/cadencii/VibratoPointIterator.java \
              ./build/java/org/kbinani/cadencii/VolumeTracker.java ./build/java/org/kbinani/cadencii/VsqBPListComparisonContext.java \
              ./build/java/org/kbinani/cadencii/VsqFileEx.java \
              ./build/java/org/kbinani/cadencii/VSTiProxy.java \
              ./build/java/org/kbinani/cadencii/WaveDrawContext.java \
              ./build/java/org/kbinani/cadencii/WaveView.java

.SUFFIXES:	.cs .java

.cs.java:
	mono ./pp_cs2java.exe $(PPCS2JAVA_OPT) -i $< -o $@

all: first $(TARGET)/Cadencii.exe

first: ./first.pl
	perl ./first.pl

work/Test.java: work/Test.cs

jcadencii_win: jcadencii
	exewrap -g -t 1.6 -o .\build\java\jCadencii.exe -i .\Cadencii\resources\icon.ico .\build\java\Cadencii.jar

jcadencii: pp_cs2java.exe jcorlib japputil jmedia jvsq $(SRC_JCADENCII) ./Cadencii/Resources.cs
#	mono ./pp_cs2java.exe $(PPCS2JAVA_OPT) -b ./build/java -t ./Cadencii
	$(CP) ./build/java/org/kbinani/cadencii/NumberTextBox.java ./BuildJavaUI/src/org/kbinani/cadencii/NumberTextBox.java
	$(CP) ./build/java/org/kbinani/cadencii/NumericUpDownEx.java ./BuildJavaUI/src/org/kbinani/cadencii/NumericUpDownEx.java
	javac ./build/java/Cadencii.java ./build/java/org/kbinani/cadencii/*.java ./build/java/org/kbinani/*.java ./build/java/org/kbinani/apputil/*.java ./build/java/org/kbinani/media/*.java ./build/java/org/kbinani/vsq/*.java ./build/java/org/kbinani/cadencii/*.java ./build/java/org/kbinani/componentmodel/*.java ./build/java/org/kbinani/windows/forms/*.java ./build/java/org/kbinani/xml/*.java -encoding UTF8 -target 1.5 -source 1.5
	$(CP) ./Cadencii/Cadencii.mf ./build/java/Cadencii.mf
	cd ./build/java && jar cfm Cadencii.jar Cadencii.mf Cadencii.class org/kbinani/cadencii/*.class org/kbinani/*.class org/kbinani/apputil/*.class org/kbinani/media/*.class org/kbinani/vsq/*.class org/kbinani/cadencii/*.class org/kbinani/componentmodel/*.class org/kbinani/windows/forms/*.class org/kbinani/xml/*.class

jeditotoini: pp_cs2java.exe jcorlib japputil jmedia jvsq
	mono ./pp_cs2java.exe $(PPCS2JAVA_OPT) -b ./build/java -t ./EditOtoIni
	javac $(JROOT)/editotoini/*.java $(JROOT)/*.java $(JROOT)/windows/forms/*.java $(JROOT)/componentmodel/*.java $(JROOT)/vsq/*.java $(JROOT)/xml/*.java $(JROOT)/apputil/*.java $(JROOT)/media/*.java -encoding UTF8

jcorlib: pp_cs2java.exe $(SRC_JCORLIB)
	$(CP) ./build/java/org/kbinani/math.java ./BuildJavaUI/src/org/kbinani/math.java
	$(CP) ./build/java/org/kbinani/PortUtil.java ./BuildJavaUI/src/org/kbinani/PortUtil.java
	$(CP) ./build/java/org/kbinani/InternalStdErr.java ./BuildJavaUI/src/org/kbinani/InternalStdErr.java
	$(CP) ./build/java/org/kbinani/InternalStdOut.java ./BuildJavaUI/src/org/kbinani/InternalStdOut.java
	$(CP) ./build/java/org/kbinani/ByRef.java ./BuildJavaUI/src/org/kbinani/ByRef.java
	$(CP) ./build/java/org/kbinani/xml/XmlSerializable.java ./BuildJavaUI/src/org/kbinani/xml/XmlSerializable.java

japputil: pp_cs2java.exe $(SRC_JAPPUTIL)

jmedia: pp_cs2java.exe $(SRC_JMEDIA)

jvsq: pp_cs2java.exe $(SRC_JVSQ)

pp_cs2java.exe: first $(TARGET)/org.kbinani.dll ./pp_cs2java/Program.cs
	gmcs ./pp_cs2java/Program.cs -out:./pp_cs2java.exe -r:$(TARGET)/org.kbinani.dll,System.Drawing $(MCS_OPT) -define:DEBUG
	$(CP) $(TARGET)/org.kbinani.dll ./org.kbinani.dll

$(TARGET)/Cadencii.exe: ./Cadencii/Resources.cs ./Cadencii/*.cs $(TARGET)/org.kbinani.dll $(TARGET)/org.kbinani.apputil.dll $(TARGET)/org.kbinani.media.dll $(TARGET)/org.kbinani.vsq.dll $(TARGET)/PlaySound.dll
	gmcs -recurse:./Cadencii/*.cs -define:MONO,ENABLE_VOCALOID,ENABLE_AQUESTONE,ENABLE_MIDI,ENABLE_PROPERTY -out:$(TARGET)/Cadencii.exe -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll,$(TARGET)/org.kbinani.apputil.dll,$(TARGET)/org.kbinani.media.dll,$(TARGET)/org.kbinani.vsq.dll -unsafe+ -codepage:utf8 $(MCS_OPT)

./Cadencii/Resources.cs: ./Cadencii/Resources.list makeRes.exe
	mono ./makeRes.exe -i ./Cadencii/Resources.list -o ./Cadencii/Resources.cs -p org.kbinani.cadencii -n org.kbinani.cadencii

makeRes.exe: ./makeRes.cs
	gmcs makeRes.cs $(MCS_OPT)

$(TARGET)/PlaySound.dll: ./PlaySound2/PlaySound2.cpp ./PlaySound2/PlaySound2.h
	g++ -shared -s -o $(TARGET)/PlaySound.dll ./PlaySound2/PlaySound2.cpp -lwinmm

$(TARGET)/org.kbinani.dll: ./org.kbinani/*.cs
	gmcs -recurse:./org.kbinani/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.dll -r:System.Windows.Forms,System.Drawing -unsafe+ -codepage:utf8 $(MCS_OPT)

$(TARGET)/org.kbinani.apputil.dll: $(TARGET)/org.kbinani.dll ./org.kbinani.apputil/*.cs
	gmcs -recurse:./org.kbinani.apputil/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.apputil.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll -unsafe+ -codepage:utf8 $(MCS_OPT)

$(TARGET)/org.kbinani.media.dll: $(TARGET)/org.kbinani.dll ./org.kbinani.media/*.cs
	gmcs -recurse:./org.kbinani.media/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.media.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll -unsafe+ -codepage:utf8 $(MCS_OPT)

$(TARGET)/org.kbinani.vsq.dll: $(TARGET)/org.kbinani.dll ./org.kbinani.vsq/*.cs
	gmcs -recurse:./org.kbinani.vsq/*.cs -target:library -define:MONO -out:$(TARGET)/org.kbinani.vsq.dll -r:System.Windows.Forms,System.Drawing,$(TARGET)/org.kbinani.dll  -codepage:utf8 $(MCS_OPT)

doc: jcadencii
	javadoc -sourcepath ./build/java org.kbinani.vsq org.kbinani org.kbinani.apputil org.kbinani.media org.kbinani.cadencii -encoding UTF8 -use -public

clean:
	$(RM) $(TARGET)/org.kbinani.dll $(TARGET)/org.kbinani.apputil.dll $(TARGET)/org.kbinani.media.dll $(TARGET)/org.kbinani.vsq.dll $(TARGET)/Cadencii.exe $(TARGET)/PlaySound.dll pp_cs2java.exe org.kbinani.dll

./build/java/org/kbinani/apputil/AuthorListEntry.java: ./org.kbinani.apputil/AuthorListEntry.java
	$(CP) $< $@
./build/java/org/kbinani/apputil/InputBox.java: ./org.kbinani.apputil/InputBox.java
	$(CP) $< $@
./build/java/org/kbinani/apputil/MessageBody.java: ./org.kbinani.apputil/MessageBody.java
	$(CP) $< $@
./build/java/org/kbinani/apputil/MessageBodyEntry.java: ./org.kbinani.apputil/MessageBodyEntry.java
	$(CP) $< $@
./build/java/org/kbinani/apputil/PolylineDrawer.java: ./org.kbinani.apputil/PolylineDrawer.java
	$(CP) $< $@
./build/java/org/kbinani/apputil/Util.java: ./org.kbinani.apputil/Util.java
	$(CP) $< $@
./build/java/org/kbinani/apputil/Messaging.java: ./org.kbinani.apputil/Messaging.java
	$(CP) $< $@
./build/java/org/kbinani/Base64.java: ./org.kbinani/Base64.java
	$(CP) $< $@
./build/java/org/kbinani/BDelegate.java: ./org.kbinani/BDelegate.java
	$(CP) $< $@
./build/java/org/kbinani/BEvent.java: ./org.kbinani/BEvent.java
	$(CP) $< $@
./build/java/org/kbinani/BEventArgs.java: ./org.kbinani/BEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/BEventHandler.java: ./org.kbinani/BEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/ByRef.java: ./org.kbinani/ByRef.java
	$(CP) $< $@
./build/java/org/kbinani/InternalStdErr.java: ./org.kbinani/InternalStdErr.java
	$(CP) $< $@
./build/java/org/kbinani/InternalStdOut.java: ./org.kbinani/InternalStdOut.java
	$(CP) $< $@
./build/java/org/kbinani/math.java: ./org.kbinani/math.java
	$(CP) $< $@
./build/java/org/kbinani/PortUtil.java: ./org.kbinani/PortUtil.java
	$(CP) $< $@
./build/java/org/kbinani/ValuePair.java: ./org.kbinani/ValuePair.java
	$(CP) $< $@
./build/java/org/kbinani/XmlPoint.java: ./org.kbinani/XmlPoint.java
	$(CP) $< $@
./build/java/org/kbinani/XmlRectangle.java: ./org.kbinani/XmlRectangle.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BBackgroundWorker.java: ./org.kbinani/BBackgroundWorker.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BCancelEventArgs.java: ./org.kbinani/BCancelEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BCancelEventHandler.java: ./org.kbinani/BCancelEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BDoWorkEventArgs.java: ./org.kbinani/BDoWorkEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BDoWorkEventHandler.java: ./org.kbinani/BDoWorkEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BProgressChangedEventArgs.java: ./org.kbinani/BProgressChangedEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BProgressChangedEventHandler.java: ./org.kbinani/BProgressChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BRunWorkerCompletedEventArgs.java: ./org.kbinani/BRunWorkerCompletedEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/componentmodel/BRunWorkerCompletedEventHandler.java: ./org.kbinani/BRunWorkerCompletedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BButton.java: ./org.kbinani/BButton.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BCheckBox.java: ./org.kbinani/BCheckBox.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BComboBox.java: ./org.kbinani/BComboBox.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BDialog.java: ./org.kbinani/BDialog.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BDialogResult.java: ./org.kbinani/BDialogResult.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFileChooser.java: ./org.kbinani/BFileChooser.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFolderBrowser.java: ./org.kbinani/BFolderBrowser.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFontChooser.java: ./org.kbinani/BFontChooser.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BForm.java: ./org.kbinani/BForm.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFormClosedEventArgs.java: ./org.kbinani/BFormClosedEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFormClosedEventHandler.java: ./org.kbinani/BFormClosedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFormClosingEventArgs.java: ./org.kbinani/BFormClosingEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFormClosingEventHandler.java: ./org.kbinani/BFormClosingEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BFormWindowState.java: ./org.kbinani/BFormWindowState.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BGroupBox.java: ./org.kbinani/BGroupBox.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BHScrollBar.java: ./org.kbinani/BHScrollBar.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BKeyEventArgs.java: ./org.kbinani/BKeyEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BKeyEventHandler.java: ./org.kbinani/BKeyEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BKeyPressEventArgs.java: ./org.kbinani/BKeyPressEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BKeyPressEventHandler.java: ./org.kbinani/BKeyPressEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BKeys.java: ./org.kbinani/BKeys.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BLabel.java: ./org.kbinani/BLabel.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BListView.java: ./org.kbinani/BListView.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BListViewItem.java: ./org.kbinani/BListViewItem.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMenu.java: ./org.kbinani/BMenu.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMenuBar.java: ./org.kbinani/BMenuBar.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMenuItem.java: ./org.kbinani/BMenuItem.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMenuSeparator.java: ./org.kbinani/BMenuSeparator.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMouseButtons.java: ./org.kbinani/BMouseButtons.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMouseEventArgs.java: ./org.kbinani/BMouseEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BMouseEventHandler.java: ./org.kbinani/BMouseEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BNumericUpDown.java: ./org.kbinani/BNumericUpDown.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPaintEventArgs.java: ./org.kbinani/BPaintEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPaintEventHandler.java: ./org.kbinani/BPaintEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPanel.java: ./org.kbinani/BPanel.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPictureBox.java: ./org.kbinani/BPictureBox.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPopupMenu.java: ./org.kbinani/BPopupMenu.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPreviewKeyDownEventArgs.java: ./org.kbinani/BPreviewKeyDownEventArgs.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BPreviewKeyDownEventHandler.java: ./org.kbinani/BPreviewKeyDownEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BProgressBar.java: ./org.kbinani/BProgressBar.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BRadioButton.java: ./org.kbinani/BRadioButton.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BScrollBar.java: ./org.kbinani/BScrollBar.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BSlider.java: ./org.kbinani/BSlider.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BSplitPane.java: ./org.kbinani/BSplitPane.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BStatusLabel.java: ./org.kbinani/BStatusLabel.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BTextArea.java: ./org.kbinani/BTextArea.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BTextBox.java: ./org.kbinani/BTextBox.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BTimer.java: ./org.kbinani/BTimer.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BToggleButton.java: ./org.kbinani/BToggleButton.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BToolBar.java: ./org.kbinani/BToolBar.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BToolStripButton.java: ./org.kbinani/BToolStripButton.java
	$(CP) $< $@
./build/java/org/kbinani/windows/forms/BVScrollBar.java: ./org.kbinani/BVScrollBar.java
	$(CP) $< $@
./build/java/org/kbinani/xml/XmlMember.java: ./org.kbinani/XmlMember.java
	$(CP) $< $@
./build/java/org/kbinani/xml/XmlSerializable.java: ./org.kbinani/XmlSerializable.java
	$(CP) $< $@
./build/java/org/kbinani/xml/XmlSerializer.java: ./org.kbinani/XmlSerializer.java
	$(CP) $< $@
./build/java/org/kbinani/media/BSoundPlayer.java: ./org.kbinani.media/BSoundPlayer.java
	$(CP) $< $@
./build/java/org/kbinani/media/IWaveReceiver.java: ./org.kbinani.media/IWaveReceiver.java
	$(CP) $< $@
./build/java/org/kbinani/media/MidiInDevice.java: ./org.kbinani.media/MidiInDevice.java
	$(CP) $< $@
./build/java/org/kbinani/media/Wave.java: ./org.kbinani.media/Wave.java
	$(CP) $< $@
./build/java/org/kbinani/media/WaveRateConvertAdapter.java: ./org.kbinani.media/WaveRateConvertAdapter.java
	$(CP) $< $@
./build/java/org/kbinani/media/WaveRateConverter.java: ./org.kbinani.media/WaveRateConverter.java
	$(CP) $< $@
./build/java/org/kbinani/media/WaveReader.java: ./org.kbinani.media/WaveReader.java
	$(CP) $< $@
./build/java/org/kbinani/media/WaveWriter.java: ./org.kbinani.media/WaveWriter.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/BPPair.java: ./org.kbinani.vsq/BPPair.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/DynamicsMode.java: ./org.kbinani.vsq/DynamicsMode.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/ExpressionConfigSys.java: ./org.kbinani.vsq/ExpressionConfigSys.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/IconDynamicsHandle.java: ./org.kbinani.vsq/IconDynamicsHandle.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/IconHandle.java: ./org.kbinani.vsq/IconHandle.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/IconParameter.java: ./org.kbinani.vsq/IconParameter.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/IndexIteratorKind.java: ./org.kbinani.vsq/IndexIteratorKind.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/ITextWriter.java: ./org.kbinani.vsq/ITextWriter.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/Lyric.java: ./org.kbinani.vsq/Lyric.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/LyricHandle.java: ./org.kbinani.vsq/LyricHandle.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/MidiEvent.java: ./org.kbinani.vsq/MidiEvent.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/MidiFile.java: ./org.kbinani.vsq/MidiFile.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/NoteHeadHandle.java: ./org.kbinani.vsq/NoteHeadHandle.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/NRPN.java: ./org.kbinani.vsq/NRPN.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/NrpnData.java: ./org.kbinani.vsq/NrpnData.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/NrpnIterator.java: ./org.kbinani.vsq/NrpnIterator.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/PlayMode.java: ./org.kbinani.vsq/PlayMode.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/SingerConfig.java: ./org.kbinani.vsq/SingerConfig.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/SingerConfigSys.java: ./org.kbinani.vsq/SingerConfigSys.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/SymbolTable.java: ./org.kbinani.vsq/SymbolTable.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/SynthesizerType.java: ./org.kbinani.vsq/SynthesizerType.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/TempoTableEntry.java: ./org.kbinani.vsq/TempoTableEntry.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/TempoVector.java: ./org.kbinani.vsq/TempoVector.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/TextStream.java: ./org.kbinani.vsq/TextStream.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/Timesig.java: ./org.kbinani.vsq/Timesig.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/TimeSigTableEntry.java: ./org.kbinani.vsq/TimeSigTableEntry.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/TransCodeUtil.java: ./org.kbinani.vsq/TransCodeUtil.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstEnvelope.java: ./org.kbinani.vsq/UstEnvelope.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstEvent.java: ./org.kbinani.vsq/UstEvent.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstFile.java: ./org.kbinani.vsq/UstFile.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstPortamento.java: ./org.kbinani.vsq/UstPortamento.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstPortamentoPoint.java: ./org.kbinani.vsq/UstPortamentoPoint.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstPortamentoType.java: ./org.kbinani.vsq/UstPortamentoType.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstTrack.java: ./org.kbinani.vsq/UstTrack.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/UstVibrato.java: ./org.kbinani.vsq/UstVibrato.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VibratoBPList.java: ./org.kbinani.vsq/VibratoBPList.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VibratoBPPair.java: ./org.kbinani.vsq/VibratoBPPair.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VibratoHandle.java: ./org.kbinani.vsq/VibratoHandle.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VocaloSysUtil.java: ./org.kbinani.vsq/VocaloSysUtil.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqBarLineType.java: ./org.kbinani.vsq/VsqBarLineType.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqBPList.java: ./org.kbinani.vsq/VsqBPList.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqBPPair.java: ./org.kbinani.vsq/VsqBPPair.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqBPPairSearchContext.java: ./org.kbinani.vsq/VsqBPPairSearchContext.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqCommand.java: ./org.kbinani.vsq/VsqCommand.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqCommandType.java: ./org.kbinani.vsq/VsqCommandType.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqCommon.java: ./org.kbinani.vsq/VsqCommon.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqEvent.java: ./org.kbinani.vsq/VsqEvent.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqEventList.java: ./org.kbinani.vsq/VsqEventList.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqFile.java: ./org.kbinani.vsq/VsqFile.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqHandle.java: ./org.kbinani.vsq/VsqHandle.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqHandleType.java: ./org.kbinani.vsq/VsqHandleType.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqID.java: ./org.kbinani.vsq/VsqID.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqIDType.java: ./org.kbinani.vsq/VsqIDType.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqMaster.java: ./org.kbinani.vsq/VsqMaster.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqMetaText.java: ./org.kbinani.vsq/VsqMetaText.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqMixer.java: ./org.kbinani.vsq/VsqMixer.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqMixerEntry.java: ./org.kbinani.vsq/VsqMixerEntry.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqNote.java: ./org.kbinani.vsq/VsqNote.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqNrpn.java: ./org.kbinani.vsq/VsqNrpn.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqPhoneticSymbol.java: ./org.kbinani.vsq/VsqPhoneticSymbol.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqTrack.java: ./org.kbinani.vsq/VsqTrack.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/VsqVoiceLanguage.java: ./org.kbinani.vsq/VsqVoiceLanguage.java
	$(CP) $< $@
./build/java/org/kbinani/vsq/WrappedStreamWriter.java: ./org.kbinani.vsq/WrappedStreamWriter.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AmplifyCoefficient.java: ./Cadencii/AmplifyCoefficient.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AppManager.java: ./Cadencii/AppManager.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AquesToneDriver.java: ./Cadencii/AquesToneDriver.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AquesToneRenderingRunner.java: ./Cadencii/AquesToneRenderingRunner.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AttachedCurve.java: ./Cadencii/AttachedCurve.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AttackVariation.java: ./Cadencii/AttackVariation.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AttackVariationConverter.java: ./Cadencii/AttackVariationConverter.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AutoVibratoMinLengthEnum.java: ./Cadencii/AutoVibratoMinLengthEnum.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/AutoVibratoMinLengthUtil.java: ./Cadencii/AutoVibratoMinLengthUtil.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BAssemblyInfo.java: ./Cadencii/BAssemblyInfo.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BezierChain.java: ./Cadencii/BezierChain.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BezierControlType.java: ./Cadencii/BezierControlType.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BezierCurves.java: ./Cadencii/BezierCurves.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BezierPickedSide.java: ./Cadencii/BezierPickedSide.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BezierPoint.java: ./Cadencii/BezierPoint.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BgmFile.java: ./Cadencii/BgmFile.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/BooleanEnum.java: ./Cadencii/BooleanEnum.java
	$(CP) $< $@
./build/java/Cadencii.java: ./Cadencii/Cadencii.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/CadenciiCommand.java: ./Cadencii/CadenciiCommand.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/CadenciiCommandType.java: ./Cadencii/CadenciiCommandType.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ClipboardEntry.java: ./Cadencii/ClipboardEntry.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ClockResolution.java: ./Cadencii/ClockResolution.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ClockResolutionUtility.java: ./Cadencii/ClockResolutionUtility.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/CommandTree.java: ./Cadencii/CommandTree.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/CommandTreeUnit.java: ./Cadencii/CommandTreeUnit.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/CurveType.java: ./Cadencii/CurveType.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/DefaultVibratoLengthEnum.java: ./Cadencii/DefaultVibratoLengthEnum.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/DefaultVibratoLengthUtil.java: ./Cadencii/DefaultVibratoLengthUtil.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/DrawObject.java: ./Cadencii/DrawObject.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/DrawObjectType.java: ./Cadencii/DrawObjectType.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/DynaffComparisonContext.java: ./Cadencii/DynaffComparisonContext.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditedZone.java: ./Cadencii/EditedZone.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditedZoneCommand.java: ./Cadencii/EditedZoneCommand.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditedZoneUnit.java: ./Cadencii/EditedZoneUnit.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditMode.java: ./Cadencii/EditMode.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditorConfig.java: ./Cadencii/EditorConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditorStatus.java: ./Cadencii/EditorStatus.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EditTool.java: ./Cadencii/EditTool.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/EmptyRenderingRunner.java: ./Cadencii/EmptyRenderingRunner.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FederChangedEventHandler.java: ./Cadencii/FederChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormAskKeySoundGeneration.java: ./Cadencii/FormAskKeySoundGeneration.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormBeatConfig.java: ./Cadencii/FormBeatConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormBezierPointEdit.java: ./Cadencii/FormBezierPointEdit.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormCompileResult.java: ./Cadencii/FormCompileResult.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormCurvePointEdit.java: ./Cadencii/FormCurvePointEdit.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormDeleteBar.java: ./Cadencii/FormDeleteBar.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormExportMusicXml.java: ./Cadencii/FormExportMusicXml.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormGameControlerConfig.java: ./Cadencii/FormGameControlerConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormGenerateKeySound.java: ./Cadencii/FormGenerateKeySound.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormIconPalette.java: ./Cadencii/FormIconPalette.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormImportLyric.java: ./Cadencii/FormImportLyric.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormInsertBar.java: ./Cadencii/FormInsertBar.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormMain.java: ./Cadencii/FormMain.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormMidiConfig.java: ./Cadencii/FormMidiConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormMidiImExport.java: ./Cadencii/FormMidiImExport.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormMidiImExportConfig.java: ./Cadencii/FormMidiImExportConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormMixer.java: ./Cadencii/FormMixer.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormNoteExpressionConfig.java: ./Cadencii/FormNoteExpressionConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormNoteProperty.java: ./Cadencii/FormNoteProperty.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormPluginUi.java: ./Cadencii/FormPluginUi.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormRandomize.java: ./Cadencii/FormRandomize.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormRealtimeConfig.java: ./Cadencii/FormRealtimeConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormShortcutKeys.java: ./Cadencii/FormShortcutKeys.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormSingerStyleConfig.java: ./Cadencii/FormSingerStyleConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormSplash.java: ./Cadencii/FormSplash.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormSynthesize.java: ./Cadencii/FormSynthesize.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormTempoConfig.java: ./Cadencii/FormTempoConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormTrackProperty.java: ./Cadencii/FormTrackProperty.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormVibratoConfig.java: ./Cadencii/FormVibratoConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/FormWordDictionary.java: ./Cadencii/FormWordDictionary.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/HScroll.java: ./Cadencii/HScroll.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ICommand.java: ./Cadencii/ICommand.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ICommandRunnable.java: ./Cadencii/ICommandRunnable.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/IComparisonContext.java: ./Cadencii/IComparisonContext.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/IPaletteTool.java: ./Cadencii/IPaletteTool.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/KanaDeRomanization.java: ./Cadencii/KanaDeRomanization.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/KeySoundPlayer.java: ./Cadencii/KeySoundPlayer.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/MemoryManager.java: ./Cadencii/MemoryManager.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/MidiDeviceImp.java: ./Cadencii/MidiDeviceImp.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/MidiPlayer.java: ./Cadencii/MidiPlayer.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/MidiPortConfig.java: ./Cadencii/MidiPortConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/MidiQueue.java: ./Cadencii/MidiQueue.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/MuteChangedEventHandler.java: ./Cadencii/MuteChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/NoteNumberExpressionType.java: ./Cadencii/NoteNumberExpressionType.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/NoteNumberProperty.java: ./Cadencii/NoteNumberProperty.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/NoteNumberPropertyConverter.java: ./Cadencii/NoteNumberPropertyConverter.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/NumberTextBox.java: ./Cadencii/NumberTextBox.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/NumericUpDownEx.java: ./Cadencii/NumericUpDownEx.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/OtoArgs.java: ./Cadencii/OtoArgs.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PaletteToolConfig.java: ./Cadencii/PaletteToolConfig.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PaletteToolServer.java: ./Cadencii/PaletteToolServer.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PanelState.java: ./Cadencii/PanelState.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PanpotChangedEventHandler.java: ./Cadencii/PanpotChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PencilMode.java: ./Cadencii/PencilMode.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PencilModeEnum.java: ./Cadencii/PencilModeEnum.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PictPianoRoll.java: ./Cadencii/PictPianoRoll.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PlatformEnum.java: ./Cadencii/PlatformEnum.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PlayPositionSpecifier.java: ./Cadencii/PlayPositionSpecifier.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PlaySound.java: ./Cadencii/PlaySound.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PointD.java: ./Cadencii/PointD.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/Preference.java: ./Cadencii/Preference.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PropertyPanel.java: ./Cadencii/PropertyPanel.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PropertyPanelContainer.java: ./Cadencii/PropertyPanelContainer.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/PropertyPanelState.java: ./Cadencii/PropertyPanelState.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/QuantizeMode.java: ./Cadencii/QuantizeMode.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/QuantizeModeUtil.java: ./Cadencii/QuantizeModeUtil.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/Range.java: ./Cadencii/Range.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/RenderedStatus.java: ./Cadencii/RenderedStatus.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/RendererKind.java: ./Cadencii/RendererKind.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/RenderingRunner.java: ./Cadencii/RenderingRunner.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/RenderQueue.java: ./Cadencii/RenderQueue.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/RenderRequiredEventHandler.java: ./Cadencii/RenderRequiredEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/Resources.java: ./Cadencii/Resources.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/RgbColor.java: ./Cadencii/RgbColor.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SandBox.java: ./Cadencii/SandBox.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ScreenStatus.java: ./Cadencii/ScreenStatus.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ScriptInvoker.java: ./Cadencii/ScriptInvoker.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ScriptReturnStatus.java: ./Cadencii/ScriptReturnStatus.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ScriptServer.java: ./Cadencii/ScriptServer.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedBezierPoint.java: ./Cadencii/SelectedBezierPoint.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedCurveChangedEventHandler.java: ./Cadencii/SelectedCurveChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedEventChangedEventHandler.java: ./Cadencii/SelectedEventChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedEventEntry.java: ./Cadencii/SelectedEventEntry.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedEventEntryPropertyDescriptor.java: ./Cadencii/SelectedEventEntryPropertyDescriptor.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedEventEntryTypeConverter.java: ./Cadencii/SelectedEventEntryTypeConverter.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedRegion.java: ./Cadencii/SelectedRegion.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedTempoEntry.java: ./Cadencii/SelectedTempoEntry.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedTimesigEntry.java: ./Cadencii/SelectedTimesigEntry.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SelectedTrackChangedEventHandler.java: ./Cadencii/SelectedTrackChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SingerEventComparisonContext.java: ./Cadencii/SingerEventComparisonContext.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SoloChangedEventHandler.java: ./Cadencii/SoloChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/StateChangeRequiredEventHandler.java: ./Cadencii/StateChangeRequiredEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/StraightRenderingQueue.java: ./Cadencii/StraightRenderingQueue.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/StraightRenderingRunner.java: ./Cadencii/StraightRenderingRunner.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/SymmetricMatrix.java: ./Cadencii/SymmetricMatrix.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/TagLyricTextBox.java: ./Cadencii/TagLyricTextBox.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/TextBoxEx.java: ./Cadencii/TextBoxEx.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ToolStripLocation.java: ./Cadencii/ToolStripLocation.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/TopMostChangedEventHandler.java: ./Cadencii/TopMostChangedEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/TrackSelector.java: ./Cadencii/TrackSelector.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/UpdateProgressEventHandler.java: ./Cadencii/UpdateProgressEventHandler.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/UtauFreq.java: ./Cadencii/UtauFreq.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/UtauRenderingRunner.java: ./Cadencii/UtauRenderingRunner.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/UtauVoiceDB.java: ./Cadencii/UtauVoiceDB.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/Utility.java: ./Cadencii/Utility.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/ValuePairOfStringArrayOfKeys.java: ./Cadencii/ValuePairOfStringArrayOfKeys.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VersionInfo.java: ./Cadencii/VersionInfo.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VibratoLengthEditingRule.java: ./Cadencii/VibratoLengthEditingRule.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VibratoPoint.java: ./Cadencii/VibratoPoint.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VibratoPointIterator.java: ./Cadencii/VibratoPointIterator.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VibratoVariation.java: ./Cadencii/VibratoVariation.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VibratoVariationConverter.java: ./Cadencii/VibratoVariationConverter.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VocaloidDriver.java: ./Cadencii/VocaloidDriver.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VocaloidRenderingRunner.java: ./Cadencii/VocaloidRenderingRunner.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VolumeTracker.java: ./Cadencii/VolumeTracker.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VsqBPListComparisonContext.java: ./Cadencii/VsqBPListComparisonContext.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VsqFileEx.java: ./Cadencii/VsqFileEx.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/vstidrv.java: ./Cadencii/vstidrv.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VSTiProxy.java: ./Cadencii/VSTiProxy.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/VstSdk.java: ./Cadencii/VstSdk.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/WaveDrawContext.java: ./Cadencii/WaveDrawContext.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/WaveReceiver.java: ./Cadencii/WaveReceiver.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/WaveView.java: ./Cadencii/WaveView.java
	$(CP) $< $@
./build/java/org/kbinani/cadencii/winmmhelp.java: ./Cadencii/winmmhelp.java
	$(CP) $< $@
