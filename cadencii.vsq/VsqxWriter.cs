/*
 * VsqxWriter.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * org.kbinani.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * org.kbinani.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */

using System.Text;
using System.Xml;

namespace cadencii.vsq
{
    /// <summary>
    /// Vsqx ファイルへのエクスポートを行うクラス。
    /// </summary>
    public class VsqxWriter
    {
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

        /// <summary>
        /// エクスポートを行う。
        /// </summary>
        /// <param name="path">出力先のファイルパス</param>
        /// <param name="sequence">出力するシーケンス</param>
        public void exportAsVsqx(string path, VsqFile sequence)
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
            root.AppendChild(createMixerNode(sequence.Mixer));
            doc_.AppendChild(root);
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                var writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                doc_.Save(writer);
            }
        }

        private XmlElement createNode<T>(string name, T value)
        {
            var result = doc_.CreateElement(name);
            result.InnerText = value.ToString();
            return result;
        }

        private XmlElement createMixerUnitNode(VsqMixerEntry entry, int index)
        {
            var result = doc_.CreateElement(MIXER_UNIT_VS);
            result.AppendChild(createNode(MIXER_NODE_VSTRCKNO, index));
            result.AppendChild(createNode(MIXER_NODE_INGAIN, "0"));
            result.AppendChild(createNode(MIXER_NODE_SENDLEVEL, "-898"));
            result.AppendChild(createNode(MIXER_NODE_SENDENABLE, "0"));
            result.AppendChild(createNode(MIXER_NODE_MUTE, entry.Mute));
            result.AppendChild(createNode(MIXER_NODE_SOLO, entry.Solo));
            result.AppendChild(createNode(MIXER_NODE_PAN, entry.Panpot + 64));
            result.AppendChild(createNode(MIXER_NODE_VOL, entry.Feder));
            return result;
        }

        private XmlElement createMixerSoundEffectUnitNode()
        {
            var result = doc_.CreateElement(MIXER_UNIT_SE);
            result.AppendChild(createNode(MIXER_NODE_INGAIN, "0"));
            result.AppendChild(createNode(MIXER_NODE_SENDLEVEL, "-898"));
            result.AppendChild(createNode(MIXER_NODE_SENDENABLE, "0"));
            result.AppendChild(createNode(MIXER_NODE_MUTE, 0));
            result.AppendChild(createNode(MIXER_NODE_SOLO, 0));
            result.AppendChild(createNode(MIXER_NODE_PAN, 64));
            result.AppendChild(createNode(MIXER_NODE_VOL, 0));
            return result;
        }

        private XmlElement createMixerKaraokeUnitNode()
        {
            var result = doc_.CreateElement(MIXER_UNIT_KARAOKE);
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
    }
}
