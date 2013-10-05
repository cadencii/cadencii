/*
 * Program.cs
 * Copyright © 2008-2010 kbinani
 *
 * This file is part of cadencii.generatekeysound.
 *
 * cadencii.generatekeysound is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.generatekeysound is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using cadencii;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;



namespace cadencii.generatekeysound
{

    class Program
    {
        #region static members
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppManager.init();
            string singer = "Miku";
            object locker = new object();
            double amp = 1.0;
            string dir = Path.Combine(Application.StartupPath, "cache");
            bool replace = true;
            int search = -1;
            int arguments = 0;
            while (search + 1 < args.Length) {
                search++;
                switch (args[search].ToLower()) {
                    case "-help":
                    case "-h":
                    case "/help":
                    case "/h":
                    case "-?":
                    case "/?":
                    case "--h":
                    case "--help":
                    arguments++;
                    ShowHelp();
                    return;
                    case "-amplify":
                    case "-a":
                    case "/amplify":
                    case "/a":
                    if (search + 1 < args.Length) {
                        double t_amp = amp;
                        if (double.TryParse(args[search + 1], out t_amp)) {
                            if (t_amp < 0.0) {
                                Console.WriteLine("error; amilify coefficient must be >= 0. specified value was \"" + t_amp + "\"");
                                return;
                            }
                            amp = t_amp;
                        } else {
                            InvalidNumberExpressionAt(args[search + 1]);
                            return;
                        }
                    } else {
                        TooFewArgumentFor(args[search]);
                        return;
                    }
                    arguments++;
                    search++;
                    break;
                    case "-singer":
                    case "-s":
                    case "/singer":
                    case "/s":
                    if (search + 1 < args.Length) {
                        singer = args[search + 1];
                    } else {
                        TooFewArgumentFor(args[search]);
                        return;
                    }
                    arguments++;
                    search++;
                    break;
                    case "-dir":
                    case "-d":
                    case "/dir":
                    case "/d":
                    if (search + 1 < args.Length) {
                        dir = args[search + 1];
                    } else {
                        TooFewArgumentFor(args[search]);
                        return;
                    }
                    arguments++;
                    search++;
                    break;
                    case "-replace":
                    case "-r":
                    case "/replace":
                    case "/r":
                    replace = true;
                    arguments++;
                    break;
                    default:
                    Console.WriteLine("error; unknown option \"" + args[search] + "\"");
                    return;
                }
            }
            if (arguments == 0) {
                Application.Run(new FormGenerateKeySound(false));
            } else {
                FormGenerateKeySound.PrepareStartArgument arg = new FormGenerateKeySound.PrepareStartArgument();
                arg.singer = singer;
                arg.amplitude = amp;
                arg.directory = dir;
                arg.replace = replace;
                run(arg);
            }
        }

        static void InvalidNumberExpressionAt(string expression)
        {
            Console.WriteLine("error; string parse error. invalid number expression at \"" + expression + "\"");
        }

        static void TooFewArgumentFor(string argument)
        {
            Console.WriteLine("error; too few argument for \"" + argument + "\"");
        }

        static void ShowHelp()
        {
            Console.WriteLine("GenerateKeySound, Copyright (C) 2008-2009, kbinani");
            Console.WriteLine("Usage: GenerateKeySound [options]");
            Console.WriteLine("    -help            Shows this message and return (short: -h, -?)");
            Console.WriteLine("    -amplify AMP     Sets sound amplify coefficients (short: -a)");
            Console.WriteLine("                     AMP must be 0 <= AMP (defualt is 1.0)");
            //Console.WriteLine( "    -pchange NUMBER  Sets the value of Program Change (short: -p)" );
            Console.WriteLine("    -dir DIRECTORY   Specifies the directory of output (short: -d)");
            Console.WriteLine("                     default of DIRECTORY is \"." + System.IO.Path.DirectorySeparatorChar + "cache\"");
            Console.WriteLine("    -replace         Switch to overwrite exisiting WAVs (short: -r)");
            Console.WriteLine("    -singer          Specifies singer (short: -s)");
            Console.WriteLine();
            Console.WriteLine("Options can be of the form -option or /option");
        }

        private static void run(FormGenerateKeySound.PrepareStartArgument arg)
        {
            string singer = arg.singer;
            double amp = arg.amplitude;
            string dir = arg.directory;
            bool replace = arg.replace;
            // 音源を準備
            if (!Directory.Exists(dir)) {
                System.IO.Directory.CreateDirectory(dir);
            }

            for (int i = 0; i < 127; i++) {
                string path = Path.Combine(dir, i + ".wav");
                Console.Write("writing \"" + path + "\" ...");
                if (replace || (!replace && !File.Exists(path))) {
                    try {
                        FormGenerateKeySound.GenerateSinglePhone(i, singer, path, amp);
                        if (File.Exists(path)) {
                            try {
                                Wave wv = new Wave(path);
                                wv.trimSilence();
                                wv.monoralize();
                                wv.write(path);
                            } catch (Exception ex) {
                                serr.println("Program#run; ex=" + ex);
                            }
                        }
                    } catch (Exception ex) {
                        serr.println("Program#run; ex=" + ex);
                    }
                }
                sout.println(" done");
            }
        }

        #endregion
    }

}
