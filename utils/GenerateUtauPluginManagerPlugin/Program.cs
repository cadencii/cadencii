using System;
using System.IO;
using System.Windows.Forms;

static class ParseUtauPluginInvoker
{
    private static string Parent(this string file_path)
    {
        return Path.GetDirectoryName(file_path);
    }

    private static string Child(this string directory, string sub_file)
    {
        return Path.Combine(directory, sub_file);
    }

    // 
    public static void Main(string[] args)
    {
        string base_dir = System.Windows.Forms.Application.StartupPath.Parent().Parent().Parent().Parent().Parent().Child("src");
        //Console.WriteLine( "base_dir=" + base_dir );
        string out_file = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(base_dir, "Cadencii"), "bin"), "x86"), "Release"), "script"), "UTAU Plugin Manager.txt");
        //Console.WriteLine( "out_file=" + out_file );
        string in_file1 = Path.Combine(Path.Combine(base_dir, "ScriptImplement"), "Utau Plugin Invoker.cs");
        string in_file2 = Path.Combine(Path.Combine(base_dir, "ScriptImplement"), "UTAU Plugin Manager.cs");
        //Console.WriteLine( "in_file1=" + in_file1 );
        //Console.WriteLine( "in_file2=" + in_file2 );
        if (!File.Exists(in_file1)) {
            Console.WriteLine("error; file not found '" + in_file1 + "'");
            System.Environment.ExitCode = -1;
            return;
        }
        if (!File.Exists(in_file2)) {
            Console.WriteLine("error; file not found '" + in_file2 + "'");
            System.Environment.ExitCode = -1;
            return;
        }

        // base_dir    trunk
        // in_file2    trunk\ScriptImplement\Utau Plugin Invoker.cs
        // in_file1    trunk\ScriptImplement\UTAU Plugin Manager.cs
        // out_file    trunk\Cadencii\bin\x86\Release\UTAU Plugin Manager.txt

        // テンプレートを読み込む
        string template = "";
        StreamReader sr = null;
        try {
            sr = new StreamReader(in_file1);
            string line = "";
            int count = 0;
            //Console.WriteLine( "'" + in_file1 + "'" );
            while ((line = sr.ReadLine()) != null) {
                //Console.WriteLine( line );
                line = line.Replace("\"", "\\\"");
                line = line.Replace("Utau_Plugin_Invoker", "{0}");
                line = line.Replace("E:\\Program Files\\UTAU\\plugins\\TestUtauScript\\plugin.txt", "{1}");
                template += (count == 0 ? "" : "\n        ") + "\"" + line + "\\n\" +";
                count++;
            }
            template += " \"\"";
        } catch (Exception ex) {
            Console.WriteLine(ex.StackTrace);
        } finally {
            if (sr != null) {
                try {
                    sr.Close();
                } catch {
                }
            }
        }

        StreamWriter sw = null;
        sr = null;
        try {
            sw = new StreamWriter(out_file);
            sr = new StreamReader(in_file2);
            string line = "";
            //Console.WriteLine( "'" + in_file2 + "'" );
            while ((line = sr.ReadLine()) != null) {
                //Console.WriteLine( line );
                line = line.Replace("\"@@TEXT@@\"", template);
                sw.WriteLine(line);
            }
        } catch (Exception ex) {
            Console.WriteLine(ex.StackTrace);
        } finally {
            if (sr != null) {
                try {
                    sr.Close();
                } catch {
                }
            }
            if (sw != null) {
                try {
                    sw.Close();
                } catch {
                }
            }
        }

        return;
    }

    private static void printUsage()
    {
        Console.WriteLine("ParseUtauPluginInvoker");
        Console.WriteLine("Copyright (C) 2011 kbinani");
        Console.WriteLine("Usage:");
        Console.WriteLine("    ParseUtauPluginInvoker");
    }
}
