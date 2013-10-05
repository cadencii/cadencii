/*
 * Program.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using cadencii;

namespace cadencii.utils.PostBuildCreateVersionInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                System.Environment.Exit(1);
                return;
            }
            var output_file = args[0];

            var info = new cadencii.updater.UpdateInfo();

            var version = new Version(BAssemblyInfo.fileVersion);
            info.Major = version.Major;
            info.Minor = version.Minor;
            info.Build = version.Build;
            info.ReleaseDate = DateTime.UtcNow;
            info.DownloadUrl = BAssemblyInfo.downloadUrl;

            var serializer = new XmlSerializer(typeof(cadencii.updater.UpdateInfo));
            using (var stream = new FileStream(output_file, FileMode.Create, FileAccess.Write)) {
                var writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, info);
            }
        }
    }
}
