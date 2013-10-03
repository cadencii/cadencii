/*
 * VsqxWriter.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace cadencii.vsq.io
{
    public static class XmlAttributeUtil
    {
        public static XmlElement Attribute(this XmlElement element, string key, string value)
        {
            var attr = element.OwnerDocument.CreateAttribute(key);
            attr.Value = value;
            element.Attributes.Append(attr);
            return element;
        }
    }

    /// <summary>
    /// Vsqx ファイルへのエクスポートを行うクラス。
    /// </summary>
    public class VsqxWriter : ISequenceWriter
    {
        public class ExportSettings
        {
            public int pmBendDepth = 8;
            public int pmBendLength = 0;
            public int pmbPortamentoUse = 0;
            public int demDecGainRate = 50;
            public int demAccent = 50;
            public int opening = 127;
        }

        private static readonly string MIXER_UNIT_MASTER = "masterUnit";
        private static readonly string MIXER_UNIT_VS = "vsUnit";
        private static readonly string MIXER_UNIT_SE = "seUnit";
        private static readonly string MIXER_UNIT_KARAOKE = "karaokeUnit";
        private static readonly string MIXER_NODE_INGAIN = "inGain";
        private static readonly string MIXER_NODE_SENDLEVEL = "sendLevel";
        private static readonly string MIXER_NODE_SENDENABLE = "sendEnable";
        private static readonly string MIXER_NODE_VSTRCKNO = "vsTrackNo";
        private static readonly string MIXER_NODE_MUTE = "mute";
        private static readonly string MIXER_NODE_SOLO = "solo";
        private static readonly string MIXER_NODE_PAN = "pan";
        private static readonly string MIXER_NODE_VOL = "vol";

        private XmlDocument doc_;
        private ExportSettings settings_ = new ExportSettings();

        /// <summary>
        /// エクスポートを行う。
        /// </summary>
        /// <param name="path">出力先のファイルパス</param>
        /// <param name="sequence">出力するシーケンス</param>
        public void write(VsqFile sequence, string path)
        {
            doc_ = new XmlDocument();
            const string xsi_url = "http://www.w3.org/2001/XMLSchema-instance";
            var root = doc_.CreateElement("vsq3");
            {
                var xmlns = doc_.CreateAttribute("xmlns");
                xmlns.Value = "http://www.yamaha.co.jp/vocaloid/schema/vsq3/";
                root.Attributes.Append(xmlns);
            }
            {
                var xsi = doc_.CreateAttribute("xmlns:xsi");
                xsi.Value = xsi_url;
                root.Attributes.Append(xsi);
            }
            {
                var schema = doc_.CreateAttribute("xsi", "schemaLocation", xsi_url);
                schema.Value = "http://www.yamaha.co.jp/vocaloid/schema/vsq3/ vsq3.xsd";
                root.Attributes.Append(schema);
            }
            root.AppendChild(createNode("vender", "Yamaha corporation"));
            root.AppendChild(createNode("version", "3.0.0.11"));
            root.AppendChild(createVoiceTableNode(sequence));
            root.AppendChild(createMixerNode(sequence.Mixer));
            root.AppendChild(createMasterTrackNode(sequence, sequence.Track[0]));

            for (int i = 1; i < sequence.Track.Count; ++i) {
                root.AppendChild(createTrackNode(sequence.Track[i],
                                                 i - 1,
                                                 sequence.getPreMeasureClocks(),
                                                 sequence.TotalClocks));
            }

            root.AppendChild(doc_.CreateElement("seTrack"));
            root.AppendChild(doc_.CreateElement("karaokeTrack"));
            
            doc_.AppendChild(root);
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                var writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                doc_.Save(writer);
            }
        }

        public void setSettings(ExportSettings settings)
        {
            settings_ = settings;
        }

        private XmlElement createNode<T>(string name, T value)
        {
            XmlElement result = doc_.CreateElement(name);
            if (typeof(T) == typeof(string)) {
                result.InnerXml = doc_.CreateCDataSection(value.ToString()).OuterXml;
            } else {
                result.InnerText = value.ToString();
            }
            return result;
        }

        #region <vVoiceTable>

        private List<IconHandle> collectAllSingerInfo(VsqFile sequence)
        {
            return
                sequence.Track
                    .Where((track) => track.MetaText != null)
                    .SelectMany((track) => track.MetaText.Events.Events)
                    .Where((vsq_event) => vsq_event.ID.type == VsqIDType.Singer)
                    .Select((vsq_event) => vsq_event.ID.IconHandle)
                    .Distinct()
                    .OrderBy((handle) => handle.Language * 255 + handle.Program)
                    .ToList();
        }

        private XmlElement createVoiceParamNode(IconHandle handle)
        {
            var result = doc_.CreateElement("vVoiceParam");
            result.AppendChild(createNode("bre", 0));
            result.AppendChild(createNode("bri", 0));
            result.AppendChild(createNode("cle", 0));
            result.AppendChild(createNode("gen", 0));
            result.AppendChild(createNode("ope", 0));
            return result;
        }

        private XmlElement createVoiceTableNode(VsqFile sequence)
        {
            var result = doc_.CreateElement("vVoiceTable");
            var all_assigned_singers = collectAllSingerInfo(sequence);
            all_assigned_singers.ForEach((handle) => {
                var node = doc_.CreateElement("vVoice");
                node.AppendChild(createNode("vBS", handle.Language));
                node.AppendChild(createNode("vPC", handle.Program));
                node.AppendChild(createNode("compID", "AAAAAAAAAAAAAAAA"));
                node.AppendChild(createNode("vVoiceName", handle.Caption));
                node.AppendChild(createVoiceParamNode(handle));
                result.AppendChild(node);
            });
            return result;
        }

        #endregion

        #region <mixer>

        private XmlElement createMixerUnitNode(VsqMixerEntry entry, int index)
        {
            var result = doc_.CreateElement(MIXER_UNIT_VS);
            result.AppendChild(createNode(MIXER_NODE_VSTRCKNO, index));
            result.AppendChild(createNode(MIXER_NODE_INGAIN, 0));
            result.AppendChild(createNode(MIXER_NODE_SENDLEVEL, VsqMixer.FEDER_MIN));
            result.AppendChild(createNode(MIXER_NODE_SENDENABLE, 0));
            result.AppendChild(createNode(MIXER_NODE_MUTE, entry.Mute));
            result.AppendChild(createNode(MIXER_NODE_SOLO, entry.Solo));
            result.AppendChild(createNode(MIXER_NODE_PAN, entry.Panpot + 64));
            result.AppendChild(createNode(MIXER_NODE_VOL, entry.Feder));
            return result;
        }

        private XmlElement createMixerSoundEffectUnitNode()
        {
            var result = doc_.CreateElement(MIXER_UNIT_SE);
            result.AppendChild(createNode(MIXER_NODE_INGAIN, 0));
            result.AppendChild(createNode(MIXER_NODE_SENDLEVEL, VsqMixer.FEDER_MIN));
            result.AppendChild(createNode(MIXER_NODE_SENDENABLE, 0));
            result.AppendChild(createNode(MIXER_NODE_MUTE, 0));
            result.AppendChild(createNode(MIXER_NODE_SOLO, 0));
            result.AppendChild(createNode(MIXER_NODE_PAN, 64));
            result.AppendChild(createNode(MIXER_NODE_VOL, 0));
            return result;
        }

        private XmlElement createMixerKaraokeUnitNode()
        {
            var result = doc_.CreateElement(MIXER_UNIT_KARAOKE);
            result.AppendChild(createNode(MIXER_NODE_INGAIN, 0));
            result.AppendChild(createNode(MIXER_NODE_MUTE, 0));
            result.AppendChild(createNode(MIXER_NODE_SOLO, 0));
            result.AppendChild(createNode(MIXER_NODE_VOL, -129));
            return result;
        }

        private XmlElement createMixerNode(VsqMixer mixer)
        {
            var result = doc_.CreateElement("mixer", string.Empty);

            var masterUnit = doc_.CreateElement(MIXER_UNIT_MASTER);
            masterUnit.AppendChild(createNode("outDev", 0));
            masterUnit.AppendChild(createNode("retLevel", 0));
            masterUnit.AppendChild(createNode(MIXER_NODE_VOL, mixer.MasterFeder));
            result.AppendChild(masterUnit);

            for (int index = 0; index < mixer.Slave.Count; index++) {
                var entry = mixer.Slave[index];
                var vsUnit = createMixerUnitNode(entry, index);
                result.AppendChild(vsUnit);
            }

            result.AppendChild(createMixerSoundEffectUnitNode());
            result.AppendChild(createMixerKaraokeUnitNode());

            return result;
        }

        #endregion

        #region <masterTrack>

        private XmlElement createMasterTrackNode(VsqFile sequence, VsqTrack master_track)
        {
            var result = doc_.CreateElement("masterTrack");
            result.AppendChild(createNode("seqName", master_track.getName()));
            result.AppendChild(createNode("comment", ""));
            result.AppendChild(createNode("resolution", sequence.getTickPerQuarter()));
            result.AppendChild(createNode("preMeasure", sequence.getPreMeasure()));
            sequence.TimesigTable.ForEach((time_sig) => {
                var node = doc_.CreateElement("timeSig");
                node.AppendChild(createNode("posMes", time_sig.BarCount));
                node.AppendChild(createNode("nume", time_sig.Numerator));
                node.AppendChild(createNode("denomi", time_sig.Denominator));
                result.AppendChild(node);
            });
            sequence.TempoTable.ForEach((tempo) => {
                var node = doc_.CreateElement("tempo");
                node.AppendChild(createNode("posTick", tempo.Clock));
                node.AppendChild(createNode("bpm", (int)(60e6 / tempo.Tempo * 100)));
                result.AppendChild(node);
            });
            return result;
        }

        #endregion

        #region <vsTrack>

        private XmlElement createVibratoCurveNode(int start_value, VibratoBPList curve)
        {
            var result = doc_.CreateElement("seqAttr");
            Func<VibratoBPPair, XmlElement> CreateElem = (pair) => {
                var elem = doc_.CreateElement("elem");
                int x = (int)(pair.X * 65536);
                elem.AppendChild(createNode("posNrm", x));
                elem.AppendChild(createNode("elv", pair.Y));
                return elem;
            };
            result.AppendChild(CreateElem(new VibratoBPPair(0.0f, start_value)));
            int count = curve.getCount();
            for (int i = 0; i < count; ++i) {
                var pair = curve.getElement(i);
                var elem = CreateElem(curve.getElement(i));
                result.AppendChild(elem);
            }
            return result;
        }

        private XmlElement createNoteNode(VsqEvent vsq_event, int pre_measure_clock)
        {
            var node = doc_.CreateElement("note");
            node.AppendChild(createNode("posTick", vsq_event.Clock - pre_measure_clock));
            node.AppendChild(createNode("durTick", vsq_event.ID.getLength()));
            node.AppendChild(createNode("noteNum", vsq_event.ID.Note));
            node.AppendChild(createNode("velocity", vsq_event.ID.Dynamics));
            node.AppendChild(createNode("lyric", vsq_event.ID.LyricHandle.getLyricAt(0).Phrase));
            node.AppendChild(createNode("phnms", vsq_event.ID.LyricHandle.getLyricAt(0).getPhoneticSymbol()));
            {
                var style = doc_.CreateElement("noteStyle");
                style.AppendChild(createNode("attr", vsq_event.ID.DEMaccent).Attribute("id", "accent"));
                style.AppendChild(createNode("attr", vsq_event.ID.PMBendDepth).Attribute("id", "bendDep"));
                style.AppendChild(createNode("attr", vsq_event.ID.PMBendLength).Attribute("id", "bendLen"));
                style.AppendChild(createNode("attr", vsq_event.ID.DEMdecGainRate).Attribute("id", "decay"));
                style.AppendChild(createNode("attr", vsq_event.ID.isFallPortamento() ? 1 : 0).Attribute("id", "fallPort"));
                style.AppendChild(createNode("attr", 127).Attribute("id", "opening"));
                style.AppendChild(createNode("attr", vsq_event.ID.isRisePortamento() ? 1 : 0).Attribute("id", "risePort"));
                if (vsq_event.ID.VibratoHandle != null && vsq_event.ID.VibratoDelay < vsq_event.ID.getLength()) {
                    int vibrato_length = vsq_event.ID.getLength() - vsq_event.ID.VibratoDelay;
                    int percent = (int)(vibrato_length * 100.0 / vsq_event.ID.getLength());
                    style.AppendChild(createNode("attr", percent).Attribute("id", "vibLen"));
                    style.AppendChild(createNode("attr", vsq_event.ID.VibratoHandle.Index).Attribute("id", "vibType"));
                    style.AppendChild(createVibratoCurveNode(vsq_event.ID.VibratoHandle.getStartDepth(), vsq_event.ID.VibratoHandle.getDepthBP()).Attribute("id", "vibDep"));
                    style.AppendChild(createVibratoCurveNode(vsq_event.ID.VibratoHandle.getStartRate(), vsq_event.ID.VibratoHandle.getRateBP()).Attribute("id", "vibRate"));
                }
                node.AppendChild(style);
            }
            return node;
        }

        private XmlElement createPartStyleNode()
        {
            var partStyle = doc_.CreateElement("partStyle");
            partStyle.AppendChild(createNode("attr", settings_.demAccent).Attribute("id", "accent"));
            partStyle.AppendChild(createNode("attr", settings_.pmBendDepth).Attribute("id", "bendDep"));
            partStyle.AppendChild(createNode("attr", settings_.pmBendLength).Attribute("id", "bendLen"));
            partStyle.AppendChild(createNode("attr", settings_.demDecGainRate).Attribute("id", "decay"));
            bool risePort = (settings_.pmbPortamentoUse & 1) == 1;
            bool fallPort = (settings_.pmbPortamentoUse & 2) == 2;
            partStyle.AppendChild(createNode("attr", fallPort ? 1 : 0).Attribute("id", "fallPort"));
            partStyle.AppendChild(createNode("attr", settings_.opening).Attribute("id", "opening"));
            partStyle.AppendChild(createNode("attr", risePort ? 1 : 0).Attribute("id", "risePort"));
            return partStyle;
        }

        private XmlElement createMusicalPartSingerNode(VsqEvent @event, int pre_measure_clock)
        {
            var node = doc_.CreateElement("singer");
            node.AppendChild(createNode("posTick", @event.Clock - pre_measure_clock));
            node.AppendChild(createNode("vBS", @event.ID.IconHandle.Language));
            node.AppendChild(createNode("vPC", @event.ID.IconHandle.Program));
            return node;
        }

        private XmlElement createMusicalPartNode(VsqTrack track, int pre_measure_clock, int sequence_length)
        {
            var result = doc_.CreateElement("musicalPart");
            result.AppendChild(createNode("posTick", pre_measure_clock));
            result.AppendChild(createNode("playTime", sequence_length - pre_measure_clock));
            result.AppendChild(createNode("partName", track.getName()));
            result.AppendChild(createNode("comment", ""));
            {
                var stylePlugin = doc_.CreateElement("stylePlugin");
                stylePlugin.AppendChild(createNode("stylePluginID", "ACA9C502-A04B-42b5-B2EB-5CEA36D16FCE"));
                stylePlugin.AppendChild(createNode("stylePluginName", "VOCALOID2 Compatible Style"));
                stylePlugin.AppendChild(createNode("version", "3.0.0.1"));
                result.AppendChild(stylePlugin);
            }
            {
                result.AppendChild(createPartStyleNode());
            }
            
            // Set first singer 
            var first_singer = (VsqEvent)track.getSingerEventAt(pre_measure_clock).Clone();
            first_singer.Clock = pre_measure_clock;
            result.AppendChild(createMusicalPartSingerNode(first_singer, pre_measure_clock));
            
            track.MetaText.Events.Events
                .Where((@event) => (@event.ID.type == VsqIDType.Singer ? @event.Clock > pre_measure_clock : @event.Clock >= pre_measure_clock))
                .ToList()
                .ForEach((@event) => {
                    if (@event.ID.type == VsqIDType.Singer) {
                        var node = createMusicalPartSingerNode(@event, pre_measure_clock);
                        result.AppendChild(node);
                    } else if (@event.ID.type == VsqIDType.Anote) {
                        result.AppendChild(createNoteNode(@event, pre_measure_clock));
                    }
                });
            return result;
        }

        private XmlElement createTrackNode(VsqTrack track, int index, int pre_measure_clock, int sequence_length)
        {
            var result = doc_.CreateElement("vsTrack");
            result.AppendChild(createNode("vsTrackNo", index));
            result.AppendChild(createNode("trackName", track.getName()));
            result.AppendChild(createNode("comment", ""));
            result.AppendChild(createMusicalPartNode(track, pre_measure_clock, sequence_length));
            return result;
        }

        #endregion

    }
}
