using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

class Resampler{
    public static void Main( string[] args ){
        string arg = "";
        foreach( string s in args ){
            arg += "\"" + s + "\" ";
        }
        string path = Application.StartupPath;
        using( StreamWriter sw = new StreamWriter( Path.Combine( path, "resampler.log" ), true ) ){
            sw.WriteLine( arg );
        }
        using( Process p = new Process() ){
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.Combine( path, "_resampler.exe" );
            psi.Arguments = arg;
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
    }
}
